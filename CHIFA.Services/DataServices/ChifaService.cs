global using LinqToDB;
using LinqToDB.Async;
using LinqToDB.Data;
using Npgsql;
namespace CHIFA.Services.DataServices;

public class ChifaService
{
    private static ChifaService? _instance;
    public static ChifaService Instance => _instance ??= new();

    /// <summary>Default period when a caller passes none. <c>To</c> is open-ended, so it never goes stale.</summary>
    public Period Period { get; } = new();

    /// <summary>Computed on each call: the app can stay open (in the tray) for days.</summary>
    private static DateTime YearAgo => DateTime.Now.AddYears(-1);

    /// <summary>
    /// Open bordereaux are always listed; closed ones when their extraction date is in the period.
    /// Counts and amounts cover all factures of each bordereau, whatever their date.
    /// </summary>
    public async ValueTask<IEnumerable<BordereauDto>> GetAllBordereauxAsync(Expression<Func<Bordereau, bool>>? predicate = default, Period? period = null,
        CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var activePeriod = period ?? Period;
        var start = activePeriod.Start;
        var end = activePeriod.EndExclusive;

        // The extraction date is the last facture date, so the period filter needs the same per-bordereau
        // totals as the grid: computing them in one GROUP BY over facture (no index on num_bord) is one scan
        // instead of one scan per bordereau.
        var list = await (
                from b in db.Bordereaus.Where(predicate ?? (_ => true))
                from t in FactureTotals(db).LeftJoin(t => t.NumBord == b.NumBord)
                where b.Etat == 'O' ||
                      (t.LastFacture != null && (start == null || t.LastFacture >= start) && (end == null || t.LastFacture < end))
                select new BordereauDto
                {
                    Num = b.NumBord,
                    Center = b.Center!.Nom,
                    FirstFacture = t.FirstFacture,
                    LastFacture = t.LastFacture,
                    Nmbr = (int?)t.Nmbr ?? 0,
                    Virment = b.MontVir,
                    DateDepotFtp = b.DateDepotFtp,
                    DateCloture = b.DateCloture,
                    DateOuverture = b.DateOuverture,
                    State = b.Etat,
                    MontOff = t.MontOff,
                    MontAss = t.MontAss,
                    Maj = t.Maj
                })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return list
            .OrderByDescending(x => x.FirstFacture)
            .ThenByDescending(x => x.Num)
            .ToList();
    }

    /// <summary>One bordereau with all its factures, by insured number. Null when it does not exist.</summary>
    public async ValueTask<BordereauDetailDto?> GetBordereauAsync(string numBord, CancellationToken cancellationToken = default)
    {
        BordereauDto? bordereau;
        await using (var db = new ChifaDb())
        {
            bordereau = await db.Bordereaus
                .Where(b => b.NumBord == numBord)
                .Select(b => new BordereauDto
                {
                    Num = b.NumBord,
                    Center = b.Center!.Nom,
                    Virment = b.MontVir,
                    DateDepotFtp = b.DateDepotFtp,
                    DateCloture = b.DateCloture,
                    DateOuverture = b.DateOuverture,
                    State = b.Etat
                })
                .FirstOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        if (bordereau is null)
            return null;

        var factures = (await GetAllFacturesAsync(null, null, new Period { From = null }, f => f.NumBord == numBord, cancellationToken).ConfigureAwait(false))
            .OrderBy(f => f.NumAssure, StringComparer.Ordinal)
            .ThenBy(f => f.Rang, StringComparer.Ordinal)
            .ThenBy(f => f.DateFact)
            .ToList();

        // Same totals as the list: computed from the factures already loaded instead of a second scan.
        bordereau.Nmbr = factures.Count;
        bordereau.MontOff = factures.Sum(f => f.MontOff ?? 0);
        bordereau.MontAss = factures.Sum(f => f.MontAss ?? 0);
        bordereau.Maj = factures.Sum(f => f.Majoration ?? 0);
        bordereau.FirstFacture = factures.Min(f => f.DateFact);
        bordereau.LastFacture = factures.Max(f => f.DateFact);

        return new BordereauDetailDto { Bordereau = bordereau, Factures = factures };
    }

    /// <summary>Per-bordereau totals of its factures, whatever their date.</summary>
    internal static IQueryable<FactureTotals> FactureTotals(ChifaDb db) =>
        from f in db.Factures
        where f.NumBord != null
        group f by f.NumBord into g
        select new FactureTotals
        {
            NumBord = g.Key,
            Nmbr = g.Count(),
            MontOff = g.Sum(m => m.MontOff),
            MontFact = g.Sum(m => m.MontFact),
            MontAss = g.Sum(m => m.MontAs),
            Maj = g.Sum(m => m.MontMaj),
            Fae = g.Sum(m => m.MontMajFae),
            FirstFacture = g.Min(m => m.DateFact),
            LastFacture = g.Max(m => m.DateFact)
        };

    public async ValueTask<IEnumerable<FactureDto>> GetAllFacturesAsync(bool? last, bool? ts, Period? period = null, Expression<Func<Facture, bool>>? predicate = default,
        CancellationToken cancellationToken = default)
    {
        predicate = predicate.SetPeriod(period);

        if (ts == true)
            predicate = predicate.And(x =>
                x.DetailFacts.Any(d => d.Ts == true || d.DureeTrait >= MedicalThresholds.MediumTreatmentDurationDays || (d.Ppa >= MedicalThresholds.HighPriceThreshold && d.Qte >= MedicalThresholds.HighQuantityThreshold)));

        if (last == true)
        {
            var yearAgo = YearAgo;
            predicate = predicate.And(x => x.DateFact > yearAgo);
        }

        await using var db = new ChifaDb();
        var list = await db.Factures.Where(predicate)
            .Select(f => new FactureDto
            {
                NumFact = f.NumFact,
                NumAssure = f.NumAssure,
                DateFact = f.DateFact,
                DateSoin = f.DateSoin,
                Malade = f.Beneficiaire.FullName,
                Assure = f.Assure.FullName,
                MontFact = f.MontFact,
                MontAss = f.MontAs,
                MontOff = f.MontOff,
                Majoration = f.MontMaj,
                Bordereau = f.NumBord,
                Specialite = f.Specialite!.Libelle,
                Centre = f.Center!.Nom,
                Rang = f.RangAd,
                TS = f.DetailFacts.Any(m => m.Ts == true),
                LongDuree = f.DetailFacts.Any(m => m.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays)
            })
            .OrderByDescending(x => x.DateFact)
            .ThenByDescending(x => x.NumFact)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return list;
    }

    public async ValueTask<IEnumerable<BeneficiareDto>> GetBeneficiaresAsync(CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();

        var centerByBenef = await db.Factures
            .Where(f => f.NumAssure != null && f.RangAd != null)
            .GroupBy(f => new { f.NumAssure, f.RangAd })
            .Select(g => new
            {
                g.Key.NumAssure,
                g.Key.RangAd,
                MinNumFact = g.Min(f => f.NumFact)
            })
            .ToListAsync(cancellationToken);

        var minFact = centerByBenef.Select(x => x.MinNumFact).Distinct().ToList();

        var centers = await db.Factures
            .Where(f => minFact.Contains(f.NumFact))
            .Select(f => new { f.NumFact, Center = f.Center!.Nom })
            .ToListAsync(cancellationToken);

        var centerByNumFact = centers.ToDictionary(c => c.NumFact, c => c.Center);
        var centerByBenefDict = centerByBenef
            .Where(x => x.MinNumFact != null && centerByNumFact.ContainsKey(x.MinNumFact))
            .ToDictionary(x => (x.NumAssure, x.RangAd), x => centerByNumFact[x.MinNumFact!]);

        var coverage = await Coverage.LoadAsync(db, null, cancellationToken);

        return await db.Beneficiaires
            .Select(a => new
            {
                NumAssure = a.NumAssure,
                Rang = a.RangAd,
                Beneficiare = a.FullName,
                Assure = a.Assure!.FullName
            })
            .ToListAsync(cancellationToken)
            .ContinueWith(t =>
            {
                var rows = t.Result;
                var list = new List<BeneficiareDto>(rows.Count);
                foreach (var r in rows)
                {
                    var c = coverage.GetValueOrDefault((r.NumAssure!, r.Rang!));
                    list.Add(new BeneficiareDto
                    {
                        NumAssure = r.NumAssure,
                        Rang = r.Rang,
                        Beneficiare = r.Beneficiare,
                        Assure = r.Assure,
                        Center = (r.NumAssure != null && r.Rang != null
                                  && centerByBenefDict.TryGetValue((r.NumAssure, r.Rang), out var nom))
                            ? nom : "",
                        Taux = c?.Taux,
                        TauxVarie = c?.TauxVarie ?? false,
                        Mc = c?.Mc ?? false
                    });
                }
                return (IEnumerable<BeneficiareDto>)list;
            });
    }

    public async ValueTask<BeneficiareDto?> GetBeneficiareByIdAsync(string num, string rang, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var coverage = (await Coverage.LoadAsync(db, [num], cancellationToken)).GetValueOrDefault((num, rang));
        var benef = await db.Beneficiaires
            .Where(x => x.NumAssure == num && x.RangAd == rang)
            .Select(a => new { a.NumAssure, a.RangAd, Beneficiare = a.FullName, Assure = a.Assure!.FullName })
            .FirstOrDefaultAsync(cancellationToken);
        return benef is null ? null : new BeneficiareDto
        {
            NumAssure = benef.NumAssure,
            Rang = benef.RangAd,
            Beneficiare = benef.Beneficiare,
            Assure = benef.Assure,
            Taux = coverage?.Taux,
            TauxVarie = coverage?.TauxVarie ?? false,
            Mc = coverage?.Mc ?? false
        };
    }

    public async ValueTask<IEnumerable<Centre>> GetCentersAsync()
    {
        await using var db = new ChifaDb();
        return await db.Centres.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<FactureDetailDto>> GetFactureDetailsByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        return await db.DetailFacts
            .Where(x => x.NumFact == id)
            .Select(x => new FactureDetailDto
            {
                Code = x.NumEnr,
                Medicament = x.Medicament.FullName(),
                Qt = x.Qte,
                TR = x.TarifRef,
                DureeTrait = x.DureeTrait,
                Ppa = x.Ppa,
                MontAss = x.MontAs,
                MontPharm = x.MontPharm,
                MajLocal = x.MajLocal,
                MajSub = x.MajSub,
                Ts = x.Ts
            }).ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async ValueTask<Parametre?> GetFirstOfficineAsync()
    {
        await using var db = new ChifaDb();
        return await ReadFirstOfficineAsync(db).ConfigureAwait(false);
    }

    internal static async ValueTask<Parametre?> ReadFirstOfficineAsync(ChifaDb db)
    {
        try
        {
            return await db.Parametres.FirstOrDefaultAsync().ConfigureAwait(false);
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedColumn)
        {
            var entity = db.MappingSchema.GetEntityDescriptor(typeof(Parametre));
            var columns = await db.QueryToListAsync<string>(ColumnsSql, ColumnsParameter(entity)).ConfigureAwait(false);
            return await db.Parametres.Select(ExistingColumnsProjection<Parametre>(entity, columns)).FirstOrDefaultAsync().ConfigureAwait(false);
        }
    }

    public Parametre? GetFirstOfficine()
    {
        using var db = new ChifaDb();
        try
        {
            return db.Parametres.FirstOrDefault();
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UndefinedColumn)
        {
            var entity = db.MappingSchema.GetEntityDescriptor(typeof(Parametre));
            var columns = db.Query<string>(ColumnsSql, ColumnsParameter(entity)).ToList();
            return db.Parametres.Select(ExistingColumnsProjection<Parametre>(entity, columns)).FirstOrDefault();
        }
    }

    // Resolves the table through search_path like the failing query did; regclass works on CHIFA's PostgreSQL 9.x.
    private const string ColumnsSql =
        "select attname::text from pg_attribute where attrelid = @table::text::regclass and attnum > 0 and not attisdropped";

    private static DataParameter ColumnsParameter(LinqToDB.Mapping.EntityDescriptor entity) =>
        new("table", entity.Name.Schema is { } schema ? $"\"{schema}\".\"{entity.Name.Name}\"" : $"\"{entity.Name.Name}\"", DataType.NVarChar);

    /// <summary>
    /// CHIFA_OFFICINE owns the schema, and older versions of it lack columns CHIFA2.Data maps
    /// (parametre.date_medicament2...): reads only the mapped columns that this database has.
    /// </summary>
    private static Expression<Func<T, T>> ExistingColumnsProjection<T>(LinqToDB.Mapping.EntityDescriptor entity, IEnumerable<string> columns)
    {
        var existing = columns.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var row = Expression.Parameter(typeof(T), "x");
        var bindings = entity.Columns
            .Where(c => existing.Contains(c.ColumnName))
            .Select(c => Expression.Bind(c.MemberInfo, Expression.MakeMemberAccess(row, c.MemberInfo)));
        return Expression.Lambda<Func<T, T>>(Expression.MemberInit(Expression.New(typeof(T)), bindings), row);
    }

    public async ValueTask<IEnumerable<Forme>> GetFormesAsync()
    {
        await using var db = new ChifaDb();
        return await db.Formes.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<MedicDto>> GetMedicamentsAsync(Expression<Func<Medicament, bool>>? predicate = default)
    {
        await using var db = new ChifaDb();
        predicate ??= medic => true;
        var list = await db.Medicaments
            .Where(predicate)
            .Select(x => new MedicDto
            {
                NEnr = x.NumEnr,
                NomCommercial = x.NomCom,
                DCI = x.NomDci,
                COND = x.Conditionnement,
                DOSAGE = x.Dosage,
                FORME = x.Forme.LibelleCourt,
                TR = x.TarifRef,
                PAYS = x.Pays,
                Generic = x.Generic,
                CM = x.Cm == 'O',
                CodeDCI = x.CodeDci,
                CodeMedic = x.CodeMedic
            })
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public async ValueTask<string?> GetMedicObsAsync(string nEnr)
    {
        await using var db = new ChifaDb();
        return await db.Medicaments
            .Where(x => string.IsNullOrEmpty(nEnr) || x.NumEnr == nEnr)
            .Select(x => x.Observation)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<PatientOfTraitSpec>> GetPatientsOfTraitSpecAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = default,
        CancellationToken cancellationToken = default)
    {
        predicate = predicate.SetPeriod(period)
            .And(x => (x.Ppa >= MedicalThresholds.HighPriceThreshold && x.Qte >= MedicalThresholds.HighQuantityThreshold) || x.Ts == true || x.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays);

        await using var db = new ChifaDb();
        var list = await db.DetailFacts
            .Where(predicate)
            .GroupBy(x => new { x.Facture.NumAssure, x.Facture.RangAd })
            .Select(x => new PatientOfTraitSpec
            {
                NumAssure = x.Key.NumAssure,
                Rang = x.Key.RangAd,
                Assure = x.Max(m => m.Facture.Assure.FullName),
                Malade = x.Max(m => m.Facture.Beneficiaire.FullName),
                // Count(...) > 0 is an aggregate of the group; Any(...) was a subquery per patient.
                TS = x.Count(m => m.Ts == true) > 0,
                LongDuree = x.Count(m => m.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays) > 0
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return list;
    }

    public async ValueTask<IEnumerable<TraitDetailsDto>> GetPatientTraitementAsync(string noAssure, string rang, bool proche, Expression<Func<DetailFact?, bool>>? predicate = default, Period? period = null,
        CancellationToken cancellationToken = default)
    {
        predicate ??= _ => true;
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate!.SetPeriod(period ?? Period))
            .Where(x => x.Facture.NumAssure == noAssure && x.Facture.RangAd == rang)
            .Where(x => (x.Ppa >= MedicalThresholds.HighPriceThreshold && x.Qte >= MedicalThresholds.HighQuantityThreshold) || x.Ts == true || x.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays)
            .Select(x => new
            {
                Medicament = x.Medicament.FullName(),
                NEnrg = x.NumEnr,
                x.Medicament.CodeDci,
                Duree = x.DureeTrait,
                TS = x.Ts,
                Qt = x.Qte,
                Prix = x.Ppa,
                Specialite = x.Facture.Specialite!.Libelle,
                x.Facture.DateSoin,
                x.Facture.DateFact
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var list = query
            .OrderByDescending(x => x.DateFact)
            .GroupBy(x => new { x.CodeDci })
            .Select(x =>
            {
                var items = x.ToList();
                var first = items[0];
                return new TraitDetailsDto
                {
                    DateFact = first.DateFact,
                    Duree = first.Duree,
                    DateSoin = first.DateSoin,
                    Qt = first.Qt,
                    Medicament = first.Medicament,
                    Prix = first.Prix,
                    NEnrg = first.NEnrg,
                    TS = first.TS ?? false,
                    Specialite = first.Specialite,
                    CodeDci = x.Key.CodeDci,
                    Historic = items.Count <= 1
                        ? []
                        : items.Select(m => new MedicHistory
                        {
                            Medicament = m.Medicament,
                            Duree = m.Duree,
                            DateFact = m.DateFact,
                            Qt = m.Qt,
                            Prix = m.Prix
                        }).ToList()
                };
            });

        if (proche) list = list.Where(x => x.Prochain <= DateTime.Now);
        return list.ToList();
    }

    /// <summary>
    /// Lists CHIFA users WITHOUT their password: <c>MotPasse</c> is never read from the database,
    /// so it cannot be displayed, searched, exported or kept in memory.
    /// </summary>
    public async ValueTask<IEnumerable<Utilisateur>> GetUsersAsync()
    {
        await using var db = new ChifaDb();
        return await db.Utilisateurs
            .Select(u => new Utilisateur
            {
                NomUtilisateur = u.NomUtilisateur,
                DroitAcces = u.DroitAcces,
                Admin = u.Admin
            })
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<ListeNoire>> LoadAllListNoirAsync()
    {
        await using var db = new ChifaDb();
        return await db.ListeNoires.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<ConsumptionDto>> LoadConsumptionAsync(string noAssure, string rang,
        bool distinct, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var query = await db.Factures
            .Where(x => x.NumAssure == noAssure && x.RangAd == rang)
            .SelectMany(x => x.DetailFacts, (f, d) => new ConsumptionDto
            {
                Medicament = d.Medicament.FullName(),
                Duree = d.DureeTrait,
                TS = d.Ts,
                Date = f.DateFact,
                Facture = f.NumFact,
                Bord = f.NumBord,
                Qt = d.Qte,
                Prix = d.Ppa,
                CodeDci = d.Medicament.CodeDci,
                Medecin = f.Specialite!.Libelle,
                NEnrg = d.NumEnr
            })
            .OrderByDescending(x => x.Date)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        if (distinct)
            return query;

        var list = query
            .GroupBy(x => new { x.CodeDci })
            .Select(x =>
            {
                var items = x.ToList();
                var first = items[0];
                return new ConsumptionGroupedDto
                {
                    Facture = first.Facture,
                    Bord = first.Bord,
                    Date = first.Date,
                    Duree = first.Duree,
                    Qt = first.Qt,
                    Medicament = first.Medicament,
                    Prix = first.Prix,
                    NEnrg = first.NEnrg,
                    TS = first.TS ?? false,
                    Medecin = first.Medecin,
                    CodeDci = x.Key.CodeDci,
                    Historic = items.Count <= 1
                        ? []
                        : items.Select(m => new MedicHistory
                        {
                            Facture = m.Facture,
                            Bord = m.Bord,
                            Medicament = m.Medicament,
                            Duree = m.Duree,
                            DateFact = m.Date,
                            Qt = m.Qt,
                            Prix = m.Prix
                        }).ToList()
                };
            }).ToList();
        return list;
    }

    public async ValueTask<IEnumerable<FactureCm>> LoadControlsMedicalAsync()
    {
        await using var db = new ChifaDb();
        return await db.FactureCms.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<FactureDto>> LoadHistoryAsync(string noAssure, string rang, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var query = await db.Factures
            .Where(x => x.NumAssure == noAssure && x.RangAd == rang)
            .Select(f => new FactureDto
            {
                NumFact = f.NumFact,
                DateSoin = f.DateSoin,
                DateFact = f.DateFact,
                Bordereau = f.NumBord,
                Specialite = f.Specialite!.Libelle,
                TS = f.DetailFacts.Any(x => x.Ts == true),
                LongDuree = f.DetailFacts.Any(x => x.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays),
                Taux = Coverage.Percent(f.Taux),
                MontFact = f.MontFact,
                Majoration = f.MontMajFae,
                MontAss = f.MontAs,
                MontOff = f.MontOff
            })
            .OrderByDescending(x => x.DateFact)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return query;
    }

    public async ValueTask<IEnumerable<TraitSpec2>> PatientsWithTraitSpec2Async(Expression<Func<DetailFact, bool>>? predicate = default)
    {
        predicate ??= (fact => true);
        var yearAgo = YearAgo;
        predicate = predicate.And(x => x.Facture.DateFact > yearAgo);
        predicate = predicate.And(x =>
            x.Ts == true
            || (x.DureeTrait == 1 && x.Qte >= MedicalThresholds.HighQuantityThreshold
                ? MedicalThresholds.VeryLongTreatmentDurationDays
                : x.DureeTrait) > MedicalThresholds.MediumTreatmentDurationDays
            || (x.Ppa >= MedicalThresholds.HighPriceThreshold && x.Qte >= MedicalThresholds.HighQuantityThreshold));

        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate)
            .Select(d => new
            {
                d.Facture.NumAssure,
                d.Facture.RangAd,
                d.NumFact,
                d.Facture.NumBord,
                d.Medicament.CodeDci,
                Medicament = d.Medicament.FullName(),
                d.Facture.DateFact,
                Duree = d.DureeTrait == 1 && d.Qte >= MedicalThresholds.HighQuantityThreshold ? MedicalThresholds.VeryLongTreatmentDurationDays : d.DureeTrait,
                NEnrg = d.NumEnr,
                Qt = d.Qte,
                TS = d.Ts,
                Prix = d.Ppa,
                Malade = d.Facture.Beneficiaire.FullName,
                Assure = d.Facture.Assure.FullName
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query
            .OrderByDescending(x => x.DateFact)
            .GroupBy(x => new { x.NumAssure, x.RangAd, x.CodeDci })
            .Select(x =>
            {
                var items = x.ToList();
                var first = items[0];
                return new TraitSpec2
                {
                    NumFact = first.NumFact,
                    Bord = first.NumBord,
                    Assure = first.Assure,
                    DateFact = first.DateFact,
                    Duree = first.Duree,
                    Qt = first.Qt,
                    Medicament = first.Medicament,
                    Malade = first.Malade,
                    Prix = first.Prix,
                    NEnrg = first.NEnrg,
                    TS = first.TS == true,
                    Code = x.Key.CodeDci,
                    NumAssure = x.Key.NumAssure,
                    Rang = x.Key.RangAd!,
                    TC = true,
                    Historic = (items.Count <= 1
                        ? null
                        : items.Select(m => new MedicHistory
                        {
                            Facture = m.NumFact,
                            Bord = m.NumBord,
                            Medicament = m.Medicament,
                            Duree = m.Duree,
                            DateFact = m.DateFact,
                            Prix = m.Prix,
                            Qt = m.Qt
                        }).ToList())!
                };
            })
            .OrderByDescending(x => x.Montant)
            .ToList();
        return list;
    }

    public async ValueTask<IEnumerable<PatientWithTraitSpec>> PatientsWithTraitSpecAsync(
        Expression<Func<DetailFact, bool>>? predicate = default)
    {
        predicate ??= _ => true;
        var yearAgo = YearAgo;
        predicate = predicate.And(x =>
            (x.Facture.DateFact > yearAgo &&x.Ppa >= MedicalThresholds.HighPriceThreshold && x.Qte >= MedicalThresholds.HighQuantityThreshold) || x.Ts == true || x.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays);
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate)
            .Select(a => new
            {
                a.Facture.NumAssure,
                Rang = a.Facture.RangAd,
                Assure = a.Facture.Assure.FullName,
                Malade = a.Facture.Beneficiaire.FullName,
                a.Medicament.NomCom, a.Medicament.Dosage, a.Medicament.Conditionnement,
                a.Facture.DateFact,
                Duree = a.DureeTrait == 1 && a.Qte >= MedicalThresholds.HighQuantityThreshold
                    ? MedicalThresholds.VeryLongTreatmentDurationDays
                    : a.DureeTrait,
                NEnrg = a.NumEnr,
                Qt = a.Qte,
                a.Ts,
                a.Ppa,
                Specialite = a.Facture.Specialite!.Libelle,
                a.Facture.DateSoin,
                a.Medicament.CodeDci
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query
            .OrderByDescending(x => x.DateFact)
            .GroupBy(x => new { x.NumAssure, x.Rang })
            .Select(a =>
            {
                var items = a.ToList();
                var first = items[0];
                return new PatientWithTraitSpec
                {
                    NumAssure = a.Key.NumAssure,
                    Rang = a.Key.Rang,
                    Assure = first.Assure,
                    Malade = first.Malade,
                    DetailsDtos = items.Select(m => new TraitDetailsDto
                    {
                        Medicament = $"{m.NomCom} {m.Dosage} {m.Conditionnement}",
                        DateFact = m.DateFact,
                        Duree = m.Duree,
                        NEnrg = m.NEnrg,
                        Qt = m.Qt,
                        TS = m.Ts,
                        Prix = m.Ppa,
                        Specialite = m.Specialite,
                        CodeDci = m.CodeDci,
                        DateSoin = m.DateSoin
                    })
                        .ToList()
                };
            }).ToList();
        return list;
    }

    public async ValueTask<IEnumerable<Specialite>> SpecialitesAsync()
    {
        await using var db = new ChifaDb();
        return await db.Specialites.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask UpdateCenter(Centre center)
    {
        await using var db = new ChifaDb();
        await db.UpdateAsync(center);
    }

    public async ValueTask GetMinAndMaxDatesAsync()
    {
        await using var db = new ChifaDb();
        Period.MinDate = await db.Factures.MinAsync(x => x.DateFact) ?? new DateTime(2000, 1, 1);
        Period.MaxDate = await db.Factures.MaxAsync(x => x.DateFact) ?? DateTime.Now;
    }

    public async ValueTask<Facture?> GetFactureForPrintAsync(string numFact)
    {
        await using var db = new ChifaDb();
        return await db.Factures
            .LoadWith(x => x.Beneficiaire)
            .LoadWith(x => x.Assure)
            .LoadWith(x => x.Center)
            .LoadWith(x => x.DetailFacts)
            .ThenLoad(x => x.Medicament)
            .FirstOrDefaultAsync(x => x.NumFact == numFact)
            .ConfigureAwait(false);
    }

    public Facture? GetFactureForPrint(string numFact)
    {
        using var db = new ChifaDb();
        return db.Factures
            .LoadWith(x => x.Beneficiaire)
            .LoadWith(x => x.Assure)
            .LoadWith(x => x.Center)
            .LoadWith(x => x.DetailFacts)
            .ThenLoad(x => x.Medicament)
            .FirstOrDefault(x => x.NumFact == numFact);
    }

    public async ValueTask DeleteFactureAsync(string numFact)
    {
        await using var db = new ChifaDb();
        await db.BeginTransactionAsync().ConfigureAwait(false);
        try
        {
            var fact = await db.Factures.FirstOrDefaultAsync(x => x.NumFact == numFact).ConfigureAwait(false);
            if (fact is not null)
            {
                await db.GetTable<Signature>().Where(x => x.NumFact == numFact).DeleteAsync().ConfigureAwait(false);
                await db.GetTable<DetailFact>().Where(x => x.NumFact == numFact).DeleteAsync().ConfigureAwait(false);
                await db.DeleteAsync(fact).ConfigureAwait(false);
            }

            await db.CommitTransactionAsync().ConfigureAwait(false);
        }
        catch
        {
            await db.RollbackTransactionAsync().ConfigureAwait(false);
            throw;
        }
    }
}

internal sealed class FactureTotals
{
    public string? NumBord { get; init; }
    public int Nmbr { get; init; }
    public decimal? MontOff { get; init; }
    public decimal? MontFact { get; init; }
    public decimal? MontAss { get; init; }
    public decimal? Maj { get; init; }
    public decimal? Fae { get; init; }
    public DateTime? FirstFacture { get; init; }
    public DateTime? LastFacture { get; init; }
}

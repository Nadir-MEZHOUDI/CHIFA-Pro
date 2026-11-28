using LinqToDB.Async;

namespace CHIFA.Services.DataServices;

public class ScopeService
{
    private static ScopeService? _instance;
    public static ScopeService Instance => _instance ??= new ScopeService();

    public readonly Period Period = new();

    public async ValueTask<IEnumerable<RejetDto>> GetRejetsAsync(Period? period = null)
    {
        await using var db = new ChifaDb();
        var rows = await BordereauRows(db, BordereauxInPeriod(db, period ?? Period))
            .ToListAsync()
            .ConfigureAwait(false);

        return rows
            .Select(ToRejet)
            .OrderByDescending(r => r.MontantRejete)
            .ThenByDescending(r => r.DateBord)
            .ToList();
    }

    /// <summary>Same rows as <see cref="GetRejetsAsync"/>, latest first, one page read from the database.</summary>
    public async ValueTask<(int Total, List<RejetDto> Items)> GetRejetsPageAsync(Period? period, int skip, int take)
    {
        await using var db = new ChifaDb();
        var (total, rows) = await BordereauRows(db, BordereauxInPeriod(db, period ?? Period))
            .OrderBy(b => b.DateBord == null)
            .ThenByDescending(b => b.DateBord)
            .ThenByDescending(b => b.MontantRejete)
            .ThenByDescending(b => b.NumBord)
            .ToPageAsync(skip, take)
            .ConfigureAwait(false);

        return (total, rows.Select(ToRejet).ToList());
    }

    /// <summary>
    /// A bordereau belongs to the period by its closing date, or its FTP deposit date when CHIFA left
    /// the closing date at the 1900 placeholder (it does so for every bordereau).
    /// </summary>
    private static IQueryable<Bordereau> BordereauxInPeriod(ChifaDb db, Period period)
    {
        var start = period.Start;
        var end = period.EndExclusive;
        var minDate = RejetDto.DateMinimaleValide;
        return db.Bordereaus.Where(b =>
            (start == null || (b.DateCloture >= minDate ? b.DateCloture : b.DateDepotFtp) >= start) &&
            (end == null || (b.DateCloture >= minDate ? b.DateCloture : b.DateDepotFtp) < end));
    }

    /// <summary>Numbers of the bordereaux still open in CHIFA: cheap, polled every few minutes.</summary>
    public async ValueTask<List<string>> GetOpenBordereauxAsync()
    {
        await using var db = new ChifaDb();
        return await db.Bordereaus
            .Where(b => b.Etat == 'O')
            .Select(b => b.NumBord)
            .ToListAsync()
            .ConfigureAwait(false);
    }

    /// <summary>
    /// The given bordereaux with their totals, whatever their dates: CHIFA never fills the closing date, and
    /// leaves the FTP date empty for bordereaux that are not sent by FTP (CASNOS).
    /// </summary>
    public async ValueTask<List<RejetDto>> GetBordereauxByNumAsync(IReadOnlyCollection<string> numBords)
    {
        if (numBords.Count == 0)
            return [];

        await using var db = new ChifaDb();
        var rows = await BordereauRows(db, db.Bordereaus.Where(b => numBords.Contains(b.NumBord)))
            .ToListAsync()
            .ConfigureAwait(false);

        return rows.Select(ToRejet).ToList();
    }

    /// <summary>The bordereaux with their totals, the GROUP BY over facture limited to these bordereaux.</summary>
    private static IQueryable<BordereauRow> BordereauRows(ChifaDb db, IQueryable<Bordereau> bordereaux)
    {
        var minDate = RejetDto.DateMinimaleValide;
        var tolerance = RejetDto.ToleranceArrondi;
        return from b in bordereaux
            from t in ChifaService.FactureTotals(db, bordereaux.Select(x => x.NumBord)).LeftJoin(t => t.NumBord == b.NumBord)
            let partCaisse = t.MontOff ?? 0m
            let majorations = t.Maj ?? 0m
            let fae = t.Fae ?? 0m
            let virement = b.MontVir ?? 0m
            let ecart = partCaisse + majorations + fae - virement
            select new BordereauRow
            {
                NumBord = b.NumBord,
                Center = b.Center!.Nom,
                DateCloture = b.DateCloture,
                DateDepotFtp = b.DateDepotFtp,
                MontVir = b.MontVir,
                Etat = b.Etat.ToString(),
                EstOuvert = b.Etat == 'O',
                NombreFactures = (int?)t.Nmbr ?? 0,
                PartCaisse = partCaisse,
                Majorations = majorations,
                Fae = fae,
                // RejetDto.DateReelle and RejetDto.MontantRejete in SQL, for the ORDER BY of a page.
                DateBord = b.DateCloture >= minDate ? b.DateCloture : b.DateDepotFtp >= minDate ? b.DateDepotFtp : null,
                MontantRejete = b.Etat == 'O' ? 0m : virement > 0 && ecart >= tolerance ? ecart : 0m
            };
    }

    private static RejetDto ToRejet(BordereauRow b) => new()
    {
        NumBord = b.NumBord,
        Centre = b.Center ?? "Inconnu",
        DateBord = RejetDto.DateReelle(b.DateCloture, b.DateDepotFtp),
        DateDepotFtp = b.DateDepotFtp,
        NombreFactures = b.NombreFactures,
        MontantPartCaisse = b.PartCaisse,
        MontantMajorations = b.Majorations,
        MontantFae = b.Fae,
        MontantVirement = b.MontVir ?? 0m,
        EstOuvert = b.EstOuvert,
        EtatBordereau = b.Etat ?? ""
    };

    private sealed class BordereauRow
    {
        public string NumBord { get; init; } = "";
        public string? Center { get; init; }
        public DateTime? DateCloture { get; init; }
        public DateTime? DateDepotFtp { get; init; }
        public decimal? MontVir { get; init; }
        public string? Etat { get; init; }
        public bool EstOuvert { get; init; }
        public int NombreFactures { get; init; }
        public decimal PartCaisse { get; init; }
        public decimal Majorations { get; init; }
        public decimal Fae { get; init; }
        public DateTime? DateBord { get; init; }
        public decimal MontantRejete { get; init; }
    }

    public async ValueTask<IEnumerable<PsychotropeDto>> GetPsychotropesAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var list = await PsychotropeRows(db, period ?? Period, predicate)
            // Regulatory register: every dispensing of the period must be listed, never truncated.
            .OrderByDescending(p => p.DateDelivrance)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return list.Select(ToPsychotrope).ToList();
    }

    /// <summary>Same rows as <see cref="GetPsychotropesAsync"/>, latest first, one page read from the database.</summary>
    public async ValueTask<(int Total, List<PsychotropeDto> Items)> GetPsychotropesPageAsync(Period? period, int skip, int take,
        CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var (total, rows) = await PsychotropeRows(db, period ?? Period, null)
            .OrderBy(p => p.DateDelivrance == null)
            .ThenByDescending(p => p.DateDelivrance)
            .ThenByDescending(p => p.NumFact)
            .ThenBy(p => p.CodeEnr)
            .ThenBy(p => p.NumLot)
            .ToPageAsync(skip, take, cancellationToken)
            .ConfigureAwait(false);

        return (total, rows.Select(ToPsychotrope).ToList());
    }

    private static IQueryable<PsychotropeRow> PsychotropeRows(ChifaDb db, Period period, Expression<Func<DetailFact, bool>>? predicate)
    {
        var start = period.Start;
        var end = period.EndExclusive;

        var explicitCodes = PsychotropesHelper.ExplicitDciCodes;
        var excludedCodes = PsychotropesHelper.ExcludedDciCodes.ToList();

        Expression<Func<DetailFact, bool>> basePredicate = d =>
            (
                d.Medicament.Tableau == 'B' ||
                explicitCodes.Contains(d.Medicament.CodeDci!) ||
                (
                    d.Medicament.CodeDci != null &&
                    (d.Medicament.CodeDci.StartsWith("16B") ||
                     d.Medicament.CodeDci.StartsWith("16C") ||
                     d.Medicament.CodeDci.StartsWith("15A") ||
                     d.Medicament.CodeDci.StartsWith("15D") ||
                     d.Medicament.CodeDci.StartsWith("03F") ||
                     d.Medicament.CodeDci.StartsWith("16A") ||
                     d.Medicament.CodeDci.StartsWith("16D") ||
                     d.Medicament.CodeDci.StartsWith("16E")) &&
                    !excludedCodes.Contains(d.Medicament.CodeDci!)
                )
            ) &&
            (start == null || d.Facture.DateFact >= start) &&
            (end == null || d.Facture.DateFact < end);

        if (predicate != null)
            basePredicate = basePredicate.And(predicate);

        return db.DetailFacts
            .Where(basePredicate)
            .Select(d => new PsychotropeRow
            {
                NumFact = d.Facture.NumFact,
                DateDelivrance = d.Facture.DateFact,
                NumAssure = d.Facture.NumAssure,
                NomAssure = d.Facture.Assure.FullName,
                NomBeneficiaire = d.Facture.Beneficiaire.FullName,
                Rang = d.Facture.RangAd,
                Medecin = d.Facture.Prescripteur,
                SpecialiteMedecin = d.Facture.Specialite!.Libelle,
                CodeEnr = d.NumEnr,
                NumLot = d.NumLot,
                CodeDci = d.Medicament.CodeDci,
                Tableau = d.Medicament.Tableau != null ? d.Medicament.Tableau.ToString() : null,
                MedicamentNom = d.Medicament.NomCom,
                Dci = d.Medicament.NomDci,
                Dosage = d.Medicament.Dosage,
                Forme = d.Medicament.Forme.LibelleCourt,
                Quantite = d.Qte,
                DureeTrait = d.DureeTrait,
                Ppa = d.Ppa,
                TarifRef = d.TarifRef,
                Montant = d.Mont,
                NumeroOrdonnance = d.Facture.NumFact,
                EstTraitementSpecial = d.Ts == true
            });
    }

    private static PsychotropeDto ToPsychotrope(PsychotropeRow d) => new()
    {
        NumFact = d.NumFact,
        DateDelivrance = d.DateDelivrance,
        NumAssure = d.NumAssure + "-" + d.Rang,
        NomAssure = d.NomAssure,
        NomBeneficiaire = d.NomBeneficiaire,
        Medecin = d.Medecin,
        CategoriePsychotrope = PsychotropesHelper.GetCategorie(d.CodeDci),
        Tableau = d.Tableau,
        MedicamentNom = d.MedicamentNom,
        Dci = d.Dci,
        Dosage = d.Dosage,
        Forme = d.Forme,
        Quantite = d.Quantite,
        DureeTrait = d.DureeTrait,
        Ppa = d.Ppa,
        TarifRef = d.TarifRef,
        Montant = d.Montant,
        NumeroOrdonnance = d.NumeroOrdonnance,
        EstTraitementSpecial = d.EstTraitementSpecial
    };

    private sealed class PsychotropeRow
    {
        public string? NumFact { get; init; }
        public DateTime? DateDelivrance { get; init; }
        public string? NumAssure { get; init; }
        public string? NomAssure { get; init; }
        public string? NomBeneficiaire { get; init; }
        public string? Rang { get; init; }
        public string? Medecin { get; init; }
        public string? SpecialiteMedecin { get; init; }
        public string? CodeEnr { get; init; }
        public string? NumLot { get; init; }
        public string? CodeDci { get; init; }
        public string? Tableau { get; init; }
        public string? MedicamentNom { get; init; }
        public string? Dci { get; init; }
        public string? Dosage { get; init; }
        public string? Forme { get; init; }
        public decimal? Quantite { get; init; }
        public decimal? DureeTrait { get; init; }
        public decimal? Ppa { get; init; }
        public decimal? TarifRef { get; init; }
        public decimal? Montant { get; init; }
        public string? NumeroOrdonnance { get; init; }
        public bool EstTraitementSpecial { get; init; }
    }

    public async ValueTask<ForecastSummaryDto> GetChronicForecastsAsync(int lookbackMonths = 6, int forecastDays = 30)
    {
        await using var db = new ChifaDb();
        var lookbackDate = DateTime.Today.AddMonths(-lookbackMonths);
        var maxForecastDate = DateTime.Today.AddDays(forecastDays);

        var chronicDetails = await db.DetailFacts
            .Where(d => d.Facture.DateFact >= lookbackDate && (d.DureeTrait >= 20 || d.Ts == true))
            .Select(d => new
            {
                d.Facture.NumAssure,
                Rang = d.Facture.RangAd,
                NomAssure = d.Facture.Assure.FullName,
                NomBenef = d.Facture.Beneficiaire.FullName,
                DateFact = d.Facture.DateFact,
                Duree = (int)(d.DureeTrait ?? 30m),
                d.MontPharm,
                MedicNom = d.Medicament.NomCom,
                Specialite = d.Facture.Specialite!.Libelle
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var groupedPatients = chronicDetails
            .Where(x => x.NumAssure != null && x.DateFact.HasValue)
            .GroupBy(x => new { x.NumAssure, x.Rang })
            .Select(g =>
            {
                var latestFacture = g.OrderByDescending(x => x.DateFact).First();
                var lastDate = latestFacture.DateFact!.Value;
                var estimatedDuration = latestFacture.Duree <= 0 ? 30 : latestFacture.Duree;
                var estimatedReturn = lastDate.Date.AddDays(estimatedDuration);

                var avgAmount = g.GroupBy(x => x.DateFact).Average(grp => grp.Sum(i => i.MontPharm) ?? 0m);
                var medList = string.Join(", ", g.Select(x => x.MedicNom).Where(n => !string.IsNullOrEmpty(n)).Distinct().Take(3));

                return new ForecastRefillDto
                {
                    NumAssure = g.Key.NumAssure,
                    Rang = g.Key.Rang,
                    NomAssure = latestFacture.NomAssure,
                    NomBeneficiaire = latestFacture.NomBenef,
                    DateDerniereVisite = lastDate,
                    DureeDernierTraitement = estimatedDuration,
                    DatePrevisionnelleRetour = estimatedReturn,
                    MontantMoyenHabituel = Math.Round(avgAmount, 2),
                    NombreTraitementsPrevus = g.Select(x => x.MedicNom).Distinct().Count(),
                    TraitementHabituelResume = medList,
                    MedecinHabituel = latestFacture.Specialite
                };
            })
            .Where(f => f.DatePrevisionnelleRetour.HasValue &&
                        f.DatePrevisionnelleRetour.Value.Date >= DateTime.Today.AddDays(-15) &&
                        f.DatePrevisionnelleRetour.Value.Date <= maxForecastDate)
            .OrderBy(f => f.DatePrevisionnelleRetour)
            .ToList();

        var totalForecastAmount = groupedPatients
            .Where(f => f.DatePrevisionnelleRetour >= DateTime.Today)
            .Sum(f => f.MontantMoyenHabituel);

        return new ForecastSummaryDto
        {
            TotalPatientsAttendus = groupedPatients.Count,
            PatientsEnRetard = groupedPatients.Count(f => f.JoursRestants < 0),
            PatientsCetteSemaine = groupedPatients.Count(f => f.JoursRestants >= 0 && f.JoursRestants <= 7),
            EstimationCaPrevisionnel = Math.Round(totalForecastAmount, 2),
            Details = groupedPatients
        };
    }

    /// <summary>
    /// The last factures. Digits match the facture number or the insured's number; words match the
    /// insured's or the patient's name (every word).
    /// </summary>
    public async ValueTask<IEnumerable<FactureDto>> GetRecentFacturesLiveAsync(int limit = 50, string? text = null)
    {
        await using var db = new ChifaDb();

        var factures = db.Factures.AsQueryable();
        var term = text?.Trim();
        if (!string.IsNullOrEmpty(term))
        {
            if (term.All(char.IsDigit))
                factures = factures.Where(f => f.NumFact.Contains(term) || f.NumAssure!.StartsWith(term));
            else
                foreach (var word in term.ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(5))
                    factures = factures.Where(f =>
                        (f.Assure.Nom + " " + f.Assure.Prenom).ToUpper().Contains(word) ||
                        (f.Beneficiaire.Nom + " " + f.Beneficiaire.Prenom).ToUpper().Contains(word));
        }

        // Pick the last factures first: with the joins in the same SELECT, PostgreSQL joined and checked
        // every facture before applying the LIMIT.
        var numFacts = await factures
            .OrderByDescending(f => f.DateFact)
            .Take(limit)
            .Select(f => f.NumFact)
            .ToListAsync()
            .ConfigureAwait(false);

        return await db.Factures
            .Where(f => numFacts.Contains(f.NumFact))
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
            .ToListAsync()
            .ConfigureAwait(false);
    }

    /// <summary>Number and date of the factures dated from <paramref name="since"/>: cheap, polled every few minutes.</summary>
    public async ValueTask<List<(string NumFact, DateTime DateFact)>> GetFactureStampsSinceAsync(DateTime since, int limit = 500)
    {
        await using var db = new ChifaDb();
        var rows = await db.Factures
            .Where(f => f.DateFact >= since)
            .OrderBy(f => f.DateFact)
            .Take(limit)
            .Select(f => new { f.NumFact, f.DateFact })
            .ToListAsync()
            .ConfigureAwait(false);
        return rows.Select(r => (r.NumFact, r.DateFact!.Value)).ToList();
    }

    public async ValueTask<List<WatchedFactureDto>> GetWatchedFacturesAsync(IReadOnlyCollection<string> numFacts)
    {
        if (numFacts.Count == 0)
            return [];

        await using var db = new ChifaDb();
        var factures = await db.Factures
            .Where(f => numFacts.Contains(f.NumFact))
            .Select(f => new WatchedFactureDto
            {
                NumFact = f.NumFact,
                DateFact = f.DateFact!.Value,
                NumAssure = f.NumAssure,
                Rang = f.RangAd,
                Malade = f.Beneficiaire.FullName,
                MontFact = f.MontFact ?? 0
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var lignes = await db.DetailFacts
            .Where(d => numFacts.Contains(d.NumFact))
            .Select(d => new { d.NumFact, d.NumEnr, d.Medicament.NomCom, d.Qte, d.Medicament.Tableau, d.Medicament.CodeDci })
            .ToListAsync()
            .ConfigureAwait(false);

        var byFacture = lignes.ToLookup(l => l.NumFact);
        foreach (var facture in factures)
            facture.Lignes = byFacture[facture.NumFact].Select(l => new WatchedLigneDto
            {
                NumEnr = l.NumEnr ?? "",
                Nom = l.NomCom,
                Qte = l.Qte,
                Psychotrope = PsychotropesHelper.IsPsychotrope(l.Tableau, l.CodeDci)
            }).ToList();

        return factures.OrderBy(f => f.DateFact).ToList();
    }
}

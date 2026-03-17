global using LinqToDB;
using LinqToDB.Async;
namespace CHIFA.DAL.DataServices;

public class ChifaService : IChifaService
{
    private static ChifaService? _instance;
    public static ChifaService Instance => _instance ??= new();

    public Period Period { get; } = new();
    private DateTime YearAgo { get; } = DateTime.Now.AddYears(-1);

    public async ValueTask<IEnumerable<BordereauDto>> GetAllBordereauxAsync(Expression<Func<Bordereau, bool>>? predicate = default)
    {
        await using var db = new ChifaDb();

        var list = await db.Bordereaus
            .Where(predicate.SetPeriod(Period))
            .Select(x => new BordereauDto
            {
                Num = x.NumBord,
                Center = x.Center!.Nom,
                FirstFacture = x.DateGen,
                LastFacture = x.DateExtract,
                Nmbr = x.Factures.Count(),
                Virment = x.MontVir,
                DateDepotFtp = x.DateDepotFtp,
                //DateVirment=x.DateVirement,
                DateCloture = x.DateCloture,
                DateOuverture = x.DateOuverture,
                State = x.Etat,
                MontOff = x.Factures.Sum(m => m.MontOff),
                Maj = x.Factures.Sum(m => m.MontMaj)
            })
            .OrderByDescending(x => x.FirstFacture)
            .ThenByDescending(x => x.Num)
            .ToListAsync()
            .ConfigureAwait(false);

        return list;
    }

    public async ValueTask<IEnumerable<FactureDto>> GetAllFacturesAsync(bool? last, bool? ts, Period? period = null, Expression<Func<Facture, bool>>? predicate = default)
    {
        predicate = predicate.SetPeriod(period);

        if (ts == true)
            predicate = predicate.And(x =>
                x.DetailFacts.Any(d => d.Ts == true || d.DureeTrait >= MedicalThresholds.MediumTreatmentDurationDays || (d.Ppa >= MedicalThresholds.HighPriceThreshold && d.Qte >= MedicalThresholds.HighQuantityThreshold)));

        if (last == true)
            predicate = predicate.And(x => x.DateFact > YearAgo);

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
            .ToListAsync()
            .ConfigureAwait(false);

        return list;
    }

    public async ValueTask<IEnumerable<BeneficiareDto>> GetBeneficiaresAsync()
    {
        await using var db = new ChifaDb();
        return await db.Beneficiaires.Select(a => new BeneficiareDto
        {
            NumAssure = a.NumAssure,
            Rang = a.RangAd,
            Beneficiare = a.FullName,
            Assure = a.Assure!.FullName,
            Center = a.Factures.FirstOrDefault()!.Center!.Nom
        }).ToListAsync();
    }

    public async ValueTask<BeneficiareDto?> GetBeneficiareByIdAsync(string num, string rang)
    {
        await using var db = new ChifaDb();
        return await db.Beneficiaires
            .Where(x => x.NumAssure == num && x.RangAd == rang).Select(a => new BeneficiareDto
            {
                NumAssure = a.NumAssure,
                Rang = a.RangAd,
                Beneficiare = a.FullName,
                Assure = a.Assure!.FullName
            }).FirstOrDefaultAsync();
    }

    public async ValueTask<IEnumerable<Centre>> GetCentersAsync()
    {
        await using var db = new ChifaDb();
        return await db.Centres.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<FactureDetailDto>> GetFactureDetailsByIdAsync(string id)
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
            }).ToListAsync()
            .ConfigureAwait(false);
    }

    public async ValueTask<Parametre?> GetFirstOfficineAsync()
    {
        await using var db = new ChifaDb();
        return await db.Parametres.FirstOrDefaultAsync()
                .ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<Forme>> GetFormesAsync()
    {
        await using var db = new ChifaDb();
        return await db.Formes.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<MedicDto>> GetMedicamentsAsync(
        Expression<Func<Medicament, bool>>? predicate = default)
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

    public async ValueTask<IEnumerable<PatientOfTraitSpec>> GetPatientsOfTraitSpecAsync(Period? period = null,
        Expression<Func<DetailFact, bool>>? predicate = default)
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
                TS = x.Any(m => m.Ts == true),
                LongDuree = x.Any(m => m.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays)
            })
            .ToListAsync()
            .ConfigureAwait(false);

        return list;
    }

    public async ValueTask<IEnumerable<TraitDetailsDto>> GetPatientTraitementAsync(string noAssure, string rang, bool proche, Expression<Func<DetailFact?, bool>>? predicate = default)
    {
        predicate ??= _ => true;
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate!.SetPeriod(Period))
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
            .ToListAsync()
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

    public async ValueTask<IEnumerable<Utilisateur>> GetUsersAsync()
    {
        await using var db = new ChifaDb();
        return await db.Utilisateurs.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<ListeNoire>> LoadAllListNoirAsync()
    {
        await using var db = new ChifaDb();
        return await db.ListeNoires.ToListAsync().ConfigureAwait(false);
    }

    public async ValueTask<IEnumerable<ConsumptionDto>> LoadConsumptionAsync(string noAssure, string rang,
        bool distinct)
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
            .ToListAsync()
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

    public async ValueTask<IEnumerable<FactureDto>> LoadHistoryAsync(string noAssure, string rang)
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
                MontFact = f.MontFact,
                Majoration = f.MontMajFae,
                MontAss = f.MontAs,
                MontOff = f.MontOff
            })
            .OrderByDescending(x => x.DateFact)
            .ToListAsync()
            .ConfigureAwait(false);
        return query;
    }

    public async ValueTask<IEnumerable<TraitSpec2>> PatientsWithTraitSpec2Async(Expression<Func<DetailFact, bool>>? predicate = default)
    {
        predicate ??= (fact => true);
        predicate = predicate.And(x => x.Facture.DateFact > YearAgo);
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
        predicate = predicate.And(x =>
            (x.Facture.DateFact > YearAgo && x.Ppa >= MedicalThresholds.HighPriceThreshold && x.Qte >= MedicalThresholds.HighQuantityThreshold) || x.Ts == true || x.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays);
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate)
            .Select(a => new
            {
                a.Facture.NumAssure,
                Rang = a.Facture.RangAd,
                Assure = a.Facture.Assure.FullName,
                Malade = a.Facture.Beneficiaire.FullName,
                Medicament = a.Medicament.FullName(),
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
                var first = a.First();
                return new PatientWithTraitSpec
                {
                    NumAssure = a.Key.NumAssure,
                    Rang = a.Key.Rang,
                    Assure = first.Assure,
                    Malade = first.Malade,
                    DetailsDtos = a.Select(m => new TraitDetailsDto
                    {
                        Medicament = m.Medicament,
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
}

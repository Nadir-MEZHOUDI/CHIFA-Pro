using CHIFA.DAL.DTOs;
using CHIFA.DAL.Helpers;

using DataModel;

using LinqToDB;

using System.Linq.Expressions;

namespace CHIFA.DAL.DataServices;

public static class DataService
{
    public static Period period = new();
    private static readonly DateTime YearAgo = DateTime.Now.AddYears(-1);
    private static DateTime? maxDate;
    private static DateTime? minDate;

    public static async Task<IEnumerable<BordereauDto>> GetAllBordereauxAsync(  Expression<Func<Bordereau, bool>>? predicate = default)
    {        
        await using var db = new ChifaDb();
        var list = await db.Bordereaus
            .Where(predicate.SetPeriod(period))
            .Select(x => new BordereauDto
            {
                Num = x.NumBord,
                Center = x.Center.Nom,
                FirstFacture = x.DateGen,
                LastFacture = x.DateExtract,
                Nmbr = x.Factures.Count(),
                Virment = x.MontVir,
                DateDepotFtp = x.DateDepotFtp,
                DateCloture = x.DateCloture,
                DateOuverture = x.DateOuverture,
                State = x.Etat,
                MontOff = x.Factures.Sum(m => m.MontOff),
                Maj = x.Factures.Sum(m => m.MontMaj),
            })
            .OrderByDescending(x => x.FirstFacture)
            .ThenByDescending(x => x.Num)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<FactureDto>> GetAllFacturesAsync(bool? last, bool? ts, Period? period =null,  Expression<Func<Facture, bool>>? predicate = default)
    {
        predicate = predicate.SetPeriod(period);

        if (ts == true)
            predicate = predicate.And(x => x.DetailFacts.Any(d => d.Ts == true || d.DureeTrait >= 30 || (d.Ppa >= 1000 && d.Qte >= 3)));

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
                    Specialite = f.Specialite.Libelle,
                    Centre = f.Center.Nom,
                    Rang = f.RangAd,
                    TS = f.DetailFacts.Any(m => m.Ts == true),
                    LongDuree = f.DetailFacts.Any(m => m.DureeTrait >= 60)
                })
           .OrderByDescending(x => x.DateFact)
           .ThenByDescending(x => x.NumFact)
           .ToListAsync()
           .ConfigureAwait(false);

        return list;
    }

    public static async Task<IEnumerable<BeneficiareDto>> GetBeneficiaresAsync()
    {
        await using var db = new ChifaDb();
        return await db.Beneficiaires.
                      Select(a => new BeneficiareDto
                      {
                          NumAssure = a.NumAssure,
                          Rang = a.RangAd,
                          Beneficiare = a.FullName,
                          Assure = a.Assure.FullName,
                          Center = a.Factures.FirstOrDefault().Center.Nom
                      }).ToListAsync();
    }


    public static async Task<BeneficiareDto?> GetBeneficiareByIdAsync(string num, string rang)
    {
        await using var db = new ChifaDb();
        return await db.Beneficiaires
            .Where(x => x.NumAssure == num && x.RangAd == rang).
                      Select(a => new BeneficiareDto
                      {
                          NumAssure = a.NumAssure,
                          Rang = a.RangAd,
                          Beneficiare = a.FullName,
                          Assure = a.Assure.FullName,
                      }).FirstOrDefaultAsync();
    }
    public static async Task<IEnumerable<Centre>> GetCentersAsync()
    {
        await using var db = new ChifaDb();
        return await db.Centres.ToListAsync().ConfigureAwait(false);
    }

    public static async Task<IEnumerable<FactureDetailDto>> GetFacturDetailsByIdAsync(string id)
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


    public static async Task<Parametre?> GetFirstOfficineAsync()
    {
        await using var db = new ChifaDb();
        return await db.Parametres.FirstOrDefaultAsync();
    }

    public static async Task<IEnumerable<Forme>> GetFormesAsync()
    {
        await using var db = new ChifaDb();
        return await db.Formes.ToListAsync().ConfigureAwait(false);
    }

    public static async Task<IEnumerable<MedicDto>> GetMedicamentsAsync(Expression<Func<Medicament, bool>>? predicate = default)
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
               CodeMedic = x.CodeMedic,
           })
           .ToListAsync()
           .ConfigureAwait(false);
        return list;
    }

    public static async Task<string?> GetMedicObsAsync(string nEnr)
    {
        await using var db = new ChifaDb();
        return await db.Medicaments
                .Where(x => string.IsNullOrEmpty(nEnr) || x.NumEnr == nEnr)
                .Select(x => x.Observation)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
    }

    public static async Task<(DateTime min, DateTime max)> GetMinAndMaxDatesAsync()
    {
        await using var db = new ChifaDb();
        minDate ??= await db.Factures.MinAsync(x => x.DateFact).ConfigureAwait(false);
        maxDate ??= await db.Factures.MaxAsync(x => x.DateFact).ConfigureAwait(false);
        return (minDate?.AddDays(-1) ?? new DateTime(2000, 1, 1), maxDate?.AddDays(1) ?? new DateTime(2200, 1, 1));
    }

    public static async Task<IEnumerable<PatientOfTraitSpec>> GetPatientsOfTraitSpecAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = default)
    {
        
        predicate = predicate.SetPeriod(period).And(x => (x.Ppa >= 1000 && x.Qte >= 3) || x.Ts == true || x.DureeTrait >= 60);

        await using var db = new ChifaDb();
        var list = await db.DetailFacts.Where(predicate).
                          Select(a => new PatientOfTraitSpec
                          {
                              NumAssure = a.Facture.NumAssure,
                              Assure = a.Facture.Assure.FullName,
                              Rang = a.Facture.RangAd,
                              Malade = a.Facture.Beneficiaire.FullName,
                              TS = a.Ts,
                              LongDuree = a.DureeTrait >= 60
                          }

    ).ToListAsync().ConfigureAwait(false);
        list = list
            .GroupBy(x => new { x.NumAssure, x.Rang })
            .Select(x => x.FirstOrDefault())
            .ToList()!;
        return list;
    }

    public static async Task<IEnumerable<TraitDetailsDto>> GetPatientTraitementAsync(string noAssure, string rang, bool proche,
          Expression<Func<DetailFact?, bool>>? predicate = default)
    {
    
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .Where(x => x!.Facture.NumAssure == noAssure && x.Facture.RangAd == rang)
            .Where(x => x!.Ppa >= 1000 && x.Qte >= 3 || x.Ts == true || x.DureeTrait >= 60)
            .SelectMany(a => db.Beneficiaires.Where(b => b.NumAssure == noAssure && b.RangAd == rang),
                (x, a) => new
                {
                    Medicament = x!.Medicament.FullName(),
                    NEnrg = x.NumEnr,
                    x.Medicament.CodeDci,
                    Duree = x.DureeTrait,
                    TS = x.Ts,
                    Qt = x.Qte,
                    Prix = x.Ppa,
                    Specialite = x.Facture.Specialite.Libelle,
                    x.Facture.DateSoin,
                    x.Facture.DateFact
                })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query
            .OrderByDescending(x => x.DateFact)
            .GroupBy(x => new { x.CodeDci })
            .Select(x => new TraitDetailsDto
            {
                DateFact = x.FirstOrDefault()?.DateFact,
                Duree = x.FirstOrDefault()?.Duree,
                DateSoin = x.FirstOrDefault()?.DateSoin,
                Qt = x.FirstOrDefault()?.Qt,
                Medicament = x.FirstOrDefault()!.Medicament,
                Prix = x.FirstOrDefault()!.Prix,
                NEnrg = x.FirstOrDefault()!.NEnrg,
                TS = x.FirstOrDefault()?.TS,
                CodeDci = x.Key.CodeDci,
                Historic = x.Count() <= 1 ? [] : x.Select(m => new MedicHistory
                {
                    Medicament = m.Medicament,
                    Duree = m.Duree,
                    DateFact = m.DateFact,
                    Qt = m.Qt,
                    Prix = m.Prix,
                }).ToList()
            });

        if (proche)
        {
            list = list.Where(x => x.Prochain <= DateTime.Now);
        }
        return list.ToList();
    }

    public static async Task<IEnumerable<Utilisateur>> GetUsersAsync()
    {
        await using var db = new ChifaDb();
        return await db.Utilisateurs.ToListAsync().ConfigureAwait(false);
    }

    public static async Task<IEnumerable<ListeNoire>> LoadAllListNoirAsync()
    {
        await using var db = new ChifaDb();
        return await db.ListeNoires.ToListAsync().ConfigureAwait(false);
    }

    public static async Task<IEnumerable<ConsumptionDto>> LoadConsumptionAsync(string noAssure, string rang, bool distinct)
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
                Médecin = f.Specialite.Libelle,
                NEnrg = d.NumEnr
            })
            .OrderByDescending(x => x.Date)
            .ToListAsync()
            .ConfigureAwait(false);
        if (distinct)
            return query;

        var list = query
            .GroupBy(x => new { x.CodeDci })
            .Select(x => new ConsumptionGroupedDto
            {
                Facture = x.FirstOrDefault()?.Facture,
                Bord = x.FirstOrDefault()?.Bord,
                Date = x.FirstOrDefault()?.Date,
                Duree = x.FirstOrDefault()?.Duree,
                Qt = x.FirstOrDefault()?.Qt,
                Medicament = x.FirstOrDefault()!.Medicament,
                Prix = x.FirstOrDefault()?.Prix,
                NEnrg = x.FirstOrDefault()?.NEnrg,
                TS = x.FirstOrDefault()?.TS,
                Médecin = x.FirstOrDefault()?.Médecin,
                CodeDci = x.Key.CodeDci,
                Historic = x.Count() <= 1 ? [] : x.Select(m => new MedicHistory
                {
                    Facture = m.Facture,
                    Bord = m.Bord,
                    Medicament = m.Medicament,
                    Duree = m.Duree,
                    DateFact = m.Date,
                    Qt = m.Qt,
                    Prix = m.Prix,
                }).ToList()
            }).ToList();
        return list;
    }

    public static async Task<IEnumerable<Cm>> LoadControlsMedicalAsync()
    {
        await using var db = new ChifaDb();
        return await db.Cms.ToListAsync().ConfigureAwait(false);
    }

    public static async Task<IEnumerable<FactureDto>> LoadHistoryAsync(string noAssure, string rang)
    {
        var db = new ChifaDb();
        var query = await db.Factures
            .Where(x => x.NumAssure == noAssure && x.RangAd == rang)
            .Select(f => new FactureDto
            {
                NumAssure = f.NumAssure,
                NumFact = f.NumFact,
                DateSoin = f.DateSoin,
                DateFact = f.DateFact,
                Assure = f.Assure.FullName,
                Malade = f.Beneficiaire.FullName,
                Bordereau = f.NumBord,
                Specialite = f.Specialite.Libelle,
                TS = f.DetailFacts.Any(x => x.Ts == true),
                LongDuree = f.DetailFacts.Any(x => x.DureeTrait >= 60),
                MontFact = f.MontFact,
                Centre = f.Center.Nom,
                Majoration = f.MontMajFae,
                MontAss = f.MontAs,
                MontOff = f.MontOff,
                Rang = f.RangAd,
            })
            .OrderByDescending(x => x.DateFact)
            .ToListAsync()
            .ConfigureAwait(false);
        return query;
    }

    public static async Task<IEnumerable<TraitSpec2>> PatientsWithTraitSpec2Async(Expression<Func<DetailFact, bool>>? predicate = default)
    {
        predicate ??= fact => true;
        predicate = predicate.And(x => x.Facture.DateFact > YearAgo);
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Select(d => new
            {
                d.Facture.NumAssure,
                d.Facture.RangAd,
                d.NumFact,
                d.Facture.NumBord,
                d.Medicament.CodeDci,
                Medicament = d.Medicament.FullName(),
                d.Facture.DateFact,
                Duree = (d.DureeTrait == 1 && d.Qte >= 3) ? 80 : d.DureeTrait,
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
           .Where(x => x.Any(d => d.TS == true || d.Duree > 30 || d.Prix >= 1000 && d.Qt >= 3))
           .Select(x => new TraitSpec2
           {
               NumFact = x.FirstOrDefault()?.NumFact,
               Bord = x.FirstOrDefault()?.NumBord,
               Assure = x.FirstOrDefault()?.Assure!,
               DateFact = x.FirstOrDefault()?.DateFact,
               Duree = x.FirstOrDefault()?.Duree,
               Qt = x.FirstOrDefault()?.Qt,
               Medicament = x.FirstOrDefault()!.Medicament,
               Malade = x.FirstOrDefault()?.Malade,
               Prix = x.FirstOrDefault()?.Prix,
               NEnrg = x.FirstOrDefault()?.NEnrg!,
               TS = x.FirstOrDefault()?.TS ?? false,
               Code = x.Key.CodeDci,
               NumAssure = x.Key.NumAssure,
               Rang = x.Key.RangAd,
               TC = x.Any(d => d.TS == true || d.Duree > 30 || d.Prix >= 1000 && d.Qt >= 3),
               Historic = (x.Count() <= 1 ? null : x.Select(m => new MedicHistory
               {
                   Facture = m.NumFact,
                   Bord = m.NumBord,
                   Medicament = m.Medicament,
                   Duree = m.Duree,
                   DateFact = m.DateFact,
                   Prix = m.Prix,
                   Qt = m.Qt
               }).ToList())!
           })
           .OrderByDescending(x => x.Montant)
           .ToList();
        return list;
    }

    public static async Task<IEnumerable<PatientWithTraitSpec>> PatientsWithTraitSpecAsync(Expression<Func<DetailFact, bool>>? predicate = default)
    {
        predicate ??= _ => true;
        predicate = predicate.And(x => x.Facture.DateFact > YearAgo && x.Ppa >= 1000 && x.Qte >= 3 || x.Ts == true || x.DureeTrait >= 60);
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
                Duree = a.DureeTrait == 1 && a.Qte >= 3 ? 80 : a.DureeTrait,
                NEnrg = a.NumEnr,
                Qt = a.Qte,
                a.Ts,
                a.Ppa,
                Specialite=    a.Facture.Specialite.Libelle,
                a.Facture.DateSoin,
                a.Medicament.CodeDci
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query
            .OrderByDescending(x => x.DateFact)
            .GroupBy(x => new { x.NumAssure, x.Rang })
            .Select(a => new PatientWithTraitSpec
            {
                NumAssure = a.Key.NumAssure,
                Rang = a.Key.Rang,
                Assure = a.FirstOrDefault()!.Assure,
                Malade = a.FirstOrDefault()!.Malade,
                DetailsDtos = a.Select(m => new TraitDetailsDto
                {
                    Medicament = m.Medicament,
                    DateFact = m.DateFact,
                    Duree = m.Duree,
                    NEnrg = m.NEnrg,
                    Qt = m.Qt,
                    TS = m.Ts,
                    Prix=m.Ppa,
                    Specialite = m.Specialite,
                    CodeDci = m.CodeDci,
                    DateSoin = m.DateSoin,
                })
                    //.DistinctBy(x => x.NEnrg)
                    .ToList()
            }).ToList();
        return list;
    }

    public static async Task<IEnumerable<Specialite>> SpecialitesAsync()
    {
        await using var db = new ChifaDb();
        return await db.Specialites.ToListAsync().ConfigureAwait(false);
    }

    public static async Task UpdateCenter(Centre center)
    {
        await using var db = new ChifaDb();
        await db.UpdateAsync(center);
    }
}
 
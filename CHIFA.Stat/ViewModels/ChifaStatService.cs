using CHIFA.Stat.Dtos;

using LinqToDB;

namespace CHIFA.Stat.ViewModels;
public class ChifaStatService
{
    private static readonly Lazy<ChifaStatService> _instance = new(() => new ChifaStatService());
    public static ChifaStatService Instance => _instance.Value;

    private List<FactureStat>? _factures;
    public List<FactureStat> Factures => _factures ??= GetFactures() ?? new();

    private List<BordStat>? _bordereaux;
    public List<BordStat> Bordereaux => _bordereaux ??= GetBordereaux() ?? new();

    private List<MedicStat>? _medicaments;
    public List<MedicStat> Medicaments => _medicaments ??= GetMedics() ?? new();

    private readonly ChifaDb _db;

    public static DateTime LastDate { get; set; } = DateTime.Today;
    private ChifaStatService()
    {
        _db = new();
        LastDate = _db.Factures.Max(x => x.DateFact).GetValueOrDefault();
    }
    public void LoadData()
    {
        _factures = GetFactures(); _bordereaux = GetBordereaux(); _medicaments = GetMedics();
    }
    public Task LoadDataAsync() => Task.Run(LoadData);

    private List<MedicStat>? GetMedics()
    {
        try
        {
            var list = _db.DetailFacts
                         .Select(x => new MedicStat
                         {
                             DateFact = x.Facture.DateFact,
                             DateSoin = x.Facture.DateSoin,
                             NumFact = x.NumFact,
                             Qte = x.Qte,
                             CodeDci = x.Medicament.CodeDci,
                             Medicament = x.Medicament.FullName,
                             CodeMedic = x.Medicament.CodeMedic,
                             Ppa = x.Ppa,
                             NomDci = x.Medicament.NomDci,
                             Generic = x.Medicament.Generic,
                             Pays = x.Medicament.Pays,
                             Labo = x.Medicament.Laboratoire,
                             Maj = x.MajLocal + x.MajSub,
                             MontAs = x.MontAs,
                             MontOff = x.MontPharm,
                         })
                        .ToList();
            return list;
        }
        catch (Exception ex)
        {
            ex.Log();
            return null;
        }
    }
    private List<FactureStat>? GetFactures()
    {
        try
        {
            var list = _db.Factures
                         .Select(f => new FactureStat
                         {
                             NumFact = f.NumFact,
                             Beneficiaire = f.Beneficiaire.FullName,
                             Montant = f.MontFact,
                             Maj = f.MontMaj,
                             NumBord = f.NumBord,
                             DateSoin = f.DateSoin,
                             DateFact = f.DateFact,
                             Center = f.Center!.Nom,
                             NumMalad = f.NumAssure + f.RangAd,
                             NumAssure = f.NumAssure + "00",
                             Details = f.DetailFacts.Select(x => new DetailFactStat
                             {
                                 NumFact = x.NumFact,
                                 Qte = x.Qte,
                                 CodeDci = x.Medicament.CodeDci,
                                 Medicament = x.Medicament.FullName,
                                 CodeMedic = x.Medicament.CodeMedic,
                                 Ppa = x.Ppa,
                             })
                         })
                        .ToList();
            return list;
        }
        catch (Exception ex)
        {
            ex.Log();
            return null;
        }
    }
    private List<BordStat>? GetBordereaux()
    {
        try
        {
            var list = _db.Bordereaus
                .Select(x => new BordStat
                {
                    Center = x.Center!.Nom!,
                    NumBord = x.NumBord,
                    IsOpen = x.Etat == 'O',
                    Factures = x.Factures
                                .Select(f => new FactureStat
                                {
                                    NumFact = f.NumFact,
                                    Beneficiaire = f.Beneficiaire.FullName,
                                    Montant = f.MontFact,
                                    Maj = f.MontMaj,
                                    NumBord = f.NumBord,
                                    DateSoin = f.DateSoin,
                                    DateFact = f.DateFact,
                                    Center = f.Center!.Nom!,
                                    NumMalad = f.NumAssure + "-" + f.RangAd,
                                    NumAssure = f.NumAssure + "-00",
                                    Details = f.DetailFacts.Select(d => new DetailFactStat
                                    {
                                        NumFact = d.NumFact,
                                        Qte = d.Qte,
                                        CodeDci = d.Medicament.CodeDci,
                                        Medicament = d.Medicament.FullName,
                                        CodeMedic = d.Medicament.CodeMedic,
                                        Ppa = d.Ppa,
                                    })
                                })
                });

            return list.ToList();
        }
        catch (Exception ex)
        {
            ex.Log();
            return null;
        }
    }

    public static async Task<List<int>?> GetYearsAsync()
    {
        try
        {
            var db = new ChifaDb();
            var list = (await db.Factures
                .Select(x => x.DateFact!.Value.Year)
                .Distinct()
                .ToListAsync())
                .OrderDescending()
                .ToList();
            return list;
        }
        catch (Exception ex)
        {
            ex.Log();
            return null;
        }
    }

    public static List<Month> GetMonths()
    {
        return Enumerable.Range(1, 12).Select(x => new Month(x, new DateTime(2020, x, 1).ToString("MMMM"))).ToList();
    }
}

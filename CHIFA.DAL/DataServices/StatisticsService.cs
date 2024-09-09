using CHIFA.DAL.DTOs;
using CHIFA.DAL.Statistics;

using DataModel;

using LinqToDB;

using System.Linq.Expressions;

namespace CHIFA.DAL.DataServices;

public static class StatisticsService
{
    public static Period period = new();
    public static async Task<IEnumerable<BordMonthlyStatDto>> BordereauxMonthlyAsync(Expression<Func<Bordereau, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<BordMonthlyStatDto> query = await db.Bordereaus
            .Where(x => x.Etat == 'C')
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.DateExtract!.Value.Year, x.DateExtract.Value.Month })
            .Select(g => new BordMonthlyStatDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Montant = g.Sum(x => x.Factures.Sum(f => f.MontFact)),
                Factures = g.Sum(x => x.Factures.Count()),
                Borderaux = g.Count()
            })
            .OrderByDescending(x => x.Year)
            .ThenByDescending(x => x.Month)
            .ToListAsync()
            .ConfigureAwait(false);
        return query;
    }
    public static async Task<IEnumerable<BordStatDto>> BordereauxAsync(Expression<Func<Bordereau, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<BordStatDto> query = await db.Bordereaus
            .Where(x => x.Etat == 'C')
            .Where(predicate.SetPeriod(period))
            .Select(x => new BordStatDto
            {
                MontantOff = x.Factures.Sum(f => f.MontFact),
                MontantFact = x.Factures.Sum(f => f.MontOff),
                MontantMaj = x.Factures.Sum(f => f.MontMaj),
                MontantFE = x.Factures.Sum(f => f.MontMajFae),
                Factures = x.Factures.Count(),
                DateDebut = x.DateGen,
                DateFin = x.DateExtract,
                Center = x.Center.Nom,
                Num = x.NumBord,
                Virement = x.MontVir,
            })
            .OrderBy(x => x.DateFin)
            .ToListAsync()
            .ConfigureAwait(false);
        return query;
    }
    public static async Task<IEnumerable<MonthlyStat>> MonthlyAsync(Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<MonthlyStat> list = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.DateFact!.Value.Year, x.DateFact.Value.Month })
            .Select(x => new MonthlyStat
            {
                Year = x.Key.Year,
                Month = x.Key.Month,
                MontantFact = x.Sum(f => f.MontFact),
                MontantOff = x.Sum(f => f.MontOff),
                MontantMaj = x.Sum(f => f.MontMaj),
                Factures = x.Count(),
                DateDebut = x.Min(f => f.DateFact),
                DateFin = x.Max(f => f.DateFact),
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }
    public static async Task<IEnumerable<WeeklyStat>> WeeklyAsync(Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<WeeklyStat> query = await db.Factures
            .Where(predicate.SetPeriod(period))
            .OrderByDescending(x => x.DateFact)
            .GroupBy(x => new { Start = x.DateFact.Value.Date.AddDays(-(int)x.DateFact.Value.DayOfWeek) })
            .Select(x => new WeeklyStat
            {
                DateDebut = x.Key.Start,
                MontantFact = x.Sum(f => f.MontFact),
                MontantMaj = x.Sum(f => f.MontMaj),
                MontantOff = x.Sum(f => f.MontOff),
                Factures = x.Count(),
            })
            .OrderByDescending(x => x.DateDebut)
            .ToListAsync()
            .ConfigureAwait(false);
        return query;
    }
    public static async Task<IEnumerable<DailyStat>> DailyAsync(Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();

        List<DailyStat> query = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.DateFact.Value.Date})
            .Select(g => new DailyStat
            {
                DateTime=g.Key.Date,                
                MontantFact = g.Sum(f => f.MontFact),
                MontantMaj = g.Sum(f => f.MontMaj),
                MontantOff = g.Sum(f => f.MontOff),
                Factures = g.Count(),

            })
            .OrderByDescending(x => x.DateTime) 
            .ToListAsync()
            .ConfigureAwait(false);
        return query;
    }
    public static async Task<IEnumerable<ClientsStat>> ByClientAsync(Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<ClientsStat> list = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.NumAssure, x.RangAd })
            .Select(x => new ClientsStat
            {
                NumAssure = x.Key.NumAssure,
                Malade = db.Beneficiaires.FirstOrDefault(a => a.NumAssure == x.Key.NumAssure && a.RangAd == x.Key.RangAd)!.FullName,
                MantFact = x.Sum(f => f.MontFact),
                MontMaj = x.Sum(f => f.MontMaj),
                MontAss = x.Sum(f => f.MontAs),
                MontOff = x.Sum(f => f.MontOff),
                Boites = x.Sum(f => f.DetailFacts.Sum(d => d.Qte)),
                Factures = x.Count(),
            })
            .OrderByDescending(x => x.MantFact)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<ProductStat>> ProductsAsync(Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var list = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .Select(
            x => new {x.NumEnr,x.Local,  x.Qte, x.Mont, x.Ppa ,x.Medicament.CodeDci,x.Medicament.NomDci,x.Medicament.FullName })
            .ToListAsync();

        var query = list            
            .GroupBy(x => x.NumEnr)
            .Select(x => new ProductStat
            {
                CodeDci = x.FirstOrDefault().CodeDci,
                Dci = x.FirstOrDefault().NomDci,
                Produits =x.FirstOrDefault().FullName,
                Qt = x.Sum(m => m.Qte),
                Prix = x.FirstOrDefault().Ppa,
                Montant = x.Sum(m => m.Mont),
                NumEnr = x.FirstOrDefault()!.NumEnr,
                Local = x.FirstOrDefault()!.Local
            })
            .OrderByDescending(x => x.Qt)
            .ToList();

        return query;
    }

    public static async Task<IEnumerable<Gp>> PrincepsVsGenericAsync(Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<Gp> list = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => x.Medicament.Generic)
            .Select(x => new Gp { Montant = x.Sum(m => m.Mont), Type = x.Key })
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<ProductsDaily>> ProductsDailyAsync(Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<ProductsDaily> list = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => x.Facture.DateFact!.Value)
            .Select(x => new ProductsDaily
            {
                Date = x.Key.ToString("dd/MM/yyyy"),

                PrincepsCount = x.Count(m => m.Medicament.Generic == 'P'),
                PrincepsMontant = x.Where(m => m.Medicament.Generic == 'P').Sum(m => m.Mont),

                GenericCount = x.Count(m => m.Medicament.Generic == 'G'),
                GenericMontant = x.Where(m => m.Medicament.Generic == 'G').Sum(m => m.Mont),
            })
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<ProductsDaily>> ProductsMonthlyAsync(Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();

        List<ProductsDaily> query = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => $"{x.Facture.DateFact!.Value.Month:D2} - {x.Facture.DateFact.Value.Year:D4}")
            .Select(g => new ProductsDaily
            {
                Date = g.Key,

                PrincepsCount = g.Count(m => m.Medicament.Generic == 'P'),
                PrincepsMontant = g.Where(m => m.Medicament.Generic == 'P').Sum(m => m.Mont),

                GenericCount = g.Count(m => m.Medicament.Generic == 'G'),
                GenericMontant = g.Where(m => m.Medicament.Generic == 'G').Sum(m => m.Mont),
            })
            .ToListAsync()
            .ConfigureAwait(false);

        return query;
    }

    public static async Task<IEnumerable<TopSeal>> Top10ProductsByMontantAsync(Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<TopSeal> query = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.Medicament.CodeDci, x.Medicament.NomDci })
            .Select(g => new TopSeal
            {
                Code = g.Key.CodeDci,
                Produit = g.Key.NomDci,
                Qt = g.Sum(x => x.Qte),
            })
            .OrderByDescending(x => x.Qt)
            .Take(10)
            .ToListAsync()
            .ConfigureAwait(false);
        return query;
    }

    public static async Task<IEnumerable<TopSeal>> Top10ProuctsByQuantityAsync(Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<TopSeal> query = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.Medicament.CodeDci, x.Medicament.NomDci, x.Ppa })
            .Select(g => new TopSeal
            {
                Code = g.Key.CodeDci,
                Produit = g.Key.NomDci,
                Qt = g.Sum(x => x.Qte),
                Prix = g.Key.Ppa,
            })
            .OrderByDescending(x => x.Qt)
            .Take(10)
            .ToListAsync()
            .ConfigureAwait(false);
        return query;
    }
}
using CHIFA.DAL.DTOs;
using CHIFA.DAL.Helpers;
using CHIFA.DAL.Statistics;

using DataModel;

using LinqToDB;

using System.Globalization;
using System.Linq.Expressions;

namespace CHIFA.DAL.DataServices;

public static class StatisticsService
{
    public static async Task<IEnumerable<BordMonthlyStatDto>> BordereauxMonthlyAsync(Period? period = null, Expression<Func<Bordereau, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var query = await db.Bordereaus
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

    public static async Task<IEnumerable<MouvementDto>> DetailedMovementsAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<MouvementDto> list = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .Select(x => new MouvementDto
            {
                Produit = x.Medicament.FullName(),
                Qt = x.Qte,
                Date = x.Facture.DateFact,
                Code = x.NumEnr,
                Prix = x.Ppa,
            })
            .OrderBy(x => x.Date)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<FacturesByClient>> FacturesByClientAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<FacturesByClient> list = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.NumAssure, x.RangAd })
            .Select(x => new FacturesByClient
            {
                NumAssure = x.Key.NumAssure,
                Malade = db.Beneficiaires.FirstOrDefault(a => a.NumAssure == x.Key.NumAssure && a.RangAd == x.Key.RangAd)!.FullName,
                Maj = x.Sum(f => f.MontMaj),
                Factures = x.Count(),
                TR = x.Sum(f => f.MontAs),
                MantFact = x.Sum(f => f.MontFact)
            })
            .OrderByDescending(x => x.MantFact)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<FactureByMonth>> FacturesDailyAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();

        var query = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.DateFact.Value.Year, x.DateFact.Value.Month, x.DateFact.Value.Day })
            .Select(g => new FactureByMonth
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Day = g.Key.Day,
                Date = $"{g.Key.Day:D2} - {g.Key.Month:D2} - {g.Key.Year:D4}",
                Montant = g.Sum(f => f.MontFact),
                Count = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Day)
            .ToListAsync()
            .ConfigureAwait(false);

        return query;
    }

    public static async Task<IEnumerable<FactureByMonth>> FacturesMonthlyAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var list = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.DateFact!.Value.Year, x.DateFact.Value.Month })
            .Select(x => new FactureByMonth
            {
                Year = x.Key.Year,
                Month = x.Key.Month,
                Date = $"{x.Key.Month:D2}-{x.Key.Year:D4}",
                Montant = x.Sum(f => f.MontFact),
                Count = x.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    private readonly static CultureInfo culture = new("ar");

    public static async Task<IEnumerable<FactureByMonth>> FacturesWeeklyAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var query = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => x.DateFact.Value.DayOfWeek)
            .Select(g => new FactureByMonth
            {
                Date = g.Key.ToString(),
                Montant = g.Sum(f => f.MontFact),
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToListAsync()
            .ConfigureAwait(false);

        return query;
    }


    public static async Task<IEnumerable<MouvementDto>> GetStatisticsAllProductsByMonthAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var list = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => x.NumEnr)
            .Select(x => new MouvementDto
            {
                Code = x.Key,
                Produit = x.FirstOrDefault()!.Medicament.FullName(),
                Qt = x.Sum(m => m.Qte),
                Prix = x.Select(m => m.Ppa).FirstOrDefault()
            })
            .OrderByDescending(x => x.Qt)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<MouvementDto>> GlobalMovementsAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
             .Where(predicate.SetPeriod(period))
            .Select(x => new
            {
                x.NumEnr,
                FullName = $"{x.Medicament.NomCom} {x.Medicament.Dosage} {x.Medicament.Conditionnement}",
                x.Qte,
                x.Ppa,
                x.Medicament.Generic,
                x.Medicament.CodeDci,
                x.Medicament.CodeMedic,
            })            
            .ToListAsync()
            .ConfigureAwait(false);
        var list = query
            .GroupBy(p => p.NumEnr)
            .Select(g => new MouvementDto
            {
                Code = g.Key,
                Produit = g.First().FullName!,
                Qt = g.Sum(s => s.Qte),
                Prix = g.First().Ppa,
                Generic = g.First().Generic,
                CodeDci = g.First().CodeDci,
                CodeMedic = g.First().CodeMedic,
            })
            .OrderByDescending(x => x.Qt)
            .ToList();
        return list;
    }


    public static async Task<IEnumerable<Gp>> PrincepsVsGenericAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var list = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => x.Medicament.Generic)
            .Select(x => new Gp { Montant = x.Sum(m => m.Mont), Type = x.Key })
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<ProductsDaily>> ProductsDailyAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var list = await db.DetailFacts
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

    public static async Task<IEnumerable<ProductsDaily>> ProductsMonthlyAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();

        var query = await db.DetailFacts
            //.Where(x=>x.Facture.DateFact.HasValue)
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => $"{x.Facture.DateFact.Value.Month:D2} - {x.Facture.DateFact.Value.Year:D4}") //x.Facture.DateFact.Value.Date.Year + " - " + x.Facture.DateFact.Value.Date.Month )
            .Select(g => new ProductsDaily
            {
                Date =g.Key, // $"{g.Key.Month:D2} - {g.Key.Year:D4}",

                PrincepsCount = g.Count(m => m.Medicament.Generic == 'P'),
                PrincepsMontant = g.Where(m => m.Medicament.Generic == 'P').Sum(m => m.Mont),

                GenericCount = g.Count(m => m.Medicament.Generic == 'G'),
                GenericMontant = g.Where(m => m.Medicament.Generic == 'G').Sum(m => m.Mont),
            })
            .ToListAsync()
            .ConfigureAwait(false);

        return query;
    }

    public static async Task<IEnumerable<TopSeal>> Top10ProuctsByMontantAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();

        var query = await db.DetailFacts
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



    public static async Task<IEnumerable<TopSeal>> Top10ProuctsByQuantityAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
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
public static class StatisticsExtensions
{

    private static readonly Period defaultPeriod = new();

    public static Expression<Func<Facture, bool>> SetPeriod(this Expression<Func<Facture, bool>>? predicate, Period? period = default)
    {
        period ??= defaultPeriod;

        predicate ??= _ => true;

        predicate = predicate.And(x => x.DateFact != null);

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateFact > period.From);

        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateFact < period.To);

        return predicate;

    }
    public static Expression<Func<DetailFact, bool>> SetPeriod(this Expression<Func<DetailFact, bool>>? predicate, Period? period = default)
    {
        period ??= defaultPeriod;
        predicate ??= _ => true;

        predicate = predicate.And(x => x.Facture.DateFact.HasValue);

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);

        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);

        return predicate;
    }
    public static Expression<Func<Bordereau, bool>> SetPeriod(this Expression<Func<Bordereau, bool>>? predicate, Period? period = default)
    {
        period ??= defaultPeriod;

        predicate ??= _ => true;

        predicate = predicate.And(x => x.DateExtract !=null);

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateExtract > period.From);

        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateExtract < period.To);

        return predicate;
    }
    public static string FullName(this Medicament? m) => $"{m?.NomCom} {m?.Dosage} {m?.Conditionnement}";

}
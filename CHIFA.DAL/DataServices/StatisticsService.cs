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
    public static async Task<IEnumerable<BordMonthlyStatDto>> BordereauxMonthlyAsync(Period? period = default, Expression<Func<Bordereau, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var query = await db.Bordereaus
            .Where(x => x.Etat == 'C')
            .Where(predicate.SetPeriod(period))
            .Select(x => new
            {
                x.DateExtract,
                Montant = x.Factures.Sum(f => f.MontFact),
                Count = x.Factures.Count(),
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query
            .GroupBy(x => new { x.DateExtract?.Year, x.DateExtract?.Month })
            .Select(x => new BordMonthlyStatDto
            {
                Year = x.Key.Year,
                Month = x.Key.Month,
                Montant = x.Sum(m => m.Montant),
                Factures = x.Sum(m => m.Count),
                Borderaux = x.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToList();
        return list;
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

    public static async Task<IEnumerable<FactureByMonth>> FacturesDailyAsync(Period? period = default, Expression<Func<Facture, bool>>? predicate = null)
    {

        await using var db = new ChifaDb();
        var query = await db.Factures
            .Where(predicate.SetPeriod(period))
            .Select(x => new { x.DateFact, x.MontFact })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query.GroupBy(x => new { x.DateFact?.Year, x.DateFact?.Month, x.DateFact?.Day })
            .Select(x => new FactureByMonth
            {
                Year = x.Key.Year,
                Month = x.Key.Month,
                Day = x.Key.Day,

                Date = $"{x.Key.Day}-{x.Key.Month}-{x.Key.Year}",
                Montant = x.Sum(f => f.MontFact),
                Count = x.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ThenBy(x => x.Day)
            .ToList();
        return list;
    }

    public static async Task<IEnumerable<FactureByMonth>> FacturesMonthlyAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<FactureByMonth> list = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { x.DateFact!.Value.Year, x.DateFact.Value.Month })
            .Select(x => new FactureByMonth
            {
                Year = x.Key.Year,
                Month = x.Key.Month,
                Date = $"{x.Key.Month} - {x.Key.Year}",
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
            .Select(x => new { x.DateFact, x.MontFact })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query.GroupBy(x => new { DayOfWeek = x.DateFact?.ToString("dddd", culture) })
            .Select(x => new FactureByMonth
            {
                DayOfWeek = x.Key.DayOfWeek,
                Date = $"{x.Key}",
                Montant = x.Sum(f => f.MontFact),
                Count = x.Count()
            })
            .OrderBy(x => x.DayOfWeek)
            .ToList();
        return list;
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
                x.Qte,
                x.NumEnr,
                x.Ppa,
                FullName = x.Medicament.FullName(),
                x.Medicament.NomCom,
                x.Medicament.Dosage,
                x.Medicament.Conditionnement,
                x.Medicament.Generic,
                x.Medicament.CodeDci,
                x.Medicament.NomDci,
                x.Medicament.CodeMedic,
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query.GroupBy(x => x.NumEnr)
            .Select(x => new MouvementDto
            {
                Code = x.Key,
                Produit = x.FirstOrDefault()!.FullName,
                Qt = x.Sum(m => m.Qte),
                Prix = x.FirstOrDefault()?.Ppa,
                Generic = x.FirstOrDefault()!.Generic,
                CodeDci = x.FirstOrDefault()!.CodeDci,
                CodeMedic = x.FirstOrDefault()!.CodeMedic,
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
                //Date = $"{x.Key.Date.Month} - {x.Key.Date.Year} - {x.Key.Date.Day}",
                Date = x.Key.Date.ToString("d"),
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
            .Where(predicate.SetPeriod(period))
            .Select(x => new { x.Facture.DateFact, x.Medicament.Generic, x.Mont })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query
            .GroupBy(x => new { x.DateFact?.Year, x.DateFact?.Month })
            .Select(x => new ProductsDaily
            {
                Date = $"{x.Key.Year} - {x.Key.Month}",

                PrincepsCount = x.Count(m => m.Generic == 'P'),
                PrincepsMontant = x.Where(m => m.Generic == 'P').Sum(m => m.Mont),

                GenericCount = x.Count(m => m.Generic == 'G'),
                GenericMontant = x.Where(m => m.Generic == 'G').Sum(m => m.Mont),
            })
            .ToList();
        return list;
    }

    public static async Task<IEnumerable<TopSeal>> Top10MontantAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .Select(x => new
            {
                x.Qte,
                x.Ppa,
                CodeDCI = x.Medicament.CodeDci,
                x.Medicament.NomDci,
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query.GroupBy(x => x.CodeDCI)
            .Select(x => new TopSeal
            {
                Code = x.Key,
                Produit = x.FirstOrDefault()?.NomDci,
                Qt = x.Sum(m => m.Qte),
            })
            .OrderByDescending(x => x.Qt)
            .ToList();
        return list;
    }

    public static async Task<IEnumerable<TopSeal>> Top10QuantityAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {

        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate.SetPeriod(period))
            .Select(x => new
            {
                x.Qte,
                x.Ppa,
                CodeDCI = x.Medicament.CodeDci,
                x.Medicament.NomDci,
            })
            .ToListAsync().ConfigureAwait(false);

        var list = query.GroupBy(x => x.CodeDCI)
            .Select(x => new TopSeal
            {
                Code = x.Key,
                Produit = x.FirstOrDefault()?.NomDci,
                Qt = x.Sum(m => m.Qte),
                Prix = x.FirstOrDefault()?.Ppa,
            })
            .OrderByDescending(x => x.Qt).ToList();
        return list;
    }
}
public static class StatisticsExtensions
{

    private static readonly Period defaultPeriod = new();

    public static Expression<Func<Facture, bool>> SetPeriod(this Expression<Func<Facture, bool>>? predicate, Period? period = default)
    {
        period ??= defaultPeriod;
        predicate ??= _ => true;

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

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateExtract > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateExtract < period.To);
        return predicate;
    }

}
using System.Linq.Expressions;

using CHIFA.DAL.DTOs;
using CHIFA.DAL.Helpers;
using CHIFA.DAL.Statistics;

using DataModel;


using LinqToDB;

namespace CHIFA.DAL.DataServices;

public static class StatisticsService
{
    public static async Task<IEnumerable<BordMonthlyStatDto>> BordereauxMonthlyAsync(Period? period = null, Expression<Func<Bordereau, bool>>? predicate = null)
    {
        predicate ??= mouvement => true;

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateExtract > period.From);

        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateExtract < period.To);

        await using var db = new ChifaDb();
        var query = await db.Bordereaus

            .Where(x => x.Etat == 'C')
            .Where(predicate)
            .Select(x => new
            {
                x.DateGen,
                Montant = x.Factures.Sum(f => f.MontFact),
                Count = x.Factures.Count(),
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query
            
            .GroupBy(x => new { x.DateGen?.Year, x.DateGen?.Month })
            .Select(x => new BordMonthlyStatDto
            {
                Date = $"{x.Key.Month} - {x.Key.Year}",
                Montant = x.Sum(m => m.Montant),
                Factures = x.Sum(m => m.Count),
                Borderaux = x.Count()
            }).ToList();
        return list;
    }

    public static async Task<IEnumerable<MouvementDto>> DetailedMouvementsAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);

        await using var db = new ChifaDb();
        var list = await db.DetailFacts
            .Where(predicate)
            .Select(x => new MouvementDto
            {
                Produit = x.Medicament.FullName(),
                Qt = x.Qte,
                Date = x.Facture.DateFact,
                Code = x.NumEnr,
                Prix = x.Ppa,
            })
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<FacturesByClient>> FacturesByClientAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateFact < period.To);

        await using var db = new ChifaDb();
        var list = await db.Factures
            .Where(predicate)
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
            .ToListAsync().ConfigureAwait(false);
        return list.OrderByDescending(x => x.MantFact).ToList();
    }

    public static async Task<IEnumerable<FactureByMonth>> FacturesDailyAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        predicate ??= _ => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateFact < period.To);
        await using var db = new ChifaDb();
        var query = await db.Factures
            .Where(predicate)
            .Select(x => new { x.DateFact, x.MontFact })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query.GroupBy(x => new { x.DateFact?.Year, x.DateFact?.Month, x.DateFact?.Day })
            .Select(x => new FactureByMonth
            {
                Date = $"{x.Key.Day}-{x.Key.Month}-{x.Key.Year}",
                Montant = x.Sum(f => f.MontFact),
                Count = x.Count()
            }).ToList();
        return list;
    }

    public static async Task<IEnumerable<FactureByMonth>> FacturesMonthlyAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateFact < period.To);
        await using var db = new ChifaDb();
        var list = await db.Factures
            .Where(predicate)
            .GroupBy(x => new { x.DateFact!.Value.Year, x.DateFact.Value.Month })
            .Select(x => new FactureByMonth
            {
                Date = $"{x.Key.Month} - {x.Key.Year}",
                Montant = x.Sum(f => f.MontFact),
                Count = x.Count()
            })
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }

    public static async Task<IEnumerable<FactureByMonth>> FacturesWeeklyAsync(Period? period = null, Expression<Func<Facture, bool>>? predicate = null)
    {
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateFact < period.To);
        await using var db = new ChifaDb();
        var query = await db.Factures
            .Where(predicate)
            .Select(x => new { x.DateSoin, x.MontFact })
            .ToListAsync()
            .ConfigureAwait(false);

        var list = query.GroupBy(x => new { x.DateSoin?.DayOfWeek })
            .Select(x => new FactureByMonth
            {
                Date = $"{x.Key}",
                Montant = x.Sum(f => f.MontFact),
                Count = x.Count()
            }).ToList();
        return list;
    }

    public static async Task<IEnumerable<MouvementDto>> GetStatisticsAllProductsByMonthAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);
        await using var db = new ChifaDb();
        var list = await db.DetailFacts
            .Where(predicate)
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
        predicate ??= _ => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);

        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate)
            .Select(x => new
            {
                x.Qte,
                x.NumEnr,
                x.Ppa,
                FullName=x.Medicament.FullName(),
                x.Medicament.NomCom,
                x.Medicament.Dosage,
                x.Medicament.Conditionnement,
                x.Medicament.Generic,
                x.Medicament.CodeDci,
                x.Medicament.NomDci,
                x.Medicament.CodeMedic,
            })
            .ToListAsync().ConfigureAwait(false);

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
            .OrderByDescending(x => x.Qt).ToList();
        return list;
    }

    public static async Task<IEnumerable<Gp>> PrincepsVsGenericAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);
        await using var db = new ChifaDb();
        var list = await db.DetailFacts
            .Where(predicate)
            .GroupBy(x => x.Medicament.Generic)
            .Select(x => new Gp { Montant = x.Sum(m => m.Mont), Type = x.Key })
            .ToListAsync()
            .ConfigureAwait(false);

        return list;
    }

    public static async Task<IEnumerable<ProductsDaily>> ProductsDailyAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);
        await using var db = new ChifaDb();
        var list = await db.DetailFacts
            .Where(predicate)
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
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);

        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate)
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
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);

        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate)
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
        predicate ??= mouvement => true;
        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact > period.From);
        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.Facture.DateFact < period.To);

        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate)
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
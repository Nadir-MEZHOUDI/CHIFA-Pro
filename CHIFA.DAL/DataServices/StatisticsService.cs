using CHIFA.DAL.DTOs;
using CHIFA.DAL.Helpers;
using CHIFA.DAL.Statistics;

using DataModel;

using LinqToDB;

using System.Linq.Expressions;

namespace CHIFA.DAL.DataServices;

public static class StatisticsService
{
    public static Period period = new();
    public static async Task<List<ThisWeekStat>> GetThisWeekStatsAsync()
    {

        await using var db = new ChifaDb();
        DateTime firstDate = DateTime.Today.AddDays(-8);
        List<ThisWeekStat> weekStats = await db.Factures
                                  .Where(f => f.DateFact.Value.Date >= firstDate)
                                  .GroupBy(f => f.DateFact.Value.Date)
                                  .Select(g => new ThisWeekStat
                                  {
                                      Date = g.Key,
                                      Count = g.Count(),
                                      Montant = g.Sum(f => f.MontFact),
                                      MontantAs = g.Sum(f => f.MontAs),
                                      MontantOff = g.Sum(f => f.MontOff),
                                      Maj = g.Sum(f => f.MontMaj),
                                  })
                                  .ToListAsync();

        return Enumerable.Range(0, 8)
                               .Select(x => DateTime.Today.AddDays(-x))
                               .Select(date => weekStats.FirstOrDefault(ws => ws.Date == date) ?? new ThisWeekStat(date))
                               .OrderByDescending(ws => ws.Date)
                               .ToList();

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
                Assuries=x.Factures.Select(f => f.NumAssure).Distinct().Count(),
                Beneficiaires = x.Factures.Select(f => f.NumAssure + f.RangAd).Distinct().Count(),
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
    public static async Task<IEnumerable<YearlyStat>> YearlyAsync(Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<YearlyStat> list = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x =>  x.DateFact.Value.Date.Year )
            .Select(x => new YearlyStat
            {
                Year = x.Key,
                MontantFact = x.Sum(f => f.MontFact),
                MontantOff = x.Sum(f => f.MontOff),
                MontantMaj = x.Sum(f => f.MontMaj),
                MontantFE = x.Sum(f => f.MontMajFae),
                Assureis = x.Select(f => f.NumAssure).Distinct().Count(),
                Beneficiaires = x.Select(f => f.NumAssure+f.RangAd).Distinct().Count(),
                Factures = x.Count(),
                DateDebut = x.Min(f => f.DateFact),
                DateFin = x.Max(f => f.DateFact),
            })
            .OrderBy(x => x.Year)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }
    public static async Task<IEnumerable<MonthlyStat>> MonthlyAsync(Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<MonthlyStat> list = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { Year = $"{x.DateFact!.Value.Year}", Month = $"{x.DateFact.Value.Month}" })
            .Select(x => new MonthlyStat
            {
                Year = x.Key.Year,
                Month = x.Key.Month,
                MontantFact = x.Sum(f => f.MontFact),
                MontantOff = x.Sum(f => f.MontOff),
                MontantMaj = x.Sum(f => f.MontMaj),
                MontantFE = x.Sum(f => f.MontMajFae),
                Factures = x.Count(),
                DateDebut = x.Min(f => f.DateFact),
                DateFin = x.Max(f => f.DateFact),
                Assureis = x.Select(f => f.NumAssure).Distinct().Count(),
                Beneficiaires = x.Select(f => f.NumAssure + f.RangAd).Distinct().Count(),
            })
            .OrderBy(x => x.DateDebut)
            .ToListAsync()
            .ConfigureAwait(false);
        return list;
    }
    public static async Task<IEnumerable<WeeklyStat>> WeeklyAsync(Expression<Func<Facture, bool>>? predicate = null)
    {
        await using var db = new ChifaDb();
        List<WeeklyStat> query = await db.Factures
            .Where(predicate.SetPeriod(period))
            .GroupBy(x => new { Start = $"{x.DateFact.Value.Date.AddDays(-(int)x.DateFact.Value.Date.DayOfWeek)}" })
            .Select(x => new WeeklyStat
            {
                Key = x.Key.Start,
                DateDebut = DateTime.Parse(x.Key.Start),
                MontantFact = x.Sum(f => f.MontFact),
                MontantMaj = x.Sum(f => f.MontMaj),
                MontantOff = x.Sum(f => f.MontOff),
                MontantFE = x.Sum(f => f.MontMajFae),
                Factures = x.Count(),
                Assureis = x.Select(f => f.NumAssure).Distinct().Count(),
                Beneficiaires = x.Select(f => f.NumAssure + f.RangAd).Distinct().Count(),
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
            .GroupBy(x => x.DateFact!.Value.Date)
            .Select(g => new DailyStat
            {
                DateTime = g.Key,
                MontantFact = g.Sum(f => f.MontFact),
                MontantMaj = g.Sum(f => f.MontMaj),
                MontantOff = g.Sum(f => f.MontOff),
                MontantFE = g.Sum(f => f.MontMajFae),
                Factures = g.Count(),
                Assureis = g.Select(f => f.NumAssure).Distinct().Count(),
                Beneficiaires = g.Select(f => f.NumAssure + f.RangAd).Distinct().Count(),
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
                NumAssure = x.Key.NumAssure!,
                Malade = db.Beneficiaires.Where(a => a.NumAssure == x.Key.NumAssure && a.RangAd == x.Key.RangAd).Select(b => b.Nom + " " + b.Prenom).FirstOrDefault(),
                MantFact = x.Sum(f => f.MontFact),
                MontMaj = x.Sum(f => f.MontMaj),
                MontAss = x.Sum(f => f.MontAs),
                MontOff = x.Sum(f => f.MontOff),
                MontFE = x.Sum(f => f.MontMajFae),
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
            x => new { x.NumEnr, x.Local, x.InfTr, x.MajLocal, x.MajSub, x.Medicament.Generic, x.Qte, x.Mont, x.Ppa, x.Medicament.Dosage, x.Medicament.Conditionnement, x.Medicament.NomCom, x.Medicament.CodeDci, x.Medicament.NomDci, x.Medicament.FullName })
            .ToListAsync();

        var query = list
            .GroupBy(x => x.NumEnr)
            .Select(x => new ProductStat
            {
                NomCom = x.FirstOrDefault().NomCom,
                CodeDci = x.FirstOrDefault().CodeDci,
                Dci = x.FirstOrDefault().NomDci,
                Condition = x.FirstOrDefault().Conditionnement,
                Dosage = x.FirstOrDefault().Dosage,
                Qt = x.Sum(m => m.Qte),
                Prix = x.FirstOrDefault().Ppa,
                Montant = x.Sum(m => m.Mont),
                NumEnr = x.FirstOrDefault()!.NumEnr,
                Local = x.FirstOrDefault()!.Local,
                Generic = x.FirstOrDefault()!.Generic,
                InfTr = x.FirstOrDefault()!.InfTr,
            })
            .OrderByDescending(x => x.Qt)
            .ToList();

        return query;
    }
    public static Expression<Func<Facture, bool>> SetPeriod(this Expression<Func<Facture, bool>>? predicate, Period? period = default)
    {
        period ??= period;

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
        period ??= period;
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
        period ??= period;

        predicate ??= _ => true;

        predicate = predicate.And(x => x.DateExtract != null);

        if (period?.From.HasValue == true)
            predicate = predicate.And(x => x.DateExtract > period.From);

        if (period?.To.HasValue == true)
            predicate = predicate.And(x => x.DateExtract < period.To);

        return predicate;
    }
    public static string FullName(this Medicament? m) => $"{m?.NomCom} {m?.Dosage} {m?.Conditionnement}";

}
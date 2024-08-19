using CHIFA.DAL.Statistics;
using LinqToDB;
using DataModel;


namespace CHIFA.DAL.DataServices;

public static class WeekStatService
{
    public static   List<WeekStat> GetWeekStats()
    {

        var db = new ChifaDb();

        var first = DateTime.Today.AddDays(-7);

        var range = Enumerable.Range(0, 7).Select(x => DateTime.Today.AddDays(-x)).ToList();


        var list =   range
                   .GroupJoin(
                       db.Factures
                       .Where(f => f.DateFact.Value.Date >= first)
                       .Where(f => range.Contains(f.DateFact.Value.Date)),
                       date => date,
                       facture => facture.DateFact.Value.Date,
                       (date, factures) => new { Date = date, Factures = factures.DefaultIfEmpty() }
                   )
                   .Select(x => new WeekStat
                   {
                       Date = x.Date,
                       Count = x.Factures.Count(f => f != null),
                       Montant = x.Factures.Where(f => f != null).Sum(f => f.MontFact),
                       MontantAs = x.Factures.Where(f => f != null).Sum(f => f.MontAs),
                       MontantOff = x.Factures.Where(f => f != null).Sum(f => f.MontOff),
                       Maj = x.Factures.Where(f => f != null).Sum(f => f.MontMaj),
                   })
                   .OrderByDescending(x => x.Date)
                   
                   .ToList();

        return list;
    }

}



using CHIFA.DAL.Statistics;

using DataModel;

using LinqToDB;

namespace CHIFA.DAL.DataServices;

public static class WeekStatService
{
    public static async Task<List<WeekStat>> GetWeekStatsAsync()
    {

        await using var db = new ChifaDb();
        DateTime firstDate = DateTime.Today.AddYears(-1).AddDays(-8);
        var weekStats = await db.Factures
                                  .Where(f => f.DateFact.Value.Date >= firstDate)
                                  .GroupBy(f => f.DateFact.Value.Date)
                                  .Select(g => new WeekStat
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
                               .Select(x => DateTime.Today.AddYears(-1).AddDays(-x))
                               .Select(date => weekStats.FirstOrDefault(ws => ws.Date == date) ?? new WeekStat(date))
                               .OrderByDescending(ws => ws.Date)
                               .ToList();
         
    }
}


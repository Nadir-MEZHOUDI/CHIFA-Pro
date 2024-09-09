using DataModel;

using LinqToDB;

using System.Globalization;

namespace CHIFA.DAL.DataServices;

public static class WeekStatService
{
    public static async Task<List<WeekStat>> GetWeekStatsAsync()
    {

        await using var db = new ChifaDb();
        DateTime firstDate = DateTime.Today.AddDays(-8);
        List<WeekStat> weekStats = await db.Factures
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
                               .Select(x => DateTime.Today.AddDays(-x))
                               .Select(date => weekStats.FirstOrDefault(ws => ws.Date == date) ?? new WeekStat(date))
                               .OrderByDescending(ws => ws.Date)
                               .ToList();

    }
    public class WeekStat
    {
        public WeekStat(DateTime date)
        {
            Date = date;
        }
        public WeekStat()
        {

        }

        private static readonly CultureInfo culture = new("ar");
        public DateOnly DateFact => DateOnly.FromDateTime(Date);
        public string Day => DateFact.ToString("dddd", culture).ToUpper();
        public int Count { get; set; } = 0;
        public DateTime Date { get; set; }
        public decimal? Montant { get; set; } = 0;
        public decimal? MontantAs { get; set; } = 0;
        public decimal? MontantOff { get; set; } = 0;
        public decimal? Maj { get; set; } = 0;
    }

}


using System.Globalization;

namespace CHIFA.DAL.Statistics;

public class ThisWeekStat
{
    public ThisWeekStat(DateTime date) => Date = date;
    public ThisWeekStat() { }

    private static readonly CultureInfo culture = new("ar");
    public DateOnly DateFact => DateOnly.FromDateTime(Date);
    public string Day => DateFact.ToString("dddd", culture).ToUpper();
    public int Count { get; set; }
    public DateTime Date { get; set; }
    public decimal? Montant { get; set; } = 0;
    public decimal? MontantAs { get; set; } = 0;
    public decimal? MontantOff { get; set; } = 0;
    public decimal? Maj { get; set; } = 0;
}




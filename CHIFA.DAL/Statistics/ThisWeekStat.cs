using System.Globalization;

namespace CHIFA.DAL.Statistics;

public class ThisWeekStat
{
    private static readonly CultureInfo Culture = new("ar");
    public ThisWeekStat(DateTime date) => Date = date;
    public ThisWeekStat() { }
    public DateOnly? DateFact => DateOnly.FromDateTime(Date);
    public string? Day => DateFact?.ToString("dddd", Culture).ToUpper();
    public int Count { get; set; }
    public DateTime Date { get; set; }
    public decimal? Montant { get; set; } = 0;
    public decimal? MontantAs { get; set; } = 0;
    public decimal? MontantOff { get; set; } = 0;
    public decimal? Maj { get; set; } = 0;
}




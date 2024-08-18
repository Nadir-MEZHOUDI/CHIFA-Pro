using System.Globalization;
namespace CHIFA.DAL.Statistics;

public class WeekStat
{
    private static readonly CultureInfo culture = new("ar-AR");
    public DateOnly DateFact => DateOnly.FromDateTime(Date);
    public string Day => DateFact.ToString("dddd", culture).ToUpper();
    public int Count { get; set; }
    public DateTime Date { get; set; }
    public decimal? Montant { get; set; }
    public decimal? MontantAs { get; set; }
    public decimal? MontantOff { get; set; }
    public decimal? Maj { get;  set; }
}



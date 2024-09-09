using System.Globalization;

namespace CHIFA.DAL.Statistics;

public class DailyStat
{
    private static readonly CultureInfo culture = new("fr");
    public string Date => DateTime.ToString("dd - MM - yyyy");
    public string Day => DateTime.ToString("dddd", culture).ToUpper();
    public DateTime DateTime { get; set; }
    public int Factures { get; set; } = 0;
    public decimal? MontantFacture => MontantOff / Factures;
    public decimal? MontantMaj { get;  set; }
    public decimal? MontantFact { get;  set; }
    public decimal? MontantOff { get;  set; }
    public decimal? Marge => MontantOff / 6;
    public decimal? Brute => Marge + MontantMaj;
}

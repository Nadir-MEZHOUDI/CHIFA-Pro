namespace CHIFA.Contract.Statistics;

public class DailyStat
{
    private static readonly CultureInfo Culture = new("ar");
    public string Date => DateTime.ToString("dd - MM - yyyy");
    public string Day => DateTime.ToString("dddd", Culture).ToUpper();
    public DateTime DateTime { get; init; }
    public int Factures { get; init; }
    public decimal? MontantMaj { get;  init; }
    public decimal? MontantFact { get;  init; }
    public decimal? MontantOff { get;  init; }
    public decimal? MontantFE { get; init; }
    public int Assureis { get; init; }
    public int Beneficiaires { get; init; }
    public decimal? MontantFacture => (MontantOff / Factures).ToDecimal();
    public decimal? Marge =>( MontantOff / 6).ToDecimal();
    public decimal? Brute => (Marge + MontantMaj).ToDecimal();
    public decimal? MontantGlobal => (MontantOff + MontantMaj + MontantFE).ToDecimal();

}

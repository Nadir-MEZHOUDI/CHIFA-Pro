namespace CHIFA.DAL.Statistics;

public class BordMonthlyStatDto
{
    public int? Year { get; set; }=0;
    public int? Month { get; set; } = 0;
    public string Date => $"{Month} - {Year}";
    public int Borderaux { get; set; } = 0;
    public int Factures { get; set; } =0;
    public decimal? Montant { get; set; } = 0;
}
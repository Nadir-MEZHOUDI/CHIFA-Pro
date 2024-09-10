namespace CHIFA.DAL.Statistics;

public class ClientsStat
{
    public int Factures { get; set; }
    public decimal Boites { get; set; }
    public decimal? MontOff { get; set; }
    public decimal? MantFact { get; set; }
    public decimal? MontMaj { get; set; }
    public decimal? MontAss { get; set; }
    public decimal? MontFE { get; set; }
    public decimal? MontGlobal => MontOff + MontMaj + MontFE;
    public decimal? Marge => MantFact / 6;
    public decimal? Brut => Marge + MontMaj;
    public string Malade { get; set; } = "";
    public string NumAssure { get; set; }
}

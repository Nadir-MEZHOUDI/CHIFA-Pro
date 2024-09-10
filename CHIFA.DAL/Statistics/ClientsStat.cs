using CHIFA.DAL.Helpers;

namespace CHIFA.DAL.Statistics;

public class ClientsStat
{    
    public string NumAssure { get; init; }
    public string? Malade { get; init; } 
    public int Factures { get; init; }
    public decimal Boites { get; init; }
    public decimal? MontOff { get; init; }
    public decimal? MantFact { get; init; }
    public decimal? MontMaj { get; init; }
    public decimal? MontAss { get; init; }
    public decimal? MontFE { get; init; }
    public decimal? MontGlobal => (MontOff + MontMaj + MontFE).ToDecimal()  ;
    public decimal? Marge => (MantFact / 6).ToDecimal();
    public decimal? Brut => (Marge + MontMaj).ToDecimal();

}

namespace CHIFA.Services.Statistics;

public class ProductStat
{
    public decimal Qt { get; init; } = 0;
    public decimal Prix { get; init; } = 0;
    public string? CodeDci { get; init; }
    public decimal Montant { get; init; } = 0;
    public decimal? MontAss { get; init; }
    public string? Dci { get; init; }
    public bool? Local { get; init; }
    public string? NumEnr { get; init; }
    public string? NomCom { get; init; }
    public char? Generic { get; init; }
    public bool? InfTr { get; init; }
    public string? Dosage { get; init; }
    public string? Condition { get; init; }  
    public decimal Total => Qt * Prix;
    public string Produits => $"{NomCom} {Dosage} {Condition}";

    public decimal? Maj { get;  set; }
    public decimal? Marge => (Total / 6).ToDecimal();
    public decimal? Brut => (Maj + Marge).ToDecimal();
}
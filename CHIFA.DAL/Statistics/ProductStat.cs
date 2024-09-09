namespace CHIFA.DAL.Statistics;

public class ProductStat
{
    public decimal Qt { get; internal set; }
    public decimal Prix { get; internal set; }
    public decimal Total => Qt * Prix;
    public string Produits { get; internal set; }
    public string? CodeDci { get; internal set; }
    public decimal Montant { get; internal set; }
    public string? Dci { get; internal set; }
    public bool? Local { get; internal set; }
    public string NumEnr { get; internal set; }
}
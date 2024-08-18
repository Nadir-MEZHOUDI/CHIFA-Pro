namespace CHIFA.DAL.DTOs;

public class TopSeal
{
    public string Code { get; set; }
    public decimal? Prix { get; internal set; }
    public string Produit { get; set; }
    public decimal? Qt { get; set; }
}
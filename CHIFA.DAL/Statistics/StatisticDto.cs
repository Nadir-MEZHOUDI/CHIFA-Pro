namespace CHIFA.DAL.Statistics;

public record StatisticDto
{
    public string Code { get; set; }
    public DateTime? Date { get; set; }
    public decimal Montant { get; set; }
    public string Produit { get; set; }
    public decimal? Qt { get; set; }
}
namespace CHIFA.DAL.Statistics;

public class FacturesByClient
{
    public int? Factures { get; set; }
    public decimal? Maj { get; set; }
    public string Malade { get; set; }
    public decimal? MantFact { get; set; }
    public decimal? Marge => MantFact * (decimal?)0.1667;
    public string NumAssure { get; set; }
    public decimal? TR { get; set; }
}
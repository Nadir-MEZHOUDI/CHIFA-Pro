namespace CHIFA.Stat.Dtos;

public class DetailFactStat
{
    public string? NumFact { get; set; }
    public decimal Qte { get; set; } = 0;
    public string? CodeDci { get; set; }
    public string? Medicament { get; set; }
    public string? CodeMedic { get; set; }
    public decimal Ppa { get; set; } = 0;
}
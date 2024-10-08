namespace CHIFA.Stat.Dtos;

public class MedicStat
{
    public string? NumFact { get; set; }
    public decimal Qte { get; set; }
    public string? CodeDci { get; set; }
    public string? Medicament { get; set; }
    public string? CodeMedic { get; set; }
    public string? NomDci { get; set; }
    public decimal Ppa { get; set; }
    public char? Generic { get; set; }
    public string? Pays { get; set; }
    public string? Labo { get; set; }
    public decimal? Maj { get; set; }
    public decimal? MontAs { get; set; }
    public decimal? MontOff { get; set; }
    public DateTime? DateSoin { get; internal set; }
    public DateTime? DateFact { get; internal set; }
}

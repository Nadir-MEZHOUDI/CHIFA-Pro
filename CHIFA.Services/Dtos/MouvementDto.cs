namespace CHIFA.Contract.Dtos;

public class MouvementDto
{
    public string? Code { get; set; }
    public string? CodeDci { get; set; }
    public string? CodeMedic { get; set; }
    public DateTime? Date { get; set; }
    public char? Generic { get; set; }
    public string? Labo { get; set; }
    public decimal? Montant => Prix * Qt;
    public decimal? Prix { get; set; }
    public decimal? Qt { get; set; }
    public string Produit { get; set; } = "*";

}
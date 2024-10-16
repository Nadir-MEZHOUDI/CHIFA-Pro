namespace CHIFA.Contract.Dtos;

public class MedicHistory
{
    public DateTime? DateFact { get; set; }
    public decimal? Duree { get; set; }
    public string? Medicament { get; set; }
    public decimal? Prix { get; set; }
    public decimal? Qt { get; set; }
    public string? Facture { get;  set; }
    public string? Bord { get;  set; }
}
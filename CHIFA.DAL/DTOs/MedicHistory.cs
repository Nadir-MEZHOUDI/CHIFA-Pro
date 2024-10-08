namespace CHIFA.DAL.DTOs;

public class MedicHistory
{
    public DateTime? DateFact { get; set; }
    public decimal? Duree { get; set; }
    public string? Medicament { get; set; }
    public decimal? Prix { get; set; }
    public decimal? Qt { get; set; }
    public string? Facture { get; internal set; }
    public string? Bord { get; internal set; }
}
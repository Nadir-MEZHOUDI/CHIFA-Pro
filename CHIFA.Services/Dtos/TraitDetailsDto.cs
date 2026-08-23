namespace CHIFA.Services.Dtos;

public class TraitDetailsDto
{
    public string? CodeDci { get; set; }
    public DateTime? DateFact { get; set; }
    public DateTime? DateSoin { get; set; }
    public decimal? Duree { get; set; }
    public List<MedicHistory> Historic { get; set; }= [];
    public string? Medicament { get; set; }
    public string? NEnrg { get; set; }
    public decimal? Prix { get; set; }
    public DateTime? Prochain => DateFact?.AddDays((double?)Duree ?? (double?)Qt * 30 ?? 0d);
    public decimal? Qt { get; set; }
    public string? Specialite { get; set; }
    public bool? TS { get; set; }
}
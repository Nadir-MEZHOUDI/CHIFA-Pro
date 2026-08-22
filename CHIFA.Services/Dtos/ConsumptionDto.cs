namespace CHIFA.Contract.Dtos;

public class ConsumptionDto
{
    public string? CodeDci { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public decimal? Duree { get; set; } = 0;
    public string? Medecin { get; set; } = string.Empty;
    public string? Medicament { get; set; } = string.Empty;
    public string? NEnrg { get; set; }
    public decimal? Prix { get; set; }
    public DateTime? Prochain => Date?.AddDays((double?)Duree ?? 0d);
    public decimal? Qt { get; set; }
    public bool? TS { get; set; }
    public string? Facture { get; set; } = string.Empty;
    public string? Bord { get; set; } = string.Empty;

    public override bool Equals(object? o) => NEnrg == (o as ConsumptionDto)?.NEnrg;
    public override int GetHashCode() => NEnrg?.GetHashCode() ?? 0;
}

public class ConsumptionGroupedDto : ConsumptionDto
{
    public List<MedicHistory>? Historic { get; set; }
}

namespace CHIFA.Services.Dtos;

public class TraitSpec2
{
     public string? Assure { get; set; }
    public string? Code { get; set; }
    public DateTime? DateFact { get; set; }
    public decimal? Duree { get; set; }
    public List<MedicHistory> Historic { get; set; } = [];
    public string Malad => $"{NumAssure}-{Rang} | {Malade}{(Rang == "00" ? "" : $" | ({Assure})")}";
    public string? Malade { get; set; }
    public string? Medicament { get; set; }
    public string? NomCommercial { get; set; }
    public string? Dosage { get; set; }
    public string? Cond { get; set; }
    public decimal? Montant => Prix * Qt;
    public string? NEnrg { get; set; }
    public string? NumAssure { get; set; }
    public decimal? Prix { get; set; }
    public DateTime? Prochain => DateFact?.AddDays((double)(Duree ?? 0));
    public decimal? Qt { get; set; }
    public string Rang { get; set; } = "";
    public bool TC { get; set; }
    public bool TS { get; set; }
    public string? NumFact { get;  set; }
    public string? Bord { get;  set; }
}
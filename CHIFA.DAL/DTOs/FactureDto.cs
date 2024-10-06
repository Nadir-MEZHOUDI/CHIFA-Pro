using System.Globalization;

namespace CHIFA.DAL.DTOs;

public class FactureDto
{
    private static CultureInfo frCulture = new CultureInfo("fr-FR");
    public string? Assure { get; set; }
    public string? Bordereau { get; set; }
    public string? Centre { get; set; }
    public DateTime? DateFact { get; set; }
    public DateTime? DateSoin { get; set; }
    public bool LongDuree { get; set; }
    public decimal? Majoration { get; set; }
    public string? Malade { get; set; }
    public decimal? MontAss { get; set; }
    public decimal? MontFact { get; set; }
    public decimal? MontOff { get; set; }
    public string? NumFact { get; init; }
    public string? NumAssure { get; set; }
    public string? Rang { get; set; }
    public string? Specialite { get; set; }
    public bool? TS { get; set; }
    public string? Time => DateFact.HasValue ? TimeOnly.FromDateTime(DateFact.Value).ToShortTimeString() : "";
    public string JourSemaine => DateFact?.ToString("dddd", frCulture).ToUpper() ?? string.Empty;

    public override bool Equals(object? o)
    {
        return o is FactureDto f && NumFact == f.NumFact;
    }
    public override int GetHashCode() => NumFact?.GetHashCode() ?? 0;
}

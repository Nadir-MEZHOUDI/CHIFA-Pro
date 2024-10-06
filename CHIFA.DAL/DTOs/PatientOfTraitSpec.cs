namespace CHIFA.DAL.DTOs;

public class PatientOfTraitSpec
{
    public string? Assure { get; set; }
    public bool LongDuree { get; set; }
    public string? Malade { get; set; }
    public string? NumAssure { get; set; }
    public string? Rang { get; set; }
    public bool? TS { get; set; }

    public override bool Equals(object? obj) => NumAssure == (obj as PatientOfTraitSpec)?.NumAssure && Rang == (obj as PatientOfTraitSpec)?.Rang;
}
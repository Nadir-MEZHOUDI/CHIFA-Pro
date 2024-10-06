namespace CHIFA.DAL.DTOs;

public class PatientWithTraitSpec
{
    public string? Assure { get; set; }
    public List<TraitDetailsDto> DetailsDtos { get; set; } = [];
    public bool? LongDuree => DetailsDtos?.Any(x => x.Duree >= 60);
    public string? Malade { get; set; }
    public string? NumAssure { get; set; }
    public string? Rang { get; set; }
    public bool? TS => DetailsDtos?.Any(x => x.TS == true);

    public override bool Equals(object? obj)
    {
        return NumAssure == (obj as PatientWithTraitSpec)?.NumAssure && Rang == (obj as PatientWithTraitSpec)?.Rang;
    }

    // ReSharper disable NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => (NumAssure?.GetHashCode() ?? 0) ^ (Rang?.GetHashCode() ?? 0);
}
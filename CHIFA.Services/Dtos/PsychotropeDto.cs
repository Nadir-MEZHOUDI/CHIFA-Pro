namespace CHIFA.Services.Dtos;

public class PsychotropeDto
{
    public string? NumFact { get; set; }
    public DateTime? DateDelivrance { get; set; }
    public string? NumAssure { get; set; }
    public string? NomAssure { get; set; }
    public string? NomBeneficiaire { get; set; }
    public string? Medecin { get; set; }
    public string? CategoriePsychotrope { get; set; }
    public string? Tableau { get; set; }
    public string? MedicamentNom { get; set; }
    public string? Dci { get; set; }
    public string? Dosage { get; set; }
    public string? Forme { get; set; }
    public decimal? Quantite { get; set; }
    public decimal? DureeTrait { get; set; }
    public decimal? Ppa { get; set; }
    public bool EstTraitementSpecial { get; set; }
}

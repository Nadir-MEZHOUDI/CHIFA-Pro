using System.ComponentModel.DataAnnotations;

namespace CHIFA.Services.Dtos;

/// <summary>A medicine sold again to the same beneficiary before the previous treatment of the same DCI ended.</summary>
public class EarlyRefillDto
{
    public string? Malade { get; set; }
    public string? Assure { get; set; }
    [Display(Name = "N° Assuré")] public string? NumAssure { get; set; }
    public string? Rang { get; set; }
    [Display(Name = "Médicament")] public string? Medicament { get; set; }
    [Display(Name = "Date vente")] public DateTime? DateVente { get; set; }
    [Display(Name = "Facture")] public string? NumFact { get; set; }
    [Display(Name = "Fin traitement précédent")] public DateTime? FinTraitement { get; set; }
    [Display(Name = "Jours d'avance")] public int JoursAvance { get; set; }
    [Display(Name = "Vente précédente")] public DateTime? DateVentePrecedente { get; set; }
    [Display(Name = "Facture précédente")] public string? NumFactPrecedente { get; set; }
    [Display(Name = "Durée précédente")] public decimal? DureePrecedente { get; set; }
    [Display(Name = "Durée")] public decimal? Duree { get; set; }
    [Display(Name = "Qté")] public decimal? Qte { get; set; }
    public decimal? Montant { get; set; }
    public string? Bordereau { get; set; }

    /// <summary>The facture can still be corrected or removed before the bordereau is sent.</summary>
    [Display(Name = "Non envoyé")] public bool BordereauOuvert { get; set; }

    [Display(Name = "Spécialité")] public string? Specialite { get; set; }
    [Display(Name = "DCI")] public string? CodeDci { get; set; }
}

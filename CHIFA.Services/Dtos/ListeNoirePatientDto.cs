using System.ComponentModel.DataAnnotations;

namespace CHIFA.Services.Dtos;

/// <summary>A patient of the pharmacy who used a Chifa card that is now in the CNAS liste noire.</summary>
public class ListeNoirePatientDto
{
    /// <summary>The blacklisted card is the last one the patient used: the next sale with it will be refused.</summary>
    [Display(AutoGenerateField = false)] public bool CarteActuelle { get; set; }

    [Display(Name = "Statut", Order = 0)]
    public string Statut => CarteActuelle ? "Carte actuelle bloquée" : "Ancienne carte bloquée";

    [Display(Name = "Malade", Order = 1)] public string? Malade { get; set; }
    [Display(Name = "N° Assuré", Order = 2)] public string? NumAssure { get; set; }
    [Display(Name = "Rang", Order = 3)] public string? Rang { get; set; }
    [Display(Name = "N° série carte", Order = 4)] public long NumSerie { get; set; }
    [Display(Name = "Factures avec cette carte", Order = 5)] public int Factures { get; set; }
    [Display(Name = "Dernière utilisation", Order = 6)] public DateTime? DerniereUtilisation { get; set; }
    [Display(Name = "Dernière visite", Order = 7)] public DateTime? DerniereVisite { get; set; }
}

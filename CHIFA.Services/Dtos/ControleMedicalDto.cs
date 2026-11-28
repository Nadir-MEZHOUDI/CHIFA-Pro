using System.ComponentModel.DataAnnotations;

namespace CHIFA.Services.Dtos;

public enum CmStatut
{
    EnAttente,
    Accordee,
    Signee,
    RejetPartiel,
    RejetGlobal,
    Convoque,
    Traitee
}

/// <summary>An ordonnance sent to the CNAS contrôle médical (facture_cm), decoded as CHIFA_OFFICINE shows it.</summary>
public class ControleMedicalDto
{
    [Display(AutoGenerateField = false)] public char? Etat { get; set; }
    [Display(AutoGenerateField = false)] public int? DecisionCm { get; set; }

    [Display(AutoGenerateField = false)] public CmStatut Statut => Decode(Etat, DecisionCm);
    [Display(Name = "Statut", Order = 0)] public string StatutText => Label(Etat, DecisionCm);

    [Display(Name = "Facture", Order = 1)] public string NumFact { get; set; } = "";
    [Display(Name = "Date facture", Order = 2)] public DateTime? DateFact { get; set; }
    [Display(Name = "Date soin", Order = 3)] public DateTime? DateSoin { get; set; }
    [Display(Name = "Date CM", Order = 4)] public DateTime? DateCm { get; set; }
    [Display(Name = "Malade", Order = 5)] public string? Malade { get; set; }
    [Display(Name = "N° Assuré", Order = 6)] public string? NumAssure { get; set; }
    [Display(Name = "Rang", Order = 7)] public string? Rang { get; set; }
    [Display(Name = "Centre", Order = 8)] public string? Centre { get; set; }
    [Display(Name = "Bordereau", Order = 9)] public string? NumBord { get; set; }
    [Display(Name = "Montant", Order = 10)] public decimal? MontFact { get; set; }
    [Display(Name = "Part CNAS", Order = 11)] public decimal? MontOff { get; set; }
    [Display(Name = "Motif CM", Order = 12)] public string? MotifCm { get; set; }

    [Display(Name = "Médicaments")] public List<ControleMedicalLigneDto> Lignes { get; set; } = [];

    /// <summary>
    /// C = sent and waiting; T = answered (1 accordée, 2 rejet partiel, 3 rejet global, 4 convoqué);
    /// S = signed by the pharmacist after an accord, partial or not.
    /// </summary>
    public static CmStatut Decode(char? etat, int? decision) => (etat, decision) switch
    {
        ('C', _) => CmStatut.EnAttente,
        ('S', 2) or ('T', 2) => CmStatut.RejetPartiel,
        ('S', _) => CmStatut.Signee,
        ('T', 1) => CmStatut.Accordee,
        ('T', 3) => CmStatut.RejetGlobal,
        ('T', 4) => CmStatut.Convoque,
        _ => CmStatut.Traitee
    };

    public static string Label(char? etat, int? decision) => (etat, Decode(etat, decision)) switch
    {
        (_, CmStatut.EnAttente) => "Envoyée — en attente",
        ('S', CmStatut.RejetPartiel) => "Signée — rejet partiel",
        (_, CmStatut.RejetPartiel) => "Rejet partiel — à signer",
        (_, CmStatut.Signee) => "Signée — accordée",
        (_, CmStatut.Accordee) => "Accordée — à signer",
        (_, CmStatut.RejetGlobal) => "Rejet global",
        (_, CmStatut.Convoque) => "Malade convoqué au CM",
        _ => "Traitée"
    };
}

public class ControleMedicalLigneDto
{
    [Display(AutoGenerateField = false)] public string NumFact { get; set; } = "";
    [Display(Name = "Décision", Order = 0)] public string Decision => (QteAccord, QteRej) switch
    {
        (0, > 0) => "Rejeté",
        (> 0, > 0) => "Partiel",
        (> 0, _) => "Accordé",
        _ => "—"
    };
    [Display(Name = "Médicament", Order = 1)] public string? Medicament { get; set; }
    [Display(Name = "Durée", Order = 2)] public decimal? Duree { get; set; }
    [Display(Name = "Qté prescrite", Order = 3)] public decimal Qte { get; set; }
    [Display(Name = "Qté accordée", Order = 4)] public decimal? QteAccord { get; set; }
    [Display(Name = "Qté rejetée", Order = 5)] public decimal? QteRej { get; set; }
    [Display(Name = "PPA", Order = 6)] public decimal Ppa { get; set; }
    [Display(Name = "Montant", Order = 7)] public decimal Mont { get; set; }
}

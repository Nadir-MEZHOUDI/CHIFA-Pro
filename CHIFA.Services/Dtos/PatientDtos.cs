namespace CHIFA.Services.Dtos;

public class PatientRowDto
{
    public string NumAssure { get; set; } = "";
    public string Rang { get; set; } = "";
    public string? Nom { get; set; }

    /// <summary>The insured person, when the patient is a dependant (rang other than 00).</summary>
    public string? Assure { get; set; }

    public DateTime? DerniereVisite { get; set; }
    public int NombreFactures { get; set; }
}

public class PatientDetailDto
{
    public string NumAssure { get; set; } = "";
    public string Rang { get; set; } = "";
    public string? Nom { get; set; }
    public string? Assure { get; set; }
    public string? Centre { get; set; }

    /// <summary>End of the patient's CNAS rights, from the last facture.</summary>
    public DateTime? DateFinDroit { get; set; }

    public DateTime? PremiereVisite { get; set; }
    public DateTime? DerniereVisite { get; set; }
    public int NombreFactures { get; set; }
    public decimal MontantTotal { get; set; }

    /// <summary>The insured person and every dependant on the same card.</summary>
    public List<FamilyMemberDto> Famille { get; set; } = [];

    /// <summary>One line per molecule (DCI), latest purchase first.</summary>
    public List<TreatmentDto> Traitements { get; set; } = [];

    public List<FactureDto> Factures { get; set; } = [];
}

public class FamilyMemberDto
{
    public string Rang { get; set; } = "";
    public string? Nom { get; set; }
}

public class TreatmentDto
{
    public string? NumEnr { get; set; }
    public string? Medicament { get; set; }
    public DateTime? DerniereDate { get; set; }
    public decimal? Quantite { get; set; }
    public decimal? Duree { get; set; }

    /// <summary>When the last box runs out: last date + treatment days. Null without a duration.</summary>
    public DateTime? Prochain { get; set; }

    public bool Ts { get; set; }
    public string? Medecin { get; set; }

    /// <summary>How many times it was dispensed.</summary>
    public int Delivrances { get; set; }
}

public class MedicRowDto
{
    public string NumEnr { get; set; } = "";
    public string? Nom { get; set; }
    public string? Dci { get; set; }
    public string? Laboratoire { get; set; }
    public decimal Quantite { get; set; }
    public decimal Montant { get; set; }
    public int Factures { get; set; }
    public int Patients { get; set; }
}

public class MedicDetailDto
{
    public string NumEnr { get; set; } = "";
    public string? Nom { get; set; }
    public string? Dci { get; set; }
    public string? Laboratoire { get; set; }
    public string? Forme { get; set; }
    public string? Pays { get; set; }
    public decimal? TarifRef { get; set; }
    public bool Generique { get; set; }
    public bool Psychotrope { get; set; }
    public decimal Quantite { get; set; }
    public decimal Montant { get; set; }
    public int Factures { get; set; }

    /// <summary>Boxes per month over the period, oldest first.</summary>
    public List<MonthQuantityDto> ParMois { get; set; } = [];

    /// <summary>Who took it in the period, most recent first.</summary>
    public List<MedicPatientDto> Patients { get; set; } = [];
    public int NombrePatients { get; set; }
}

public class MonthQuantityDto
{
    public int Annee { get; set; }
    public int Mois { get; set; }
    public decimal Quantite { get; set; }
}

public class MedicPatientDto
{
    public string NumAssure { get; set; } = "";
    public string Rang { get; set; } = "";
    public string? Nom { get; set; }
    public decimal Quantite { get; set; }
    public int Factures { get; set; }
    public DateTime? DerniereDate { get; set; }
}

public class FactureFullDto
{
    public string NumFact { get; set; } = "";
    public DateTime? DateFact { get; set; }
    public DateTime? DateSoin { get; set; }
    public string? NumAssure { get; set; }
    public string? Rang { get; set; }
    public string? Malade { get; set; }
    public string? Assure { get; set; }
    public string? Centre { get; set; }
    public string? Specialite { get; set; }
    public string? Prescripteur { get; set; }
    public string? Bordereau { get; set; }
    public decimal? MontFact { get; set; }
    public decimal? MontOff { get; set; }
    public decimal? MontAss { get; set; }
    public decimal? Majoration { get; set; }
    public List<FactureDetailDto> Lignes { get; set; } = [];
}

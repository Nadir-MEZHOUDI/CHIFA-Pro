namespace CHIFA.Mobile;

// Shapes of the gateway JSON (the pharmacy agent's DTOs), limited to what the phone shows.

public record LoginResponse(string AccessToken, DateTime ExpiresAt, string Pharmacy);

public record OfficineStatus(string Name, bool Online, DateTime? LastSeenAt);

public record AgentInfo(string? Officine, string AgentVersion, DateTime ServerTime);

public record Page<T>(int Total, List<T> Items);

public class Dashboard
{
    public decimal MontantTotalFact { get; init; }
    public decimal MontantTotalCaisse { get; init; }
    public decimal MontantTotalAssure { get; init; }
    public decimal MontantTotalMajoration { get; init; }
    public int NombreFactures { get; init; }
    public int NombreAssuresUniques { get; init; }
    public decimal NombreBoites { get; init; }
    public decimal PanierMoyen { get; init; }
    public decimal TauxCouvertureCaisse { get; init; }
    public List<HourlyActivity> ActiviteHoraire { get; init; } = [];
    public List<CenterSummary> RepartitionParCaisse { get; init; } = [];
    public List<TopProduct> TopMedicaments { get; init; } = [];
}

public class HourlyActivity
{
    public int Heure { get; init; }
    public int NombreFactures { get; init; }
    public decimal MontantTotal { get; init; }
}

public class CenterSummary
{
    public string Centre { get; init; } = "";
    public int NombreFactures { get; init; }
    public decimal MontantTotal { get; init; }
    public decimal MontantCaisse { get; init; }
}

public class TopProduct
{
    public string Designation { get; init; } = "";
    public string Dci { get; init; } = "";
    public decimal QuantiteTotale { get; init; }
    public decimal MontantTotal { get; init; }
}

public class Facture
{
    public string? NumFact { get; init; }
    public DateTime? DateFact { get; init; }
    public string? Malade { get; init; }
    public string? Assure { get; init; }
    public decimal? MontFact { get; init; }
    public decimal? MontOff { get; init; }
    public decimal? MontAss { get; init; }
    public decimal? Majoration { get; init; }
    public string? Centre { get; init; }
    public string? Specialite { get; init; }
    public bool? Ts { get; init; }
    public bool LongDuree { get; init; }
}

public class Bordereau
{
    public string? Num { get; init; }
    public string? Center { get; init; }
    public DateTime? FirstFacture { get; init; }
    public DateTime? LastFacture { get; init; }
    public int? Nmbr { get; init; }
    public decimal? Virment { get; init; }
    public char? State { get; init; }
    public decimal? MontOff { get; init; }
    public decimal? Maj { get; init; }
    public decimal? MontGlobal { get; init; }
    public DateTime? DateDepotFtp { get; init; }

    public bool EstOuvert => State == 'O';

    /// <summary>CHIFA writes 01/01/1900 when the bordereau was never deposited.</summary>
    public DateTime? Depot => DateDepotFtp is { Year: > 2000 } d ? d : null;
}

public class Rejet
{
    public string? NumBord { get; init; }
    public string? Centre { get; init; }
    public DateTime? DateBord { get; init; }
    public int NombreFactures { get; init; }
    public decimal MontantFacture { get; init; }
    public decimal MontantVirement { get; init; }
    public decimal MontantRejete { get; init; }
    public decimal MontantEnAttente { get; init; }
    public decimal TauxRejet { get; init; }
    public bool EstOuvert { get; init; }
    public bool EstPaye { get; init; }
    public string? StatutRejet { get; init; }
}

public class Psychotrope
{
    public string? NumFact { get; init; }
    public DateTime? DateDelivrance { get; init; }
    public string? NomBeneficiaire { get; init; }
    public string? NomAssure { get; init; }
    public string? Medecin { get; init; }
    public string? SpecialiteMedecin { get; init; }
    public string? MedicamentNom { get; init; }
    public string? Dosage { get; init; }
    public string? Forme { get; init; }
    public decimal? Quantite { get; init; }
    public string? CategoriePsychotrope { get; init; }
    public string? Tableau { get; init; }
    public string? NumeroOrdonnance { get; init; }
}

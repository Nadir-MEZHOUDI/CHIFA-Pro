namespace CHIFA.Mobile;

// Shapes of the gateway JSON (the pharmacy agent's DTOs), limited to what the phone shows.

/// <param name="DeviceToken">Linked phones only: signs in again without a code until logout.</param>
public record LoginResponse(string AccessToken, DateTime ExpiresAt, string Pharmacy, string? DeviceToken = null);

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
    public string? NumAssure { get; init; }
    public string? Rang { get; init; }
    public string? Bordereau { get; init; }
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
    public decimal? MontAss { get; init; }
    public DateTime? DateDepotFtp { get; init; }
    public DateTime? DateOuverture { get; init; }
    public DateTime? DateCloture { get; init; }

    /// <summary>False for CASNOS: CHIFA never receives its virements. An agent older than the flag sends nothing: true.</summary>
    public bool PaiementSuivi { get; init; } = true;

    public bool EstOuvert => State == 'O';

    /// <summary>CHIFA writes 01/01/1900 when the bordereau was never deposited.</summary>
    public DateTime? Depot => DateDepotFtp is { Year: > 2000 } d ? d : null;

    public static string UrlOf(string num) => $"bordereaux/{Uri.EscapeDataString(num)}";
}

public class BordereauDetail
{
    public Bordereau Bordereau { get; init; } = new();
    public List<Facture> Factures { get; init; } = [];
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

public class FactureFull
{
    public string NumFact { get; init; } = "";
    public DateTime? DateFact { get; init; }
    public DateTime? DateSoin { get; init; }
    public string? NumAssure { get; init; }
    public string? Rang { get; init; }
    public string? Malade { get; init; }
    public string? Assure { get; init; }
    public string? Centre { get; init; }
    public string? Specialite { get; init; }
    public string? Prescripteur { get; init; }
    public string? Bordereau { get; init; }
    public decimal? MontFact { get; init; }
    public decimal? MontOff { get; init; }
    public decimal? MontAss { get; init; }
    public decimal? Majoration { get; init; }
    public int? Taux { get; init; }
    public bool? Mc { get; init; }
    public List<FactureLigne> Lignes { get; init; } = [];
}

public class FactureLigne
{
    public string? Code { get; init; }
    public string Medicament { get; init; } = "";
    public decimal? Qt { get; init; }
    public decimal? Ppa { get; init; }
    public decimal? TR { get; init; }
    public decimal? DureeTrait { get; init; }
    public decimal? MontAss { get; init; }
    public decimal? MontPharm { get; init; }
    public bool? Ts { get; init; }
}

public class PatientRow
{
    public string NumAssure { get; init; } = "";
    public string Rang { get; init; } = "";
    public string? Nom { get; init; }
    public string? Assure { get; init; }
    public DateTime? DerniereVisite { get; init; }
    public int NombreFactures { get; init; }

    /// <summary>80 or 100 (%), from the last facture. Null when the agent is older than this field.</summary>
    public int? Taux { get; init; }
    public bool? TauxVarie { get; init; }
    public bool? Mc { get; init; }
}

public class PatientDetail
{
    public string NumAssure { get; init; } = "";
    public string Rang { get; init; } = "";
    public string? Nom { get; init; }
    public string? Assure { get; init; }
    public string? Centre { get; init; }
    public DateTime? DateFinDroit { get; init; }
    public int? Taux { get; init; }
    public bool? TauxVarie { get; init; }
    public bool? Mc { get; init; }
    public DateTime? PremiereVisite { get; init; }
    public DateTime? DerniereVisite { get; init; }
    public int NombreFactures { get; init; }
    public decimal MontantTotal { get; init; }
    public List<FamilyMember> Famille { get; init; } = [];
    public List<Treatment> Traitements { get; init; } = [];
    public List<Facture> Factures { get; init; } = [];
}

public class FamilyMember
{
    public string Rang { get; init; } = "";
    public string? Nom { get; init; }
}

public class Treatment
{
    public string? NumEnr { get; init; }
    public string? Medicament { get; init; }
    public DateTime? DerniereDate { get; init; }
    public decimal? Quantite { get; init; }
    public decimal? Duree { get; init; }
    public DateTime? Prochain { get; init; }
    public bool Ts { get; init; }
    public string? Medecin { get; init; }
    public int Delivrances { get; init; }
}

/// <summary>A chronic patient whose treatment runs out soon (or already did).</summary>
public class Reminder
{
    public string? NumAssure { get; init; }
    public string? Rang { get; init; }
    public string? NomAssure { get; init; }
    public string? NomBeneficiaire { get; init; }
    public DateTime? DateDerniereVisite { get; init; }
    public DateTime? DatePrevisionnelleRetour { get; init; }
    public int JoursRestants { get; init; }
    public string? TraitementHabituelResume { get; init; }
    public decimal MontantMoyenHabituel { get; init; }
}

public class MedicRow
{
    public string NumEnr { get; init; } = "";
    public string? Nom { get; init; }
    public string? Dci { get; init; }
    public string? Laboratoire { get; init; }
    public decimal Quantite { get; init; }
    public decimal Montant { get; init; }
    public int Factures { get; init; }
    public int Patients { get; init; }
}

public class MedicDetail
{
    public string NumEnr { get; init; } = "";
    public string? Nom { get; init; }
    public string? Dci { get; init; }
    public string? Laboratoire { get; init; }
    public string? Forme { get; init; }
    public string? Pays { get; init; }
    public decimal? TarifRef { get; init; }
    public bool Generique { get; init; }
    public bool Psychotrope { get; init; }
    public decimal Quantite { get; init; }
    public decimal Montant { get; init; }
    public int Factures { get; init; }
    public List<MonthQuantity> ParMois { get; init; } = [];
    public List<MedicPatient> Patients { get; init; } = [];
    public int NombrePatients { get; init; }
}

public class MonthQuantity
{
    public int Annee { get; init; }
    public int Mois { get; init; }
    public decimal Quantite { get; init; }
}

public class MedicPatient
{
    public string NumAssure { get; init; } = "";
    public string Rang { get; init; } = "";
    public string? Nom { get; init; }
    public decimal Quantite { get; init; }
    public int Factures { get; init; }
    public DateTime? DerniereDate { get; init; }
}

/// <summary>The browser's push subscription, as <c>chifa.push</c> returns it.</summary>
public record PushSubscriptionInfo(string Endpoint, string P256dh, string Auth);

public record PushKey(string PublicKey);

/// <param name="Status"><c>unknown</c>, <c>active</c>, <c>trial</c>, <c>expiring</c> or <c>expired</c>.</param>
/// <param name="CheckedAt">When SmartCHIFA last sent its license to the server.</param>
public record Subscription(string Status, bool IsTrial, DateTime? ExpiresAt, int? DaysLeft, DateTime? CheckedAt);

/// <param name="IsCurrent">This phone.</param>
public record PhoneDevice(Guid Id, string Name, DateTime CreatedAt, DateTime? LastSeenAt, bool IsCurrent);

/// <param name="Code">Shown as <c>ABCD-2345</c>, like in SmartCHIFA. One use, a few minutes.</param>
public record LinkCode(string Code, DateTime ExpiresAt, bool AgentOnline);

/// <summary>One day of the home chart (today first). <see cref="Marge"/> is estimated like on the desktop home page.</summary>
public record DayStat(DateTime Date, int Factures, decimal Montant, decimal Maj, decimal Marge);

/// <summary>A row of a statistics tab: a day, week, month, year, bordereau, medicine or patient.</summary>
public class StatRow
{
    public string Label { get; init; } = "";
    public string? Short { get; init; }
    public string? Detail { get; init; }
    public DateTime? Date { get; init; }
    public string? Key { get; init; }
    public string? Rang { get; init; }
    public int Factures { get; init; }
    public decimal Quantite { get; init; }
    public decimal Montant { get; init; }
    public decimal Caisse { get; init; }
    public decimal Maj { get; init; }
    public decimal Marge { get; init; }
    public decimal Brut { get; init; }
    public decimal? Virement { get; init; }
    public decimal? Ecart { get; init; }
    public bool PaiementSuivi { get; init; } = true;
}

public record Stats(string Kind, int Count, List<StatRow> Rows, StatRow Totals);

/// <summary>Watch rules kept on the pharmacy computer: each new facture that matches one is pushed to the phones.</summary>
public class Watchlist
{
    public bool Psychotropes { get; set; }
    public decimal? MontantMin { get; set; }
    public int? BoitesMin { get; set; }
    public List<WatchedPatient> Patients { get; set; } = [];
    public List<WatchedMedicament> Medicaments { get; set; } = [];
}

public record WatchedPatient(string NumAssure, string Rang, string? Nom);

public record WatchedMedicament(string NumEnr, string? Nom);

/// <param name="CanReview">Paid subscription: trials and expired pharmacies cannot review.</param>
/// <param name="Sent">The pharmacy already gave its review.</param>
public record ReviewState(bool CanReview, bool Sent, string SuggestedName);

public record ReviewRequest(int Stars, string Comment, string AuthorName, string? City);

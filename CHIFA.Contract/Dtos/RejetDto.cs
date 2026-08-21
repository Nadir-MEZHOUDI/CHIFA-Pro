namespace CHIFA.Contract.Dtos;

public class RejetDto
{
    public string? NumBord { get; set; }
    public string? Centre { get; set; }
    public DateTime? DateBord { get; set; }
    public DateTime? DateVirement { get; set; }
    public int NombreFactures { get; set; }
    public decimal MontantFacture { get; set; }
    public decimal MontantVirement { get; set; }
    public decimal MontantRejete => Math.Max(0, MontantFacture - MontantVirement);
    public decimal TauxRejet => MontantFacture > 0 ? (MontantRejete / MontantFacture) * 100m : 0m;
    public string StatutRejet => MontantRejete == 0 ? "Sans Rejet" : (MontantVirement == 0 ? "Non Payé / Rejet Total" : "Rejet Partiel");
    public string MotifPrincipal { get; set; } = "Écart de virement bordereau";
    public string EtatBordereau { get; set; } = string.Empty;
}

public class RejetDetailDto
{
    public string? NumFact { get; set; }
    public string? NumAssure { get; set; }
    public string? NomAssure { get; set; }
    public DateTime? DateFact { get; set; }
    public decimal MontantFact { get; set; }
    public decimal MontantCaisse { get; set; }
    public string? MotifRejet { get; set; }
    public string Statut { get; set; } = "En Cours";
}

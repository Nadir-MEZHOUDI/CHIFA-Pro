namespace CHIFA.Contract.Dtos;

public class ScopeDashboardDto
{
    public decimal MontantTotalFact { get; set; }
    public decimal MontantTotalCaisse { get; set; }
    public decimal MontantTotalAssure { get; set; }
    public decimal MontantTotalMajoration { get; set; }
    public int NombreFactures { get; set; }
    public int NombreAssuresUniques { get; set; }
    public decimal NombreBoites { get; set; }
    public decimal PanierMoyen => NombreFactures > 0 ? MontantTotalFact / NombreFactures : 0m;
    public decimal TauxCouvertureCaisse => MontantTotalFact > 0 ? (MontantTotalCaisse / MontantTotalFact) * 100m : 0m;

    public List<HourlyActivityDto> ActiviteHoraire { get; set; } = [];
    public List<CenterSummaryDto> RepartitionParCaisse { get; set; } = [];
    public List<TopProductDto> TopMedicaments { get; set; } = [];
}

public class HourlyActivityDto
{
    public int Heure { get; set; }
    public string TrancheHoraire => $"{Heure:00}h - {Heure + 1:00}h";
    public int NombreFactures { get; set; }
    public decimal MontantTotal { get; set; }
}

public class CenterSummaryDto
{
    public string Centre { get; set; } = string.Empty;
    public int NombreFactures { get; set; }
    public decimal MontantTotal { get; set; }
    public decimal MontantCaisse { get; set; }
    public decimal MontantMaj { get; set; }
    public decimal MontantAssure => MontantTotal - MontantCaisse;
}

public class TopProductDto
{
    public string CodeEnr { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string Dci { get; set; } = string.Empty;
    public decimal QuantiteTotale { get; set; }
    public decimal MontantTotal { get; set; }
}

namespace CHIFA.Contract.Dtos;

public enum AnomalySeverity
{
    Information,
    Avertissement,
    Critique
}

public class AuditAnomalyDto
{
    public string? NumFact { get; set; }
    public string? NumBord { get; set; }
    public DateTime? DateFact { get; set; }
    public string? NumAssure { get; set; }
    public string? NomAssure { get; set; }
    public string? NomProduit { get; set; }
    public string TypeAnomalie { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public AnomalySeverity Severite { get; set; } = AnomalySeverity.Avertissement;
    public string ActionRecommandee { get; set; } = string.Empty;
    public decimal? MontantImpacte { get; set; }
}

public class AuditReportDto
{
    public int TotalFacturesAuditees { get; set; }
    public int FacturesConformes { get; set; }
    public int FacturesAvecAnomalies { get; set; }
    public int TotalAnomaliesCritiques { get; set; }
    public int TotalAvertissements { get; set; }
    public decimal MontantRisqueRejet { get; set; }
    public List<AuditAnomalyDto> Anomalies { get; set; } = [];
}

namespace CHIFA.Services.Dtos;

public class ForecastRefillDto
{
    public string? NumAssure { get; set; }
    public string? Rang { get; set; }
    public string? NomAssure { get; set; }
    public string? NomBeneficiaire { get; set; }
    public DateTime? DateDerniereVisite { get; set; }
    public int DureeDernierTraitement { get; set; }
    public DateTime? DatePrevisionnelleRetour { get; set; }
    public int JoursRestants => DatePrevisionnelleRetour.HasValue 
        ? (int)(DatePrevisionnelleRetour.Value.Date - DateTime.Today).TotalDays 
        : 0;

    public string StatutObservance => JoursRestants switch
    {
        < 0 => $"En retard ({-JoursRestants} j)",
        0 => "Attendu aujourd'hui",
        <= 3 => "Très imminent (≤ 3j)",
        <= 7 => "Cette semaine",
        _ => "Prochainement"
    };

    public decimal MontantMoyenHabituel { get; set; }
    public int NombreTraitementsPrevus { get; set; }
    public string? TraitementHabituelResume { get; set; }
    public string? MedecinHabituel { get; set; }
}

public class ForecastSummaryDto
{
    public int TotalPatientsAttendus { get; set; }
    public int PatientsEnRetard { get; set; }
    public int PatientsCetteSemaine { get; set; }
    public decimal EstimationCaPrevisionnel { get; set; }
    public List<ForecastRefillDto> Details { get; set; } = [];
}

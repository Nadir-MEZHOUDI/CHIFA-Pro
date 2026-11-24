using System.Globalization;
using System.Text.Json;
using CHIFA.Contract;
using CHIFA.Services.DataServices;
using CHIFA.Services.Dtos;

namespace CHIFA.Agent;

/// <summary>
/// Finds what is new since the gateway last asked: bordereaux paid by the CNAS (with their rejected amount)
/// and, once a day, the chronic patients to remind. What was already reported is kept in a file shared by
/// the Windows service and SmartCHIFA, so a restart or a switch between them never repeats an alert.
/// </summary>
public class AlertTracker(ILogger logger)
{
    /// <summary>Above this, the paid bordereaux are reported as one summary.</summary>
    public const int MaxDetailedPayments = 4;

    /// <summary>The daily chronic reminder is not sent before this hour.</summary>
    public const int ChronicReminderHour = 8;

    private static readonly CultureInfo French = CultureInfo.GetCultureInfo("fr-FR");
    private readonly SemaphoreSlim _lock = new(1, 1);
    private AlertState? _state;

    public static string StatePath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SmartCHIFA", "alerts.json");

    public async Task<List<AgentAlert>> GetNewAlertsAsync()
    {
        await _lock.WaitAsync();
        try
        {
            var state = _state ??= await LoadAsync();
            var rejets = await ScopeService.Instance.GetRejetsAsync(new Period { From = DateTime.Today.AddMonths(-6) });
            var forecast = DateTime.Now.Hour >= ChronicReminderHour && state.LastChronicDay != DateTime.Today
                ? await ScopeService.Instance.GetChronicForecastsAsync()
                : null;

            var alerts = Compute(state, rejets, forecast, DateTime.Now);
            await SaveAsync(state);
            return alerts;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Pure part: updates <paramref name="state"/> and returns the alerts. On the first run it only records
    /// what is already paid, so installing the agent does not report months of old payments.
    /// </summary>
    public static List<AgentAlert> Compute(AlertState state, IEnumerable<RejetDto> rejets, ForecastSummaryDto? forecast, DateTime now)
    {
        var alerts = new List<AgentAlert>();
        var paid = rejets.Where(r => r is { EstOuvert: false, EstPaye: true } && !string.IsNullOrEmpty(r.NumBord)).ToList();
        var newlyPaid = paid.Where(r => !state.PaidBordereaux.Contains(r.NumBord!)).OrderBy(r => r.DateBord).ToList();

        if (state.Initialized && newlyPaid.Count > MaxDetailedPayments)
        {
            var rejete = newlyPaid.Sum(r => r.MontantRejete);
            alerts.Add(new AgentAlert(
                $"paid:{string.Join(',', newlyPaid.Select(r => r.NumBord))}",
                $"{newlyPaid.Count} bordereaux payés",
                $"Virements : {Money(newlyPaid.Sum(r => r.MontantVirement))}" + (rejete > 0 ? $" · rejeté : {Money(rejete)}" : ""),
                "rejets"));
        }
        else if (state.Initialized)
        {
            alerts.AddRange(newlyPaid.Select(r => new AgentAlert(
                $"paid:{r.NumBord}",
                r.MontantRejete > 0 ? $"Rejet sur le bordereau N° {r.NumBord}" : $"Bordereau N° {r.NumBord} payé",
                r.MontantRejete > 0
                    ? $"{Money(r.MontantRejete)} rejetés ({r.TauxRejet.ToString("N1", French)} %) · viré {Money(r.MontantVirement)} · {r.Centre}"
                    : $"Virement de {Money(r.MontantVirement)} · {r.Centre}",
                "rejets")));
        }

        // Only the last six months are checked: keeping older numbers would only grow the file.
        state.PaidBordereaux = paid.Select(r => r.NumBord!).ToHashSet();
        state.Initialized = true;

        if (forecast is not null)
        {
            state.LastChronicDay = now.Date;
            var late = forecast.Details.Count(f => f.JoursRestants < 0);
            var today = forecast.Details.Count(f => f.JoursRestants == 0);
            if (late + today > 0)
            {
                var parts = new List<string>();
                if (late > 0) parts.Add($"{late} en retard");
                if (today > 0) parts.Add($"{today} attendu{(today > 1 ? "s" : "")} aujourd'hui");
                alerts.Add(new AgentAlert(
                    $"chronic:{now:yyyy-MM-dd}",
                    "Malades chroniques à relancer",
                    string.Join(" · ", parts) + $" · {forecast.PatientsCetteSemaine} cette semaine",
                    "patients?relance=1"));
            }
        }

        return alerts;
    }

    private static string Money(decimal value) => $"{value.ToString("N0", French)} DA";

    private async Task<AlertState> LoadAsync()
    {
        try
        {
            await using var stream = File.OpenRead(StatePath);
            return await JsonSerializer.DeserializeAsync<AlertState>(stream) ?? new AlertState();
        }
        catch (FileNotFoundException)
        {
            return new AlertState();
        }
        catch (DirectoryNotFoundException)
        {
            return new AlertState();
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Unreadable {Path}, starting over", StatePath);
            return new AlertState();
        }
    }

    /// <summary>Best effort: the state stays in memory, the file only matters across restarts.</summary>
    private async Task SaveAsync(AlertState state)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(StatePath)!);
            var temp = StatePath + ".tmp";
            await File.WriteAllTextAsync(temp, JsonSerializer.Serialize(state));
            File.Move(temp, StatePath, overwrite: true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            logger.LogWarning(ex, "Could not save {Path}", StatePath);
        }
    }
}

public class AlertState
{
    public bool Initialized { get; set; }
    public HashSet<string> PaidBordereaux { get; set; } = [];
    public DateTime? LastChronicDay { get; set; }
}

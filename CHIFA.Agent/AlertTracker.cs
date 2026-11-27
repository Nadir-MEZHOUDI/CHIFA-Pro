using System.Globalization;
using System.Text.Json;
using CHIFA.Contract;
using CHIFA.Services.DataServices;
using CHIFA.Services.Dtos;

namespace CHIFA.Agent;

/// <summary>
/// Finds what is new since the gateway last asked: bordereaux paid by the CNAS (with their rejected amount),
/// once a day the chronic patients to remind, and the new factures that match the watch rules. What was
/// already reported is kept in a file shared by the Windows service and SmartCHIFA, so a restart or a
/// switch between them never repeats an alert.
/// </summary>
public class AlertTracker(ILogger logger, WatchlistStore watchlist)
{
    /// <summary>Above this, the paid bordereaux are reported as one summary.</summary>
    public const int MaxDetailedPayments = 4;

    /// <summary>The daily chronic reminder is not sent before this hour.</summary>
    public const int ChronicReminderHour = 8;

    /// <summary>Above this, the watched factures of one poll are reported as one summary.</summary>
    public const int MaxDetailedWatch = 5;

    /// <summary>
    /// New factures are looked for from this long before the last one seen: a second computer whose clock
    /// is a little behind, or a facture saved late, is not missed.
    /// </summary>
    public static readonly TimeSpan FactureOverlap = TimeSpan.FromHours(2);

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

            var now = DateTime.Now;
            var alerts = Compute(state, rejets, forecast, now);

            var stamps = await ScopeService.Instance.GetFactureStampsSinceAsync(FacturesSince(state, now));
            var fresh = TrackFactures(state, stamps, now);
            var rules = await watchlist.LoadAsync();
            if (fresh.Count > 0 && !rules.IsEmpty)
                alerts.AddRange(ComputeWatch(rules, await ScopeService.Instance.GetWatchedFacturesAsync(fresh)));

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

    public static DateTime FacturesSince(AlertState state, DateTime now) => (state.LastFactureDate ?? now) - FactureOverlap;

    /// <summary>
    /// Remembers the factures seen and returns the numbers of those not seen before. The first run, and
    /// factures more than a day old (the computer was off), are only remembered.
    /// </summary>
    public static List<string> TrackFactures(AlertState state, IReadOnlyCollection<(string NumFact, DateTime DateFact)> stamps, DateTime now)
    {
        var fresh = stamps.Where(s => !state.SeenFactures.ContainsKey(s.NumFact)).ToList();
        foreach (var (num, date) in stamps)
            state.SeenFactures[num] = date;

        if (stamps.Count > 0)
        {
            var last = stamps.Max(s => s.DateFact);
            if (state.LastFactureDate is null || last > state.LastFactureDate)
                state.LastFactureDate = last;
        }

        // What is older than the next query's start can no longer come back: no need to keep it.
        var since = FacturesSince(state, now);
        foreach (var old in state.SeenFactures.Where(s => s.Value < since).Select(s => s.Key).ToList())
            state.SeenFactures.Remove(old);

        var initialized = state.FacturesInitialized;
        state.FacturesInitialized = true;
        return initialized
            ? fresh.Where(s => s.DateFact >= now.AddDays(-1)).Select(s => s.NumFact).ToList()
            : [];
    }

    public static List<AgentAlert> ComputeWatch(Watchlist rules, IEnumerable<WatchedFactureDto> factures)
    {
        var patients = rules.Patients.Select(p => (p.NumAssure, p.Rang)).ToHashSet();
        var medicaments = rules.Medicaments.Select(m => m.NumEnr).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var matches = new List<(WatchedFactureDto Facture, WatchMatch Match)>();

        foreach (var f in factures)
        {
            var match = new WatchMatch(
                f.NumAssure is { } num && f.Rang is { } rang && patients.Contains((num.Trim(), rang.Trim())),
                f.Lignes.Where(l => medicaments.Contains(l.NumEnr.Trim())).Select(Name).Distinct().ToList(),
                rules.Psychotropes ? f.Lignes.Where(l => l.Psychotrope).Select(Name).Distinct().ToList() : [],
                rules.MontantMin is { } min && f.MontFact >= min,
                rules.BoitesMin is { } boites && f.Boites >= boites);
            if (match.Any)
                matches.Add((f, match));
        }

        if (matches.Count > MaxDetailedWatch)
        {
            var parts = new List<string>();
            AddCount(parts, matches.Count(m => m.Match.Patient), "patient suivi", "patients suivis");
            AddCount(parts, matches.Count(m => m.Match.Medicaments.Count > 0), "médicament suivi", "médicaments suivis");
            AddCount(parts, matches.Count(m => m.Match.Psychotropes.Count > 0), "psychotrope", "psychotropes");
            AddCount(parts, matches.Count(m => m.Match.Montant || m.Match.Boites), "grosse ordonnance", "grosses ordonnances");
            return
            [
                new AgentAlert(
                    $"watch:{matches[0].Facture.NumFact}-{matches[^1].Facture.NumFact}:{matches.Count}",
                    $"{matches.Count} ordonnances signalées",
                    string.Join(" · ", parts),
                    "factures")
            ];
        }

        return matches.Select(m => ToAlert(m.Facture, m.Match)).ToList();
    }

    private static AgentAlert ToAlert(WatchedFactureDto f, WatchMatch m)
    {
        var malade = string.IsNullOrWhiteSpace(f.Malade) ? f.NumAssure ?? "Patient" : f.Malade.Trim();
        var boites = $"{f.Boites.ToString("0.##", French)} boîte{(f.Boites > 1 ? "s" : "")}";
        var title = m.Patient ? $"Patient suivi : {malade}"
            : m.Medicaments.Count > 0 ? "Médicament suivi vendu"
            : m.Psychotropes.Count > 0 ? "Psychotrope délivré"
            : m.Montant ? $"Ordonnance de {Money(f.MontFact)}"
            : $"Ordonnance de {boites}";

        var parts = new List<string>();
        if (!m.Patient)
            parts.Add(malade);
        if (m.Medicaments.Count > 0)
            parts.Add(string.Join(", ", m.Medicaments));
        var psychotropes = m.Psychotropes.Except(m.Medicaments).ToList();
        if (psychotropes.Count > 0)
            parts.Add((m.Medicaments.Count > 0 || m.Patient ? "Psychotrope : " : "") + string.Join(", ", psychotropes));
        parts.Add(Money(f.MontFact));
        parts.Add(boites);

        return new AgentAlert($"watch:{f.NumFact}", title, string.Join(" · ", parts), $"factures/{Uri.EscapeDataString(f.NumFact)}");
    }

    private static string Name(WatchedLigneDto l) => string.IsNullOrWhiteSpace(l.Nom) ? l.NumEnr : l.Nom.Trim();

    private static void AddCount(List<string> parts, int count, string one, string many)
    {
        if (count > 0)
            parts.Add($"{count} {(count > 1 ? many : one)}");
    }

    private sealed record WatchMatch(bool Patient, List<string> Medicaments, List<string> Psychotropes, bool Montant, bool Boites)
    {
        public bool Any => Patient || Medicaments.Count > 0 || Psychotropes.Count > 0 || Montant || Boites;
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

    public bool FacturesInitialized { get; set; }
    public DateTime? LastFactureDate { get; set; }

    /// <summary>Factures already looked at, with their date, within <see cref="AlertTracker.FactureOverlap"/>.</summary>
    public Dictionary<string, DateTime> SeenFactures { get; set; } = [];
}

namespace CHIFA.Contract;

/// <summary>
/// What the gateway may ask an agent. The agent only answers these methods: this list is the allowlist.
/// </summary>
public static class AgentMethods
{
    public const string GetInfo = nameof(GetInfo);
    public const string GetDashboard = nameof(GetDashboard);
    public const string GetRejets = nameof(GetRejets);
    public const string GetBordereaux = nameof(GetBordereaux);
    public const string GetPsychotropes = nameof(GetPsychotropes);
    public const string GetRecentFactures = nameof(GetRecentFactures);
    public const string GetFacture = nameof(GetFacture);
    public const string SearchPatients = nameof(SearchPatients);
    public const string GetPatient = nameof(GetPatient);
    public const string GetPatientsToRemind = nameof(GetPatientsToRemind);
    public const string SearchMedicaments = nameof(SearchMedicaments);
    public const string GetMedicament = nameof(GetMedicament);
    public const string GetRecentDays = nameof(GetRecentDays);
    public const string GetStats = nameof(GetStats);

    /// <summary>Polled by the gateway for phone notifications. Each alert is returned only once.</summary>
    public const string GetAlerts = nameof(GetAlerts);
}

public static class AgentHub
{
    public const string Route = "/hubs/agent";
}

/// <summary>Both bounds are whole, inclusive days. <c>To = null</c> means up to today.</summary>
public record PeriodQuery(DateTime? From, DateTime? To);

/// <param name="Text">Search text, for the lists that have one (patients, medicines).</param>
public record PageQuery(DateTime? From, DateTime? To, int Skip = 0, int Take = PageQuery.DefaultTake, string? Text = null)
{
    public const int DefaultTake = 50;
    public const int MaxTake = 200;

    public int SafeSkip => Math.Max(0, Skip);
    public int SafeTake => Math.Clamp(Take, 1, MaxTake);
}

public record Page<T>(int Total, IReadOnlyList<T> Items);

public record AgentInfo(string? Officine, string AgentVersion, DateTime ServerTime);

public record PatientKey(string NumAssure, string Rang);

public record MedicamentQuery(string NumEnr, DateTime? From, DateTime? To);

/// <param name="Kind">One of <see cref="All"/>.</param>
public record StatsQuery(string Kind, DateTime? From, DateTime? To)
{
    public static readonly IReadOnlyList<string> All = ["jours", "semaines", "mois", "annees", "bordereaux", "produits", "clients"];
}

/// <param name="Key">Stable identity of the event: the agent never returns the same key twice.</param>
/// <param name="Url">Page of the phone app to open, relative to its root (e.g. <c>rejets</c>).</param>
public record AgentAlert(string Key, string Title, string Body, string Url);

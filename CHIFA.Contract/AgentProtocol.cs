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
}

public static class AgentHub
{
    public const string Route = "/hubs/agent";
}

/// <summary>Both bounds are whole, inclusive days. <c>To = null</c> means up to today.</summary>
public record PeriodQuery(DateTime? From, DateTime? To);

public record PageQuery(DateTime? From, DateTime? To, int Skip = 0, int Take = PageQuery.DefaultTake)
{
    public const int DefaultTake = 50;
    public const int MaxTake = 200;

    public int SafeSkip => Math.Max(0, Skip);
    public int SafeTake => Math.Clamp(Take, 1, MaxTake);
}

public record Page<T>(int Total, IReadOnlyList<T> Items);

public record AgentInfo(string? Officine, string AgentVersion, DateTime ServerTime);

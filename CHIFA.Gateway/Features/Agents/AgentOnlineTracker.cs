using System.Collections.Concurrent;

namespace CHIFA.Gateway.Features.Agents;

public record AgentOnlineRules(TimeSpan MinOffline, TimeSpan Cooldown, TimeSpan StartupGrace, TimeSpan StableDelay)
{
    public static AgentOnlineRules From(GatewayOptions options) => new(
        TimeSpan.FromMinutes(options.BackOnlineMinOfflineMinutes),
        TimeSpan.FromMinutes(options.BackOnlineCooldownMinutes),
        StartupGrace: TimeSpan.FromMinutes(3),
        StableDelay: TimeSpan.FromSeconds(30));
}

/// <summary>
/// Decides when a pharmacy is back online after a real outage, so its phones can be told.
/// Short drops, a handover between the in-app agent and the Windows service, and every agent reconnecting
/// after the gateway itself restarted are not outages.
/// </summary>
public class AgentOnlineTracker(DateTime startedAt, AgentOnlineRules rules)
{
    private sealed class State
    {
        public bool Seen;
        public bool Online;
        public DateTime OnlineSince;
        public DateTime? OfflineSince;
        public DateTime? NotifiedAt;
    }

    private readonly ConcurrentDictionary<Guid, State> _states = new();

    public AgentOnlineRules Rules => rules;

    /// <param name="lastSeenAt">The pharmacy's <c>LastSeenAt</c> before this connection: when it went offline, if this gateway never saw it.</param>
    public void Connected(Guid pharmacyId, DateTime now, DateTime? lastSeenAt)
    {
        var state = _states.GetOrAdd(pharmacyId, _ => new State());
        lock (state)
        {
            // Not seen since the gateway started. Right after a start every agent reconnects: that is not an outage.
            if (!state.Seen && now - startedAt >= rules.StartupGrace)
                state.OfflineSince ??= lastSeenAt;
            state.Seen = true;
            state.Online = true;
            state.OnlineSince = now;
        }
    }

    /// <summary>The pharmacy's current agent left (not an older agent replaced by a newer one).</summary>
    public void Disconnected(Guid pharmacyId, DateTime now)
    {
        if (!_states.TryGetValue(pharmacyId, out var state))
            return;
        lock (state)
        {
            state.Online = false;
            // Kept when a reconnect dropped again before it was stable: the outage goes on.
            state.OfflineSince ??= now;
        }
    }

    /// <summary>
    /// Called <see cref="AgentOnlineRules.StableDelay"/> after each connect. Returns how long the pharmacy was offline
    /// when its phones should be told it is back, otherwise <c>null</c>.
    /// </summary>
    public TimeSpan? CheckBackOnline(Guid pharmacyId, DateTime now)
    {
        if (!_states.TryGetValue(pharmacyId, out var state))
            return null;
        lock (state)
        {
            // Dropped again, or a newer connection whose own check will decide.
            if (!state.Online || now - state.OnlineSince < rules.StableDelay)
                return null;
            if (state.OfflineSince is not { } offlineSince)
                return null;

            state.OfflineSince = null;
            var outage = state.OnlineSince - offlineSince;
            if (outage < rules.MinOffline || state.OnlineSince - startedAt < rules.StartupGrace)
                return null;
            if (state.NotifiedAt is { } notifiedAt && now - notifiedAt < rules.Cooldown)
                return null;

            state.NotifiedAt = now;
            return outage;
        }
    }

    /// <summary>"45 min", "2 h 15", "3 j 4 h".</summary>
    public static string FormatDuration(TimeSpan duration)
    {
        if (duration.TotalHours < 1)
            return $"{Math.Max(1, (int)duration.TotalMinutes)} min";
        if (duration.TotalDays < 1)
            return duration.Minutes == 0 ? $"{duration.Hours} h" : $"{duration.Hours} h {duration.Minutes:00}";
        return duration.Hours == 0 ? $"{duration.Days} j" : $"{duration.Days} j {duration.Hours} h";
    }
}

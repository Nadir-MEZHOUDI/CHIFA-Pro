using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace CHIFA.Gateway.Features.Agents;

/// <summary>Which SignalR connection serves each pharmacy. One agent per pharmacy: the newest wins.</summary>
public class AgentRegistry
{
    private readonly ConcurrentDictionary<Guid, HubCallerContext> _agents = new();

    public string? GetConnectionId(Guid pharmacyId) =>
        _agents.TryGetValue(pharmacyId, out var context) ? context.ConnectionId : null;

    public bool IsOnline(Guid pharmacyId) => _agents.ContainsKey(pharmacyId);

    public void Register(Guid pharmacyId, HubCallerContext context)
    {
        HubCallerContext? previous = null;
        _agents.AddOrUpdate(pharmacyId, context, (_, old) =>
        {
            previous = old;
            return context;
        });

        // A second agent with the same key (old PC, copied install): drop the old one.
        if (previous is not null && previous.ConnectionId != context.ConnectionId)
            previous.Abort();
    }

    public void Unregister(Guid pharmacyId, string connectionId)
    {
        if (_agents.TryGetValue(pharmacyId, out var current) && current.ConnectionId == connectionId)
            _agents.TryRemove(KeyValuePair.Create(pharmacyId, current));
    }

    /// <summary>Closes the agent connection, e.g. after its key was replaced.</summary>
    public void Disconnect(Guid pharmacyId)
    {
        if (_agents.TryRemove(pharmacyId, out var context))
            context.Abort();
    }
}

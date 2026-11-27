using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CHIFA.Gateway.Features.Agents;

/// <summary>
/// The pharmacy agent keeps this connection open. The gateway calls the agent through it
/// (<see cref="AgentInvoker"/>); the agent never calls the gateway.
/// </summary>
[Authorize(AuthenticationSchemes = AgentKeyAuthenticationHandler.SchemeName)]
public class AgentConnectionHub(
    AgentRegistry registry,
    AgentOnlineNotifier online,
    IDbContextFactory<GatewayDbContext> dbFactory,
    ILogger<AgentConnectionHub> logger) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var pharmacyId = Context.User!.GetPharmacyId();
        registry.Register(pharmacyId, Context);
        logger.LogInformation("Agent connected: {Pharmacy} ({PharmacyId})", Context.User!.Identity?.Name, pharmacyId);
        online.Connected(pharmacyId, await TouchAsync(pharmacyId));
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var pharmacyId = Context.User!.GetPharmacyId();
        // A replaced agent (handover between the app and the Windows service) is not the pharmacy going offline.
        if (registry.Unregister(pharmacyId, Context.ConnectionId))
            online.Disconnected(pharmacyId);
        logger.LogInformation(exception, "Agent disconnected: {Pharmacy} ({PharmacyId})", Context.User!.Identity?.Name, pharmacyId);
        await TouchAsync(pharmacyId);
    }

    /// <summary>Sets <c>LastSeenAt</c> to now and returns its previous value.</summary>
    private async Task<DateTime?> TouchAsync(Guid pharmacyId)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            var previous = await db.Pharmacies.Where(p => p.Id == pharmacyId).Select(p => p.LastSeenAt).FirstOrDefaultAsync();
            await db.Pharmacies.Where(p => p.Id == pharmacyId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.LastSeenAt, DateTime.UtcNow));
            return previous;
        }
        catch (Exception ex)
        {
            // "Last seen" is informational: never fail the agent connection for it.
            logger.LogWarning(ex, "Could not update LastSeenAt for {PharmacyId}", pharmacyId);
            return null;
        }
    }
}

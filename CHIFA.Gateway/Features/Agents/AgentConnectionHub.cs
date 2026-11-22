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
    IDbContextFactory<GatewayDbContext> dbFactory,
    ILogger<AgentConnectionHub> logger) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var pharmacyId = Context.User!.GetPharmacyId();
        registry.Register(pharmacyId, Context);
        logger.LogInformation("Agent connected: {Pharmacy} ({PharmacyId})", Context.User!.Identity?.Name, pharmacyId);
        await TouchAsync(pharmacyId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var pharmacyId = Context.User!.GetPharmacyId();
        registry.Unregister(pharmacyId, Context.ConnectionId);
        logger.LogInformation(exception, "Agent disconnected: {Pharmacy} ({PharmacyId})", Context.User!.Identity?.Name, pharmacyId);
        await TouchAsync(pharmacyId);
    }

    private async Task TouchAsync(Guid pharmacyId)
    {
        try
        {
            await using var db = await dbFactory.CreateDbContextAsync();
            await db.Pharmacies.Where(p => p.Id == pharmacyId)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.LastSeenAt, DateTime.UtcNow));
        }
        catch (Exception ex)
        {
            // "Last seen" is informational: never fail the agent connection for it.
            logger.LogWarning(ex, "Could not update LastSeenAt for {PharmacyId}", pharmacyId);
        }
    }
}

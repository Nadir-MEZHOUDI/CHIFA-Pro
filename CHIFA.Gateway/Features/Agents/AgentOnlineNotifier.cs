using System.Diagnostics;
using CHIFA.Gateway.Features.Push;
using Microsoft.Extensions.Options;

namespace CHIFA.Gateway.Features.Agents;

/// <summary>Tells a pharmacy's phones when its computer is reachable again after an outage (see <see cref="AgentOnlineTracker"/>).</summary>
public class AgentOnlineNotifier(
    IDbContextFactory<GatewayDbContext> dbFactory,
    PushSender sender,
    IOptions<GatewayOptions> options,
    IHostApplicationLifetime lifetime,
    ILogger<AgentOnlineNotifier> logger)
{
    public const string Tag = "agent-online";

    private readonly AgentOnlineTracker _tracker = new(
        Process.GetCurrentProcess().StartTime.ToUniversalTime(), AgentOnlineRules.From(options.Value));

    public void Connected(Guid pharmacyId, DateTime? lastSeenAt)
    {
        _tracker.Connected(pharmacyId, DateTime.UtcNow, lastSeenAt);
        _ = NotifyWhenStableAsync(pharmacyId, lifetime.ApplicationStopping);
    }

    public void Disconnected(Guid pharmacyId) => _tracker.Disconnected(pharmacyId, DateTime.UtcNow);

    private async Task NotifyWhenStableAsync(Guid pharmacyId, CancellationToken ct)
    {
        try
        {
            await Task.Delay(_tracker.Rules.StableDelay, ct);
            if (_tracker.CheckBackOnline(pharmacyId, DateTime.UtcNow) is not { } outage)
                return;

            List<Data.PushSubscription> subscriptions;
            await using (var db = await dbFactory.CreateDbContextAsync(ct))
            {
                subscriptions = await db.PushSubscriptions.AsNoTracking()
                    .Where(s => s.PharmacyId == pharmacyId && s.Pharmacy.IsActive && (s.Device == null || s.Device.RevokedAt == null))
                    .ToListAsync(ct);
            }
            if (subscriptions.Count == 0)
                return;

            logger.LogInformation("Pharmacy {PharmacyId} back online after {Outage}, notifying {Count} subscription(s)", pharmacyId, outage, subscriptions.Count);
            await sender.SendAsync(subscriptions, new PushPayload(
                "La pharmacie est de nouveau en ligne",
                $"SmartCHIFA est accessible depuis votre téléphone (hors ligne pendant {AgentOnlineTracker.FormatDuration(outage)}).",
                "",
                Tag), ct);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Back-online notification failed for {PharmacyId}", pharmacyId);
        }
    }
}

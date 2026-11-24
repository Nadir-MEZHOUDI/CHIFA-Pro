using CHIFA.Gateway.Features.Agents;
using Microsoft.Extensions.Options;

namespace CHIFA.Gateway.Features.Push;

/// <summary>
/// Asks each online pharmacy agent that has subscribed phones for its new alerts, and pushes them.
/// The agent decides what is new; the gateway keeps nothing but the subscriptions.
/// </summary>
public class AlertPoller(
    IDbContextFactory<GatewayDbContext> dbFactory,
    AgentRegistry registry,
    AgentInvoker agent,
    PushSender sender,
    IOptions<GatewayOptions> options,
    ILogger<AlertPoller> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(options.Value.AlertPollMinutes));
        do
        {
            try
            {
                await PollAsync(stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Alert polling failed");
            }
        } while (await timer.WaitForNextTickAsync(stoppingToken).ConfigureAwait(false));
    }

    public async Task PollAsync(CancellationToken ct)
    {
        List<Data.PushSubscription> subscriptions;
        await using (var db = await dbFactory.CreateDbContextAsync(ct))
        {
            subscriptions = await db.PushSubscriptions.AsNoTracking()
                .Where(s => s.Pharmacy.IsActive && (s.Device == null || s.Device.RevokedAt == null))
                .ToListAsync(ct);
        }

        foreach (var pharmacy in subscriptions.GroupBy(s => s.PharmacyId).Where(g => registry.IsOnline(g.Key)))
        {
            var alerts = await agent.TryCallAsync<List<AgentAlert>>(pharmacy.Key, AgentMethods.GetAlerts, ct);
            foreach (var alert in alerts ?? [])
            {
                logger.LogInformation("Alert {Key} for {PharmacyId} to {Count} subscription(s)", alert.Key, pharmacy.Key, pharmacy.Count());
                await sender.SendAsync(pharmacy.ToList(), new PushPayload(alert.Title, alert.Body, alert.Url, alert.Key), ct);
            }
        }
    }
}

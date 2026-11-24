using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;

namespace CHIFA.Gateway.Features.Agents;

/// <summary>Forwards a phone request to the pharmacy agent and returns its JSON answer untouched.</summary>
public class AgentInvoker(
    AgentRegistry registry,
    IHubContext<AgentConnectionHub> hub,
    IOptions<GatewayOptions> options,
    ILogger<AgentInvoker> logger)
{
    public async Task<IResult> InvokeAsync(Guid pharmacyId, string method, object? argument, CancellationToken requestAborted)
    {
        var connectionId = registry.GetConnectionId(pharmacyId);
        if (connectionId is null)
            return Offline();

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(requestAborted);
        timeout.CancelAfter(TimeSpan.FromSeconds(options.Value.AgentTimeoutSeconds));

        try
        {
            var client = hub.Clients.Client(connectionId);
            var result = argument is null
                ? await client.InvokeAsync<JsonElement>(method, timeout.Token)
                : await client.InvokeAsync<JsonElement>(method, argument, timeout.Token);
            return Results.Json(result);
        }
        // SignalR reports a cancelled invocation as a HubException, not an OperationCanceledException.
        catch (Exception ex) when (ex is OperationCanceledException or HubException
                                   && timeout.IsCancellationRequested && !requestAborted.IsCancellationRequested)
        {
            logger.LogWarning("Agent {PharmacyId} did not answer {Method} in time", pharmacyId, method);
            return Results.Problem(statusCode: StatusCodes.Status504GatewayTimeout,
                title: "La pharmacie ne répond pas", detail: "Le délai d'attente a expiré, réessayez.");
        }
        catch (HubException ex)
        {
            logger.LogWarning("Agent {PharmacyId} failed {Method}: {Error}", pharmacyId, method, ex.Message);
            return Results.Problem(statusCode: StatusCodes.Status502BadGateway,
                title: "Erreur côté pharmacie", detail: ex.Message);
        }
        catch (IOException)
        {
            // The agent disconnected while we were waiting.
            return Offline();
        }
    }

    /// <summary>For background work: the agent's answer, or default when it is offline, slow or fails (logged).</summary>
    public async Task<T?> TryCallAsync<T>(Guid pharmacyId, string method, CancellationToken ct)
    {
        var connectionId = registry.GetConnectionId(pharmacyId);
        if (connectionId is null)
            return default;

        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeout.CancelAfter(TimeSpan.FromSeconds(options.Value.AgentTimeoutSeconds));
        try
        {
            return await hub.Clients.Client(connectionId).InvokeAsync<T>(method, timeout.Token);
        }
        catch (Exception ex) when (!ct.IsCancellationRequested && ex is OperationCanceledException or HubException or IOException)
        {
            // An agent older than the method answers with an error too: it gets it after its next update.
            logger.LogDebug("Agent {PharmacyId} did not answer {Method}: {Error}", pharmacyId, method, ex.Message);
            return default;
        }
    }

    private static IResult Offline() =>
        Results.Problem(statusCode: StatusCodes.Status503ServiceUnavailable,
            title: "Pharmacie hors ligne",
            detail: "L'ordinateur de la pharmacie est éteint ou n'a pas d'internet.");
}

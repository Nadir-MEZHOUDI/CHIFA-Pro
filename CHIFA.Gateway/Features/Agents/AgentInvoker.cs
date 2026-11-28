using System.Text.Json;
using System.Text.Json.Nodes;
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
    /// <param name="hidden">Properties removed from the answer (and from its items, for a list or a page) before it
    /// reaches the phone. Done here rather than in the agent, so an agent not yet updated cannot leak them.</param>
    public async Task<IResult> InvokeAsync(Guid pharmacyId, string method, object? argument, CancellationToken requestAborted, string[]? hidden = null)
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
            return hidden is { Length: > 0 } ? Results.Json(Without(result, hidden)) : Results.Json(result);
        }
        // SignalR reports a cancelled invocation as a HubException, not an OperationCanceledException.
        catch (Exception ex) when (ex is OperationCanceledException or HubException
                                   && timeout.IsCancellationRequested && !requestAborted.IsCancellationRequested)
        {
            logger.LogWarning("Agent {PharmacyId} did not answer {Method} in time", pharmacyId, method);
            return Results.Problem(statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "La pharmacie ne répond pas", detail: "Le délai d'attente a expiré, réessayez.");
        }
        // Not 502/504: Cloudflare replaces those with its own HTML page and the phone loses the message.
        catch (HubException ex) when (IsUnknownMethod(ex))
        {
            logger.LogWarning("Agent {PharmacyId} is too old for {Method}: {Error}", pharmacyId, method, ex.Message);
            return Results.Problem(statusCode: StatusCodes.Status424FailedDependency,
                title: "Mise à jour de SmartCHIFA nécessaire",
                detail: "Mettez à jour SmartCHIFA sur l'ordinateur de la pharmacie pour utiliser cette page.");
        }
        catch (HubException ex)
        {
            logger.LogWarning("Agent {PharmacyId} failed {Method}: {Error}", pharmacyId, method, ex.Message);
            return Results.Problem(statusCode: StatusCodes.Status424FailedDependency,
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

    public static JsonNode? Without(JsonElement json, string[] names)
    {
        var node = JsonNode.Parse(json.GetRawText(), new JsonNodeOptions { PropertyNameCaseInsensitive = true });
        IEnumerable<JsonObject> objects = node switch
        {
            JsonArray list => list.OfType<JsonObject>(),
            JsonObject page when page["items"] is JsonArray items => [page, .. items.OfType<JsonObject>()],
            JsonObject single => [single],
            _ => []
        };

        foreach (var obj in objects)
            foreach (var name in names)
                obj.Remove(name);
        return node;
    }

    // What the SignalR client answers when it has no handler for the method.
    private static bool IsUnknownMethod(HubException ex) =>
        ex.Message.Contains("failed to parse argument", StringComparison.OrdinalIgnoreCase)
        || ex.Message.Contains("didn't provide a result", StringComparison.OrdinalIgnoreCase);

    private static IResult Offline() =>
        Results.Problem(statusCode: StatusCodes.Status503ServiceUnavailable,
            title: "Pharmacie hors ligne",
            detail: "L'ordinateur de la pharmacie est éteint ou n'a pas d'internet.");
}

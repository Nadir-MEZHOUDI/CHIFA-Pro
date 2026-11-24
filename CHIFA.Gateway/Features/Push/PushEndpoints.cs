using System.Security.Claims;
using CHIFA.Gateway.Features.Agents;
using CHIFA.Gateway.Features.Auth;

namespace CHIFA.Gateway.Features.Push;

public record PushKeyResponse(string PublicKey);

/// <summary>The browser's <c>PushSubscription.toJSON()</c>, flattened.</summary>
public record PushSubscribeRequest(string Endpoint, string P256dh, string Auth);

public record PushEndpointRequest(string Endpoint);

public static class PushEndpoints
{
    public static void MapPushEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/push").RequireAuthorization();

        api.MapGet("/key", async (PushSender sender, CancellationToken ct) =>
            Results.Ok(new PushKeyResponse(await sender.GetPublicKeyAsync(ct))));
        api.MapPost("/subscribe", SubscribeAsync);
        api.MapPost("/unsubscribe", UnsubscribeAsync);
        api.MapPost("/test", TestAsync);
    }

    private static async Task<IResult> SubscribeAsync(PushSubscribeRequest request, ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        if (!IsValid(request))
            return Results.Problem(title: "Abonnement invalide.", statusCode: StatusCodes.Status400BadRequest);

        var pharmacyId = user.GetPharmacyId();
        Guid? deviceId = Guid.TryParse(user.FindFirstValue(AuthEndpoints.DeviceIdClaim), out var id) ? id : null;

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        // Same browser signed in to another pharmacy, or keys renewed: the endpoint is the identity.
        var subscription = await db.PushSubscriptions.FirstOrDefaultAsync(s => s.Endpoint == request.Endpoint, ct);
        if (subscription is null)
        {
            subscription = new Data.PushSubscription { Endpoint = request.Endpoint, P256dh = request.P256dh, Auth = request.Auth };
            db.PushSubscriptions.Add(subscription);
        }

        subscription.PharmacyId = pharmacyId;
        subscription.DeviceId = deviceId;
        subscription.P256dh = request.P256dh;
        subscription.Auth = request.Auth;
        await db.SaveChangesAsync(ct);
        return Results.NoContent();
    }

    private static async Task<IResult> UnsubscribeAsync(PushEndpointRequest request, ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        await db.PushSubscriptions.Where(s => s.Endpoint == request.Endpoint && s.PharmacyId == pharmacyId).ExecuteDeleteAsync(ct);
        return Results.NoContent();
    }

    /// <summary>Lets the pharmacist check at once that notifications reach this phone.</summary>
    private static async Task<IResult> TestAsync(PushEndpointRequest request, ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, PushSender sender, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var subscription = await db.PushSubscriptions.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Endpoint == request.Endpoint && s.PharmacyId == pharmacyId, ct);
        if (subscription is null)
            return Results.NotFound();

        await sender.SendAsync([subscription],
            new PushPayload("Notifications activées", "Vous serez prévenu des paiements, des rejets et des malades à relancer.", "", "test"), ct);
        return Results.NoContent();
    }

    /// <summary>The gateway posts to the endpoint: only the browsers' push services are accepted, not any URL.</summary>
    private static readonly string[] PushServiceHosts =
        ["fcm.googleapis.com", ".push.services.mozilla.com", ".notify.windows.com", ".push.apple.com"];

    public static bool IsPushService(Uri uri) =>
        uri.Scheme == Uri.UriSchemeHttps && uri.IsDefaultPort
        && PushServiceHosts.Any(h => h.StartsWith('.') ? uri.Host.EndsWith(h, StringComparison.OrdinalIgnoreCase) : uri.Host.Equals(h, StringComparison.OrdinalIgnoreCase));

    private static bool IsValid(PushSubscribeRequest r) =>
        Uri.TryCreate(r.Endpoint, UriKind.Absolute, out var uri) && IsPushService(uri)
        && r.Endpoint.Length <= 1000
        && !string.IsNullOrWhiteSpace(r.P256dh) && r.P256dh.Length <= 200
        && !string.IsNullOrWhiteSpace(r.Auth) && r.Auth.Length <= 100;
}

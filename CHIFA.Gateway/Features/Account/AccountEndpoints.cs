using System.Security.Claims;
using CHIFA.Gateway.Features.Agents;
using CHIFA.Gateway.Features.Auth;
using CHIFA.Gateway.Features.Link;

namespace CHIFA.Gateway.Features.Account;

/// <param name="IsCurrent">The phone making the request.</param>
public record PhoneDevice(Guid Id, string Name, DateTime CreatedAt, DateTime? LastSeenAt, bool IsCurrent);

/// <param name="Status">One of <see cref="SubscriptionStates"/>.</param>
/// <param name="ExpiresAt"><c>null</c>: no end date, or <see cref="Status"/> is unknown.</param>
/// <param name="CheckedAt">When SmartCHIFA last sent its license.</param>
public record SubscriptionStatus(string Status, bool IsTrial, DateTime? ExpiresAt, int? DaysLeft, DateTime? CheckedAt);

public static class SubscriptionStates
{
    public const string Unknown = "unknown";
    public const string Active = "active";
    public const string Trial = "trial";
    public const string Expiring = "expiring";
    public const string Expired = "expired";

    /// <summary>Same warning as SmartCHIFA on the computer.</summary>
    public const int ExpiringDays = 15;
}

/// <summary>
/// The pharmacy's own account, seen from a phone: its subscription and its linked phones.
/// Read from the gateway's database, so it works while the pharmacy computer is off.
/// </summary>
public static class AccountEndpoints
{
    public const string TrialEdition = "trial";

    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api").RequireAuthorization();
        api.MapGet("/subscription", SubscriptionAsync);
        api.MapGet("/devices", DevicesAsync);
        api.MapDelete("/devices/{id:guid}", RemoveDeviceAsync);
    }

    private static async Task<IResult> SubscriptionAsync(ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pharmacyId, ct);
        return pharmacy is null ? Results.NotFound() : Results.Ok(Subscription(pharmacy, DateTime.UtcNow));
    }

    public static SubscriptionStatus Subscription(Pharmacy pharmacy, DateTime utcNow)
    {
        if (pharmacy.LicenseCheckedAt is not { } checkedAt)
            return new SubscriptionStatus(SubscriptionStates.Unknown, false, null, null, null);

        var isTrial = string.Equals(pharmacy.LicenseEdition, TrialEdition, StringComparison.OrdinalIgnoreCase);
        if (pharmacy.LicenseExpiresAt is not { } end)
            return new SubscriptionStatus(isTrial ? SubscriptionStates.Trial : SubscriptionStates.Active, isTrial, null, null, checkedAt);

        var daysLeft = Math.Max(0, (int)Math.Ceiling((end - utcNow).TotalDays));
        var status = end <= utcNow ? SubscriptionStates.Expired
            : isTrial ? SubscriptionStates.Trial
            : daysLeft <= SubscriptionStates.ExpiringDays ? SubscriptionStates.Expiring
            : SubscriptionStates.Active;
        return new SubscriptionStatus(status, isTrial, end, daysLeft, checkedAt);
    }

    private static async Task<IResult> DevicesAsync(ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        var current = CurrentDeviceId(user);
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var devices = await db.MobileDevices.AsNoTracking()
            .Where(d => d.PharmacyId == pharmacyId && d.RevokedAt == null)
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new PhoneDevice(d.Id, d.Name, d.CreatedAt, d.LastSeenAt, d.Id == current))
            .ToListAsync(ct);
        return Results.Ok(devices);
    }

    /// <summary>Unlinks another phone of the pharmacy. This phone leaves with « Se déconnecter » instead.</summary>
    private static async Task<IResult> RemoveDeviceAsync(Guid id, ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        if (id == CurrentDeviceId(user))
            return Results.Problem(statusCode: StatusCodes.Status400BadRequest,
                title: "C'est ce téléphone.", detail: "Pour le retirer, utilisez « Se déconnecter » dans le menu.");

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await LinkEndpoints.RevokeDeviceAsync(db, user.GetPharmacyId(), id, ct)
            ? Results.NoContent()
            : Results.Problem(statusCode: StatusCodes.Status404NotFound, title: "Appareil introuvable.", detail: "Il a peut-être déjà été retiré.");
    }

    // Phones signed in with a password (before pairing codes) have no device.
    private static Guid? CurrentDeviceId(ClaimsPrincipal user) =>
        Guid.TryParse(user.FindFirstValue(AuthEndpoints.DeviceIdClaim), out var id) ? id : null;
}

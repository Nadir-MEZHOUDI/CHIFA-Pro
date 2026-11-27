using System.Security.Claims;
using CHIFA.Gateway.Features.Agents;
using CHIFA.Gateway.Features.Auth;
using CHIFA.Gateway.Features.Link;
using Microsoft.Extensions.Options;

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
        api.MapPost("/devices/code", CreateCodeAsync).RequireSubscription();
    }

    /// <summary>
    /// The trial or subscription has ended. Only a license SmartCHIFA sent counts: a pharmacy whose license is
    /// unknown (added by the admin, older SmartCHIFA) or without an end date is never blocked.
    /// </summary>
    public static bool HasEnded(Pharmacy pharmacy, DateTime utcNow) =>
        pharmacy.LicenseCheckedAt is not null && pharmacy.LicenseExpiresAt is { } end && end <= utcNow;

    public static bool IsTrial(Pharmacy pharmacy) =>
        string.Equals(pharmacy.LicenseEdition, TrialEdition, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Refuses the request once the trial or subscription has ended. SmartCHIFA itself stops opening then, but
    /// the CHIFA.Agent service keeps answering, so the gateway is where the phone is stopped. It lifts as soon
    /// as SmartCHIFA sends the renewed license.
    /// </summary>
    public static TBuilder RequireSubscription<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder =>
        builder.AddEndpointFilter(async (context, next) =>
        {
            var http = context.HttpContext;
            var pharmacyId = http.User.GetPharmacyId();
            await using var db = await http.RequestServices.GetRequiredService<IDbContextFactory<GatewayDbContext>>()
                .CreateDbContextAsync(http.RequestAborted);
            var pharmacy = await db.Pharmacies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pharmacyId, http.RequestAborted);
            return pharmacy is not null && HasEnded(pharmacy, DateTime.UtcNow)
                ? Ended(IsTrial(pharmacy))
                : await next(context);
        });

    // 402, not a 5xx: Cloudflare would replace a 502/504 with its own page and the phone would lose the message.
    private static IResult Ended(bool isTrial) =>
        Results.Problem(statusCode: StatusCodes.Status402PaymentRequired,
            title: isTrial ? "Essai gratuit terminé" : "Abonnement terminé",
            detail: isTrial
                ? "Demandez une licence dans SmartCHIFA (menu « Licence ») pour consulter à nouveau la pharmacie sur ce téléphone."
                : "Renouvelez l'abonnement auprès de SmartApp, puis cliquez sur « Vérifier en ligne » dans SmartCHIFA.");

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

        var isTrial = IsTrial(pharmacy);
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

    /// <summary>
    /// A linked phone shows a code (and its QR code) for another phone or browser, for when the pharmacist
    /// is away from the pharmacy computer. Same code as the one CHIFA Pro shows: it replaces it.
    /// </summary>
    private static async Task<IResult> CreateCodeAsync(
        ClaimsPrincipal user,
        IDbContextFactory<GatewayDbContext> dbFactory,
        AgentRegistry registry,
        IOptions<GatewayOptions> options,
        CancellationToken ct)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return Results.Ok(await LinkEndpoints.IssueCodeAsync(db, registry, user.GetPharmacyId(), options.Value.PairingCodeMinutes, ct));
    }

    // Phones signed in with a password (before pairing codes) have no device.
    private static Guid? CurrentDeviceId(ClaimsPrincipal user) =>
        Guid.TryParse(user.FindFirstValue(AuthEndpoints.DeviceIdClaim), out var id) ? id : null;
}

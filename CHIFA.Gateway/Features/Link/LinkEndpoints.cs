using System.Security.Claims;
using CHIFA.Gateway.Features.Agents;
using CHIFA.Gateway.Features.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CHIFA.Gateway.Features.Link;

/// <summary>
/// Called by CHIFA Pro. It registers its pharmacy once with its signed license (no admin step), then uses
/// the returned agent key both to run the agent and to show pairing codes for phones.
/// </summary>
public static class LinkEndpoints
{
    public static void MapLinkEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(LinkRoutes.Register, RegisterAsync).RequireRateLimiting(AuthEndpoints.LoginRateLimit).AllowAnonymous();

        var pharmacy = app.MapGroup("").RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = AgentKeyAuthenticationHandler.SchemeName });
        pharmacy.MapPost(LinkRoutes.Codes, CreateCodeAsync);
        pharmacy.MapGet(LinkRoutes.Devices, ListDevicesAsync);
        pharmacy.MapDelete(LinkRoutes.Devices + "/{id:guid}", RemoveDeviceAsync);
        pharmacy.MapPost(LinkRoutes.License, UpdateLicenseAsync);
    }

    /// <summary>
    /// Creates the pharmacy of this license, or gives it a new agent key when CHIFA Pro lost its own
    /// (reinstall). The old key stops working at once.
    /// </summary>
    private static async Task<IResult> RegisterAsync(
        RegisterPharmacyRequest request,
        IDbContextFactory<GatewayDbContext> dbFactory,
        AgentRegistry registry,
        IOptions<GatewayOptions> options,
        CancellationToken ct)
    {
        var license = LicenseTokens.Verify(request.License, options.Value.LicensePublicKey, options.Value.LicenseProduct, DateTimeOffset.UtcNow, out var error);
        if (license is null)
            return Results.Problem(title: error, statusCode: StatusCodes.Status403Forbidden);

        var key = AgentKeys.Generate();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.FirstOrDefaultAsync(p => p.LicenseId == license.LicenseId, ct);
        var existed = pharmacy is not null;

        if (pharmacy is null)
        {
            pharmacy = new Pharmacy { Name = PharmacyName(license.Customer), LicenseId = license.LicenseId, AgentKeyHash = AgentKeys.Hash(key) };
            db.Pharmacies.Add(pharmacy);
            KeepLicense(pharmacy, license);
        }
        else if (!pharmacy.IsActive)
        {
            return Results.Problem(title: "L'application mobile est désactivée pour cette pharmacie. Contactez SmartApp.",
                statusCode: StatusCodes.Status403Forbidden);
        }
        else
        {
            pharmacy.AgentKeyHash = AgentKeys.Hash(key);
            KeepLicense(pharmacy, license);
        }

        await db.SaveChangesAsync(ct);
        if (existed)
            registry.Disconnect(pharmacy.Id);

        return Results.Ok(new RegisterPharmacyResponse(pharmacy.Id, pharmacy.Name, key));
    }

    /// <summary>A new code replaces the previous one, so only the code on screen works.</summary>
    private static async Task<IResult> CreateCodeAsync(
        ClaimsPrincipal user,
        IDbContextFactory<GatewayDbContext> dbFactory,
        AgentRegistry registry,
        IOptions<GatewayOptions> options,
        CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        var now = DateTime.UtcNow;
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        await db.PairingCodes.Where(c => c.PharmacyId == pharmacyId || c.ExpiresAt < now).ExecuteDeleteAsync(ct);

        var expiresAt = now.AddMinutes(options.Value.PairingCodeMinutes);
        for (var attempt = 0; ; attempt++)
        {
            var code = PairingCodes.Generate();
            var row = new PairingCode { PharmacyId = pharmacyId, CodeHash = PairingCodes.Hash(code), ExpiresAt = expiresAt };
            db.PairingCodes.Add(row);
            try
            {
                await db.SaveChangesAsync(ct);
                return Results.Ok(new PairingCodeResponse(PairingCodes.Display(code), expiresAt, registry.IsOnline(pharmacyId)));
            }
            catch (DbUpdateException) when (attempt < 2)
            {
                // Same code pending for another pharmacy (1 in 31^8): draw again.
                db.Entry(row).State = EntityState.Detached;
            }
        }
    }

    private static async Task<IResult> ListDevicesAsync(ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var devices = await db.MobileDevices.AsNoTracking()
            .Where(d => d.PharmacyId == pharmacyId && d.RevokedAt == null)
            .OrderByDescending(d => d.CreatedAt)
            .Select(d => new LinkedDevice(d.Id, d.Name, d.CreatedAt, d.LastSeenAt))
            .ToListAsync(ct);
        return Results.Ok(devices);
    }

    /// <summary>For a lost or replaced phone: it is signed out on its next request.</summary>
    private static async Task<IResult> RemoveDeviceAsync(Guid id, ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        return await RevokeDeviceAsync(db, user.GetPharmacyId(), id, ct) ? Results.NoContent() : Results.NotFound();
    }

    /// <summary>Signs the phone out at once and stops its notifications. False when the pharmacy has no such active phone.</summary>
    public static async Task<bool> RevokeDeviceAsync(GatewayDbContext db, Guid pharmacyId, Guid deviceId, CancellationToken ct)
    {
        var removed = await db.MobileDevices
            .Where(d => d.Id == deviceId && d.PharmacyId == pharmacyId && d.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(d => d.RevokedAt, DateTime.UtcNow), ct);
        if (removed == 0)
            return false;

        await db.PushSubscriptions.Where(s => s.DeviceId == deviceId).ExecuteDeleteAsync(ct);
        return true;
    }

    /// <summary>
    /// CHIFA Pro sends its license file again after each online check-in, so the phone shows the current
    /// end of the subscription. The signature makes the dates trustworthy.
    /// </summary>
    private static async Task<IResult> UpdateLicenseAsync(
        LicenseUpdateRequest request,
        ClaimsPrincipal user,
        IDbContextFactory<GatewayDbContext> dbFactory,
        IOptions<GatewayOptions> options,
        CancellationToken ct)
    {
        var license = LicenseTokens.Verify(request.License, options.Value.LicensePublicKey, options.Value.LicenseProduct, DateTimeOffset.UtcNow, out var error);
        if (license is null)
            return Results.Problem(title: error, statusCode: StatusCodes.Status403Forbidden);

        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.FirstAsync(p => p.Id == pharmacyId, ct);
        if (pharmacy.LicenseId != license.LicenseId)
            return Results.Problem(title: "Cette licence n'est pas celle de cette pharmacie.", statusCode: StatusCodes.Status403Forbidden);

        KeepLicense(pharmacy, license);
        await db.SaveChangesAsync(ct);
        return Results.NoContent();
    }

    private static void KeepLicense(Pharmacy pharmacy, VerifiedLicense license)
    {
        pharmacy.LicenseExpiresAt = license.ExpiresAt?.UtcDateTime;
        pharmacy.LicenseEdition = license.Edition is { Length: > 50 } edition ? edition[..50] : license.Edition;
        pharmacy.LicenseCheckedAt = DateTime.UtcNow;
    }

    private static string PharmacyName(string? customer)
    {
        var name = string.IsNullOrWhiteSpace(customer) ? "Pharmacie" : customer.Trim();
        return name.Length > 200 ? name[..200] : name;
    }
}

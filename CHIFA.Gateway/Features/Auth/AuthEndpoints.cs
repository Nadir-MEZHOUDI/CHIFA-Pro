using System.Security.Claims;
using System.Text;
using CHIFA.Gateway.Features.Agents;
using CHIFA.Gateway.Features.Link;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CHIFA.Gateway.Features.Auth;

public record LoginRequest(string UserName, string Password);

/// <param name="DeviceToken">Only when a phone is linked: it keeps it to sign in again without a code.</param>
public record LoginResponse(string AccessToken, DateTime ExpiresAt, string Pharmacy, string? DeviceToken = null);

/// <param name="Code">The code shown in SmartCHIFA, e.g. <c>ABCD-2345</c>.</param>
public record PairRequest(string Code, string? DeviceName);

public record DeviceLoginRequest(string DeviceToken);

public static class AuthEndpoints
{
    public const string LoginRateLimit = "login";
    public const string DeviceIdClaim = "device_id";

    // Verified against when the user does not exist, so both cases take the same time.
    private static readonly string DummyHash = new PasswordHasher<MobileUser>().HashPassword(null!, "not-a-real-password");

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", LoginAsync).RequireRateLimiting(LoginRateLimit).AllowAnonymous();
        app.MapPost("/api/auth/pair", PairAsync).RequireRateLimiting(LoginRateLimit).AllowAnonymous();
        app.MapPost("/api/auth/device", DeviceLoginAsync).RequireRateLimiting(LoginRateLimit).AllowAnonymous();
        app.MapPost("/api/auth/logout", LogoutAsync).RequireAuthorization();
    }

    /// <summary>Links a phone: the one-time code becomes a lasting device token.</summary>
    private static async Task<IResult> PairAsync(PairRequest request, IDbContextFactory<GatewayDbContext> dbFactory, IOptions<JwtOptions> jwt, CancellationToken ct)
    {
        var invalid = Results.Problem(
            title: "Code invalide ou expiré.",
            detail: "Affichez un nouveau code dans SmartCHIFA (Application mobile) puis réessayez.",
            statusCode: StatusCodes.Status400BadRequest);

        var code = PairingCodes.Normalize(request.Code);
        if (code is null)
            return invalid;

        var hash = PairingCodes.Hash(code);
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pairing = await db.PairingCodes
            .Include(c => c.Pharmacy)
            .FirstOrDefaultAsync(c => c.CodeHash == hash && c.ExpiresAt > DateTime.UtcNow && c.Pharmacy.IsActive, ct);
        if (pairing is null)
            return invalid;

        var token = AgentKeys.Generate();
        var device = new MobileDevice
        {
            PharmacyId = pairing.PharmacyId,
            Name = DeviceName(request.DeviceName),
            TokenHash = AgentKeys.Hash(token),
            LastSeenAt = DateTime.UtcNow
        };
        db.PairingCodes.Remove(pairing);
        db.MobileDevices.Add(device);
        await db.SaveChangesAsync(ct);

        var expiresAt = DateTime.UtcNow.AddHours(jwt.Value.LifetimeHours);
        return Results.Ok(new LoginResponse(CreateToken(DeviceClaims(device), expiresAt, jwt.Value), expiresAt, pairing.Pharmacy.Name, token));
    }

    /// <summary>A linked phone opening the app again: no code, no password.</summary>
    private static async Task<IResult> DeviceLoginAsync(DeviceLoginRequest request, IDbContextFactory<GatewayDbContext> dbFactory, IOptions<JwtOptions> jwt, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.DeviceToken))
            return Results.Unauthorized();

        var hash = AgentKeys.Hash(request.DeviceToken.Trim());
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var device = await db.MobileDevices
            .Include(d => d.Pharmacy)
            .FirstOrDefaultAsync(d => d.TokenHash == hash && d.RevokedAt == null && d.Pharmacy.IsActive, ct);
        if (device is null)
            return Results.Unauthorized();

        device.LastSeenAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        var expiresAt = DateTime.UtcNow.AddHours(jwt.Value.LifetimeHours);
        return Results.Ok(new LoginResponse(CreateToken(DeviceClaims(device), expiresAt, jwt.Value), expiresAt, device.Pharmacy.Name));
    }

    /// <summary>The phone forgets its token; revoking it here makes a copy of it useless too.</summary>
    private static async Task<IResult> LogoutAsync(ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        if (Guid.TryParse(user.FindFirstValue(DeviceIdClaim), out var deviceId))
        {
            await using var db = await dbFactory.CreateDbContextAsync(ct);
            await db.MobileDevices.Where(d => d.Id == deviceId && d.RevokedAt == null)
                .ExecuteUpdateAsync(s => s.SetProperty(d => d.RevokedAt, DateTime.UtcNow), ct);
            await db.PushSubscriptions.Where(s => s.DeviceId == deviceId).ExecuteDeleteAsync(ct);
        }

        return Results.NoContent();
    }

    /// <summary>
    /// JWT check for phones: a device removed in SmartCHIFA or logged out loses access at once,
    /// not when its access token expires.
    /// </summary>
    public static async Task RejectRevokedDeviceAsync(TokenValidatedContext context)
    {
        if (!Guid.TryParse(context.Principal?.FindFirstValue(DeviceIdClaim), out var deviceId))
            return;

        var dbFactory = context.HttpContext.RequestServices.GetRequiredService<IDbContextFactory<GatewayDbContext>>();
        await using var db = await dbFactory.CreateDbContextAsync(context.HttpContext.RequestAborted);
        var active = await db.MobileDevices.AnyAsync(
            d => d.Id == deviceId && d.RevokedAt == null && d.Pharmacy.IsActive, context.HttpContext.RequestAborted);
        if (!active)
            context.Fail("This phone was unlinked.");
    }

    private static Claim[] DeviceClaims(MobileDevice device) =>
    [
        new(JwtRegisteredClaimNames.Sub, device.Id.ToString()),
        new(JwtRegisteredClaimNames.Name, device.Name),
        new(DeviceIdClaim, device.Id.ToString()),
        new(PharmacyClaims.PharmacyId, device.PharmacyId.ToString())
    ];

    private static string DeviceName(string? name)
    {
        name = name?.Trim();
        if (string.IsNullOrEmpty(name))
            return "Téléphone";
        return name.Length > 100 ? name[..100] : name;
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        IDbContextFactory<GatewayDbContext> dbFactory,
        IPasswordHasher<MobileUser> hasher,
        IOptions<JwtOptions> jwt,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrEmpty(request.Password))
            return Results.Unauthorized();

        var userName = NormalizeUserName(request.UserName);
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var user = await db.MobileUsers
            .Include(u => u.Pharmacy)
            .FirstOrDefaultAsync(u => u.UserName == userName && u.IsActive && u.Pharmacy.IsActive, ct);

        if (user is null)
        {
            hasher.VerifyHashedPassword(null!, DummyHash, request.Password);
            return Results.Unauthorized();
        }

        var check = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (check == PasswordVerificationResult.Failed)
            return Results.Unauthorized();

        if (check == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = hasher.HashPassword(user, request.Password);
            await db.SaveChangesAsync(ct);
        }

        var expiresAt = DateTime.UtcNow.AddHours(jwt.Value.LifetimeHours);
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Name, user.UserName),
            new(PharmacyClaims.PharmacyId, user.PharmacyId.ToString())
        ];
        return Results.Ok(new LoginResponse(CreateToken(claims, expiresAt, jwt.Value), expiresAt, user.Pharmacy.Name));
    }

    public static string NormalizeUserName(string userName) => userName.Trim().ToLowerInvariant();

    public static SymmetricSecurityKey SigningKey(JwtOptions options) => new(Encoding.UTF8.GetBytes(options.SigningKey));

    private static string CreateToken(Claim[] claims, DateTime expiresAt, JwtOptions options) =>
        new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = options.Issuer,
            Audience = options.Audience,
            Expires = expiresAt,
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(SigningKey(options), SecurityAlgorithms.HmacSha256)
        });
}

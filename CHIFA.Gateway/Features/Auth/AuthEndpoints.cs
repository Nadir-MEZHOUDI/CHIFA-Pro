using System.Security.Claims;
using System.Text;
using CHIFA.Gateway.Features.Agents;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CHIFA.Gateway.Features.Auth;

public record LoginRequest(string UserName, string Password);

public record LoginResponse(string AccessToken, DateTime ExpiresAt, string Pharmacy);

public static class AuthEndpoints
{
    public const string LoginRateLimit = "login";

    // Verified against when the user does not exist, so both cases take the same time.
    private static readonly string DummyHash = new PasswordHasher<MobileUser>().HashPassword(null!, "not-a-real-password");

    public static void MapAuthEndpoints(this IEndpointRouteBuilder app) =>
        app.MapPost("/api/auth/login", LoginAsync).RequireRateLimiting(LoginRateLimit).AllowAnonymous();

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
        return Results.Ok(new LoginResponse(CreateToken(user, expiresAt, jwt.Value), expiresAt, user.Pharmacy.Name));
    }

    public static string NormalizeUserName(string userName) => userName.Trim().ToLowerInvariant();

    public static SymmetricSecurityKey SigningKey(JwtOptions options) => new(Encoding.UTF8.GetBytes(options.SigningKey));

    private static string CreateToken(MobileUser user, DateTime expiresAt, JwtOptions options) =>
        new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = options.Issuer,
            Audience = options.Audience,
            Expires = expiresAt,
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(PharmacyClaims.PharmacyId, user.PharmacyId.ToString())
            ]),
            SigningCredentials = new SigningCredentials(SigningKey(options), SecurityAlgorithms.HmacSha256)
        });
}

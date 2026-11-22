using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace CHIFA.Gateway.Features.Agents;

/// <summary>Authenticates the pharmacy agent with its key, sent as a bearer token.</summary>
public class AgentKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IDbContextFactory<GatewayDbContext> dbFactory)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "AgentKey";

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var key = ReadKey();
        if (string.IsNullOrEmpty(key))
            return AuthenticateResult.NoResult();

        var hash = AgentKeys.Hash(key);
        await using var db = await dbFactory.CreateDbContextAsync(Context.RequestAborted);
        var pharmacy = await db.Pharmacies.AsNoTracking()
            .Where(p => p.AgentKeyHash == hash && p.IsActive)
            .Select(p => new { p.Id, p.Name })
            .FirstOrDefaultAsync(Context.RequestAborted);

        if (pharmacy is null)
            return AuthenticateResult.Fail("Invalid agent key.");

        var identity = new ClaimsIdentity(
            [new Claim(PharmacyClaims.PharmacyId, pharmacy.Id.ToString()), new Claim(ClaimTypes.Name, pharmacy.Name)],
            SchemeName);
        return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), SchemeName));
    }

    private string? ReadKey()
    {
        var header = Request.Headers.Authorization.ToString();
        if (header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return header["Bearer ".Length..].Trim();

        // WebSocket clients that cannot set headers send the token in the query string.
        return Request.Query["access_token"].ToString();
    }
}

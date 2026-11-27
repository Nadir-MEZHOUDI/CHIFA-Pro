using System.Security.Cryptography;
using System.Text;
using CHIFA.Gateway.Features.Agents;
using CHIFA.Gateway.Features.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CHIFA.Gateway.Features.Admin;

public record CreatePharmacyRequest(string Name);

public record AgentKeyResponse(Guid Id, string Name, string AgentKey);

public record CreateUserRequest(string UserName, string Password);

public record PharmacySummary(Guid Id, string Name, bool IsActive, bool Online, DateTime? LastSeenAt, int Users);

/// <summary>Your own tooling: register a pharmacy, give its agent a key, create phone users.</summary>
public static class AdminEndpoints
{
    public const string AdminKeyHeader = "X-Admin-Key";
    public const int MinPasswordLength = 8;

    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var admin = app.MapGroup("/admin").AllowAnonymous().AddEndpointFilter(RequireAdminKey);

        admin.MapGet("/pharmacies", ListPharmaciesAsync);
        admin.MapPost("/pharmacies", CreatePharmacyAsync);
        admin.MapPost("/pharmacies/{id:guid}/agent-key", ReplaceAgentKeyAsync);
        admin.MapPost("/pharmacies/{id:guid}/users", CreateUserAsync);
    }

    public static async ValueTask<object?> RequireAdminKey(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var http = context.HttpContext;
        var expected = http.RequestServices.GetRequiredService<IOptions<GatewayOptions>>().Value.AdminKey;
        var provided = http.Request.Headers[AdminKeyHeader].ToString();

        var valid = CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(provided), Encoding.UTF8.GetBytes(expected));
        return valid ? await next(context) : Results.Unauthorized();
    }

    private static async Task<IResult> ListPharmaciesAsync(IDbContextFactory<GatewayDbContext> dbFactory, AgentRegistry registry, CancellationToken ct)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var rows = await db.Pharmacies.AsNoTracking()
            .OrderBy(p => p.Name)
            .Select(p => new { p.Id, p.Name, p.IsActive, p.LastSeenAt, Users = p.Users.Count })
            .ToListAsync(ct);

        return Results.Ok(rows.Select(p => new PharmacySummary(p.Id, p.Name, p.IsActive, registry.IsOnline(p.Id), p.LastSeenAt, p.Users)));
    }

    private static async Task<IResult> CreatePharmacyAsync(CreatePharmacyRequest request, IDbContextFactory<GatewayDbContext> dbFactory, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Results.ValidationProblem(new Dictionary<string, string[]> { ["name"] = ["Le nom est obligatoire."] });

        var key = AgentKeys.Generate();
        var pharmacy = new Pharmacy { Name = request.Name.Trim(), AgentKeyHash = AgentKeys.Hash(key) };

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        db.Pharmacies.Add(pharmacy);
        await db.SaveChangesAsync(ct);

        return Results.Ok(new AgentKeyResponse(pharmacy.Id, pharmacy.Name, key));
    }

    private static async Task<IResult> ReplaceAgentKeyAsync(Guid id, IDbContextFactory<GatewayDbContext> dbFactory, AgentRegistry registry, CancellationToken ct)
    {
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.FindAsync([id], ct);
        if (pharmacy is null)
            return Results.NotFound();

        var key = AgentKeys.Generate();
        pharmacy.AgentKeyHash = AgentKeys.Hash(key);
        await db.SaveChangesAsync(ct);

        // The connected agent still uses the old key: cut it off.
        registry.Disconnect(id);
        return Results.Ok(new AgentKeyResponse(pharmacy.Id, pharmacy.Name, key));
    }

    private static async Task<IResult> CreateUserAsync(
        Guid id,
        CreateUserRequest request,
        IDbContextFactory<GatewayDbContext> dbFactory,
        IPasswordHasher<MobileUser> hasher,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || (request.Password ?? "").Length < MinPasswordLength)
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["user"] = [$"Nom d'utilisateur obligatoire, mot de passe d'au moins {MinPasswordLength} caractères."]
            });

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        if (!await db.Pharmacies.AnyAsync(p => p.Id == id, ct))
            return Results.NotFound();

        var userName = AuthEndpoints.NormalizeUserName(request.UserName);
        if (await db.MobileUsers.AnyAsync(u => u.UserName == userName, ct))
            return Results.Conflict(new { error = "Ce nom d'utilisateur existe déjà." });

        var user = new MobileUser { PharmacyId = id, UserName = userName, PasswordHash = "" };
        user.PasswordHash = hasher.HashPassword(user, request.Password!);
        db.MobileUsers.Add(user);
        await db.SaveChangesAsync(ct);

        return Results.Ok(new { user.Id, user.UserName });
    }
}

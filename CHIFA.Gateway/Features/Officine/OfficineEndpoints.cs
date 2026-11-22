using System.Security.Claims;
using CHIFA.Gateway.Features.Agents;

namespace CHIFA.Gateway.Features.Officine;

public record OfficineStatus(string Name, bool Online, DateTime? LastSeenAt);

/// <summary>
/// What the phone can read. Each call is answered live by the pharmacy agent; the gateway stores nothing.
/// The pharmacy always comes from the token, never from the request.
/// </summary>
public static class OfficineEndpoints
{
    public static void MapOfficineEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/officine").RequireAuthorization();

        api.MapGet("/status", StatusAsync);
        api.MapGet("/info", (ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetInfo, null, ct));

        api.MapGet("/dashboard", (DateTime? from, DateTime? to, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetDashboard, new PeriodQuery(from, to), ct));

        MapPaged(api, "/rejets", AgentMethods.GetRejets);
        MapPaged(api, "/bordereaux", AgentMethods.GetBordereaux);
        MapPaged(api, "/psychotropes", AgentMethods.GetPsychotropes);

        api.MapGet("/factures/recent", (int? take, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetRecentFactures, new PageQuery(null, null, 0, take ?? PageQuery.DefaultTake), ct));
    }

    private static void MapPaged(RouteGroupBuilder api, string route, string method) =>
        api.MapGet(route, (DateTime? from, DateTime? to, int? skip, int? take, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), method, new PageQuery(from, to, skip ?? 0, take ?? PageQuery.DefaultTake), ct));

    private static async Task<IResult> StatusAsync(ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, AgentRegistry registry, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.AsNoTracking()
            .Where(p => p.Id == pharmacyId)
            .Select(p => new { p.Name, p.LastSeenAt })
            .FirstOrDefaultAsync(ct);

        return pharmacy is null
            ? Results.NotFound()
            : Results.Ok(new OfficineStatus(pharmacy.Name, registry.IsOnline(pharmacyId), pharmacy.LastSeenAt));
    }
}

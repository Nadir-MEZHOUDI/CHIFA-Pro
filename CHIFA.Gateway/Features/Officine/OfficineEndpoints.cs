using System.Security.Claims;
using CHIFA.Gateway.Features.Account;
using CHIFA.Gateway.Features.Agents;

namespace CHIFA.Gateway.Features.Officine;

/// <param name="SubscriptionEnded">The trial or subscription has ended: the other routes answer 402.</param>
public record OfficineStatus(string Name, bool Online, DateTime? LastSeenAt, bool SubscriptionEnded = false, bool IsTrial = false);

/// <summary>
/// What the phone can read. Each call is answered live by the pharmacy agent; the gateway stores nothing.
/// The pharmacy always comes from the token, never from the request.
/// </summary>
public static class OfficineEndpoints
{
    public static void MapOfficineEndpoints(this IEndpointRouteBuilder app)
    {
        var officine = app.MapGroup("/api/officine").RequireAuthorization();
        officine.MapGet("/status", StatusAsync);

        var api = officine.MapGroup("").RequireSubscription();
        api.MapGet("/info", (ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetInfo, null, ct));

        // An employee's phone: patients, médicaments, factures and psychotropes, without the totals below.
        MapPaged(api, "/psychotropes", AgentMethods.GetPsychotropes);

        api.MapGet("/factures/recent", (int? take, string? q, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetRecentFactures, new PageQuery(null, null, 0, take ?? PageQuery.DefaultTake, q), ct));
        api.MapGet("/factures/{numFact}", (string numFact, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetFacture, numFact, ct));

        MapPaged(api, "/patients", AgentMethods.SearchPatients);
        api.MapGet("/patients/relance", (ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetPatientsToRemind, null, ct, StaffHidden(user, ReminderTotals)));
        api.MapGet("/patients/{numAssure}/{rang}", (string numAssure, string rang, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetPatient, new PatientKey(numAssure, rang), ct, StaffHidden(user, PatientTotals)));

        MapPaged(api, "/medicaments", AgentMethods.SearchMedicaments, MedicamentTotals);
        api.MapGet("/medicaments/{numEnr}", (string numEnr, DateTime? from, DateTime? to, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetMedicament, new MedicamentQuery(numEnr, from, to), ct, StaffHidden(user, MedicamentTotals)));

        // The pharmacist's only: turnover, margins, CNAS payments and rejections, and the alerts set for the pharmacy.
        var owner = api.MapGroup("").RequireOwner();
        owner.MapGet("/dashboard", (DateTime? from, DateTime? to, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetDashboard, new PeriodQuery(from, to), ct));

        MapPaged(owner, "/rejets", AgentMethods.GetRejets);
        MapPaged(owner, "/bordereaux", AgentMethods.GetBordereaux);
        owner.MapGet("/bordereaux/{numBord}", (string numBord, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetBordereau, numBord, ct));

        owner.MapGet("/stats/recent", (ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetRecentDays, null, ct));
        owner.MapGet("/stats/{kind}", async (string kind, DateTime? from, DateTime? to, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            StatsQuery.All.Contains(kind)
                ? await agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetStats, new StatsQuery(kind, from, to), ct)
                : Results.NotFound());

        owner.MapGet("/watchlist", (ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.GetWatchlist, null, ct));
        owner.MapPut("/watchlist", (Watchlist watchlist, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            watchlist.Patients.Count > Watchlist.MaxItems || watchlist.Medicaments.Count > Watchlist.MaxItems
                ? Task.FromResult(Results.Problem(statusCode: StatusCodes.Status400BadRequest,
                    title: $"Au plus {Watchlist.MaxItems} patients et {Watchlist.MaxItems} médicaments suivis."))
                : agent.InvokeAsync(user.GetPharmacyId(), AgentMethods.SaveWatchlist, watchlist, ct));
    }

    // Sums of money an employee's phone never gets. A single facture keeps its amounts: they are needed at the counter.
    public static readonly string[] PatientTotals = ["montantTotal"];
    public static readonly string[] ReminderTotals = ["montantMoyenHabituel"];
    public static readonly string[] MedicamentTotals = ["montant"];

    private static string[]? StaffHidden(ClaimsPrincipal user, string[]? fields) => user.IsStaff() ? fields : null;

    private static void MapPaged(RouteGroupBuilder api, string route, string method, string[]? staffHidden = null) =>
        api.MapGet(route, (DateTime? from, DateTime? to, int? skip, int? take, string? q, ClaimsPrincipal user, AgentInvoker agent, CancellationToken ct) =>
            agent.InvokeAsync(user.GetPharmacyId(), method, new PageQuery(from, to, skip ?? 0, take ?? PageQuery.DefaultTake, q), ct,
                StaffHidden(user, staffHidden)));

    private static async Task<IResult> StatusAsync(ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, AgentRegistry registry, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == pharmacyId, ct);

        return pharmacy is null
            ? Results.NotFound()
            : Results.Ok(new OfficineStatus(pharmacy.Name, registry.IsOnline(pharmacyId), pharmacy.LastSeenAt,
                AccountEndpoints.HasEnded(pharmacy, DateTime.UtcNow), AccountEndpoints.IsTrial(pharmacy)));
    }
}

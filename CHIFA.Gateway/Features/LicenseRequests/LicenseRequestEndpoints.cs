using System.Security.Claims;
using CHIFA.Gateway.Features.Account;
using CHIFA.Gateway.Features.Agents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CHIFA.Gateway.Features.LicenseRequests;

/// <summary>What the pharmacist fills in on the phone; the rest comes from the pharmacy's last license.</summary>
public record PhoneLicenseRequest(string FullName, string Phone, string? BusinessName = null, string? Wilaya = null, string? Message = null);

/// <summary>
/// The pharmacist asks for a license from the phone, for the pharmacy's computer (its machine id comes from the
/// last license SmartCHIFA sent), even while that computer is off or its trial has ended. SmartApp calls about the
/// payment and approves it; SmartCHIFA then picks up the serial here at its next start and activates itself.
/// </summary>
public static class LicenseRequestEndpoints
{
    private const string PhoneRoute = "/api/license-request";

    public static void MapLicenseRequestEndpoints(this IEndpointRouteBuilder app)
    {
        var phone = app.MapGroup(PhoneRoute).RequireAuthorization();
        phone.MapGet("", StateAsync);
        phone.MapPost("", SendAsync);

        app.MapGet(LinkRoutes.LicenseRequest, StateAsync)
            .RequireAuthorization(new AuthorizeAttribute { AuthenticationSchemes = AgentKeyAuthenticationHandler.SchemeName });
    }

    /// <summary>Only a trial or an ended license needs one: a running subscription is renewed, not requested again.</summary>
    public static bool CanRequest(Pharmacy pharmacy, DateTime utcNow) =>
        pharmacy.MachineId is not null && (AccountEndpoints.IsTrial(pharmacy) || AccountEndpoints.HasEnded(pharmacy, utcNow));

    /// <summary>The request is done once SmartCHIFA installed its new license, or when it no longer exists.</summary>
    public static void Forget(Pharmacy pharmacy)
    {
        pharmacy.LicenseRequestId = null;
        pharmacy.LicenseRequestToken = null;
        pharmacy.LicenseRequestedAt = null;
    }

    private static async Task<IResult> StateAsync(
        ClaimsPrincipal user, IDbContextFactory<GatewayDbContext> dbFactory, LicensingServer server, CancellationToken ct)
    {
        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.FirstOrDefaultAsync(p => p.Id == pharmacyId, ct);
        if (pharmacy is null)
            return Results.NotFound();

        var state = await ReadAsync(db, pharmacy, server, ct);
        return state is null ? Unreachable() : Results.Ok(state);
    }

    private static async Task<IResult> SendAsync(
        PhoneLicenseRequest request,
        ClaimsPrincipal user,
        IDbContextFactory<GatewayDbContext> dbFactory,
        LicensingServer server,
        IOptions<GatewayOptions> options,
        CancellationToken ct)
    {
        var fullName = request.FullName?.Trim();
        if (string.IsNullOrEmpty(fullName))
            return Invalid("Saisissez le nom et le prénom du pharmacien.");
        if ((request.Phone ?? "").Count(char.IsDigit) < 9)
            return Invalid("Saisissez un numéro de téléphone valide : SmartApp vous appelle pour le paiement.");

        var pharmacyId = user.GetPharmacyId();
        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.FirstOrDefaultAsync(p => p.Id == pharmacyId, ct);
        if (pharmacy is null)
            return Results.NotFound();
        if (!CanRequest(pharmacy, DateTime.UtcNow))
            return Results.Problem(statusCode: StatusCodes.Status409Conflict,
                title: pharmacy.MachineId is null ? "Demande impossible depuis le téléphone" : "Abonnement en cours",
                detail: pharmacy.MachineId is null
                    ? "Demandez la licence dans SmartCHIFA (menu « Licence ») ou contactez SmartApp."
                    : "Pour le renouveler, contactez SmartApp : votre numéro de série reste le même.");

        var message = string.IsNullOrWhiteSpace(request.Message)
            ? "Demande envoyée depuis l'application mobile."
            : $"Demande envoyée depuis l'application mobile. {request.Message.Trim()}";
        var (receipt, error) = await server.SubmitAsync(new LicensingSubmission(
            options.Value.LicenseProduct, pharmacy.MachineId!, fullName, Clean(request.BusinessName) ?? pharmacy.Name, request.Phone!.Trim(),
            Email: null, Clean(request.Wilaya), Address: null, CodePs: null, message, MachineName: null, AppVersion: null), ct);
        if (receipt is null)
            return error is null ? Unreachable() : Refused(error);

        // Sending again replaces the request, and its token, on the licensing server.
        pharmacy.LicenseRequestId = receipt.Id;
        pharmacy.LicenseRequestToken = receipt.Token;
        pharmacy.LicenseRequestedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Results.Ok(new LicenseRequestState(true, LicenseRequestStates.Pending, pharmacy.LicenseRequestedAt));
    }

    /// <returns><c>null</c> when the licensing server cannot be reached.</returns>
    private static async Task<LicenseRequestState?> ReadAsync(GatewayDbContext db, Pharmacy pharmacy, LicensingServer server, CancellationToken ct)
    {
        var canRequest = CanRequest(pharmacy, DateTime.UtcNow);
        if (pharmacy is not { LicenseRequestId: { } id, LicenseRequestToken: { } token, MachineId: { } machineId })
            return new LicenseRequestState(canRequest, LicenseRequestStates.None);

        var (status, error) = await server.StatusAsync(new LicensingStatusQuery(id, token, machineId), ct);
        var sentAt = pharmacy.LicenseRequestedAt;
        switch (status?.Status)
        {
            case LicenseRequestStates.Pending:
                return new LicenseRequestState(canRequest, LicenseRequestStates.Pending, sentAt);
            case LicenseRequestStates.Approved:
                return new LicenseRequestState(canRequest, LicenseRequestStates.Approved, sentAt, status.Serial);
            case LicenseRequestStates.Rejected:
                Forget(pharmacy);
                await db.SaveChangesAsync(ct);
                return new LicenseRequestState(canRequest, LicenseRequestStates.Rejected, sentAt, Message: status.Message);
        }

        // Sent again from SmartCHIFA (which replaces the token) or deleted: the phone may ask again.
        if (error?.Code != LicensingError.RequestNotFound)
            return null;
        Forget(pharmacy);
        await db.SaveChangesAsync(ct);
        return new LicenseRequestState(canRequest, LicenseRequestStates.None);
    }

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static IResult Invalid(string title) =>
        Results.Problem(statusCode: StatusCodes.Status400BadRequest, title: title);

    private static IResult Refused(LicensingError error) =>
        Results.Problem(statusCode: StatusCodes.Status424FailedDependency,
            title: "Demande refusée par le serveur de licences",
            detail: error.Message ?? "Réessayez plus tard ou contactez SmartApp.");

    private static IResult Unreachable() =>
        Results.Problem(statusCode: StatusCodes.Status503ServiceUnavailable,
            title: "Serveur de licences injoignable", detail: "Réessayez dans quelques minutes.");
}

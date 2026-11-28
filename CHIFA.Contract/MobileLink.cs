namespace CHIFA.Contract;

/// <summary>
/// How CHIFA Pro links phones to its pharmacy: it registers with its signed license, then shows
/// one-time pairing codes that a phone exchanges for a lasting device token.
/// </summary>
public static class LinkRoutes
{
    /// <summary>Anonymous: the signed license file is the proof.</summary>
    public const string Register = "/api/link/register";

    /// <summary>Called with the agent key.</summary>
    public const string Codes = "/api/link/codes";

    /// <summary>Called with the agent key. <c>PUT {Devices}/{id}/role</c> changes what a phone may see.</summary>
    public const string Devices = "/api/link/devices";

    /// <summary>Called with the agent key: the current license file, so the phone shows the subscription's end.</summary>
    public const string License = "/api/link/license";

    /// <summary>Called with the agent key: the license request sent from a phone, with its serial once approved.</summary>
    public const string LicenseRequest = "/api/link/license-request";
}

/// <summary>A license request sent from a phone for the pharmacy's computer.</summary>
/// <param name="CanRequest">The gateway knows the computer (from its last license), so a phone can ask for it.</param>
/// <param name="Status">One of <see cref="LicenseRequestStates"/>.</param>
/// <param name="Serial">Once approved: SmartCHIFA activates it, and the phone shows it in case it must be typed.</param>
/// <param name="Message">Why SmartApp rejected it.</param>
/// <param name="Contact">The pharmacist's details from the trial or the last request: the phone sends them again in one tap.</param>
public record LicenseRequestState(
    bool CanRequest, string Status, DateTime? SentAt = null, string? Serial = null, string? Message = null, PharmacistContact? Contact = null);

/// <summary>Who SmartApp calls about the payment: what the pharmacist typed for the trial or a license request.</summary>
/// <param name="UpdatedAt">When the pharmacist typed them (UTC); <c>null</c> when SmartCHIFA only read its own settings,
/// which then never replace details typed somewhere.</param>
/// <param name="IdentityFromChifa">The name, address and code PS come from the CHIFA database: they cannot be changed
/// from a phone, and SmartCHIFA always sends the current ones.</param>
public record PharmacistContact(
    string FullName,
    string Phone,
    string? BusinessName = null,
    string? Email = null,
    string? Wilaya = null,
    string? Address = null,
    string? CodePs = null,
    DateTime? UpdatedAt = null,
    bool IdentityFromChifa = false);

public static class LicenseRequestStates
{
    public const string None = "none";
    public const string Pending = "pending";
    public const string Approved = "approved";
    public const string Rejected = "rejected";
}

/// <param name="License">Content of the SmartApp license file (<c>license.lic</c>) of this computer.</param>
/// <param name="Contact">The pharmacist's details, so a phone can ask for the license without typing them again.</param>
public record RegisterPharmacyRequest(string License, PharmacistContact? Contact = null);

/// <param name="License">Content of <c>license.lic</c> after an online check-in (renewal, new edition).</param>
/// <param name="Contact">Sent again when it changed.</param>
public record LicenseUpdateRequest(string License, PharmacistContact? Contact = null);

/// <summary>The agent key is shown once: CHIFA Pro keeps it to run the agent and ask for codes.</summary>
public record RegisterPharmacyResponse(Guid PharmacyId, string Name, string AgentKey);

/// <summary>
/// What a linked phone may see. The pharmacist's own phone sees everything; an employee's phone sees the patients,
/// médicaments, factures and psychotropes, but never the turnover, margins, bordereaux or payments.
/// </summary>
public static class DeviceRoles
{
    public const string Owner = "owner";
    public const string Staff = "staff";

    public static bool IsValid(string? role) => role is Owner or Staff;

    public static string Label(string? role) => role == Staff ? "Employé" : "Titulaire";
}

/// <param name="Role">One of <see cref="DeviceRoles"/>; <c>null</c> (older CHIFA Pro) links the pharmacist's own phone.</param>
public record PairingCodeRequest(string? Role = null);

/// <param name="Code">Eight characters shown as <c>ABCD-2345</c>.</param>
public record PairingCodeResponse(string Code, DateTime ExpiresAt, bool AgentOnline, string Role = DeviceRoles.Owner);

public record LinkedDevice(Guid Id, string Name, DateTime CreatedAt, DateTime? LastSeenAt, string Role = DeviceRoles.Owner);

public record DeviceRoleRequest(string Role);

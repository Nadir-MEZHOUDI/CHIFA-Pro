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

    /// <summary>Called with the agent key.</summary>
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
public record LicenseRequestState(bool CanRequest, string Status, DateTime? SentAt = null, string? Serial = null, string? Message = null);

public static class LicenseRequestStates
{
    public const string None = "none";
    public const string Pending = "pending";
    public const string Approved = "approved";
    public const string Rejected = "rejected";
}

/// <param name="License">Content of the SmartApp license file (<c>license.lic</c>) of this computer.</param>
public record RegisterPharmacyRequest(string License);

/// <param name="License">Content of <c>license.lic</c> after an online check-in (renewal, new edition).</param>
public record LicenseUpdateRequest(string License);

/// <summary>The agent key is shown once: CHIFA Pro keeps it to run the agent and ask for codes.</summary>
public record RegisterPharmacyResponse(Guid PharmacyId, string Name, string AgentKey);

/// <param name="Code">Eight characters shown as <c>ABCD-2345</c>.</param>
public record PairingCodeResponse(string Code, DateTime ExpiresAt, bool AgentOnline);

public record LinkedDevice(Guid Id, string Name, DateTime CreatedAt, DateTime? LastSeenAt);

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
}

/// <param name="License">Content of the SmartApp license file (<c>license.lic</c>) of this computer.</param>
public record RegisterPharmacyRequest(string License);

/// <summary>The agent key is shown once: CHIFA Pro keeps it to run the agent and ask for codes.</summary>
public record RegisterPharmacyResponse(Guid PharmacyId, string Name, string AgentKey);

/// <param name="Code">Eight characters shown as <c>ABCD-2345</c>.</param>
public record PairingCodeResponse(string Code, DateTime ExpiresAt, bool AgentOnline);

public record LinkedDevice(Guid Id, string Name, DateTime CreatedAt, DateTime? LastSeenAt);

namespace CHIFA.Gateway.Data;

public class Pharmacy
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Name { get; set; }

    /// <summary>The SmartApp license of the CHIFA Pro computer that registered this pharmacy, if it did.</summary>
    public Guid? LicenseId { get; set; }

    /// <summary>End of the last signed license CHIFA Pro sent; <c>null</c> once sent means it never expires.</summary>
    public DateTime? LicenseExpiresAt { get; set; }

    /// <summary>Edition of that license, e.g. <c>standard</c> or <c>trial</c>.</summary>
    public string? LicenseEdition { get; set; }

    /// <summary>When CHIFA Pro last sent its license. <c>null</c> when it never did (added by the admin, or an older SmartCHIFA).</summary>
    public DateTime? LicenseCheckedAt { get; set; }

    /// <summary>The computer that license is bound to (hashed by SmartApp): a phone can ask for a license for it.</summary>
    public string? MachineId { get; set; }

    /// <summary>License request sent from a phone; its token is the only way to read its status.</summary>
    public Guid? LicenseRequestId { get; set; }

    public string? LicenseRequestToken { get; set; }

    public DateTime? LicenseRequestedAt { get; set; }

    /// <summary>
    /// The pharmacist's details from the trial or the last license request (sent by SmartCHIFA or typed on a
    /// phone): a phone asks for the license without typing them again.
    /// </summary>
    public string? ContactName { get; set; }

    public string? ContactPhone { get; set; }
    public string? ContactBusinessName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactWilaya { get; set; }
    public string? ContactAddress { get; set; }
    public string? ContactCodePs { get; set; }

    /// <summary>When the pharmacist typed them: newer details replace older ones, wherever they were typed.</summary>
    public DateTime? ContactUpdatedAt { get; set; }

    /// <summary>SHA-256 of the agent key. The key itself is shown once, when created.</summary>
    public required string AgentKeyHash { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Last time the agent connected or disconnected.</summary>
    public DateTime? LastSeenAt { get; set; }

    public List<MobileUser> Users { get; set; } = [];
    public List<MobileDevice> Devices { get; set; } = [];
}

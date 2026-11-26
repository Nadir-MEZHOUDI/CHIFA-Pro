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

    /// <summary>SHA-256 of the agent key. The key itself is shown once, when created.</summary>
    public required string AgentKeyHash { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Last time the agent connected or disconnected.</summary>
    public DateTime? LastSeenAt { get; set; }

    public List<MobileUser> Users { get; set; } = [];
    public List<MobileDevice> Devices { get; set; } = [];
}

namespace CHIFA.Gateway.Data;

public class Pharmacy
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public required string Name { get; set; }

    /// <summary>The SmartApp license of the CHIFA Pro computer that registered this pharmacy, if it did.</summary>
    public Guid? LicenseId { get; set; }

    /// <summary>SHA-256 of the agent key. The key itself is shown once, when created.</summary>
    public required string AgentKeyHash { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Last time the agent connected or disconnected.</summary>
    public DateTime? LastSeenAt { get; set; }

    public List<MobileUser> Users { get; set; } = [];
    public List<MobileDevice> Devices { get; set; } = [];
}

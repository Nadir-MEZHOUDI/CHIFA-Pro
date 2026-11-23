namespace CHIFA.Gateway.Data;

/// <summary>A phone linked with a pairing code. It signs in with its device token until it logs out or is removed.</summary>
public class MobileDevice
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = null!;

    public required string Name { get; set; }

    /// <summary>SHA-256 of the device token. The token itself only lives on the phone.</summary>
    public required string TokenHash { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastSeenAt { get; set; }

    /// <summary>Set on logout or when CHIFA Pro removes the phone. A revoked device cannot sign in again.</summary>
    public DateTime? RevokedAt { get; set; }
}

/// <summary>A one-time code shown in CHIFA Pro. Only one is pending per pharmacy.</summary>
public class PairingCode
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = null!;

    public required string CodeHash { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

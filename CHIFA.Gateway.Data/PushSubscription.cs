namespace CHIFA.Gateway.Data;

/// <summary>A browser that accepted notifications (Web Push). The endpoint is unique per browser install.</summary>
public class PushSubscription
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = null!;

    /// <summary>The linked phone that subscribed, if it signed in with a pairing code. Its revocation stops the notifications.</summary>
    public Guid? DeviceId { get; set; }
    public MobileDevice? Device { get; set; }

    public required string Endpoint { get; set; }
    public required string P256dh { get; set; }
    public required string Auth { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>The gateway's VAPID key pair (base64url), created on first start. One row.</summary>
public class VapidKey
{
    public int Id { get; set; }
    public required string PublicKey { get; set; }
    public required string PrivateKey { get; set; }
}

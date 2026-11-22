namespace CHIFA.Gateway.Data;

public class MobileUser
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid PharmacyId { get; set; }
    public Pharmacy Pharmacy { get; set; } = null!;

    /// <summary>Stored lower-case, unique across all pharmacies.</summary>
    public required string UserName { get; set; }

    public required string PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

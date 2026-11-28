using Microsoft.EntityFrameworkCore;

namespace CHIFA.Gateway.Data;

public class GatewayDbContext(DbContextOptions<GatewayDbContext> options) : DbContext(options)
{
    public DbSet<Pharmacy> Pharmacies => Set<Pharmacy>();
    public DbSet<MobileUser> MobileUsers => Set<MobileUser>();
    public DbSet<MobileDevice> MobileDevices => Set<MobileDevice>();
    public DbSet<PairingCode> PairingCodes => Set<PairingCode>();
    public DbSet<PushSubscription> PushSubscriptions => Set<PushSubscription>();
    public DbSet<VapidKey> VapidKeys => Set<VapidKey>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<DeviceActivity> DeviceActivities => Set<DeviceActivity>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Pharmacy>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(200);
            e.Property(p => p.AgentKeyHash).HasMaxLength(64);
            e.Property(p => p.LicenseEdition).HasMaxLength(50);
            e.Property(p => p.MachineId).HasMaxLength(128);
            e.Property(p => p.LicenseRequestToken).HasMaxLength(100);
            // Same limits as the licensing server's license requests.
            e.Property(p => p.ContactName).HasMaxLength(160);
            e.Property(p => p.ContactPhone).HasMaxLength(40);
            e.Property(p => p.ContactBusinessName).HasMaxLength(160);
            e.Property(p => p.ContactEmail).HasMaxLength(256);
            e.Property(p => p.ContactWilaya).HasMaxLength(64);
            e.Property(p => p.ContactAddress).HasMaxLength(256);
            e.Property(p => p.ContactCodePs).HasMaxLength(64);
            e.HasIndex(p => p.AgentKeyHash).IsUnique();
            e.HasIndex(p => p.LicenseId).IsUnique();
            // Looked up when a new license (the paid one after the trial) registers from the same computer.
            e.HasIndex(p => p.MachineId);
        });

        b.Entity<MobileUser>(e =>
        {
            e.Property(u => u.UserName).HasMaxLength(100);
            e.HasIndex(u => u.UserName).IsUnique();
        });

        b.Entity<MobileDevice>(e =>
        {
            e.Property(d => d.Name).HasMaxLength(100);
            e.Property(d => d.TokenHash).HasMaxLength(64);
            e.Property(d => d.Role).HasMaxLength(20).HasDefaultValue(DeviceRoles.Owner);
            e.HasIndex(d => d.TokenHash).IsUnique();
        });

        b.Entity<PairingCode>(e =>
        {
            e.Property(c => c.CodeHash).HasMaxLength(64);
            e.Property(c => c.Role).HasMaxLength(20).HasDefaultValue(DeviceRoles.Owner);
            e.HasIndex(c => c.CodeHash).IsUnique();
        });

        b.Entity<PushSubscription>(e =>
        {
            e.Property(s => s.Endpoint).HasMaxLength(1000);
            e.Property(s => s.P256dh).HasMaxLength(200);
            e.Property(s => s.Auth).HasMaxLength(100);
            e.HasIndex(s => s.Endpoint).IsUnique();
            e.HasOne(s => s.Device).WithMany().OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<VapidKey>(e =>
        {
            e.Property(k => k.Id).ValueGeneratedNever();
            e.Property(k => k.PublicKey).HasMaxLength(100);
            e.Property(k => k.PrivateKey).HasMaxLength(100);
        });

        b.Entity<Review>(e =>
        {
            e.Property(r => r.Comment).HasMaxLength(Review.MaxCommentLength);
            e.Property(r => r.AuthorName).HasMaxLength(Review.MaxNameLength);
            e.Property(r => r.City).HasMaxLength(Review.MaxNameLength);
            e.HasIndex(r => r.PharmacyId).IsUnique();
            e.HasOne(r => r.Pharmacy).WithMany().OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<DeviceActivity>(e =>
        {
            e.HasKey(a => new { a.DeviceId, a.Day });
            e.HasIndex(a => a.Day);
            e.HasIndex(a => new { a.PharmacyId, a.Day });
            e.HasOne(a => a.Device).WithMany().OnDelete(DeleteBehavior.Cascade);
        });
    }
}

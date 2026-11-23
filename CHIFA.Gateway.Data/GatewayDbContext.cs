using Microsoft.EntityFrameworkCore;

namespace CHIFA.Gateway.Data;

public class GatewayDbContext(DbContextOptions<GatewayDbContext> options) : DbContext(options)
{
    public DbSet<Pharmacy> Pharmacies => Set<Pharmacy>();
    public DbSet<MobileUser> MobileUsers => Set<MobileUser>();
    public DbSet<MobileDevice> MobileDevices => Set<MobileDevice>();
    public DbSet<PairingCode> PairingCodes => Set<PairingCode>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Pharmacy>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(200);
            e.Property(p => p.AgentKeyHash).HasMaxLength(64);
            e.HasIndex(p => p.AgentKeyHash).IsUnique();
            e.HasIndex(p => p.LicenseId).IsUnique();
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
            e.HasIndex(d => d.TokenHash).IsUnique();
        });

        b.Entity<PairingCode>(e =>
        {
            e.Property(c => c.CodeHash).HasMaxLength(64);
            e.HasIndex(c => c.CodeHash).IsUnique();
        });
    }
}

using Microsoft.EntityFrameworkCore;

namespace CHIFA.Gateway.Data;

public class GatewayDbContext(DbContextOptions<GatewayDbContext> options) : DbContext(options)
{
    public DbSet<Pharmacy> Pharmacies => Set<Pharmacy>();
    public DbSet<MobileUser> MobileUsers => Set<MobileUser>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Pharmacy>(e =>
        {
            e.Property(p => p.Name).HasMaxLength(200);
            e.Property(p => p.AgentKeyHash).HasMaxLength(64);
            e.HasIndex(p => p.AgentKeyHash).IsUnique();
        });

        b.Entity<MobileUser>(e =>
        {
            e.Property(u => u.UserName).HasMaxLength(100);
            e.HasIndex(u => u.UserName).IsUnique();
        });
    }
}

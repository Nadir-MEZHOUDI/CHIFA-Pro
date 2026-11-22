using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CHIFA.Gateway.Data;

/// <summary>Used only by <c>dotnet ef</c> to create migrations.</summary>
public class GatewayDbContextFactory : IDesignTimeDbContextFactory<GatewayDbContext>
{
    public GatewayDbContext CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder<GatewayDbContext>()
            .UseNpgsql("Host=localhost;Database=chifa_gateway")
            .UseSnakeCaseNamingConvention()
            .Options);
}

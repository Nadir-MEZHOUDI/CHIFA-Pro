using CHIFA.Services.Helpers;
using Npgsql;

namespace CHIFA.Tests.Helpers;

public class DatabaseDefaultsTests
{
    private const string ChifaDefault = "Server=localhost; Port=5432; User Id=pharm; Password=secret; Database=CHIFA_OFFICINE;";

    [Fact]
    public void ForDesktop_BoundsThePoolAndReleasesIdleConnectionsSooner()
    {
        var tuned = new NpgsqlConnectionStringBuilder(DatabaseDefaults.ForDesktop(ChifaDefault));

        tuned.MaxPoolSize.Should().Be(DatabaseDefaults.DesktopMaxPoolSize);
        tuned.ConnectionIdleLifetime.Should().Be(DatabaseDefaults.DesktopConnectionIdleLifetimeSeconds);
        tuned.CommandTimeout.Should().Be(DatabaseDefaults.DesktopCommandTimeoutSeconds);
    }

    [Fact]
    public void ForDesktop_KeepsTheConnectionTarget()
    {
        var tuned = new NpgsqlConnectionStringBuilder(DatabaseDefaults.ForDesktop(ChifaDefault));

        tuned.Host.Should().Be("localhost");
        tuned.Port.Should().Be(5432);
        tuned.Username.Should().Be("pharm");
        tuned.Database.Should().Be("CHIFA_OFFICINE");
    }

    [Fact]
    public void ForDesktop_KeepsValuesSetExplicitly()
    {
        var custom = new NpgsqlConnectionStringBuilder(ChifaDefault) { MaxPoolSize = 5, CommandTimeout = 300, ConnectionIdleLifetime = 10 };

        var tuned = new NpgsqlConnectionStringBuilder(DatabaseDefaults.ForDesktop(custom.ConnectionString));

        tuned.MaxPoolSize.Should().Be(5);
        tuned.CommandTimeout.Should().Be(300);
        tuned.ConnectionIdleLifetime.Should().Be(10);
    }

    [Fact]
    public void ForDesktop_IsIdempotent()
    {
        var once = DatabaseDefaults.ForDesktop(ChifaDefault);

        DatabaseDefaults.ForDesktop(once).Should().Be(once);
    }
}

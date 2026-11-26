using Npgsql;

namespace CHIFA.Services.Helpers;

/// <summary>
/// Connection defaults shared by the application, its tests and its PostgreSQL tooling.
/// </summary>
public static class DatabaseDefaults
{
    /// <summary>Default PostgreSQL port used when no explicit port is configured.</summary>
    public const int Port = 5432;

    /// <summary>
    /// A desktop needs a handful of connections, but Npgsql's default pool allows 100 and keeps idle ones
    /// for 5 minutes. Several workstations plus CHIFA itself share one server (max_connections is often 100),
    /// so a burst of loads on one PC could leave the others with "53300: too many clients".
    /// </summary>
    public const int DesktopMaxPoolSize = 20;

    public const int DesktopConnectionIdleLifetimeSeconds = 60;

    /// <summary>Statistics over several years can take more than Npgsql's default 30 s on a slow PC.</summary>
    public const int DesktopCommandTimeoutSeconds = 120;

    /// <summary>
    /// Applies the desktop pool and timeout settings, keeping any value the connection string already sets
    /// to something other than Npgsql's default.
    /// </summary>
    public static string ForDesktop(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var defaults = new NpgsqlConnectionStringBuilder();

        if (builder.MaxPoolSize == defaults.MaxPoolSize)
            builder.MaxPoolSize = DesktopMaxPoolSize;
        if (builder.ConnectionIdleLifetime == defaults.ConnectionIdleLifetime)
            builder.ConnectionIdleLifetime = DesktopConnectionIdleLifetimeSeconds;
        if (builder.CommandTimeout == defaults.CommandTimeout)
            builder.CommandTimeout = DesktopCommandTimeoutSeconds;

        return builder.ConnectionString;
    }
}

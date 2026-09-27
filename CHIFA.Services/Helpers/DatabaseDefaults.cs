namespace CHIFA.Services.Helpers;

/// <summary>
/// Connection defaults shared by the application, its tests and its PostgreSQL tooling.
/// </summary>
public static class DatabaseDefaults
{
    /// <summary>Default PostgreSQL port used when no explicit port is configured.</summary>
    public const int Port = 5432;
}

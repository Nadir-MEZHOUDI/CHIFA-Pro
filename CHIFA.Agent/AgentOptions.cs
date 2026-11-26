using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace CHIFA.Agent;

public class AgentOptions
{
    public const string Section = "Agent";

    /// <summary>
    /// Written by SmartCHIFA when the pharmacy links its phones (« Application mobile »). Holds the
    /// <c>Agent</c> section, so the Windows service uses the same key as the desktop app.
    /// </summary>
    public static string SharedSettingsPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SmartCHIFA", "mobile.json");

    /// <summary>Base address of the gateway, e.g. <c>https://chifa.example.com</c>.</summary>
    [Required, Url]
    public string GatewayUrl { get; set; } = string.Empty;

    /// <summary>The key given by <c>POST /admin/pharmacies</c>. It identifies this pharmacy.</summary>
    [Required]
    public string AgentKey { get; set; } = string.Empty;
}

/// <summary>Agent options set in code rather than read from configuration: SmartCHIFA hosting the agent itself, tests.</summary>
public sealed class FixedOptionsMonitor<T>(T value) : IOptionsMonitor<T>
{
    public T CurrentValue => value;

    public T Get(string? name) => value;

    public IDisposable? OnChange(Action<T, string?> listener) => null;
}

/// <summary>
/// Where the local CHIFA database is. A Windows service runs as LocalSystem and does not see the
/// <c>CHIFA_OFFICINE_SERVER</c> user variable CHIFA.Pro relies on, so it is set here.
/// </summary>
public class ChifaDbOptions
{
    public const string Section = "Chifa";

    /// <summary>Full Npgsql connection string. When empty, the CHIFA default is used with <see cref="Host"/> and <see cref="Port"/>.</summary>
    public string? ConnectionString { get; set; }

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5432;

    /// <summary>
    /// The CHIFA_OFFICINE folder (holding <c>CHIFA_OFFICINE_DB</c>), given by SmartCHIFA. When set and the
    /// database is on this PC, the service starts CHIFA_OFFICINE's PostgreSQL if it finds it stopped.
    /// </summary>
    public string? Folder { get; set; }
}

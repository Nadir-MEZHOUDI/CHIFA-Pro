using System.ComponentModel.DataAnnotations;

namespace CHIFA.Gateway;

public class GatewayOptions
{
    public const string Section = "Gateway";

    /// <summary>Sent in the <c>X-Admin-Key</c> header to manage pharmacies and users.</summary>
    [Required, MinLength(32)]
    public string AdminKey { get; set; } = string.Empty;

    /// <summary>How long a phone request waits for the agent before giving up.</summary>
    [Range(1, 300)]
    public int AgentTimeoutSeconds { get; set; } = 30;
}

public class JwtOptions
{
    public const string Section = "Jwt";

    [Required, MinLength(32)]
    public string SigningKey { get; set; } = string.Empty;

    public string Issuer { get; set; } = "chifa-gateway";
    public string Audience { get; set; } = "chifa-mobile";

    [Range(1, 24 * 30)]
    public int LifetimeHours { get; set; } = 12;
}

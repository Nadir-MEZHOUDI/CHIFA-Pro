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

    /// <summary>SmartApp licensing public key: verifies the license CHIFA Pro sends to register its pharmacy.</summary>
    [Required]
    public string LicensePublicKey { get; set; } = """
        -----BEGIN PUBLIC KEY-----
        MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAEPckUvTCNoSzE+EQrTlAb9qaxnTiA
        l9GJ/e8oSV/rMmErXqmPAiyeKemgKYDL7+BV6o3umXv6GJJxn0yyGPyVQg==
        -----END PUBLIC KEY-----
        """;

    /// <summary>Product slug the license must be issued for.</summary>
    [Required]
    public string LicenseProduct { get; set; } = "chifa-pro";

    [Range(1, 60)]
    public int PairingCodeMinutes { get; set; } = 10;

    /// <summary>Sign-in, pairing and registration attempts allowed per IP address and minute.</summary>
    [Range(1, 10_000)]
    public int AuthRequestsPerMinute { get; set; } = 10;
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

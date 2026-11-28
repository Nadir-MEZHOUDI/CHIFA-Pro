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

    /// <summary>SmartApp licensing server: phones send their license requests to it through the gateway.</summary>
    [Required, Url]
    public string LicensingUrl { get; set; } = "https://license.smartappdz.org/";

    [Range(1, 60)]
    public int PairingCodeMinutes { get; set; } = 10;

    /// <summary>Sign-in, pairing and registration attempts allowed per IP address and minute.</summary>
    [Range(1, 10_000)]
    public int AuthRequestsPerMinute { get; set; } = 10;

    /// <summary>How often online agents are asked for new alerts to push to phones.</summary>
    [Range(1, 1440)]
    public int AlertPollMinutes { get; set; } = 5;

    /// <summary>Phones are told the pharmacy is back online only after it was offline at least this long.</summary>
    [Range(1, 1440)]
    public int BackOnlineMinOfflineMinutes { get; set; } = 5;

    /// <summary>At most one "back online" notification per pharmacy in this time.</summary>
    [Range(1, 1440)]
    public int BackOnlineCooldownMinutes { get; set; } = 60;

    /// <summary>VAPID contact given to the browsers' push services (a mailto: or https: URL).</summary>
    [Required]
    public string PushSubject { get; set; } = "https://smartchifa.smartappdz.org";

    /// <summary>The landing page allowed to read the published reviews from the browser (CORS).</summary>
    [Required]
    public string WebsiteOrigin { get; set; } = "https://smartchifa.smartappdz.org";
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

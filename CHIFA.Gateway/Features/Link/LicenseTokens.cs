using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CHIFA.Gateway.Features.Link;

public sealed record VerifiedLicense(Guid LicenseId, string? Customer);

/// <summary>
/// Checks a SmartApp license file (<c>SL1.&lt;payload&gt;.&lt;signature&gt;</c>, ECDSA P-256 / SHA-256)
/// with the licensing server's public key. Only the licensing server can sign one, so a valid file
/// proves the caller is an activated CHIFA Pro computer.
/// </summary>
public static class LicenseTokens
{
    private const string Prefix = "SL1.";
    private const int FormatVersion = 1;

    /// <returns>The license, or <c>null</c> with a French reason.</returns>
    public static VerifiedLicense? Verify(string? token, string publicKeyPem, string product, DateTimeOffset now, out string error)
    {
        error = "Licence invalide.";
        token = token?.Trim();
        if (string.IsNullOrEmpty(token) || !token.StartsWith(Prefix, StringComparison.Ordinal) || token.Length > 16 * 1024)
            return null;

        var separator = token.LastIndexOf('.');
        if (separator <= Prefix.Length)
            return null;

        var signingInput = token[..separator];
        Payload? payload;
        try
        {
            using var key = ECDsa.Create();
            key.ImportFromPem(publicKeyPem);
            var signature = Base64Url.DecodeFromChars(token.AsSpan(separator + 1));
            if (!key.VerifyData(Encoding.ASCII.GetBytes(signingInput), signature, HashAlgorithmName.SHA256))
                return null;

            payload = JsonSerializer.Deserialize<Payload>(Base64Url.DecodeFromChars(signingInput.AsSpan(Prefix.Length)), JsonSerializerOptions.Web);
        }
        catch (Exception ex) when (ex is FormatException or JsonException or CryptographicException)
        {
            return null;
        }

        if (payload is null || payload.FormatVersion != FormatVersion || payload.LicenseId == Guid.Empty)
            return null;

        if (!string.Equals(payload.Product, product, StringComparison.OrdinalIgnoreCase))
        {
            error = "Cette licence n'est pas une licence SmartCHIFA.";
            return null;
        }

        // OfflineUntil is renewed each time CHIFA Pro checks in, so it also catches revoked licenses within days.
        if (payload.ExpiresAt <= now || payload.OfflineUntil <= now)
        {
            error = "Licence expirée. Vérifiez la licence en ligne dans SmartCHIFA puis réessayez.";
            return null;
        }

        error = "";
        return new VerifiedLicense(payload.LicenseId, payload.Customer);
    }

    private sealed record Payload(
        int FormatVersion,
        Guid LicenseId,
        string? Product,
        string? Customer,
        DateTimeOffset? ExpiresAt,
        DateTimeOffset OfflineUntil);
}

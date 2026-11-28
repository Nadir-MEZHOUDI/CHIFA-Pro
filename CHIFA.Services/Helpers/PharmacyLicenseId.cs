using System.Security.Cryptography;
using System.Text;

namespace CHIFA.Services.Helpers;

/// <summary>
/// The id the SmartCHIFA license is bound to: derived from the pharmacy's code PS, so every computer on the same
/// CHIFA database shares one serial and a Windows reinstall keeps it. The "site-" prefix tells the licensing server
/// it is shared by a whole site. Never change the formula: every activated pharmacy is bound to its result.
/// </summary>
public static class PharmacyLicenseId
{
    public const string Prefix = "site-";

    /// <returns><c>null</c> when the code PS is empty.</returns>
    public static string? FromCodePs(string? codePs)
    {
        var code = new string((codePs ?? string.Empty).Where(c => !char.IsWhiteSpace(c)).ToArray()).ToUpperInvariant();
        if (code.Length == 0) return null;

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes($"smartapp-licensing:chifa-pro:codeps:{code}"));
        return Prefix + Convert.ToHexString(hash, 0, 16).ToLowerInvariant();
    }

    public static bool IsPharmacy(string? machineId) => machineId?.StartsWith(Prefix, StringComparison.Ordinal) == true;
}

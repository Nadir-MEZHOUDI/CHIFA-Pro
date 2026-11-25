using System.Security.Cryptography;
using CHIFA.Pro.Helpers.Settings;

namespace CHIFA.Pro.Helpers;

public enum ProtectedItem
{
    Statistics,
    Mobile,
    License,
    Bordereaux,
    HomeFactures,
    HomeMontant,
    HomeMarge
}

/// <summary>Administrator session: while logged out, the items the administrator marked as protected are hidden.</summary>
internal static class AccessGuard
{
    public const string DefaultPassword = "0000";
    private const int Iterations = 100_000;

    public static bool IsAdmin { get; private set; }

    public static event Action? Changed;

    public static bool IsDefaultPassword => string.IsNullOrEmpty(SecuritySettings.Default.PasswordHash);

    public static bool IsHidden(ProtectedItem item) => !IsAdmin && SecuritySettings.Default.IsHidden(item);

    public static ProtectedItem? ItemFor(Type viewType) =>
        viewType == typeof(StatisticsUc) ? ProtectedItem.Statistics
        : viewType == typeof(BordereauxUc) ? ProtectedItem.Bordereaux
        : null;

    public static bool Login(string password)
    {
        if (!Verify(password)) return false;
        IsAdmin = true;
        Changed?.Invoke();
        return true;
    }

    public static void Logout()
    {
        IsAdmin = false;
        Changed?.Invoke();
    }

    /// <summary>True when <paramref name="item"/> is visible, asking for the administrator password if needed.</summary>
    public static bool EnsureAccess(ProtectedItem item, IWin32Window? owner)
    {
        if (!IsHidden(item)) return true;
        using var form = new LoginForm();
        form.ShowDialog(owner);
        return !IsHidden(item);
    }

    public static void ChangePassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, 32);
        SecuritySettings.Default.PasswordHash = $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
        SecuritySettings.Default.Save();
    }

    public static void SaveSettings()
    {
        SecuritySettings.Default.Save();
        Changed?.Invoke();
    }

    private static bool Verify(string password)
    {
        var stored = SecuritySettings.Default.PasswordHash;
        if (string.IsNullOrEmpty(stored))
            return password == DefaultPassword;

        var parts = stored.Split(':');
        if (parts.Length != 2) return false;
        try
        {
            var salt = Convert.FromBase64String(parts[0]);
            var expected = Convert.FromBase64String(parts[1]);
            var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, expected.Length);
            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}

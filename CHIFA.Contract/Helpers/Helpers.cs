using System.Security.Cryptography;
using System.Text;

namespace CHIFA.Contract.Helpers;

public static class Helpers
{
    public static string GetHash(this string input)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}
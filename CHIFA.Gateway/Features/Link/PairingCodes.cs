using System.Security.Cryptography;
using CHIFA.Gateway.Features.Agents;

namespace CHIFA.Gateway.Features.Link;

/// <summary>
/// Eight characters without the look-alikes 0/O, 1/I/L, shown as <c>ABCD-2345</c>: easy to read on a
/// screen and type on a phone. 31^8 combinations, single use, a few minutes, rate-limited.
/// </summary>
public static class PairingCodes
{
    private const string Alphabet = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
    private const int Length = 8;

    public static string Generate()
    {
        var chars = new char[Length];
        for (var i = 0; i < Length; i++)
            chars[i] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        return new string(chars);
    }

    public static string Display(string code) => $"{code[..4]}-{code[4..]}";

    /// <summary>Accepts what people type: any case, with or without the dash or spaces.</summary>
    public static string? Normalize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var code = new string(input.Where(c => c is not ('-' or ' ')).Select(char.ToUpperInvariant).ToArray());
        return code.Length == Length && code.All(Alphabet.Contains) ? code : null;
    }

    public static string Hash(string normalizedCode) => AgentKeys.Hash(normalizedCode);
}

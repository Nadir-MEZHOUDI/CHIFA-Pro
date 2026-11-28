using System.Text;
using System.Text.Json.Serialization;

namespace CHIFA.Contract;

/// <summary>
/// The pharmacist's own notes on a bénéficiaire (phones, note, favourite). Kept on the pharmacy computer by the
/// agent, never in the CNAS CHIFA database.
/// </summary>
/// <param name="Nom">Name when saved, to list favourites without asking the CHIFA database.</param>
/// <param name="Telephone1">National digits (<c>0661234567</c>) once saved; see <see cref="AlgerianPhone"/>.</param>
public record PatientContact(
    string NumAssure,
    string Rang,
    string? Nom = null,
    string? Telephone1 = null,
    string? Telephone2 = null,
    string? Note = null,
    bool Favori = false)
{
    public const int MaxNoteLength = 1000;
    public const int MaxContacts = 5000;

    [JsonIgnore]
    public bool IsEmpty => Telephone1 is null && Telephone2 is null && string.IsNullOrWhiteSpace(Note) && !Favori;

    /// <summary>What is wrong with the contact, in French, or null when it can be saved.</summary>
    public string? Problem()
    {
        if (string.IsNullOrWhiteSpace(NumAssure) || string.IsNullOrWhiteSpace(Rang))
            return "Patient inconnu.";
        if (!string.IsNullOrWhiteSpace(Telephone1) && !AlgerianPhone.IsValid(Telephone1))
            return "Téléphone 1 invalide : 05, 06 ou 07 suivi de 8 chiffres, ou un fixe (0xx xx xx xx).";
        if (!string.IsNullOrWhiteSpace(Telephone2) && !AlgerianPhone.IsValid(Telephone2))
            return "Téléphone 2 invalide : 05, 06 ou 07 suivi de 8 chiffres, ou un fixe (0xx xx xx xx).";
        if (Note?.Trim().Length > MaxNoteLength)
            return $"Note trop longue (au plus {MaxNoteLength} caractères).";
        return null;
    }
}

/// <summary>
/// Algerian numbers: mobiles 05/06/07 + 8 digits, landlines 02x/03x/04x + 6 digits, with or without +213 / 00213.
/// Used by CHIFA.Mobile too, so the phone and the agent accept the same numbers.
/// </summary>
public static class AlgerianPhone
{
    /// <summary>National digits (<c>0661234567</c>, <c>021234567</c>) or null when not a valid Algerian number.</summary>
    public static string? Normalize(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        var digits = new StringBuilder();
        var text = input.Trim();
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            if (char.IsAsciiDigit(c))
                digits.Append(c);
            else if (c == '+' && i == 0)
                digits.Append("00");
            else if (c is not (' ' or '.' or '-' or '(' or ')' or '/'))
                return null;
        }

        var number = digits.ToString();
        if (number.StartsWith("00213"))
            number = "0" + number[5..];
        else if (number.StartsWith("213") && number.Length is 11 or 12)
            number = "0" + number[3..];
        else if (number.Length is 8 or 9 && number[0] != '0')
            number = "0" + number;

        // "+213 (0) 661…": the trunk 0 written after the country code.
        if (number.StartsWith("00") && number.Length is 10 or 11)
            number = number[1..];

        return IsMobile(number) || IsLandline(number) ? number : null;
    }

    public static bool IsValid(string? input) => Normalize(input) is not null;

    /// <summary><c>0661 23 45 67</c> or <c>021 23 45 67</c>; the input as is when it is not valid.</summary>
    public static string Format(string? input)
    {
        if (Normalize(input) is not { } n)
            return input ?? "";
        var head = n.Length == 10 ? 4 : 3;
        return $"{n[..head]} {n[head..(head + 2)]} {n[(head + 2)..(head + 4)]} {n[(head + 4)..]}";
    }

    /// <summary><c>+213661234567</c>, for <c>tel:</c> and <c>sms:</c> links.</summary>
    public static string? ToInternational(string? input) => Normalize(input) is { } n ? "+213" + n[1..] : null;

    public static bool IsMobile(string national) =>
        national.Length == 10 && national[0] == '0' && national[1] is '5' or '6' or '7' && national.All(char.IsAsciiDigit);

    private static bool IsLandline(string national) =>
        national.Length == 9 && national[0] == '0' && national[1] is '2' or '3' or '4' && national.All(char.IsAsciiDigit);
}

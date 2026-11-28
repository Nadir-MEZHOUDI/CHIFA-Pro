using CHIFA.Contract;

namespace CHIFA.Mobile;

/// <summary><c>tel:</c> and <c>sms:</c> links for the numbers of the patient phone book.</summary>
public static class ContactLinks
{
    public static string? Call(string? phone) => AlgerianPhone.ToInternational(phone) is { } n ? $"tel:{n}" : null;

    /// <summary>Opens the phone's messaging app with the text already written; null for a landline.</summary>
    public static string? Sms(string? phone, string? body = null)
    {
        if (AlgerianPhone.Normalize(phone) is not { } national || !AlgerianPhone.IsMobile(national))
            return null;
        var link = $"sms:{AlgerianPhone.ToInternational(national)}";
        return string.IsNullOrEmpty(body) ? link : $"{link}?body={Uri.EscapeDataString(body)}";
    }

    /// <summary>The first mobile of the contact: landlines cannot receive an SMS.</summary>
    public static string? SmsPhone(PatientContact? contact) =>
        new[] { contact?.Telephone1, contact?.Telephone2 }
            .FirstOrDefault(p => AlgerianPhone.Normalize(p) is { } n && AlgerianPhone.IsMobile(n));

    public static string ReminderText(DateTime? fin, string? pharmacy)
    {
        var name = PharmacyName(pharmacy);
        return fin switch
        {
            { } d when d.Date < DateTime.Today => $"Bonjour, votre traitement s'est terminé le {Fmt.Date(d)}. Pensez à le renouveler. {name ?? "Votre pharmacie"}.",
            { } d => $"Bonjour, votre traitement se termine le {Fmt.Date(d)}. {name ?? "Votre pharmacie"}.",
            _ => name is null ? "Bonjour, " : $"Bonjour, ici la {char.ToLowerInvariant(name[0])}{name[1..]}. "
        };
    }

    /// <summary>"Pharmacie El Amel" whether the officine is named "El Amel" or "Pharmacie El Amel".</summary>
    private static string? PharmacyName(string? pharmacy)
    {
        if (string.IsNullOrWhiteSpace(pharmacy))
            return null;
        var name = pharmacy.Trim();
        return name.StartsWith("pharmacie", StringComparison.OrdinalIgnoreCase) ? "Pharmacie" + name[9..] : $"Pharmacie {name}";
    }
}

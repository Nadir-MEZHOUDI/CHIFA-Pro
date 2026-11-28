namespace CHIFA.Gateway.Features.LicenseRequests;

/// <summary>The pharmacist's details kept with the pharmacy, so a phone sends the license request in one tap.</summary>
public static class PharmacistContacts
{
    public static PharmacistContact? Of(Pharmacy pharmacy) =>
        pharmacy is { ContactName: { } name, ContactPhone: { } phone }
            ? new PharmacistContact(name, phone, pharmacy.ContactBusinessName, pharmacy.ContactEmail, pharmacy.ContactWilaya,
                pharmacy.ContactAddress, pharmacy.ContactCodePs, pharmacy.ContactUpdatedAt)
            : null;

    /// <summary>
    /// Keeps usable details (a name and a phone number) when they are newer than the kept ones. Details SmartCHIFA
    /// only read from its settings (no date) fill an empty slot but never replace details the pharmacist typed.
    /// </summary>
    public static void Keep(Pharmacy pharmacy, PharmacistContact? contact)
    {
        if (Clean(contact?.FullName, 160) is not { } name || Clean(contact!.Phone, 40) is not { } phone || phone.Count(char.IsDigit) < 9)
            return;

        var newer = contact.UpdatedAt is { } typed
            ? pharmacy.ContactUpdatedAt is not { } kept || typed > kept
            : pharmacy.ContactName is null;
        if (!newer)
            return;

        pharmacy.ContactName = name;
        pharmacy.ContactPhone = phone;
        pharmacy.ContactBusinessName = Clean(contact.BusinessName, 160);
        pharmacy.ContactEmail = Clean(contact.Email, 256);
        pharmacy.ContactWilaya = Clean(contact.Wilaya, 64);
        pharmacy.ContactAddress = Clean(contact.Address, 256);
        pharmacy.ContactCodePs = Clean(contact.CodePs, 64);
        pharmacy.ContactUpdatedAt = contact.UpdatedAt is { } at ? DateTime.SpecifyKind(at.ToUniversalTime(), DateTimeKind.Utc) : null;
    }

    private static string? Clean(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        value = value.Trim();
        return value.Length > maxLength ? value[..maxLength] : value;
    }
}

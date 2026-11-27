namespace CHIFA.Mobile.Admin;

// Shapes of the gateway's /admin/usage JSON.

public record UsageDay(DateOnly Day, int ActivePhones, int ActivePharmacies, int Requests, int Minutes);

public record UsageOverview(
    int Pharmacies,
    int PharmaciesOnline,
    int PharmaciesWithPhones,
    int LinkedPhones,
    int PhonesOnline,
    int ActivePhonesToday,
    int ActivePhones7Days,
    int ActivePhones30Days,
    int ActivePharmacies7Days,
    List<UsageDay> Days);

public record OnlinePharmacy(Guid Id, string Name, DateTime? Since);

public record OnlinePhone(Guid Id, string Name, Guid PharmacyId, string Pharmacy, DateTime LastSeenAt);

public record OnlineNow(List<OnlinePharmacy> Pharmacies, List<OnlinePhone> Phones);

public record PharmacyUsage(
    Guid Id,
    string Name,
    bool IsActive,
    bool Online,
    DateTime? LastSeenAt,
    DateTime CreatedAt,
    string? LicenseEdition,
    DateTime? LicenseExpiresAt,
    int LinkedPhones,
    int PhonesOnline,
    DateTime? LastPhoneUse,
    int ActiveDays30,
    int Requests30,
    int Minutes30);

public record DeviceUsage(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    DateTime? LastSeenAt,
    DateTime? RevokedAt,
    bool Online,
    int ActiveDays30,
    int Requests30,
    int Minutes30);

public record PharmacyUsageDetail(PharmacyUsage Pharmacy, List<DeviceUsage> Devices, List<UsageDay> Days);

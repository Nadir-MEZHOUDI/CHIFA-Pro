namespace CHIFA.Pro.Helpers.Settings;

public class SecuritySettings : SettingsToRegistry<SecuritySettings>
{
    /// <summary>"salt:hash" (PBKDF2-SHA256, base64). Empty until the default password is changed.</summary>
    public string PasswordHash { get; set; } = "";

    public bool HideStatistics { get; set; } = true;
    public bool HideMobile { get; set; } = true;
    public bool HideLicense { get; set; } = true;
    public bool HideBordereaux { get; set; } = true;
    public bool HideHomeFactures { get; set; } = true;
    public bool HideHomeMontant { get; set; } = true;
    public bool HideHomeMarge { get; set; } = true;

    public bool IsHidden(ProtectedItem item) => item switch
    {
        ProtectedItem.Statistics => HideStatistics,
        ProtectedItem.Mobile => HideMobile,
        ProtectedItem.License => HideLicense,
        ProtectedItem.Bordereaux => HideBordereaux,
        ProtectedItem.HomeFactures => HideHomeFactures,
        ProtectedItem.HomeMontant => HideHomeMontant,
        ProtectedItem.HomeMarge => HideHomeMarge,
        _ => false
    };

    public void SetHidden(ProtectedItem item, bool hidden)
    {
        switch (item)
        {
            case ProtectedItem.Statistics: HideStatistics = hidden; break;
            case ProtectedItem.Mobile: HideMobile = hidden; break;
            case ProtectedItem.License: HideLicense = hidden; break;
            case ProtectedItem.Bordereaux: HideBordereaux = hidden; break;
            case ProtectedItem.HomeFactures: HideHomeFactures = hidden; break;
            case ProtectedItem.HomeMontant: HideHomeMontant = hidden; break;
            case ProtectedItem.HomeMarge: HideHomeMarge = hidden; break;
        }
    }
}

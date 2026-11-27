namespace CHIFA.Gateway.Data;

/// <summary>How much a linked phone used the app on one day (Algerian time). One row per phone and day.</summary>
public class DeviceActivity
{
    public Guid DeviceId { get; set; }
    public MobileDevice Device { get; set; } = null!;

    /// <summary>Copied from the device so the admin pages can sum per pharmacy without a join.</summary>
    public Guid PharmacyId { get; set; }

    public DateOnly Day { get; set; }

    /// <summary>API calls made by the phone, including the status refresh every 30 s while the app is open.</summary>
    public int Requests { get; set; }

    /// <summary>Distinct minutes with at least one call: roughly how long the app stayed open.</summary>
    public int ActiveMinutes { get; set; }
}

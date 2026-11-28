namespace CHIFA.Gateway.Features.Push;

public static class DeliverableSubscriptions
{
    /// <summary>
    /// Subscriptions that may still be notified: active pharmacy, phone not removed, and trial or subscription not
    /// ended (the SQL form of <see cref="Account.AccountEndpoints.HasEnded"/>).
    /// </summary>
    public static IQueryable<PushSubscription> Deliverable(this IQueryable<PushSubscription> subscriptions, DateTime utcNow) =>
        subscriptions.Where(s => s.Pharmacy.IsActive
                                 && (s.Device == null || s.Device.RevokedAt == null)
                                 && (s.Pharmacy.LicenseCheckedAt == null || s.Pharmacy.LicenseExpiresAt == null || s.Pharmacy.LicenseExpiresAt > utcNow));
}

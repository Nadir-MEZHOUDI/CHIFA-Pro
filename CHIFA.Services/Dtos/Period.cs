namespace CHIFA.Services.Dtos;

/// <summary>
/// A date range filter. Both bounds are whole days and inclusive:
/// <c>From = 01/08</c> and <c>To = 31/08</c> means every facture from 01/08 00:00 to 31/08 23:59:59.
/// <c>To = null</c> means "up to now" (no upper bound), so long-running sessions never miss new factures.
/// </summary>
public class Period
{
    public DateTime? From { get; set; } = DateTime.Today.AddYears(-2);

    /// <summary>Last day included. <c>null</c> = no upper bound (up to today).</summary>
    public DateTime? To { get; set; }

    /// <summary>Inclusive lower bound used in queries: start of the <see cref="From"/> day.</summary>
    public DateTime? Start => From?.Date;

    /// <summary>Exclusive upper bound used in queries: start of the day after <see cref="To"/>.</summary>
    public DateTime? EndExclusive => To?.Date.AddDays(1);

    public static DateTime MaxDate { get; set; }
    public static DateTime MinDate { get; set; }
}

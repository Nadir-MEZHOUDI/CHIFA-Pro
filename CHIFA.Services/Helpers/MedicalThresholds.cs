namespace CHIFA.Services.Helpers;

public static class MedicalThresholds
{
    public const decimal HighPriceThreshold = 1000m;
    public const int HighQuantityThreshold = 3;
    public const int MediumTreatmentDurationDays = 30;
    public const int LongTreatmentDurationDays = 60;
    public const int VeryLongTreatmentDurationDays = 80;

    /// <summary>A renewal this many days before the previous treatment ends, or fewer, is not flagged.</summary>
    public const int EarlyRefillToleranceDays = 5;

    /// <summary>
    /// Longest plausible treatment: early-refill detection looks this far before the period, and ignores longer
    /// durations as entry errors (e.g. 810 days) that would hide every later sale.
    /// </summary>
    public const int MaxTreatmentDurationDays = 180;
}

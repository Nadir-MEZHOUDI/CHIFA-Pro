using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;

namespace CHIFA.Pro.Helpers;

/// <summary>
/// Shared setup for the "Du / Au" date editors of every screen.
/// An empty "Au" editor means "up to today": the app can stay open (in the tray) for days,
/// so a fixed end date taken when the screen opened would hide every new facture.
/// </summary>
internal static class DateFilter
{
    public const string UpToTodayText = "Aujourd'hui";

    /// <summary>
    /// Call after <see cref="ChifaService.GetMinAndMaxDatesAsync"/>. Keeps the lower limit,
    /// removes the upper limit (it was frozen on the last facture) and lets "Au" be empty.
    /// </summary>
    public static void Configure(RepositoryItemDateEdit fromRepo, RepositoryItemDateEdit toRepo)
    {
        foreach (var repo in new[] { fromRepo, toRepo })
        {
            repo.MinValue = Period.MinDate;
            repo.MaxValue = DateTime.MinValue; // DateTime.MinValue = no upper limit
        }

        toRepo.AllowNullInput = DefaultBoolean.True;
        toRepo.NullText = UpToTodayText;
        toRepo.TodayDate = DateTime.Today;
    }

    /// <summary>Reads a date editor value; an empty or cleared editor gives <c>null</c>.</summary>
    public static DateTime? Value(object? editValue)
        => editValue is DateTime date && date != DateTime.MinValue ? date : null;

    public static Period ToPeriod(object? fromEditValue, object? toEditValue)
        => new() { From = Value(fromEditValue), To = Value(toEditValue) };
}

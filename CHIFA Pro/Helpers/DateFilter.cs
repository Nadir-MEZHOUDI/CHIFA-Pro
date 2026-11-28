using DevExpress.Utils;
using DevExpress.XtraBars;
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
    private static readonly TimeSpan ReloadDelay = TimeSpan.FromMilliseconds(400);

    /// <summary>
    /// Shows <paramref name="from"/> → today, then runs <paramref name="load"/> (debounced) each time a date changes.
    /// </summary>
    /// <param name="calendarOnFrom">Opens the "Du" calendar on <paramref name="from"/> instead of today.</param>
    public static async Task SetUpAsync(BarEditItem fromEdit, BarEditItem toEdit, DateTime from, bool calendarOnFrom,
        ReloadCancellation reloads, Func<CancellationToken, Task> load)
    {
        try
        {
            await ConfigureAsync(fromEdit, toEdit);
            if (calendarOnFrom)
                ((RepositoryItemDateEdit)fromEdit.Edit).TodayDate = from;

            fromEdit.EditValue = from;
            toEdit.EditValue = null; // up to today

            fromEdit.EditValueChanged += DateChanged;
            toEdit.EditValueChanged += DateChanged;
        }
        catch (Exception ex)
        {
            ex.Log();
        }

        async void DateChanged(object? sender, EventArgs e)
        {
            try
            {
                await DebounceAsync(reloads, load);
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        }
    }

    /// <summary>
    /// Reads the first facture date as the lower limit, removes the upper limit (it was frozen on the
    /// last facture) and lets "Au" be empty.
    /// </summary>
    public static async Task ConfigureAsync(BarEditItem fromEdit, BarEditItem toEdit)
    {
        await ChifaService.Instance.GetMinAndMaxDatesAsync();

        var fromRepo = (RepositoryItemDateEdit)fromEdit.Edit;
        var toRepo = (RepositoryItemDateEdit)toEdit.Edit;
        foreach (var repo in new[] { fromRepo, toRepo })
        {
            repo.MinValue = Period.MinDate;
            // Not DateTime.MinValue: next to a MinValue, the WXI calendar takes it as the upper limit and shows no day.
            repo.MaxValue = DateTime.Today.AddYears(50);
        }

        toRepo.AllowNullInput = DefaultBoolean.True;
        toRepo.NullText = UpToTodayText;
        toRepo.TodayDate = DateTime.Today;
    }

    /// <summary>Waits for the typing to settle, then loads; a newer change or reload cancels this one.</summary>
    public static async Task DebounceAsync(ReloadCancellation reloads, Func<CancellationToken, Task> load)
    {
        var cancellationToken = reloads.Next();

        try
        {
            await Task.Delay(ReloadDelay, cancellationToken);
            await load(cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
    }

    /// <summary>Reads a date editor value; an empty or cleared editor gives <c>null</c>.</summary>
    public static DateTime? Value(object? editValue)
        => editValue is DateTime date && date != DateTime.MinValue ? date : null;

    public static Period ToPeriod(object? fromEditValue, object? toEditValue)
        => new() { From = Value(fromEditValue), To = Value(toEditValue) };
}

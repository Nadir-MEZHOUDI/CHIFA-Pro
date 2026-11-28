using Microsoft.AspNetCore.Components;

namespace CHIFA.Mobile.Components;

/// <summary>A list loaded page by page for a period, with "load more". The pharmacy agent pages the data.</summary>
public abstract class PagedPage<T> : ComponentBase
{
    [Inject] protected Api Api { get; set; } = default!;

    protected Period Period { get; set; } = PeriodChips.Named("3 mois");
    protected List<T> Items { get; } = [];
    protected int Total { get; private set; }
    protected bool Loading { get; private set; }
    protected ApiException? Error { get; private set; }
    protected bool HasMore => Items.Count < Total;

    // Bumped on each reload: a reply for an older period arriving late is dropped instead of mixed into the list.
    private int _generation;

    protected abstract Task<Page<T>> FetchAsync(Period period, int skip);

    protected override Task OnInitializedAsync() => ReloadAsync();

    protected async Task ChangePeriodAsync(Period period)
    {
        Period = period;
        await ReloadAsync();
    }

    protected async Task ReloadAsync()
    {
        _generation++;
        Items.Clear();
        Total = 0;
        await LoadMoreAsync();
    }

    protected async Task LoadMoreAsync()
    {
        var generation = _generation;
        Loading = true;
        Error = null;
        try
        {
            var page = await FetchAsync(Period, Items.Count);
            if (generation != _generation)
                return;
            Total = page.Total;
            Items.AddRange(page.Items);
        }
        catch (ApiException ex) when (generation == _generation)
        {
            Error = ex;
        }
        catch (ApiException)
        {
        }
        finally
        {
            if (generation == _generation)
                Loading = false;
        }
    }
}

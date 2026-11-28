using Microsoft.AspNetCore.Components;

namespace CHIFA.Mobile.Components;

/// <summary>A page showing one thing loaded from the pharmacy. <c>null</c> from the agent means it does not exist.</summary>
public abstract class Detail<T> : ComponentBase where T : class
{
    [Inject] protected Api Api { get; set; } = default!;

    protected T? Data { get; private set; }
    protected bool Loading { get; private set; } = true;
    protected bool NotFound { get; private set; }
    protected ApiException? Error { get; private set; }

    // Bumped on each load: a reply for an older period arriving late is dropped instead of overwriting the newer one.
    private int _generation;

    protected abstract Task<T?> FetchAsync();

    protected override Task OnParametersSetAsync() => LoadAsync();

    protected async Task LoadAsync()
    {
        var generation = ++_generation;
        Loading = true;
        Error = null;
        NotFound = false;
        try
        {
            var data = await FetchAsync();
            if (generation != _generation)
                return;
            Data = data;
            NotFound = data is null;
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

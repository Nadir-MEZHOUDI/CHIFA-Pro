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

    protected abstract Task<T?> FetchAsync();

    protected override Task OnParametersSetAsync() => LoadAsync();

    protected async Task LoadAsync()
    {
        Loading = true;
        Error = null;
        NotFound = false;
        try
        {
            Data = await FetchAsync();
            NotFound = Data is null;
        }
        catch (ApiException ex)
        {
            Error = ex;
        }
        finally
        {
            Loading = false;
        }
    }
}

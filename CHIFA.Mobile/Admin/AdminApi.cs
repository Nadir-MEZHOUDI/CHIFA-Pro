using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;

namespace CHIFA.Mobile.Admin;

/// <summary>
/// The admin console's calls, signed with the gateway's admin key. The key is typed once on this browser and kept
/// in its localStorage; it is never part of the app itself.
/// </summary>
public sealed class AdminApi(HttpClient http, IJSRuntime js)
{
    private const string StorageKey = "chifa.admin";
    private const string Header = "X-Admin-Key";

    private string? _key;
    private bool _loaded;

    public bool IsSignedIn => _key is not null;

    public async Task LoadAsync()
    {
        if (_loaded)
            return;
        _key = await js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        _loaded = true;
    }

    /// <returns><c>false</c> when the gateway refuses the key.</returns>
    public async Task<bool> SignInAsync(string key)
    {
        _key = key.Trim();
        try
        {
            await OnlineAsync();
        }
        catch (ApiException)
        {
            _key = null;
            return false;
        }

        await js.InvokeVoidAsync("localStorage.setItem", StorageKey, _key);
        return true;
    }

    public async Task SignOutAsync()
    {
        _key = null;
        await js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }

    public Task<UsageOverview> OverviewAsync() => GetAsync<UsageOverview>("admin/usage");

    public Task<OnlineNow> OnlineAsync() => GetAsync<OnlineNow>("admin/usage/online");

    public Task<List<PharmacyUsage>> PharmaciesAsync() => GetAsync<List<PharmacyUsage>>("admin/usage/pharmacies");

    public Task<PharmacyUsageDetail> PharmacyAsync(Guid id) => GetAsync<PharmacyUsageDetail>($"admin/usage/pharmacies/{id}");

    private async Task<T> GetAsync<T>(string url)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add(Header, _key ?? "");
        using var response = await ApiHttp.SendAsync(http, request, "Vérifiez la connexion internet.");
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new ApiException("Clé admin refusée.", "Saisissez de nouveau la clé admin.");
        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new ApiException("Introuvable.");
        if (!response.IsSuccessStatusCode)
            throw new ApiException("Erreur du serveur.", $"Erreur {(int)response.StatusCode}. Réessayez.");
        return (await response.Content.ReadFromJsonAsync<T>(JsonSerializerOptions.Web))!;
    }
}

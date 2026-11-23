using System.Text.Json;
using Microsoft.JSInterop;

namespace CHIFA.Mobile;

/// <summary>The signed-in phone user, kept in localStorage so the app stays signed in until the token expires.</summary>
public sealed class Session(IJSRuntime js)
{
    private const string StorageKey = "chifa.session";

    public LoginResponse? Current { get; private set; }

    public bool IsSignedIn => Current is { } s && s.ExpiresAt.ToUniversalTime() > DateTime.UtcNow.AddMinutes(1);

    public async Task LoadAsync()
    {
        try
        {
            var json = await js.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            Current = json is null ? null : JsonSerializer.Deserialize<LoginResponse>(json, JsonSerializerOptions.Web);
        }
        catch (JsonException)
        {
            Current = null;
        }
    }

    public async Task SignInAsync(LoginResponse login)
    {
        Current = login;
        await js.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(login, JsonSerializerOptions.Web));
    }

    public async Task SignOutAsync()
    {
        Current = null;
        await js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }
}

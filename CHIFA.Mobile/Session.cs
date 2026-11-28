using System.Text.Json;
using Microsoft.JSInterop;

namespace CHIFA.Mobile;

/// <summary>
/// The signed-in phone, kept in localStorage. A linked phone also keeps its device token, so it gets a new
/// access token by itself when the current one expires: the pharmacist enters the code only once.
/// </summary>
public sealed class Session(IJSRuntime js)
{
    private const string StorageKey = "chifa.session";

    public LoginResponse? Current { get; private set; }

    public bool HasValidToken => Current is { } s && s.ExpiresAt.ToUniversalTime() > DateTime.UtcNow.AddMinutes(1);

    public string? DeviceToken => Current?.DeviceToken;

    public bool IsSignedIn => HasValidToken || DeviceToken is not null;

    /// <summary>An employee's phone: the gateway refuses it the turnover, margins, payments and the pharmacy's account.</summary>
    public bool IsStaff => Current?.Role == Roles.Staff;

    /// <summary>Raised when a renewal brought another role (changed in SmartCHIFA), so the menu follows.</summary>
    public event Action? Changed;

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
        await SaveAsync();
    }

    /// <summary>A new access token for the linked phone: the device token stays.</summary>
    public async Task RenewAsync(LoginResponse login)
    {
        var roleChanged = login.Role != Current?.Role;
        Current = login with { DeviceToken = login.DeviceToken ?? Current?.DeviceToken };
        await SaveAsync();
        if (roleChanged)
            Changed?.Invoke();
    }

    public async Task SignOutAsync()
    {
        Current = null;
        await js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
    }

    private ValueTask SaveAsync() =>
        js.InvokeVoidAsync("localStorage.setItem", StorageKey, JsonSerializer.Serialize(Current, JsonSerializerOptions.Web));
}

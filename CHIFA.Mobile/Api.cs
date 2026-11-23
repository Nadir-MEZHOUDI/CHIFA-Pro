using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace CHIFA.Mobile;

/// <summary>Error to show as-is to the user (already in French).</summary>
public sealed class ApiException(string title, string? detail = null) : Exception(title)
{
    public string Title { get; } = title;
    public string? Detail { get; } = detail;
}

/// <summary>Calls the gateway on the same origin. A 401 signs the user out and returns to the login page.</summary>
public sealed class Api(HttpClient http, Session session, NavigationManager nav)
{
    private const int PageSize = 30;

    public async Task<LoginResponse> LoginAsync(string userName, string password)
    {
        HttpResponseMessage response;
        try
        {
            response = await http.PostAsJsonAsync("api/auth/login", new { userName, password });
        }
        catch (HttpRequestException)
        {
            throw Unreachable();
        }

        return response.StatusCode switch
        {
            HttpStatusCode.OK => (await response.Content.ReadFromJsonAsync<LoginResponse>(JsonSerializerOptions.Web))!,
            HttpStatusCode.Unauthorized => throw new ApiException("Nom d'utilisateur ou mot de passe incorrect."),
            HttpStatusCode.TooManyRequests => throw new ApiException("Trop de tentatives.", "Réessayez dans une minute."),
            _ => throw new ApiException("Connexion impossible.", $"Erreur {(int)response.StatusCode}.")
        };
    }

    public Task<OfficineStatus> StatusAsync() => GetAsync<OfficineStatus>("api/officine/status");

    public Task<AgentInfo> InfoAsync() => GetAsync<AgentInfo>("api/officine/info");

    public Task<Dashboard> DashboardAsync(Period period) =>
        GetAsync<Dashboard>($"api/officine/dashboard?{period.Query}");

    public Task<List<Facture>> RecentFacturesAsync(int take = 50) =>
        GetAsync<List<Facture>>($"api/officine/factures/recent?take={take}");

    public Task<Page<Rejet>> RejetsAsync(Period period, int skip) => PageAsync<Rejet>("rejets", period, skip);

    public Task<Page<Bordereau>> BordereauxAsync(Period period, int skip) => PageAsync<Bordereau>("bordereaux", period, skip);

    public Task<Page<Psychotrope>> PsychotropesAsync(Period period, int skip) => PageAsync<Psychotrope>("psychotropes", period, skip);

    private Task<Page<T>> PageAsync<T>(string route, Period period, int skip) =>
        GetAsync<Page<T>>($"api/officine/{route}?{period.Query}&skip={skip}&take={PageSize}");

    private async Task<T> GetAsync<T>(string url)
    {
        if (!session.IsSignedIn)
            throw await SignedOutAsync();

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", session.Current!.AccessToken);

        HttpResponseMessage response;
        try
        {
            response = await http.SendAsync(request);
        }
        catch (HttpRequestException)
        {
            throw Unreachable();
        }

        using (response)
        {
            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<T>(JsonSerializerOptions.Web))!;

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw await SignedOutAsync();

            // The gateway answers agent problems (offline, timeout, error) with French ProblemDetails.
            var problem = await ReadProblemAsync(response);
            throw new ApiException(problem?.Title ?? $"Erreur {(int)response.StatusCode}", problem?.Detail);
        }
    }

    private async Task<ApiException> SignedOutAsync()
    {
        await session.SignOutAsync();
        nav.NavigateTo("login");
        return new ApiException("Session expirée.", "Reconnectez-vous.");
    }

    private static ApiException Unreachable() =>
        new("Serveur injoignable.", "Vérifiez la connexion internet du téléphone.");

    private static async Task<Problem?> ReadProblemAsync(HttpResponseMessage response)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<Problem>(JsonSerializerOptions.Web);
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            return null;
        }
    }

    private sealed record Problem(string? Title, string? Detail);
}

/// <summary>A date range picked on screen, sent as whole days.</summary>
public sealed record Period(string Label, DateTime From, DateTime? To = null)
{
    public string Query => To is { } to
        ? $"from={From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}&to={to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}"
        : $"from={From.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}";

    public static IReadOnlyList<Period> Choices()
    {
        var today = DateTime.Today;
        return
        [
            new("Aujourd'hui", today),
            new("7 jours", today.AddDays(-6)),
            new("Ce mois", new DateTime(today.Year, today.Month, 1)),
            new("3 mois", today.AddMonths(-3)),
            new("Cette année", new DateTime(today.Year, 1, 1)),
            new("2 ans", today.AddYears(-2))
        ];
    }
}

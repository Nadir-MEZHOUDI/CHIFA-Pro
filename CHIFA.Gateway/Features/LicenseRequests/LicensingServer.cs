using System.Net.Http.Json;
using System.Text.Json;

namespace CHIFA.Gateway.Features.LicenseRequests;

// Same JSON as SmartApp.Licensing's /api/v1/requests routes.
public record LicensingSubmission(
    string Product,
    string MachineId,
    string FullName,
    string? BusinessName,
    string? Phone,
    string? Email,
    string? Wilaya,
    string? Address,
    string? CodePs,
    string? Message,
    string? MachineName,
    string? AppVersion);

public record LicensingReceipt(Guid Id, string Token);

public record LicensingStatusQuery(Guid Id, string Token, string MachineId);

/// <param name="Status">pending, approved or rejected.</param>
public record LicensingStatus(string Status, string? License, string? Serial, string? Message);

public record LicensingError(string? Code, string? Message)
{
    public const string RequestNotFound = "request_not_found";
}

/// <summary>
/// Sends phone license requests to the SmartApp licensing server for the pharmacy's computer, exactly as
/// SmartCHIFA would. A <c>null</c> result with a <c>null</c> error means the server could not be reached.
/// </summary>
public class LicensingServer(HttpClient http, ILogger<LicensingServer> logger)
{
    public Task<(LicensingReceipt? Value, LicensingError? Error)> SubmitAsync(LicensingSubmission submission, CancellationToken ct) =>
        PostAsync<LicensingReceipt>("api/v1/requests", submission, ct);

    public Task<(LicensingStatus? Value, LicensingError? Error)> StatusAsync(LicensingStatusQuery query, CancellationToken ct) =>
        PostAsync<LicensingStatus>("api/v1/requests/status", query, ct);

    private async Task<(T? Value, LicensingError? Error)> PostAsync<T>(string route, object body, CancellationToken ct) where T : class
    {
        try
        {
            using var response = await http.PostAsJsonAsync(route, body, JsonSerializerOptions.Web, ct);
            if (response.IsSuccessStatusCode)
                return (await response.Content.ReadFromJsonAsync<T>(JsonSerializerOptions.Web, ct), null);

            // Down, or its per-IP limit reached (every phone request comes from the gateway's address).
            if ((int)response.StatusCode >= 500 || response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                logger.LogWarning("Licensing server unavailable for {Route}: {Status}", route, (int)response.StatusCode);
                return (null, null);
            }

            var error = await ReadErrorAsync(response, ct);
            logger.LogWarning("Licensing server refused {Route}: {Status} {Code}", route, (int)response.StatusCode, error.Code);
            return (null, error);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException && !ct.IsCancellationRequested)
        {
            logger.LogWarning("Licensing server unreachable for {Route}: {Error}", route, ex.Message);
            return (null, null);
        }
    }

    private static async Task<LicensingError> ReadErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            return await response.Content.ReadFromJsonAsync<LicensingError>(JsonSerializerOptions.Web, ct)
                   ?? new LicensingError(null, null);
        }
        catch (JsonException)
        {
            return new LicensingError(null, null);
        }
    }
}

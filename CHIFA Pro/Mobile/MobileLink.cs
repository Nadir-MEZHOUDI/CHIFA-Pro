using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.ServiceProcess;
using System.Text.Json;
using CHIFA.Agent;
using CHIFA.Contract;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;

namespace CHIFA.Pro.Mobile;

/// <summary>A problem to show as-is to the pharmacist (already in French).</summary>
internal sealed class MobileLinkException(string message) : Exception(message);

/// <summary>
/// Links phones to this pharmacy. On first use SmartCHIFA registers with its signed license and receives
/// the pharmacy's agent key; it keeps it in <see cref="AgentOptions.SharedSettingsPath"/>, runs the agent
/// that answers the phones, and asks the server for one-time pairing codes.
/// <para>
/// When the CHIFA.Agent Windows service is running (to answer phones while SmartCHIFA is closed), it reads
/// the same file and SmartCHIFA does not start a second agent.
/// </para>
/// </summary>
internal static class MobileLink
{
    private const string DefaultGatewayUrl = "https://chifa-gateway.smartappdz.org";
    private const string AgentServiceName = "CHIFA.Agent";

    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };
    private static readonly SemaphoreSlim RegisterLock = new(1, 1);
    private static readonly JsonSerializerOptions FileJson = new() { WriteIndented = true };
    private static AgentWorker? _agent;

    /// <summary><c>CHIFA_GATEWAY_URL</c> points to a test server.</summary>
    public static string GatewayUrl =>
        (Environment.GetEnvironmentVariable("CHIFA_GATEWAY_URL") is { Length: > 0 } url ? url : DefaultGatewayUrl).TrimEnd('/');

    /// <summary>Opened by the QR code: the phone app fills the code in by itself.</summary>
    public static string PhoneUrl(string code) => $"{GatewayUrl}/login?code={Uri.EscapeDataString(code)}";

    public static string PhoneHost => new Uri(GatewayUrl).Host;

    public static bool AgentRunsAsService
    {
        get
        {
            try
            {
                using var service = new ServiceController(AgentServiceName);
                return service.Status == ServiceControllerStatus.Running;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }
    }

    /// <summary>At startup: answer the linked phones while SmartCHIFA is open.</summary>
    public static void StartAgent()
    {
        if (Load() is { } settings)
            RestartAgent(settings);
    }

    public static async Task StopAgentAsync()
    {
        var agent = Interlocked.Exchange(ref _agent, null);
        if (agent is null) return;
        try
        {
            await agent.StopAsync(CancellationToken.None);
        }
        finally
        {
            agent.Dispose();
        }
    }

    /// <summary>A new code; the previous one stops working.</summary>
    public static Task<PairingCodeResponse> NewCodeAsync() =>
        SendAsync<PairingCodeResponse>(HttpMethod.Post, LinkRoutes.Codes);

    public static Task<List<LinkedDevice>> DevicesAsync() =>
        SendAsync<List<LinkedDevice>>(HttpMethod.Get, LinkRoutes.Devices);

    public static Task RemoveDeviceAsync(Guid id) =>
        SendAsync<object>(HttpMethod.Delete, $"{LinkRoutes.Devices}/{id}");

    /// <summary>Calls the server with the agent key, registering first (or again) when there is no valid key.</summary>
    private static async Task<T> SendAsync<T>(HttpMethod method, string route)
    {
        try
        {
            var settings = Load() is { } saved && saved.Agent.GatewayUrl == GatewayUrl ? saved : await RegisterAsync();
            var response = await SendWithKeyAsync(method, route, settings.Agent.AgentKey);

            // Key replaced elsewhere (SmartCHIFA reinstalled, file restored): register again once.
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Dispose();
                settings = await RegisterAsync();
                response = await SendWithKeyAsync(method, route, settings.Agent.AgentKey);
            }

            using (response)
            {
                if (!response.IsSuccessStatusCode)
                    throw new MobileLinkException($"Le serveur SmartCHIFA a répondu par une erreur ({(int)response.StatusCode}). Réessayez plus tard.");

                return response.StatusCode == HttpStatusCode.NoContent
                    ? default!
                    : (await response.Content.ReadFromJsonAsync<T>(JsonSerializerOptions.Web))!;
            }
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            throw new MobileLinkException("Serveur SmartCHIFA injoignable. Vérifiez la connexion Internet de ce poste.");
        }
    }

    private static async Task<HttpResponseMessage> SendWithKeyAsync(HttpMethod method, string route, string agentKey)
    {
        using var request = new HttpRequestMessage(method, GatewayUrl + route);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", agentKey);
        return await Http.SendAsync(request);
    }

    private static async Task<Settings> RegisterAsync()
    {
        await RegisterLock.WaitAsync();
        try
        {
            var license = LicenseGate.LicenseFile()
                ?? throw new MobileLinkException("L'application mobile nécessite une licence SmartCHIFA valide sur ce poste.");

            using var response = await Http.PostAsJsonAsync(GatewayUrl + LinkRoutes.Register, new RegisterPharmacyRequest(license));
            if (response.StatusCode == HttpStatusCode.Forbidden)
                throw new MobileLinkException(await ProblemTitleAsync(response) ?? "Cette licence ne permet pas l'application mobile.");
            if (!response.IsSuccessStatusCode)
                throw new MobileLinkException($"Le serveur SmartCHIFA a répondu par une erreur ({(int)response.StatusCode}). Réessayez plus tard.");

            var registered = (await response.Content.ReadFromJsonAsync<RegisterPharmacyResponse>(JsonSerializerOptions.Web))!;
            var settings = new Settings(new AgentSettings(GatewayUrl, registered.AgentKey), registered.PharmacyId, registered.Name);
            Save(settings);
            Log.Information("Mobile link: pharmacy {Pharmacy} ({PharmacyId}) registered", registered.Name, registered.PharmacyId);
            RestartAgent(settings);
            return settings;
        }
        finally
        {
            RegisterLock.Release();
        }
    }

    private static void RestartAgent(Settings settings)
    {
        _ = Task.Run(async () =>
        {
            try
            {
                await StopAgentAsync();
                if (AgentRunsAsService)
                {
                    Log.Information("Mobile link: the {Service} service answers the phones", AgentServiceName);
                    return;
                }

                Microsoft.Extensions.Logging.ILoggerFactory loggers = new SerilogLoggerFactory(Log.Logger);
                var options = new AgentOptions { GatewayUrl = settings.Agent.GatewayUrl, AgentKey = settings.Agent.AgentKey };
                var agent = new AgentWorker(
                    new FixedOptionsMonitor<AgentOptions>(options),
                    new AgentHandlers(loggers.CreateLogger<AgentHandlers>()),
                    loggers.CreateLogger<AgentWorker>());
                _agent = agent;
                await agent.StartAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                ex.Log();
            }
        });
    }

    private static Settings? Load()
    {
        try
        {
            if (!File.Exists(AgentOptions.SharedSettingsPath)) return null;
            var settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(AgentOptions.SharedSettingsPath));
            return settings is { Agent.AgentKey.Length: > 0, Agent.GatewayUrl.Length: > 0 } ? settings : null;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            return null;
        }
    }

    private static void Save(Settings settings)
    {
        var path = AgentOptions.SharedSettingsPath;
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var temp = path + ".tmp";
        File.WriteAllText(temp, JsonSerializer.Serialize(settings, FileJson));
        File.Move(temp, path, overwrite: true);
    }

    private static async Task<string?> ProblemTitleAsync(HttpResponseMessage response)
    {
        try
        {
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return json.RootElement.TryGetProperty("title", out var title) ? title.GetString() : null;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    /// <summary>The <c>Agent</c> section is what the Windows service reads.</summary>
    private sealed record Settings(AgentSettings Agent, Guid PharmacyId, string Pharmacy);

    private sealed record AgentSettings(string GatewayUrl, string AgentKey);
}

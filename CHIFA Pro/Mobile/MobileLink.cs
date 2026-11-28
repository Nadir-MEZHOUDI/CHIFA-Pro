using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

    private static readonly TimeSpan ServiceCheckInterval = TimeSpan.FromSeconds(30);
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(20) };
    private static readonly SemaphoreSlim RegisterLock = new(1, 1);
    private static readonly SemaphoreSlim AgentLock = new(1, 1);
    private static readonly JsonSerializerOptions FileJson = new() { WriteIndented = true };
    private static AgentWorker? _agent;
    private static CancellationTokenSource? _watch;
    private static string? _reportedLicense;

    /// <summary><c>CHIFA_GATEWAY_URL</c> points to a test server.</summary>
    public static string GatewayUrl =>
        (Environment.GetEnvironmentVariable("CHIFA_GATEWAY_URL") is { Length: > 0 } url ? url : DefaultGatewayUrl).TrimEnd('/');

    /// <summary>Opened by the QR code: the phone app fills the code in by itself.</summary>
    public static string PhoneUrl(string code) => $"{GatewayUrl}/login?code={Uri.EscapeDataString(code)}";

    public static string PhoneHost => new Uri(GatewayUrl).Host;

    public static bool AgentRunsAsService => AgentService.State is AgentServiceState.Running or AgentServiceState.Outdated;

    public static bool IsLinked => Load() is not null;

    /// <summary>At startup: answer the linked phones while SmartCHIFA is open and the Windows service does not.</summary>
    public static void StartAgent()
    {
        if (_watch is not null) return;
        _watch = new CancellationTokenSource();
        var token = _watch.Token;
        _ = Task.Run(() => WatchServiceAsync(token));
    }

    /// <summary>When SmartCHIFA closes.</summary>
    public static async Task ShutdownAsync()
    {
        Interlocked.Exchange(ref _watch, null)?.Cancel();
        await AgentLock.WaitAsync();
        try
        {
            await StopAgentAsync();
        }
        finally
        {
            AgentLock.Release();
        }
    }

    /// <summary>Runs SmartCHIFA's own agent exactly when the Windows service is not running.</summary>
    public static async Task SyncWithServiceAsync()
    {
        await AgentLock.WaitAsync();
        try
        {
            if (AgentRunsAsService)
            {
                if (_agent is null) return;
                await StopAgentAsync();
                Log.Information("Mobile link: the {Service} service answers the phones", AgentService.Name);
            }
            else if (_agent is null && Load() is { } settings)
            {
                await StartOwnAgentAsync(settings);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            AgentLock.Release();
        }
    }

    // The service starts about two minutes after Windows (delayed start) and Windows restarts it after a crash:
    // checking only at startup would leave two agents pushing each other off the gateway, or none at all.
    private static async Task WatchServiceAsync(CancellationToken ct)
    {
        using var timer = new PeriodicTimer(ServiceCheckInterval);
        try
        {
            do
            {
                await SyncWithServiceAsync();
                await ReportLicenseAsync();
            } while (await timer.WaitForNextTickAsync(ct));
        }
        catch (OperationCanceledException)
        {
        }
    }

    /// <summary>
    /// Sends the license file when it changed (renewal, trial turned into a paid license), so the phone shows
    /// the subscription's real end. Only once the pharmacy is registered: it never registers by itself.
    /// </summary>
    private static async Task ReportLicenseAsync()
    {
        if (Load() is not { } settings || settings.Agent.GatewayUrl != GatewayUrl) return;
        var license = LicenseGate.LicenseFile();
        if (license is null || license == _reportedLicense) return;

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, GatewayUrl + LinkRoutes.License)
            {
                Content = JsonContent.Create(new LicenseUpdateRequest(license))
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.Agent.AgentKey);
            using var response = await Http.SendAsync(request);
            // Any answer is final for this file (an older server answers 404): only a network error is retried.
            _reportedLicense = license;
            if (!response.IsSuccessStatusCode)
                Log.Warning("Mobile link: the server refused the license ({Status})", (int)response.StatusCode);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
        }
    }

    /// <summary>
    /// The serial of a license requested from a linked phone and approved by SmartApp, or <c>null</c>. Never
    /// registers: only a pharmacy that already linked a phone can have one.
    /// </summary>
    public static async Task<string?> ApprovedPhoneRequestSerialAsync(CancellationToken ct)
    {
        if (Load() is not { } settings || settings.Agent.GatewayUrl != GatewayUrl) return null;
        try
        {
            using var response = await SendWithKeyAsync(HttpMethod.Get, LinkRoutes.LicenseRequest, settings.Agent.AgentKey, ct);
            if (!response.IsSuccessStatusCode) return null;
            var state = await response.Content.ReadFromJsonAsync<LicenseRequestState>(JsonSerializerOptions.Web, ct);
            return state is { Status: LicenseRequestStates.Approved, Serial.Length: > 0 } ? state.Serial : null;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            return null;
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

    private static async Task<HttpResponseMessage> SendWithKeyAsync(HttpMethod method, string route, string agentKey, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(method, GatewayUrl + route);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", agentKey);
        return await Http.SendAsync(request, ct);
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
            _ = RestartAgentAsync(settings);
            return settings;
        }
        finally
        {
            RegisterLock.Release();
        }
    }

    /// <summary>After a new key: SmartCHIFA's agent restarts with it; the service reads it from the shared file.</summary>
    private static async Task RestartAgentAsync(Settings settings)
    {
        await AgentLock.WaitAsync();
        try
        {
            await StopAgentAsync();
            if (!AgentRunsAsService)
                await StartOwnAgentAsync(settings);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            AgentLock.Release();
        }
    }

    /// <summary>Caller holds <see cref="AgentLock"/>.</summary>
    private static async Task StartOwnAgentAsync(Settings settings)
    {
        Microsoft.Extensions.Logging.ILoggerFactory loggers = new SerilogLoggerFactory(Log.Logger);
        var options = new AgentOptions { GatewayUrl = settings.Agent.GatewayUrl, AgentKey = settings.Agent.AgentKey };
        var agent = new AgentWorker(
            new FixedOptionsMonitor<AgentOptions>(options),
            new AgentHandlers(loggers.CreateLogger<AgentHandlers>()),
            loggers.CreateLogger<AgentWorker>());
        _agent = agent;
        await agent.StartAsync(CancellationToken.None);
        Log.Information("Mobile link: SmartCHIFA answers the phones");
    }

    /// <summary>Caller holds <see cref="AgentLock"/>.</summary>
    private static async Task StopAgentAsync()
    {
        var agent = _agent;
        if (agent is null) return;
        _agent = null;
        try
        {
            await agent.StopAsync(CancellationToken.None);
        }
        finally
        {
            agent.Dispose();
        }
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

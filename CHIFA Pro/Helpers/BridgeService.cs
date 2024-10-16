namespace CHIFA.Pro.Helpers;

public class BridgeService(Action<string>? logger = default)
{
    private readonly HttpClient _client = new();
    public int Port { get; private set; }
    public string? Address { get; private set; }
    public required string? BaseUri { get; set; }
    public required string? Key { get; set; }
    public async Task StartAsync(CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Key);
        ArgumentException.ThrowIfNullOrWhiteSpace(BaseUri);

        await GetServerAddressAndForwardPort();
        await RegisterServer(token);
        StartServer();
        _ = UpdateServerRegistration(token);
    }

    private async Task RegisterServer(CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Key);
        ArgumentException.ThrowIfNullOrWhiteSpace(Address);
        var request = new { Key = Key.GetHash(), Address, Port };
        HttpResponseMessage? result = null;
        do
        {
            if (token.IsCancellationRequested) return;
            try
            {
                result = await _client.PostAsJsonAsync(BaseUri + "Register", request, cancellationToken: token);
                await Task.Delay(2000, token);
            }
            catch (Exception ex)
            {
                logger?.Invoke($"Failed to register server: {ex.Message}");
            }
        } while (result?.IsSuccessStatusCode != true && !token.IsCancellationRequested);

        logger?.Invoke("Server Registered");
    }
    private async Task GetServerAddressAndForwardPort()
    {
        Port = Random.Shared.Next(7000, 10000);
        var natDiscoverer = new NatDiscoverer();
        var cts = new CancellationTokenSource(10_000);
        var device = await natDiscoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
        Address = (await device.GetExternalIPAsync()).ToString();
        logger?.Invoke($"Server address: {Address}:{Port}{Environment.NewLine}");

        if (Port == 0)
            throw new ArgumentOutOfRangeException(nameof(Port));

        await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, Port));
        await device.CreatePortMapAsync(new Mapping(Protocol.Udp, Port, Port));

        logger?.Invoke($"Port: {Port} forwarded to this pc {Address}");
    }


    public Server? Server { get; private set; } = new();
    public void AddService<T>(T service)
    {
        Server ??= new Server();

        if (service != null)
            Server?.Services.AddCodeFirst(service);
    }
    private void StartServer()
    {

        Server?.Ports.Add(new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure));
        Server?.Start();
        logger?.Invoke("Server started...");
    }



    public async Task Stop()
    {
        if (Server is not null)
            await Server.ShutdownAsync();
        await UnRegisterServer();
    }

    private async Task UnRegisterServer()
    {
        if (string.IsNullOrWhiteSpace(Key))
            return;

        var result = await _client.DeleteAsync($"{BaseUri}Unregister/{Key.GetHash()}");

        if (result.IsSuccessStatusCode)
            logger?.Invoke("Server Unregistered");
    }

    //Update server registration every 5 minutes in background
    private async Task UpdateServerRegistration(CancellationToken cts)
    {
        while (!cts.IsCancellationRequested)
        {
            await Task.Delay(300_000, cts);
            await RegisterServer(cts);
            logger?.Invoke("Server registration updated");
        }
    }
}

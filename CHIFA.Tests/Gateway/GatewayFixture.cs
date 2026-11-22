using System.Net.Http.Headers;
using System.Net.Http.Json;
using CHIFA.Contract;
using CHIFA.Gateway.Data;
using CHIFA.Gateway.Features.Admin;
using CHIFA.Gateway.Features.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CHIFA.Tests.Gateway;

/// <summary>
/// The real gateway on Kestrel (real HTTP and WebSockets, like in production) with an in-memory SQLite database.
/// </summary>
public sealed class GatewayFixture : IAsyncLifetime
{
    public const string AdminKey = "test-admin-key-0123456789-0123456789";
    public const int AgentTimeoutSeconds = 2;

    private readonly SqliteConnection _db = new("DataSource=:memory:");
    private WebApplicationFactory<Program> _factory = null!;

    public Uri BaseAddress { get; private set; } = null!;
    public IServiceProvider Services => _factory.Services;

    public async Task InitializeAsync()
    {
        await _db.OpenAsync();
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(b =>
        {
            b.UseSetting("ConnectionStrings:Gateway", "unused");
            b.UseSetting("Gateway:AdminKey", AdminKey);
            b.UseSetting("Gateway:AgentTimeoutSeconds", AgentTimeoutSeconds.ToString());
            b.UseSetting("Jwt:SigningKey", "test-signing-key-0123456789-0123456789-0123456789");
            b.ConfigureServices(s =>
            {
                s.RemoveAll<IDbContextOptionsConfiguration<GatewayDbContext>>();
                s.RemoveAll<DbContextOptions<GatewayDbContext>>();
                s.RemoveAll<IDbContextFactory<GatewayDbContext>>();
                s.AddDbContextFactory<GatewayDbContext>(o => o.UseSqlite(_db));
            });
        });
        _factory.UseKestrel(0);
        _factory.StartServer();
        BaseAddress = _factory.ClientOptions.BaseAddress;
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
        await _db.DisposeAsync();
    }

    public HttpClient CreateClient() => new() { BaseAddress = BaseAddress };

    public HttpClient CreateAdminClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Add(AdminEndpoints.AdminKeyHeader, AdminKey);
        return client;
    }

    /// <summary>A new pharmacy with one phone user, as the admin would set it up.</summary>
    public async Task<TestPharmacy> CreatePharmacyAsync()
    {
        using var admin = CreateAdminClient();
        var name = $"Pharmacie {Guid.NewGuid():N}";
        var created = await (await admin.PostAsJsonAsync("/admin/pharmacies", new CreatePharmacyRequest(name)))
            .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<AgentKeyResponse>();

        var userName = $"user-{Guid.NewGuid():N}";
        const string password = "Mot2Passe!";
        (await admin.PostAsJsonAsync($"/admin/pharmacies/{created!.Id}/users", new CreateUserRequest(userName, password)))
            .EnsureSuccessStatusCode();

        return new TestPharmacy(created.Id, created.Name, created.AgentKey, userName, password);
    }

    /// <summary>An HTTP client logged in as the pharmacy's phone user.</summary>
    public async Task<HttpClient> LoginAsync(TestPharmacy pharmacy)
    {
        var client = CreateClient();
        var login = await (await client.PostAsJsonAsync("/api/auth/login", new LoginRequest(pharmacy.UserName, pharmacy.Password)))
            .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<LoginResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login!.AccessToken);
        return client;
    }

    /// <summary>A minimal agent: the test registers the handlers it needs before starting it.</summary>
    public HubConnection CreateAgentConnection(string agentKey) =>
        new HubConnectionBuilder()
            .WithUrl(new Uri(BaseAddress, AgentHub.Route), o => o.AccessTokenProvider = () => Task.FromResult<string?>(agentKey))
            .Build();

    public async Task WaitUntilAsync(Func<Task<bool>> condition, TimeSpan? timeout = null)
    {
        var deadline = DateTime.UtcNow + (timeout ?? TimeSpan.FromSeconds(15));
        while (!await condition())
        {
            if (DateTime.UtcNow > deadline)
                throw new TimeoutException("Condition not met in time.");
            await Task.Delay(100);
        }
    }
}

public record TestPharmacy(Guid Id, string Name, string AgentKey, string UserName, string Password);

[CollectionDefinition(Name)]
public class GatewayCollection : ICollectionFixture<GatewayFixture>
{
    public const string Name = "Gateway";
}

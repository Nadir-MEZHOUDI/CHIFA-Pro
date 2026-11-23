using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using CHIFA.Contract;
using CHIFA.Gateway.Data;
using CHIFA.Gateway.Features.Auth;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CHIFA.Tests.Gateway;

/// <summary>CHIFA Pro registers with its license and shows a code; the phone links with it once.</summary>
[Collection(GatewayCollection.Name)]
public class LinkTests(GatewayFixture gateway)
{
    [Fact]
    public async Task Pharmacy_registers_with_its_license_and_a_phone_links_with_the_code()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense(customer: "Pharmacie El Amel"));
        await using var agent = gateway.CreateAgentConnection(pharmacy.AgentKey);
        agent.On(AgentMethods.GetInfo, () => Task.FromResult(new AgentInfo("El Amel", "test", DateTime.Now)));
        await agent.StartAsync();

        var code = await NewCodeAsync(pharmacy.AgentKey);
        // Typed on the phone: lower case, no dash.
        var session = await PairAsync(code.Code.Replace("-", "").ToLowerInvariant(), "Android");
        using var phone = PhoneClient(session.AccessToken);

        pharmacy.Name.Should().Be("Pharmacie El Amel");
        code.Code.Should().MatchRegex("^[A-HJKMNP-Z2-9]{4}-[A-HJKMNP-Z2-9]{4}$");
        code.AgentOnline.Should().BeTrue();
        session.Pharmacy.Should().Be("Pharmacie El Amel");
        session.DeviceToken.Should().NotBeNullOrEmpty();
        (await phone.GetFromJsonAsync<AgentInfo>("/api/officine/info", JsonSerializerOptions.Web))!.Officine.Should().Be("El Amel");
        (await DevicesAsync(pharmacy.AgentKey)).Should().ContainSingle().Which.Name.Should().Be("Android");
    }

    [Fact]
    public async Task A_code_works_once_and_a_new_code_replaces_the_previous_one()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var first = await NewCodeAsync(pharmacy.AgentKey);
        var second = await NewCodeAsync(pharmacy.AgentKey);

        (await TryPairAsync(first.Code)).StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await TryPairAsync(second.Code)).StatusCode.Should().Be(HttpStatusCode.OK);
        (await TryPairAsync(second.Code)).StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Expired_code_is_refused()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var code = await NewCodeAsync(pharmacy.AgentKey);
        await using (var db = await gateway.Services.GetRequiredService<IDbContextFactory<GatewayDbContext>>().CreateDbContextAsync())
            await db.PairingCodes.Where(c => c.PharmacyId == pharmacy.PharmacyId)
                .ExecuteUpdateAsync(s => s.SetProperty(c => c.ExpiresAt, DateTime.UtcNow.AddSeconds(-1)));

        var response = await TryPairAsync(code.Code);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await response.Content.ReadAsStringAsync()).Should().Contain("Code invalide ou expir");
    }

    [Fact]
    public async Task Linked_phone_signs_in_again_with_its_device_token_until_it_logs_out()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var session = await PairAsync((await NewCodeAsync(pharmacy.AgentKey)).Code);
        using var client = gateway.CreateClient();

        var again = await client.PostAsJsonAsync("/api/auth/device", new DeviceLoginRequest(session.DeviceToken!));
        using var phone = PhoneClient(session.AccessToken);
        var logout = await phone.PostAsync("/api/auth/logout", null);
        var afterLogout = await client.PostAsJsonAsync("/api/auth/device", new DeviceLoginRequest(session.DeviceToken!));

        again.StatusCode.Should().Be(HttpStatusCode.OK);
        logout.StatusCode.Should().Be(HttpStatusCode.NoContent);
        afterLogout.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await phone.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await DevicesAsync(pharmacy.AgentKey)).Should().BeEmpty();
    }

    [Fact]
    public async Task Phone_removed_in_chifa_pro_loses_access_at_once()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var session = await PairAsync((await NewCodeAsync(pharmacy.AgentKey)).Code);
        using var phone = PhoneClient(session.AccessToken);
        (await phone.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.OK);
        var device = (await DevicesAsync(pharmacy.AgentKey)).Single();

        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);
        var removed = await desktop.DeleteAsync($"{LinkRoutes.Devices}/{device.Id}");

        removed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await phone.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task A_pharmacy_cannot_remove_another_pharmacys_phone()
    {
        var mine = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var other = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        await PairAsync((await NewCodeAsync(other.AgentKey)).Code);
        var otherDevice = (await DevicesAsync(other.AgentKey)).Single();

        using var desktop = gateway.CreatePharmacyClient(mine.AgentKey);
        var response = await desktop.DeleteAsync($"{LinkRoutes.Devices}/{otherDevice.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await DevicesAsync(other.AgentKey)).Should().ContainSingle();
    }

    [Fact]
    public async Task Registering_again_keeps_the_pharmacy_and_replaces_its_agent_key()
    {
        var license = GatewayFixture.CreateLicense();
        var first = await gateway.RegisterAsync(license);
        var second = await gateway.RegisterAsync(license);

        using var oldKey = gateway.CreatePharmacyClient(first.AgentKey);
        var response = await oldKey.PostAsync(LinkRoutes.Codes, null);

        second.PharmacyId.Should().Be(first.PharmacyId);
        second.AgentKey.Should().NotBe(first.AgentKey);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    public static TheoryData<string> RefusedLicenses()
    {
        using var otherKey = ECDsa.Create(ECCurve.NamedCurves.nistP256);
        return
        [
            "not-a-license",
            GatewayFixture.CreateLicense(signingKey: otherKey),
            GatewayFixture.CreateLicense(product: "smartpharm"),
            GatewayFixture.CreateLicense(expiresAt: DateTimeOffset.UtcNow.AddDays(-1)),
            GatewayFixture.CreateLicense(offlineUntil: DateTimeOffset.UtcNow.AddDays(-1))
        ];
    }

    [Theory]
    [MemberData(nameof(RefusedLicenses))]
    public async Task Forged_foreign_or_expired_licenses_are_refused(string license)
    {
        using var client = gateway.CreateClient();

        var response = await client.PostAsJsonAsync(LinkRoutes.Register, new RegisterPharmacyRequest(license));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Codes_require_the_agent_key()
    {
        using var client = gateway.CreateClient();

        var response = await client.PostAsync(LinkRoutes.Codes, null);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<PairingCodeResponse> NewCodeAsync(string agentKey)
    {
        using var desktop = gateway.CreatePharmacyClient(agentKey);
        return (await (await desktop.PostAsync(LinkRoutes.Codes, null)).EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<PairingCodeResponse>())!;
    }

    private async Task<List<LinkedDevice>> DevicesAsync(string agentKey)
    {
        using var desktop = gateway.CreatePharmacyClient(agentKey);
        return (await desktop.GetFromJsonAsync<List<LinkedDevice>>(LinkRoutes.Devices))!;
    }

    private async Task<HttpResponseMessage> TryPairAsync(string code, string? deviceName = null)
    {
        using var client = gateway.CreateClient();
        return await client.PostAsJsonAsync("/api/auth/pair", new PairRequest(code, deviceName));
    }

    private async Task<LoginResponse> PairAsync(string code, string? deviceName = null) =>
        (await (await TryPairAsync(code, deviceName)).EnsureSuccessStatusCode().Content.ReadFromJsonAsync<LoginResponse>())!;

    private HttpClient PhoneClient(string accessToken)
    {
        var client = gateway.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        return client;
    }
}

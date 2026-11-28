using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using CHIFA.Contract;
using CHIFA.Gateway.Features.Account;
using CHIFA.Gateway.Features.Auth;
using CHIFA.Gateway.Features.Push;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CHIFA.Tests.Gateway;

/// <summary>An employee's phone: the counter's data, never the turnover, margins, payments or the pharmacy's account.</summary>
[Collection(GatewayCollection.Name)]
public class RoleTests(GatewayFixture gateway)
{
    [Theory]
    [InlineData("GET", "/api/officine/dashboard")]
    [InlineData("GET", "/api/officine/stats/recent")]
    [InlineData("GET", "/api/officine/stats/mensuelles")]
    [InlineData("GET", "/api/officine/rejets")]
    [InlineData("GET", "/api/officine/bordereaux")]
    [InlineData("GET", "/api/officine/bordereaux/123")]
    [InlineData("GET", "/api/officine/watchlist")]
    [InlineData("GET", "/api/devices")]
    [InlineData("POST", "/api/devices/code")]
    [InlineData("GET", "/api/review")]
    [InlineData("GET", "/api/license-request")]
    public async Task Staff_phone_is_refused_the_pharmacists_pages(string method, string route)
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var (staff, _) = await LinkPhoneAsync(pharmacy.AgentKey, "Comptoir", DeviceRoles.Staff);

        var response = await staff.SendAsync(new HttpRequestMessage(new HttpMethod(method), route));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        (await staff.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Code_says_which_role_the_phone_gets()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());

        var (_, staff) = await LinkPhoneAsync(pharmacy.AgentKey, "Comptoir", DeviceRoles.Staff);
        var (_, owner) = await LinkPhoneAsync(pharmacy.AgentKey, "Titulaire", role: null);

        staff.Role.Should().Be(DeviceRoles.Staff);
        owner.Role.Should().Be(DeviceRoles.Owner);
        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);
        var devices = await desktop.GetFromJsonAsync<List<LinkedDevice>>(LinkRoutes.Devices, JsonSerializerOptions.Web);
        devices!.ToDictionary(d => d.Name, d => d.Role).Should().BeEquivalentTo(new Dictionary<string, string>
        {
            ["Comptoir"] = DeviceRoles.Staff,
            ["Titulaire"] = DeviceRoles.Owner
        });
    }

    [Fact]
    public async Task Unknown_role_is_refused()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);

        var response = await desktop.PostAsJsonAsync(LinkRoutes.Codes, new PairingCodeRequest("admin"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Staff_phone_gets_patients_and_medicaments_without_their_totals()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        await using var agent = gateway.CreateAgentConnection(pharmacy.AgentKey);
        agent.On<PatientKey, object>(AgentMethods.GetPatient, _ => Task.FromResult<object>(new
        {
            NumAssure = "123", Nom = "BENALI", MontantTotal = 98000m,
            Factures = new[] { new { NumFact = "F1", MontantTotal = 1200m } }
        }));
        agent.On<PageQuery, object>(AgentMethods.SearchMedicaments, _ => Task.FromResult<object>(new
        {
            Total = 1, Items = new[] { new { NumEnr = "A1", Nom = "DOLIPRANE", Quantite = 40m, Montant = 5200m } }
        }));
        await agent.StartAsync();
        var (staff, _) = await LinkPhoneAsync(pharmacy.AgentKey, "Comptoir", DeviceRoles.Staff);
        var (owner, _) = await LinkPhoneAsync(pharmacy.AgentKey, "Titulaire", DeviceRoles.Owner);

        var staffPatient = await JsonAsync(staff, "/api/officine/patients/123/00");
        var ownerPatient = await JsonAsync(owner, "/api/officine/patients/123/00");
        var staffMedics = await JsonAsync(staff, "/api/officine/medicaments");
        var ownerMedics = await JsonAsync(owner, "/api/officine/medicaments");

        staffPatient["montantTotal"].Should().BeNull();
        staffPatient["nom"]!.GetValue<string>().Should().Be("BENALI");
        staffPatient["factures"]![0]!["montantTotal"]!.GetValue<decimal>().Should().Be(1200m, "a single facture keeps its amount");
        ownerPatient["montantTotal"]!.GetValue<decimal>().Should().Be(98000m);
        staffMedics["items"]![0]!["montant"].Should().BeNull();
        staffMedics["items"]![0]!["quantite"]!.GetValue<decimal>().Should().Be(40m);
        ownerMedics["items"]![0]!["montant"]!.GetValue<decimal>().Should().Be(5200m);
    }

    [Fact]
    public async Task Role_changed_in_SmartCHIFA_applies_on_the_phones_next_request()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var (phone, session) = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung", DeviceRoles.Owner);
        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);
        var id = (await desktop.GetFromJsonAsync<List<LinkedDevice>>(LinkRoutes.Devices, JsonSerializerOptions.Web))!.Single().Id;

        (await desktop.PutAsJsonAsync($"{LinkRoutes.Devices}/{id}/role", new DeviceRoleRequest(DeviceRoles.Staff)))
            .StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await phone.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var renewed = (await (await gateway.CreateClient().PostAsJsonAsync("/api/auth/device", new DeviceLoginRequest(session.DeviceToken!)))
            .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<LoginResponse>())!;
        renewed.Role.Should().Be(DeviceRoles.Staff);
        phone.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", renewed.AccessToken);
        (await phone.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.OK);
        (await phone.GetAsync("/api/devices")).StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Pharmacists_phone_sets_the_role_of_another_phone_but_not_its_own()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var (owner, _) = await LinkPhoneAsync(pharmacy.AgentKey, "Titulaire", DeviceRoles.Owner);
        await LinkPhoneAsync(pharmacy.AgentKey, "Comptoir", DeviceRoles.Owner);
        var devices = (await owner.GetFromJsonAsync<List<PhoneDevice>>("/api/devices", JsonSerializerOptions.Web))!;
        var self = devices.Single(d => d.IsCurrent);
        var other = devices.Single(d => !d.IsCurrent);

        var demoted = await owner.PutAsJsonAsync($"/api/devices/{other.Id}/role", new DeviceRoleRequest(DeviceRoles.Staff));
        var selfDemoted = await owner.PutAsJsonAsync($"/api/devices/{self.Id}/role", new DeviceRoleRequest(DeviceRoles.Staff));

        demoted.StatusCode.Should().Be(HttpStatusCode.NoContent);
        selfDemoted.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await owner.GetFromJsonAsync<List<PhoneDevice>>("/api/devices", JsonSerializerOptions.Web))!
            .Single(d => d.Id == other.Id).Role.Should().Be(DeviceRoles.Staff);
    }

    [Fact]
    public async Task Alerts_are_not_sent_to_staff_phones()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var (staff, _) = await LinkPhoneAsync(pharmacy.AgentKey, "Comptoir", DeviceRoles.Staff);
        (await staff.PostAsJsonAsync("/api/push/subscribe", new PushSubscribeRequest(
            $"https://fcm.googleapis.com/fcm/send/{Guid.NewGuid():N}",
            "BNcRdreALRFXTkOOUHK1EtK2wtaz5Ry4YfYCA_0QTpQtUbVlUls0VJXg7A8u-Ts1XbjhazAkj7I99e8QcYP7DkM",
            "tBHItJI5svbpez7KI4CCXg"))).EnsureSuccessStatusCode();

        var asked = false;
        await using var agent = gateway.CreateAgentConnection(pharmacy.AgentKey);
        agent.On(AgentMethods.GetAlerts, () =>
        {
            asked = true;
            return Task.FromResult(new List<AgentAlert>());
        });
        await agent.StartAsync();

        await gateway.Services.GetServices<IHostedService>().OfType<AlertPoller>().Single().PollAsync(CancellationToken.None);

        asked.Should().BeFalse();
    }

    private async Task<(HttpClient Phone, LoginResponse Session)> LinkPhoneAsync(string agentKey, string name, string? role)
    {
        using var desktop = gateway.CreatePharmacyClient(agentKey);
        var codeResponse = role is null
            ? await desktop.PostAsync(LinkRoutes.Codes, null)
            : await desktop.PostAsJsonAsync(LinkRoutes.Codes, new PairingCodeRequest(role));
        var code = (await codeResponse.EnsureSuccessStatusCode().Content.ReadFromJsonAsync<PairingCodeResponse>())!;

        var client = gateway.CreateClient();
        var session = (await (await client.PostAsJsonAsync("/api/auth/pair", new PairRequest(code.Code, name)))
            .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<LoginResponse>())!;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.AccessToken);
        return (client, session);
    }

    private static async Task<JsonNode> JsonAsync(HttpClient phone, string route)
    {
        var response = await phone.GetAsync(route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return JsonNode.Parse(await response.Content.ReadAsStringAsync())!;
    }
}

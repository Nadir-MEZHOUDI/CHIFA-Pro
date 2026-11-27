using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CHIFA.Contract;
using CHIFA.Gateway.Data;
using CHIFA.Gateway.Features.Push;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CHIFA.Tests.Gateway;

[Collection(GatewayCollection.Name)]
public class PushTests(GatewayFixture gateway)
{
    private static PushSubscribeRequest Subscription(string id = "") => new(
        $"https://fcm.googleapis.com/fcm/send/{id}{Guid.NewGuid():N}",
        "BNcRdreALRFXTkOOUHK1EtK2wtaz5Ry4YfYCA_0QTpQtUbVlUls0VJXg7A8u-Ts1XbjhazAkj7I99e8QcYP7DkM",
        "tBHItJI5svbpez7KI4CCXg");

    private async Task<List<CHIFA.Gateway.Data.PushSubscription>> SubscriptionsAsync(Guid pharmacyId)
    {
        await using var db = await gateway.Services.GetRequiredService<IDbContextFactory<GatewayDbContext>>().CreateDbContextAsync();
        return await EntityFrameworkQueryableExtensions.ToListAsync(db.PushSubscriptions.AsNoTracking().Where(s => s.PharmacyId == pharmacyId));
    }

    [Fact]
    public async Task Server_key_is_created_once_and_kept()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        using var phone = await gateway.LoginAsync(pharmacy);

        var first = await phone.GetFromJsonAsync<PushKeyResponse>("/api/push/key", JsonSerializerOptions.Web);
        var second = await phone.GetFromJsonAsync<PushKeyResponse>("/api/push/key", JsonSerializerOptions.Web);

        first!.PublicKey.Should().NotBeNullOrEmpty().And.Be(second!.PublicKey);
    }

    [Fact]
    public async Task Subscribing_twice_keeps_one_row_and_unsubscribe_removes_it()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        using var phone = await gateway.LoginAsync(pharmacy);
        var subscription = Subscription();

        (await phone.PostAsJsonAsync("/api/push/subscribe", subscription)).StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await phone.PostAsJsonAsync("/api/push/subscribe", subscription with { Auth = "renewed-auth-secret" })).EnsureSuccessStatusCode();

        var rows = await SubscriptionsAsync(pharmacy.Id);
        rows.Should().ContainSingle().Which.Auth.Should().Be("renewed-auth-secret");

        (await phone.PostAsJsonAsync("/api/push/unsubscribe", new PushEndpointRequest(subscription.Endpoint))).EnsureSuccessStatusCode();
        (await SubscriptionsAsync(pharmacy.Id)).Should().BeEmpty();
    }

    [Fact]
    public async Task Gateway_only_posts_to_browser_push_services()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        using var phone = await gateway.LoginAsync(pharmacy);

        var response = await phone.PostAsJsonAsync("/api/push/subscribe",
            Subscription() with { Endpoint = "https://internal.example/steal" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await SubscriptionsAsync(pharmacy.Id)).Should().BeEmpty();
    }

    [Fact]
    public async Task Push_requires_a_signed_in_phone()
    {
        using var anonymous = gateway.CreateClient();

        (await anonymous.PostAsJsonAsync("/api/push/subscribe", Subscription())).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await anonymous.GetAsync("/api/push/key")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Poller_asks_the_agents_of_subscribed_pharmacies_for_alerts()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        using var phone = await gateway.LoginAsync(pharmacy);
        (await phone.PostAsJsonAsync("/api/push/subscribe", Subscription())).EnsureSuccessStatusCode();

        var asked = new TaskCompletionSource();
        await using var agent = gateway.CreateAgentConnection(pharmacy.AgentKey);
        agent.On(AgentMethods.GetAlerts, () =>
        {
            asked.TrySetResult();
            return Task.FromResult(new List<AgentAlert>());
        });
        await agent.StartAsync();

        await gateway.Services.GetServices<IHostedService>().OfType<AlertPoller>().Single().PollAsync(CancellationToken.None);

        asked.Task.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Fact]
    public async Task Poller_sends_no_alerts_once_the_license_has_ended()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        using var phone = await gateway.LoginAsync(pharmacy);
        (await phone.PostAsJsonAsync("/api/push/subscribe", Subscription())).EnsureSuccessStatusCode();
        await using (var db = await gateway.Services.GetRequiredService<IDbContextFactory<GatewayDbContext>>().CreateDbContextAsync())
        {
            var row = await EntityFrameworkQueryableExtensions.SingleAsync(db.Pharmacies, p => p.Id == pharmacy.Id);
            row.LicenseEdition = "trial";
            row.LicenseCheckedAt = DateTime.UtcNow.AddDays(-1);
            row.LicenseExpiresAt = DateTime.UtcNow.AddMinutes(-1);
            await db.SaveChangesAsync();
        }

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

    [Fact]
    public async Task New_routes_reach_the_agent_with_their_arguments()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        string? facture = null;
        PatientKey? patient = null;
        PageQuery? search = null;
        MedicamentQuery? medic = null;
        await using var agent = gateway.CreateAgentConnection(pharmacy.AgentKey);
        agent.On<string, object>(AgentMethods.GetFacture, n => { facture = n; return Task.FromResult<object>(new { }); });
        agent.On<PatientKey, object>(AgentMethods.GetPatient, k => { patient = k; return Task.FromResult<object>(new { }); });
        agent.On<PageQuery, Page<int>>(AgentMethods.SearchPatients, q => { search = q; return Task.FromResult(new Page<int>(0, [])); });
        agent.On<MedicamentQuery, object>(AgentMethods.GetMedicament, q => { medic = q; return Task.FromResult<object>(new { }); });
        await agent.StartAsync();
        using var phone = await gateway.LoginAsync(pharmacy);

        (await phone.GetAsync("/api/officine/factures/61001126")).EnsureSuccessStatusCode();
        (await phone.GetAsync("/api/officine/patients/800172012061/03")).EnsureSuccessStatusCode();
        (await phone.GetAsync("/api/officine/patients?q=ben%20moh&skip=30&take=30")).EnsureSuccessStatusCode();
        (await phone.GetAsync("/api/officine/medicaments/06893?from=2026-01-01")).EnsureSuccessStatusCode();

        facture.Should().Be("61001126");
        patient.Should().Be(new PatientKey("800172012061", "03"));
        search.Should().BeEquivalentTo(new { Text = "ben moh", Skip = 30, Take = 30 });
        medic.Should().Be(new MedicamentQuery("06893", new DateTime(2026, 1, 1), null));
    }
}

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CHIFA.Contract;
using CHIFA.Gateway.Data;
using CHIFA.Gateway.Features.Auth;
using CHIFA.Gateway.Features.Usage;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CHIFA.Tests.Gateway;

/// <summary>The admin console: phone app usage per pharmacy and who is online now.</summary>
[Collection(GatewayCollection.Name)]
public class UsageTests(GatewayFixture gateway)
{
    [Fact]
    public async Task Counts_each_phones_calls_and_shows_it_online()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var used = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");
        using var idle = await LinkPhoneAsync(pharmacy.AgentKey, "iPhone");
        for (var i = 0; i < 3; i++)
            (await used.GetAsync("/api/devices")).EnsureSuccessStatusCode();

        var detail = await DetailAsync(pharmacy.PharmacyId);

        detail.Pharmacy.LinkedPhones.Should().Be(2);
        detail.Pharmacy.PhonesOnline.Should().Be(1);
        detail.Pharmacy.Requests30.Should().Be(3);
        var samsung = detail.Devices.Single(d => d.Name == "Samsung");
        samsung.Online.Should().BeTrue();
        samsung.Requests30.Should().Be(3);
        samsung.ActiveDays30.Should().Be(1);
        samsung.Minutes30.Should().BeInRange(1, 2);
        detail.Devices.Single(d => d.Name == "iPhone").Should().Match<DeviceUsage>(d => !d.Online && d.Requests30 == 0);
        detail.Days.Should().HaveCount(UsageEndpoints.PeriodDays);
        detail.Days[^1].Should().Match<UsageDay>(d => d.ActivePhones == 1 && d.Requests == 3);
    }

    [Fact]
    public async Task Adds_to_the_days_row_on_each_save()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");

        (await phone.GetAsync("/api/devices")).EnsureSuccessStatusCode();
        await DetailAsync(pharmacy.PharmacyId);
        (await phone.GetAsync("/api/devices")).EnsureSuccessStatusCode();
        var detail = await DetailAsync(pharmacy.PharmacyId);

        detail.Pharmacy.Requests30.Should().Be(2);
        detail.Pharmacy.LastPhoneUse.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        await using var db = await gateway.Services.GetRequiredService<IDbContextFactory<GatewayDbContext>>().CreateDbContextAsync();
        (await EntityFrameworkQueryableExtensions.CountAsync(db.DeviceActivities, a => a.PharmacyId == pharmacy.PharmacyId)).Should().Be(1);
    }

    [Fact]
    public async Task Online_now_names_the_pharmacy_of_each_agent_and_phone()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense(customer: "Pharmacie En Ligne"));
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");
        (await phone.GetAsync("/api/devices")).EnsureSuccessStatusCode();
        await using var agent = gateway.CreateAgentConnection(pharmacy.AgentKey);
        await agent.StartAsync();
        using var admin = gateway.CreateAdminClient();

        OnlineNow online = null!;
        await gateway.WaitUntilAsync(async () =>
        {
            online = (await admin.GetFromJsonAsync<OnlineNow>("/admin/usage/online", JsonSerializerOptions.Web))!;
            return online.Pharmacies.Any(p => p.Id == pharmacy.PharmacyId);
        });

        online.Pharmacies.Single(p => p.Id == pharmacy.PharmacyId).Name.Should().Be(pharmacy.Name);
        online.Phones.Should().ContainSingle(p => p.PharmacyId == pharmacy.PharmacyId)
            .Which.Should().Match<OnlinePhone>(p => p.Name == "Samsung" && p.Pharmacy == pharmacy.Name);
    }

    [Fact]
    public async Task Overview_counts_linked_and_active_phones()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");
        (await phone.GetAsync("/api/devices")).EnsureSuccessStatusCode();
        using var admin = gateway.CreateAdminClient();

        var overview = (await admin.GetFromJsonAsync<UsageOverview>("/admin/usage", JsonSerializerOptions.Web))!;
        var list = (await admin.GetFromJsonAsync<List<PharmacyUsage>>("/admin/usage/pharmacies", JsonSerializerOptions.Web))!;

        overview.LinkedPhones.Should().BeGreaterThanOrEqualTo(1);
        overview.ActivePhonesToday.Should().BeGreaterThanOrEqualTo(1);
        overview.PhonesOnline.Should().BeGreaterThanOrEqualTo(1);
        overview.Days.Should().HaveCount(UsageEndpoints.PeriodDays);
        list.Single(p => p.Id == pharmacy.PharmacyId).Should().Match<PharmacyUsage>(p => p.LinkedPhones == 1 && p.Requests30 == 1);
    }

    [Theory]
    [InlineData("/admin/usage")]
    [InlineData("/admin/usage/online")]
    [InlineData("/admin/usage/pharmacies")]
    public async Task Needs_the_admin_key(string url)
    {
        using var client = gateway.CreateClient();

        (await client.GetAsync(url)).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<PharmacyUsageDetail> DetailAsync(Guid pharmacyId)
    {
        using var admin = gateway.CreateAdminClient();
        return (await admin.GetFromJsonAsync<PharmacyUsageDetail>($"/admin/usage/pharmacies/{pharmacyId}", JsonSerializerOptions.Web))!;
    }

    private async Task<HttpClient> LinkPhoneAsync(string agentKey, string name)
    {
        using var desktop = gateway.CreatePharmacyClient(agentKey);
        var code = (await (await desktop.PostAsync(LinkRoutes.Codes, null)).EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<PairingCodeResponse>())!;

        var client = gateway.CreateClient();
        var session = (await (await client.PostAsJsonAsync("/api/auth/pair", new PairRequest(code.Code, name)))
            .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<LoginResponse>())!;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.AccessToken);
        return client;
    }
}

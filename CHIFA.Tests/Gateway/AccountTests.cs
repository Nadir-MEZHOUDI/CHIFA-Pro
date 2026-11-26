using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CHIFA.Contract;
using CHIFA.Gateway.Data;
using CHIFA.Gateway.Features.Account;
using CHIFA.Gateway.Features.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CHIFA.Tests.Gateway;

/// <summary>The phone's « Abonnement et appareils » page: the subscription and the other linked phones.</summary>
[Collection(GatewayCollection.Name)]
public class AccountTests(GatewayFixture gateway)
{
    [Fact]
    public async Task Phone_sees_only_its_pharmacys_phones_and_itself_marked_current()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var other = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var mine = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");
        using var second = await LinkPhoneAsync(pharmacy.AgentKey, "iPhone");
        using var foreign = await LinkPhoneAsync(other.AgentKey, "Autre");

        var devices = await DevicesAsync(mine);

        devices.Select(d => d.Name).Should().BeEquivalentTo("Samsung", "iPhone");
        devices.Should().ContainSingle(d => d.IsCurrent).Which.Name.Should().Be("Samsung");
    }

    [Fact]
    public async Task Removed_phone_loses_access_at_once_and_its_notifications()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var mine = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");
        using var lost = await LinkPhoneAsync(pharmacy.AgentKey, "Perdu");
        var lostId = (await DevicesAsync(lost)).Single(d => d.IsCurrent).Id;
        await using (var db = await DbAsync())
        {
            db.PushSubscriptions.Add(new PushSubscription
            {
                PharmacyId = pharmacy.PharmacyId, DeviceId = lostId, Endpoint = $"https://push.example/{Guid.NewGuid()}", P256dh = "k", Auth = "a"
            });
            await db.SaveChangesAsync();
        }

        var removed = await mine.DeleteAsync($"/api/devices/{lostId}");

        removed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await lost.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await DevicesAsync(mine)).Should().ContainSingle().Which.Name.Should().Be("Samsung");
        await using (var db = await DbAsync())
            (await EntityFrameworkQueryableExtensions.AnyAsync(db.PushSubscriptions, s => s.DeviceId == lostId)).Should().BeFalse();
    }

    [Fact]
    public async Task Phone_cannot_remove_itself_here()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");
        var self = (await DevicesAsync(phone)).Single();

        var response = await phone.DeleteAsync($"/api/devices/{self.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await phone.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Phone_cannot_remove_another_pharmacys_phone()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        var other = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");
        using var foreign = await LinkPhoneAsync(other.AgentKey, "Autre");
        var foreignId = (await DevicesAsync(foreign)).Single().Id;

        var response = await phone.DeleteAsync($"/api/devices/{foreignId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await foreign.GetAsync("/api/officine/status")).StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Devices_and_subscription_require_a_phone_token()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var anonymous = gateway.CreateClient();
        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);

        (await anonymous.GetAsync("/api/devices")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await anonymous.GetAsync("/api/subscription")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        (await desktop.GetAsync("/api/devices")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Subscription_comes_from_the_registered_license()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense(expiresAt: DateTimeOffset.UtcNow.AddDays(100)));
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");

        var subscription = await SubscriptionAsync(phone);

        subscription.Status.Should().Be(SubscriptionStates.Active);
        subscription.IsTrial.Should().BeFalse();
        subscription.DaysLeft.Should().Be(100);
        subscription.CheckedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Trial_and_expiring_licenses_are_flagged()
    {
        var trial = await gateway.RegisterAsync(GatewayFixture.CreateLicense(edition: "trial", expiresAt: DateTimeOffset.UtcNow.AddDays(5)));
        var expiring = await gateway.RegisterAsync(GatewayFixture.CreateLicense(expiresAt: DateTimeOffset.UtcNow.AddDays(10)));
        using var trialPhone = await LinkPhoneAsync(trial.AgentKey, "A");
        using var expiringPhone = await LinkPhoneAsync(expiring.AgentKey, "B");

        var trialStatus = await SubscriptionAsync(trialPhone);
        var expiringStatus = await SubscriptionAsync(expiringPhone);

        trialStatus.Status.Should().Be(SubscriptionStates.Trial);
        trialStatus.IsTrial.Should().BeTrue();
        expiringStatus.Status.Should().Be(SubscriptionStates.Expiring);
        expiringStatus.DaysLeft.Should().Be(10);
    }

    [Fact]
    public async Task Renewed_license_sent_by_chifa_pro_updates_the_subscription()
    {
        var licenseId = Guid.NewGuid();
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense(licenseId, expiresAt: DateTimeOffset.UtcNow.AddDays(3)));
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey, "Samsung");
        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);
        var renewedUntil = DateTimeOffset.UtcNow.AddDays(365);

        var renewed = await desktop.PostAsJsonAsync(LinkRoutes.License,
            new LicenseUpdateRequest(GatewayFixture.CreateLicense(licenseId, expiresAt: renewedUntil)));
        var foreign = await desktop.PostAsJsonAsync(LinkRoutes.License, new LicenseUpdateRequest(GatewayFixture.CreateLicense()));
        var subscription = await SubscriptionAsync(phone);

        renewed.StatusCode.Should().Be(HttpStatusCode.NoContent);
        foreign.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        subscription.Status.Should().Be(SubscriptionStates.Active);
        subscription.ExpiresAt.Should().BeCloseTo(renewedUntil.UtcDateTime, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Pharmacy_without_a_reported_license_shows_unknown()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        using var phone = await gateway.LoginAsync(pharmacy);

        var subscription = await SubscriptionAsync(phone);

        subscription.Status.Should().Be(SubscriptionStates.Unknown);
        subscription.ExpiresAt.Should().BeNull();
    }

    [Fact]
    public void Past_end_date_is_expired()
    {
        var now = DateTime.UtcNow;
        var pharmacy = new Pharmacy
        {
            Name = "P", AgentKeyHash = "h", LicenseEdition = "trial", LicenseExpiresAt = now.AddHours(-1), LicenseCheckedAt = now.AddDays(-2)
        };

        var subscription = AccountEndpoints.Subscription(pharmacy, now);

        subscription.Status.Should().Be(SubscriptionStates.Expired);
        subscription.IsTrial.Should().BeTrue();
        subscription.DaysLeft.Should().Be(0);
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

    private static async Task<List<PhoneDevice>> DevicesAsync(HttpClient phone) =>
        (await phone.GetFromJsonAsync<List<PhoneDevice>>("/api/devices", JsonSerializerOptions.Web))!;

    private static async Task<SubscriptionStatus> SubscriptionAsync(HttpClient phone) =>
        (await phone.GetFromJsonAsync<SubscriptionStatus>("/api/subscription", JsonSerializerOptions.Web))!;

    private Task<GatewayDbContext> DbAsync() =>
        gateway.Services.GetRequiredService<IDbContextFactory<GatewayDbContext>>().CreateDbContextAsync();
}

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CHIFA.Contract;
using CHIFA.Gateway.Data;
using CHIFA.Gateway.Features.Account;
using CHIFA.Gateway.Features.Auth;
using CHIFA.Gateway.Features.LicenseRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CHIFA.Tests.Gateway;

/// <summary>The pharmacist asks for a license from the phone; SmartCHIFA picks up the approved serial.</summary>
[Collection(GatewayCollection.Name)]
public class LicenseRequestTests(GatewayFixture gateway)
{
    private static readonly PhoneLicenseRequest Form = new("Amel Benali", "0661 23 45 67", Wilaya: "Annaba", Message: "Formule annuelle");

    [Fact]
    public async Task Phone_asks_for_a_license_for_its_pharmacys_computer()
    {
        var (pharmacy, machine) = await RegisterTrialAsync();
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);

        var before = await StateAsync(phone);
        var sent = await phone.PostAsJsonAsync("/api/license-request", Form);
        var after = await StateAsync(phone);

        before.Should().BeEquivalentTo(new { CanRequest = true, Status = LicenseRequestStates.None });
        sent.StatusCode.Should().Be(HttpStatusCode.OK);
        after.Status.Should().Be(LicenseRequestStates.Pending);
        after.SentAt.Should().NotBeNull();
        gateway.Licensing.For(machine)!.Submission.Should().BeEquivalentTo(new
        {
            Product = GatewayFixture.LicenseProduct, MachineId = machine, FullName = "Amel Benali", Phone = "0661 23 45 67", Wilaya = "Annaba",
            BusinessName = "Pharmacie Test"
        });
        gateway.Licensing.For(machine)!.Submission.Message.Should().Contain("application mobile").And.Contain("Formule annuelle");
    }

    [Fact]
    public async Task Approved_request_reaches_smartchifa_and_its_new_license_unblocks_the_phone()
    {
        var (pharmacy, machine) = await RegisterTrialAsync();
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        (await phone.PostAsJsonAsync("/api/license-request", Form)).EnsureSuccessStatusCode();
        await EndLicenseAsync(pharmacy.PharmacyId);
        gateway.Licensing.Approve(machine, "AAAAA-BBBBB-CCCCC-DDDDD-EEEEE");

        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);
        var forPhone = await StateAsync(phone);
        var forDesktop = (await desktop.GetFromJsonAsync<LicenseRequestState>(LinkRoutes.LicenseRequest, JsonSerializerOptions.Web))!;

        forPhone.Should().BeEquivalentTo(new { Status = LicenseRequestStates.Approved, Serial = "AAAAA-BBBBB-CCCCC-DDDDD-EEEEE" });
        forDesktop.Serial.Should().Be("AAAAA-BBBBB-CCCCC-DDDDD-EEEEE");

        // SmartCHIFA activated the serial: a new license, for the same computer, replaces the trial.
        var paid = GatewayFixture.CreateLicense(machineId: machine);
        (await desktop.PostAsJsonAsync(LinkRoutes.License, new LicenseUpdateRequest(paid))).StatusCode.Should().Be(HttpStatusCode.NoContent);

        (await phone.GetAsync("/api/officine/stats/recent")).StatusCode.Should().NotBe(HttpStatusCode.PaymentRequired);
        (await phone.GetFromJsonAsync<SubscriptionStatus>("/api/subscription", JsonSerializerOptions.Web))!.Status.Should().Be(SubscriptionStates.Active);
        (await StateAsync(phone)).Should().BeEquivalentTo(new { CanRequest = false, Status = LicenseRequestStates.None });
    }

    [Fact]
    public async Task License_of_another_computer_does_not_replace_the_pharmacys()
    {
        var (pharmacy, _) = await RegisterTrialAsync();
        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);

        var response = await desktop.PostAsJsonAsync(LinkRoutes.License, new LicenseUpdateRequest(GatewayFixture.CreateLicense()));

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Registering_with_the_paid_license_of_the_same_computer_keeps_the_pharmacy()
    {
        var (trial, machine) = await RegisterTrialAsync();

        var paid = await gateway.RegisterAsync(GatewayFixture.CreateLicense(machineId: machine));

        paid.PharmacyId.Should().Be(trial.PharmacyId);
    }

    [Fact]
    public async Task Rejected_request_shows_why_and_can_be_sent_again()
    {
        var (pharmacy, machine) = await RegisterTrialAsync();
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        (await phone.PostAsJsonAsync("/api/license-request", Form)).EnsureSuccessStatusCode();
        gateway.Licensing.Reject(machine, "Numéro injoignable");

        var rejected = await StateAsync(phone);
        var next = await StateAsync(phone);

        rejected.Should().BeEquivalentTo(new { Status = LicenseRequestStates.Rejected, Message = "Numéro injoignable" });
        next.Should().BeEquivalentTo(new { CanRequest = true, Status = LicenseRequestStates.None });
    }

    [Fact]
    public async Task Request_sent_again_from_smartchifa_is_forgotten_on_the_phone()
    {
        var (pharmacy, machine) = await RegisterTrialAsync();
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        (await phone.PostAsJsonAsync("/api/license-request", Form)).EnsureSuccessStatusCode();
        gateway.Licensing.ReplaceToken(machine);

        (await StateAsync(phone)).Status.Should().Be(LicenseRequestStates.None);
    }

    [Fact]
    public async Task Running_subscription_cannot_be_requested_again()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);

        var state = await StateAsync(phone);
        var sent = await phone.PostAsJsonAsync("/api/license-request", Form);

        state.CanRequest.Should().BeFalse();
        sent.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [InlineData("", "0661234567")]
    [InlineData("Amel", "0661")]
    public async Task Name_and_phone_are_required(string fullName, string phoneNumber)
    {
        var (pharmacy, machine) = await RegisterTrialAsync();
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);

        var sent = await phone.PostAsJsonAsync("/api/license-request", Form with { FullName = fullName, Phone = phoneNumber });

        sent.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        gateway.Licensing.For(machine).Should().BeNull();
    }

    [Fact]
    public async Task Unreachable_licensing_server_answers_503()
    {
        var (pharmacy, machine) = await RegisterTrialAsync();
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        gateway.Licensing.SetDown(machine);

        var sent = await phone.PostAsJsonAsync("/api/license-request", Form);

        sent.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        (await phone.GetFromJsonAsync<LicenseRequestState>("/api/license-request", JsonSerializerOptions.Web))!.CanRequest.Should().BeTrue();
    }

    [Fact]
    public async Task Unreadable_answer_from_the_licensing_server_answers_503()
    {
        var (pharmacy, machine) = await RegisterTrialAsync();
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        gateway.Licensing.SetGarbled(machine);

        var sent = await phone.PostAsJsonAsync("/api/license-request", Form);

        sent.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task Sending_in_a_loop_is_limited_per_pharmacy()
    {
        var (pharmacy, _) = await RegisterTrialAsync();
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        for (var i = 0; i < 5; i++)
            (await phone.PostAsJsonAsync("/api/license-request", Form)).EnsureSuccessStatusCode();

        var limited = await phone.PostAsJsonAsync("/api/license-request", Form);
        var (other, _) = await RegisterTrialAsync();
        using var otherPhone = await LinkPhoneAsync(other.AgentKey);

        limited.StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
        (await limited.Content.ReadFromJsonAsync<Microsoft.AspNetCore.Mvc.ProblemDetails>())!.Title.Should().Be("Trop de tentatives.");
        (await otherPhone.PostAsJsonAsync("/api/license-request", Form)).StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Pharmacy_without_a_known_computer_cannot_request_from_the_phone()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        using var phone = await gateway.LoginAsync(pharmacy);

        (await StateAsync(phone)).CanRequest.Should().BeFalse();
        (await phone.PostAsJsonAsync("/api/license-request", Form)).StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task Desktop_route_needs_the_agent_key()
    {
        using var anonymous = gateway.CreateClient();

        (await anonymous.GetAsync(LinkRoutes.LicenseRequest)).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static readonly PharmacistContact TrialDetails = new(
        "Amel Benali", "0661 23 45 67", "Pharmacie El Amel", "amel@example.dz", "Annaba", "12 rue des Roses", "PS-123",
        DateTime.UtcNow.AddDays(-7));

    [Fact]
    public async Task Details_typed_for_the_trial_come_back_to_the_phone_and_go_with_the_request()
    {
        var (pharmacy, machine) = await RegisterTrialAsync(TrialDetails);
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);

        var state = await StateAsync(phone);
        (await phone.PostAsJsonAsync("/api/license-request",
            new PhoneLicenseRequest(TrialDetails.FullName, TrialDetails.Phone, TrialDetails.BusinessName, TrialDetails.Wilaya))).EnsureSuccessStatusCode();

        state.Contact.Should().BeEquivalentTo(TrialDetails, o => o.Excluding(c => c.UpdatedAt));
        // The phone does not show the e-mail, address and code PS: they still reach SmartApp.
        gateway.Licensing.For(machine)!.Submission.Should().BeEquivalentTo(new { Email = "amel@example.dz", Address = "12 rue des Roses", CodePs = "PS-123" });
    }

    [Fact]
    public async Task Details_changed_on_the_phone_are_not_overwritten_by_older_ones_from_smartchifa()
    {
        var licenseId = Guid.NewGuid();
        var machine = Guid.NewGuid().ToString("N");
        var license = GatewayFixture.CreateLicense(licenseId, edition: "trial", machineId: machine);
        var pharmacy = await RegisterAsync(license, TrialDetails);
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        (await phone.PostAsJsonAsync("/api/license-request", Form with { Phone = "0550 11 22 33" })).EnsureSuccessStatusCode();

        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);
        (await desktop.PostAsJsonAsync(LinkRoutes.License, new LicenseUpdateRequest(license, TrialDetails))).EnsureSuccessStatusCode();
        var afterOlder = (await StateAsync(phone)).Contact;
        (await desktop.PostAsJsonAsync(LinkRoutes.License,
            new LicenseUpdateRequest(license, TrialDetails with { Phone = "0770 99 88 77", UpdatedAt = DateTime.UtcNow.AddMinutes(1) }))).EnsureSuccessStatusCode();
        var afterNewer = (await StateAsync(phone)).Contact;

        afterOlder!.Phone.Should().Be("0550 11 22 33");
        afterNewer!.Phone.Should().Be("0770 99 88 77");
    }

    [Fact]
    public async Task Pharmacy_settings_only_fill_in_missing_details()
    {
        var machine = Guid.NewGuid().ToString("N");
        var license = GatewayFixture.CreateLicense(edition: "trial", machineId: machine);
        var fromSettings = new PharmacistContact("Officine Settings", "0661 00 00 00");
        var pharmacy = await RegisterAsync(license, fromSettings);
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);

        var first = (await StateAsync(phone)).Contact;
        (await desktop.PostAsJsonAsync(LinkRoutes.License, new LicenseUpdateRequest(license, TrialDetails))).EnsureSuccessStatusCode();
        (await desktop.PostAsJsonAsync(LinkRoutes.License, new LicenseUpdateRequest(license, fromSettings))).EnsureSuccessStatusCode();
        var last = (await StateAsync(phone)).Contact;

        first!.FullName.Should().Be("Officine Settings");
        last!.FullName.Should().Be("Amel Benali");
    }

    private static readonly PharmacistContact FromChifa = new(
        "BENALI Amel", "0661 23 45 67", "Pharmacie El Amel", "amel@example.dz", null, "12 rue des Roses, Annaba", "PS-123",
        IdentityFromChifa: true);

    [Fact]
    public async Task Name_address_and_code_ps_from_chifa_cannot_be_changed_from_the_phone()
    {
        var (pharmacy, machine) = await RegisterTrialAsync(FromChifa);
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);

        (await phone.PostAsJsonAsync("/api/license-request", new PhoneLicenseRequest(
            "Quelqu'un d'autre", "0550 11 22 33", "Pharmacie Nouvelle", "Annaba", Email: "nouveau@example.dz"))).EnsureSuccessStatusCode();
        var kept = (await StateAsync(phone)).Contact;

        gateway.Licensing.For(machine)!.Submission.Should().BeEquivalentTo(new
        {
            FullName = "BENALI Amel", Address = "12 rue des Roses, Annaba", CodePs = "PS-123",
            Phone = "0550 11 22 33", Email = "nouveau@example.dz", BusinessName = "Pharmacie Nouvelle"
        });
        kept.Should().BeEquivalentTo(new { FullName = "BENALI Amel", Phone = "0550 11 22 33", Email = "nouveau@example.dz", IdentityFromChifa = true });
    }

    [Fact]
    public async Task Phone_may_leave_out_the_name_when_it_comes_from_chifa()
    {
        var (pharmacy, _) = await RegisterTrialAsync(FromChifa);
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);

        var sent = await phone.PostAsJsonAsync("/api/license-request", new PhoneLicenseRequest(null, "0661 23 45 67"));

        sent.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Chifa_identity_replaces_a_typed_name_but_keeps_the_typed_phone()
    {
        var license = GatewayFixture.CreateLicense(edition: "trial", machineId: Guid.NewGuid().ToString("N"));
        var pharmacy = await RegisterAsync(license, TrialDetails with { Phone = "0550 11 22 33", UpdatedAt = DateTime.UtcNow });
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        using var desktop = gateway.CreatePharmacyClient(pharmacy.AgentKey);

        (await desktop.PostAsJsonAsync(LinkRoutes.License, new LicenseUpdateRequest(license, FromChifa))).EnsureSuccessStatusCode();

        (await StateAsync(phone)).Contact.Should().BeEquivalentTo(new
        {
            FullName = "BENALI Amel", Address = "12 rue des Roses, Annaba", CodePs = "PS-123", Phone = "0550 11 22 33", IdentityFromChifa = true
        });
    }

    private async Task<RegisterPharmacyResponse> RegisterAsync(string license, PharmacistContact contact)
    {
        using var client = gateway.CreateClient();
        return (await (await client.PostAsJsonAsync(LinkRoutes.Register, new RegisterPharmacyRequest(license, contact)))
            .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<RegisterPharmacyResponse>())!;
    }

    private async Task<(RegisterPharmacyResponse Pharmacy, string Machine)> RegisterTrialAsync(PharmacistContact contact)
    {
        var machine = Guid.NewGuid().ToString("N");
        return (await RegisterAsync(GatewayFixture.CreateLicense(edition: "trial", machineId: machine), contact), machine);
    }

    private async Task<(RegisterPharmacyResponse Pharmacy, string Machine)> RegisterTrialAsync()
    {
        var machine = Guid.NewGuid().ToString("N");
        return (await gateway.RegisterAsync(GatewayFixture.CreateLicense(edition: "trial", machineId: machine)), machine);
    }

    private async Task EndLicenseAsync(Guid pharmacyId)
    {
        await using var db = await gateway.Services.GetRequiredService<IDbContextFactory<GatewayDbContext>>().CreateDbContextAsync();
        var pharmacy = await EntityFrameworkQueryableExtensions.SingleAsync(db.Pharmacies, p => p.Id == pharmacyId);
        pharmacy.LicenseExpiresAt = DateTime.UtcNow.AddMinutes(-1);
        await db.SaveChangesAsync();
    }

    private async Task<HttpClient> LinkPhoneAsync(string agentKey)
    {
        using var desktop = gateway.CreatePharmacyClient(agentKey);
        var code = (await (await desktop.PostAsync(LinkRoutes.Codes, null)).EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<PairingCodeResponse>())!;

        var client = gateway.CreateClient();
        var session = (await (await client.PostAsJsonAsync("/api/auth/pair", new PairRequest(code.Code, "Samsung")))
            .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<LoginResponse>())!;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.AccessToken);
        return client;
    }

    private static async Task<LicenseRequestState> StateAsync(HttpClient phone) =>
        (await phone.GetFromJsonAsync<LicenseRequestState>("/api/license-request", JsonSerializerOptions.Web))!;
}

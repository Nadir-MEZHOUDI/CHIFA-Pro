using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CHIFA.Contract;
using CHIFA.Gateway.Data;
using CHIFA.Gateway.Features.Auth;
using CHIFA.Gateway.Features.Reviews;

namespace CHIFA.Tests.Gateway;

/// <summary>Subscribed pharmacies rate SmartCHIFA from the phone; the website lists the published reviews.</summary>
[Collection(GatewayCollection.Name)]
public class ReviewTests(GatewayFixture gateway)
{
    [Fact]
    public async Task Subscribed_pharmacy_reviews_once_and_the_phone_stops_asking()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense(expiresAt: DateTimeOffset.UtcNow.AddDays(200)));
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        using var secondPhone = await LinkPhoneAsync(pharmacy.AgentKey);
        var comment = $"Très pratique au quotidien {Guid.NewGuid():N}";

        var before = await StateAsync(phone);
        var sent = await phone.PostAsJsonAsync("/api/review", new ReviewRequest(5, $"  {comment}  ", " Pharmacie du Centre ", "Constantine"));
        var again = await secondPhone.PostAsJsonAsync("/api/review", new ReviewRequest(4, "Encore un avis de test", "Autre", null));
        var after = await StateAsync(secondPhone);

        before.Should().Be(new ReviewState(true, false, before.SuggestedName));
        before.SuggestedName.Should().NotBeNullOrWhiteSpace();
        sent.StatusCode.Should().Be(HttpStatusCode.NoContent);
        again.StatusCode.Should().Be(HttpStatusCode.Conflict);
        after.Sent.Should().BeTrue();
        (await TestimonialsAsync()).Items.Should().ContainSingle(t => t.Comment == comment)
            .Which.Should().Match<Testimonial>(t => t.Stars == 5 && t.AuthorName == "Pharmacie du Centre" && t.City == "Constantine");
    }

    [Fact]
    public async Task Trial_pharmacy_cannot_review()
    {
        var trial = await gateway.RegisterAsync(GatewayFixture.CreateLicense(edition: "trial", expiresAt: DateTimeOffset.UtcNow.AddDays(5)));
        using var phone = await LinkPhoneAsync(trial.AgentKey);

        (await StateAsync(phone)).CanReview.Should().BeFalse();
        (await phone.PostAsJsonAsync("/api/review", new ReviewRequest(5, "Excellent logiciel pour la pharmacie", "Test", null)))
            .StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Theory]
    [InlineData("standard", 30, true)]
    [InlineData("standard", 5, true)]
    [InlineData("standard", -1, false)]
    [InlineData("trial", 5, false)]
    [InlineData("standard", null, true)]
    public void Only_paid_unexpired_licenses_can_review(string edition, int? daysLeft, bool expected)
    {
        var now = DateTime.UtcNow;
        var pharmacy = new Pharmacy
        {
            Name = "P", AgentKeyHash = "h", LicenseEdition = edition, LicenseExpiresAt = daysLeft is { } d ? now.AddDays(d) : null, LicenseCheckedAt = now
        };

        ReviewEndpoints.CanReview(pharmacy, now).Should().Be(expected);
    }

    [Fact]
    public async Task Pharmacy_without_a_reported_license_cannot_review()
    {
        var pharmacy = await gateway.CreatePharmacyAsync();
        using var phone = await gateway.LoginAsync(pharmacy);

        (await StateAsync(phone)).CanReview.Should().BeFalse();
        (await phone.PostAsJsonAsync("/api/review", new ReviewRequest(5, "Excellent logiciel pour la pharmacie", "Test", null)))
            .StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Theory]
    [InlineData(0, "Un commentaire assez long", "Nom")]
    [InlineData(6, "Un commentaire assez long", "Nom")]
    [InlineData(5, "Court", "Nom")]
    [InlineData(5, "Un commentaire assez long", "  ")]
    public async Task Invalid_review_is_refused_and_can_be_sent_again(int stars, string comment, string authorName)
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);

        var response = await phone.PostAsJsonAsync("/api/review", new ReviewRequest(stars, comment, authorName, null));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        (await StateAsync(phone)).Sent.Should().BeFalse();
    }

    [Fact]
    public async Task Review_needs_a_phone_token_and_testimonials_are_public_for_the_website()
    {
        using var anonymous = gateway.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/testimonials");
        request.Headers.Add("Origin", "https://smartchifa.smartappdz.org");

        var testimonials = await anonymous.SendAsync(request);

        (await anonymous.GetAsync("/api/review")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        testimonials.StatusCode.Should().Be(HttpStatusCode.OK);
        testimonials.Headers.GetValues("Access-Control-Allow-Origin").Should().Equal("https://smartchifa.smartappdz.org");
    }

    [Fact]
    public async Task Admin_can_hide_a_review_from_the_website()
    {
        var pharmacy = await gateway.RegisterAsync(GatewayFixture.CreateLicense());
        using var phone = await LinkPhoneAsync(pharmacy.AgentKey);
        var comment = $"Avis à masquer {Guid.NewGuid():N}";
        (await phone.PostAsJsonAsync("/api/review", new ReviewRequest(1, comment, "Spam", null))).EnsureSuccessStatusCode();
        using var admin = gateway.CreateAdminClient();
        using var anonymous = gateway.CreateClient();

        var review = (await admin.GetFromJsonAsync<List<AdminReview>>("/admin/reviews", JsonSerializerOptions.Web))!.Single(r => r.Comment == comment);
        var hidden = await admin.PutAsJsonAsync($"/admin/reviews/{review.Id}", new ReviewModeration(false));

        review.PharmacyId.Should().Be(pharmacy.PharmacyId);
        hidden.StatusCode.Should().Be(HttpStatusCode.NoContent);
        (await TestimonialsAsync()).Items.Should().NotContain(t => t.Comment == comment);
        (await anonymous.GetAsync("/admin/reviews")).StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private async Task<HttpClient> LinkPhoneAsync(string agentKey)
    {
        using var desktop = gateway.CreatePharmacyClient(agentKey);
        var code = (await (await desktop.PostAsync(LinkRoutes.Codes, null)).EnsureSuccessStatusCode()
            .Content.ReadFromJsonAsync<PairingCodeResponse>())!;

        var client = gateway.CreateClient();
        var session = (await (await client.PostAsJsonAsync("/api/auth/pair", new PairRequest(code.Code, "Téléphone")))
            .EnsureSuccessStatusCode().Content.ReadFromJsonAsync<LoginResponse>())!;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session.AccessToken);
        return client;
    }

    private static async Task<ReviewState> StateAsync(HttpClient phone) =>
        (await phone.GetFromJsonAsync<ReviewState>("/api/review", JsonSerializerOptions.Web))!;

    private async Task<TestimonialsResponse> TestimonialsAsync()
    {
        using var client = gateway.CreateClient();
        return (await client.GetFromJsonAsync<TestimonialsResponse>("/api/testimonials", JsonSerializerOptions.Web))!;
    }
}

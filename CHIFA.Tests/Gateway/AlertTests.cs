using CHIFA.Agent;
using CHIFA.Gateway.Features.Push;
using CHIFA.Services.Dtos;

namespace CHIFA.Tests.Gateway;

public class AlertTests
{
    private static readonly DateTime Morning = new(2026, 11, 24, 9, 0, 0);

    private static RejetDto Paid(string num, decimal virement, decimal rejete = 0) => new()
    {
        NumBord = num,
        Centre = "ANNABA",
        DateBord = new DateTime(2026, 11, 1),
        MontantPartCaisse = virement + rejete,
        MontantVirement = virement
    };

    private static RejetDto Unpaid(string num) => new() { NumBord = num, MontantPartCaisse = 3000 };

    [Fact]
    public void First_run_only_remembers_what_is_already_paid()
    {
        var state = new AlertState();

        var alerts = AlertTracker.Compute(state, [Paid("100", 5000), Paid("101", 3000)], null, Morning);

        alerts.Should().BeEmpty();
        state.Initialized.Should().BeTrue();
        state.PaidBordereaux.Should().BeEquivalentTo(["100", "101"]);
    }

    [Fact]
    public void A_newly_paid_bordereau_is_reported_once()
    {
        var state = new AlertState();
        AlertTracker.Compute(state, [Paid("100", 5000), Unpaid("101")], null, Morning);

        var alerts = AlertTracker.Compute(state, [Paid("100", 5000), Paid("101", 3000)], null, Morning);
        var again = AlertTracker.Compute(state, [Paid("100", 5000), Paid("101", 3000)], null, Morning);

        alerts.Should().ContainSingle().Which.Should().BeEquivalentTo(new { Key = "paid:101", Title = "Bordereau N° 101 payé", Url = "rejets" });
        again.Should().BeEmpty();
    }

    [Fact]
    public void A_rejected_amount_changes_the_alert()
    {
        var state = new AlertState { Initialized = true };

        var alert = AlertTracker.Compute(state, [Paid("102", 9000, rejete: 1000)], null, Morning).Single();

        alert.Title.Should().Be("Rejet sur le bordereau N° 102");
        alert.Body.Should().Contain("rejetés").And.Contain("10,0 %");
    }

    [Fact]
    public void Many_payments_at_once_become_one_summary()
    {
        var state = new AlertState { Initialized = true };
        var paid = Enumerable.Range(1, AlertTracker.MaxDetailedPayments + 1).Select(i => Paid($"2{i:00}", 1000)).ToList();

        var alerts = AlertTracker.Compute(state, paid, null, Morning);

        alerts.Should().ContainSingle().Which.Title.Should().Be($"{paid.Count} bordereaux payés");
    }

    [Fact]
    public void Chronic_reminder_is_sent_once_a_day_when_someone_is_due()
    {
        var state = new AlertState { Initialized = true };
        var forecast = new ForecastSummaryDto
        {
            PatientsCetteSemaine = 4,
            Details =
            [
                new ForecastRefillDto { DatePrevisionnelleRetour = DateTime.Today.AddDays(-3) },
                new ForecastRefillDto { DatePrevisionnelleRetour = DateTime.Today },
                new ForecastRefillDto { DatePrevisionnelleRetour = DateTime.Today.AddDays(5) }
            ]
        };

        var alert = AlertTracker.Compute(state, [], forecast, Morning).Single();

        alert.Key.Should().Be("chronic:2026-11-24");
        alert.Body.Should().Be("1 en retard · 1 attendu aujourd'hui · 4 cette semaine");
        alert.Url.Should().Be("patients?relance=1");
        state.LastChronicDay.Should().Be(Morning.Date);
    }

    [Theory]
    [InlineData("https://fcm.googleapis.com/fcm/send/abc", true)]
    [InlineData("https://updates.push.services.mozilla.com/wpush/v2/abc", true)]
    [InlineData("https://wns2-par02p.notify.windows.com/w/?token=abc", true)]
    [InlineData("https://web.push.apple.com/abc", true)]
    [InlineData("http://fcm.googleapis.com/fcm/send/abc", false)]
    [InlineData("https://fcm.googleapis.com:8443/abc", false)]
    [InlineData("https://evil.example/fcm.googleapis.com", false)]
    [InlineData("https://notify.windows.com.evil.example/abc", false)]
    [InlineData("https://localhost/abc", false)]
    public void Only_browser_push_services_are_accepted(string endpoint, bool accepted)
    {
        PushEndpoints.IsPushService(new Uri(endpoint)).Should().Be(accepted);
    }

    [Fact]
    public void First_poll_only_records_the_open_bordereaux()
    {
        var state = new AlertState { Initialized = true };

        AlertTracker.TrackClosures(state, ["200", "201"]).Should().BeEmpty();

        state.OpenBordereaux.Should().BeEquivalentTo(["200", "201"]);
    }

    [Fact]
    public void A_bordereau_that_leaves_the_open_list_is_closed_once()
    {
        var state = new AlertState { OpenBordereaux = ["200", "201"] };

        var closed = AlertTracker.TrackClosures(state, ["201", "202"]);
        var again = AlertTracker.TrackClosures(state, ["201", "202"]);

        closed.Should().Equal("200");
        again.Should().BeEmpty();
    }

    [Fact]
    public void Closure_alert_gives_the_factures_and_the_amount_due()
    {
        var bordereau = new RejetDto { NumBord = "200", Centre = "CASNOS", NombreFactures = 17, MontantPartCaisse = 12000, MontantMajorations = 500, MontantFae = 85 };

        var alert = AlertTracker.ComputeClosures([bordereau]).Single();

        alert.Should().BeEquivalentTo(new { Key = "closed:200", Title = "Bordereau N° 200 clôturé", Url = "bordereaux" });
        alert.Body.Should().Contain("17 factures").And.Contain(12585m.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("fr-FR")) + " DA").And.Contain("CASNOS");
    }

    [Fact]
    public void A_bordereau_still_open_or_deleted_gives_no_closure_alert()
    {
        AlertTracker.ComputeClosures([new RejetDto { NumBord = "200", EstOuvert = true }]).Should().BeEmpty();
        AlertTracker.ComputeClosures([]).Should().BeEmpty();
    }

    [Fact]
    public void Many_closures_at_once_become_one_summary()
    {
        var closed = Enumerable.Range(1, AlertTracker.MaxDetailedPayments + 1)
            .Select(i => new RejetDto { NumBord = $"{i}", NombreFactures = 10, MontantPartCaisse = 1000 });

        var alert = AlertTracker.ComputeClosures(closed).Single();

        alert.Title.Should().Be($"{AlertTracker.MaxDetailedPayments + 1} bordereaux clôturés");
        alert.Body.Should().StartWith($"{(AlertTracker.MaxDetailedPayments + 1) * 10} factures");
    }

    [Fact]
    public void A_poll_works_on_a_copy_so_a_failure_halfway_reports_nothing_as_seen()
    {
        var kept = new AlertState();
        AlertTracker.Compute(kept, [Paid("100", 5000)], null, Morning);
        AlertTracker.TrackClosures(kept, ["200"]);
        AlertTracker.TrackFactures(kept, [("F1", Morning)], Morning);

        // What a poll does before a later query throws: nothing of it may reach the kept state.
        var poll = kept.Clone();
        AlertTracker.Compute(poll, [Paid("100", 5000), Paid("101", 3000)], null, Morning);
        AlertTracker.TrackClosures(poll, []);
        AlertTracker.TrackFactures(poll, [("F1", Morning), ("F2", Morning)], Morning);

        kept.PaidBordereaux.Should().BeEquivalentTo(["100"]);
        kept.OpenBordereaux.Should().BeEquivalentTo(["200"]);
        kept.SeenFactures.Keys.Should().BeEquivalentTo(["F1"]);
        AlertTracker.Compute(kept, [Paid("100", 5000), Paid("101", 3000)], null, Morning)
            .Should().ContainSingle(a => a.Key == "paid:101");
    }

    [Fact]
    public void Vapid_key_has_the_browser_format()
    {
        var key = PushSender.CreateKey();

        System.Buffers.Text.Base64Url.DecodeFromChars(key.PublicKey).Should().HaveCount(65).And.StartWith([0x04]);
        System.Buffers.Text.Base64Url.DecodeFromChars(key.PrivateKey).Should().HaveCount(32);
    }
}

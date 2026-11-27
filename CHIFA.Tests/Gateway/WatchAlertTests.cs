using CHIFA.Agent;
using CHIFA.Contract;
using CHIFA.Services.Dtos;

namespace CHIFA.Tests.Gateway;

public class WatchAlertTests
{
    private static readonly DateTime Now = new(2026, 11, 27, 10, 0, 0);

    private static WatchedFactureDto Facture(string num, decimal montant = 1000, string assure = "123", string rang = "00",
        params WatchedLigneDto[] lignes) => new()
    {
        NumFact = num,
        DateFact = Now,
        NumAssure = assure,
        Rang = rang,
        Malade = "BENALI AHMED",
        MontFact = montant,
        Lignes = lignes.Length > 0 ? [.. lignes] : [Ligne("A1", "DOLIPRANE")]
    };

    private static WatchedLigneDto Ligne(string numEnr, string nom, decimal qte = 1, bool psy = false) =>
        new() { NumEnr = numEnr, Nom = nom, Qte = qte, Psychotrope = psy };

    [Fact]
    public void First_run_only_remembers_the_factures()
    {
        var state = new AlertState();

        var fresh = AlertTracker.TrackFactures(state, [("F1", Now.AddMinutes(-5))], Now);

        fresh.Should().BeEmpty();
        state.FacturesInitialized.Should().BeTrue();
        state.LastFactureDate.Should().Be(Now.AddMinutes(-5));
    }

    [Fact]
    public void A_new_facture_is_returned_once()
    {
        var state = new AlertState();
        AlertTracker.TrackFactures(state, [("F1", Now.AddMinutes(-5))], Now);

        var first = AlertTracker.TrackFactures(state, [("F1", Now.AddMinutes(-5)), ("F2", Now.AddMinutes(-1))], Now);
        var again = AlertTracker.TrackFactures(state, [("F1", Now.AddMinutes(-5)), ("F2", Now.AddMinutes(-1))], Now);

        first.Should().Equal("F2");
        again.Should().BeEmpty();
    }

    [Fact]
    public void Old_factures_are_not_reported_and_seen_ones_are_pruned()
    {
        var state = new AlertState { FacturesInitialized = true };

        var fresh = AlertTracker.TrackFactures(state, [("OLD", Now.AddDays(-3)), ("NEW", Now)], Now);

        fresh.Should().Equal("NEW");
        state.SeenFactures.Keys.Should().Equal("NEW");
        AlertTracker.FacturesSince(state, Now).Should().Be(Now - AlertTracker.FactureOverlap);
    }

    [Fact]
    public void Nothing_matches_an_empty_watchlist()
    {
        AlertTracker.ComputeWatch(new Watchlist(), [Facture("F1", 99_999)]).Should().BeEmpty();
    }

    [Fact]
    public void A_watched_patient_is_reported_with_a_link_to_the_facture()
    {
        var rules = new Watchlist(Patients: [new WatchedPatient("123", "01", "BENALI")]);

        var alert = AlertTracker.ComputeWatch(rules, [Facture("F/1", rang: "01"), Facture("F2", rang: "00")]).Single();

        alert.Should().BeEquivalentTo(new { Key = "watch:F/1", Title = "Patient suivi : BENALI AHMED", Url = "factures/F%2F1" });
    }

    [Fact]
    public void A_watched_medicine_and_a_psychotrope_are_named()
    {
        var rules = new Watchlist(Psychotropes: true, Medicaments: [new WatchedMedicament("A1", "DOLIPRANE")]);

        var alert = AlertTracker.ComputeWatch(rules,
            [Facture("F1", lignes: [Ligne("A1", "DOLIPRANE", 2), Ligne("P1", "LYRICA", 1, psy: true)])]).Single();

        alert.Title.Should().Be("Médicament suivi vendu");
        alert.Body.Should().Contain("DOLIPRANE").And.Contain("Psychotrope : LYRICA").And.Contain("3 boîtes");
    }

    [Fact]
    public void Psychotropes_are_ignored_unless_asked()
    {
        var facture = Facture("F1", lignes: [Ligne("P1", "LYRICA", psy: true)]);

        AlertTracker.ComputeWatch(new Watchlist(MontantMin: 50_000), [facture]).Should().BeEmpty();
        AlertTracker.ComputeWatch(new Watchlist(Psychotropes: true), [facture]).Single().Title.Should().Be("Psychotrope délivré");
    }

    [Fact]
    public void Amount_and_box_thresholds_are_inclusive()
    {
        var rules = new Watchlist(MontantMin: 5000, BoitesMin: 10);

        var alerts = AlertTracker.ComputeWatch(rules,
        [
            Facture("BIG", 5000),
            Facture("BOXES", 100, lignes: [Ligne("A1", "X", 10)]),
            Facture("SMALL", 4999, lignes: [Ligne("A1", "X", 9)])
        ]);

        alerts.Select(a => a.Key).Should().Equal("watch:BIG", "watch:BOXES");
        alerts[1].Title.Should().Be("Ordonnance de 10 boîtes");
    }

    [Fact]
    public void Many_matches_in_one_poll_become_one_summary()
    {
        var rules = new Watchlist(Psychotropes: true);
        var factures = Enumerable.Range(1, AlertTracker.MaxDetailedWatch + 1)
            .Select(i => Facture($"F{i}", lignes: [Ligne("P1", "LYRICA", psy: true)]));

        var alert = AlertTracker.ComputeWatch(rules, factures).Single();

        alert.Title.Should().Be($"{AlertTracker.MaxDetailedWatch + 1} ordonnances signalées");
        alert.Url.Should().Be("factures");
    }

    [Theory]
    [InlineData('B', null, true)]
    [InlineData(null, "03F013", true)]
    [InlineData(null, "16B001", true)]
    [InlineData(null, "16B040", false)]
    [InlineData(null, "02A001", false)]
    [InlineData(null, null, false)]
    public void Psychotrope_rule_matches_the_register(char? tableau, string? codeDci, bool expected)
    {
        CHIFA.Services.Helpers.PsychotropesHelper.IsPsychotrope(tableau, codeDci).Should().Be(expected);
    }

    [Fact]
    public void Normalize_drops_duplicates_and_empty_thresholds()
    {
        var clean = WatchlistStore.Normalize(new Watchlist(
            MontantMin: 0,
            BoitesMin: -1,
            Patients: [new(" 123 ", "00", "A"), new("123", "00", "B"), new("", "00", null)],
            Medicaments: [new("A1", "X"), new("a1 ", "Y")]));

        clean.MontantMin.Should().BeNull();
        clean.BoitesMin.Should().BeNull();
        clean.Patients.Should().ContainSingle().Which.NumAssure.Should().Be("123");
        clean.Medicaments.Should().HaveCount(2, "medicine numbers are compared as CHIFA stores them");
    }

    [Fact]
    public void A_watchlist_round_trips_through_json()
    {
        var rules = new Watchlist(true, 3000, 5, [new("1", "00", "A")], [new("M", "N")]);

        var copy = System.Text.Json.JsonSerializer.Deserialize<Watchlist>(
            System.Text.Json.JsonSerializer.Serialize(rules, System.Text.Json.JsonSerializerOptions.Web),
            System.Text.Json.JsonSerializerOptions.Web)!;

        copy.Should().BeEquivalentTo(rules);
        System.Text.Json.JsonSerializer.Deserialize<Watchlist>("{}")!.Patients.Should().BeEmpty();
    }
}

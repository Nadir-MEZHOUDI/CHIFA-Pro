using CHIFA.Contract;
using CHIFA.Services.DataServices;
using CHIFA.Services.Dtos;
using Xunit.Abstractions;

namespace CHIFA.Tests.Services;

[Trait("Category", "DbFunctional")]
public class PhoneStatsTests(ITestOutputHelper output)
{
    private readonly StatisticsService _svc = StatisticsService.Instance;
    private static Period ThisYear => new() { From = new DateTime(DateTime.Today.Year, 1, 1) };

    [Fact]
    public async Task Recent_days_are_the_last_eight_days_today_first()
    {
        var days = await _svc.GetRecentDaysAsync();

        days.Select(d => d.Date).Should().Equal(Enumerable.Range(0, 8).Select(i => DateTime.Today.AddDays(-i)));
        days.Should().OnlyContain(d => d.Marge == Math.Round(d.Montant / 6, 2) || d.Montant == 0);
    }

    [Theory]
    [MemberData(nameof(Kinds))]
    public async Task Every_kind_answers_with_totals_matching_its_rows(string kind)
    {
        using var w = new TimeWatch(output: output);
        var stats = await _svc.GetStatsAsync(kind, ThisYear);

        stats.Kind.Should().Be(kind);
        stats.Rows.Should().NotBeEmpty();
        stats.Count.Should().BeGreaterThanOrEqualTo(stats.Rows.Count);
        if (stats.Count == stats.Rows.Count)
            stats.Totals.Montant.Should().Be(stats.Rows.Sum(r => r.Montant));
        stats.Rows.Should().OnlyContain(r => !string.IsNullOrWhiteSpace(r.Label));
        output.WriteLine($"{kind}: {stats.Count} rows, {stats.Totals.Montant:N2} DA, marge {stats.Totals.Marge:N2}");
    }

    [Fact]
    public async Task Time_kinds_agree_on_the_year_totals()
    {
        var jours = await _svc.GetStatsAsync("jours", ThisYear);
        var mois = await _svc.GetStatsAsync("mois", ThisYear);

        mois.Totals.Factures.Should().Be(jours.Totals.Factures);
        mois.Totals.Montant.Should().Be(jours.Totals.Montant);
        mois.Rows.Select(r => r.Date).Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task Clients_carry_the_rang_for_the_patient_link()
    {
        var clients = await _svc.GetStatsAsync("clients", ThisYear);

        clients.Rows.Should().HaveCountLessThanOrEqualTo(100).And.OnlyContain(r => r.Key != null && r.Rang != null);
        clients.Rows.Select(r => r.Montant).Should().BeInDescendingOrder();
    }

    public static TheoryData<string> Kinds => new(StatsQuery.All);
}

using CHIFA.DAL.DataServices;

namespace CHIFA.Tests.Services;

[Trait("Category", "DbFunctional")]
public class StatisticsServiceTests : IDisposable
{
    private readonly StatisticsService _svc;

    public StatisticsServiceTests()
    {
        _svc = StatisticsService.Instance;
        _svc.Period.From = new DateTime(2000, 1, 1);
        _svc.Period.To = new DateTime(2100, 1, 1);
    }

    public void Dispose() { }

    [Fact]
    public async Task GetThisWeekStatsAsync_ReturnsEightDays()
    {
        using var w = new TimeWatch();
        var rows = await _svc.GetThisWeekStatsAsync();
        rows.Count.Should().Be(8);
        rows[0].Date.Should().BeOnOrAfter(rows[^1].Date);
    }

    [Fact]
    public async Task BordereauxAsync_ReturnsBordereaux()
    {
        using var w = new TimeWatch();
        var rows = (await _svc.BordereauxAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].Num.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task YearlyAsync_ReturnsAtLeastOneYear()
    {
        using var w = new TimeWatch();
        var rows = (await _svc.YearlyAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].Year.Should().BeGreaterThan(2000);
        rows[0].Factures.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task MonthlyAsync_ReturnsMonthsWithFactures()
    {
        using var w = new TimeWatch();
        var rows = (await _svc.MonthlyAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows.Should().OnlyContain(r => r.Year != null && r.Month != null);
    }

    [Fact]
    public async Task WeeklyAsync_ReturnsWeeks()
    {
        using var w = new TimeWatch();
        var rows = (await _svc.WeeklyAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].DateDebut.Should().NotBe(default);
    }

    [Fact]
    public async Task DailyAsync_ReturnsDays()
    {
        using var w = new TimeWatch();
        var rows = (await _svc.DailyAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].Factures.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task ByClientAsync_ReturnsClients()
    {
        using var w = new TimeWatch();
        var rows = (await _svc.ByClientAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].NumAssure.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task ProductsAsync_ReturnsProducts()
    {
        using var w = new TimeWatch();
        var rows = (await _svc.ProductsAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].NumEnr.Should().NotBeNullOrEmpty();
        rows[0].Qt.Should().BeGreaterThan(0);
    }
}

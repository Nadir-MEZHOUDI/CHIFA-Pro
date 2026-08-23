using CHIFA.Services.Dtos;
using CHIFA.Services.DataServices;
using Xunit.Abstractions;

namespace CHIFA.Tests.Services;

[Trait("Category", "DbFunctional")]
public class ScopeServiceTests : IDisposable
{
    private readonly ScopeService _svc;
    private readonly ITestOutputHelper _output;

    public ScopeServiceTests(ITestOutputHelper output)
    {
        _svc = ScopeService.Instance;
        _svc.Period.From = new DateTime(2000, 1, 1);
        _svc.Period.To = new DateTime(2100, 1, 1);
        _output = output;
    }

    public void Dispose() { }

    private TimeWatch StartWatch() => new(output: _output);

    [Fact]
    public async Task GetScopeDashboardAsync_ReturnsAggregatedKpis()
    {
        using var w = StartWatch();
        var dashboard = await _svc.GetScopeDashboardAsync();

        dashboard.Should().NotBeNull();
        dashboard.NombreFactures.Should().BeGreaterThanOrEqualTo(0);
        dashboard.ActiviteHoraire.Should().NotBeNull();
        dashboard.ActiviteHoraire.Count.Should().Be(14);
    }

    [Fact]
    public async Task GetRejetsAsync_ReturnsRejetList()
    {
        using var w = StartWatch();
        var rejets = (await _svc.GetRejetsAsync()).ToList();

        rejets.Should().NotBeNull();
        if (rejets.Count > 0)
        {
            var first = rejets[0];
            first.NumBord.Should().NotBeNullOrEmpty();
            first.MontantRejete.Should().BeGreaterThanOrEqualTo(0);
        }
    }

    [Fact]
    public async Task GetPsychotropesAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var psychotropes = (await _svc.GetPsychotropesAsync()).ToList();

        psychotropes.Should().NotBeNull();
    }

    [Fact]
    public async Task GetChronicForecastsAsync_ReturnsForecastSummary()
    {
        using var w = StartWatch();
        var forecast = await _svc.GetChronicForecastsAsync(lookbackMonths: 12, forecastDays: 60);

        forecast.Should().NotBeNull();
        forecast.Details.Should().NotBeNull();
    }

    [Fact]
    public async Task AuditBordereauAsync_ReturnsAuditReport()
    {
        using var w = StartWatch();
        var report = await _svc.AuditBordereauAsync();

        report.Should().NotBeNull();
        report.TotalFacturesAuditees.Should().BeGreaterThanOrEqualTo(0);
        report.Anomalies.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRecentFacturesLiveAsync_ReturnsLimitedRows()
    {
        using var w = StartWatch();
        var liveFactures = (await _svc.GetRecentFacturesLiveAsync(10)).ToList();

        liveFactures.Should().NotBeNull();
        liveFactures.Count.Should().BeLessThanOrEqualTo(10);
    }
}

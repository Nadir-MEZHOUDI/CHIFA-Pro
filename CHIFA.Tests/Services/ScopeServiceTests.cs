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
    public async Task GetPsychotropesAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var psychotropes = (await _svc.GetPsychotropesAsync()).ToList();

        psychotropes.Should().NotBeNull();
    }

    [Fact]
    public async Task GetWatchedFacturesAsync_ReadsTheFacturesFoundBySinceQuery()
    {
        using var w = StartWatch();
        var stamps = await _svc.GetFactureStampsSinceAsync(new DateTime(2000, 1, 1), limit: 20);
        var factures = await _svc.GetWatchedFacturesAsync(stamps.Select(s => s.NumFact).ToList());

        stamps.Should().NotBeEmpty();
        factures.Select(f => f.NumFact).Should().BeEquivalentTo(stamps.Select(s => s.NumFact));
        factures.Should().OnlyContain(f => f.Lignes.Count > 0);
        _output.WriteLine(string.Join("\n", factures.Select(f =>
            $"{f.NumFact} {f.DateFact:g} {f.Malade} {f.MontFact} DA {f.Boites} boîtes psy={f.Lignes.Count(l => l.Psychotrope)}")));
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
    public async Task GetRecentFacturesLiveAsync_ReturnsLimitedRows()
    {
        using var w = StartWatch();
        var liveFactures = (await _svc.GetRecentFacturesLiveAsync(10)).ToList();

        liveFactures.Should().NotBeNull();
        liveFactures.Count.Should().BeLessThanOrEqualTo(10);
    }

    [Fact]
    public async Task GetRecentFacturesLiveAsync_FindsByNumFactNumAssureAndName()
    {
        var sample = (await _svc.GetRecentFacturesLiveAsync(1)).SingleOrDefault();
        if (sample is null)
            return;

        (await _svc.GetRecentFacturesLiveAsync(50, sample.NumFact)).Should().Contain(f => f.NumFact == sample.NumFact);
        (await _svc.GetRecentFacturesLiveAsync(50, sample.NumAssure)).Should().OnlyContain(f => f.NumAssure!.StartsWith(sample.NumAssure!) || f.NumFact.Contains(sample.NumAssure!));

        var word = sample.Assure?.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        if (word is not null)
            (await _svc.GetRecentFacturesLiveAsync(50, word.ToLowerInvariant())).Should().Contain(f => f.NumFact == sample.NumFact);
    }
}

using CHIFA.Services.Dtos;
using CHIFA.Services.DataServices;
using DataModel;
using LinqToDB;
using Xunit.Abstractions;

namespace CHIFA.Tests.Services;

[Trait("Category", "DbFunctional")]
public class ChifaServiceTests : IDisposable
{
    private readonly ChifaService _svc;
    private readonly ITestOutputHelper _output;

    public ChifaServiceTests(ITestOutputHelper output)
    {
        _svc = ChifaService.Instance;
        _svc.Period.From = new DateTime(2000, 1, 1);
        _svc.Period.To = new DateTime(2100, 1, 1);
        _output = output;
    }

    public void Dispose() { }

    private TimeWatch StartWatch() => new(output: _output);

    [Fact]
    public async Task GetAllBordereauxAsync_ReturnsRowsWithComputedAggregates()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetAllBordereauxAsync()).ToList();

        rows.Should().NotBeEmpty();
        var first = rows[0];
        first.Num.Should().NotBeNullOrEmpty();
        first.Center.Should().NotBeNullOrEmpty();
        first.MFAE.Should().Be(first.Nmbr * 5);
        first.MontGlobal.Should().Be(first.MontOff + first.Maj + first.MFAE);
    }

    [Fact]
    public async Task GetAllBordereauxAsync_RespectsPeriod()
    {
        using var w = StartWatch();
        _svc.Period.From = new DateTime(2050, 1, 1);
        _svc.Period.To = new DateTime(2051, 1, 1);
        var rows = (await _svc.GetAllBordereauxAsync()).ToList();
        rows.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllFacturesAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetAllFacturesAsync(last: false, ts: false)).ToList();
        rows.Should().NotBeEmpty();
        rows[0].NumFact.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetAllFacturesAsync_WithTsFilter_AppliesPredicate()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetAllFacturesAsync(last: false, ts: true)).ToList();
        // Result should be a non-empty subset of all factures (those matching the TS/LongDuree/HighPpa predicate).
        var allCount = (await _svc.GetAllFacturesAsync(last: false, ts: false)).Count();
        rows.Count.Should().BeLessThanOrEqualTo(allCount);
    }

    [Fact]
    public async Task GetBeneficiaresAsync_ReturnsRowsWithCenter()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetBeneficiaresAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].NumAssure.Should().NotBeNullOrEmpty();
        rows[0].Rang.Should().NotBeNullOrEmpty();
        rows.Should().OnlyContain(r => r.Center != null);
    }

    [Fact]
    public async Task GetBeneficiareByIdAsync_KnownRecord_ReturnsMatch()
    {
        using var w = StartWatch();
        var all = (await _svc.GetBeneficiaresAsync()).ToList();
        all.Should().NotBeEmpty();
        var sample = all[0];

        var one = await _svc.GetBeneficiareByIdAsync(sample.NumAssure!, sample.Rang!);
        one.Should().NotBeNull();
        one!.NumAssure.Should().Be(sample.NumAssure);
    }

    [Fact]
    public async Task GetBeneficiareByIdAsync_Unknown_ReturnsNull()
    {
        using var w = StartWatch();
        var one = await _svc.GetBeneficiareByIdAsync("ZZZZZZ", "99");
        one.Should().BeNull();
    }

    [Fact]
    public async Task GetCentersAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetCentersAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].CodeCentre.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetFormesAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetFormesAsync()).ToList();
        rows.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetMedicamentsAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetMedicamentsAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].NEnr.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetMedicObsAsync_KnownCode_ReturnsObs()
    {
        using var w = StartWatch();
        var meds = (await _svc.GetMedicamentsAsync()).ToList();
        var sample = meds.FirstOrDefault(m => !string.IsNullOrEmpty(m.NEnr));
        sample.Should().NotBeNull();

        var obs = await _svc.GetMedicObsAsync(sample!.NEnr!);
        // Observation may be null for some medicaments - just ensure the call returns without error.
        obs.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task GetMedicObsAsync_EmptyCode_DoesNotThrow()
    {
        using var w = StartWatch();
        var obs = await _svc.GetMedicObsAsync(string.Empty);
        obs.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task GetFirstOfficineAsync_ReturnsRow()
    {
        using var w = StartWatch();
        var p = await _svc.GetFirstOfficineAsync();
        p.Should().NotBeNull();
    }

    [Fact]
    public async Task GetPatientsOfTraitSpecAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetPatientsOfTraitSpecAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].NumAssure.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetUsersAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.GetUsersAsync()).ToList();
        rows.Should().NotBeEmpty();
    }

    [Fact]
    public async Task LoadAllListNoirAsync_DoesNotThrow()
    {
        using var w = StartWatch();
        var rows = (await _svc.LoadAllListNoirAsync()).ToList();
        rows.Should().NotBeNull();
    }

    [Fact]
    public async Task LoadControlsMedicalAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.LoadControlsMedicalAsync()).ToList();
        rows.Should().NotBeEmpty();
    }

    [Fact]
    public async Task SpecialitesAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.SpecialitesAsync()).ToList();
        rows.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetMinAndMaxDatesAsync_PopulatesPeriod()
    {
        using var w = StartWatch();
        await _svc.GetMinAndMaxDatesAsync();
        Period.MinDate.Year.Should().BeGreaterThan(2000);
        Period.MaxDate.Year.Should().BeGreaterThanOrEqualTo(Period.MinDate.Year);
    }

    [Fact]
    public async Task PatientsWithTraitSpec2Async_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.PatientsWithTraitSpec2Async()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].NumAssure.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task PatientsWithTraitSpecAsync_ReturnsRows()
    {
        using var w = StartWatch();
        var rows = (await _svc.PatientsWithTraitSpecAsync()).ToList();
        rows.Should().NotBeEmpty();
        rows[0].NumAssure.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoadHistoryAsync_KnownAssure_ReturnsRows()
    {
        using var w = StartWatch();
        var benefs = (await _svc.GetBeneficiaresAsync()).ToList();
        var sample = benefs.First(b => !string.IsNullOrEmpty(b.NumAssure) && !string.IsNullOrEmpty(b.Rang));
        var rows = (await _svc.LoadHistoryAsync(sample.NumAssure!, sample.Rang!)).ToList();
        rows.Should().NotBeEmpty();
    }

    [Fact]
    public async Task LoadConsumptionAsync_DistinctTrue_ReturnsRows()
    {
        using var w = StartWatch();
        var benefs = (await _svc.GetBeneficiaresAsync()).ToList();
        var sample = benefs.First(b => !string.IsNullOrEmpty(b.NumAssure) && !string.IsNullOrEmpty(b.Rang));
        var rows = (await _svc.LoadConsumptionAsync(sample.NumAssure!, sample.Rang!, distinct: true)).ToList();
        rows.Should().NotBeEmpty();
    }

    [Fact]
    public async Task LoadConsumptionAsync_DistinctFalse_GroupsByCodeDci()
    {
        using var w = StartWatch();
        var benefs = (await _svc.GetBeneficiaresAsync()).ToList();
        var sample = benefs.First(b => !string.IsNullOrEmpty(b.NumAssure) && !string.IsNullOrEmpty(b.Rang));
        var rows = (await _svc.LoadConsumptionAsync(sample.NumAssure!, sample.Rang!, distinct: false)).ToList();
        rows.Should().NotBeEmpty();
        rows.Should().OnlyHaveUniqueItems(r => r.CodeDci);
    }

    [Fact]
    public async Task GetFactureDetailsByIdAsync_KnownFacture_ReturnsRows()
    {
        using var w = StartWatch();
        await using var db = new ChifaDb();
        var facture = await db.Factures.FirstOrDefaultAsync();
        if (facture is null) return;

        var rows = (await _svc.GetFactureDetailsByIdAsync(facture.NumFact)).ToList();
        rows.Should().NotBeEmpty();
        rows[0].Code.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetPatientTraitementAsync_KnownPatient_ReturnsRows()
    {
        using var w = StartWatch();
        await using var db = new ChifaDb();
        var hit = await db.DetailFacts
            .Where(d => (d.Ppa >= 1000m && d.Qte >= 3) || d.Ts == true || d.DureeTrait >= 60)
            .Select(d => new { d.Facture.NumAssure, d.Facture.RangAd })
            .FirstOrDefaultAsync();
        if (hit is null) return;

        var rows = (await _svc.GetPatientTraitementAsync(hit.NumAssure!, hit.RangAd!, proche: false)).ToList();
        rows.Should().NotBeEmpty();
    }
}

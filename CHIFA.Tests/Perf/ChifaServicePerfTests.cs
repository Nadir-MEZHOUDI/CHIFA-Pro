using CHIFA.Contract.Dtos;

using Xunit.Abstractions;

namespace CHIFA.Tests.Perf;

[Trait("Category", "Perf")]
public class ChifaServicePerfTests : DbPerfTestBase
{
    public ChifaServicePerfTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task GetAllBordereauxAsync_CompletesWithinBudget()
    {
        await using var db = new DataModel.ChifaDb();
        var rowCount = await db.Bordereaus.CountAsync();
        Output.WriteLine($"bordereau rows: {rowCount:N0}");

        if (rowCount == 0) return;

        Chifa.Period.From = new DateTime(2000, 1, 1);
        Chifa.Period.To = new DateTime(2100, 1, 1);

        var (iters, mean, median, min, max) = await MeasureAsync(async () =>
        {
            var data = await Chifa.GetAllBordereauxAsync();
            var list = data as IList<BordereauDto> ?? data.ToList();
            list.Should().NotBeEmpty();
        }, warmup: 1, iterations: 5);

        Output.WriteLine($"GetAllBordereauxAsync -> iters={iters}  mean={mean:F1}ms  median={median:F1}ms  min={min:F1}ms  max={max:F1}ms");

        median.Should().BeLessThan(2000, "aggregate query should be fast");
    }

    [Fact]
    public async Task GetBeneficiaresAsync_CompletesWithinBudget()
    {
        await using var db = new DataModel.ChifaDb();
        var rowCount = await db.Beneficiaires.CountAsync();
        Output.WriteLine($"beneficiaire rows: {rowCount:N0}");

        if (rowCount == 0) return;

        var (iters, mean, median, min, max) = await MeasureAsync(async () =>
        {
            var data = await Chifa.GetBeneficiaresAsync();
            data.Should().NotBeEmpty();
        }, warmup: 1, iterations: 3);

        Output.WriteLine($"GetBeneficiaresAsync -> iters={iters}  mean={mean:F1}ms  median={median:F1}ms");

        median.Should().BeLessThan(5000);
    }

    [Fact]
    public async Task GetAllFacturesAsync_CompletesWithinBudget()
    {
        await using var db = new DataModel.ChifaDb();
        var rowCount = await db.Factures.CountAsync();
        Output.WriteLine($"facture rows: {rowCount:N0}");

        if (rowCount == 0) return;

        Chifa.Period.From = new DateTime(2000, 1, 1);
        Chifa.Period.To = new DateTime(2100, 1, 1);

        var (iters, mean, median, min, max) = await MeasureAsync(async () =>
        {
            var data = await Chifa.GetAllFacturesAsync(last: false, ts: false);
            data.Should().NotBeEmpty();
        }, warmup: 1, iterations: 3);

        Output.WriteLine($"GetAllFacturesAsync -> iters={iters}  mean={mean:F1}ms  median={median:F1}ms");

        median.Should().BeLessThan(5000);
    }
}

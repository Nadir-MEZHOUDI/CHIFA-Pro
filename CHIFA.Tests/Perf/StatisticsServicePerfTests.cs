using Xunit.Abstractions;

namespace CHIFA.Tests.Perf;

[Trait("Category", "Perf")]
public class StatisticsServicePerfTests : DbPerfTestBase
{
    public StatisticsServicePerfTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public async Task YearlyAsync_CompletesWithinBudget()
    {
        await using var db = new DataModel.ChifaDb();
        var rowCount = await db.Factures.CountAsync();
        Output.WriteLine($"facture rows: {rowCount:N0}");
        if (rowCount == 0) return;

        Stats.Period.From = new DateTime(2000, 1, 1);
        Stats.Period.To = new DateTime(2100, 1, 1);

        var (iters, mean, median, min, max) = await MeasureAsync(async () =>
        {
            var data = await Stats.YearlyAsync();
            data.Should().NotBeEmpty();
        }, warmup: 1, iterations: 3);

        Output.WriteLine($"YearlyAsync -> iters={iters}  mean={mean:F1}ms  median={median:F1}ms");
        median.Should().BeLessThan(5000);
    }

    [Fact]
    public async Task MonthlyAsync_CompletesWithinBudget()
    {
        await using var db = new DataModel.ChifaDb();
        var rowCount = await db.Factures.CountAsync();
        Output.WriteLine($"facture rows: {rowCount:N0}");
        if (rowCount == 0) return;

        Stats.Period.From = new DateTime(2000, 1, 1);
        Stats.Period.To = new DateTime(2100, 1, 1);

        var (iters, mean, median, min, max) = await MeasureAsync(async () =>
        {
            var data = await Stats.MonthlyAsync();
            data.Should().NotBeEmpty();
        }, warmup: 1, iterations: 3);

        Output.WriteLine($"MonthlyAsync -> iters={iters}  mean={mean:F1}ms  median={median:F1}ms");
        median.Should().BeLessThan(5000);
    }

    [Fact]
    public async Task DailyAsync_CompletesWithinBudget()
    {
        await using var db = new DataModel.ChifaDb();
        var rowCount = await db.Factures.CountAsync();
        Output.WriteLine($"facture rows: {rowCount:N0}");
        if (rowCount == 0) return;

        Stats.Period.From = new DateTime(2000, 1, 1);
        Stats.Period.To = new DateTime(2100, 1, 1);

        var (iters, mean, median, min, max) = await MeasureAsync(async () =>
        {
            var data = await Stats.DailyAsync();
            data.Should().NotBeEmpty();
        }, warmup: 1, iterations: 3);

        Output.WriteLine($"DailyAsync -> iters={iters}  mean={mean:F1}ms  median={median:F1}ms");
        median.Should().BeLessThan(5000);
    }
}

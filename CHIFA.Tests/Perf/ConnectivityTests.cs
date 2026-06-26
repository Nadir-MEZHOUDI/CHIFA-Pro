using DataModel;

using Xunit.Abstractions;

namespace CHIFA.Tests.Perf;

[Trait("Category", "Perf")]
public class ConnectivityTests(ITestOutputHelper output) : DbPerfTestBase(output)
{
    [Fact]
    public async Task Database_IsReachable()
    {
        await using var db = new ChifaDb();
        var one = await db.Bordereaus.Take(1).ToListAsync();
        one.Should().NotBeNull();
        Output.WriteLine($"DB reachable. Sampled {one.Count} row(s) from bordereau.");
    }

    [Fact]
    public async Task Database_RowCounts()
    {
        await using var db = new ChifaDb();
        var counts = new Dictionary<string, long>
        {
            ["bordereau"] = await db.Bordereaus.CountAsync(),
            ["facture"] = await db.Factures.CountAsync(),
            ["detail_fact"] = await db.DetailFacts.CountAsync(),
            ["beneficiaire"] = await db.Beneficiaires.CountAsync(),
            ["assure (benef rang 00)"] = await db.Beneficiaires.CountAsync(b => b.RangAd == "00"),
            ["medicament"] = await db.Medicaments.CountAsync(),
            ["centre"] = await db.Centres.CountAsync(),
        };
        foreach (var kv in counts)
            Output.WriteLine($"{kv.Key,-30} {kv.Value,12:N0}");
    }
}

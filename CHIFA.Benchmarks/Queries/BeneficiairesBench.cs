using System.Data;

using CHIFA.Contract.Dtos;

using DataModel;

namespace CHIFA.Benchmarks.Queries;

[Config(typeof(Config))]
[MemoryDiagnoser]
[BenchmarkCategory("Beneficiaires")]
public class BeneficiairesBench
{
    [Benchmark(Baseline = true), BenchmarkCategory("LinqToDB")]
    public async Task<List<BeneficiareDto>> LinqToDB_GetBeneficiares()
    {
        await using var db = new ChifaDb();

        var centerByBenef = await db.Factures
            .Where(f => f.NumAssure != null && f.RangAd != null)
            .GroupBy(f => new { f.NumAssure, f.RangAd })
            .Select(g => new
            {
                g.Key.NumAssure,
                g.Key.RangAd,
                MinNumFact = g.Min(f => f.NumFact)
            })
            .ToListAsync();

        var minFact = centerByBenef.Select(x => x.MinNumFact).Distinct().ToList();

        var centers = await db.Factures
            .Where(f => minFact.Contains(f.NumFact))
            .Select(f => new { f.NumFact, Center = f.Center!.Nom })
            .ToListAsync();

        var centerByNumFact = centers.ToDictionary(c => c.NumFact, c => c.Center);
        var centerByBenefDict = centerByBenef
            .Where(x => x.MinNumFact != null && centerByNumFact.ContainsKey(x.MinNumFact))
            .ToDictionary(x => (x.NumAssure, x.RangAd), x => centerByNumFact[x.MinNumFact!]);

        var rows = await db.Beneficiaires
            .Select(a => new
            {
                NumAssure = a.NumAssure,
                Rang = a.RangAd,
                Beneficiare = a.FullName,
                Assure = a.Assure!.FullName
            })
            .ToListAsync();

        var list = new List<BeneficiareDto>(rows.Count);
        foreach (var r in rows)
        {
            list.Add(new BeneficiareDto
            {
                NumAssure = r.NumAssure,
                Rang = r.Rang,
                Beneficiare = r.Beneficiare,
                Assure = r.Assure,
                Center = (r.NumAssure != null && r.Rang != null
                          && centerByBenefDict.TryGetValue((r.NumAssure, r.Rang), out var nom))
                    ? nom : ""
            });
        }
        return list;
    }

    [Benchmark, BenchmarkCategory("Dapper")]
    public async Task<List<BeneficiareDto>> Dapper_GetBeneficiares()
    {
        // 1 round-trip to get the first facture per (num_assure, rang_ad) using DISTINCT ON
        const string firstFactureSql = @"
            SELECT DISTINCT ON (f.num_assure, f.rang_ad)
                f.num_assure, f.rang_ad, f.num_fact, c.nom AS centre
            FROM facture f
            JOIN centre c ON c.code_centre = f.code_centre
            ORDER BY f.num_assure, f.rang_ad, f.num_fact;";

        using IDbConnection conn = DbConfig.OpenDapper();
        var firstFactures = (await conn.QueryAsync<FirstFactureRow>(firstFactureSql)).ToList();
        var centerByKey = firstFactures.ToDictionary(r => (r.num_assure!, r.rang_ad!), r => r.centre!);

        // 1 round-trip for beneficiaires
        const string benefSql = @"
            SELECT
                b.num_assure, b.rang_ad,
                b.nom || ' ' || b.prenom AS beneficiare,
                ass.nom || ' ' || ass.prenom AS assure
            FROM beneficiaire b
            JOIN beneficiaire ass ON ass.num_assure = b.num_assure AND ass.rang_ad = '00';";

        var rows = (await conn.QueryAsync<BenefRow>(benefSql)).ToList();

        var list = new List<BeneficiareDto>(rows.Count);
        foreach (var r in rows)
        {
            list.Add(new BeneficiareDto
            {
                NumAssure = r.num_assure,
                Rang = r.rang_ad,
                Beneficiare = r.beneficiare,
                Assure = r.assure,
                Center = (r.num_assure != null && r.rang_ad != null
                          && centerByKey.TryGetValue((r.num_assure, r.rang_ad), out var nom))
                    ? nom : ""
            });
        }
        return list;
    }

    private class FirstFactureRow
    {
        public string? num_assure { get; set; }
        public string? rang_ad { get; set; }
        public string? num_fact { get; set; }
        public string? centre { get; set; }
    }

    private class BenefRow
    {
        public string? num_assure { get; set; }
        public string? rang_ad { get; set; }
        public string? beneficiare { get; set; }
        public string? assure { get; set; }
    }
}

using System.Data;
using BenchmarkDotNet.Configs;
using CHIFA.Benchmarks;
using CHIFA.Contract.Statistics;
using DataModel;

namespace CHIFA.Benchmarks.Queries;

[Config(typeof(Config))]
[MemoryDiagnoser]
[BenchmarkCategory("Statistics")]
public class StatisticsBench
{
    [Benchmark(Baseline = true), BenchmarkCategory("LinqToDB")]
    public async Task<List<YearlyStat>> LinqToDB_Yearly()
    {
        await using var db = new ChifaDb();
        return await db.Factures
            .Where(f => f.DateFact != null)
            .GroupBy(x => x.DateFact!.Value.Date.Year)
            .Select(x => new YearlyStat
            {
                Year = x.Key,
                MontantFact = x.Sum(f => f.MontFact),
                MontantOff = x.Sum(f => f.MontOff),
                MontantMaj = x.Sum(f => f.MontMaj),
                MontantFE = x.Sum(f => f.MontMajFae),
                Factures = x.Count(),
                DateDebut = x.Min(f => f.DateFact),
                DateFin = x.Max(f => f.DateFact),
                Assureis = x.CountExt(f => f.NumAssure, Sql.AggregateModifier.Distinct),
                Beneficiaires = x.CountExt(f => f.NumAssure + "|" + f.RangAd, Sql.AggregateModifier.Distinct)
            })
            .OrderBy(x => x.Year)
            .ToListAsync();
    }

    [Benchmark, BenchmarkCategory("Dapper")]
    public async Task<List<YearlyStat>> Dapper_Yearly()
    {
        const string sql = @"
            SELECT
                EXTRACT(YEAR FROM date_fact)::int AS year,
                SUM(mont_fact) AS montant_fact,
                SUM(mont_off)  AS montant_off,
                SUM(mont_maj)  AS montant_maj,
                SUM(mont_maj_fae) AS montant_fe,
                COUNT(*) AS factures,
                MIN(date_fact) AS date_debut,
                MAX(date_fact) AS date_fin,
                COUNT(DISTINCT num_assure) AS assureis,
                COUNT(DISTINCT (num_assure || '|' || rang_ad)) AS beneficiaires
            FROM facture
            WHERE date_fact IS NOT NULL
            GROUP BY EXTRACT(YEAR FROM date_fact)
            ORDER BY year;";

        using IDbConnection conn = DbConfig.OpenDapper();
        var rows = await conn.QueryAsync<YearlyStat>(sql);
        return rows.ToList();
    }
}

using System.Data;
using BenchmarkDotNet.Configs;
using CHIFA.Benchmarks;
using CHIFA.Contract.Dtos;
using DataModel;

namespace CHIFA.Benchmarks.Queries;

[Config(typeof(Config))]
[MemoryDiagnoser]
[BenchmarkCategory("Bordereaux")]
public class BordereauxBench
{
    private const string PeriodFrom = "2000-01-01";
    private const string PeriodTo = "2100-01-01";

    [Benchmark(Baseline = true), BenchmarkCategory("LinqToDB")]
    public async Task<List<BordereauDto>> LinqToDB_GetAllBordereaux()
    {
        await using var db = new ChifaDb();
        var periodFrom = (DateTime?)new DateTime(2000, 1, 1);
        var periodTo = (DateTime?)new DateTime(2100, 1, 1);
        var aggregates = await db.Factures
            .Where(f => f.NumBord != null)
            .Where(f => f.DateFact != null
                && (periodFrom == null || f.DateFact > periodFrom)
                && (periodTo == null || f.DateFact < periodTo))
            .GroupBy(f => f.NumBord)
            .Select(g => new Aggregate
            {
                NumBord = g.Key,
                Nmbr = g.Count(),
                MontOff = g.Sum(m => m.MontOff),
                Maj = g.Sum(m => m.MontMaj),
                FirstFacture = g.Min(m => m.DateFact),
                LastFacture = g.Max(m => m.DateFact)
            })
            .ToListAsync();

        var aggByNum = aggregates.ToDictionary(a => a.NumBord!);

        var rows = await db.Bordereaus
            .Where(b => b.DateExtract != null
                && (periodFrom == null || b.DateExtract > periodFrom)
                && (periodTo == null || b.DateExtract < periodTo))
            .Select(x => new
            {
                x.NumBord,
                Center = x.Center!.Nom,
                x.MontVir,
                x.DateDepotFtp,
                x.DateCloture,
                x.DateOuverture,
                x.Etat
            })
            .ToListAsync();

        var list = new List<BordereauDto>(rows.Count);
        foreach (var x in rows)
        {
            aggByNum.TryGetValue(x.NumBord, out var a);
            list.Add(new BordereauDto
            {
                Num = x.NumBord,
                Center = x.Center,
                FirstFacture = a?.FirstFacture,
                LastFacture = a?.LastFacture,
                Nmbr = a?.Nmbr ?? 0,
                Virment = x.MontVir,
                DateDepotFtp = x.DateDepotFtp,
                DateCloture = x.DateCloture,
                DateOuverture = x.DateOuverture,
                State = x.Etat,
                MontOff = a?.MontOff,
                Maj = a?.Maj
            });
        }
        return list
            .OrderByDescending(x => x.FirstFacture)
            .ThenByDescending(x => x.Num)
            .ToList();
    }

    [Benchmark, BenchmarkCategory("Dapper")]
    public async Task<List<BordereauDto>> Dapper_GetAllBordereaux()
    {
        var aggregates = await AggregateFacturesByNumBordAsync();
        var aggByNum = aggregates.ToDictionary(a => a.NumBord!);

        // Filter by facture date range (no date_extract column on bordereau)
        const string sql = @"
            SELECT
                b.num_bord, c.nom AS center, b.mont_vir AS virment,
                b.date_depot_ftp, b.date_cloture, b.date_ouverture, b.etat AS state
            FROM bordereau b
            JOIN centre c ON c.code_centre = b.code_centre
            WHERE EXISTS (
                SELECT 1 FROM facture f
                WHERE f.num_bord = b.num_bord
                  AND f.date_fact IS NOT NULL
                  AND f.date_fact > @from
                  AND f.date_fact < @to
            )
            ORDER BY b.num_bord DESC;";

        using IDbConnection conn = DbConfig.OpenDapper();
        var rows = (await conn.QueryAsync<BordereauRow>(sql, new { from = new DateTime(2000, 1, 1), to = new DateTime(2100, 1, 1) })).ToList();

        var list = new List<BordereauDto>(rows.Count);
        foreach (var x in rows)
        {
            aggByNum.TryGetValue(x.num_bord, out var a);
            list.Add(new BordereauDto
            {
                Num = x.num_bord,
                Center = x.center,
                FirstFacture = a?.FirstFacture,
                LastFacture = a?.LastFacture,
                Nmbr = a?.Nmbr ?? 0,
                Virment = x.virment,
                DateDepotFtp = x.date_depot_ftp,
                DateCloture = x.date_cloture,
                DateOuverture = x.date_ouverture,
                State = x.state,
                MontOff = a?.MontOff,
                Maj = a?.Maj
            });
        }
        return list
            .OrderByDescending(x => x.FirstFacture)
            .ThenByDescending(x => x.Num)
            .ToList();
    }

    private static async Task<List<Aggregate>> AggregateFacturesByNumBordAsync()
    {
        await using var db = new ChifaDb();
        return await db.Factures
            .Where(f => f.NumBord != null)
            .GroupBy(f => f.NumBord)
            .Select(g => new Aggregate
            {
                NumBord = g.Key,
                Nmbr = g.Count(),
                MontOff = g.Sum(m => m.MontOff),
                Maj = g.Sum(m => m.MontMaj),
                FirstFacture = g.Min(m => m.DateFact),
                LastFacture = g.Max(m => m.DateFact)
            })
            .ToListAsync();
    }

    public class Aggregate
    {
        public string? NumBord { get; set; }
        public int Nmbr { get; set; }
        public decimal? MontOff { get; set; }
        public decimal? Maj { get; set; }
        public DateTime? FirstFacture { get; set; }
        public DateTime? LastFacture { get; set; }
    }

    public class BordereauRow
    {
        public string num_bord { get; set; } = "";
        public string center { get; set; } = "";
        public decimal? virment { get; set; }
        public DateTime? date_depot_ftp { get; set; }
        public DateTime? date_cloture { get; set; }
        public DateTime? date_ouverture { get; set; }
        public char? state { get; set; }
    }
}

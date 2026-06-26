using System.Data;
using BenchmarkDotNet.Configs;
using CHIFA.Benchmarks;
using DataModel;

namespace CHIFA.Benchmarks.Queries;

[Config(typeof(Config))]
[MemoryDiagnoser]
[BenchmarkCategory("Factures")]
public class FacturesBench
{
    private static readonly DateTime From = new(2000, 1, 1);
    private static readonly DateTime To = new(2100, 1, 1);

    [Benchmark(Baseline = true), BenchmarkCategory("LinqToDB")]
    public async Task<List<FactureRow>> LinqToDB_GetAllFactures()
    {
        await using var db = new ChifaDb();
        var list = await db.Factures
            .Where(f => f.DateFact != null
                && f.DateFact > From
                && f.DateFact < To)
            .OrderByDescending(f => f.DateFact)
            .ThenByDescending(f => f.NumFact)
            .Take(5_000)
            .Select(f => new FactureRow
            {
                num_fact = f.NumFact,
                num_bord = f.NumBord,
                num_assure = f.NumAssure,
                rang_ad = f.RangAd,
                code_centre = f.CodeCentre,
                code_sp = f.CodeSp,
                date_fact = f.DateFact,
                date_soin = f.DateSoin.HasValue ? DateOnly.FromDateTime(f.DateSoin.Value) : (DateOnly?)null,
                mont_fact = f.MontFact,
                mont_off = f.MontOff,
                mont_as = f.MontAs,
                mont_maj = f.MontMaj,
                centre = f.Center!.Nom ?? "",
                specialite = f.Specialite!.Libelle ?? "",
                assure = f.Assure.FullName,
                malade = f.Beneficiaire.FullName
            })
            .ToListAsync();
        return list;
    }

    [Benchmark, BenchmarkCategory("Dapper")]
    public async Task<List<FactureRow>> Dapper_GetAllFactures()
    {
        const string sql = @"
            SELECT
                f.num_fact, f.num_bord, f.num_assure, f.rang_ad, f.code_centre, f.code_sp,
                f.date_fact, f.date_soin, f.mont_fact, f.mont_off, f.mont_as, f.mont_maj,
                c.nom  AS centre,
                s.libelle AS specialite,
                ass.nom || ' ' || ass.prenom AS assure,
                ben.nom || ' ' || ben.prenom AS malade
            FROM facture f
            JOIN centre      c   ON c.code_centre = f.code_centre
            JOIN specialite  s   ON s.code_sp     = f.code_sp
            JOIN beneficiaire ass ON ass.num_assure = f.num_assure AND ass.rang_ad = '00'
            JOIN beneficiaire ben ON ben.num_assure = f.num_assure AND ben.rang_ad = f.rang_ad
            WHERE f.date_fact IS NOT NULL
              AND f.date_fact > @from
              AND f.date_fact < @to
            ORDER BY f.date_fact DESC, f.num_fact DESC
            LIMIT @take;";

        using IDbConnection conn = DbConfig.OpenDapper();
        var rows = (await conn.QueryAsync<FactureRow>(sql, new { from = From, to = To, take = 5_000 })).ToList();
        return rows;
    }
}

public class FactureRow
{
    public string num_fact { get; set; } = "";
    public string? num_bord { get; set; }
    public string? num_assure { get; set; }
    public string? rang_ad { get; set; }
    public string? code_centre { get; set; }
    public string? code_sp { get; set; }
    public DateTime? date_fact { get; set; }
    public DateOnly? date_soin { get; set; }
    public decimal? mont_fact { get; set; }
    public decimal? mont_off { get; set; }
    public decimal? mont_as { get; set; }
    public decimal? mont_maj { get; set; }
    public string centre { get; set; } = "";
    public string specialite { get; set; } = "";
    public string assure { get; set; } = "";
    public string malade { get; set; } = "";
}

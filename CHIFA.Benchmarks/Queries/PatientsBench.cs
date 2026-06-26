using System.Data;
using BenchmarkDotNet.Configs;
using CHIFA.Benchmarks;
using CHIFA.Contract.Dtos;
using DataModel;

namespace CHIFA.Benchmarks.Queries;

[Config(typeof(Config))]
[MemoryDiagnoser]
[BenchmarkCategory("PatientsWithTraitSpec")]
public class PatientsBench
{
    private static readonly DateTime YearAgo = new DateTime(2025, 1, 1);

    [Benchmark(Baseline = true), BenchmarkCategory("LinqToDB")]
    public async Task<List<PatientSpecRow>> LinqToDB_PatientsWithTraitSpec()
    {
        await using var db = new ChifaDb();
        var rows = await db.DetailFacts
            .Where(d => d.Facture.DateFact > YearAgo
                && ((d.Ppa >= 1000m && d.Qte >= 3) || d.Ts == true || d.DureeTrait >= 60))
            .Select(d => new PatientSpecRow
            {
                num_assure = d.Facture.NumAssure!,
                rang_ad = d.Facture.RangAd!,
                num_fact = d.NumFact,
                num_enr = d.NumEnr,
                code_dci = d.Medicament.CodeDci,
                date_fact = d.Facture.DateFact,
                duree = d.DureeTrait == 1 && d.Qte >= 3 ? 80 : d.DureeTrait,
                qte = d.Qte,
                ppa = d.Ppa,
                ts = d.Ts,
                assure = d.Facture.Assure.FullName,
                malade = d.Facture.Beneficiaire.FullName
            })
            .Take(5_000)
            .ToListAsync();
        return rows;
    }

    [Benchmark, BenchmarkCategory("Dapper")]
    public async Task<List<PatientSpecRow>> Dapper_PatientsWithTraitSpec()
    {
        const string sql = @"
            SELECT
                f.num_assure, f.rang_ad, d.num_fact, d.num_enr, m.code_dci,
                f.date_fact,
                CASE WHEN d.duree_trait = 1 AND d.qte >= 3 THEN 80 ELSE d.duree_trait END AS duree,
                d.qte, d.ppa, d.ts,
                ass.nom || ' ' || ass.prenom AS assure,
                ben.nom || ' ' || ben.prenom AS malade
            FROM detail_fact d
            JOIN facture       f   ON f.num_fact = d.num_fact
            JOIN medicament    m   ON m.num_enr  = d.num_enr
            JOIN beneficiaire  ass ON ass.num_assure = f.num_assure AND ass.rang_ad = '00'
            JOIN beneficiaire  ben ON ben.num_assure = f.num_assure AND ben.rang_ad = f.rang_ad
            WHERE f.date_fact > @yearAgo
              AND ((d.ppa >= 1000 AND d.qte >= 3) OR d.ts = TRUE OR d.duree_trait >= 60)
            LIMIT @take;";

        using IDbConnection conn = DbConfig.OpenDapper();
        var rows = (await conn.QueryAsync<PatientSpecRow>(sql, new { yearAgo = YearAgo, take = 5_000 })).ToList();
        return rows;
    }
}

public class PatientSpecRow
{
    public string num_assure { get; set; } = "";
    public string rang_ad { get; set; } = "";
    public string num_fact { get; set; } = "";
    public string num_enr { get; set; } = "";
    public string? code_dci { get; set; }
    public DateTime? date_fact { get; set; }
    public decimal? duree { get; set; }
    public decimal qte { get; set; }
    public decimal ppa { get; set; }
    public bool? ts { get; set; }
    public string assure { get; set; } = "";
    public string malade { get; set; } = "";
}

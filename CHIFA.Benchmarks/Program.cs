using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using CHIFA.Benchmarks;
using CHIFA.Benchmarks.Queries;
using CHIFA.Benchmarks.Smoke;

namespace CHIFA.Benchmarks;

public static class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine("=== CHIFA Benchmarks ===");
        Console.WriteLine($"Connection: {MaskPwd(DbConfig.ConnectionString)}");

        // quick connectivity check
        try
        {
            using var conn = DbConfig.OpenDapper();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT 1";
            cmd.ExecuteScalar();
            Console.WriteLine("DB OK");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("DB UNREACHABLE: " + ex.Message);
            return 2;
        }

        var config = ManualConfig.CreateMinimumViable()
            .AddJob(BenchmarkDotNet.Jobs.Job.Default
                .WithWarmupCount(2)
                .WithIterationCount(5)
                .WithUnrollFactor(1)
                .WithInvocationCount(1))
            .AddDiagnoser(BenchmarkDotNet.Diagnosers.MemoryDiagnoser.Default)
            .AddLogger(ConsoleLogger.Default)
            .WithOptions(ConfigOptions.DisableOptimizationsValidator);

        var summaries = new List<Summary>();
        var types = new[]
        {
            typeof(StatisticsBench),
            typeof(BordereauxBench),
            typeof(FacturesBench),
            typeof(BeneficiairesBench),
            typeof(PatientsBench),
            typeof(SmokeBench),
        };

        foreach (var t in types)
        {
            try
            {
                var s = BenchmarkRunner.Run(t, config, args: args);
                summaries.Add(s);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Benchmark {t.Name} failed: {ex.Message}");
            }
        }

        MarkdownReport.Save(summaries, "BenchmarkReport.md");
        Console.WriteLine();
        Console.WriteLine("Report: " + Path.GetFullPath("BenchmarkReport.md"));
        return 0;
    }

    private static string MaskPwd(string cs)
    {
        var idx = cs.IndexOf("Password=", StringComparison.OrdinalIgnoreCase);
        if (idx < 0) return cs;
        var end = cs.IndexOf(';', idx);
        if (end < 0) end = cs.Length;
        return cs[..(idx + 9)] + "****" + cs[end..];
    }
}

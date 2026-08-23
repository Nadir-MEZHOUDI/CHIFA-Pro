using System.Diagnostics;
using CHIFA.Services.DataServices;
using DataModel;
using Xunit.Abstractions;

namespace CHIFA.Tests.Perf;

public abstract class DbPerfTestBase : IDisposable
{
    protected ITestOutputHelper Output { get; }
    protected ChifaService Chifa { get; }
    protected StatisticsService Stats { get; }

    static DbPerfTestBase()
    {
        var host = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_HOST");
        var port = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_PORT") ?? "5432";
        var user = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_USER") ?? "pharm";
        var pwd = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_PWD") ?? "";
        var db = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_NAME") ?? "CHIFA_OFFICINE";

        if (!string.IsNullOrEmpty(host))
        {
            ChifaDb.ConString = $"Server={host};Port={port};User Id={user};Password={pwd};Database={db};Timeout=15;Command Timeout=60;";
        }
    }

    protected DbPerfTestBase(ITestOutputHelper output)
    {
        Output = output;
        Chifa = ChifaService.Instance;
        Stats = StatisticsService.Instance;
    }

    protected async Task<(int iterations, double meanMs, double medianMs, double minMs, double maxMs)>
        MeasureAsync(Func<Task> action, int warmup = 1, int iterations = 5)
    {
        for (var i = 0; i < warmup; i++)
            await action();

        var sw = new Stopwatch();
        var samples = new double[iterations];

        for (var i = 0; i < iterations; i++)
        {
            sw.Restart();
            await action();
            sw.Stop();
            samples[i] = sw.Elapsed.TotalMilliseconds;
        }

        Array.Sort(samples);
        var mean = samples.Average();
        var median = iterations % 2 == 0
            ? (samples[iterations / 2 - 1] + samples[iterations / 2]) / 2.0
            : samples[iterations / 2];

        Output.WriteLine($"Samples (ms): {string.Join(", ", samples.Select(s => s.ToString("F1")))}");
        Output.WriteLine($"Min={samples[0]:F1}  Median={median:F1}  Mean={mean:F1}  Max={samples[^1]:F1}");

        return (iterations, mean, median, samples[0], samples[^1]);
    }

    public void Dispose()
    {
    }
}

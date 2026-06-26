using BenchmarkDotNet.Configs;

namespace CHIFA.Benchmarks.Smoke;

[Config(typeof(Config))]
[MemoryDiagnoser]
[BenchmarkCategory("Smoke")]
public class SmokeBench
{
    [Benchmark(Baseline = true), BenchmarkCategory("LinqToDB")]
    public int LinqToDB_Dummy() => 1;

    [Benchmark, BenchmarkCategory("Dapper")]
    public int Dapper_Dummy() => 1;
}

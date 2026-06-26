namespace CHIFA.Benchmarks;

[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
public class Config : ManualConfig
{
    public Config()
    {
        AddJob(Job.Default
            .WithUnrollFactor(1)
            .WithInvocationCount(1)
            .WithWarmupCount(2)
            .WithIterationCount(5)
            .WithId("Quick"));
    }
}

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;

namespace CHIFA.Tests;

public sealed class TimeWatch(int budgetMs = 500, ITestOutputHelper? output = null,
    [CallerMemberName] string? label = null) : IDisposable
{
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private readonly string _label = label ?? "?";
    private ITestOutputHelper? _output = output;

    public long ElapsedMs => _sw.ElapsedMilliseconds;
    public bool ExceededBudget => _sw.ElapsedMilliseconds > budgetMs;
    public int BudgetMs => budgetMs;
    public string Label => _label;

    public void Attach(ITestOutputHelper output) => _output = output;

    public void Dispose()
    {
        _sw.Stop();
        if (ExceededBudget)
        {
            var line = $"[TimeWatch] {_label}: {_sw.ElapsedMilliseconds}ms (budget {budgetMs}ms, exceeded by {_sw.ElapsedMilliseconds - budgetMs}ms)";
            _output?.WriteLine(line);
            Console.Error.WriteLine(line);
        }
    }
}

using CHIFA.Services.Helpers;

namespace CHIFA.Tests.Helpers;

public class ReloadCancellationTests
{
    private static async Task WaitCancelledAsync(CancellationToken token)
    {
        var cancelled = new TaskCompletionSource();
        await using var _ = token.Register(() => cancelled.TrySetResult());
        await cancelled.Task.WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Next_CancelsThePreviousLoadAtOnce()
    {
        var reloads = new ReloadCancellation();

        var first = reloads.Next();
        var second = reloads.Next();

        first.IsCancellationRequested.Should().BeTrue();
        second.IsCancellationRequested.Should().BeFalse();
    }

    [Fact]
    public async Task Next_RunsTheCancelCallbacksOfThePreviousLoad()
    {
        var reloads = new ReloadCancellation();
        var first = reloads.Next();
        var callbackRan = new TaskCompletionSource();
        first.Register(() => callbackRan.TrySetResult());

        reloads.Next();

        await callbackRan.Task.WaitAsync(TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Stop_CancelsTheLoadInFlightAndEveryLaterOne()
    {
        var reloads = new ReloadCancellation();
        var inFlight = reloads.Next();

        reloads.Stop();

        await WaitCancelledAsync(inFlight);
        reloads.IsStopped.Should().BeTrue();
        reloads.Next().IsCancellationRequested.Should().BeTrue();
    }

    /// <summary>
    /// 25f972cd, 4261f747, d0189f3d, 50400d86: a load still running when its screen closed hit
    /// "Cannot access a disposed object" on the screen's synchronisation objects.
    /// </summary>
    [Fact]
    public async Task OldTokens_StayUsableAfterStop()
    {
        var reloads = new ReloadCancellation();
        var token = reloads.Next();
        reloads.Stop();

        var use = async () =>
        {
            await using var registration = token.Register(() => { });
            await Task.Delay(10, token);
        };

        await use.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public void ManyConcurrentReloads_LeaveExactlyOneLive()
    {
        var reloads = new ReloadCancellation();
        var tokens = new System.Collections.Concurrent.ConcurrentBag<CancellationToken>();

        Parallel.For(0, 200, _ => tokens.Add(reloads.Next()));

        tokens.Count(t => !t.IsCancellationRequested).Should().Be(1);
    }
}

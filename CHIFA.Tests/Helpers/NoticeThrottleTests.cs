using CHIFA.Services.Helpers;

namespace CHIFA.Tests.Helpers;

public class NoticeThrottleTests
{
    private static readonly DateTime T0 = new(2026, 11, 27, 10, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void LetsOneNoticeThroughPerInterval()
    {
        var throttle = new NoticeThrottle(TimeSpan.FromMinutes(1));

        throttle.TryEnter(T0).Should().BeTrue();
        throttle.TryEnter(T0.AddSeconds(1)).Should().BeFalse();
        throttle.TryEnter(T0.AddSeconds(59)).Should().BeFalse();
        throttle.TryEnter(T0.AddMinutes(1)).Should().BeTrue();
    }

    [Fact]
    public void EveryScreenFailingAtOnce_GivesOneNotice()
    {
        var throttle = new NoticeThrottle(TimeSpan.FromMinutes(1));
        var allowed = 0;

        Parallel.For(0, 100, _ =>
        {
            if (throttle.TryEnter(T0)) Interlocked.Increment(ref allowed);
        });

        allowed.Should().Be(1);
    }
}

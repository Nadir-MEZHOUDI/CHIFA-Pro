namespace CHIFA.Services.Helpers;

/// <summary>Lets a notice through at most once per <paramref name="interval"/>, whichever thread asks.</summary>
public sealed class NoticeThrottle(TimeSpan interval)
{
    private long _nextAllowedTicks = DateTime.MinValue.Ticks;

    public bool TryEnter(DateTime utcNow)
    {
        while (true)
        {
            var next = Interlocked.Read(ref _nextAllowedTicks);
            if (utcNow.Ticks < next) return false;
            if (Interlocked.CompareExchange(ref _nextAllowedTicks, (utcNow + interval).Ticks, next) == next) return true;
        }
    }
}

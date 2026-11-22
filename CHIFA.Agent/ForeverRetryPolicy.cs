using Microsoft.AspNetCore.SignalR.Client;

namespace CHIFA.Agent;

/// <summary>
/// The default policy gives up after 4 attempts (about 45 s); a pharmacy may lose internet for hours.
/// This one retries forever: fast at first, then every minute.
/// </summary>
public class ForeverRetryPolicy : IRetryPolicy
{
    private static readonly TimeSpan[] Delays =
        [TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30)];

    private static readonly TimeSpan MaxDelay = TimeSpan.FromSeconds(60);

    public TimeSpan? NextRetryDelay(RetryContext retryContext) => DelayFor(retryContext.PreviousRetryCount);

    public static TimeSpan DelayFor(long previousAttempts)
    {
        var delay = previousAttempts < Delays.Length ? Delays[previousAttempts] : MaxDelay;
        // Jitter, so hundreds of agents do not all reconnect in the same second after a gateway restart.
        return delay + TimeSpan.FromMilliseconds(Random.Shared.Next(0, 1000));
    }
}

namespace CHIFA.Services.Helpers;

/// <summary>
/// "Latest load wins" for a screen: each <see cref="Next"/> cancels the load still in flight, so a
/// superseded query stops on the server and gives its connection back instead of running to the end.
/// Sources are cancelled but never disposed: a load still holding an old token can always use it
/// (no <see cref="ObjectDisposedException"/> after the screen closes). They hold no timer or wait handle.
/// </summary>
public sealed class ReloadCancellation
{
    private readonly Lock _lock = new();
    private CancellationTokenSource? _current;
    private bool _stopped;

    public bool IsStopped
    {
        get { lock (_lock) return _stopped; }
    }

    /// <summary>Cancels the previous load and returns the token for the new one (already cancelled once stopped).</summary>
    public CancellationToken Next()
    {
        CancellationTokenSource? previous;
        CancellationTokenSource next;
        lock (_lock)
        {
            if (_stopped) return new CancellationToken(true);
            previous = _current;
            _current = next = new CancellationTokenSource();
        }

        CancelInBackground(previous);
        return next.Token;
    }

    /// <summary>Cancels the load in flight and every later <see cref="Next"/>: call it when the screen is disposed.</summary>
    public void Stop()
    {
        CancellationTokenSource? previous;
        lock (_lock)
        {
            _stopped = true;
            previous = _current;
            _current = null;
        }

        CancelInBackground(previous);
    }

    /// <summary>
    /// The token reads as cancelled at once; Npgsql's cancel request to the server runs on the thread pool,
    /// so the UI thread never waits on the network.
    /// </summary>
    private static void CancelInBackground(CancellationTokenSource? source)
    {
        if (source is null) return;
        source.CancelAsync().ContinueWith(static t => _ = t.Exception, TaskContinuationOptions.OnlyOnFaulted);
    }
}

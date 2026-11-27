namespace CHIFA.Gateway.Features.Usage;

/// <summary>
/// Counts the linked phones' API calls in memory and saves them about once a minute, so a phone call never waits
/// on an extra database write. Also knows which phones are online now: the app refreshes the pharmacy status every
/// 30 s while it is open.
/// </summary>
public class PhoneActivity(IDbContextFactory<GatewayDbContext> dbFactory, ILogger<PhoneActivity> logger)
{
    public static readonly TimeSpan OnlineWindow = TimeSpan.FromMinutes(2);

    // Algeria stays on UTC+1 all year (no daylight saving), and the container may have no time zone data.
    private static readonly TimeSpan AlgeriaOffset = TimeSpan.FromHours(1);

    private sealed class Counts
    {
        public Guid PharmacyId;
        public int Requests;
        public int Minutes;
    }

    public sealed record Seen(Guid PharmacyId, DateTime LastSeenAt, long Minute);

    private readonly Lock _lock = new();
    private Dictionary<(Guid Device, DateOnly Day), Counts> _pending = [];
    private readonly Dictionary<Guid, Seen> _seen = [];
    private readonly SemaphoreSlim _flushing = new(1, 1);

    public static DateOnly Today(DateTime utcNow) => DateOnly.FromDateTime(utcNow + AlgeriaOffset);

    public void Touch(Guid deviceId, Guid pharmacyId, DateTime utcNow)
    {
        var minute = utcNow.Ticks / TimeSpan.TicksPerMinute;
        lock (_lock)
        {
            var newMinute = !_seen.TryGetValue(deviceId, out var seen) || seen.Minute != minute;
            _seen[deviceId] = new Seen(pharmacyId, utcNow, minute);

            var key = (deviceId, Today(utcNow));
            if (!_pending.TryGetValue(key, out var counts))
                _pending[key] = counts = new Counts { PharmacyId = pharmacyId };
            counts.Requests++;
            if (newMinute)
                counts.Minutes++;
        }
    }

    /// <summary>Phones that called the gateway within <see cref="OnlineWindow"/>, by device id.</summary>
    public Dictionary<Guid, Seen> Online(DateTime utcNow)
    {
        lock (_lock)
            return _seen.Where(s => utcNow - s.Value.LastSeenAt <= OnlineWindow).ToDictionary();
    }

    /// <summary>Saves the counts gathered since the last save. The admin pages call it first, to show exact numbers.</summary>
    public async Task FlushAsync(CancellationToken ct)
    {
        await _flushing.WaitAsync(ct);
        try
        {
            Dictionary<(Guid Device, DateOnly Day), Counts> batch;
            Dictionary<Guid, DateTime> lastSeen;
            lock (_lock)
            {
                if (_pending.Count == 0)
                    return;
                batch = _pending;
                _pending = [];
                lastSeen = batch.Keys.Select(k => k.Device).Distinct().ToDictionary(id => id, id => _seen[id].LastSeenAt);

                // Only the last minute matters for counting minutes, and only recent phones for "online".
                foreach (var stale in _seen.Where(s => DateTime.UtcNow - s.Value.LastSeenAt > OnlineWindow + TimeSpan.FromMinutes(1)).Select(s => s.Key).ToList())
                    _seen.Remove(stale);
            }

            try
            {
                await SaveAsync(batch, lastSeen, ct);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogWarning(ex, "Could not save phone activity; kept for the next try");
                lock (_lock)
                {
                    foreach (var (key, counts) in batch)
                    {
                        if (_pending.TryGetValue(key, out var current))
                        {
                            current.Requests += counts.Requests;
                            current.Minutes += counts.Minutes;
                        }
                        else
                            _pending[key] = counts;
                    }
                }
            }
        }
        finally
        {
            _flushing.Release();
        }
    }

    private async Task SaveAsync(Dictionary<(Guid Device, DateOnly Day), Counts> batch, Dictionary<Guid, DateTime> lastSeen, CancellationToken ct)
    {
        var deviceIds = lastSeen.Keys.ToList();
        var days = batch.Keys.Select(k => k.Day).Distinct().ToList();

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var devices = await db.MobileDevices.Where(d => deviceIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, ct);
        var rows = await db.DeviceActivities
            .Where(a => deviceIds.Contains(a.DeviceId) && days.Contains(a.Day))
            .ToDictionaryAsync(a => (a.DeviceId, a.Day), ct);

        foreach (var ((deviceId, day), counts) in batch)
        {
            if (!devices.ContainsKey(deviceId))
                continue;
            if (rows.TryGetValue((deviceId, day), out var row))
            {
                row.Requests += counts.Requests;
                row.ActiveMinutes += counts.Minutes;
            }
            else
            {
                db.DeviceActivities.Add(new DeviceActivity
                {
                    DeviceId = deviceId, PharmacyId = counts.PharmacyId, Day = day, Requests = counts.Requests, ActiveMinutes = counts.Minutes
                });
            }
        }

        foreach (var (id, seenAt) in lastSeen)
            if (devices.TryGetValue(id, out var device) && (device.LastSeenAt is null || device.LastSeenAt < seenAt))
                device.LastSeenAt = seenAt;

        await db.SaveChangesAsync(ct);
    }
}

/// <summary>Saves <see cref="PhoneActivity"/> every minute and once more when the gateway stops.</summary>
public class PhoneActivityFlusher(PhoneActivity activity, ILogger<PhoneActivityFlusher> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
                await activity.FlushAsync(stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        try
        {
            await activity.FlushAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Could not save phone activity on shutdown");
        }
    }
}

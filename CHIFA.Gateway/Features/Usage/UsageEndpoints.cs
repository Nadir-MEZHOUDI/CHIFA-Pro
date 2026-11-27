using CHIFA.Gateway.Features.Admin;
using CHIFA.Gateway.Features.Agents;

namespace CHIFA.Gateway.Features.Usage;

public record UsageDay(DateOnly Day, int ActivePhones, int ActivePharmacies, int Requests, int Minutes);

public record UsageOverview(
    int Pharmacies,
    int PharmaciesOnline,
    int PharmaciesWithPhones,
    int LinkedPhones,
    int PhonesOnline,
    int ActivePhonesToday,
    int ActivePhones7Days,
    int ActivePhones30Days,
    int ActivePharmacies7Days,
    List<UsageDay> Days);

/// <param name="Since">When its agent connected.</param>
public record OnlinePharmacy(Guid Id, string Name, DateTime? Since);

public record OnlinePhone(Guid Id, string Name, Guid PharmacyId, string Pharmacy, DateTime LastSeenAt);

public record OnlineNow(List<OnlinePharmacy> Pharmacies, List<OnlinePhone> Phones);

/// <param name="LastSeenAt">The agent's last connect or disconnect.</param>
public record PharmacyUsage(
    Guid Id,
    string Name,
    bool IsActive,
    bool Online,
    DateTime? LastSeenAt,
    DateTime CreatedAt,
    string? LicenseEdition,
    DateTime? LicenseExpiresAt,
    int LinkedPhones,
    int PhonesOnline,
    DateTime? LastPhoneUse,
    int ActiveDays30,
    int Requests30,
    int Minutes30);

public record DeviceUsage(
    Guid Id,
    string Name,
    DateTime CreatedAt,
    DateTime? LastSeenAt,
    DateTime? RevokedAt,
    bool Online,
    int ActiveDays30,
    int Requests30,
    int Minutes30);

public record PharmacyUsageDetail(PharmacyUsage Pharmacy, List<DeviceUsage> Devices, List<UsageDay> Days);

/// <summary>The admin console: how much the phone app is used, per pharmacy, and who is online now.</summary>
public static class UsageEndpoints
{
    public const int PeriodDays = 30;

    public static void MapUsageEndpoints(this IEndpointRouteBuilder app)
    {
        var usage = app.MapGroup("/admin/usage").AllowAnonymous().AddEndpointFilter(AdminEndpoints.RequireAdminKey);

        usage.MapGet("", OverviewAsync);
        usage.MapGet("/online", OnlineAsync);
        usage.MapGet("/pharmacies", PharmaciesAsync);
        usage.MapGet("/pharmacies/{id:guid}", PharmacyAsync);
    }

    private sealed record ActivityRow(Guid DeviceId, Guid PharmacyId, DateOnly Day, int Requests, int ActiveMinutes);

    private sealed record DeviceRow(Guid Id, Guid PharmacyId, string Name, DateTime CreatedAt, DateTime? LastSeenAt, DateTime? RevokedAt);

    private static async Task<IResult> OverviewAsync(
        IDbContextFactory<GatewayDbContext> dbFactory, PhoneActivity activity, AgentRegistry registry, CancellationToken ct)
    {
        await activity.FlushAsync(ct);
        var now = DateTime.UtcNow;
        var today = PhoneActivity.Today(now);
        var from = today.AddDays(-(PeriodDays - 1));

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacies = await db.Pharmacies.CountAsync(ct);
        var linked = await db.MobileDevices.Where(d => d.RevokedAt == null).Select(d => d.PharmacyId).ToListAsync(ct);
        var rows = await ActivityAsync(db, from, null, ct);

        var phonesSince = (DateOnly day) => rows.Where(r => r.Day >= day).Select(r => r.DeviceId).Distinct().Count();
        return Results.Ok(new UsageOverview(
            pharmacies,
            registry.OnlinePharmacies.Count,
            linked.Distinct().Count(),
            linked.Count,
            activity.Online(now).Count,
            phonesSince(today),
            phonesSince(today.AddDays(-6)),
            phonesSince(from),
            rows.Where(r => r.Day >= today.AddDays(-6)).Select(r => r.PharmacyId).Distinct().Count(),
            Days(rows, from, today)));
    }

    private static async Task<IResult> OnlineAsync(
        IDbContextFactory<GatewayDbContext> dbFactory, PhoneActivity activity, AgentRegistry registry, CancellationToken ct)
    {
        var phones = activity.Online(DateTime.UtcNow);
        var agentIds = registry.OnlinePharmacies.ToList();
        var pharmacyIds = agentIds.Concat(phones.Values.Select(p => p.PharmacyId)).Distinct().ToList();
        var deviceIds = phones.Keys.ToList();

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacies = await db.Pharmacies.AsNoTracking()
            .Where(p => pharmacyIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, p => new { p.Name, p.LastSeenAt }, ct);
        var names = await db.MobileDevices.AsNoTracking()
            .Where(d => deviceIds.Contains(d.Id))
            .ToDictionaryAsync(d => d.Id, d => d.Name, ct);

        var online = agentIds
            .Where(pharmacies.ContainsKey)
            .Select(id => new OnlinePharmacy(id, pharmacies[id].Name, pharmacies[id].LastSeenAt))
            .OrderBy(p => p.Name)
            .ToList();
        var onlinePhones = phones
            .Where(p => names.ContainsKey(p.Key) && pharmacies.ContainsKey(p.Value.PharmacyId))
            .Select(p => new OnlinePhone(p.Key, names[p.Key], p.Value.PharmacyId, pharmacies[p.Value.PharmacyId].Name, p.Value.LastSeenAt))
            .OrderBy(p => p.Pharmacy).ThenBy(p => p.Name)
            .ToList();
        return Results.Ok(new OnlineNow(online, onlinePhones));
    }

    private static async Task<IResult> PharmaciesAsync(
        IDbContextFactory<GatewayDbContext> dbFactory, PhoneActivity activity, AgentRegistry registry, CancellationToken ct)
    {
        await activity.FlushAsync(ct);
        var now = DateTime.UtcNow;
        var from = PhoneActivity.Today(now).AddDays(-(PeriodDays - 1));

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacies = await db.Pharmacies.AsNoTracking().OrderBy(p => p.Name).ToListAsync(ct);
        var devices = (await DevicesAsync(db, null, ct)).ToLookup(d => d.PharmacyId);
        var rows = (await ActivityAsync(db, from, null, ct)).ToLookup(r => r.PharmacyId);
        var online = activity.Online(now);

        return Results.Ok(pharmacies.Select(p => Summary(p, devices[p.Id], rows[p.Id], online, registry)).ToList());
    }

    private static async Task<IResult> PharmacyAsync(
        Guid id, IDbContextFactory<GatewayDbContext> dbFactory, PhoneActivity activity, AgentRegistry registry, CancellationToken ct)
    {
        await activity.FlushAsync(ct);
        var now = DateTime.UtcNow;
        var today = PhoneActivity.Today(now);
        var from = today.AddDays(-(PeriodDays - 1));

        await using var db = await dbFactory.CreateDbContextAsync(ct);
        var pharmacy = await db.Pharmacies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);
        if (pharmacy is null)
            return Results.NotFound();

        var devices = await DevicesAsync(db, id, ct);
        var rows = await ActivityAsync(db, from, id, ct);
        var online = activity.Online(now);
        var byDevice = rows.ToLookup(r => r.DeviceId);

        var phones = devices
            .OrderBy(d => d.RevokedAt is not null).ThenByDescending(d => online.ContainsKey(d.Id)).ThenByDescending(d => d.LastSeenAt)
            .Select(d => new DeviceUsage(
                d.Id, d.Name, d.CreatedAt, d.LastSeenAt, d.RevokedAt, online.ContainsKey(d.Id),
                byDevice[d.Id].Count(), byDevice[d.Id].Sum(r => r.Requests), byDevice[d.Id].Sum(r => r.ActiveMinutes)))
            .ToList();
        return Results.Ok(new PharmacyUsageDetail(Summary(pharmacy, devices, rows, online, registry), phones, Days(rows, from, today)));
    }

    private static PharmacyUsage Summary(
        Pharmacy p, IEnumerable<DeviceRow> devices, IEnumerable<ActivityRow> rows, Dictionary<Guid, PhoneActivity.Seen> online, AgentRegistry registry)
    {
        var list = devices.ToList();
        var activity = rows.ToList();
        return new PharmacyUsage(
            p.Id, p.Name, p.IsActive, registry.IsOnline(p.Id), p.LastSeenAt, p.CreatedAt, p.LicenseEdition, p.LicenseExpiresAt,
            list.Count(d => d.RevokedAt is null),
            list.Count(d => online.ContainsKey(d.Id)),
            list.Max(d => d.LastSeenAt),
            activity.Select(r => r.Day).Distinct().Count(),
            activity.Sum(r => r.Requests),
            activity.Sum(r => r.ActiveMinutes));
    }

    private static List<UsageDay> Days(List<ActivityRow> rows, DateOnly from, DateOnly today)
    {
        var byDay = rows.ToLookup(r => r.Day);
        return Enumerable.Range(0, today.DayNumber - from.DayNumber + 1)
            .Select(i => from.AddDays(i))
            .Select(day => new UsageDay(
                day,
                byDay[day].Select(r => r.DeviceId).Distinct().Count(),
                byDay[day].Select(r => r.PharmacyId).Distinct().Count(),
                byDay[day].Sum(r => r.Requests),
                byDay[day].Sum(r => r.ActiveMinutes)))
            .ToList();
    }

    // A few rows per phone and day: summed here rather than in SQL, which keeps the queries simple.
    private static Task<List<ActivityRow>> ActivityAsync(GatewayDbContext db, DateOnly from, Guid? pharmacyId, CancellationToken ct) =>
        db.DeviceActivities.AsNoTracking()
            .Where(a => a.Day >= from && (pharmacyId == null || a.PharmacyId == pharmacyId))
            .Select(a => new ActivityRow(a.DeviceId, a.PharmacyId, a.Day, a.Requests, a.ActiveMinutes))
            .ToListAsync(ct);

    private static Task<List<DeviceRow>> DevicesAsync(GatewayDbContext db, Guid? pharmacyId, CancellationToken ct) =>
        db.MobileDevices.AsNoTracking()
            .Where(d => pharmacyId == null || d.PharmacyId == pharmacyId)
            .Select(d => new DeviceRow(d.Id, d.PharmacyId, d.Name, d.CreatedAt, d.LastSeenAt, d.RevokedAt))
            .ToListAsync(ct);
}

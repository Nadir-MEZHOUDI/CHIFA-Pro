using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;
using CHIFA.Contract;

namespace CHIFA.Agent;

/// <summary>
/// The watch rules, in a file next to alerts.json. Like it, the file is shared by the Windows service and
/// SmartCHIFA, so the data stays on the pharmacy computer whichever of the two answers the phones.
/// </summary>
public class WatchlistStore(ILogger logger)
{
    private readonly SemaphoreSlim _lock = new(1, 1);

    public static string Path =>
        System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SmartCHIFA", "watchlist.json");

    public async Task<Watchlist> LoadAsync()
    {
        await _lock.WaitAsync();
        try
        {
            await using var stream = File.OpenRead(Path);
            return await JsonSerializer.DeserializeAsync<Watchlist>(stream) ?? new Watchlist();
        }
        catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
        {
            return new Watchlist();
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Unreadable {Path}, no watch rules", Path);
            return new Watchlist();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<Watchlist> SaveAsync(Watchlist watchlist)
    {
        var clean = Normalize(watchlist);
        await _lock.WaitAsync();
        try
        {
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path)!);
            var temp = Path + ".tmp";
            await File.WriteAllTextAsync(temp, JsonSerializer.Serialize(clean));
            AllowUsers(temp);
            File.Move(temp, Path, overwrite: true);
            return clean;
        }
        finally
        {
            _lock.Release();
        }
    }

    public static Watchlist Normalize(Watchlist w) => new(
        w.Psychotropes,
        w.MontantMin > 0 ? Math.Round(w.MontantMin.Value, 2) : null,
        w.BoitesMin > 0 ? w.BoitesMin : null,
        w.Patients
            .Where(p => !string.IsNullOrWhiteSpace(p.NumAssure) && !string.IsNullOrWhiteSpace(p.Rang))
            .Select(p => p with { NumAssure = p.NumAssure.Trim(), Rang = p.Rang.Trim(), Nom = p.Nom?.Trim() })
            .DistinctBy(p => (p.NumAssure, p.Rang))
            .Take(Watchlist.MaxItems)
            .ToList(),
        w.Medicaments
            .Where(m => !string.IsNullOrWhiteSpace(m.NumEnr))
            .Select(m => m with { NumEnr = m.NumEnr.Trim(), Nom = m.Nom?.Trim() })
            .DistinctBy(m => m.NumEnr)
            .Take(Watchlist.MaxItems)
            .ToList());

    /// <summary>
    /// A file written by the LocalSystem service is read-only for the pharmacist's Windows account, and
    /// SmartCHIFA could then no longer save the rules when it answers the phones itself.
    /// </summary>
    private void AllowUsers(string file)
    {
        if (!OperatingSystem.IsWindows())
            return;
        try
        {
            var info = new FileInfo(file);
            var security = info.GetAccessControl();
            security.AddAccessRule(new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), FileSystemRights.Modify, AccessControlType.Allow));
            info.SetAccessControl(security);
        }
        catch (Exception ex) when (ex is UnauthorizedAccessException or IOException or InvalidOperationException)
        {
            logger.LogDebug(ex, "Could not open {Path} to the Users group", file);
        }
    }
}

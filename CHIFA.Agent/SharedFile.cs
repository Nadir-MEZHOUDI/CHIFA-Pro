using System.Security.AccessControl;
using System.Security.Principal;

namespace CHIFA.Agent;

/// <summary>The agent's own files under ProgramData, shared by the Windows service and SmartCHIFA.</summary>
public static class SharedFile
{
    public static string Folder =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SmartCHIFA");

    /// <summary>Replaces the file in one move, so a reader never sees half of it.</summary>
    public static async Task WriteAsync(string path, string content, ILogger logger)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var temp = path + ".tmp";
        await File.WriteAllTextAsync(temp, content);
        AllowUsers(temp, logger);
        File.Move(temp, path, overwrite: true);
    }

    /// <summary>
    /// A file written by the LocalSystem service is read-only for the pharmacist's Windows account, and
    /// SmartCHIFA could then no longer save it when it answers the phones itself.
    /// </summary>
    private static void AllowUsers(string file, ILogger logger)
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

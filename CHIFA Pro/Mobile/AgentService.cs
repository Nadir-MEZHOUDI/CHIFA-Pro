using System.ComponentModel;
using System.ServiceProcess;
using CHIFA.Pro.Helpers.Settings;
using Npgsql;

namespace CHIFA.Pro.Mobile;

internal enum AgentServiceState
{
    NotInstalled,
    Stopped,
    Running,

    /// <summary>Running, but SmartCHIFA was updated since: its agent is newer.</summary>
    Outdated
}

/// <summary>
/// The CHIFA.Agent Windows service answers phones while SmartCHIFA is closed. SmartCHIFA installs it with
/// <c>install-agent.ps1</c> (shipped next to it), which needs one UAC confirmation, and again after an
/// update to bring the service's agent up to date.
/// </summary>
internal static class AgentService
{
    public const string Name = "CHIFA.Agent";

    private const int UacCancelled = 1223;

    private static string InstalledDirectory =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "SmartCHIFA", "Agent");

    private static string LogFile =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "SmartCHIFA", "agent-install.log");

    /// <summary>The agent published complete next to SmartCHIFA (see BuildAgentForService in the project file).</summary>
    private static string ShippedDirectory => Path.Combine(AppContext.BaseDirectory, "agent");

    public static AgentServiceState State
    {
        get
        {
            ServiceControllerStatus status;
            try
            {
                using var service = new ServiceController(Name);
                status = service.Status;
            }
            catch (InvalidOperationException)
            {
                return AgentServiceState.NotInstalled;
            }

            if (status != ServiceControllerStatus.Running)
                return AgentServiceState.Stopped;
            return IsOutdated() ? AgentServiceState.Outdated : AgentServiceState.Running;
        }
    }

    /// <summary>Installs or updates the service. Windows asks the user to confirm (UAC).</summary>
    public static async Task InstallAsync()
    {
        var script = Path.Combine(ShippedDirectory, "install-agent.ps1");
        if (!File.Exists(script))
            throw new MobileLinkException("Les fichiers du service sont absents de cette installation de SmartCHIFA.");

        var database = new NpgsqlConnectionStringBuilder(ChifaDb.ConString);
        // Lets the service start CHIFA_OFFICINE's PostgreSQL when it finds it stopped.
        var folder = DbChecker.IsChifaFolder(AppSettings.Default.ChifaPath)
            ? $" -ChifaFolder \"{AppSettings.Default.ChifaPath.TrimEnd('\\', '/')}\""
            : "";
        var start = new ProcessStartInfo("powershell.exe")
        {
            Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{script}\" -Source \"{ShippedDirectory}\" " +
                        $"-ChifaHost \"{database.Host}\" -ChifaPort {database.Port}{folder}",
            UseShellExecute = true,
            Verb = "runas",
            WindowStyle = ProcessWindowStyle.Hidden
        };

        Process? process;
        try
        {
            process = Process.Start(start);
        }
        catch (Win32Exception ex) when (ex.NativeErrorCode == UacCancelled)
        {
            throw new MobileLinkException("Installation annulée : Windows n'a pas reçu l'autorisation.");
        }

        if (process is null)
            throw new MobileLinkException("Impossible de lancer l'installation du service.");

        using (process)
        {
            await process.WaitForExitAsync();
            if (process.ExitCode != 0)
            {
                Log.Warning("Agent service install failed with code {Code}, see {Log}", process.ExitCode, LogFile);
                throw new MobileLinkException($"L'installation du service a échoué. Détails : {LogFile}");
            }
        }

        Log.Information("Agent service installed from {Source}", ShippedDirectory);
        // The service now answers the phones: a second agent in SmartCHIFA would fight it for the connection.
        await MobileLink.SyncWithServiceAsync();
    }

    /// <summary>Each SmartCHIFA build stamps its own version: a different one means the service runs an older agent.</summary>
    private static bool IsOutdated()
    {
        var installed = Path.Combine(InstalledDirectory, "CHIFA.Agent.dll");
        var shipped = Path.Combine(ShippedDirectory, "CHIFA.Agent.dll");
        if (!File.Exists(installed) || !File.Exists(shipped))
            return false;
        return FileVersionInfo.GetVersionInfo(installed).FileVersion != FileVersionInfo.GetVersionInfo(shipped).FileVersion;
    }
}

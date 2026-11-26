using CHIFA.Pro.Helpers.Settings;
using Microsoft.Win32.SafeHandles;
using Npgsql;

namespace CHIFA.Pro.Helpers;

public static class DbChecker
{
    /// <summary>CHIFA_OFFICINE always listens on 5432; the port is not configurable.</summary>
    public const int DefaultDbPort = DatabaseDefaults.Port;

    /// <summary>Major version prefix of the portable PostgreSQL server bundled with the app.</summary>
    private const string SupportedServerMajorVersion = "9.";

    private static bool _isConnected;

    /// <summary>
    /// Clears the cached connection state so the next check re-evaluates the current server,
    /// for example after the user changes the connection settings without restarting the app.
    /// </summary>
    internal static void ResetServerState() => _isConnected = false;

    /// <param name="quiet">Only log to the file: used while waiting for a server that is still starting.</param>
    public static async Task<bool> CheckDbConnectionAsync(string? conStr = null, bool quiet = false)
    {
        NpgsqlConnection? con = null;
        try
        {
            var connectionString = BuildConnectionString(conStr);
            var host = new NpgsqlConnectionStringBuilder(connectionString).Host;
            if (!DbConnectionRules.TryNormalizeHost(host, out _, out var hostError))
            {
                Serilog.Log.Warning("Invalid database host {Host}: {Error}", host, hostError);
                if (!quiet) XtraMessageBox.Show(hostError, "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            con = new NpgsqlConnection(connectionString);
            await con.OpenAsync();
            return true;
        }
        catch (Exception ex)
        {
            if (quiet) Serilog.Log.Debug(ex, "Database not reachable yet");
            else ex.Log(reportToTelemetry: !DbConnectionRules.IsExpectedConnectionError(ex));
            return false;
        }
        finally
        {
            if (con != null)
                await con.CloseAsync();
        }
    }

    private static string BuildConnectionString(string? connectionOrHost)
    {
        if (string.IsNullOrWhiteSpace(connectionOrHost))
            return ChifaDb.ConString;

        var input = connectionOrHost.Trim();

        try
        {
            _ = new NpgsqlConnectionStringBuilder(input);
            return input;
        }
        catch (ArgumentException)
        {
            var baseConnection = new NpgsqlConnectionStringBuilder(ChifaDb.ConString);
            var (host, port) = ParseHostAndPort(input);

            if (!string.IsNullOrWhiteSpace(host))
                baseConnection.Host = host;

            if (port is > 0)
                baseConnection.Port = port.Value;

            return baseConnection.ConnectionString;
        }
    }

    private static (string Host, int? Port) ParseHostAndPort(string value)
    {
        if (Uri.TryCreate($"tcp://{value}", UriKind.Absolute, out var uri) &&
            !string.IsNullOrWhiteSpace(uri.Host))
        {
            return (uri.Host, uri.IsDefaultPort ? null : uri.Port);
        }

        var separatorIndex = value.LastIndexOf(':');
        if (separatorIndex > 0 &&
            separatorIndex < value.Length - 1 &&
            int.TryParse(value[(separatorIndex + 1)..], out var parsedPort) &&
            parsedPort > 0)
        {
            return (value[..separatorIndex], parsedPort);
        }

        return (value, null);
    }

    /// <summary>True when <paramref name="folder"/> is a CHIFA_OFFICINE installation with its portable server.</summary>
    public static bool IsChifaFolder(string? folder) =>
        !string.IsNullOrWhiteSpace(folder)
        && File.Exists(Path.Combine(folder, "CHIFA_OFFICINE_DB", "bin", "postgres.exe"))
        && File.Exists(Path.Combine(folder, "CHIFA_OFFICINE_DB", "Lancer_Serveur.bat"));

    public static bool IsLocalHost(string? host) =>
        string.IsNullOrWhiteSpace(host)
        || host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
        || host is "127.0.0.1" or "::1"
        || host.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase);

    /// <summary>The database is on this computer: SmartCHIFA may start CHIFA_OFFICINE's server.</summary>
    public static bool IsLocalServer =>
        AppSettings.Default.IsServer || IsLocalHost(new NpgsqlConnectionStringBuilder(ChifaDb.ConString).Host);

    /// <summary>
    /// Runs before the license dialog, which fills the pharmacy's details from the database.
    /// Starts CHIFA_OFFICINE's server when it is on this computer, otherwise asks where the database is.
    /// Returns <c>false</c> when the application must exit.
    /// </summary>
    public static bool EnsureConnectedAtStartup()
    {
        var connected = Task.Run(() => CheckDbConnectionAsync(quiet: true)).GetAwaiter().GetResult();

        if (!connected && IsLocalServer && IsChifaFolder(AppSettings.Default.ChifaPath))
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowDefaultWaitForm("SmartCHIFA", "Démarrage du serveur CHIFA_OFFICINE…");
            try
            {
                connected = Task.Run(StartLocalServerAsync).GetAwaiter().GetResult();
            }
            finally
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseDefaultWaitForm();
            }
        }

        if (connected)
        {
            _isConnected = true;
            return true;
        }

        using var form = new ConnectionForm(startup: true);
        return form.ShowDialog() == DialogResult.OK;
    }

    /// <summary>
    /// Starts CHIFA_OFFICINE's portable PostgreSQL silently, as CHIFA_OFFICINE does itself
    /// (Lancer_Serveur.bat in a hidden window), unless it already runs. True once it accepts connections.
    /// </summary>
    public static async Task<bool> StartLocalServerAsync()
    {
        try
        {
            // The portable PostgreSQL 9.x is the only supported server; a system installation
            // (e.g. PostgreSQL 18) must never be started in its place.
            if (!IsChifaFolder(AppSettings.Default.ChifaPath))
                return false;

            if (!IsPortableServerRunning())
            {
                if (!IsSupportedServerVersion()) return false;

                // The batch file tests "IF EXIST data" relative to the current directory.
                using var process = Process.Start(new ProcessStartInfo(AppSettings.Default.ChifaLancerServeur)
                {
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    WorkingDirectory = AppSettings.Default.ChifaDbFolder
                });
            }

            var local = new NpgsqlConnectionStringBuilder(ChifaDb.ConString) { Host = "localhost", Port = DefaultDbPort };
            for (var attempt = 0; attempt < 20; attempt++)
            {
                if (await CheckDbConnectionAsync(local.ConnectionString, quiet: true)) return true;
                await Task.Delay(1000);
            }
        }
        catch (Exception ex)
        {
            ex.Log(showMessage: false);
        }

        return false;
    }

    /// <summary>
    /// True when the portable server data directory is PostgreSQL 9.x (the version the application
    /// expects). Returns true when the version cannot be determined, so an unknown layout is not blocked.
    /// </summary>
    private static bool IsSupportedServerVersion()
    {
        try
        {
            var versionFile = Path.Combine(AppSettings.Default.ChifaData, "PG_VERSION");
            if (!File.Exists(versionFile)) return true;

            var version = File.ReadLines(versionFile).FirstOrDefault()?.Trim();

            // PG_VERSION holds values such as "9.6" or "18".
            if (version is not null && version.StartsWith(SupportedServerMajorVersion, StringComparison.Ordinal))
                return true;

            Serilog.Log.Warning("Unsupported portable PostgreSQL version {Version}; expected 9.x. Server will not be started.", version);
            return false;
        }
        catch (Exception ex)
        {
            ex.Log(false);
            return true;
        }
    }

    /// <summary>True when a postgres process is running from the portable PostgreSQL 9.x bundle.</summary>
    private static bool IsPortableServerRunning()
    {
        var portableBin = Path.GetFullPath(AppSettings.Default.ChifaBin)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            + Path.DirectorySeparatorChar;

        var running = false;
        foreach (var process in Process.GetProcessesByName("postgres"))
        {
            using (process)
            {
                if (running) continue;
                var path = GetProcessImagePath(process.Id);
                running = path is not null && Path.GetFullPath(path).StartsWith(portableBin, StringComparison.OrdinalIgnoreCase);
            }
        }

        return running;
    }

    private const uint ProcessQueryLimitedInformation = 0x1000;

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern SafeProcessHandle OpenProcess(uint desiredAccess, bool inheritHandle, int processId);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "QueryFullProcessImageNameW")]
    private static extern bool QueryFullProcessImageName(SafeProcessHandle process, uint flags, [Out] char[] exeName, ref int size);

    /// <summary>
    /// The executable path of a process, or null when it cannot be read. Unlike <see cref="Process.MainModule"/>,
    /// this only needs PROCESS_QUERY_LIMITED_INFORMATION, so it works for a postgres run by a service account;
    /// a process that exited or stays protected is expected and not reported.
    /// </summary>
    private static string? GetProcessImagePath(int processId)
    {
        using var handle = OpenProcess(ProcessQueryLimitedInformation, false, processId);
        if (handle.IsInvalid)
        {
            Serilog.Log.Debug("Cannot open process {ProcessId}: Win32 error {Error}", processId, Marshal.GetLastWin32Error());
            return null;
        }

        var buffer = new char[1024];
        var size = buffer.Length;
        return QueryFullProcessImageName(handle, 0, buffer, ref size) ? new string(buffer, 0, size) : null;
    }
    public static async Task RunServerAsync()
    {
        if (!IsLocalServer) return;

        if (_isConnected) return;
        try
        {
            _isConnected = await CheckDbConnectionAsync(quiet: true) || await StartLocalServerAsync();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private static string BackupFileName => $"CHIFA_OFFICINE_{DateTime.Now:dd-MM-yyyy__HH-mm-ss}.backup";

    /// <summary>
    /// Returns true when the portable PostgreSQL tool exists; system PostgreSQL tools are never used.
    /// Otherwise tells the user why the backup/restore they asked for cannot run on this computer.
    /// </summary>
    private static bool EnsureToolExists(string toolPath, string caption)
    {
        if (File.Exists(toolPath)) return true;

        if (!Directory.Exists(AppSettings.Default.ChifaDbFolder))
        {
            Serilog.Log.Information("Portable PostgreSQL not installed at {Folder}; backup/restore skipped.", AppSettings.Default.ChifaDbFolder);
            MessageBox.Show(
                $@"Le dossier « CHIFA_OFFICINE_DB » est introuvable sur ce poste :
{AppSettings.Default.ChifaDbFolder}
Lancez cette opération depuis le poste serveur où CHIFA_OFFICINE est installé, ou corrigez le chemin CHIFA dans Paramètres.",
                caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        Serilog.Log.Warning("Portable PostgreSQL tool not found: {ToolPath}", toolPath);
        MessageBox.Show(
            $@"Outil PostgreSQL portable introuvable :
{toolPath}
Vérifiez l'installation du dossier « CHIFA_OFFICINE_DB » dans le chemin CHIFA défini dans Paramètres.",
            caption, MessageBoxButtons.OK, MessageBoxIcon.Error);

        return false;
    }

    public static async Task SaveBackup()
    {
        try
        {
            if (!EnsureToolExists(AppSettings.Default.ChifaDump, @"Sauvegarde"))
                return;

            Environment.SetEnvironmentVariable(@"PGPASSWORD", AppSettings.Default.DbPassword);

            if (!Directory.Exists(AppSettings.Default.ChifaBackup))
            {
                Directory.CreateDirectory(AppSettings.Default.ChifaBackup);
            }

            var backupFile = Path.Combine(AppSettings.Default.ChifaBackup, BackupFileName);

            ProcessStartInfo startInfo = new()
            {
                FileName = AppSettings.Default.ChifaDump,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = $"""-h "{ChifaDb.Server}" -p {DefaultDbPort} -U pharm -c -Ft -d CHIFA_OFFICINE -f "{backupFile}" """
            };

            using Process process = new() { StartInfo = startInfo };

            process.Start();

            // Drain both streams while the process runs, otherwise a full pipe buffer can deadlock the wait.
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            await outputTask;
            var error = await errorTask;

            if (process.ExitCode == 0)
            {
                MessageBox.Show(@"Sauvegarde effectuée avec succès.", @"Sauvegarde", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(@"Échec de la sauvegarde. Erreur : " + error, @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
    public static async Task Restore(string fileName)
    {
        try
        {
            if (!EnsureToolExists(AppSettings.Default.ChifaRestore, @"Restauration"))
                return;

            Environment.SetEnvironmentVariable("PGPASSWORD", AppSettings.Default.DbPassword);
            ProcessStartInfo processStartInfo = new()
            {
                FileName = AppSettings.Default.ChifaRestore,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Arguments = $"""-h "{ChifaDb.Server}" -c  -p {DefaultDbPort} -U pharm -d CHIFA_OFFICINE "{fileName}" """,
            };

            using Process process = new();
            process.StartInfo = processStartInfo;
            process.Start();

            // Drain both streams while the process runs, otherwise a full pipe buffer can deadlock the wait.
            var outputTask = process.StandardOutput.ReadToEndAsync();
            var errorTask = process.StandardError.ReadToEndAsync();

            await process.WaitForExitAsync();

            await outputTask;
            var error = await errorTask;

            if (process.ExitCode == 0)
            {
                MessageBox.Show(@"Restauration de la base de données effectuée avec succès.", @"Restauration", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(@"Échec de la restauration de la base de données. Erreur : " + error, @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}

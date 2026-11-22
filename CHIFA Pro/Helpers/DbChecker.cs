using CHIFA.Pro.Helpers.Settings;
using Npgsql;

namespace CHIFA.Pro.Helpers;

public static class DbChecker
{
    /// <summary>Default PostgreSQL port (5432), shared with the whole solution.</summary>
    public const int DefaultDbPort = DatabaseDefaults.Port;

    /// <summary>Major version prefix of the portable PostgreSQL server bundled with the app.</summary>
    private const string SupportedServerMajorVersion = "9.";

    private static bool _isConnected;
    //private static readonly Func<bool> ChangeSettingsMsg = () => XtraMessageBox.Show("Cannot connect to database \n Do you want to Change Settings?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;

    /// <summary>
    /// Port used by the PostgreSQL command-line tools: an explicit <c>CHIFA_DB_PORT</c> wins,
    /// otherwise the supplied fallback, otherwise <see cref="DefaultDbPort"/> (5432).
    /// </summary>
    public static int GetDbPort(int? fallbackPort = null)
    {
        if (int.TryParse(Environment.GetEnvironmentVariable("CHIFA_DB_PORT"), out var port) && port > 0)
            return port;

        return fallbackPort is > 0 ? fallbackPort.Value : DefaultDbPort;
    }

    /// <summary>
    /// Clears the cached connection state so the next check re-evaluates the current server,
    /// for example after the user changes the connection settings without restarting the app.
    /// </summary>
    internal static void ResetServerState() => _isConnected = false;

    public static async Task<bool> CheckDbConnectionAsync(string? conStr = null)
    {
        var connectionString = BuildConnectionString(conStr);
        NpgsqlConnection? con = null;
        try
        {
            con = new NpgsqlConnection(connectionString);
            await con.OpenAsync();
            return true;
        }
        catch (Exception ex)
        {
            ex.Log();
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

    private static bool CheckOrDownloadServer()
    {
        try
        {
            // The portable PostgreSQL 9.x is the only supported server; a system installation
            // (e.g. PostgreSQL 18) must never be detected or used. If the portable bundle is
            // missing, skip launching instead of starting an unrelated server.
            if (!File.Exists(AppSettings.Default.ChifaPostgres))
                return false;

            if (IsPortableServerRunning()) return true;

            if (!IsSupportedServerVersion()) return false;

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(AppSettings.Default.ChifaLancerServeur)
                {
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.WaitForExit(1000);
            // process.Close();
            //process.Dispose();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        return IsPortableServerRunning();
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

        foreach (var process in Process.GetProcessesByName("postgres"))
        {
            try
            {
                var path = process.MainModule?.FileName;
                if (path is not null && Path.GetFullPath(path).StartsWith(portableBin, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            catch (Exception ex)
            {
                // A 32/64-bit mismatch or access denied reading MainModule; ignore this process.
                ex.Log(false);
            }
        }

        return false;
    }
    public static async Task RunServerAsync()
    {
        if (!AppSettings.Default.IsServer) return;

        if (_isConnected) return;
        try
        {
            if (!CheckOrDownloadServer())
            {
                //   MessageBox.Show(@"Postgres SQL Server Not working! and cannot run it!", @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _isConnected = await CheckDbConnectionAsync();

            if (!_isConnected)
            {
                // ParametersUc.ShowAsForm();
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private static string BackupFileName => $"CHIFA_OFFICINE_{DateTime.Now:dd-MM-yyyy__HH-mm-ss}.backup";

    /// <summary>
    /// Returns true when the portable PostgreSQL tool exists.
    /// Skips silently when the portable database folder is not installed at all
    /// (nothing to back up on this machine); system PostgreSQL tools are never used.
    /// Warns only when the folder exists but the tool is missing (broken installation).
    /// </summary>
    private static bool EnsureToolExists(string toolPath, string caption)
    {
        if (File.Exists(toolPath)) return true;

        if (!Directory.Exists(AppSettings.Default.ChifaDbFolder))
        {
            Serilog.Log.Information("Portable PostgreSQL not installed at {Folder}; backup/restore skipped.", AppSettings.Default.ChifaDbFolder);
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
                Arguments = $"""-h "{ChifaDb.Server}" -p {GetDbPort()} -U pharm -c -Ft -d CHIFA_OFFICINE -f "{backupFile}" """
            };

            Process process = new() { StartInfo = startInfo };

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
                Arguments = $"""-h "{ChifaDb.Server}" -c  -p {GetDbPort()} -U pharm -d CHIFA_OFFICINE "{fileName}" """,
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

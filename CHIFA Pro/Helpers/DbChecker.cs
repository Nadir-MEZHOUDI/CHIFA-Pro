using Npgsql;

namespace CHIFA.Pro.Helpers;

public static class DbChecker
{
    private static bool isConnected;
    private static readonly Func<bool> ChangeSettingsMsg = () => XtraMessageBox.Show("Cannot connect to database \n Do you want to Change Settings?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;
    public static async Task<bool> CheckDbConnectionAsync(string? server = null)
    {
        server ??= ChifaDb.Server;

        NpgsqlConnection? con = null;
        try
        {
            var ConnectionString = $"Server={server}; Port=5432; User Id=pharm; Password=REDACTED; Database=CHIFA_OFFICINE;";
            con = new NpgsqlConnection(ConnectionString);
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
    public static bool CheckOrDownloadServer()
    {
        var isRunning = Process.GetProcessesByName("postgres").Any();

        if (isRunning) return true;

        if (!File.Exists(AppSettings.Default.ChifaPostgres))
        {
            XtraMessageBox.Show("Server not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        for (var i = 0; i < 5; i++)
        {
            var process = new Process()
            {
                StartInfo= new ProcessStartInfo(AppSettings.Default.ChifaLancer_Serveur)
                {
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                }
            };

            process.Start();
            process.WaitForExit(1000);

            isRunning = Process.GetProcessesByName("postgres").Any();
        }
        return isRunning;
    }
    public static async Task RunServerAsync()
    {
        if (isConnected) return;
        try
        {
            if (!CheckOrDownloadServer())
            {
                MessageBox.Show("Postgres SQL Server Not working! and cannot run it!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            isConnected = await CheckDbConnectionAsync();
 
            if (!isConnected && ChangeSettingsMsg())
            {
                ParametersUc.ShowAsForm();
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
    public static string BackupFileName => $"CHIFA_OFFICINE_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.backup";
    public static async Task SaveBackup()
    {
        try
        {
            Environment.SetEnvironmentVariable("PGPASSWORD", AppSettings.Default.DbPassword);

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
                Arguments = $"""-h "{ChifaDb.Server}" -p 5432 -U pharm -c -Ft -d CHIFA_OFFICINE -f "{backupFile}" """
            };

            Process process = new() { StartInfo = startInfo };

            process.Start();
            await process.WaitForExitAsync();

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();

            if (process.ExitCode == 0)
            {
                MessageBox.Show("Backup completed successfully.");
            }
            else
            {
                MessageBox.Show("Database restore failed. Error: " + error);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
    public static async Task Restore(string fileName)
    {
        Environment.SetEnvironmentVariable("PGPASSWORD", AppSettings.Default.DbPassword);
        try
        {
            ProcessStartInfo processStartInfo = new()
            {
                FileName = AppSettings.Default.ChifaRestor,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Arguments = $"""-h "{ChifaDb.Server}" -c  -p 5432 -U pharm -d CHIFA_OFFICINE "{fileName}" """,
            };

            using Process process = new() { StartInfo = processStartInfo };
            process.Start();
            await process.WaitForExitAsync();

            var output = await process.StandardOutput.ReadToEndAsync();
            var error = await process.StandardError.ReadToEndAsync();

            if (process.ExitCode == 0)
            {
                MessageBox.Show("Database restore completed successfully.");
            }
            else
            {
                MessageBox.Show("Database restore failed. Error: " + error);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}
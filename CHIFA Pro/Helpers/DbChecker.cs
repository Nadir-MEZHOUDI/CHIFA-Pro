using System.DirectoryServices;

namespace CHIFA.Pro.Helpers;

public static class DbChecker
{
    public static string ConnectionString => $"Server={AppSettings.Default.ServerName}; Port={AppSettings.Default.Port}; User Id={AppSettings.Default.DbUser}; Password={AppSettings.Default.DbPassword}; Database={AppSettings.Default.DbName};";

    private static readonly Func<bool> ChangeSettingsMsg = () => XtraMessageBox.Show("Cannot connect to database \n Do you want to Change Settings?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes;

    private static bool isConnected;

    private static async Task<bool> CheckDbConnection()
    {
        try
        {
            await using var db = new ChifaDb();
            var _ = await db.Utilisateurs.CountAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static async Task<bool> CheckOrDownloadServer()
    {
        var isRunning = Process.GetProcessesByName("postgres").Length != 0;
        if (isRunning) return true;

        if (!File.Exists(AppSettings.Default.ChifaPostgres))
        {
            XtraMessageBox.Show("Server not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        for (int i = 0; i < 5; i++)
        {
            Process.Start(new ProcessStartInfo(AppSettings.Default.ChifaLancer_Serveur)
            {
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
            });

            isRunning = Process.GetProcessesByName("postgres").Length != 0;
            if (isRunning) return true;
            await Task.Delay(1000);
        }
        return false;
    }

    public static async Task RunServerAsync()
    {
        if (isConnected) return;
        try
        {
            do
            {
                var serverIsRunning = await CheckOrDownloadServer();

                if (!serverIsRunning)
                {
                    MessageBox.Show("Postgres SQL Server Not working! and cannot run it!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }

                isConnected = await CheckDbConnection();
                if (isConnected) break;
                if (ChangeSettingsMsg())
                {
                    ParametersUc.ShowAsForm();
                }
                else
                {
                    break;
                }
            }
            while (!isConnected);
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
                Arguments = $"""-h "{AppSettings.Default.ServerName}" -p {AppSettings.Default.Port} -U "{AppSettings.Default.DbUser}" -c -Ft -d "{AppSettings.Default.DbName}" -f "{backupFile}" """
            };

            Process process = new() { StartInfo = startInfo };

            process.Start();
            await process.WaitForExitAsync();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

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


    public static async Task Restor(string fileName)
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
                Arguments = $"""-h "{AppSettings.Default.ServerName}" -c  -p "{AppSettings.Default.Port}" -U "{AppSettings.Default.DbUser}" -d "{AppSettings.Default.DbName}" "{fileName}" """,
            };

            using Process process = new() { StartInfo = processStartInfo };
            process.Start();
            await process.WaitForExitAsync();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

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



    public static List<string> ListNetworkComputers()
    {
        using var root = new DirectoryEntry("WinNT:");
        return (from DirectoryEntry computer in root.Children
                where computer.Name != "Schema"
                select computer.Name)
               .ToList();
    }
}
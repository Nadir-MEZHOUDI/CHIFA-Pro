using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using DataModel;
using Microsoft.Extensions.Options;
using Npgsql;

namespace CHIFA.Agent;

/// <summary>
/// Starts CHIFA_OFFICINE's portable PostgreSQL when it stays stopped, so phones get answers without anyone
/// opening CHIFA_OFFICINE or SmartCHIFA. It runs <c>Lancer_Serveur.bat</c> as the signed-in Windows user, never
/// as LocalSystem: CHIFA_OFFICINE finds its server by reading the postgres process path, which a user cannot do
/// on a LocalSystem process; it would then report the server as stopped and refuse to log in.
/// Before anyone signs in there is no user to start it as, so it waits.
/// </summary>
public class ChifaServerStarter(IOptionsMonitor<ChifaDbOptions> options, ILogger<ChifaServerStarter> logger) : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan AfterFailedStart = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan StartTimeout = TimeSpan.FromSeconds(30);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Stopped on two checks in a row: CHIFA_OFFICINE's own PostgreSQL_Manager, started at sign-in, goes first.
        var wasDown = false;

        while (!stoppingToken.IsCancellationRequested)
        {
            var wait = CheckInterval;
            try
            {
                var folder = DbFolder();
                if (folder is null || await IsReachableAsync(stoppingToken))
                {
                    wasDown = false;
                }
                else if (!wasDown)
                {
                    wasDown = true;
                }
                else if (TryStart(folder))
                {
                    if (await WaitReachableAsync(stoppingToken))
                    {
                        logger.LogInformation("CHIFA_OFFICINE PostgreSQL started from {Folder}", folder);
                        wasDown = false;
                    }
                    else
                    {
                        logger.LogWarning("CHIFA_OFFICINE PostgreSQL did not accept connections {Seconds} s after starting it, see {Log}",
                            StartTimeout.TotalSeconds, Path.Combine(folder, "logfile"));
                        wait = AfterFailedStart;
                    }
                }
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogWarning(ex, "Could not start CHIFA_OFFICINE PostgreSQL");
                wait = AfterFailedStart;
            }

            try
            {
                await Task.Delay(wait, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    /// <summary>The portable server folder, or null when the database is not ours to start.</summary>
    private string? DbFolder()
    {
        var settings = options.CurrentValue;
        if (string.IsNullOrWhiteSpace(settings.Folder)) return null;

        var host = new NpgsqlConnectionStringBuilder(ChifaDb.ConString).Host;
        var local = string.IsNullOrWhiteSpace(host)
                    || host.Equals("localhost", StringComparison.OrdinalIgnoreCase)
                    || host is "127.0.0.1" or "::1"
                    || host.Equals(Environment.MachineName, StringComparison.OrdinalIgnoreCase);
        if (!local) return null;

        var folder = Path.Combine(settings.Folder, "CHIFA_OFFICINE_DB");
        if (!File.Exists(Path.Combine(folder, "Lancer_Serveur.bat")) || !File.Exists(Path.Combine(folder, "bin", "postgres.exe")))
            return null;

        // Only the bundled PostgreSQL 9.x; an initdb on a missing data folder is CHIFA_OFFICINE's job.
        var versionFile = Path.Combine(folder, "data", "PG_VERSION");
        if (!File.Exists(versionFile)) return null;
        var version = File.ReadLines(versionFile).FirstOrDefault()?.Trim();
        return version?.StartsWith("9.", StringComparison.Ordinal) == true ? folder : null;
    }

    private static async Task<bool> IsReachableAsync(CancellationToken token)
    {
        try
        {
            var builder = new NpgsqlConnectionStringBuilder(ChifaDb.ConString) { Timeout = 5, Pooling = false };
            await using var connection = new NpgsqlConnection(builder.ConnectionString);
            await connection.OpenAsync(token);
            return true;
        }
        catch (Exception ex) when (ex is NpgsqlException or TimeoutException or System.Net.Sockets.SocketException)
        {
            return false;
        }
    }

    private static async Task<bool> WaitReachableAsync(CancellationToken token)
    {
        var deadline = DateTime.UtcNow + StartTimeout;
        while (DateTime.UtcNow < deadline)
        {
            await Task.Delay(TimeSpan.FromSeconds(2), token);
            if (await IsReachableAsync(token)) return true;
        }
        return false;
    }

    private bool TryStart(string folder)
    {
        var session = ActiveUserSession();
        if (session is null)
        {
            logger.LogDebug("CHIFA_OFFICINE PostgreSQL is stopped but nobody is signed in to start it as");
            return false;
        }

        var bat = Path.Combine(folder, "Lancer_Serveur.bat");
        // The batch file tests "IF EXIST data" relative to the current directory.
        UserProcess.Start(session.Value, new StringBuilder($"cmd.exe /c \"\"{bat}\"\""), folder);
        logger.LogInformation("CHIFA_OFFICINE PostgreSQL was stopped: running {Bat} in session {Session}", bat, session.Value);
        return true;
    }

    private static uint? ActiveUserSession()
    {
        if (!UserProcess.WTSEnumerateSessions(IntPtr.Zero, 0, 1, out var sessions, out var count))
            return null;
        try
        {
            var size = Marshal.SizeOf<UserProcess.WtsSessionInfo>();
            for (var i = 0; i < count; i++)
            {
                var info = Marshal.PtrToStructure<UserProcess.WtsSessionInfo>(sessions + i * size);
                if (info.State == UserProcess.WtsActive && info.SessionId != 0)
                    return info.SessionId;
            }
            return null;
        }
        finally
        {
            UserProcess.WTSFreeMemory(sessions);
        }
    }

    private static class UserProcess
    {
        public const int WtsActive = 0;
        private const uint CreateNoWindow = 0x08000000;
        private const uint CreateUnicodeEnvironment = 0x00000400;

        public static void Start(uint session, StringBuilder commandLine, string workingDirectory)
        {
            if (!WTSQueryUserToken(session, out var token))
                throw new Win32Exception(Marshal.GetLastWin32Error(), "WTSQueryUserToken");
            var environment = IntPtr.Zero;
            try
            {
                if (!CreateEnvironmentBlock(out environment, token, false))
                    environment = IntPtr.Zero;

                var startup = new StartupInfo { cb = Marshal.SizeOf<StartupInfo>(), lpDesktop = @"winsta0\default" };
                if (!CreateProcessAsUser(token, null, commandLine, IntPtr.Zero, IntPtr.Zero, false,
                        CreateNoWindow | CreateUnicodeEnvironment, environment, workingDirectory, ref startup, out var process))
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "CreateProcessAsUser");

                CloseHandle(process.hThread);
                CloseHandle(process.hProcess);
            }
            finally
            {
                if (environment != IntPtr.Zero) DestroyEnvironmentBlock(environment);
                CloseHandle(token);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WtsSessionInfo
        {
            public uint SessionId;
            public IntPtr WinStationName;
            public int State;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct StartupInfo
        {
            public int cb;
            public string? lpReserved;
            public string? lpDesktop;
            public string? lpTitle;
            public int dwX, dwY, dwXSize, dwYSize, dwXCountChars, dwYCountChars, dwFillAttribute, dwFlags;
            public short wShowWindow, cbReserved2;
            public IntPtr lpReserved2, hStdInput, hStdOutput, hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ProcessInformation
        {
            public IntPtr hProcess, hThread;
            public int dwProcessId, dwThreadId;
        }

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSEnumerateSessions(IntPtr server, int reserved, int version, out IntPtr sessions, out int count);

        [DllImport("wtsapi32.dll")]
        public static extern void WTSFreeMemory(IntPtr memory);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSQueryUserToken(uint sessionId, out IntPtr token);

        [DllImport("userenv.dll", SetLastError = true)]
        private static extern bool CreateEnvironmentBlock(out IntPtr environment, IntPtr token, bool inherit);

        [DllImport("userenv.dll", SetLastError = true)]
        private static extern bool DestroyEnvironmentBlock(IntPtr environment);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CreateProcessAsUser(IntPtr token, string? applicationName, StringBuilder commandLine,
            IntPtr processAttributes, IntPtr threadAttributes, bool inheritHandles, uint creationFlags,
            IntPtr environment, string currentDirectory, ref StartupInfo startupInfo, out ProcessInformation processInformation);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr handle);
    }
}

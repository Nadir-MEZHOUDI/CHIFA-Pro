using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

using DevExpress.XtraBars.Alerter;
using DevExpress.XtraGrid.Views.Grid;

using Npgsql;

namespace CHIFA.Pro.Helpers;

public static class XtraHelper
{
    /// <summary>One load per grid: starting a new one cancels the query of the previous one.</summary>
    private static readonly ConditionalWeakTable<GridView, ReloadCancellation> GridLoads = new();

    public static Task LoadDataAsync<T>(this GridView gridView, Func<ValueTask<IEnumerable<T>>> func,
        CancellationToken cancellationToken = default)
        => LoadDataAsync(gridView, _ => func(), cancellationToken);

    public static async Task LoadDataAsync<T>(this GridView gridView, Func<CancellationToken, ValueTask<IEnumerable<T>>> func,
        CancellationToken cancellationToken = default)
    {
        var gridControl = gridView.GridControl;
        if (!CanUseControl(gridControl) || cancellationToken.IsCancellationRequested) return;

        var gridToken = NextGridLoad(gridView);
        using var load = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, gridToken);
        var token = load.Token;
        try
        {
            gridView.ShowLoadingPanel();
            gridView.OptionsView.BestFitMaxRowCount = 40;

            gridControl.DataSource = new List<T>();

            var data = await func(token).ConfigureAwait(false);

            if (token.IsCancellationRequested) return;

            TryInvoke(gridControl, () =>
            {
                if (!CanUseControl(gridControl) || token.IsCancellationRequested) return;

                if (gridControl.DataSource is BindingSource bindingSource)
                    bindingSource.DataSource = data;
                else
                    gridControl.DataSource = data;

                if (token.IsCancellationRequested) return;

                gridView.BestFitColumns();
            });
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            HideLoadingPanel(gridView, gridToken);
        }
    }

    public static Task LoadDataAsync<T>(this BindingSource bindingSource, GridView gridView,
        Func<ValueTask<IEnumerable<T>>> func, CancellationToken cancellationToken = default)
        => LoadDataAsync(bindingSource, gridView, _ => func(), cancellationToken);

    public static async Task LoadDataAsync<T>(this BindingSource bindingSource, GridView gridView,
        Func<CancellationToken, ValueTask<IEnumerable<T>>> func, CancellationToken cancellationToken = default)
    {
        var gridControl = gridView.GridControl;
        if (!CanUseControl(gridControl) || cancellationToken.IsCancellationRequested) return;

        var gridToken = NextGridLoad(gridView);
        using var load = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, gridToken);
        var token = load.Token;
        try
        {
            gridView.ShowLoadingPanel();
            gridView.OptionsView.BestFitMaxRowCount = 40;

            var data = await func(token).ConfigureAwait(false);

            if (token.IsCancellationRequested) return;

            TryInvoke(gridControl, () =>
            {
                if (!CanUseControl(gridControl) || token.IsCancellationRequested) return;

                bindingSource.DataSource = data;

                if (token.IsCancellationRequested) return;

                gridView.BestFitColumns();
            });
        }
        catch (OperationCanceledException) when (token.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            HideLoadingPanel(gridView, gridToken);
        }
    }

    private static CancellationToken NextGridLoad(GridView gridView)
    {
        var loads = GridLoads.GetValue(gridView, static view =>
        {
            var reloads = new ReloadCancellation();
            if (view.GridControl is { } grid)
                grid.Disposed += (_, _) => reloads.Stop();
            return reloads;
        });
        return loads.Next();
    }

    /// <summary>A superseded load leaves the panel to the load that replaced it.</summary>
    private static void HideLoadingPanel(GridView gridView, CancellationToken gridToken)
    {
        if (gridToken.IsCancellationRequested) return;

        TryInvoke(gridView.GridControl, () =>
        {
            if (CanUseControl(gridView.GridControl))
                gridView.HideLoadingPanel();
        });
    }

    private static bool CanUseControl(Control? control)
        => control is { IsDisposed: false, Disposing: false, IsHandleCreated: true };

    private static void TryInvoke(Control? control, Action action)
    {
        if (!CanUseControl(control)) return;

        try
        {
            if (control!.InvokeRequired)
                control.Invoke(action);
            else
                action();
        }
        catch (ObjectDisposedException)
        {
        }
        catch (InvalidOperationException)
        {
        }
    }

    public static void SetOptions(this GridView? grid)
    {
        if (grid is null) return;
        grid.OptionsBehavior.ReadOnly = true;
        grid.OptionsBehavior.Editable = false;
        grid.OptionsFind.AlwaysVisible = true;
        grid.OptionsFind.FindNullPrompt = "...";
        grid.OptionsView.ShowFooter = true;
        grid.OptionsView.BestFitMaxRowCount = 40;
    }

    public static void Log(this Exception ex, bool showMessage = true, [CallerMemberName] string methodName = "",
        [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0, bool reportToTelemetry = true)
    {
        // A load superseded by a newer one, or by its screen closing: nothing went wrong.
        if (ex is OperationCanceledException)
        {
            Serilog.Log.Debug(ex, "Cancelled in {MethodName}", methodName);
            return;
        }

        // Server stopped, overloaded or slow: the customer's environment, not a bug. Every open screen
        // fails at once, so the user gets one notice and telemetry gets nothing.
        var dbError = DbErrors.Classify(ex);
        if (dbError != DbErrorKind.None)
        {
            Serilog.Log.Warning(ex, "Database {Kind} in method {MethodName}, file {FileName}, line {LineNumber}",
                dbError, methodName, Path.GetFileName(fileName), lineNumber);
            if (showMessage) NotifyDatabaseProblem(dbError);
            return;
        }

        Serilog.Log.Error(ex, "An error occurred in method {MethodName}, file {FileName}, line {LineNumber}", methodName, fileName, lineNumber);

        if (reportToTelemetry)
        {
            LogTelemetry(ex, methodName, fileName, lineNumber);
        }

        var result = ex.Message;

        if (Debugger.IsAttached)
            result = $"Method: {methodName}\nFile:{fileName}\nLine:{lineNumber}\n{ex}";
        if (showMessage)
            XtraMessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private static readonly NoticeThrottle DatabaseNoticeThrottle = new(TimeSpan.FromMinutes(1));
    private static AlertControl? _databaseAlert;

    /// <summary>A toast that closes by itself, at most once a minute, whatever thread the failure came from.</summary>
    private static void NotifyDatabaseProblem(DbErrorKind kind)
    {
        try
        {
            var owner = Application.OpenForms.Cast<Form>()
                .FirstOrDefault(f => f is { IsDisposed: false, IsHandleCreated: true, Visible: true });
            if (owner is null || !DatabaseNoticeThrottle.TryEnter(DateTime.UtcNow)) return;

            owner.BeginInvoke(() =>
            {
                if (owner.IsDisposed) return;
                _databaseAlert ??= new AlertControl { AutoFormDelay = 10_000 };
                _databaseAlert.Show(owner, "Base de données indisponible", DbErrors.UserMessage(kind));
            });
        }
        catch (Exception notifyError) when (notifyError is InvalidOperationException or ObjectDisposedException)
        {
            // The form closed meanwhile; the failure is already in the log file.
        }
    }

    private static void LogTelemetry(Exception ex, string methodName, string fileName, int lineNumber)
    {
        try
        {
            Program.Telemetry?.TrackException(ex, new
            {
                methodName,
                fileName = Path.GetFileName(fileName),
                lineNumber
            });
        }
        catch
        {
            // Telemetry is fire-and-forget: never let it break the UI/logging path.
        }
    }

    public static string GetThis_PC_IP_Address()
    {
        var addresses = Dns.GetHostAddresses(Dns.GetHostName());
        var localIp = addresses.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ip));
        return localIp?.ToString() ?? "";
    }

    public static async Task<string[]> ListAllDevicesOnLocalNetwork()
    {
        var activeIPs = new ConcurrentBag<string>();
        var localIp = GetThis_PC_IP_Address();

        if (!string.IsNullOrEmpty(localIp))
        {
            var baseIp = localIp[..(localIp.LastIndexOf('.') + 1)];
            var tasks = Enumerable.Range(2, 254).AsParallel().Select(i =>
            {
                var ip = baseIp + i;
                return Task.Run(async () =>
                {
                    var ping = new Ping();
                    try
                    {
                        var reply = await ping.SendPingAsync(ip, 500);
                        if (reply.Status == IPStatus.Success) activeIPs.Add(ip);
                    }
                    catch
                    {
                        // Handle exceptions (e.g., network errors)
                    }
                });
            }).ToArray();
            await Task.WhenAll(tasks);
        }

        activeIPs.Add("localhost");
        activeIPs.Add("127.0.0.1");
        return activeIPs.ToArray();
    }

    public static void SetServer(string server)
    {
        SetServerVariable(server);

        // Apply the new host to the live connection string as well, otherwise every query
        // would keep using the server captured when the process started.
        ApplyConnectionSettings(server, DbChecker.DefaultDbPort);
    }

    private static void SetServerVariable(string server)
    {
        Environment.SetEnvironmentVariable(ChifaDb.CHIFA_OFFICINE_SERVER, server, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable(ChifaDb.CHIFA_OFFICINE_SERVER, server, EnvironmentVariableTarget.User);
    }

    /// <summary>
    /// Updates <see cref="ChifaDb.ConString"/> so connections opened after a settings change use the
    /// new server without restarting the application. Connections already opened keep their server.
    /// </summary>
    private static void ApplyConnectionSettings(string server, int port)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(server)) return;

            var connection = new NpgsqlConnectionStringBuilder(ChifaDb.ConString)
            {
                Host = server,
                Port = port
            };

            ChifaDb.ConString = DatabaseDefaults.ForDesktop(connection.ConnectionString);
            DbChecker.ResetServerState();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}

using System.Collections.Concurrent;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Helpers;

public static class XtraHelper
{
    public static async Task LoadDataAsync<T>(this GridView gridView, Func<ValueTask<IEnumerable<T>>> func)
    {
        var gridControl = gridView.GridControl;
        try
        {
            if (!CanUseControl(gridControl)) return;

            gridView.ShowLoadingPanel();
            gridView.OptionsView.BestFitMaxRowCount = 40;

            gridControl.DataSource = new List<T>();

            var data = await func.Invoke().ConfigureAwait(false);

            TryInvoke(gridControl, () =>
            {
                if (!CanUseControl(gridControl)) return;

                if (gridControl.DataSource is BindingSource bindingSource)
                    bindingSource.DataSource = data;
                else
                    gridControl.DataSource = data;

                gridView.BestFitColumns();
            });
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            TryInvoke(gridControl, () =>
            {
                if (CanUseControl(gridControl))
                    gridView.HideLoadingPanel();
            });
        }
    }

    public static async Task LoadDataAsync<T>(this BindingSource bindingSource, GridView gridView,
        Func<ValueTask<IEnumerable<T>>> func)
    {
        var gridControl = gridView.GridControl;
        try
        {
            if (!CanUseControl(gridControl)) return;

            gridView.ShowLoadingPanel();
            gridView.OptionsView.BestFitMaxRowCount = 40;

            var data = await func.Invoke().ConfigureAwait(false);

            TryInvoke(gridControl, () =>
            {
                if (!CanUseControl(gridControl)) return;

                bindingSource.DataSource = data;
                gridView.BestFitColumns();
            });
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            TryInvoke(gridControl, () =>
            {
                if (CanUseControl(gridControl))
                    gridView.HideLoadingPanel();
            });
        }
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
        [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
    {
        
        Serilog.Log.Error(ex, "An error occurred in method {MethodName}, file {FileName}, line {LineNumber}", methodName, fileName, lineNumber);
        var result = ex.Message;

        if (ex is Npgsql.NpgsqlException pg && pg.Message.Contains("Failed to connect"))
        {
            //    XtraMessageBox.Show("Cannot connect to Server Run Server Or Check Settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (Debugger.IsAttached)
            result = $"Method: {methodName}\nFile:{fileName}\nLine:{lineNumber}\n{ex}";
        if (showMessage)
            XtraMessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        Environment.SetEnvironmentVariable(ChifaDb.CHIFA_OFFICINE_SERVER, server, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable(ChifaDb.CHIFA_OFFICINE_SERVER, server, EnvironmentVariableTarget.User);
    }

    public static void SetServerAndPort(string server, int port)
    {
        SetServer(server);

        var dbPort = port > 0 ? port : DbChecker.DefaultDbPort;
        var portValue = dbPort.ToString();

        Environment.SetEnvironmentVariable("CHIFA_DB_PORT", portValue, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("CHIFA_DB_PORT", portValue, EnvironmentVariableTarget.User);
    }
}

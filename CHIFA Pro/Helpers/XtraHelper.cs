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
        try
        {
            gridView.ShowLoadingPanel();
            gridView.OptionsView.BestFitMaxRowCount = 40;

            if (gridView.GridControl != null)
                gridView.GridControl.DataSource = new List<T>();

            var data = await func.Invoke().ConfigureAwait(false);
            gridView.GridControl?.Invoke(() =>
            {
                if (gridView.GridControl.DataSource is BindingSource bindingSource)
                    bindingSource.DataSource = data;
                else
                    gridView.GridControl.DataSource = data;

                gridView.BestFitColumns();
            });
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            gridView.HideLoadingPanel();
        }
    }

    public static async Task LoadDataAsync<T>(this BindingSource bindingSource, GridView gridView,
        Func<ValueTask<IEnumerable<T>>> func)
    {
        try
        {
            gridView.ShowLoadingPanel();
            gridView.OptionsView.BestFitMaxRowCount = 40;
            await Task.Run(async () =>
            {
                var data = await func.Invoke().ConfigureAwait(false);
                if (gridView.GridControl.InvokeRequired)
                {
                    gridView.GridControl?.Invoke(() =>
                    {
                        bindingSource.DataSource = data;
                        gridView.BestFitColumns();
                    });
                }
                else
                {
                    bindingSource.DataSource = data;
                    gridView.BestFitColumns();
                }
            });
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            gridView.HideLoadingPanel();
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

        activeIPs.Add("127.0.0.1");
        activeIPs.Add("localhost");
        return activeIPs.ToArray();
    }

    public static void SetServer(string server)
    {
        Environment.SetEnvironmentVariable(ChifaDb.CHIFA_OFFICINE_SERVER, server, EnvironmentVariableTarget.User);
    }
}
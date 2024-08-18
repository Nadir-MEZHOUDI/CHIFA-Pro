using DevExpress.XtraGrid.Views.Grid;

using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;

namespace CHIFA.Pro.Helpers;

public static class XtraHelper
{
    public static async Task LoadDataAsync<T>(this GridView gridView, Func<Task<IEnumerable<T>>> func)
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
            gridView?.HideLoadingPanel();
        }
    }
    public static async Task LoadDataAsync<T>(this BindingSource bindingSource, GridView gridView, Func<Task<IEnumerable<T>>> func)
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
            gridView?.HideLoadingPanel();
        }
    }
    public static void SetOptions(this GridView grid)
    {
        grid.OptionsBehavior.ReadOnly = true;
        grid.OptionsBehavior.Editable = false;
        grid.OptionsFind.AlwaysVisible = true;
        grid.OptionsFind.FindNullPrompt = "...";
        grid.OptionsView.ShowFooter = true;
        grid.OptionsView.BestFitMaxRowCount = 40;
    }

    public static void Log(this Exception ex, [CallerMemberName] string methodName = "", [CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0)
    {
        var result = ex.Message;
        if (Debugger.IsAttached)
        {
            result = $"Method: {methodName}\nFile:{fileName}\nLine:{lineNumber}\n{ex}";
        }
        XtraMessageBox.Show(result, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static string ListLocalIPAddresses()
    {
        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (networkInterface.OperationalStatus != OperationalStatus.Up ||
                networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
            {
                continue;
            }

            IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

            foreach (UnicastIPAddressInformation ip in ipProperties.UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(ip.Address) &&
                    !ip.Address.ToString().StartsWith("169.254."))
                {
                    return ip.Address.ToString();
                }
            }
        }
        return "";
    }


}

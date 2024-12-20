// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable NotAccessedField.Local

using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows;
using System.Windows.Interop;
using Serilog;

namespace CHIFA.Server.Helpers;

public sealed class SingleInstance : IDisposable
{
    private static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name!;
    private const int HWND_BROADCAST = 0xffff;
    public static readonly int WM_SHOWFIRSTINSTANCE = RegisterWindowMessage($"{AppName}_WM_SHOWFIRSTINSTANCE");
    private static Mutex? _mutex;
    private static readonly ILogger _logger = Log.ForContext<SingleInstance>();
    private bool _disposed;

    [DllImport("user32.dll")]
    private static extern int RegisterWindowMessage(string lpString);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    private static void ShowFirstInstance()
    {
        try
        {
            if (!PostMessage((IntPtr)HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero))
            {
                var error = Marshal.GetLastWin32Error();
                _logger.Warning("Failed to show first instance. Error code: {ErrorCode}", error);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error showing first instance");
        }
    }

    public static bool CheckAndRunInstance()
    {
        try
        {
            bool createdNew;
            _mutex = new Mutex(false, $"Global\\{AppName}", out createdNew);

            if (createdNew)
            {
                try
                {
                    _mutex.WaitOne();
                }
                catch (AbandonedMutexException)
                {
                    // Previous instance crashed
                    _logger.Warning("Previous instance crashed, continuing as new instance");
                }
                return false;
            }

            _logger.Information("Another instance is already running");
            ShowFirstInstance();
            Application.Current.Shutdown();
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error checking single instance");
            return false;
        }
    }

    public static void AttachInstanceCallback(Window window)
    {
        try
        {
            var source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
            source?.AddHook(WndProc);
            _logger.Debug("Successfully attached instance callback to window");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error attaching instance callback");
        }
    }

    private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        try
        {
            if (msg == WM_SHOWFIRSTINSTANCE)
            {
                var window = Application.Current.MainWindow;
                if (window != null)
                {
                    if (window.WindowState == WindowState.Minimized)
                    {
                        window.WindowState = WindowState.Normal;
                    }
                    window.Activate();
                    window.Topmost = true;
                    window.Topmost = false;
                    window.Focus();
                }
                handled = true;
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error in WndProc");
        }
        return IntPtr.Zero;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            if (_mutex != null)
            {
                try
                {
                    _mutex.ReleaseMutex();
                    _mutex.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error disposing mutex");
                }
            }
            _disposed = true;
        }
    }
}

#region Usage

//To use this class, simply call the CheckAndRunInstance method in the App constructor like this:
// if (SingleInstance.CheckAndRunInstance()) return;

//This will prevent the application from running multiple instances.

//In WPF

//To bring the single instance to the front, add the following code to the MainWindow.xaml.cs file:



//#region Bring Single Instance To Front

//protected override void OnSourceInitialized(EventArgs e)
//{
//    try
//    {
//        base.OnSourceInitialized(e);
//        var source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
//        source?.AddHook(WndProc);
//    }
//    catch (Exception ex)
//    {
//        ex.Log();
//    }
//}

//[DebuggerStepThrough]
//private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
//{
//    try
//    {
//        if (msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
//        {
//            if (WindowState == WindowState.Minimized)
//                WindowState = WindowState.Maximized;
//            Activate();
//            handled = true;
//        }
//    }
//    catch (Exception ex)
//    {
//        ex.Log();
//    }

//    return IntPtr.Zero;
//}

//#endregion



//In WinForms Application To bring the single instance to the front, add the following code to the main form file:

//#region Bring Single Instance To Front

//protected override void WndProc(ref Message m)
//{
//    try
//    {
//        if (m.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
//        {
//            Show();

//            Notification.Visible = false;

//            if (WindowState == FormWindowState.Minimized)
//                WindowState = FormWindowState.Maximized;

//            Activate();
//        }
//    }
//    catch (Exception)
//    {
//        //ignore
//    }

//    base.WndProc(ref m);
//}

//#endregion



#endregion
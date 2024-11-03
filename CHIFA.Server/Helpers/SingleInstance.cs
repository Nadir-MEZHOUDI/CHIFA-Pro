// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable NotAccessedField.Local

using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace CHIFA.Server.Helpers;
public static class SingleInstance
{
    private static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name!;

    private const int HWND_BROADCAST = 0xffff;

    public static readonly int WM_SHOWFIRSTINSTANCE = RegisterWindowMessage(AppName + '_' + "WM_SHOWFIRSTINSTANCE");

    [DllImport("user32.dll")] private static extern int RegisterWindowMessage(string lpString);

    [DllImport("user32.dll")] private static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    private static void ShowFirstInstance() => PostMessage(HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);

    private static Mutex _mutex = null!;

    public static bool AppIsRunning()
    {
        try
        {
            _mutex = new Mutex(true, AppName, out var createdNew);
            if (createdNew) return false;
            ShowFirstInstance();
            Application.Current.Shutdown();
            return true;
        }
        catch (Exception)
        {
            //Ignore
        }
        return false;
    }
}

#region Usage

//To use this class, simply call the AppIsRunning method in the App constructor like this:
// if (SingleInstance.AppIsRunning()) return;

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
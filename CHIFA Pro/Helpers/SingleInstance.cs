using System.Runtime.InteropServices;

namespace CHIFA.Pro.Helpers;
public static class SingleInstance
{
    private const string appName = "CHIFA.Pro";

    private const int HWND_BROADCAST = 0xffff;

    public static readonly int WM_SHOWFIRSTINSTANCE = RegisterWindowMessage("WM_SHOWFIRSTINSTANCE");

    [DllImport("user32.dll")]
    private static extern int RegisterWindowMessage(string lpString);

    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    private static void ShowFirstInstance() => PostMessage(HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);

    private static Mutex _mutex = null!;

    public static bool AppIsRunning()
    {
        _mutex = new Mutex(true, appName, out var createdNew);
        if (createdNew) return false;
        ShowFirstInstance();
        Application.Exit();
        return true;
    }
}

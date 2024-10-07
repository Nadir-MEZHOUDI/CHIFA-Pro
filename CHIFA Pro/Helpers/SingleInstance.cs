// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable NotAccessedField.Local

namespace CHIFA.Pro.Helpers;
public static class SingleInstance
{
    private static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name!;

    private const int HWND_BROADCAST = 0xffff;

    public static readonly int WM_SHOWFIRSTINSTANCE = RegisterWindowMessage(AppName + '_' + "WM_SHOWFIRSTINSTANCE");

    [DllImport("user32.dll")]
    private static extern int RegisterWindowMessage(string lpString);

    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    private static void ShowFirstInstance() => PostMessage(HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);

    private static Mutex _mutex = null!;

    public static bool AppIsRunning()
    {
        try
        {
            _mutex = new Mutex(true, AppName, out var createdNew);
            if (createdNew) return false;
            ShowFirstInstance();
            Application.Exit();
            return true;
        }
        catch (Exception)
        {
            //Ignore
        }
        return false;
    }
}

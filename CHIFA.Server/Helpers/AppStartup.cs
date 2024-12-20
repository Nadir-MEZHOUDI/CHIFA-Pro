using System.Reflection;
using Microsoft.Win32;

namespace CHIFA.Server.Helpers;

public class AppStartup
{
    private static readonly string AppName = Assembly.GetExecutingAssembly().GetName().Name!;

    public static void AddApplicationToStartup()
    {
        var appPath = Assembly.GetExecutingAssembly().Location; // Path to the executable
        if(appPath.EndsWith(".dll",StringComparison.InvariantCultureIgnoreCase))
            appPath = appPath.Replace(".dll", ".exe");

        var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

        if (registryKey == null) return;
        registryKey.SetValue(AppName, "\"" + appPath + "\" minimized");
        registryKey.Close();
    }

    public static void RemoveApplicationFromStartup()
    {
        var registryKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);

        if (registryKey == null) return;
        registryKey.DeleteValue(AppName, false);
        registryKey.Close();
    }
}
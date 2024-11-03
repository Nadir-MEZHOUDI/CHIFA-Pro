using System.Runtime.InteropServices;

using CHIFA.Server.Helpers;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace CHIFA.Server;

public partial class App
{
#if DEBUG
    // Import AllocConsole from kernel32.dll to create a new console window
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool AllocConsole();
#endif

    public App()
    {
        if (SingleInstance.AppIsRunning()) return;

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
#if DEBUG
            .WriteTo.Console(theme: AnsiConsoleTheme.Code)
            //  .WriteTo.Debug()
#endif
            .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

#if DEBUG
        AllocConsole();
        //  Console.WriteLine(@"Console is ready");
        //  DataConnection.TurnTraceSwitchOn();
        //  DataConnection.WriteTraceLine = Log.Debug!;
#endif

        Log.Information("Starting CHIFA Server...");
        Velopack.VelopackApp.Build().Run();

    }

}


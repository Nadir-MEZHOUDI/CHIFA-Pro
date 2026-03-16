using System.IO;
using System.Runtime.InteropServices;
using System.Windows;

using CHIFA.Server.Helpers;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace CHIFA.Server;

public partial class App
{

    public static StringWriter LogWriter { get;  } = new();


#if DEBUG
    // Import AllocConsole from kernel32.dll to create a new console window
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static partial void AllocConsole();
#endif

    public App()
    {
        try
        {
            if (SingleInstance.CheckAndRunInstance()) return;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.TextWriter(LogWriter, outputTemplate: "[{Timestamp:HH:mm:ss}] {Level:u3}: {Message:lj}{NewLine}{Exception}")
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
        catch (Exception ex)
        {
            try
            {
                ex.Log();
            }
            catch
            {
                MessageBox.Show(ex.Message, "Fatal error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Shutdown(-1);
        }

    }

}


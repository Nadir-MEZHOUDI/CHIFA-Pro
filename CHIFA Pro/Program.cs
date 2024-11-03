global using System.Diagnostics;
global using System.Globalization;
global using System.IO;
global using System.Linq.Expressions;
global using System.Reflection;
global using System.Runtime.InteropServices;

global using CHIFA.DAL.DataServices;
global using CHIFA.Pro.Helpers;
global using CHIFA.Pro.Views;

global using DataModel;

global using DevExpress.XtraEditors;
global using DevExpress.XtraGrid.Views.Base;

global using LinqToDB;
global using Serilog;
global using Serilog.Sinks.SystemConsole.Themes;

namespace CHIFA.Pro;
internal static partial class Program
{
#if DEBUG
    // Import AllocConsole from kernel32.dll to create a new console window
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial void AllocConsole();
#endif


    [STAThread]
    private static void Main()
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
        Console.WriteLine(@"Console is ready");
     //   DataConnection.TurnTraceSwitchOn();
      //  DataConnection.WriteTraceLine = Log.Debug!;
#endif

        SetExceptionHandling();

        Velopack.VelopackApp.Build().Run();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        SetCulture();

        Application.Run(new FrmMain());
    }

    private static void SetExceptionHandling()
    {
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        Application.ThreadException += ThreadException_Handler;
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

        TaskScheduler.UnobservedTaskException += (_, e) =>
        {
            Log.Error(e.Exception, "Unobserved task exception");
            e.SetObserved();
        };
    }

    private static void ThreadException_Handler(object sender, ThreadExceptionEventArgs e)
    {
        e.Exception.Log();
    }

    private static void SetCulture()
    {
        CultureInfo frCulture = new("fr-FR");
        Thread.CurrentThread.CurrentCulture = frCulture;
        Thread.CurrentThread.CurrentUICulture = frCulture;
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = (Exception)e.ExceptionObject;
        ex.Log();
        if (ex is Npgsql.NpgsqlException)
            MessageBox.Show(@"Cannot connect to Database, Check Your Server");
    }
}
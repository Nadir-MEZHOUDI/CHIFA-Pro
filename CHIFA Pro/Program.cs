global using CHIFA.DAL.Helpers;
global using CHIFA.Pro.Others;

global using DevExpress.XtraGrid.Views.Base;

global using Helpers.Settings;

global using LinqToDB;

global using System;
global using System.Collections.Generic;
global using System.Data;
global using System.Diagnostics;
global using System.IO;
global using System.Reflection;

using LinqToDB.Data;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

using System.Globalization;
using System.Runtime.InteropServices;

namespace CHIFA.Pro;
internal static class Program
{
#if DEBUG
    // Import AllocConsole from kernel32.dll to create a new console window
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool AllocConsole();

#endif

    public static CultureInfo frCulture = new("fr-FR");

    [STAThread]
    private static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext() // Enrich log events with properties from Serilog's LogContext
            .WriteTo.Console(theme: AnsiConsoleTheme.Literate, // Use a built-in color theme (or create custom themes)
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"  // Template for formatting logs
            )
            .WriteTo.Debug()
            .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

#if DEBUG
        AllocConsole();
        Console.WriteLine("Console is ready");
        DataConnection.TurnTraceSwitchOn();
        DataConnection.WriteTraceLine = Log.Information;
#endif

        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        Application.ThreadException += ThreadException_Handler;
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            Log.Error(e.Exception, "Unobserved task exception");
            e.SetObserved();
        };

        Velopack.VelopackApp.Build().Run();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        SetCulture();

        Application.Run(new frmMain());
    }

    private static void ThreadException_Handler(object sender, ThreadExceptionEventArgs e)
    {
        e.Exception.Log();
    }

    private static void SetCulture()
    {
        Thread.CurrentThread.CurrentCulture = frCulture;
        Thread.CurrentThread.CurrentUICulture = frCulture;
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        var ex = (Exception)e.ExceptionObject;
        ex.Log();
        if (ex is Npgsql.NpgsqlException)
            MessageBox.Show("Cannot connect to Database, Check Your Server");

    }
}
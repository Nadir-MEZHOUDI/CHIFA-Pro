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

using Serilog;

using System.Globalization;

namespace CHIFA.Pro;
internal static class Program
{
    public static CultureInfo frCulture = new("fr-FR");

    [STAThread]
    private static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Debug()
            .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

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
        Exception ex = (Exception)e.ExceptionObject;
        ex.Log();
        if (ex is Npgsql.NpgsqlException)
            MessageBox.Show("Cannot connect to Database, Check Your Server");        
        
    }
}
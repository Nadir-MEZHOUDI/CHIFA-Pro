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
using System.Globalization;

namespace CHIFA.Pro;
internal static class Program
{
    public static CultureInfo frCulture = new CultureInfo("fr-FR");


    [STAThread]
    private static void Main()
    {
        Velopack.VelopackApp.Build().Run(); 
        SetDbSettings();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        SetCulture();

        Application.Run(new frmMain());

    }

    private static void SetCulture()
    {
        Thread.CurrentThread.CurrentCulture = frCulture;
        Thread.CurrentThread.CurrentUICulture = frCulture;
    }

    private static void SetDbSettings()
    {
        LinqToDB.Data.DataConnection.TurnTraceSwitchOn();
        LinqToDB.Data.DataConnection.WriteTraceLine = (s1, s2, _) => Debug.WriteLine(s1, s2);
        ChifaDb.ConString = DbChecker.ConnectionString;
    }

    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Npgsql.NpgsqlException)
            MessageBox.Show("Cannot connect to Database, Check Your Server");
        else
            MessageBox.Show(e.ExceptionObject.ToString());
    }
}
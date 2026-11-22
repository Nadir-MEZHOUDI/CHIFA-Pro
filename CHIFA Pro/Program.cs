global using System.Diagnostics;
global using System.Globalization;
global using System.IO;
global using System.Linq.Expressions;
global using System.Reflection;
global using System.Runtime.InteropServices;

global using CHIFA.Pro.Helpers;
global using CHIFA.Pro.Views;

global using DataModel;

global using DevExpress.XtraEditors;
global using DevExpress.XtraGrid.Views.Base;

global using LinqToDB;
global using Serilog;

namespace CHIFA.Pro;
internal static partial class Program
{
    private static TelemetrySession? _telemetry;
    internal static TelemetrySession? Telemetry => _telemetry; 

    [STAThread]
    private static void Main()
    {
        try
        {
            if (SingleInstance.AppIsRunning()) return;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
#if DEBUG
                .WriteTo.Debug()
#endif
                .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
 
            SetExceptionHandling();
            SetTelemetry();

            Velopack.VelopackApp.Build().Run();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            SetCulture();

            Application.Run(new FrmMain());
        }
        catch (Exception ex)
        {
            try
            {
                ex.Log();
            }
            catch
            {
                MessageBox.Show(ex.Message, "Erreur fatale", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            DisposeTelemetry();
            Log.CloseAndFlush();
        }
    }

    private static void SetTelemetry()
    {
        try
        {
            _telemetry = TelemetryFactory.Create(options =>
            {
                options.Application = "chifa-pro";
#if DEBUG
                options.Endpoint = "http://localhost:5000";
#else
                options.Endpoint = "https://telemetry.smartappdz.org";
#endif
                options.Version = Application.ProductVersion.Split("+")[0];
            });

            TelemetryExceptionHooks.AttachProcessWide(_telemetry);
            _telemetry.TrackAppStarted();

            Application.ApplicationExit += (_, _) =>
            {
                _telemetry.TrackAppClosed();
                DisposeTelemetry();
            };
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private static void DisposeTelemetry()
    {
        if (_telemetry is null) return;
        try
        {
            _telemetry.DisposeAsync().AsTask().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            _telemetry = null;
        }
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
        // Application.ThreadException is NOT covered by TelemetryExceptionHooks.AttachProcessWide,
        // so ex.Log() must report to telemetry (reportToTelemetry: true via default).
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
        // AppDomain.UnhandledException IS covered by TelemetryExceptionHooks.AttachProcessWide -> avoid double-report.
        ex.Log(reportToTelemetry: false);
        if (ex is Npgsql.NpgsqlException)
            MessageBox.Show(@"Impossible de se connecter à la base de données. Vérifiez votre serveur.", @"Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

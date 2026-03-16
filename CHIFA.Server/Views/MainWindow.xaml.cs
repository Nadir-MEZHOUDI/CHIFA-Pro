using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;
using CHIFA.Server.Helpers;
using CHIFA.Server.Helpers.Settings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using SmartApp.Bridge;

namespace CHIFA.Server.Views;

[ObservableObject]
public partial class MainWindow
{
    private const int MaxDisplayedLogLength = 18000;

    private readonly CancellationTokenSource _logUpdaterCancellation = new();
    private string _lastRenderedLogs = "";

    [ObservableProperty] private BridgeClient? _client;
    [ObservableProperty] private string _logMessages = "";
    [ObservableProperty] private BridgeServer? _server;
    [ObservableProperty] private GrpcServer? _service;
    [ObservableProperty] private string _status = "Ready";

    [ObservableProperty] private string _version = "";

    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        BridgeClient.BaseUri =
            "https://smartappbridge.azurewebsites.net/api/GetInfo?code=hnI7cPto10N0CAhzU1WfAIOmycgMDDreFihcxMFu_lNiAzFuQbI2MQ%3D%3D";
        BridgeServer.BaseUri =
            "https://smartappbridge.azurewebsites.net/api/Register?code=p9Abe-0btlIvEw7KGhq29PyxzW1Nz5jp34JH4IjFBh6dAzFuI9wWEg%3D%3D";
#endif

        _ = Task.Run(() => UpdateLogMessagesAsync(_logUpdaterCancellation.Token));
    }

    public static AppSettings AppSettings => AppSettings.Default;
    public UpdateService UpdateService { get; } = new();

    private async Task UpdateLogMessagesAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var builder = App.LogWriter.GetStringBuilder();
                var logsLength = builder.Length;
                var tailStart = Math.Max(logsLength - MaxDisplayedLogLength, 0);
                var displayedLogs = builder.ToString(tailStart, logsLength - tailStart);

                if (!string.Equals(displayedLogs, _lastRenderedLogs, StringComparison.Ordinal))
                {
                    _lastRenderedLogs = displayedLogs;
                    var dispatcher = Application.Current?.Dispatcher;
                    if (dispatcher != null)
                        _ = dispatcher.BeginInvoke(() => LogMessages = displayedLogs);
                }
            }
            catch (Exception ex)
            {
                ex.Log(false);
            }

            try
            {
                await Task.Delay(500, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task RunChifaMobileServer()
    {
        try
        {
            Server?.Stop();
            Server = new BridgeServer(AppSettings.ChifaMobilEmail!, AppSettings.ChifaMobilPassword!, Log.Logger);
            Service?.Stop();
            Service = new GrpcServer(Log.Logger, Server);

            await Service.StartAsync();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    [RelayCommand]
    private async Task GetClient()
    {
        try
        {
            Client ??= new BridgeClient();
            await Client.GetInfo(AppSettings.ChifaMobilEmail!, AppSettings.ChifaMobilPassword!);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }


    [RelayCommand]
    private void StopServer()
    {
        Server?.Stop();
        _ = Service?.Stop();
    }

    private void StartWithWin_OnChecked(object sender, RoutedEventArgs e)
    {
        try
        {
            AppStartup.AddApplicationToStartup();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void StartWithWin_OnUnchecked(object sender, RoutedEventArgs e)
    {
        try
        {
            AppStartup.RemoveApplicationFromStartup();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void MainWindow_OnClosed(object? sender, EventArgs e)
    {
        try
        {
            _logUpdaterCancellation.Cancel();
            Server?.Stop();
            _ = Service?.Stop();
        }
        catch (Exception ex)
        {
            ex.Log(false);
        }
        finally
        {
            _logUpdaterCancellation.Dispose();
        }
    }

    private void MainWindow_OnContentRendered(object? sender, EventArgs e)
    {
        try
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "";
            Status = "Ready";
            _ = UpdateService.UpdateAppCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (AppSettings.StartWithWin) 
                await RunChifaMobileServerCommand.ExecuteAsync(null);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }


    #region Bring Single Instance To Front

    protected override void OnSourceInitialized(EventArgs e)
    {
        try
        {
            base.OnSourceInitialized(e);
            var source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source?.AddHook(WndProc);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    [DebuggerStepThrough]
    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        try
        {
            if (msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                if (WindowState == WindowState.Minimized)
                    WindowState = WindowState.Maximized;
                Activate();
                handled = true;
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }

        return IntPtr.Zero;
    }

    #endregion
}

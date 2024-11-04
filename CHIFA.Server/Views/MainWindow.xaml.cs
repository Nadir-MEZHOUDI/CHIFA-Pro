using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Interop;

using CHIFA.DAL.DataServices;
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
    public static AppSettings AppSettings => AppSettings.Default;
    public UpdateService UpdateService { get; } = new();

    [ObservableProperty] private BridgeServer? _server;
    [ObservableProperty] private BridgeClient? _client;
    [ObservableProperty] private GrpcServer? _service;
    public MainWindow()
    {
        InitializeComponent();

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

    private void StartWithWin_OnChecked(object sender, RoutedEventArgs e)
    {
        AppStartup.AddApplicationToStartup();
    }

    private void StartWithWin_OnUnchecked(object sender, RoutedEventArgs e)
    {
        AppStartup.RemoveApplicationFromStartup();
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


    [ObservableProperty] private string _version;
    [ObservableProperty] private string _status;

}
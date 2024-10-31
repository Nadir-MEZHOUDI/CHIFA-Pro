using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using CHIFA.DAL.DataServices;
using CHIFA.Server.Helpers;
using CHIFA.Server.Helpers.Settings;

using Open.Nat;

using Serilog;

namespace CHIFA.Server;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public static MainWindow Instance => (Application.Current.MainWindow as MainWindow)!;

    public MainWindow()
    {
        InitializeComponent();
    }


    private async Task GetServerAddressAndForwardPort()
    {
        const int Port = 5432;
        var natDiscoverer = new NatDiscoverer();
        var cts = new CancellationTokenSource(10_000);
        var device = await natDiscoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
        var Address = (await device.GetExternalIPAsync()).ToString();

        await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, Port));
        await device.CreatePortMapAsync(new Mapping(Protocol.Udp, Port, Port));
        Log.Information($"Port: {Port} forwarded to this pc {Address}");
    }


    public static string Api => Debugger.IsAttached ? "https://localhost:7048/api/peers/" : "https://smartpharm.azurewebsites.net/api/peers/";
    public BridgeService? Server { get; private set; }
    public async Task RunChifaMobileServer()
    {
        try
        {
            await GetServerAddressAndForwardPort();

            if (AppSettings.Default.UseChifaMobile)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(AppSettings.Default.ChifaMobilEmail);
                ArgumentException.ThrowIfNullOrWhiteSpace(AppSettings.Default.ChifaMobilPassword);
                Server?.Stop();

                Server = new BridgeService(Log.Information)
                {
                    BaseUri = Api,
                    Key = AppSettings.Default.ChifaMobilEmail + AppSettings.Default.ChifaMobilPassword,
                };
                Server.AddService(new ChifaService());
                Server.AddService(new StatisticsService());
                await Server.StartAsync();
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
    private async void btnRunServerConnection_Click(object sender, EventArgs e)
    {
        try
        {
            await MainWindow.Instance.RunChifaMobileServer();

            MessageBox.Show("Connected", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            Cursor = Cursors.Arrow;
        }

    }
    private void LoadChifaMobileInfo()
    {
        try
        {
            chkUseChifaMobile.IsChecked = AppSettings.Default.UseChifaMobile;
            txtEmail.Text = AppSettings.Default.ChifaMobilEmail;
            txtPassword.Text = AppSettings.Default.ChifaMobilPassword;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void chkUseChifaMobile_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            AppSettings.Default.UseChifaMobile = chkUseChifaMobile.IsChecked == true;
            AppSettings.Default.ChifaMobilPassword = txtPassword.Text;
            AppSettings.Default.Save();
        }
        catch (Exception ex)
        {
            ex.Log();
        }

    }
}
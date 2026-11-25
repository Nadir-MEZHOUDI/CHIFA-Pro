using CHIFA.Contract;
using QRCoder;

namespace CHIFA.Pro.Mobile;

/// <summary>
/// « Application mobile »: shows a one-time code (and its QR code) that a phone enters once to be linked
/// to this pharmacy, and the phones already linked. Every opening, and « Nouveau code », gives a new code.
/// </summary>
public sealed partial class MobileLinkForm : XtraForm
{
    private static readonly Color Teal = Color.FromArgb(14, 124, 102);

    /// <summary>Not asked again during this session of SmartCHIFA once the user refused the Windows prompt.</summary>
    private static bool _serviceDeclined;

    private PairingCodeResponse? _code;
    private List<LinkedDevice> _devices = [];
    private bool _busy;

    public MobileLinkForm()
    {
        InitializeComponent();
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (ArgumentException) { }
        _lblIntro.Text =
            $"1. Sur le téléphone, scannez le QR code ou ouvrez {MobileLink.PhoneHost}{Environment.NewLine}" +
            "2. Saisissez le code ci-dessous. Il ne sert qu'une fois : le téléphone reste ensuite connecté.";
    }

    private async void MobileLinkForm_Shown(object? sender, EventArgs e)
    {
        await NewCodeAsync();
        ShowService();
        // Linked: make the phone work with SmartCHIFA closed too, or bring the service up to date.
        if (_code is not null && !_serviceDeclined && AgentService.State is AgentServiceState.NotInstalled or AgentServiceState.Outdated)
            await InstallServiceAsync();
    }

    private async void BtnNewCode_Click(object? sender, EventArgs e) => await NewCodeAsync();

    private async void BtnRemove_Click(object? sender, EventArgs e) => await RemoveSelectedAsync();

    private async void BtnService_Click(object? sender, EventArgs e) => await InstallServiceAsync();

    private void BtnClose_Click(object? sender, EventArgs e) => Close();

    private void LstDevices_SelectedIndexChanged(object? sender, EventArgs e) =>
        _btnRemove.Enabled = !_busy && SelectedDevice() is not null;

    private void Clock_Tick(object? sender, EventArgs e) => ShowExpiry();

    private async void DevicesPoll_Tick(object? sender, EventArgs e) => await PollDevicesAsync();

    private async Task NewCodeAsync()
    {
        await RunAsync(async () =>
        {
            _code = await MobileLink.NewCodeAsync();
            ShowCode();
            ShowDevices(await MobileLink.DevicesAsync());
            _clock.Start();
            _devicesPoll.Start();
        });
    }

    private void ShowCode()
    {
        if (_code is null) return;
        _lblCode.Text = _code.Code;
        _lblCode.Appearance.ForeColor = Teal;
        _qr.Image?.Dispose();
        _qr.Image = QrCode(MobileLink.PhoneUrl(_code.Code));

        var agent = MobileLink.AgentRunsAsService ? "par le service Windows" : "par SmartCHIFA";
        SetStatus(_code.AgentOnline
            ? ($"Pharmacie en ligne : les téléphones sont servis {agent}.", Color.SeaGreen)
            : ("Pharmacie pas encore connectée au serveur : patientez quelques secondes.", Color.DarkOrange));
        ShowExpiry();
    }

    private void ShowExpiry()
    {
        if (_code is null) return;
        var left = _code.ExpiresAt.ToUniversalTime() - DateTime.UtcNow;
        if (left > TimeSpan.Zero)
        {
            _lblExpiry.Text = $"Valable {left:m\\:ss} · une seule fois";
            _lblExpiry.Appearance.ForeColor = Color.DimGray;
            return;
        }

        _clock.Stop();
        _lblCode.Appearance.ForeColor = Color.Silver;
        _qr.Image?.Dispose();
        _qr.Image = null;
        _lblExpiry.Text = "Code expiré : cliquez sur « Nouveau code ».";
        _lblExpiry.Appearance.ForeColor = Color.Crimson;
    }

    /// <summary>A phone just used the code: say so and show a fresh code for the next one.</summary>
    private async Task PollDevicesAsync()
    {
        if (_busy || IsDisposed) return;
        try
        {
            var devices = await MobileLink.DevicesAsync();
            var added = devices.Where(d => _devices.All(old => old.Id != d.Id)).ToList();
            ShowDevices(devices);
            if (added.Count == 0) return;

            await NewCodeAsync();
            SetStatus(($"« {added[0].Name} » est maintenant lié à la pharmacie.", Color.SeaGreen));
        }
        catch (MobileLinkException)
        {
            // Shown again at the next click; polling stays quiet.
        }
    }

    private void ShowDevices(List<LinkedDevice> devices)
    {
        var selected = SelectedDevice()?.Id;
        _devices = devices;
        _lstDevices.BeginUpdate();
        _lstDevices.Items.Clear();
        foreach (var d in devices)
            _lstDevices.Items.Add(Describe(d));
        if (devices.Count == 0)
            _lstDevices.Items.Add("Aucun téléphone lié pour le moment.");
        _lstDevices.SelectedIndex = selected is { } id ? devices.FindIndex(d => d.Id == id) : -1;
        _lstDevices.EndUpdate();
        _btnRemove.Enabled = !_busy && SelectedDevice() is not null;
    }

    private async Task RemoveSelectedAsync()
    {
        if (SelectedDevice() is not { } device) return;
        var answer = XtraMessageBox.Show(this,
            $"Retirer « {device.Name} » ? Ce téléphone sera déconnecté tout de suite et devra être lié à nouveau avec un code.",
            Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (answer != DialogResult.Yes) return;

        await RunAsync(async () =>
        {
            await MobileLink.RemoveDeviceAsync(device.Id);
            ShowDevices(await MobileLink.DevicesAsync());
            SetStatus(($"« {device.Name} » a été retiré.", Color.DimGray));
        });
    }

    private async Task InstallServiceAsync()
    {
        _lblService.Text = "Installation du service… Acceptez la demande de Windows.";
        _lblService.Appearance.ForeColor = Color.DimGray;
        _btnService.Enabled = false;
        try
        {
            await AgentService.InstallAsync();
            _serviceDeclined = false;
            ShowService();
            if (_code is not null)
                ShowCode();
        }
        catch (MobileLinkException ex)
        {
            _serviceDeclined = true;
            ShowService();
            _lblService.Text = ex.Message + " Le téléphone fonctionne tant que SmartCHIFA est ouvert.";
            _lblService.Appearance.ForeColor = Color.Crimson;
        }
        catch (Exception ex)
        {
            ex.Log();
            ShowService();
        }
    }

    private void ShowService()
    {
        var (text, color, button) = AgentService.State switch
        {
            AgentServiceState.Running => ("Service Windows actif : le téléphone fonctionne même quand SmartCHIFA est fermé.", Color.SeaGreen, null),
            AgentServiceState.Outdated => ("Le service Windows doit être mis à jour avec cette version de SmartCHIFA.", Color.DarkOrange, "Mettre à jour"),
            AgentServiceState.Stopped => ("Service Windows arrêté : le téléphone ne fonctionne que si SmartCHIFA est ouvert.", Color.DarkOrange, "Réparer"),
            _ => ("Sans le service Windows, le téléphone ne fonctionne que si SmartCHIFA est ouvert.", Color.DimGray, "Installer le service")
        };
        _lblService.Text = text;
        _lblService.Appearance.ForeColor = color;
        _btnService.Visible = button is not null;
        _btnService.Text = button ?? string.Empty;
        _btnService.Enabled = true;
    }

    private LinkedDevice? SelectedDevice() =>
        _lstDevices.SelectedIndex >= 0 && _lstDevices.SelectedIndex < _devices.Count ? _devices[_lstDevices.SelectedIndex] : null;

    private static string Describe(LinkedDevice d)
    {
        var seen = d.LastSeenAt is { } at ? $" · dernière connexion {at.ToLocalTime():dd/MM/yyyy HH:mm}" : string.Empty;
        return $"{d.Name} · lié le {d.CreatedAt.ToLocalTime():dd/MM/yyyy}{seen}";
    }

    private async Task RunAsync(Func<Task> action)
    {
        SetBusy(true);
        try
        {
            await action();
        }
        catch (MobileLinkException ex)
        {
            _clock.Stop();
            _code = null;
            _lblCode.Text = "––––-––––";
            _lblCode.Appearance.ForeColor = Color.Silver;
            _qr.Image?.Dispose();
            _qr.Image = null;
            _lblExpiry.Text = string.Empty;
            SetStatus((ex.Message, Color.Crimson));
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            if (!IsDisposed) SetBusy(false);
        }
    }

    private void SetStatus((string Text, Color Color) status)
    {
        _lblStatus.Text = status.Text;
        _lblStatus.Appearance.ForeColor = status.Color;
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        UseWaitCursor = busy;
        _btnNewCode.Enabled = !busy;
        _btnRemove.Enabled = !busy && SelectedDevice() is not null;
    }

    private static Image QrCode(string text)
    {
        using var generator = new QRCodeGenerator();
        using var data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.M);
        using var qr = new QRCode(data);
        return qr.GetGraphic(8, Color.Black, Color.White, drawQuietZones: true);
    }
}

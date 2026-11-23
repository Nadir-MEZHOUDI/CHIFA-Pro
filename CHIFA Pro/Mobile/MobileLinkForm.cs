using CHIFA.Contract;
using DevExpress.Utils;
using QRCoder;

namespace CHIFA.Pro.Mobile;

/// <summary>
/// « Application mobile »: shows a one-time code (and its QR code) that a phone enters once to be linked
/// to this pharmacy, and the phones already linked. Every opening, and « Nouveau code », gives a new code.
/// </summary>
public sealed class MobileLinkForm : XtraForm
{
    private static readonly Color Teal = Color.FromArgb(14, 124, 102);

    private readonly LabelControl _lblIntro = new();
    private readonly PictureEdit _qr = new();
    private readonly LabelControl _lblCode = new();
    private readonly LabelControl _lblExpiry = new();
    private readonly LabelControl _lblStatus = new();
    private readonly SimpleButton _btnNewCode = new() { Text = "Nouveau code" };
    private readonly ListBoxControl _lstDevices = new();
    private readonly SimpleButton _btnRemove = new() { Text = "Retirer ce téléphone" };
    private readonly LabelControl _lblService = new();
    private readonly SimpleButton _btnService = new();
    private readonly SimpleButton _btnClose = new() { Text = "Fermer" };
    private readonly System.Windows.Forms.Timer _clock = new() { Interval = 1000 };
    private readonly System.Windows.Forms.Timer _devicesPoll = new() { Interval = 5000 };

    /// <summary>Not asked again during this session of SmartCHIFA once the user refused the Windows prompt.</summary>
    private static bool _serviceDeclined;

    private PairingCodeResponse? _code;
    private List<LinkedDevice> _devices = [];
    private bool _busy;

    public MobileLinkForm()
    {
        BuildLayout();
        Shown += async (_, _) =>
        {
            await NewCodeAsync();
            ShowService();
            // Linked: make the phone work with SmartCHIFA closed too, or bring the service up to date.
            if (_code is not null && !_serviceDeclined && AgentService.State is AgentServiceState.NotInstalled or AgentServiceState.Outdated)
                await InstallServiceAsync();
        };
        FormClosed += (_, _) =>
        {
            _clock.Dispose();
            _devicesPoll.Dispose();
        };
    }

    private void BuildLayout()
    {
        Text = "Application mobile";
        ClientSize = new Size(640, 600);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (ArgumentException) { }

        var title = new LabelControl { Text = "Lier un téléphone", Location = new Point(24, 18) };
        title.Appearance.Font = new Font("Segoe UI", 15f, FontStyle.Bold);

        _lblIntro.AutoSizeMode = LabelAutoSizeMode.None;
        _lblIntro.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        _lblIntro.Appearance.Font = new Font("Segoe UI", 9.75f);
        _lblIntro.Appearance.ForeColor = Color.DimGray;
        _lblIntro.Text =
            $"1. Sur le téléphone, scannez le QR code ou ouvrez {MobileLink.PhoneHost}{Environment.NewLine}" +
            "2. Saisissez le code ci-dessous. Il ne sert qu'une fois : le téléphone reste ensuite connecté.";
        _lblIntro.SetBounds(24, 56, 592, 42);

        _qr.SetBounds(24, 110, 190, 190);
        _qr.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
        _qr.Properties.ShowMenu = false;
        _qr.Properties.ReadOnly = true;
        _qr.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

        _lblCode.AutoSizeMode = LabelAutoSizeMode.None;
        _lblCode.Appearance.Font = new Font("Consolas", 34f, FontStyle.Bold);
        _lblCode.Appearance.ForeColor = Teal;
        _lblCode.SetBounds(236, 120, 380, 64);

        _lblExpiry.AutoSizeMode = LabelAutoSizeMode.None;
        _lblExpiry.Appearance.Font = new Font("Segoe UI", 9.75f);
        _lblExpiry.SetBounds(240, 186, 376, 22);

        _lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
        _lblStatus.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        _lblStatus.Appearance.TextOptions.VAlignment = VertAlignment.Top;
        _lblStatus.Appearance.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold);
        _lblStatus.SetBounds(240, 214, 376, 44);

        _btnNewCode.SetBounds(240, 264, 160, 32);

        var devicesTitle = new LabelControl { Text = "Téléphones liés", Location = new Point(24, 320) };
        devicesTitle.Appearance.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
        _lstDevices.SetBounds(24, 346, 592, 130);
        _lstDevices.Appearance.Font = new Font("Segoe UI", 9.75f);

        _btnRemove.SetBounds(24, 484, 190, 30);

        _lblService.AutoSizeMode = LabelAutoSizeMode.None;
        _lblService.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        _lblService.Appearance.TextOptions.VAlignment = VertAlignment.Center;
        _lblService.Appearance.Font = new Font("Segoe UI", 9.5f);
        _lblService.SetBounds(24, 524, 420, 40);
        _btnService.SetBounds(456, 528, 160, 30);

        _btnClose.SetBounds(486, 562, 130, 30);

        _btnNewCode.Click += async (_, _) => await NewCodeAsync();
        _btnRemove.Click += async (_, _) => await RemoveSelectedAsync();
        _btnClose.Click += (_, _) => Close();
        _btnService.Click += async (_, _) => await InstallServiceAsync();
        _lstDevices.SelectedIndexChanged += (_, _) => _btnRemove.Enabled = !_busy && SelectedDevice() is not null;
        _clock.Tick += (_, _) => ShowExpiry();
        _devicesPoll.Tick += async (_, _) => await PollDevicesAsync();

        Controls.AddRange([title, _lblIntro, _qr, _lblCode, _lblExpiry, _lblStatus, _btnNewCode, devicesTitle, _lstDevices, _btnRemove, _lblService, _btnService, _btnClose]);
        CancelButton = _btnClose;
        _btnRemove.Enabled = false;
    }

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

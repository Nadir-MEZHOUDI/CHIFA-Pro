using DevExpress.Utils;

namespace CHIFA.Pro.Views;

/// <summary>
/// Activation and license details. In <c>startup</c> mode it blocks the application until the
/// license is valid (Quitter closes CHIFA Pro); otherwise it is an information dialog.
/// </summary>
public sealed class LicenseForm : XtraForm
{
    private readonly LicenseClient _client;
    private readonly bool _startup;

    private readonly LabelControl _lblStatus = new();
    private readonly LabelControl _lblDetails = new();
    private readonly LabelControl _lblSerial = new() { Text = "Numéro de série" };
    private readonly TextEdit _txtSerial = new();
    private readonly SimpleButton _btnActivate = new() { Text = "Activer" };
    private readonly SimpleButton _btnRefresh = new() { Text = "Vérifier en ligne" };
    private readonly SimpleButton _btnClose = new();

    internal LicenseForm(LicenseClient client, bool startup)
    {
        _client = client;
        _startup = startup;
        BuildLayout();
        ShowState(client.Current);
    }

    private void BuildLayout()
    {
        Text = "Licence CHIFA Pro";
        ClientSize = new Size(580, 346);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = _startup;
        StartPosition = _startup ? FormStartPosition.CenterScreen : FormStartPosition.CenterParent;
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (ArgumentException) { }

        var title = new LabelControl { Text = "Licence CHIFA Pro", Location = new Point(24, 18) };
        title.Appearance.Font = new Font("Segoe UI", 15f, FontStyle.Bold);

        _lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
        _lblStatus.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        _lblStatus.Appearance.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
        _lblStatus.SetBounds(24, 58, 532, 44);

        _lblDetails.AutoSizeMode = LabelAutoSizeMode.None;
        _lblDetails.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        _lblDetails.Appearance.TextOptions.VAlignment = VertAlignment.Top;
        _lblDetails.Appearance.Font = new Font("Segoe UI", 9.5f);
        _lblDetails.Appearance.ForeColor = Color.DimGray;
        _lblDetails.SetBounds(24, 108, 532, 112);

        _lblSerial.Location = new Point(24, 230);
        _txtSerial.SetBounds(24, 250, 390, 30);
        _txtSerial.Properties.CharacterCasing = CharacterCasing.Upper;
        _txtSerial.Properties.MaxLength = 40;
        _txtSerial.Properties.NullValuePrompt = "XXXXX-XXXXX-XXXXX-XXXXX-XXXXX";
        _txtSerial.Properties.Appearance.Font = new Font("Consolas", 12f);
        _txtSerial.ToolTip = "Un numéro de série ne fonctionne que sur un seul ordinateur.";
        _btnActivate.SetBounds(426, 249, 130, 32);

        _btnRefresh.SetBounds(24, 300, 150, 30);
        _btnClose.SetBounds(426, 300, 130, 30);

        _btnActivate.Click += BtnActivate_Click;
        _btnRefresh.Click += BtnRefresh_Click;
        _btnClose.Click += BtnClose_Click;

        Controls.AddRange([title, _lblStatus, _lblDetails, _lblSerial, _txtSerial, _btnActivate, _btnRefresh, _btnClose]);
        AcceptButton = _btnActivate;
    }

    private void ShowState(LicenseState state)
    {
        _lblStatus.Text = LicenseText.Describe(state);
        _lblStatus.Appearance.ForeColor = state.IsValid ? Color.SeaGreen : Color.Crimson;

        var license = state.License;
        var lines = new List<string>();
        if (license is not null)
        {
            lines.Add($"Client : {license.Customer}");
            lines.Add($"Édition : {license.Edition}");
            if (license.ExpiresAt is { } end)
            {
                var days = state.DaysRemaining(DateTimeOffset.UtcNow) ?? 0;
                lines.Add($"Abonnement valide jusqu'au : {end.LocalDateTime:dd/MM/yyyy}" +
                          (days >= 0 ? $" ({days} jour(s) restants)" : string.Empty));
            }
            lines.Add($"Numéro de série : •••••-•••••-•••••-•••••-{license.SerialHint}");
            lines.Add("Ce numéro de série est lié à cet ordinateur.");
        }
        lines.Add($"Identifiant du poste : {_client.MachineId[..Math.Min(12, _client.MachineId.Length)].ToUpperInvariant()}");
        _lblDetails.Text = string.Join(Environment.NewLine, lines);

        var canEnterSerial = !state.IsValid;
        _lblSerial.Visible = _txtSerial.Visible = _btnActivate.Visible = canEnterSerial;
        AcceptButton = canEnterSerial ? _btnActivate : _btnClose;
        _btnRefresh.Visible = license is not null;
        _btnClose.Text = !_startup ? "Fermer" : state.IsValid ? "Continuer" : "Quitter";
    }

    private async void BtnActivate_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtSerial.Text))
        {
            _txtSerial.Focus();
            return;
        }

        await RunAsync(() => _client.ActivateAsync(_txtSerial.Text), "CHIFA Pro est maintenant activé sur ce poste.");
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e) =>
        await RunAsync(() => _client.RefreshAsync(force: true), "La licence a été vérifiée en ligne.");

    private async Task RunAsync(Func<Task<LicenseState>> action, string successMessage)
    {
        try
        {
            SetBusy(true);
            var state = await action();
            ShowState(state);
            if (!state.IsValid) return;

            Program.Telemetry?.TrackFeatureUsed("license.valid");
            XtraMessageBox.Show(this, successMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            if (_startup)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
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

    private void BtnClose_Click(object? sender, EventArgs e)
    {
        DialogResult = _startup && _client.Current.IsValid ? DialogResult.OK : DialogResult.Cancel;
        Close();
    }

    private void SetBusy(bool busy)
    {
        UseWaitCursor = busy;
        _btnActivate.Enabled = _btnRefresh.Enabled = _btnClose.Enabled = _txtSerial.Enabled = !busy;
    }
}

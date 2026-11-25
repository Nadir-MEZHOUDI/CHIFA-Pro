namespace CHIFA.Pro.Views;

/// <summary>
/// Activation and license details. In <c>startup</c> mode it blocks the application until the
/// license is valid (Quitter closes SmartCHIFA); otherwise it is an information dialog.
/// <para>
/// A pharmacy without a serial sends a license request (« Demander une licence »). While it waits for
/// SmartApp's approval the dialog checks every few seconds and unlocks SmartCHIFA as soon as it is approved.
/// </para>
/// </summary>
public sealed partial class LicenseForm : XtraForm
{
    private readonly LicenseClient _client;
    private readonly bool _startup;
    private bool _busy;

    internal LicenseForm(LicenseClient client, bool startup)
    {
        InitializeComponent();
        _client = client;
        _startup = startup;
        if (startup)
        {
            ShowInTaskbar = true;
            StartPosition = FormStartPosition.CenterScreen;
        }
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (ArgumentException) { }
        ShowState(client.Current);
    }

    private PendingLicenseRequest? Pending => _client.Current.IsValid ? null : _client.PendingRequest;

    private async void PollTimer_Tick(object? sender, EventArgs e) => await PollRequestAsync();

    private void ShowState(LicenseState state, string? note = null)
    {
        var pending = Pending;
        var lines = new List<string>();

        if (pending is not null)
        {
            _lblStatus.Text = "Demande envoyée : en attente de la validation de SmartApp.";
            _lblStatus.Appearance.ForeColor = Color.DarkOrange;
            lines.Add($"Envoyée le {pending.SubmittedAt.LocalDateTime:dd/MM/yyyy à HH:mm} pour {pending.Info.BusinessName ?? pending.Info.FullName}.");
            lines.Add($"SmartApp vous appelle au {pending.Info.Phone} pour le paiement de l'abonnement.");
            lines.Add("Dès que le paiement est validé, SmartCHIFA s'active tout seul sur ce poste (connexion Internet nécessaire).");
        }
        else
        {
            _lblStatus.Text = note ?? LicenseText.Describe(state);
            _lblStatus.Appearance.ForeColor = state.IsValid ? Color.SeaGreen : Color.Crimson;
        }

        var license = state.License;
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

        var notLicensed = !state.IsValid;
        _lblSerial.Visible = _txtSerial.Visible = _btnActivate.Visible = _btnRequest.Visible = notLicensed;
        _btnRequest.Text = pending is null ? "Demander une licence (sans numéro de série)" : "Modifier ma demande";
        AcceptButton = notLicensed ? _btnActivate : _btnClose;
        _btnRefresh.Visible = license is not null || pending is not null;
        _btnClose.Text = !_startup ? "Fermer" : state.IsValid ? "Continuer" : "Quitter";

        _pollTimer.Enabled = pending is not null;
    }

    private async void BtnActivate_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_txtSerial.Text))
        {
            _txtSerial.Focus();
            return;
        }

        await RunAsync(() => _client.ActivateAsync(_txtSerial.Text), "SmartCHIFA est maintenant activé sur ce poste.");
    }

    private async void BtnRefresh_Click(object? sender, EventArgs e)
    {
        if (Pending is not null)
        {
            await PollRequestAsync(userAsked: true);
            return;
        }

        await RunAsync(() => _client.RefreshAsync(force: true), "La licence a été vérifiée en ligne.");
    }

    private void BtnRequest_Click(object? sender, EventArgs e)
    {
        using var form = new LicenseRequestForm(_client);
        if (form.ShowDialog(this) == DialogResult.OK)
            ShowState(_client.Current);
    }

    /// <summary>Asks the server whether the pending request was decided. Approved: the license is installed.</summary>
    private async Task PollRequestAsync(bool userAsked = false)
    {
        if (_busy || IsDisposed || Pending is null) return;
        try
        {
            SetBusy(true, userAsked);
            var result = await _client.CheckRequestAsync();
            if (IsDisposed) return;

            switch (result.Outcome)
            {
                case LicenseRequestOutcome.Approved:
                    _pollTimer.Enabled = false;
                    Program.Telemetry?.TrackFeatureUsed("license.request.approved");
                    ShowState(result.State);
                    XtraMessageBox.Show(this, "Votre demande a été validée : SmartCHIFA est maintenant activé sur ce poste.",
                        Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (_startup)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    break;
                case LicenseRequestOutcome.Rejected:
                    ShowState(result.State, "Votre demande a été refusée" +
                                            (string.IsNullOrWhiteSpace(result.Message) ? "." : " : " + result.Message) +
                                            " Vous pouvez envoyer une nouvelle demande.");
                    break;
                case LicenseRequestOutcome.None:
                    ShowState(result.State, LicenseText.DescribeError(result.ErrorCode));
                    break;
                case LicenseRequestOutcome.Failed when userAsked:
                    XtraMessageBox.Show(this, LicenseText.DescribeError(result.ErrorCode) ?? result.Message, Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case LicenseRequestOutcome.Pending when userAsked:
                    XtraMessageBox.Show(this, "Votre demande est toujours en attente de validation par SmartApp.", Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
        }
        catch (Exception ex)
        {
            ex.Log(showMessage: userAsked);
        }
        finally
        {
            if (!IsDisposed) SetBusy(false, userAsked);
        }
    }

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

    private void SetBusy(bool busy, bool showCursor = true)
    {
        _busy = busy;
        if (showCursor) UseWaitCursor = busy;
        _btnActivate.Enabled = _btnRefresh.Enabled = _btnRequest.Enabled = _btnClose.Enabled = _txtSerial.Enabled = !busy;
    }
}

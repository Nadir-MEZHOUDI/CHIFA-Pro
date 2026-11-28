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
        _lblSerial.Click += LblSerial_Click;
        Shown += (_, _) => { if (_btnTrial.Visible) _btnTrial.Select(); };
        ShowState(client.Current);
    }

    private bool _trialRefused;

    /// <summary>While the trial is offered, the serial field stays folded behind its label so the trial is the obvious choice.</summary>
    private bool _serialUnfolded;

    private void LblSerial_Click(object? sender, EventArgs e)
    {
        if (_serialUnfolded || _txtSerial.Visible) return;
        _serialUnfolded = true;
        ShowState(_client.Current);
        _txtSerial.Focus();
    }

    /// <summary>The license request of this computer; a trial keeps it so the paid license can replace the trial.</summary>
    private PendingLicenseRequest? Pending => _client.Current is { IsValid: true, IsTrial: false } ? null : _client.PendingRequest;

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
        if (license is not null && state.IsTrial)
        {
            lines.Add($"Version d'essai gratuite : {license.Customer}");
            if (license.ExpiresAt is { } end)
            {
                var days = state.DaysRemaining(DateTimeOffset.UtcNow) ?? 0;
                lines.Add($"Essai valable jusqu'au : {end.LocalDateTime:dd/MM/yyyy à HH:mm}" +
                          (days >= 0 ? $" ({days} jour(s) restants)" : string.Empty));
            }
            lines.Add("Pour continuer après l'essai, demandez une licence ou saisissez votre numéro de série.");
        }
        else if (license is not null)
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
            lines.Add(PharmacyLicenseId.IsPharmacy(_client.MachineId)
                ? "Ce numéro de série est lié au code PS de la pharmacie : tous ses ordinateurs l'utilisent."
                : "Ce numéro de série est lié à cet ordinateur.");
        }
        var id = PharmacyLicenseId.IsPharmacy(_client.MachineId) ? _client.MachineId[PharmacyLicenseId.Prefix.Length..] : _client.MachineId;
        lines.Add((PharmacyLicenseId.IsPharmacy(_client.MachineId) ? "Identifiant de la pharmacie : " : "Identifiant du poste : ") +
                  id[..Math.Min(12, id.Length)].ToUpperInvariant());
        _lblDetails.Text = string.Join(Environment.NewLine, lines);

        var canBuy = !state.IsValid || state.IsTrial;
        var offerTrial = !state.IsValid && !state.IsTrial && !_trialRefused;
        var showSerial = canBuy && (!offerTrial || _serialUnfolded);
        _btnTrial.Visible = offerTrial;
        _lblSerial.Visible = _btnRequest.Visible = canBuy;
        _txtSerial.Visible = _btnActivate.Visible = showSerial;
        _lblSerial.Text = showSerial ? "Vous avez déjà un numéro de série ?" : "Vous avez déjà un numéro de série ? Cliquez ici.";
        _lblSerial.Appearance.ForeColor = showSerial ? Color.Empty : Color.RoyalBlue;
        _lblSerial.Cursor = showSerial ? Cursors.Default : Cursors.Hand;
        _btnRequest.Text = pending is null ? "Demander une licence (sans numéro de série)" : "Modifier ma demande";
        AcceptButton = showSerial ? _btnActivate : offerTrial ? _btnTrial : _btnClose;
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

        if (!_client.Current.IsValid || _client.Current.IsTrial)
        {
            var fromPhone = await LicenseGate.InstallPhoneRequestAsync(_client);
            if (fromPhone is not null)
            {
                await RunAsync(() => Task.FromResult(fromPhone),
                    "La licence demandée depuis le téléphone est validée : SmartCHIFA est maintenant activé sur ce poste.");
                return;
            }
        }

        await RunAsync(() => _client.RefreshAsync(force: true), "La licence a été vérifiée en ligne.");
    }

    private void BtnTrial_Click(object? sender, EventArgs e)
    {
        using var form = new LicenseRequestForm(_client, trial: true);
        if (form.ShowDialog(this) != DialogResult.OK || form.TrialState is not { IsValid: true } state)
        {
            if (_client.Current.ErrorCode is SmartApp.Licensing.Core.LicenseErrorCodes.TrialUsed or SmartApp.Licensing.Core.LicenseErrorCodes.TrialUnavailable)
            {
                _trialRefused = true;
                ShowState(_client.Current);
            }
            return;
        }

        ShowState(state);
        var end = state.License?.ExpiresAt is { } trialEnd ? $" jusqu'au {trialEnd.LocalDateTime:dd/MM/yyyy à HH:mm}" : string.Empty;
        XtraMessageBox.Show(this,
            $"Votre essai gratuit a commencé : SmartCHIFA fonctionne sur ce poste{end}.\n\n" +
            "Pour continuer ensuite, demandez une licence depuis le menu « Licence ».",
            Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        if (_startup)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
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

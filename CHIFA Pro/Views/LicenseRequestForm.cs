namespace CHIFA.Pro.Views;

/// <summary>
/// Asks SmartApp for a license without a serial, or starts the free trial (<c>trial</c>). The pharmacy's
/// details come from the CHIFA database (Paramètres de l'officine); the phone number is required so
/// SmartApp can call about the payment.
/// </summary>
public sealed partial class LicenseRequestForm : XtraForm
{
    private readonly LicenseClient _client;
    private readonly bool _trial;

    internal LicenseRequestForm(LicenseClient client, bool trial = false)
    {
        InitializeComponent();
        _client = client;
        _trial = trial;
        if (trial)
        {
            Text = lblTitle.Text = "Essai gratuit de 7 jours";
            lblIntro.Text = "Vérifiez vos informations puis commencez l'essai : SmartCHIFA s'active tout de suite sur ce poste " +
                            "pour 7 jours, sans paiement. Un seul essai par ordinateur.";
            lblPhone.Text = "Téléphone *";
            _btnSend.Text = "Commencer l'essai";
        }
    }

    /// <summary>The trial license installed by « Commencer l'essai ».</summary>
    public LicenseState? TrialState { get; private set; }

    private async void LicenseRequestForm_Shown(object? sender, EventArgs e) => await FillAsync();

    /// <summary>The request was sent; the license dialog shows it as pending.</summary>
    public LicenseRequestResult? Result { get; private set; }

    /// <summary>Last sent details first, otherwise the pharmacy's details from the CHIFA database.</summary>
    private async Task FillAsync()
    {
        if (_client.PendingRequest?.Info is { } sent)
        {
            ShowInfo(sent);
            return;
        }

        try
        {
            UseWaitCursor = true;
            var officine = await ChifaService.Instance.GetFirstOfficineAsync().AsTask().WaitAsync(TimeSpan.FromSeconds(8));
            if (officine is null || IsDisposed) return;

            var name = string.Join(" ", new[] { officine.Nom, officine.Prenom }.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x!.Trim()));
            ShowInfo(new LicenseRequestInfo(
                FullName: name,
                Phone: officine.NumTel?.Trim() ?? string.Empty,
                BusinessName: officine.NomPharmacie?.Trim(),
                Email: officine.Email?.Trim(),
                Address: officine.Adresse?.Trim(),
                CodePs: officine.CodePs?.Trim()));
        }
        catch (Exception ex)
        {
            // The database may not be reachable yet: the pharmacist types the details.
            ex.Log(showMessage: false);
        }
        finally
        {
            if (!IsDisposed) UseWaitCursor = false;
        }

        (string.IsNullOrWhiteSpace(_txtFullName.Text) ? _txtFullName : _txtPhone).Focus();
    }

    private void ShowInfo(LicenseRequestInfo info)
    {
        SetIfEmpty(_txtFullName, info.FullName);
        SetIfEmpty(_txtPhone, info.Phone);
        SetIfEmpty(_txtPharmacy, info.BusinessName);
        SetIfEmpty(_txtEmail, info.Email);
        SetIfEmpty(_cboWilaya, info.Wilaya);
        SetIfEmpty(_txtAddress, info.Address);
        SetIfEmpty(_txtCodePs, info.CodePs);
        SetIfEmpty(_txtMessage, info.Message);
    }

    private static void SetIfEmpty(TextEdit editor, string? value)
    {
        if (string.IsNullOrWhiteSpace(editor.Text) && !string.IsNullOrWhiteSpace(value))
            editor.Text = value;
    }

    private async void BtnSend_Click(object? sender, EventArgs e)
    {
        _lblError.Text = string.Empty;
        if (string.IsNullOrWhiteSpace(_txtFullName.Text))
        {
            _lblError.Text = "Saisissez le nom et le prénom du pharmacien.";
            _txtFullName.Focus();
            return;
        }

        var digits = new string(_txtPhone.Text.Where(char.IsDigit).ToArray());
        if (digits.Length < 9)
        {
            _lblError.Text = _trial
                ? "Saisissez un numéro de téléphone valide."
                : "Saisissez un numéro de téléphone valide : SmartApp vous appelle pour le paiement.";
            _txtPhone.Focus();
            return;
        }

        var info = new LicenseRequestInfo(
            FullName: _txtFullName.Text.Trim(),
            Phone: _txtPhone.Text.Trim(),
            BusinessName: Clean(_txtPharmacy.Text),
            Email: Clean(_txtEmail.Text),
            Wilaya: Clean(_cboWilaya.Text),
            Address: Clean(_txtAddress.Text),
            CodePs: Clean(_txtCodePs.Text),
            Message: Clean(_txtMessage.Text));

        try
        {
            SetBusy(true);
            if (_trial)
            {
                var state = await _client.StartTrialAsync(info);
                if (!state.IsValid || !state.IsTrial)
                {
                    _lblError.Text = LicenseText.DescribeError(state.ErrorCode) ?? state.Message;
                    return;
                }

                Program.Telemetry?.TrackFeatureUsed("license.trial.started");
                LicenseGate.SaveContact(info);
                TrialState = state;
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            var result = await _client.SubmitRequestAsync(info);
            if (result.Outcome != LicenseRequestOutcome.Pending)
            {
                _lblError.Text = LicenseText.DescribeError(result.ErrorCode) ?? result.Message;
                return;
            }

            Program.Telemetry?.TrackFeatureUsed("license.request.sent");
            LicenseGate.SaveContact(info);
            Result = result;
            DialogResult = DialogResult.OK;
            Close();
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

    private void SetBusy(bool busy)
    {
        UseWaitCursor = busy;
        _btnSend.Enabled = _btnCancel.Enabled = !busy;
    }

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}

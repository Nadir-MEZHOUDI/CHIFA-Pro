using DevExpress.Utils;

namespace CHIFA.Pro.Views;

/// <summary>
/// Asks SmartApp for a license without a serial. The pharmacy's details come from the CHIFA database
/// (Paramètres de l'officine); the phone number is required so SmartApp can call about the payment.
/// </summary>
public sealed class LicenseRequestForm : XtraForm
{
    private static readonly string[] Wilayas =
    [
        "01 Adrar", "02 Chlef", "03 Laghouat", "04 Oum El Bouaghi", "05 Batna", "06 Béjaïa", "07 Biskra", "08 Béchar",
        "09 Blida", "10 Bouira", "11 Tamanrasset", "12 Tébessa", "13 Tlemcen", "14 Tiaret", "15 Tizi Ouzou", "16 Alger",
        "17 Djelfa", "18 Jijel", "19 Sétif", "20 Saïda", "21 Skikda", "22 Sidi Bel Abbès", "23 Annaba", "24 Guelma",
        "25 Constantine", "26 Médéa", "27 Mostaganem", "28 M'Sila", "29 Mascara", "30 Ouargla", "31 Oran", "32 El Bayadh",
        "33 Illizi", "34 Bordj Bou Arréridj", "35 Boumerdès", "36 El Tarf", "37 Tindouf", "38 Tissemsilt", "39 El Oued",
        "40 Khenchela", "41 Souk Ahras", "42 Tipaza", "43 Mila", "44 Aïn Defla", "45 Naâma", "46 Aïn Témouchent",
        "47 Ghardaïa", "48 Relizane", "49 Timimoun", "50 Bordj Badji Mokhtar", "51 Ouled Djellal", "52 Béni Abbès",
        "53 In Salah", "54 In Guezzam", "55 Touggourt", "56 Djanet", "57 El M'Ghair", "58 El Meniaa"
    ];

    private readonly LicenseClient _client;

    private readonly TextEdit _txtFullName = new();
    private readonly TextEdit _txtPhone = new();
    private readonly TextEdit _txtPharmacy = new();
    private readonly TextEdit _txtEmail = new();
    private readonly ComboBoxEdit _cboWilaya = new();
    private readonly TextEdit _txtAddress = new();
    private readonly TextEdit _txtTaxNumber = new();
    private readonly MemoEdit _txtMessage = new();
    private readonly LabelControl _lblError = new();
    private readonly SimpleButton _btnSend = new() { Text = "Envoyer la demande" };
    private readonly SimpleButton _btnCancel = new() { Text = "Annuler", DialogResult = DialogResult.Cancel };

    internal LicenseRequestForm(LicenseClient client)
    {
        _client = client;
        BuildLayout();
        Shown += async (_, _) => await FillAsync();
    }

    /// <summary>The request was sent; the license dialog shows it as pending.</summary>
    public LicenseRequestResult? Result { get; private set; }

    private void BuildLayout()
    {
        Text = "Demande de licence SmartCHIFA";
        ClientSize = new Size(600, 520);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (ArgumentException) { }

        var title = new LabelControl { Text = "Demander une licence", Location = new Point(24, 16) };
        title.Appearance.Font = new Font("Segoe UI", 14f, FontStyle.Bold);

        var intro = new LabelControl
        {
            Text = "Vérifiez vos informations puis envoyez la demande. SmartApp vous appelle pour le paiement de " +
                   "l'abonnement ; dès qu'il est validé, SmartCHIFA s'active tout seul sur ce poste, sans numéro de série à saisir.",
            AutoSizeMode = LabelAutoSizeMode.None
        };
        intro.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        intro.Appearance.ForeColor = Color.DimGray;
        intro.SetBounds(24, 50, 552, 40);

        Controls.AddRange([title, intro]);

        var y = 100;
        AddRow("Nom et prénom du pharmacien *", _txtFullName, ref y, 160);
        AddRow("Téléphone * (pour le paiement)", _txtPhone, ref y, 40);
        AddRow("Pharmacie", _txtPharmacy, ref y, 160);
        AddRow("Email", _txtEmail, ref y, 256);
        AddRow("Wilaya", _cboWilaya, ref y, 64);
        AddRow("Adresse", _txtAddress, ref y, 256);
        AddRow("NIF", _txtTaxNumber, ref y, 64);

        var lblMessage = new LabelControl { Text = "Message (facultatif)", Location = new Point(24, y + 6) };
        _txtMessage.SetBounds(230, y, 346, 54);
        _txtMessage.Properties.MaxLength = 1000;
        Controls.AddRange([lblMessage, _txtMessage]);
        y += 64;

        _cboWilaya.Properties.Items.AddRange(Wilayas);
        _cboWilaya.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        _txtPhone.Properties.NullValuePrompt = "05 / 06 / 07 XX XX XX XX";

        _lblError.AutoSizeMode = LabelAutoSizeMode.None;
        _lblError.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        _lblError.Appearance.ForeColor = Color.Crimson;
        _lblError.SetBounds(24, y, 552, 34);

        _btnSend.SetBounds(316, 474, 160, 32);
        _btnCancel.SetBounds(486, 474, 90, 32);
        _btnSend.Click += BtnSend_Click;

        Controls.AddRange([_lblError, _btnSend, _btnCancel]);
        AcceptButton = _btnSend;
        CancelButton = _btnCancel;
    }

    private void AddRow(string label, TextEdit editor, ref int y, int maxLength)
    {
        var lbl = new LabelControl { Text = label, Location = new Point(24, y + 6) };
        editor.SetBounds(230, y, 346, 26);
        editor.Properties.MaxLength = maxLength;
        Controls.AddRange([lbl, editor]);
        y += 36;
    }

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
                Message: string.IsNullOrWhiteSpace(officine.CodePs) ? null : $"Code PS : {officine.CodePs.Trim()}"));
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
        SetIfEmpty(_txtTaxNumber, info.TaxNumber);
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
            _lblError.Text = "Saisissez un numéro de téléphone valide : SmartApp vous appelle pour le paiement.";
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
            TaxNumber: Clean(_txtTaxNumber.Text),
            Message: Clean(_txtMessage.Text));

        try
        {
            SetBusy(true);
            var result = await _client.SubmitRequestAsync(info);
            if (result.Outcome != LicenseRequestOutcome.Pending)
            {
                _lblError.Text = LicenseText.DescribeError(result.ErrorCode) ?? result.Message;
                return;
            }

            Program.Telemetry?.TrackFeatureUsed("license.request.sent");
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

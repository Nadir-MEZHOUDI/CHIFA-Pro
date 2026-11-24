using CHIFA.Pro.Helpers.Settings;
using DevExpress.Utils;
using Npgsql;

namespace CHIFA.Pro.Views;

/// <summary>
/// Where the CHIFA_OFFICINE database is. Shown before the license dialog, which reads the pharmacy's
/// details from it. On the server computer the pharmacist drags the CHIFA_OFFICINE desktop icon here:
/// its target gives the CHIFA folder, and its PostgreSQL is started silently. The port is always 5432.
/// </summary>
public sealed class ConnectionForm : XtraForm
{
    private readonly CheckEdit _rbLocal = new() { Text = "CHIFA_OFFICINE est installé sur ce poste (serveur)" };
    private readonly CheckEdit _rbRemote = new() { Text = "La base de données est sur un autre poste" };
    private readonly PanelControl _dropZone = new();
    private readonly LabelControl _lblDrop = new();
    private readonly SimpleButton _btnBrowse = new() { Text = "Parcourir…" };
    private readonly LabelControl _lblServer = new() { Text = "Serveur :" };
    private readonly ComboBoxEdit _cboServer = new();
    private readonly LabelControl _lblStatus = new();
    private readonly SimpleButton _btnTest = new() { Text = "Tester" };
    private readonly SimpleButton _btnSave = new() { Text = "Enregistrer" };
    private readonly SimpleButton _btnCancel = new() { DialogResult = DialogResult.Cancel };

    private string? _chifaPath;
    private bool _busy;

    public ConnectionForm(bool startup = false)
    {
        BuildLayout(startup);

        var host = new NpgsqlConnectionStringBuilder(ChifaDb.ConString).Host;
        var local = AppSettings.Default.IsServer || DbChecker.IsLocalHost(host);
        _rbLocal.Checked = local;
        _rbRemote.Checked = !local;
        _cboServer.Text = local ? string.Empty : host;
        _chifaPath = DbChecker.IsChifaFolder(AppSettings.Default.ChifaPath) ? AppSettings.Default.ChifaPath : null;
        UpdateMode();

        Shown += async (_, _) => await LoadNetworkDevicesAsync();
    }

    private void BuildLayout(bool startup)
    {
        Text = "Base de données CHIFA_OFFICINE";
        ClientSize = new Size(600, 430);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = startup;
        StartPosition = startup ? FormStartPosition.CenterScreen : FormStartPosition.CenterParent;
        try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch (ArgumentException) { }

        var title = new LabelControl { Text = "Connexion à la base CHIFA_OFFICINE", Location = new Point(24, 16) };
        title.Appearance.Font = new Font("Segoe UI", 14f, FontStyle.Bold);

        var intro = new LabelControl
        {
            Text = "SmartCHIFA lit les données de CHIFA_OFFICINE. Indiquez où se trouve sa base de données.",
            AutoSizeMode = LabelAutoSizeMode.None
        };
        intro.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        intro.Appearance.ForeColor = Color.DimGray;
        intro.SetBounds(24, 50, 552, 22);

        _rbLocal.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
        _rbLocal.Properties.RadioGroupIndex = 1;
        _rbLocal.SetBounds(22, 84, 556, 24);
        _rbRemote.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
        _rbRemote.Properties.RadioGroupIndex = 1;
        _rbRemote.SetBounds(22, 236, 556, 24);
        _rbLocal.CheckedChanged += (_, _) => UpdateMode();
        _rbRemote.CheckedChanged += (_, _) => UpdateMode();

        _dropZone.SetBounds(46, 114, 400, 110);
        _dropZone.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
        _lblDrop.Dock = DockStyle.Fill;
        _lblDrop.AutoSizeMode = LabelAutoSizeMode.None;
        _lblDrop.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
        _lblDrop.Appearance.TextOptions.VAlignment = VertAlignment.Center;
        _lblDrop.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        _dropZone.Controls.Add(_lblDrop);
        _btnBrowse.SetBounds(456, 114, 120, 32);
        _btnBrowse.Click += BtnBrowse_Click;

        foreach (Control target in new Control[] { this, _dropZone, _lblDrop })
        {
            target.AllowDrop = true;
            target.DragEnter += OnDragEnter;
            target.DragDrop += OnDragDrop;
        }

        _lblServer.Location = new Point(48, 272);
        _cboServer.SetBounds(130, 268, 316, 26);
        _cboServer.Properties.NullValuePrompt = "Nom ou adresse IP du poste serveur";

        _lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
        _lblStatus.Appearance.TextOptions.WordWrap = WordWrap.Wrap;
        _lblStatus.SetBounds(24, 312, 552, 54);

        _btnCancel.Text = startup ? "Quitter" : "Annuler";
        _btnTest.SetBounds(196, 380, 120, 32);
        _btnSave.SetBounds(326, 380, 120, 32);
        _btnCancel.SetBounds(456, 380, 120, 32);
        _btnTest.Click += BtnTest_Click;
        _btnSave.Click += BtnSave_Click;

        Controls.AddRange([title, intro, _rbLocal, _dropZone, _btnBrowse, _rbRemote, _lblServer, _cboServer,
            _lblStatus, _btnTest, _btnSave, _btnCancel]);
        AcceptButton = _btnSave;
        CancelButton = _btnCancel;
    }

    private bool IsLocal => _rbLocal.Checked;

    private string Server => IsLocal ? "localhost" : _cboServer.Text.Trim();

    private void UpdateMode()
    {
        _dropZone.Enabled = _btnBrowse.Enabled = IsLocal;
        _cboServer.Enabled = !IsLocal;

        if (_chifaPath is null)
        {
            _lblDrop.Text = "Glissez ici l'icône CHIFA_OFFICINE\ndepuis le Bureau";
            _lblDrop.Appearance.ForeColor = Color.DimGray;
        }
        else
        {
            _lblDrop.Text = $"✔ CHIFA_OFFICINE\n{_chifaPath}";
            _lblDrop.Appearance.ForeColor = Color.SeaGreen;
        }
    }

    private void OnDragEnter(object? sender, DragEventArgs e) =>
        e.Effect = IsLocal && !_busy && e.Data?.GetDataPresent(DataFormats.FileDrop) == true
            ? DragDropEffects.Link
            : DragDropEffects.None;

    private async void OnDragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] { Length: > 0 } paths)
            await UseChifaLocationAsync(paths[0]);
    }

    private async void BtnBrowse_Click(object? sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Title = "Sélectionnez CHIFA_OFFICINE (raccourci du Bureau ou CHIFA_OFFICINE.exe)",
            Filter = "CHIFA_OFFICINE (*.lnk;*.exe)|*.lnk;*.exe",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
            DereferenceLinks = true
        };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            await UseChifaLocationAsync(dialog.FileName);
    }

    /// <summary>A shortcut, CHIFA_OFFICINE.exe or a folder: finds the CHIFA folder, then starts its server.</summary>
    private async Task UseChifaLocationAsync(string path)
    {
        var folder = FindChifaFolder(path);
        if (folder is null)
        {
            ShowStatus("Cette icône ne mène pas à CHIFA_OFFICINE : le dossier « CHIFA_OFFICINE_DB » est introuvable à côté du programme.", false);
            return;
        }

        _chifaPath = folder;
        UpdateMode();
        await TestAsync();
    }

    private static string? FindChifaFolder(string path)
    {
        if (path.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
            path = ShellShortcut.GetTarget(path) ?? string.Empty;

        var folder = Directory.Exists(path) ? path : File.Exists(path) ? Path.GetDirectoryName(path) : null;

        // The shortcut may point inside the installation (e.g. CHIFA_OFFICINE_DB\...): look a few levels up.
        for (var level = 0; folder is not null && level < 3; level++, folder = Path.GetDirectoryName(folder))
        {
            if (DbChecker.IsChifaFolder(folder)) return folder;
        }

        return null;
    }

    private async void BtnTest_Click(object? sender, EventArgs e) => await TestAsync();

    private async Task<bool> TestAsync()
    {
        if (!IsLocal && string.IsNullOrWhiteSpace(Server))
        {
            ShowStatus("Saisissez le nom ou l'adresse IP du poste serveur.", false);
            _cboServer.Focus();
            return false;
        }

        try
        {
            SetBusy(true);
            if (IsLocal && _chifaPath is not null)
            {
                ShowStatus("Démarrage du serveur CHIFA_OFFICINE…", null);
                AppSettings.Default.ChifaPath = _chifaPath;
                if (await DbChecker.StartLocalServerAsync())
                {
                    ShowStatus("Serveur CHIFA_OFFICINE démarré : connexion réussie.", true);
                    return true;
                }

                ShowStatus("Le serveur de CHIFA_OFFICINE n'a pas pu démarrer. Ouvrez CHIFA_OFFICINE une fois, puis cliquez sur « Tester ».", false);
                return false;
            }

            ShowStatus("Connexion en cours…", null);
            var con = new NpgsqlConnectionStringBuilder(ChifaDb.ConString) { Host = Server, Port = DbChecker.DefaultDbPort };
            if (await DbChecker.CheckDbConnectionAsync(con.ConnectionString, quiet: true))
            {
                ShowStatus("Connexion réussie.", true);
                return true;
            }

            ShowStatus(IsLocal
                ? "Base de données introuvable sur ce poste. Glissez ici l'icône CHIFA_OFFICINE du Bureau."
                : $"Connexion au poste « {Server} » impossible. Vérifiez que CHIFA_OFFICINE est ouvert sur ce poste et que le réseau fonctionne.", false);
            return false;
        }
        catch (Exception ex)
        {
            ex.Log(showMessage: false);
            ShowStatus(ex.Message, false);
            return false;
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        if (!await TestAsync()) return;

        try
        {
            if (IsLocal && _chifaPath is not null)
                AppSettings.Default.ChifaPath = _chifaPath;
            AppSettings.Default.IsServer = IsLocal;
            AppSettings.Default.Save();
            XtraHelper.SetServer(Server);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        // Cancelled: forget the folder tried in this dialog.
        if (DialogResult != DialogResult.OK) AppSettings.Default.Reset();
        base.OnFormClosed(e);
    }

    private async Task LoadNetworkDevicesAsync()
    {
        try
        {
            var devices = await Task.Run(XtraHelper.ListAllDevicesOnLocalNetwork);
            if (IsDisposed || devices is not { Length: > 0 }) return;
            _cboServer.Properties.Items.Clear();
            _cboServer.Properties.Items.AddRange(devices.Where(d => !DbChecker.IsLocalHost(d)).ToArray());
        }
        catch (Exception ex)
        {
            ex.Log(showMessage: false);
        }
    }

    private void ShowStatus(string text, bool? success)
    {
        _lblStatus.Text = text;
        _lblStatus.Appearance.ForeColor = success switch
        {
            true => Color.SeaGreen,
            false => Color.Crimson,
            null => Color.DimGray
        };
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        UseWaitCursor = busy;
        _btnTest.Enabled = _btnSave.Enabled = !busy;
        _btnBrowse.Enabled = !busy && IsLocal;
        _rbLocal.Enabled = _rbRemote.Enabled = !busy;
    }
}

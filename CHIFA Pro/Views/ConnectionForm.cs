using CHIFA.Pro.Helpers.Settings;
using Npgsql;

namespace CHIFA.Pro.Views;

/// <summary>
/// Where the CHIFA_OFFICINE database is. Shown before the license dialog, which reads the pharmacy's
/// details from it. On the server computer the pharmacist drags the CHIFA_OFFICINE desktop icon here:
/// its target gives the CHIFA folder, and its PostgreSQL is started silently. The port is always 5432.
/// </summary>
public sealed partial class ConnectionForm : XtraForm
{
    private string? _chifaPath;
    private bool _busy;

    public ConnectionForm(bool startup = false)
    {
        InitializeComponent();
        if (startup)
        {
            _btnCancel.Text = "Quitter";
            ShowInTaskbar = true;
            StartPosition = FormStartPosition.CenterScreen;
        }

        var host = new NpgsqlConnectionStringBuilder(ChifaDb.ConString).Host;
        var local = AppSettings.Default.IsServer || DbChecker.IsLocalHost(host);
        _rbLocal.Checked = local;
        _rbRemote.Checked = !local;
        _cboServer.Text = local ? string.Empty : host;
        _chifaPath = DbChecker.IsChifaFolder(AppSettings.Default.ChifaPath) ? AppSettings.Default.ChifaPath : null;
        UpdateMode();

        Shown += async (_, _) => await LoadNetworkDevicesAsync();
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

    private void Mode_CheckedChanged(object? sender, EventArgs e) => UpdateMode();

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
        if (!IsLocal && !DbConnectionRules.TryNormalizeHost(Server, out _, out var hostError))
        {
            ShowStatus(hostError!, false);
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

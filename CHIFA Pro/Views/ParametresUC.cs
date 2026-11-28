using CHIFA.Pro.Helpers.Settings;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;

namespace CHIFA.Pro.Views;

public partial class ParametersUc : XtraUserControl, INavigable
{
    /// <summary>The modern vector skins, plus « DevExpress Style », the app's look before WXI.</summary>
    private static readonly string[] Themes =
        ["WXI", "Office 2019 Colorful", "Office 2019 White", "Office 2019 Dark Gray", "Office 2019 Black", "The Bezier", "Basic", "DevExpress Style"];

    private readonly ComboBoxEdit _theme = new() { Name = "cmbTheme" };
    private readonly ComboBoxEdit _palette = new() { Name = "cmbPalette" };
    private bool _fillingPalettes;

    public string Caption { get; } = "PARAMÈTRES";
    public Image Image => FrmMain.Image(14);

    public ParametersUc()
    {
        InitializeComponent();
        AddAppearanceGroup();
    }

    private const string DefaultPalette = "Par défaut";

    /// <summary>Thème + couleurs, applied and saved at once (no need to press Enregistrer).</summary>
    private void AddAppearanceGroup()
    {
        _theme.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _palette.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _theme.Properties.Items.AddRange(Themes.Where(t => SkinManager.Default.Skins[t] is not null).ToArray());

        var group = layoutControlGroup1.AddGroup("APPARENCE", layoutControlGroup2, InsertType.Top);
        var themeItem = group.AddItem("Thème", _theme);
        group.AddItem("Couleurs", _palette, themeItem, InsertType.Right);

        _theme.EditValue = UserLookAndFeel.Default.SkinName;
        FillPalettes();

        _theme.SelectedIndexChanged += (_, _) =>
        {
            UserLookAndFeel.Default.SetSkinStyle(_theme.Text);
            FillPalettes();
            SaveTheme();
        };
        _palette.SelectedIndexChanged += (_, _) =>
        {
            if (_fillingPalettes) return;
            if (_palette.Text is DefaultPalette or "")
                UserLookAndFeel.Default.SetSkinStyle(_theme.Text);
            else
                UserLookAndFeel.Default.SetSkinStyle(_theme.Text, _palette.Text);
            SaveTheme();
        };
    }

    /// <summary>A palette-only change raises no StyleChanged, so Program's handler would not save it.</summary>
    private static void SaveTheme()
    {
        AppSettings.Default.Skin = UserLookAndFeel.Default.SkinName;
        AppSettings.Default.SkinPalette = UserLookAndFeel.Default.ActiveSvgPaletteName ?? "";
        AppSettings.Default.Save();
    }

    private void FillPalettes()
    {
        _fillingPalettes = true;
        try
        {
            var names = CommonSkins.GetSkin(UserLookAndFeel.Default).CustomSvgPalettes.Keys.Select(k => k.Name).ToArray();
            _palette.Properties.Items.Clear();
            if (names.Length > 0)
                _palette.Properties.Items.Add(DefaultPalette);
            _palette.Properties.Items.AddRange(names);
            var active = UserLookAndFeel.Default.ActiveSvgPaletteName;
            _palette.EditValue = names.Contains(active) ? active : names.Length > 0 ? DefaultPalette : null;
            _palette.Enabled = names.Length > 0;
        }
        finally
        {
            _fillingPalettes = false;
        }
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!DbConnectionRules.TryNormalizeHost(rbtServer.Checked ? "localhost" : txtServerName.Text, out var server, out var hostError))
            {
                XtraMessageBox.Show(hostError, "Paramètres", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            AppSettings.Default.ChifaPath = txtChifaPath.Text;
            AppSettings.Default.IsServer = rbtServer.Checked;

            AppSettings.Default.NotificationOnDays = chkDays.Checked;
            AppSettings.Default.NotificationOnMontant = chkMontant.Checked;
            AppSettings.Default.NotificationOnNmbr = chkNmbr.Checked;

            AppSettings.Default.MaxDays = Convert.ToInt32(txtJours.EditValue);
            AppSettings.Default.MaxMontant = Convert.ToInt32(txtMontant.EditValue);
            AppSettings.Default.MaxNmbr = Convert.ToInt32(txtFactures.EditValue);

            AppSettings.Default.Save();

            // Apply to the running session immediately, no restart required.
            XtraHelper.SetServer(server);

            var isConnected = await DbChecker.CheckDbConnectionAsync();
            if (isConnected)
            {
                XtraMessageBox.Show("Configuration enregistrée et appliquée. Connexion au serveur réussie.",
                    "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                XtraMessageBox.Show(
                    $@"Configuration enregistrée et appliquée, mais la connexion au serveur « {server} » a échoué.
Vérifiez le nom du serveur ou que PostgreSQL est démarré.",
                    "Connexion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    private async void ParametersUc_Load(object sender, EventArgs e)
    {
        try
        {
            LoadNotificationSettings();
            await LoadChifaServerInfo();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }



    private async Task LoadChifaServerInfo()
    {
        try
        {
            txtChifaPath.Text = AppSettings.Default.ChifaPath;
            txtServerName.Text = ChifaDb.Server;

            rbtServer.Checked = AppSettings.Default.IsServer;
            rbtServer.EditValue = AppSettings.Default.IsServer;

            rbtClient.Checked = !AppSettings.Default.IsServer;
            rbtClient.EditValue = !AppSettings.Default.IsServer;

            chkStartup.Checked = AppSettings.Default.StartupWithWindows;

            var items = await Task.Run(XtraHelper.ListAllDevicesOnLocalNetwork);

            if (items is not { Length: > 0 }) return;

            txtServerName.Properties.Items.Clear();
            txtServerName.Properties.Items.AddRange(items);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void btnBackup_Click(object sender, EventArgs e)
    {
        try
        {
            await DbChecker.SaveBackup();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void btnRestore_Click(object sender, EventArgs e)
    {
        try
        {
            var dialog = new OpenFileDialog()
            {
                Title = @"Sélectionner le fichier de sauvegarde",
                DefaultExt = "backup",
                InitialDirectory = AppSettings.Default.ChifaBackup,
                Filter = @"Fichiers de sauvegarde (*.backup)|*.backup",
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                await DbChecker.Restore(dialog.FileName);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtChifaPath.Text = dialog.SelectedPath;
        }
    }

    private async void btnTest_Click(object sender, EventArgs e)
    {
        try
        {
            if (!DbConnectionRules.TryNormalizeHost(rbtServer.Checked ? "localhost" : txtServerName.Text, out var server, out var hostError))
            {
                XtraMessageBox.Show(hostError, "Paramètres", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            var isConnected = await DbChecker.CheckDbConnectionAsync(server);

            if (isConnected)
            {
                XtraMessageBox.Show("Connexion réussie.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                XtraMessageBox.Show("Impossible de se connecter au serveur.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    private void rbtServer_CheckedChanged(object sender, EventArgs e)
    {
        txtServerName.Enabled = rbtClient.Checked;
        txtChifaPath.Enabled = rbtServer.Checked;
    }

    private void rbtClient_CheckedChanged(object sender, EventArgs e)
    {
        txtServerName.Enabled = rbtClient.Checked;
        txtChifaPath.Enabled = rbtServer.Checked;
    }

    private void LoadNotificationSettings()
    {
        try
        {
            chkNmbr.EditValue = AppSettings.Default.NotificationOnNmbr;
            chkMontant.EditValue = AppSettings.Default.NotificationOnMontant;
            chkDays.EditValue = AppSettings.Default.NotificationOnDays;

            txtFactures.EditValue = AppSettings.Default.MaxNmbr;
            txtMontant.EditValue = AppSettings.Default.MaxMontant;
            txtJours.EditValue = AppSettings.Default.MaxDays;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void chkStartup_CheckStateChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkStartup.Checked)
            {
                AppStartup.AddApplicationToStartup();
            }
            else
            {
                AppStartup.RemoveApplicationFromStartup();
            }

            AppSettings.Default.StartupWithWindows = chkStartup.Checked;
            AppSettings.Default.Save();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }




}

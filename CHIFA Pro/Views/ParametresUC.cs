using CHIFA.Pro.Helpers.Settings;

namespace CHIFA.Pro.Views;

public partial class ParametersUc : XtraUserControl, INavigable
{
    private Action? _closer;
    public string Caption { get; } = "PARAMETERS";
    public Image Image => FrmMain.Image(14);

    public ParametersUc()
    {
        InitializeComponent();
    }

    public static void ShowAsForm()
    {
        var frm = new XtraForm
        {
            StartPosition = FormStartPosition.CenterScreen,
            Text = @"Parameters",
            Size = new Size(600, 300)
        };
        frm.Controls.Add(new ParametersUc { Dock = DockStyle.Fill, _closer = frm.Close });
        frm.ShowDialog();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            AppSettings.Default.ChifaPath = txtChifaPath.Text;
            AppSettings.Default.IsServer = rbtServer.Checked;
            XtraHelper.SetServer(rbtServer.Checked ? "localhost" : txtServerName.Text);

            AppSettings.Default.NotificationOnDays = chkDays.Checked;
            AppSettings.Default.NotificationOnMontant = chkMontant.Checked;
            AppSettings.Default.NotificationOnNmbr = chkNmbr.Checked;

            AppSettings.Default.MaxDays = Convert.ToInt32(txtJours.EditValue);
            AppSettings.Default.MaxMontant = Convert.ToInt32(txtMontant.EditValue);
            AppSettings.Default.MaxNmbr = Convert.ToInt32(txtFactures.EditValue);


            AppSettings.Default.Save();
            XtraMessageBox.Show("Configuration successfully Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _closer?.Invoke();
        }
        catch (Exception ex)
        {
            ex.Log();
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
                Title = @"Select Backup File",
                DefaultExt = "backup",
                InitialDirectory = AppSettings.Default.ChifaBackup,
                Filter = @"Backup Files (*.backup)|*.backup",
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
            Cursor.Current = Cursors.WaitCursor;
            await DbChecker.CheckDbConnectionAsync(rbtServer.Checked ? "localhost" : txtServerName.Text);
            XtraMessageBox.Show("Connected", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            XtraMessageBox.Show("Cannot connect ot server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ex.Log();
        }
        finally
        {
            Cursor.Current = Cursors.Default;
        }
    }

    private void txtChifaPath_EditValueChanged(object sender, EventArgs e)
    {

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
        if (chkStartup.Checked)
        {
            AppStartup.AddApplicationToStartup();
        }
        else
        {
            AppStartup.RemoveApplicationFromStartup();
        }
    }

  

   
}
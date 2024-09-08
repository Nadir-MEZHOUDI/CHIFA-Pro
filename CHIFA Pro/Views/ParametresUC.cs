namespace CHIFA.Pro.Others;

public partial class ParametersUc : XtraUserControl,INavigable
{
    public Action? Closer;
    public string Caption { get; } = "PARAMETERS";
    public Image Image => frmMain.Image(14);

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
        frm.Controls.Add(new ParametersUc { Dock = DockStyle.Fill, Closer = frm.Close });
        frm.ShowDialog();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            AppSettings.Default.ChifaPath = txtChifaPath.Text;
            AppSettings.Default.Save();
            XtraHelper.SetServer(txtServerName.Text);
            XtraMessageBox.Show("Configuration successfully Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Closer?.Invoke();
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
            txtChifaPath.Text = AppSettings.Default.ChifaPath;
            txtServerName.Text = ChifaDb.Server;

            var items = await Task.Run(XtraHelper.ListAllDevicesOnLocalNetwork);

            if (items is { Length: > 0 })
            {
                txtServerName.Properties.Items.Clear();
                txtServerName.Properties.Items.AddRange(items);
            }
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
                await DbChecker.Restor(dialog.FileName);
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
            await DbChecker.CheckDbConnectionAsync(txtServerName.Text);
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

}
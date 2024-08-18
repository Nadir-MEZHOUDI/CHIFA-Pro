
using DevExpress.CodeParser;

using Npgsql;

using AppSettings = Helpers.Settings.AppSettings;

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
            AppSettings.Default.Port = int.TryParse(txtPort.Text, out int port) ? port : 5432;
            AppSettings.Default.ChifaPath = txtChifaPath.Text;
            AppSettings.Default.ServerName = txtServerName.Text;
            AppSettings.Default.Save();
            ChifaDb.ConString = DbChecker.ConnectionString;
            XtraMessageBox.Show("Configuration succesfully Saved", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtPort.Text = AppSettings.Default.Port.ToString();
            txtChifaPath.Text = AppSettings.Default.ChifaPath;
            txtServerName.Text = AppSettings.Default.ServerName;

            var items = await Task.Run(DbChecker.ListNetworkComputers);

            if (items is { Count: > 0 })
            {
                txtServerName.Properties.Items.Clear();
                txtServerName.Properties.Items.Add("localhost");
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

    private async void btnRestor_Click(object sender, EventArgs e)
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
        //select folder
        var dialog = new FolderBrowserDialog();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtChifaPath.Text = dialog.SelectedPath;
        }
    }

    private async void btnTest_Click(object sender, EventArgs e)
    {
        NpgsqlConnection? con = null;
        try
        {
            Cursor.Current = Cursors.WaitCursor;

            string ConnectionString = $"Server={txtServerName.Text}; Port={txtPort.Text}; User Id={AppSettings.Default.DbUser}; Password={AppSettings.Default.DbPassword}; Database={AppSettings.Default.DbName};";
            con = new NpgsqlConnection(ConnectionString);
            con.Open();
            XtraMessageBox.Show("Connected", "Sucess", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        catch (Exception ex)
        {
            XtraMessageBox.Show("Cannot connect ot server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            ex.Log();
        }
        finally
        {
            con?.Close();
            Cursor.Current = Cursors.Default;
        }
    }
}
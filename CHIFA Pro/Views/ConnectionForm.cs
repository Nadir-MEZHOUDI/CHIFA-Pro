using Npgsql;

namespace CHIFA.Pro.Views;

public sealed partial class ConnectionForm
{

    public string Server => _txtServer.Text.Trim();
    public int Port => Convert.ToInt32(_spnPort.Value);

    public ConnectionForm()
    {
        InitializeComponent();
        var con = new NpgsqlConnectionStringBuilder(ChifaDb.ConString);
        var currentServer = con.Host;
        var currentPort = con.Port;
        _txtServer.Text = string.IsNullOrWhiteSpace(currentServer) ? "localhost" : currentServer;
        _spnPort.EditValue = currentPort > 0 ? currentPort : DbChecker.DefaultDbPort;
    }

    private async void BtnTest_Click(object? sender, EventArgs e)
    {
        try
        {
            ToggleButtons(false);
            var con = new NpgsqlConnectionStringBuilder(ChifaDb.ConString)
            {
                Host = Server,
                Port = Port
            };
            var isConnected = await DbChecker.CheckDbConnectionAsync(con.ConnectionString);
            if (isConnected)
            {
                XtraMessageBox.Show("Connexion etablie avec succes.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            XtraMessageBox.Show("Echec de la connexion. Verifiez les informations puis reessayez.", "Erreur",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            ToggleButtons(true);
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        try
        {
            XtraHelper.SetServerAndPort(Server, Port);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }


    private void ToggleButtons(bool enabled)
    {
        _btnTest.Enabled = enabled;
        _btnSave.Enabled = enabled;
    }
}

namespace CHIFA.Pro.Views;

public partial class OfficineUc : XtraUserControl,INavigable
{
    public string Caption { get; } = "OFFICINE";
    public Image Image => FrmMain.Image(13);
    public OfficineUc()
    {
        InitializeComponent();
    }

    private async void OfficineUC_Load(object sender, EventArgs e)
    {
        var officine = await DataService.GetFirstOfficineAsync().ConfigureAwait(true);
        if (officine == null) return;
        dataLayoutControl1.DataSource = officine;
    }
}
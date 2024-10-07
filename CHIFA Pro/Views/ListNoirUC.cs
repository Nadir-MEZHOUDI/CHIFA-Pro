
namespace CHIFA.Pro.Views;

public partial class ListNoirUc : XtraUserControl,INavigable
{
    public ListNoirUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
    }

    public string Caption { get; }= "LISTE NOIR";
    public Image Image => FrmMain.Image(9);

    private async void ListNoirUc_Load(object sender, EventArgs e)
    {
        await gridView1.LoadDataAsync(DataService.LoadAllListNoirAsync);
    }
}
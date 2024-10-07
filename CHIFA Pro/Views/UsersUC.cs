
namespace CHIFA.Pro.Views;

public partial class UsersUc : XtraUserControl, INavigable
{


    public UsersUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        Load += async (_, _) => await gridView1.LoadDataAsync(DataService.GetUsersAsync);
    }

    public string Caption { get; } = "UTILISATEURS";
    public Image Image => FrmMain.Image(11);
}
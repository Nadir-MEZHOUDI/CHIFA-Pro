
namespace CHIFA.Pro.Views;

public partial class BordereauxUc : XtraUserControl, INavigable
{
    public BordereauxUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        Load += async (_, _) => await gridView1.LoadDataAsync(() => DataService.GetAllBordereauxAsync());
    }

    public string Caption { get; } = "BORDEREAUX";
    public Image Image => frmMain.Image(3);
}

namespace CHIFA.Pro.Others;

public partial class FormesUc : XtraUserControl, INavigable
{
    public FormesUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        Load += async (_, _) => await gridView1.LoadDataAsync(DataService.GetFormesAsync);
    }

    public string Caption { get; } = "FORMES";
    public Image Image => frmMain.Image(7);
}
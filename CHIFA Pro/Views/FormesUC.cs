
namespace CHIFA.Pro.Views;

public partial class FormesUc : XtraUserControl, INavigable
{
    public FormesUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        Load += async (_, _) => await gridView1.LoadDataAsync(ChifaService.Instance.GetFormesAsync);
    }

    public string Caption { get; } = "FORMES";
    public Image Image => FrmMain.Image(7);
}
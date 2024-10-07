namespace CHIFA.Pro.Views;

public partial class SpecialitesUc : XtraUserControl, INavigable
{
    public SpecialitesUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        Load += async (_, _) => await gridView1.LoadDataAsync(DataService.SpecialitesAsync);
    }

    public string Caption { get; } = "SPECIALITES";

    public Image Image => FrmMain.Image(8);
}
namespace CHIFA.Pro.Others;

public partial class SpecialitesUc : XtraUserControl, INavigable
{
    public SpecialitesUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        Load += async (_, _) => await gridView1.LoadDataAsync(DataService.SpecialistesAsync);
    }

    public string Caption { get; } = "SPECIALITES";

    public Image Image => frmMain.Image(8);
}
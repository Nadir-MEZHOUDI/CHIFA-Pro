
namespace CHIFA.Pro.uc;

public partial class ControlMedicalUc : XtraUserControl,INavigable
{
    public ControlMedicalUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        Load += async (_, _) => await gridView1.LoadDataAsync(DataService.LoadControlsMedicalAsync);
    }

    public string Caption { get; } = "CONTROL MEDICAL";
    public Image Image => frmMain.Image(10);
}
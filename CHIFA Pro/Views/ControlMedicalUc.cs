namespace CHIFA.Pro.Views;

public partial class ControlMedicalUc : XtraUserControl,INavigable
{
    public ControlMedicalUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        gridView1.HorzScrollVisibility = ScrollVisibility.Auto;

        Load += async (_, _) => await gridView1.LoadDataAsync(ChifaService.Instance.LoadControlsMedicalAsync);
    }

    public string Caption { get; } = "CONTRÔLE MÉDICAL";
    public Image Image => FrmMain.Image(10);
}
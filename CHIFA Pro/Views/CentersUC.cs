namespace CHIFA.Pro.Views;

public partial class CentersUc : XtraUserControl, INavigable
{
    public CentersUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
    }

    public string Caption { get; } = "CENTRES";
    public Image Image => FrmMain.Image(12);

    private void btnCancel_Click(object sender, EventArgs e)
    {
        cENTREBindingSource.CancelEdit();
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            cENTREBindingSource.EndEdit();
            if (cENTREBindingSource.Current is Centre centre)
                await ChifaService.Instance.UpdateCenter(centre);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void CentersUC_Load(object sender, EventArgs e)
    {
        try
        {
            var a = await ChifaService.Instance.GetCentersAsync();
            Invoke(() => cENTREBindingSource.DataSource = a);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}
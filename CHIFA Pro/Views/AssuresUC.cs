namespace CHIFA.Pro.Views;

public partial class AssuresUc : XtraUserControl, INavigable
{
    public AssuresUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        gridView1.RowCellStyle += (_, e) =>
        {
            if (e.Column == colFinDroit && e.CellValue is DateTime fin && fin.Date < DateTime.Today)
                e.Appearance.ForeColor = Color.Red;
        };
        Load += async (_, _) => await gridView1.LoadDataAsync(ChifaService.Instance.GetBeneficiaresAsync);
    }

    public string Caption { get; } = "ASSURES";
    public Image Image => FrmMain.Image(5);

    private void btnConsumption_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        OpenHistoryOfSelectedAssure();
    }

    private void gridView1_DoubleClick(object sender, EventArgs e)
    {
        OpenHistoryOfSelectedAssure();
    }

    private void OpenHistoryOfSelectedAssure()
    {
        if (gridView1.GetRow(gridView1.FocusedRowHandle) is BeneficiareDto row)
            new FrmHistory(row.NumAssure!, row.Rang!).Show();
    }
}
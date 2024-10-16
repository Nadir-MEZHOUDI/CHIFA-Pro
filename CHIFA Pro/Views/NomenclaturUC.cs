namespace CHIFA.Pro.Views;

public partial class NomenclaturUc : XtraUserControl,INavigable
{
    public NomenclaturUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
    }

    public string Caption { get; } = "NOMENCLATURE";
    public Image Image => FrmMain.Image(6);

    private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        var cm = Convert.ToBoolean(gridView1?.GetRowCellValue(e.RowHandle, "CM"));
        if (cm)
            e.Appearance.BackColor = Color.CornflowerBlue;
    }

    private async void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
        if (gridView1.GetRow(gridView1.FocusedRowHandle) is MedicDto row)
            txtObs.Text = await ChifaService.Instance.GetMedicObsAsync(row.NEnr!).ConfigureAwait(true);
    }

    private async void NomenclaturUc_Load(object sender, EventArgs e)
    {
        await gridView1.LoadDataAsync(() => ChifaService.Instance.GetMedicamentsAsync());
    }
}
 
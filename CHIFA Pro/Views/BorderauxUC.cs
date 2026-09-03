using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class BordereauxUc : XtraUserControl, INavigable
{
    public BordereauxUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        gridView1.CustomDrawCell += GridView1_CustomDrawCell;
        Load += async (_, _) => await gridView1.LoadDataAsync(() => ChifaService.Instance.GetAllBordereauxAsync());
    }

    public string Caption { get; } = "BORDEREAUX";
    public Image Image => FrmMain.Image(3);

    private void GridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (gridView1.GetRow(e.RowHandle) is not BordereauDto row)
            return;

        if (row.MontantRejete > 0)
        {
            if (e.Column.FieldName is "MontantRejete" or "TauxRejet" or "StatutRejet")
            {
                e.Appearance.BackColor = Color.FromArgb(255, 235, 235);
                e.Appearance.ForeColor = Color.Crimson;
            }
        }
        else
        {
            if (e.Column.FieldName == "StatutRejet")
            {
                e.Appearance.BackColor = Color.FromArgb(235, 255, 235);
                e.Appearance.ForeColor = Color.SeaGreen;
            }
        }
    }
}

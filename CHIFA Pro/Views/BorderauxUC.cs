using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class BordereauxUc : XtraUserControl, INavigable
{
    public BordereauxUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        EnsureMontantAssureColumn();
        gridView1.CustomDrawCell += GridView1_CustomDrawCell;
        Load += async (_, _) => await gridView1.LoadDataAsync(() => ChifaService.Instance.GetAllBordereauxAsync());
    }

    public string Caption { get; } = "BORDEREAUX";
    public Image Image => FrmMain.Image(3);

    private void EnsureMontantAssureColumn()
    {
        var existing = gridView1.Columns.ColumnByFieldName("MontAss");
        if (existing is not null)
        {
            existing.Caption = "Mnt Ass";
            existing.DisplayFormat.FormatString = "n";
            existing.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            existing.Visible = true;
            return;
        }

        var column = new DevExpress.XtraGrid.Columns.GridColumn
        {
            FieldName = "MontAss",
            Caption = "Mnt Ass",
            Name = "colMontAss_gridView1",
            Visible = true,
            VisibleIndex = gridView1.VisibleColumns.Count
        };
        column.DisplayFormat.FormatString = "n";
        column.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
        column.Summary.AddRange([new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MontAss", "{0:N2}")]);
        gridView1.Columns.Add(column);
    }

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

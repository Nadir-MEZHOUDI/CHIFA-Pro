using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class FrmTraitSpec : XtraForm
{
    public FrmTraitSpec()
    {
        InitializeComponent();
        gridViewTraitSpec1.SetOptions();
        gridViewTraitSpec2.SetOptions();
    }

    private async void frmTraitSpec_Load(object sender, EventArgs e)
    {
        await LoadData1();
        await LoadData2();
    }

    private void GetHistoryOfSelectedPatient()
    {
        if (gridViewTraitSpec2.GetRow(gridViewTraitSpec2.FocusedRowHandle) is TraitSpec2 row)
            new FrmHistory(row.NumAssure!, row.Rang).Show();
    }

    private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (gridViewTraitSpec1?.GetRow(e.RowHandle) is PatientWithTraitSpec row)
            e.Appearance.BackColor = (row.TS, row.LongDuree) switch
            {
                (true, _) => Colors.Green,
                (_, true) => Colors.Tan,
                (_, _) => e.Appearance.BackColor
            };
    }

    private void gridView2_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if ((sender as GridView)?.GetRow(e.RowHandle) is TraitSpec2 row)
            e.Appearance.BackColor = (row.TS, row.Duree >= 60, DateTime.Now > row.Prochain) switch
            {
                (_, _, true) => Colors.Red,
                (true, _, _) => Colors.Green,
                (_, true, _) => Colors.Tan,
                (_, _, _) => e.Appearance.BackColor
            };
    }

    private void gridView2_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
    {
        e.Appearance.BackColor = Colors.Green;
    }

    private void gridView2_DoubleClick(object sender, EventArgs e) => GetHistoryOfSelectedPatient();

    private Task LoadData1() => gridViewTraitSpec1.LoadDataAsync(() => DataService.PatientsWithTraitSpecAsync());

    private async Task LoadData2()
    {
        await gridViewTraitSpec2.LoadDataAsync(() => DataService.PatientsWithTraitSpec2Async());
        var firstGroupColumn = gridViewTraitSpec2.SortInfo[0].Column;
        GroupSummarySortInfo[] groupSummaryToSort = { new(gridViewTraitSpec2.GroupSummary[2], firstGroupColumn, ColumnSortOrder.Descending) };
        gridViewTraitSpec2.GroupSummarySortInfo.ClearAndAddRange(groupSummaryToSort);
    }
}
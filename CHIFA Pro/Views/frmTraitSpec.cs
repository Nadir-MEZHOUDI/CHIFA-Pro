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
        try
        {
            await LoadData1();
            await LoadData2();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void GetHistoryOfSelectedPatient()
    {
        if (gridViewTraitSpec2.GetRow(gridViewTraitSpec2.FocusedRowHandle) is TraitSpec2 row)
            new FrmHistory(row.NumAssure!, row.Rang).Show();
    }

    private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (gridViewTraitSpec1?.GetRow(e.RowHandle) is PatientWithTraitSpec row && RowColors.For(row.TS, row.LongDuree) is { } color)
            e.Appearance.BackColor = color;
    }

    private void gridView2_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if ((sender as GridView)?.GetRow(e.RowHandle) is TraitSpec2 row && RowColors.For(row.TS, row.Duree >= 60, DateTime.Now > row.Prochain) is { } color)
            e.Appearance.BackColor = color;
    }

    private void gridView2_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
    {
        e.Appearance.BackColor = RowColors.Green;
    }

    private void gridView2_DoubleClick(object sender, EventArgs e) => GetHistoryOfSelectedPatient();

    private Task LoadData1() => gridViewTraitSpec1.LoadDataAsync(() => ChifaService.Instance.PatientsWithTraitSpecAsync());

    private async Task LoadData2()
    {
        await gridViewTraitSpec2.LoadDataAsync(() => ChifaService.Instance.PatientsWithTraitSpec2Async());
        if (gridViewTraitSpec2.SortInfo.Count == 0 || gridViewTraitSpec2.GroupSummary.Count <= 2) return;
        var firstGroupColumn = gridViewTraitSpec2.SortInfo[0].Column;
        GroupSummarySortInfo[] groupSummaryToSort = { new(gridViewTraitSpec2.GroupSummary[2], firstGroupColumn, ColumnSortOrder.Descending) };
        gridViewTraitSpec2.GroupSummarySortInfo.ClearAndAddRange(groupSummaryToSort);
    }
}

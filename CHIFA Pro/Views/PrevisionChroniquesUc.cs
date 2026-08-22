using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class PrevisionChroniquesUc : XtraUserControl, INavigable
{
    private readonly SemaphoreSlim _reloadLock = new(1, 1);
    private List<ForecastRefillDto> _allForecasts = [];

    public string Caption { get; } = "PRÉVISION DES CHRONIQUES";
    public Image Image => FrmMain.Image(6);

    public PrevisionChroniquesUc()
    {
        InitializeComponent();
        viewForecast.SetOptions();
        viewForecast.CustomDrawCell += ViewForecast_CustomDrawCell;

        Load += async (_, _) => await ReloadDataAsync();
        Disposed += (_, _) => _reloadLock.Dispose();
    }

    private void ViewForecast_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewForecast.GetRow(e.RowHandle) is ForecastRefillDto row)
        {
            if (row.JoursRestants < 0)
            {
                e.Appearance.BackColor = Color.FromArgb(255, 235, 235);
                if (e.Column.FieldName == "StatutObservance" || e.Column.FieldName == "JoursRestants")
                    e.Appearance.ForeColor = Color.Crimson;
            }
            else if (row.JoursRestants <= 7)
            {
                e.Appearance.BackColor = Color.FromArgb(255, 250, 230);
                if (e.Column.FieldName == "StatutObservance")
                    e.Appearance.ForeColor = Color.DarkOrange;
            }
            else
            {
                e.Appearance.BackColor = Color.FromArgb(235, 255, 235);
                if (e.Column.FieldName == "StatutObservance")
                    e.Appearance.ForeColor = Color.SeaGreen;
            }
        }
    }

    private async Task ReloadDataAsync()
    {
        if (!await _reloadLock.WaitAsync(0)) return;

        try
        {
            Cursor = Cursors.WaitCursor;
            var summary = await ScopeService.Instance.GetChronicForecastsAsync(6, 30);
            _allForecasts = summary.Details;

            lblTotalVal.Text = $"{summary.TotalPatientsAttendus:N0}";
            lblOverdueVal.Text = $"{summary.PatientsEnRetard:N0}";
            lblThisWeekVal.Text = $"{summary.PatientsCetteSemaine:N0}";
            lblCaEstimeVal.Text = $"{summary.EstimationCaPrevisionnel:N2} DA";

            gridForecast.DataSource = _allForecasts;
            viewForecast.BestFitColumns();

            ConfigureSummary();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            Cursor = Cursors.Default;
            _reloadLock.Release();
        }
    }

    private void ConfigureSummary()
    {
        viewForecast.OptionsView.ShowFooter = true;
        if (viewForecast.Columns.Count == 0) return;

        var numAssureCol = viewForecast.Columns["NumAssure"];
        if (numAssureCol != null)
        {
            numAssureCol.Summary.Clear();
            numAssureCol.Summary.Add(DevExpress.Data.SummaryItemType.Count, "NumAssure", "Patients: {0:N0}");
        }

        var montCol = viewForecast.Columns["MontantMoyenHabituel"];
        if (montCol != null)
        {
            montCol.Summary.Clear();
            montCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "MontantMoyenHabituel", "Total: {0:N2} DA");
        }
    }

    private async void BtnRefresh_ItemClick(object sender, ItemClickEventArgs e)
    {
        await ReloadDataAsync();
    }

    private void BtnFilterAll_ItemClick(object sender, ItemClickEventArgs e)
    {
        gridForecast.DataSource = _allForecasts;
    }

    private void BtnFilterOverdue_ItemClick(object sender, ItemClickEventArgs e)
    {
        gridForecast.DataSource = _allForecasts.Where(p => p.JoursRestants < 0).ToList();
    }

    private void BtnFilterThisWeek_ItemClick(object sender, ItemClickEventArgs e)
    {
        gridForecast.DataSource = _allForecasts.Where(p => p.JoursRestants >= 0 && p.JoursRestants <= 7).ToList();
    }

    private void BtnExport_ItemClick(object sender, ItemClickEventArgs e)
    {
        gridForecast.Export("Exporter le Planning Prévisionnel des Chroniques", $"Planning_Chroniques_Chifa_{DateTime.Today:yyyyMMdd}");
    }
}

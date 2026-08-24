using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class RejetsUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private CancellationTokenSource? _reloadCts;
    private readonly Period _period = new();

    public string Caption { get; } = "GESTION DES REJETS";
    public Image Image => FrmMain.Image(9);

    public RejetsUc()
    {
        InitializeComponent();
        viewRejets.SetOptions();
        viewRejets.CustomDrawCell += ViewRejets_CustomDrawCell;

        Load += RejetsUc_Load;
        Disposed += RejetsUc_Disposed;
    }

    private void ViewRejets_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewRejets.GetRow(e.RowHandle) is RejetDto row)
        {
            if (row.MontantRejete > 0)
            {
                if (e.Column.FieldName == "MontantRejete" || e.Column.FieldName == "TauxRejet" || e.Column.FieldName == "StatutRejet")
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

    private async void RejetsUc_Load(object? sender, EventArgs e)
    {
        await LoadDateFiltersAsync();
        await ReloadDataAsync(ResetReloadToken());
    }

    private async Task LoadDateFiltersAsync()
    {
        try
        {
            await ChifaService.Instance.GetMinAndMaxDatesAsync();
            var lastYear = DateTime.Today.AddYears(-1);

            repoDateFrom.MinValue = Period.MinDate;
            repoDateFrom.MaxValue = Period.MaxDate;
            repoDateTo.MinValue = Period.MinDate;
            repoDateTo.MaxValue = Period.MaxDate;

            txtDateFrom.EditValue = lastYear;
            txtDateTo.EditValue = Period.MaxDate;

            txtDateFrom.EditValueChanged += TxtDate_EditValueChanged;
            txtDateTo.EditValueChanged += TxtDate_EditValueChanged;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void TxtDate_EditValueChanged(object? sender, EventArgs e)
    {
        ScheduleReload();
    }

    private void ScheduleReload()
    {
        var token = ResetReloadToken();

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(ReloadDebounceDelay, token);
                if (token.IsCancellationRequested) return;

                if (InvokeRequired)
                    Invoke(new Action(async () => await ReloadDataAsync(token)));
                else
                    await ReloadDataAsync(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { ex.Log(); }
        }, token);
    }

    private CancellationToken ResetReloadToken()
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = new CancellationTokenSource();
        return _reloadCts.Token;
    }

    private async Task ReloadDataAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (cancellationToken.IsCancellationRequested) return;

            _period.From = txtDateFrom.EditValue as DateTime?;
            _period.To = txtDateTo.EditValue as DateTime?;

            Cursor = Cursors.WaitCursor;
            var data = (await ScopeService.Instance.GetRejetsAsync(_period)).ToList();

            if (cancellationToken.IsCancellationRequested) return;

            var totalFact = data.Sum(r => r.MontantFacture);
            var totalVir = data.Sum(r => r.MontantVirement);
            var totalRejet = data.Sum(r => r.MontantRejete);

            lblTotalFactureVal.Text = $"{totalFact:N2} DA";
            lblTotalVirementVal.Text = $"{totalVir:N2} DA";
            lblTotalRejetVal.Text = $"{totalRejet:N2} DA";

            gridRejets.DataSource = data;
            viewRejets.BestFitColumns();

            ConfigureSummary();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private void ConfigureSummary()
    {
        viewRejets.OptionsView.ShowFooter = true;
        if (viewRejets.Columns.Count == 0) return;

        var numBordCol = viewRejets.Columns["NumBord"];
        if (numBordCol != null)
        {
            numBordCol.Summary.Clear();
            numBordCol.Summary.Add(DevExpress.Data.SummaryItemType.Count, "NumBord", "Bordereaux: {0:N0}");
        }

        var factCol = viewRejets.Columns["MontantFacture"];
        if (factCol != null)
        {
            factCol.Summary.Clear();
            factCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "MontantFacture", "Total: {0:N2} DA");
        }

        var virCol = viewRejets.Columns["MontantVirement"];
        if (virCol != null)
        {
            virCol.Summary.Clear();
            virCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "MontantVirement", "Total: {0:N2} DA");
        }

        var rejCol = viewRejets.Columns["MontantRejete"];
        if (rejCol != null)
        {
            rejCol.Summary.Clear();
            rejCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "MontantRejete", "Total: {0:N2} DA");
        }
    }

    private async void BtnRefresh_ItemClick(object sender, ItemClickEventArgs e)
    {
        await ReloadDataAsync(ResetReloadToken());
    }

    private void BtnClearDates_ItemClick(object sender, ItemClickEventArgs e)
    {
        txtDateFrom.EditValue = Period.MinDate;
        txtDateTo.EditValue = Period.MaxDate;
    }

    private void BtnExport_ItemClick(object sender, ItemClickEventArgs e)
    {
        gridRejets.Export("Exporter le Suivi des Rejets", $"Suivi_Rejets_Chifa_{DateTime.Today:yyyyMMdd}");
    }

    private void RejetsUc_Disposed(object? sender, EventArgs e)
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = null;
    }
}

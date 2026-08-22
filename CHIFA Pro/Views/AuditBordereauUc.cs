using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class AuditBordereauUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private readonly SemaphoreSlim _reloadLock = new(1, 1);
    private CancellationTokenSource? _reloadCts;
    private readonly Period _period = new();

    public string Caption { get; } = "AUDIT PRÉ-BORDEREAU";
    public Image Image => FrmMain.Image(10);

    public AuditBordereauUc()
    {
        InitializeComponent();
        viewAnomalies.SetOptions();
        viewAnomalies.CustomDrawCell += ViewAnomalies_CustomDrawCell;

        Load += AuditBordereauUc_Load;
        Disposed += AuditBordereauUc_Disposed;
    }

    private void ViewAnomalies_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewAnomalies.GetRow(e.RowHandle) is AuditAnomalyDto row)
        {
            if (row.Severite == AnomalySeverity.Critique)
            {
                e.Appearance.BackColor = Color.FromArgb(255, 235, 235);
                if (e.Column.FieldName == "Severite" || e.Column.FieldName == "TypeAnomalie")
                    e.Appearance.ForeColor = Color.Crimson;
            }
            else if (row.Severite == AnomalySeverity.Avertissement)
            {
                e.Appearance.BackColor = Color.FromArgb(255, 250, 230);
                if (e.Column.FieldName == "Severite")
                    e.Appearance.ForeColor = Color.DarkOrange;
            }
        }
    }

    private async void AuditBordereauUc_Load(object? sender, EventArgs e)
    {
        await LoadDateFiltersAsync();
        await RunAuditAsync();
    }

    private async Task LoadDateFiltersAsync()
    {
        try
        {
            await ChifaService.Instance.GetMinAndMaxDatesAsync();
            var thisMonthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            repoDateFrom.MinValue = Period.MinDate;
            repoDateFrom.MaxValue = Period.MaxDate;
            repoDateTo.MinValue = Period.MinDate;
            repoDateTo.MaxValue = Period.MaxDate;

            txtDateFrom.EditValue = thisMonthStart;
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
        ScheduleAudit();
    }

    private void ScheduleAudit()
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = new CancellationTokenSource();
        var token = _reloadCts.Token;

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(ReloadDebounceDelay, token);
                if (token.IsCancellationRequested) return;

                if (InvokeRequired)
                    Invoke(new Action(async () => await RunAuditAsync(token)));
                else
                    await RunAuditAsync(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { ex.Log(); }
        }, token);
    }

    private async Task RunAuditAsync(CancellationToken cancellationToken = default)
    {
        if (!await _reloadLock.WaitAsync(0, cancellationToken)) return;

        try
        {
            _period.From = txtDateFrom.EditValue as DateTime?;
            _period.To = txtDateTo.EditValue as DateTime?;

            Cursor = Cursors.WaitCursor;
            var report = await ScopeService.Instance.AuditBordereauAsync(null, _period);

            if (cancellationToken.IsCancellationRequested) return;

            lblConformesVal.Text = $"{report.FacturesConformes:N0}";
            lblAnomaliesVal.Text = $"{report.FacturesAvecAnomalies:N0}";
            lblRisqueVal.Text = $"{report.MontantRisqueRejet:N2} DA";

            gridAnomalies.DataSource = report.Anomalies;
            viewAnomalies.BestFitColumns();

            ConfigureColumns();
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

    private void ConfigureColumns()
    {
        if (viewAnomalies.Columns.Count == 0) return;

        SetColumn("NumFact", "N° Facture", 0);
        SetColumn("NumBord", "N° Bordereau", 1);
        SetColumn("DateFact", "Date Facture", 2);
        SetColumn("NumAssure", "N° Assuré", 3);
        SetColumn("NomAssure", "Assuré", 4);
        SetColumn("NomProduit", "Produit / Médicament", 5);
        SetColumn("TypeAnomalie", "Type d'Anomalie", 6);
        SetColumn("Severite", "Sévérité", 7);
        SetColumn("Description", "Description de l'Anomalie", 8);
        SetColumn("ActionRecommandee", "Action Recommandée", 9);
        SetColumn("MontantImpacte", "Montant Impacté", 10);

        viewAnomalies.BestFitColumns();
    }

    private void SetColumn(string fieldName, string caption, int visibleIndex)
    {
        var col = viewAnomalies.Columns[fieldName];
        if (col != null)
        {
            col.Caption = caption;
            col.VisibleIndex = visibleIndex;
        }
    }

    private void ConfigureSummary()
    {
        viewAnomalies.OptionsView.ShowFooter = true;
        if (viewAnomalies.Columns.Count == 0) return;

        var numFactCol = viewAnomalies.Columns["NumFact"];
        if (numFactCol != null)
        {
            numFactCol.Summary.Clear();
            numFactCol.Summary.Add(DevExpress.Data.SummaryItemType.Count, "NumFact", "Total: {0:N0}");
        }

        var montCol = viewAnomalies.Columns["MontantImpacte"];
        if (montCol != null)
        {
            montCol.Summary.Clear();
            montCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "MontantImpacte", "Risque: {0:N2} DA");
        }
    }

    private async void BtnRunAudit_ItemClick(object sender, ItemClickEventArgs e)
    {
        await RunAuditAsync();
    }

    private void BtnClearDates_ItemClick(object sender, ItemClickEventArgs e)
    {
        txtDateFrom.EditValue = Period.MinDate;
        txtDateTo.EditValue = Period.MaxDate;
    }

    private void BtnExport_ItemClick(object sender, ItemClickEventArgs e)
    {
        gridAnomalies.Export("Exporter le Rapport d'Audit Pré-Bordereau",
            $"Rapport_Audit_Chifa_{DateTime.Today:yyyyMMdd}", "Exportation du rapport terminée avec succès !");
    }

    private void AuditBordereauUc_Disposed(object? sender, EventArgs e)
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadLock.Dispose();
    }
}

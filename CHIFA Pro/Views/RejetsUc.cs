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
            else if (!row.EstPaye)
            {
                // Sent but not paid yet (or still open): to follow up, not a rejection.
                if (e.Column.FieldName is "StatutRejet" or "MontantEnAttente")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 248, 225);
                    e.Appearance.ForeColor = Color.DarkOrange;
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

            DateFilter.Configure(repoDateFrom, repoDateTo);

            txtDateFrom.EditValue = lastYear;
            txtDateTo.EditValue = null; // up to today

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

            _period.From = DateFilter.Value(txtDateFrom.EditValue);
            _period.To = DateFilter.Value(txtDateTo.EditValue);

            Cursor = Cursors.WaitCursor;
            var data = (await ScopeService.Instance.GetRejetsAsync(_period)).ToList();

            if (cancellationToken.IsCancellationRequested) return;

            // KPIs compare only paid bordereaux, so "dû - viré" matches "rejeté".
            // Bordereaux still waiting for their virement are reported separately.
            var paid = data.Where(r => r.EstPaye).ToList();
            var pending = data.Where(r => r.MontantEnAttente > 0).ToList();

            lblTotalFactureTitle.Text = "TOTAL DÛ — BORDEREAUX PAYÉS";
            lblTotalFactureVal.Text = $"{paid.Sum(r => r.MontantFacture):N2} DA";
            lblTotalVirementVal.Text = $"{paid.Sum(r => r.MontantVirement):N2} DA";
            lblTotalRejetVal.Text = $"{data.Sum(r => r.MontantRejete):N2} DA";
            lblSubtitle.Text = "Dû = part caisse + majorations + FAE.  En attente de virement : " +
                               $"{pending.Sum(r => r.MontantEnAttente):N2} DA ({pending.Count} bordereaux)";

            gridRejets.DataSource = data;

            ConfigureColumns();
            ConfigureSummary();
            viewRejets.BestFitColumns();
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

    private void ConfigureColumns()
    {
        if (viewRejets.Columns.Count == 0) return;

        string[] order =
        [
            "NumBord", "Centre", "DateBord", "DateDepotFtp", "NombreFactures",
            "MontantPartCaisse", "MontantMajorations", "MontantFae", "MontantFacture",
            "MontantVirement", "MontantRejete", "TauxRejet", "MontantEnAttente", "StatutRejet", "MotifPrincipal"
        ];
        var captions = new Dictionary<string, string>
        {
            ["NumBord"] = "N° Bordereau",
            ["Centre"] = "Centre",
            ["DateBord"] = "Date Clôture",
            ["DateDepotFtp"] = "Dépôt FTP",
            ["NombreFactures"] = "Factures",
            ["MontantPartCaisse"] = "Part Caisse (DA)",
            ["MontantMajorations"] = "MAJ (DA)",
            ["MontantFae"] = "FAE (DA)",
            ["MontantFacture"] = "Total Dû (DA)",
            ["MontantVirement"] = "Virement (DA)",
            ["MontantRejete"] = "Rejeté (DA)",
            ["TauxRejet"] = "Taux Rejet (%)",
            ["MontantEnAttente"] = "En Attente (DA)",
            ["StatutRejet"] = "Statut",
            ["MotifPrincipal"] = "Motif"
        };

        foreach (DevExpress.XtraGrid.Columns.GridColumn col in viewRejets.Columns)
        {
            var index = Array.IndexOf(order, col.FieldName);
            col.Visible = index >= 0; // internal fields (EstOuvert, EstPaye, JoursAttente, Etat...) stay hidden
            if (index < 0) continue;
            col.VisibleIndex = index;
            col.Caption = captions[col.FieldName];
        }
    }

    private void ConfigureSummary()
    {
        viewRejets.OptionsView.ShowFooter = true;
        if (viewRejets.Columns.Count == 0) return;

        var pendingCol = viewRejets.Columns["MontantEnAttente"];
        if (pendingCol != null)
        {
            pendingCol.Summary.Clear();
            pendingCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "MontantEnAttente", "Total: {0:N2} DA");
        }

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
        txtDateTo.EditValue = null; // up to today
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

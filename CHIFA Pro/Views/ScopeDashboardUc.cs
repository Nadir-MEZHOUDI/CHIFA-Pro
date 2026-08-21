using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class ScopeDashboardUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private readonly SemaphoreSlim _reloadLock = new(1, 1);
    private CancellationTokenSource? _reloadCts;
    private readonly Period _period = new();

    public string Caption { get; } = "CHIFA SCOPE";
    public Image Image => FrmMain.Image(4);

    public ScopeDashboardUc()
    {
        InitializeComponent();
        viewHourly.SetOptions();
        viewTopProducts.SetOptions();

        Load += ScopeDashboardUc_Load;
        Disposed += ScopeDashboardUc_Disposed;
    }

    private async void ScopeDashboardUc_Load(object? sender, EventArgs e)
    {
        await LoadDateFiltersAsync();
        await ReloadDataAsync();
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
        ScheduleReload();
    }

    private void ScheduleReload()
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
                    Invoke(new Action(async () => await ReloadDataAsync(token)));
                else
                    await ReloadDataAsync(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { ex.Log(); }
        }, token);
    }

    private async Task ReloadDataAsync(CancellationToken cancellationToken = default)
    {
        if (!await _reloadLock.WaitAsync(0, cancellationToken)) return;

        try
        {
            _period.From = txtDateFrom.EditValue as DateTime?;
            _period.To = txtDateTo.EditValue as DateTime?;

            Cursor = Cursors.WaitCursor;

            var dashboard = await ScopeService.Instance.GetScopeDashboardAsync(_period);

            if (cancellationToken.IsCancellationRequested) return;

            // 1. Top KPI Summary Cards
            lblCaVal.Text = $"{dashboard.MontantTotalFact:N2} DA";
            lblFactVal.Text = $"{dashboard.NombreFactures:N0}";
            lblBoitesVal.Text = $"{dashboard.NombreBoites:N0}";
            lblMajVal.Text = $"{dashboard.MontantTotalMajoration:N2} DA";

            // 2. Hourly Activity Grid
            gridHourly.DataSource = dashboard.ActiviteHoraire;
            viewHourly.BestFitColumns();

            // 3. Top 10 Products Grid
            gridTopProducts.DataSource = dashboard.TopMedicaments;
            viewTopProducts.BestFitColumns();

            // 4. Center KPIs Dashboard
            PopulateCenterKpis(dashboard.RepartitionParCaisse, dashboard.MontantTotalFact);
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

    private void PopulateCenterKpis(List<CenterSummaryDto> centers, decimal montantTotalGlobal)
    {
        if (centers == null || centers.Count == 0)
        {
            ResetCenterKpis();
            return;
        }

        // Segment Centers into CNAS, CASNOS, and Others
        var cnasList = centers.Where(c => IsCnas(c.Centre)).ToList();
        var casnosList = centers.Where(c => IsCasnos(c.Centre)).ToList();
        var autresList = centers.Where(c => !IsCnas(c.Centre) && !IsCasnos(c.Centre)).ToList();

        // 1. CNAS KPI Card
        var cnasMontant = cnasList.Sum(c => c.MontantTotal);
        var cnasFactures = cnasList.Sum(c => c.NombreFactures);
        var cnasPartCaisse = cnasList.Sum(c => c.MontantCaisse);
        var cnasPartAssure = cnasList.Sum(c => c.MontantAssure);
        var cnasPct = montantTotalGlobal > 0 ? (cnasMontant / montantTotalGlobal) * 100m : 0m;

        lblCnasMontant.Text = $"{cnasMontant:N2} DA";
        lblCnasFactures.Text = $"Nombre de factures : {cnasFactures:N0}";
        lblCnasPartCaisse.Text = $"Part Caisse (Tiers Payant) : {cnasPartCaisse:N2} DA";
        lblCnasPartAssure.Text = $"Part Assuré / Reste : {cnasPartAssure:N2} DA";
        lblCnasPct.Text = $"Part d'Activité : {cnasPct:N1} %";

        // 2. CASNOS KPI Card
        var casnosMontant = casnosList.Sum(c => c.MontantTotal);
        var casnosFactures = casnosList.Sum(c => c.NombreFactures);
        var casnosPartCaisse = casnosList.Sum(c => c.MontantCaisse);
        var casnosPartAssure = casnosList.Sum(c => c.MontantAssure);
        var casnosPct = montantTotalGlobal > 0 ? (casnosMontant / montantTotalGlobal) * 100m : 0m;

        lblCasnosMontant.Text = $"{casnosMontant:N2} DA";
        lblCasnosFactures.Text = $"Nombre de factures : {casnosFactures:N0}";
        lblCasnosPartCaisse.Text = $"Part Caisse (Tiers Payant) : {casnosPartCaisse:N2} DA";
        lblCasnosPartAssure.Text = $"Part Assuré / Reste : {casnosPartAssure:N2} DA";
        lblCasnosPct.Text = $"Part d'Activité : {casnosPct:N1} %";

        // 3. Autres Caisses KPI Card
        var autresMontant = autresList.Sum(c => c.MontantTotal);
        var autresFactures = autresList.Sum(c => c.NombreFactures);
        var autresPartCaisse = autresList.Sum(c => c.MontantCaisse);
        var autresPartAssure = autresList.Sum(c => c.MontantAssure);
        var autresPct = montantTotalGlobal > 0 ? (autresMontant / montantTotalGlobal) * 100m : 0m;

        lblAutresMontant.Text = $"{autresMontant:N2} DA";
        lblAutresFactures.Text = $"Nombre de factures : {autresFactures:N0}";
        lblAutresPartCaisse.Text = $"Part Caisse (Tiers Payant) : {autresPartCaisse:N2} DA";
        lblAutresPartAssure.Text = $"Part Assuré / Reste : {autresPartAssure:N2} DA";
        lblAutresPct.Text = $"Part d'Activité : {autresPct:N1} %";

        // 4. Global Shares Synthesis
        var totalPartCaisse = centers.Sum(c => c.MontantCaisse);
        var totalPartAssure = centers.Sum(c => c.MontantAssure);
        var totalMontant = totalPartCaisse + totalPartAssure;
        var tauxCouverture = totalMontant > 0 ? (totalPartCaisse / totalMontant) * 100m : 0m;
        var pctPartCaisse = totalMontant > 0 ? (totalPartCaisse / totalMontant) * 100m : 0m;
        var pctPartAssure = totalMontant > 0 ? (totalPartAssure / totalMontant) * 100m : 0m;

        lblTotalPartCaisseVal.Text = $"Total Part Caisses (Tiers Payant) : {totalPartCaisse:N2} DA ({pctPartCaisse:N1} %)";
        lblTotalPartAssureVal.Text = $"Total Part Assurés (Ticket Modérateur) : {totalPartAssure:N2} DA ({pctPartAssure:N1} %)";
        lblTauxCouvertureVal.Text = $"Taux global de prise en charge par les caisses : {tauxCouverture:N1} %";
    }

    private static bool IsCnas(string? center)
    {
        if (string.IsNullOrWhiteSpace(center)) return false;
        var upper = center.Trim().ToUpperInvariant();
        return upper.Contains("CNAS") || (!upper.Contains("CASNOS") && !upper.Contains("AUTRE") && !upper.Contains("MUTUELLE") && !upper.Contains("MILITAIRE"));
    }

    private static bool IsCasnos(string? center)
    {
        if (string.IsNullOrWhiteSpace(center)) return false;
        return center.Trim().ToUpperInvariant().Contains("CASNOS");
    }

    private void ResetCenterKpis()
    {
        lblCnasMontant.Text = "0.00 DA";
        lblCnasFactures.Text = "Nombre de factures : 0";
        lblCnasPartCaisse.Text = "Part Caisse : 0.00 DA";
        lblCnasPartAssure.Text = "Part Assuré : 0.00 DA";
        lblCnasPct.Text = "Part d'Activité : 0.0 %";

        lblCasnosMontant.Text = "0.00 DA";
        lblCasnosFactures.Text = "Nombre de factures : 0";
        lblCasnosPartCaisse.Text = "Part Caisse : 0.00 DA";
        lblCasnosPartAssure.Text = "Part Assuré : 0.00 DA";
        lblCasnosPct.Text = "Part d'Activité : 0.0 %";

        lblAutresMontant.Text = "0.00 DA";
        lblAutresFactures.Text = "Nombre de factures : 0";
        lblAutresPartCaisse.Text = "Part Caisse : 0.00 DA";
        lblAutresPartAssure.Text = "Part Assuré : 0.00 DA";
        lblAutresPct.Text = "Part d'Activité : 0.0 %";

        lblTotalPartCaisseVal.Text = "Total Part Caisses : 0.00 DA (0 %)";
        lblTotalPartAssureVal.Text = "Total Part Assurés : 0.00 DA (0 %)";
        lblTauxCouvertureVal.Text = "Taux global de prise en charge : 0 %";
    }

    private async void BtnRefresh_ItemClick(object sender, ItemClickEventArgs e)
    {
        await ReloadDataAsync();
    }

    private void BtnClearDates_ItemClick(object sender, ItemClickEventArgs e)
    {
        txtDateFrom.EditValue = Period.MinDate;
        txtDateTo.EditValue = Period.MaxDate;
    }

    private void ScopeDashboardUc_Disposed(object? sender, EventArgs e)
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadLock.Dispose();
    }
}

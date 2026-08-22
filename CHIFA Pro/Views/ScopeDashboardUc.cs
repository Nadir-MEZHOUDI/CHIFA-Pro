using System.Drawing;
using DevExpress.XtraBars;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;

namespace CHIFA.Pro.Views;

public partial class ScopeDashboardUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private readonly SemaphoreSlim _reloadLock = new(1, 1);
    private CancellationTokenSource? _reloadCts;
    private readonly Period _period = new();
    private bool _suspendDateEvents;

    public string Caption { get; } = "TOUR DE CONTRÔLE";
    public Image Image => FrmMain.Image(4);

    public ScopeDashboardUc()
    {
        InitializeComponent();
        SetupCharts();

        Load += ScopeDashboardUc_Load;
        Disposed += ScopeDashboardUc_Disposed;
    }

    private void SetupCharts()
    {
        // 1. Setup Hourly Distribution Horizontal Bar Chart
        chartHourly.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        chartHourly.Titles.Clear();
        var titleHourly = new ChartTitle
        {
            Text = "Distribution Horaire & Heures de Pointe (Nombre de Factures)",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            TextColor = Color.DarkSlateBlue
        };
        chartHourly.Titles.Add(titleHourly);

        var seriesHourly = new Series("Activité Horaire", ViewType.Bar)
        {
            ArgumentDataMember = "TrancheHoraire",
            ValueDataMembersSerializable = "NombreFactures",
            LabelsVisibility = DevExpress.Utils.DefaultBoolean.True
        };

        if (seriesHourly.View is SideBySideBarSeriesView hourlyBarView)
        {
            hourlyBarView.ColorEach = true;
            hourlyBarView.FillStyle.FillMode = FillMode.Gradient;
        }

        if (seriesHourly.Label is SideBySideBarSeriesLabel hourlyLabel)
        {
            hourlyLabel.TextPattern = "{V:N0}";
            hourlyLabel.Position = BarSeriesLabelPosition.Top;
        }

        chartHourly.Series.Clear();
        chartHourly.Series.Add(seriesHourly);

        if (chartHourly.Diagram is XYDiagram diagHourly)
        {
            diagHourly.Rotated = true;
            diagHourly.AxisX.Reverse = true;
            diagHourly.AxisX.Label.Angle = 0;
            diagHourly.AxisX.Label.Font = new Font("Segoe UI", 8.25F);
            diagHourly.AxisX.Title.Text = "Tranche Horaire";
            diagHourly.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagHourly.AxisY.Title.Text = "Nombre de Factures";
            diagHourly.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        }

        // 2. Setup Top 10 Products Horizontal Bar Chart
        chartTopProducts.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        chartTopProducts.Titles.Clear();
        var titleProducts = new ChartTitle
        {
            Text = "Top 10 Médicaments Dispensés (Quantité en Boîtes)",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            TextColor = Color.DarkSlateBlue
        };
        chartTopProducts.Titles.Add(titleProducts);

        var seriesProducts = new Series("Top Médicaments", ViewType.Bar)
        {
            ArgumentDataMember = "Designation",
            ValueDataMembersSerializable = "QuantiteTotale",
            LabelsVisibility = DevExpress.Utils.DefaultBoolean.True
        };

        if (seriesProducts.View is SideBySideBarSeriesView prodBarView)
        {
            prodBarView.ColorEach = true;
            prodBarView.FillStyle.FillMode = FillMode.Gradient;
        }

        if (seriesProducts.Label is SideBySideBarSeriesLabel prodLabel)
        {
            prodLabel.TextPattern = "{V:N0} btes";
            prodLabel.Position = BarSeriesLabelPosition.Top;
        }

        chartTopProducts.Series.Clear();
        chartTopProducts.Series.Add(seriesProducts);

        if (chartTopProducts.Diagram is XYDiagram diagProducts)
        {
            diagProducts.Rotated = true;
            diagProducts.AxisX.Reverse = true;
            diagProducts.AxisX.Label.Angle = 0;
            diagProducts.AxisX.Label.Font = new Font("Segoe UI", 8.25F);
            diagProducts.AxisX.Title.Text = "Médicaments";
            diagProducts.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            diagProducts.AxisY.Title.Text = "Nombre de Boîtes";
            diagProducts.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
        }
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
        if (_suspendDateEvents) return;
        ScheduleReload();
    }

    private void SetDates(DateTime from, DateTime to)
    {
        try
        {
            _suspendDateEvents = true;
            _period.From = from;
            _period.To = to;
            txtDateFrom.EditValue = from;
            txtDateTo.EditValue = to;
        }
        finally
        {
            _suspendDateEvents = false;
        }

        ScheduleReload();
    }

    private void BtnAllPeriod_ItemClick(object sender, ItemClickEventArgs e)
    {
        SetDates(Period.MinDate, Period.MaxDate);
    }

    private void BtnLastYear_ItemClick(object sender, ItemClickEventArgs e)
    {
        SetDates(DateTime.Now.AddYears(-1), DateTime.Now);
    }

    private void Btn6Months_ItemClick(object sender, ItemClickEventArgs e)
    {
        SetDates(DateTime.Now.AddMonths(-6), DateTime.Now);
    }

    private void BtnThisYear_ItemClick(object sender, ItemClickEventArgs e)
    {
        SetDates(new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now);
    }

    private void BtnThisMonth_ItemClick(object sender, ItemClickEventArgs e)
    {
        SetDates(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);
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

            // 1. Concise KPIs
            lblCaVal.Text = $"{dashboard.MontantTotalFact:N2} DA";
            lblFactVal.Text = $"{dashboard.NombreFactures:N0}";
            lblBoitesVal.Text = $"{dashboard.NombreBoites:N0}";

            var cnasList = dashboard.RepartitionParCaisse.Where(c => IsCnas(c.Centre)).ToList();
            var casnosList = dashboard.RepartitionParCaisse.Where(c => IsCasnos(c.Centre)).ToList();

            var cnasMontant = cnasList.Sum(c => c.MontantTotal);
            var cnasPct = dashboard.MontantTotalFact > 0 ? (cnasMontant / dashboard.MontantTotalFact) * 100m : 0m;
            lblCnasVal.Text = $"{cnasMontant:N0} DA ({cnasPct:N1}%)";

            var casnosMontant = casnosList.Sum(c => c.MontantTotal);
            var casnosPct = dashboard.MontantTotalFact > 0 ? (casnosMontant / dashboard.MontantTotalFact) * 100m : 0m;
            lblCasnosVal.Text = $"{casnosMontant:N0} DA ({casnosPct:N1}%)";

            var totalCaisse = dashboard.RepartitionParCaisse.Sum(c => c.MontantCaisse);
            var totalAssure = dashboard.RepartitionParCaisse.Sum(c => c.MontantAssure);
            var totalGeneral = totalCaisse + totalAssure;
            var tauxCouverture = totalGeneral > 0 ? (totalCaisse / totalGeneral) * 100m : 0m;
            lblTauxPriseEnChargeVal.Text = $"{tauxCouverture:N1} %";

            // 2. Bind Horizontal Charts
            chartHourly.DataSource = dashboard.ActiviteHoraire;
            chartTopProducts.DataSource = dashboard.TopMedicaments;
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

    private async void BtnRefresh_ItemClick(object sender, ItemClickEventArgs e)
    {
        await ReloadDataAsync();
    }

    private void BtnClearDates_ItemClick(object sender, ItemClickEventArgs e)
    {
        SetDates(Period.MinDate, Period.MaxDate);
    }

    private void ScopeDashboardUc_Disposed(object? sender, EventArgs e)
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadLock.Dispose();
    }
}

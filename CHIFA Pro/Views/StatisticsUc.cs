using CHIFA.DAL.Statistics;

using DevExpress.XtraCharts;
using DevExpress.XtraTab;

namespace CHIFA.Pro.Others;
public partial class StatisticsUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "STATISTICS";
    public Image Image => frmMain.Image(4);

    public Func<Period> Period = () => new();
    private IEnumerable<FactureByMonth>? FacturesDaily;
    private IEnumerable<FactureByMonth>? FacturesMonthly;
    private IEnumerable<FactureByMonth>? facturesWeekly;
    private IEnumerable<Gp>? PrincepsVsGeneric;

    public StatisticsUc()
    {
        InitializeComponent();
    }

    private async void BtnBordereaux_Click(object sender, EventArgs e)
    {
        try
        {
            var serie = new Series("Amount of Bordereaux Monthly", ViewType.Bar)
            {
                ArgumentDataMember = nameof(BordMonthlyStatDto.Date),
                ValueDataMembersSerializable = nameof(BordMonthlyStatDto.Montant),
                DataSource = await Task.Run(() => StatisticsService.BordereauxMonthlyAsync(Period?.Invoke())).ConfigureAwait(true)
            };
            ClearCharts();
            chrtCntrl.Titles.Add(new ChartTitle { Text = "Amount of Bordereaux Monthly" });
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void BtnClearDates_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            FromDate.EditValue = toDateRepo.MinValue;
            ToDate.EditValue = toDateRepo.MaxValue;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnFacturesDaily_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();

            chrtCntrl.Titles.Add(new ChartTitle { Text = "Number of Factures Daily" });

            var serie = new Series("Number Factures Daily", ViewType.Line)
            {
                ArgumentDataMember = "Date",
                ValueDataMembersSerializable = "Count",
                DataSource = FacturesDaily ??= await Task.Run(() => StatisticsService.FacturesDailyAsync(Period?.Invoke())).ConfigureAwait(true)
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnFacturesMonthly_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();

            chrtCntrl.Titles.Add(new ChartTitle { Text = "Number of Factures Monthly" });

            var serie = new Series
            {
                ArgumentDataMember = "Date",
                ValueDataMembersSerializable = "Count",
                DataSource = FacturesMonthly ??= await Task.Run(() => StatisticsService.FacturesMonthlyAsync(Period?.Invoke())).ConfigureAwait(true)
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnFacturesWeekly_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();

            chrtCntrl.Titles.Add(new ChartTitle { Text = "Number of Factures Weekly" });
            var serie = new Series
            {
                ArgumentDataMember = "Date",
                ValueDataMembersSerializable = "Count",
                DataSource = facturesWeekly ??= await Task.Run(() => StatisticsService.FacturesWeeklyAsync(Period?.Invoke())).ConfigureAwait(true)
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnMontantDaily_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();

            chrtCntrl.Titles.Add(new ChartTitle { Text = "Amount of Factures Daily" });

            var serie = new Series("Montant Factures Daily", ViewType.Line)
            {
                ArgumentDataMember = "Date",
                ValueDataMembersSerializable = "Montant",
                DataSource = FacturesDaily ??= await Task.Run(() => StatisticsService.FacturesDailyAsync(Period?.Invoke())).ConfigureAwait(true)
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnMontantMonthly_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();
            FacturesMonthly ??= await StatisticsService.FacturesMonthlyAsync(Period?.Invoke());
            chrtCntrl.Titles.Add(new ChartTitle { Text = "Amount of Factures Monthly" });
            var serie = new Series
            {
                ArgumentDataMember = "Date",
                ValueDataMembersSerializable = "Montant",
                DataSource = FacturesMonthly
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnMontantWeekly_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();
            facturesWeekly ??= await StatisticsService.FacturesWeeklyAsync(Period?.Invoke());
            chrtCntrl.Titles.Add(new ChartTitle { Text = "Montant Per Day" });

            var serie = new Series
            {
                ArgumentDataMember = "Date",
                ValueDataMembersSerializable = "Montant",
                DataSource = facturesWeekly
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnPrincepceVsGeneric_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();
            PrincepsVsGeneric ??= await StatisticsService.PrincepsVsGenericAsync(Period?.Invoke());
            chrtCntrl.Titles.Add(new ChartTitle { Text = "Princeps Vs Générique" });
            var serie = new Series("Princeps Vs Générique", ViewType.Pie)
            {
                ArgumentDataMember = "Class",
                ValueDataMembersSerializable = "Montant",
                LegendTextPattern = "{A}",
                DataSource = PrincepsVsGeneric
            };
            serie.Label.TextPattern = "{VP:p0} ({V:d} Dz)";
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnProducts_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();
            chrtCntrl.Titles.Add(new ChartTitle { Text = "Amount of Products Monthly" });
            IEnumerable<ProductsDaily> data = await StatisticsService.ProductsMonthlyAsync(Period?.Invoke());

            var serie1 = new Series("Products Monthly", ViewType.Bar)
            {
                LegendName = "Princeps",
                ArgumentDataMember = "Date",
                ValueDataMembersSerializable = "PrincepsMontant",
                DataSource = data,
                LegendText = "Princeps"
            };
            var serie2 = new Series
            {
                LegendName = "Generic",
                ArgumentDataMember = "Date",
                ValueDataMembersSerializable = "GenericMontant",
                DataSource = data,
                LegendText = "Générique"
            };
            chrtCntrl.Series.Add(serie1);
            chrtCntrl.Series.Add(serie2);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await ReloadSelectedTable(tabControl.SelectedTabPage);
    }

    private async void BtnTop10Montant_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();
            chrtCntrl.Titles.Add(new ChartTitle { Text = "Top 10 Products By Quantity" });

            var serie = new Series("Top 10 Products By Montant", ViewType.Line)
            {
                ArgumentDataMember = nameof(TopSeal.Produit),
                ValueDataMembersSerializable = nameof(TopSeal.Prix),
                DataSource = await Task.Run(() => StatisticsService.Top10ProductsByMontantAsync(Period?.Invoke())).ConfigureAwait(true)
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void BtnTOp10Quantity_Click(object sender, EventArgs e)
    {
        try
        {
            ClearCharts();

            chrtCntrl.Titles.Add(new ChartTitle { Text = "Top 10 Products By Quantity" });

            var serie = new Series("Top 10 Products By Quantity", ViewType.Line)
            {
                ArgumentDataMember = nameof(TopSeal.Produit),
                ValueDataMembersSerializable = nameof(TopSeal.Qt),
                DataSource = await Task.Run(() => StatisticsService.Top10ProuctsByQuantityAsync(Period?.Invoke())).ConfigureAwait(true)
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void ClearCharts()
    {
        chrtCntrl?.Series?.Clear();
        chrtCntrl?.Titles?.Clear();
    }

    private async void FromDate_EditValueChanged(object sender, EventArgs e)
    {
        await ReloadSelectedTable(tabControl.SelectedTabPage);
    }

    private Task LoadDataMonthly()
    {
        return gridViewMonthly.LoadDataAsync(() => StatisticsService.BordereauxMonthlyAsync(Period?.Invoke()));
    }

    private Task LoadFacturesByClient()
    {
        return gridViewByClient.LoadDataAsync(() => StatisticsService.FacturesByClientAsync(Period?.Invoke()));
    }

    private async Task LoadMaxAndMinDates()
    {
        try
        {
            (DateTime min, DateTime max) = await DataService.GetMinAndMaxDatesAsync();

            fromDateRepo.MaxValue = max;
            fromDateRepo.MinValue = min;
            fromDateRepo.TodayDate = min;

            toDateRepo.MaxValue = max;
            toDateRepo.MinValue = min;
            toDateRepo.TodayDate = max;

            FromDate.EditValue = toDateRepo.MinValue;
            ToDate.EditValue = toDateRepo.MaxValue;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private Task LoadMovements()
    {
        return gridViewMovements.LoadDataAsync(() => StatisticsService.DetailedMovementsAsync(Period?.Invoke()));
    }

    private Task LoadStatistics()
    {
        return gridViewStatistics.LoadDataAsync(() => StatisticsService.GlobalMovementsAsync(Period?.Invoke()));
    }

    private Task LoadTop10Montant()
    {
        return gridViewStatistics.LoadDataAsync(() => StatisticsService.Top10ProductsByMontantAsync(Period?.Invoke()));
    }

    private Task LoadTop10Quantity()
    {
        return gridViewStatistics.LoadDataAsync(() => StatisticsService.Top10ProuctsByQuantityAsync(Period?.Invoke()));
    }

    private async void MovementsUc_Load(object sender, EventArgs e)
    {
        gridViewMovements.SetOptions();
        gridViewStatistics.SetOptions();
        gridViewMonthly.SetOptions();
        gridViewByClient.SetOptions();
        await LoadMaxAndMinDates();
        BtnBordereaux_Click(btnBordereaux, null);
    }

    private async Task ReloadSelectedTable(XtraTabPage tab)
    {
        try
        {
            if (tab == tabMovments && mouvementDtoBindingSource.Count == 0)
            {
                await LoadMovements();
            }
            else if (tab == tabByClient && facturesByClientBindingSource.Count == 0)
            {
                await LoadFacturesByClient();
            }
            else if (tab == tabMonthly && monthlyStatisticsDtoBindingSource.Count == 0)
            {
                await LoadDataMonthly();
            }
            else if (tab == tabStatistics && statisticsBindingSource.Count == 0)
            {
                await LoadStatistics();
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void tabControl_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
    {
        await ReloadSelectedTable(e.Page);
    }

    private async void ToDate_EditValueChanged(object sender, EventArgs e)
    {
        await ReloadSelectedTable(tabControl.SelectedTabPage);
    }
}
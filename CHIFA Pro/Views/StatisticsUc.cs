using CHIFA.DAL.Statistics;

using DevExpress.XtraCharts;
using DevExpress.XtraTab;

namespace CHIFA.Pro.Others;
public partial class StatisticsUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "STATISTICS";
    public Image Image => frmMain.Image(4);

    private IEnumerable<Gp>? PrincepsVsGeneric;

    public StatisticsUc()
    {
        InitializeComponent();
    }

    private async void BtnClearDates_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await LoadMaxAndMinDates();
        await ReloadSelectedTable(tabControl.SelectedTabPage);
    }

    private async void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await ReloadSelectedTable(tabControl.SelectedTabPage);
    }

    private async void BtnTop10Montant_Click(object sender, EventArgs e)
    {
        try
        {
            chrtCntrl.Titles.Add(new ChartTitle { Text = "Top 10 Products By Quantity" });
            var serie = new Series("Top 10 Products By Montant", ViewType.Line)
            {
                ArgumentDataMember = nameof(TopSeal.Produit),
                ValueDataMembersSerializable = nameof(TopSeal.Prix),
                DataSource = await Task.Run(() => StatisticsService.Top10ProductsByMontantAsync()).ConfigureAwait(true)
            };
            chrtCntrl.Series.Add(serie);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }


    private async void FromDate_EditValueChanged(object sender, EventArgs e)
    {
        StatisticsService.period.From = (DateTime)FromDate.EditValue;
        await ReloadSelectedTable(tabControl.SelectedTabPage);
    }
    private async void ToDate_EditValueChanged(object sender, EventArgs e)
    {
        StatisticsService.period.To = (DateTime)ToDate.EditValue;
        await ReloadSelectedTable(tabControl.SelectedTabPage);
    }


    private async Task LoadMaxAndMinDates()
    {
        try
        {
            (DateTime min, DateTime max) = await DataService.GetMinAndMaxDatesAsync();

            var towYearsBefore = DateTime.Now.AddYears(-2);

            fromDateRepo.MaxValue = max;
            fromDateRepo.MinValue = min;
            fromDateRepo.TodayDate = min < towYearsBefore ? towYearsBefore : min;

            toDateRepo.MaxValue = max;
            toDateRepo.MinValue = min;
            toDateRepo.TodayDate = max;

            FromDate.EditValue = fromDateRepo.TodayDate;
            ToDate.EditValue = toDateRepo.TodayDate;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
    private async void MovementsUc_Load(object sender, EventArgs e)
    {
        viewBord.SetOptions();
        gridViewStatistics.SetOptions();
        viewMonthly.SetOptions();
        viewClients.SetOptions();
        await LoadMaxAndMinDates();
    }

    private async Task ReloadSelectedTable(XtraTabPage tab)
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            if (tab.Name is nameof(tabBordereaux) or nameof(tabBordereauxTable))
            {
                bordStatDtoBindingSource.DataSource = await StatisticsService.BordereauxAsync();
            }
            if (tab.Name is nameof(tabMonthly) or nameof(tabMonthlyTable))
            {
                monthlyStatBindingSource.DataSource = await StatisticsService.MonthlyAsync();
            }
            if (tab.Name is nameof(tabWeekly) or nameof(tabWeeklyTable))
            {
                weeklyStatBindingSource.DataSource = await StatisticsService.WeeklyAsync();
            }
            if (tab.Name is nameof(tabDaily) or nameof(tabDailyTable))
            {
                dailyStatBindingSource.DataSource = await StatisticsService.DailyAsync();
            }
            if (tab.Name is nameof(tabClients) or nameof(tabClientsTable))
            {
                byClientStatBindingSource.DataSource = await StatisticsService.ByClientAsync();
            }
            if (tab.Name is nameof(tabProducts) or nameof(tabProductTable))
            {
                productStatBindingSource.DataSource = await StatisticsService.ProductsAsync();
            }
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

    private async void tabControl_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
    {
        await ReloadSelectedTable(e.Page);
    }
}
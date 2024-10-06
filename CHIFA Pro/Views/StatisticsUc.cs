using DevExpress.XtraTab;

namespace CHIFA.Pro.Views;
public partial class StatisticsUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "STATISTICS";
    public Image Image => frmMain.Image(4);

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


    private async void FromDate_EditValueChanged(object sender, EventArgs e)
    {
        StatisticsService.Period.From = (DateTime)FromDate.EditValue;
        await ReloadSelectedTable(tabControl.SelectedTabPage);

    }
    private async void ToDate_EditValueChanged(object sender, EventArgs e)
    {
        StatisticsService.Period.To = (DateTime)ToDate.EditValue;
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
        viewYearly.SetOptions();
        viewMonthly.SetOptions();
        viewWeekly.SetOptions();
        viewDaily.SetOptions();
        viewClients.SetOptions();
        viewProducts.SetOptions();
        await LoadMaxAndMinDates();

        FromDate.EditValueChanged += FromDate_EditValueChanged!;
        ToDate.EditValueChanged += ToDate_EditValueChanged!;

        await ReloadSelectedTable(tabBordereaux);
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
            if (tab.Name is nameof(tabProducts2) or nameof(tabProductTable))
            {
                productStatBindingSource.DataSource = await StatisticsService.ProductsAsync();
            }
            if (tab.Name is nameof(tabYearly) or nameof(tabYearlyTable))
            {
                yearlyStatBindingSource.DataSource = await StatisticsService.YearlyAsync();
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

    private void btnAllPeriod_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        SetDates(fromDateRepo.MinValue, toDateRepo.MaxValue);
    }

    private void btnLastYear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        SetDates(DateTime.Now.AddYears(-1), DateTime.Now);
    }

    private void btn6Months_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        SetDates(DateTime.Now.AddMonths(-6), DateTime.Now);
    }

    private void btnThisYear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        SetDates(new DateTime(DateTime.Now.Year, 1, 1), DateTime.Now);
    }

    private void btnThisMonth_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        SetDates(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);
    }

    private void SetDates(DateTime from, DateTime to)
    {
        StatisticsService.Period.From = from;
        StatisticsService.Period.To = to;
        FromDate.EditValue = StatisticsService.Period.From;
        ToDate.EditValue = StatisticsService.Period.To;
    }
}
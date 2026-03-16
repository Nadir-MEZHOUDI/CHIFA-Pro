using System.ComponentModel;
using DevExpress.XtraGrid;
using DevExpress.XtraTab;

namespace CHIFA.Pro.Views;

public partial class StatisticsUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private readonly SemaphoreSlim _reloadLock = new(1, 1);
    private CancellationTokenSource? _reloadCts;
    private bool _suspendDateEvents;

    public string Caption { get; } = "STATISTICS";
    public Image Image => FrmMain.Image(4);
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Parametre? Officine { get; set; }

    public StatisticsUc()
    {
        InitializeComponent();
        Disposed += StatisticsUc_Disposed;
    }

    private async void BtnClearDates_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await LoadMaxAndMinDates();
        await ReloadSelectedTableImmediateAsync(tabControl.SelectedTabPage);
    }

    private async void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await ReloadSelectedTableImmediateAsync(tabControl.SelectedTabPage);
    }

    private async void FromDate_EditValueChanged(object sender, EventArgs e)
    {
        if (_suspendDateEvents)
            return;

        UpdatePeriodFromEditors();
        await ScheduleSelectedTableReloadAsync(tabControl.SelectedTabPage);
    }

    private async void ToDate_EditValueChanged(object sender, EventArgs e)
    {
        if (_suspendDateEvents)
            return;

        UpdatePeriodFromEditors();
        await ScheduleSelectedTableReloadAsync(tabControl.SelectedTabPage);
    }

    private async Task LoadMaxAndMinDates()
    {
        try
        {
            await ChifaService.Instance.GetMinAndMaxDatesAsync();

            var towYearsBefore = DateTime.Now.AddYears(-2);

            fromDateRepo.MaxValue = Period.MaxDate;
            fromDateRepo.MinValue = Period.MinDate;
            fromDateRepo.TodayDate = Period.MinDate < towYearsBefore ? towYearsBefore : Period.MinDate;

            toDateRepo.MaxValue = Period.MaxDate;
            toDateRepo.MinValue = Period.MinDate;
            toDateRepo.TodayDate = Period.MaxDate;

            _suspendDateEvents = true;
            FromDate.EditValue = fromDateRepo.TodayDate;
            ToDate.EditValue = toDateRepo.TodayDate;
            UpdatePeriodFromEditors();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            _suspendDateEvents = false;
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

        await ReloadSelectedTableImmediateAsync(tabBordereaux);
        await GetOfficineAsync();
    }

    private async Task GetOfficineAsync()
    {
        try
        {
            Officine = await ChifaService.Instance.GetFirstOfficineAsync();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async Task ScheduleSelectedTableReloadAsync(XtraTabPage? tab)
    {
        var cancellationToken = ResetReloadToken();

        try
        {
            await Task.Delay(ReloadDebounceDelay, cancellationToken);
            await ReloadSelectedTableAsync(tab, cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task ReloadSelectedTableImmediateAsync(XtraTabPage? tab)
    {
        var cancellationToken = ResetReloadToken();
        await ReloadSelectedTableAsync(tab, cancellationToken);
    }

    private CancellationToken ResetReloadToken()
    {
        var cts = new CancellationTokenSource();
        var previous = Interlocked.Exchange(ref _reloadCts, cts);
        previous?.Cancel();
        previous?.Dispose();
        return cts.Token;
    }

    private void UpdatePeriodFromEditors()
    {
        if (FromDate.EditValue is DateTime from)
            StatisticsService.Instance.Period.From = from;

        if (ToDate.EditValue is DateTime to)
            StatisticsService.Instance.Period.To = to;
    }

    private async Task ReloadSelectedTableAsync(XtraTabPage? tab, CancellationToken cancellationToken)
    {
        if (tab is null || cancellationToken.IsCancellationRequested)
            return;

        var lockTaken = false;

        try
        {
            await _reloadLock.WaitAsync(cancellationToken);
            lockTaken = true;

            if (cancellationToken.IsCancellationRequested)
                return;

            Cursor = Cursors.WaitCursor;
            if (tab.Name is nameof(tabBordereaux) or nameof(tabBordereauxTable))
            {
                var data = await StatisticsService.Instance.BordereauxAsync();
                if (cancellationToken.IsCancellationRequested)
                    return;

                bordStatDtoBindingSource.DataSource = data;
            }
            if (tab.Name is nameof(tabMonthly) or nameof(tabMonthlyTable))
            {
                var data = await StatisticsService.Instance.MonthlyAsync();
                if (cancellationToken.IsCancellationRequested)
                    return;

                monthlyStatBindingSource.DataSource = data;
            }
            if (tab.Name is nameof(tabWeekly) or nameof(tabWeeklyTable))
            {
                var data = await StatisticsService.Instance.WeeklyAsync();
                if (cancellationToken.IsCancellationRequested)
                    return;

                weeklyStatBindingSource.DataSource = data;
            }
            if (tab.Name is nameof(tabDaily) or nameof(tabDailyTable))
            {
                var data = await StatisticsService.Instance.DailyAsync();
                if (cancellationToken.IsCancellationRequested)
                    return;

                dailyStatBindingSource.DataSource = data;
            }
            if (tab.Name is nameof(tabClients) or nameof(tabClientsTable))
            {
                var data = await StatisticsService.Instance.ByClientAsync();
                if (cancellationToken.IsCancellationRequested)
                    return;

                byClientStatBindingSource.DataSource = data;
            }
            if (tab.Name is nameof(tabProducts2) or nameof(tabProductTable))
            {
                var data = await StatisticsService.Instance.ProductsAsync();
                if (cancellationToken.IsCancellationRequested)
                    return;

                productStatBindingSource.DataSource = data;
            }
            if (tab.Name is nameof(tabYearly) or nameof(tabYearlyTable))
            {
                var data = await StatisticsService.Instance.YearlyAsync();
                if (cancellationToken.IsCancellationRequested)
                    return;

                yearlyStatBindingSource.DataSource = data;
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            Cursor = Cursors.Default;
            if (lockTaken)
                _reloadLock.Release();
        }
    }

    private async void tabControl_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
    {
        try
        {
            await ScheduleSelectedTableReloadAsync(e.Page);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
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
        try
        {
            _suspendDateEvents = true;
            StatisticsService.Instance.Period.From = from;
            StatisticsService.Instance.Period.To = to;
            FromDate.EditValue = StatisticsService.Instance.Period.From;
            ToDate.EditValue = StatisticsService.Instance.Period.To;
        }
        finally
        {
            _suspendDateEvents = false;
        }

        _ = ReloadSelectedTableImmediateAsync(tabControl.SelectedTabPage);
    }

    private void btnExportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            ExportGrid("xlsx", "Excel files (*.xlsx)|*.xlsx", "Save Excel File", (grid, fileName) => grid.ExportToXlsx(fileName));
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void btnPdf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            ExportGrid("pdf", "PDF files (*.pdf)|*.pdf", "Save PDF File", (grid, fileName) => grid.ExportToPdf(fileName));
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void ExportGrid(string extension, string filter, string dialogTitle, Action<GridControl, string> exportAction)
    {
        var baseFileName = "Ph_" + Officine?.NomPharmacie + "_" +
                          StatisticsService.Instance.Period.From?.ToString("dd-MM-yyyy") + "_" +
                          StatisticsService.Instance.Period.To?.ToString("dd-MM-yyyy") + "." + extension;
        GridControl? grid = null;

        var tab = tabControl.SelectedTabPage;
        string prefix = "";

        if (tab.Name is nameof(tabBordereaux) or nameof(tabBordereauxTable))
        {
            prefix = "Bordereaux";
            grid = gridBordereaux;
        }
        else if (tab.Name is nameof(tabYearly) or nameof(tabYearlyTable))
        {
            prefix = "Yearly";
            grid = gridYearly;
        }
        else if (tab.Name is nameof(tabMonthly) or nameof(tabMonthlyTable))
        {
            prefix = "Monthly";
            grid = gridMonthly;
        }
        else if (tab.Name is nameof(tabWeekly) or nameof(tabWeeklyTable))
        {
            prefix = "Weekly";
            grid = gridWeekly;
        }
        else if (tab.Name is nameof(tabDaily) or nameof(tabDailyTable))
        {
            prefix = "Daily";
            grid = gridDaily;
        }
        else if (tab.Name is nameof(tabClients) or nameof(tabClientsTable))
        {
            prefix = "Clients";
            grid = gridClients;
        }
        else if (tab.Name is nameof(tabProducts2) or nameof(tabProductTable))
        {
            prefix = "Products";
            grid = gridProducts;
        }

        if (grid == null)
            return;

        var fileName = string.IsNullOrEmpty(prefix) ? baseFileName : $"{prefix}_{baseFileName}";

        var saveFileDialog = new SaveFileDialog
        {
            Filter = filter,
            FileName = fileName,
            Title = dialogTitle,
            OverwritePrompt = true,
            AddExtension = true,
            DefaultExt = extension
        };

        if (saveFileDialog.ShowDialog() != DialogResult.OK)
            return;

        fileName = saveFileDialog.FileName;
        exportAction?.Invoke(grid, fileName);
    }

    private void StatisticsUc_Disposed(object? sender, EventArgs e)
    {
        var previous = Interlocked.Exchange(ref _reloadCts, null);
        previous?.Cancel();
        previous?.Dispose();
        _reloadLock.Dispose();
    }
}

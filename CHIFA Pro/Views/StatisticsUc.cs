using System.ComponentModel;
using DevExpress.XtraGrid;
using DevExpress.XtraTab;

namespace CHIFA.Pro.Views;

public partial class StatisticsUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private readonly HashSet<StatisticsSection> _loadedSections = [];
    private readonly SemaphoreSlim _reloadLock = new(1, 1);
    private CancellationTokenSource? _reloadCts;
    private StatisticsSection _selectedSection = StatisticsSection.Bordereaux;
    private StatisticsView _selectedView = StatisticsView.Table;
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

    public void SelectSection(StatisticsSection section)
    {
        _selectedSection = section;
        UpdateSectionCaption();
        SelectCurrentPage();
    }

    private void UpdateSectionCaption()
    {
        if (Parent is XtraTabPage page)
            page.Text = $"{Caption} — {GetSectionDisplayName(_selectedSection)}";
    }

    private static string GetSectionDisplayName(StatisticsSection section)
    {
        return section switch
        {
            StatisticsSection.Bordereaux => "BORDEREAUX",
            StatisticsSection.Yearly => "ANNUELLES",
            StatisticsSection.Monthly => "MENSUELLES",
            StatisticsSection.Weekly => "HEBDOMADAIRES",
            StatisticsSection.Daily => "QUOTIDIENNES",
            StatisticsSection.Products => "PRODUITS",
            StatisticsSection.Clients => "CLIENTS",
            _ => "BORDEREAUX"
        };
    }

    private void SelectView(StatisticsView view)
    {
        _selectedView = view;
        btnTable.Checked = view == StatisticsView.Table;
        btnChart.Checked = view == StatisticsView.Chart;
        SelectCurrentPage();
    }

    private void SelectCurrentPage()
    {
        tabControl.SelectedTabPage = (_selectedSection, _selectedView) switch
        {
            (StatisticsSection.Bordereaux, StatisticsView.Table) => tabBordereauxTable,
            (StatisticsSection.Bordereaux, StatisticsView.Chart) => tabBordereaux,
            (StatisticsSection.Yearly, StatisticsView.Table) => tabYearlyTable,
            (StatisticsSection.Yearly, StatisticsView.Chart) => tabYearly,
            (StatisticsSection.Monthly, StatisticsView.Table) => tabMonthlyTable,
            (StatisticsSection.Monthly, StatisticsView.Chart) => tabMonthly,
            (StatisticsSection.Weekly, StatisticsView.Table) => tabWeeklyTable,
            (StatisticsSection.Weekly, StatisticsView.Chart) => tabWeekly,
            (StatisticsSection.Daily, StatisticsView.Table) => tabDailyTable,
            (StatisticsSection.Daily, StatisticsView.Chart) => tabDaily,
            (StatisticsSection.Products, StatisticsView.Table) => tabProductTable,
            (StatisticsSection.Products, StatisticsView.Chart) => tabProducts2,
            (StatisticsSection.Clients, StatisticsView.Table) => tabClientsTable,
            (StatisticsSection.Clients, StatisticsView.Chart) => tabClients,
            _ => tabBordereauxTable
        };
    }

    private void btnTable_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        SelectView(StatisticsView.Table);
    }

    private void btnChart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        SelectView(StatisticsView.Chart);
    }

    private async void BtnClearDates_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await LoadMaxAndMinDates();
        InvalidateStatisticsData();
        await ReloadSelectedTableImmediateAsync(tabControl.SelectedTabPage, true);
    }

    private async void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await ReloadSelectedTableImmediateAsync(tabControl.SelectedTabPage, true);
    }

    private async void FromDate_EditValueChanged(object sender, EventArgs e)
    {
        if (_suspendDateEvents)
            return;

        UpdatePeriodFromEditors();
        InvalidateStatisticsData();
        await ScheduleSelectedTableReloadAsync(tabControl.SelectedTabPage);
    }

    private async void ToDate_EditValueChanged(object sender, EventArgs e)
    {
        if (_suspendDateEvents)
            return;

        UpdatePeriodFromEditors();
        InvalidateStatisticsData();
        await ScheduleSelectedTableReloadAsync(tabControl.SelectedTabPage);
    }

    private async Task LoadMaxAndMinDates(bool defaultToThisYear = false)
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
            FromDate.EditValue = defaultToThisYear
                ? new DateTime(DateTime.Now.Year, 1, 1)
                : fromDateRepo.TodayDate;
            ToDate.EditValue = defaultToThisYear ? DateTime.Now : toDateRepo.TodayDate;
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

        await LoadMaxAndMinDates(defaultToThisYear: true);

        FromDate.EditValueChanged += FromDate_EditValueChanged!;

        ToDate.EditValueChanged += ToDate_EditValueChanged!;

        await ReloadSelectedTableImmediateAsync(tabControl.SelectedTabPage);
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

    private async Task ScheduleSelectedTableReloadAsync(XtraTabPage? tab, bool forceReload = false)
    {
        var cancellationToken = ResetReloadToken();

        try
        {
            await Task.Delay(ReloadDebounceDelay, cancellationToken);
            await ReloadSelectedTableAsync(tab, cancellationToken, forceReload);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task ReloadSelectedTableImmediateAsync(XtraTabPage? tab, bool forceReload = false)
    {
        var cancellationToken = ResetReloadToken();
        await ReloadSelectedTableAsync(tab, cancellationToken, forceReload);
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

    private void InvalidateStatisticsData()
    {
        _loadedSections.Clear();
    }

    private async Task ReloadSelectedTableAsync(XtraTabPage? tab, CancellationToken cancellationToken, bool forceReload)
    {
        if (tab is null || cancellationToken.IsCancellationRequested)
            return;

        var section = GetSection(tab);
        if (section is null || !forceReload && _loadedSections.Contains(section.Value))
            return;

        var lockTaken = false;

        try
        {
            await _reloadLock.WaitAsync(cancellationToken);
            lockTaken = true;

            if (cancellationToken.IsCancellationRequested)
                return;

            if (!forceReload && _loadedSections.Contains(section.Value))
                return;

            SetLoadingState(true);
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

            if (!cancellationToken.IsCancellationRequested)
                _loadedSections.Add(section.Value);
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
            SetLoadingState(false);
            if (lockTaken)
                _reloadLock.Release();
        }
    }

    private static StatisticsSection? GetSection(XtraTabPage tab)
    {
        return tab.Name switch
        {
            nameof(tabBordereaux) or nameof(tabBordereauxTable) => StatisticsSection.Bordereaux,
            nameof(tabYearly) or nameof(tabYearlyTable) => StatisticsSection.Yearly,
            nameof(tabMonthly) or nameof(tabMonthlyTable) => StatisticsSection.Monthly,
            nameof(tabWeekly) or nameof(tabWeeklyTable) => StatisticsSection.Weekly,
            nameof(tabDaily) or nameof(tabDailyTable) => StatisticsSection.Daily,
            nameof(tabProducts2) or nameof(tabProductTable) => StatisticsSection.Products,
            nameof(tabClients) or nameof(tabClientsTable) => StatisticsSection.Clients,
            _ => null
        };
    }

    private void SetLoadingState(bool isLoading)
    {
        loadingIndicator.Visible = isLoading;
        btnRefresh.Enabled = !isLoading;
        Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
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

        InvalidateStatisticsData();
        _ = ReloadSelectedTableImmediateAsync(tabControl.SelectedTabPage, true);
    }

    private void btnExportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        try
        {
            var grid = GetSelectedGrid();
            var fileName = ShowExportDialog("xlsx", "Excel files (*.xlsx)|*.xlsx", "Save Excel File", "Data");
            if (fileName is not null)
                grid.ExportToXlsx(fileName);
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
            var viewName = _selectedView == StatisticsView.Chart ? "Chart" : "Table";
            var fileName = ShowExportDialog("pdf", "PDF files (*.pdf)|*.pdf", "Save PDF File", viewName);
            if (fileName is null)
                return;

            if (_selectedView == StatisticsView.Chart)
                GetSelectedChart().ExportToPdf(fileName);
            else
                GetSelectedGrid().ExportToPdf(fileName);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private GridControl GetSelectedGrid()
    {
        return _selectedSection switch
        {
            StatisticsSection.Bordereaux => gridBordereaux,
            StatisticsSection.Yearly => gridYearly,
            StatisticsSection.Monthly => gridMonthly,
            StatisticsSection.Weekly => gridWeekly,
            StatisticsSection.Daily => gridDaily,
            StatisticsSection.Products => gridProducts,
            StatisticsSection.Clients => gridClients,
            _ => gridBordereaux
        };
    }

    private DevExpress.XtraCharts.ChartControl GetSelectedChart()
    {
        return _selectedSection switch
        {
            StatisticsSection.Bordereaux => chartBordereaux,
            StatisticsSection.Yearly => chartYearly,
            StatisticsSection.Monthly => chartMonthly,
            StatisticsSection.Weekly => chartWeekly,
            StatisticsSection.Daily => chartDaily,
            StatisticsSection.Products => chartProducts,
            StatisticsSection.Clients => chartClients,
            _ => chartBordereaux
        };
    }

    private string? ShowExportDialog(string extension, string filter, string dialogTitle, string viewName)
    {
        var section = GetSectionDisplayName(_selectedSection);
        var pharmacy = Officine?.NomPharmacie ?? "Officine";
        var from = StatisticsService.Instance.Period.From?.ToString("dd-MM-yyyy") ?? "Min";
        var to = StatisticsService.Instance.Period.To?.ToString("dd-MM-yyyy") ?? "Max";
        var fileName = SanitizeFileName($"{section}_{viewName}_Ph_{pharmacy}_{from}_{to}.{extension}");

        using var saveFileDialog = new SaveFileDialog
        {
            Filter = filter,
            FileName = fileName,
            Title = dialogTitle,
            OverwritePrompt = true,
            AddExtension = true,
            DefaultExt = extension
        };

        if (saveFileDialog.ShowDialog() != DialogResult.OK)
            return null;

        return saveFileDialog.FileName;
    }

    private static string SanitizeFileName(string fileName)
    {
        var invalidCharacters = Path.GetInvalidFileNameChars();
        return new string(fileName.Select(character => invalidCharacters.Contains(character) ? '_' : character).ToArray());
    }

    private void StatisticsUc_Disposed(object? sender, EventArgs e)
    {
        var previous = Interlocked.Exchange(ref _reloadCts, null);
        previous?.Cancel();
        previous?.Dispose();
        _reloadLock.Dispose();
    }
}

public enum StatisticsSection
{
    Bordereaux,
    Yearly,
    Monthly,
    Weekly,
    Daily,
    Products,
    Clients
}

public enum StatisticsView
{
    Table,
    Chart
}

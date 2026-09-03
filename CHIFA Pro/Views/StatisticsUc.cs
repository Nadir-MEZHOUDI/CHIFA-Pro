using System.ComponentModel;
using DevExpress.XtraBars;
using DevExpress.XtraCharts;
using DevExpress.XtraGrid;
using DevExpress.XtraTab;

namespace CHIFA.Pro.Views;

public partial class StatisticsUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private readonly HashSet<StatisticsSection> _loadedSections = [];
    private CancellationTokenSource? _reloadCts;
    private StatisticsSection _selectedSection = StatisticsSection.Bordereaux;
    private StatisticsView _selectedView = StatisticsView.Table;
    private bool _suspendDateEvents;
    private bool _disposed;

    public string Caption { get; } = "STATISTIQUES";
    public Image Image => FrmMain.Image(4);
    
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Parametre? Officine { get; set; }

    public StatisticsUc()
    {
        InitializeComponent();
        SetupDashboardCharts();
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
            StatisticsSection.Dashboard => "TABLEAU DE BORD",
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
            (StatisticsSection.Dashboard, _) => tabDashboard,
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
        try
        {
            viewBord.SetOptions();
            viewYearly.SetOptions();
            viewMonthly.SetOptions();
            viewWeekly.SetOptions();
            viewDaily.SetOptions();
            viewClients.SetOptions();
            viewProducts.SetOptions();

            await LoadMaxAndMinDates(defaultToThisYear: true);

            if (_disposed || IsDisposed) return;

            FromDate.EditValueChanged += FromDate_EditValueChanged!;

            ToDate.EditValueChanged += ToDate_EditValueChanged!;

            await ReloadSelectedTableImmediateAsync(tabControl.SelectedTabPage);
            await GetOfficineAsync();
        }
        catch (ObjectDisposedException) { }
        catch (Exception ex) { ex.Log(); }
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
        if (_disposed || IsDisposed)
            return;

        var cancellationToken = ResetReloadToken();
        if (cancellationToken.IsCancellationRequested)
            return;

        try
        {
            await Task.Delay(ReloadDebounceDelay, cancellationToken);
            await ReloadSelectedTableAsync(tab, cancellationToken, forceReload);
        }
        catch (OperationCanceledException)
        {
        }
        catch (ObjectDisposedException)
        {
        }
    }

    private async Task ReloadSelectedTableImmediateAsync(XtraTabPage? tab, bool forceReload = false)
    {
        if (_disposed || IsDisposed)
            return;

        var cancellationToken = ResetReloadToken();
        if (cancellationToken.IsCancellationRequested)
            return;

        await ReloadSelectedTableAsync(tab, cancellationToken, forceReload);
    }

    private CancellationToken ResetReloadToken()
    {
        if (_disposed || IsDisposed)
            return new CancellationToken(true);

        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = new CancellationTokenSource();
        return _reloadCts.Token;
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
        if (_disposed || IsDisposed || tab is null || cancellationToken.IsCancellationRequested)
            return;

        var section = GetSection(tab);
        if (section is null || !forceReload && _loadedSections.Contains(section.Value))
            return;

        try
        {
            if (_disposed || IsDisposed || cancellationToken.IsCancellationRequested)
                return;

            if (!forceReload && _loadedSections.Contains(section.Value))
                return;

            SetLoadingState(true);
            if (tab.Name is nameof(tabDashboard))
            {
                await ReloadDashboardAsync(cancellationToken);
                if (cancellationToken.IsCancellationRequested)
                    return;
            }
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
        catch (ObjectDisposedException)
        {
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            try { SetLoadingState(false); } catch (ObjectDisposedException) { }
        }
    }

    private static StatisticsSection? GetSection(XtraTabPage tab)
    {
        return tab.Name switch
        {
            nameof(tabDashboard) => StatisticsSection.Dashboard,
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

    #region Dashboard (Tour de Contrôle)

    private void SetupDashboardCharts()
    {
        // Hourly chart
        chartHourly.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        chartHourly.Titles.Clear();
        chartHourly.Titles.Add(new ChartTitle
        {
            Text = "Distribution Horaire & Heures de Pointe (Nombre de Factures)",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            TextColor = Color.DarkSlateBlue
        });

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

        // Top products chart
        chartTopProducts.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        chartTopProducts.Titles.Clear();
        chartTopProducts.Titles.Add(new ChartTitle
        {
            Text = "Top 10 Médicaments Dispensés (Quantité en Boîtes)",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            TextColor = Color.DarkSlateBlue
        });

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

    private async Task ReloadDashboardAsync(CancellationToken cancellationToken)
    {
        var dashboard = await StatisticsService.Instance.GetScopeDashboardAsync(StatisticsService.Instance.Period);

        if (cancellationToken.IsCancellationRequested) return;

        // KPIs
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

        chartHourly.DataSource = dashboard.ActiviteHoraire;
        chartTopProducts.DataSource = dashboard.TopMedicaments;
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

    #endregion

    private void SetLoadingState(bool isLoading)
    {
        if (_disposed || IsDisposed)
            return;

        try
        {
            if (!loadingIndicator.IsDisposed)
                loadingIndicator.Visible = isLoading;
            btnRefresh.Enabled = !isLoading;
            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }
        catch (ObjectDisposedException) { }
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
            var fileName = ShowExportDialog("xlsx", "Fichiers Excel (*.xlsx)|*.xlsx", "Enregistrer le fichier Excel", "Données");
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
            var viewName = _selectedView == StatisticsView.Chart ? "Graphique" : "Tableau";
            var fileName = ShowExportDialog("pdf", "Fichiers PDF (*.pdf)|*.pdf", "Enregistrer le fichier PDF", viewName);
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
        if (_disposed)
            return;
        _disposed = true;

        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = null;
    }
}

public enum StatisticsSection
{
    Dashboard,
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

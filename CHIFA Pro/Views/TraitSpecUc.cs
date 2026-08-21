namespace CHIFA.Pro.Views;

public partial class TraitSpecUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private readonly SemaphoreSlim _reloadLock = new(1, 1);
    private CancellationTokenSource? _reloadCts;

    public string Caption { get; } = "TRAITEMENT SPÉCIFIQUE";
    public Image Image => FrmMain.Image(2);

    public TraitSpecUc()
    {
        InitializeComponent();
        viewFectures.SetOptions();
        Disposed += TraitSpecUc_Disposed;
    }

    private void btnConsumption_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        new FrmTraitSpec().Show();
    }

    private void btnHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        GetHistoryOfSelectedPatient();
    }

    private async void facturesUC_Load(object sender, EventArgs e)
    {
        await LoadMaxAndMinDates();
        await ReloadDataImmediateAsync();
    }

    private async Task LoadMaxAndMinDates()
    {
        try
        {
            await ChifaService.Instance.GetMinAndMaxDatesAsync();
            var lastYear = DateTime.Now.AddYears(-1);
            fromDateRepo.MaxValue = Period.MaxDate;
            fromDateRepo.MinValue = Period.MinDate;
            fromDateRepo.TodayDate = lastYear;

            toDateRepo.MaxValue = Period.MaxDate;
            toDateRepo.MinValue = Period.MinDate;
            toDateRepo.TodayDate = Period.MaxDate;

            FromDate.EditValue = lastYear;
            ToDate.EditValue = toDateRepo.MaxValue;

            FromDate.EditValueChanged += FromDate_EditValueChanged;
            ToDate.EditValueChanged += ToDate_EditValueChanged;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void GetHistoryOfSelectedPatient()
    {
        if (viewFectures.GetRow(viewFectures.FocusedRowHandle) is PatientOfTraitSpec row)
            new FrmHistory(row.NumAssure!, row.Rang!).Show();
    }

    private async Task GetSelectedDetails()
    {
        if (viewFectures.GetRow(viewFectures.FocusedRowHandle) is PatientOfTraitSpec row)
            await LoadDetailsAsync(row);
    }

    private void gridView1_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewFectures?.GetRow(e.RowHandle) is PatientOfTraitSpec fc)
            e.Appearance.BackColor = (fc.TS, fc.LongDuree) switch
            {
                (true, _) => Colors.Green,
                (_, true) => Colors.Tan,
                (_, _) => e.Appearance.BackColor
            };
    }

    private async void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
        await GetSelectedDetails();
    }

    private void gridView2_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewDetails?.GetRow(e.RowHandle) is TraitDetailsDto fc)
            e.Appearance.BackColor = (fc.TS, fc.Duree >= 60, DateTime.Now > fc?.Prochain) switch
            {
                (_, _, true) => Colors.Red,
                (true, _, _) => Colors.Green,
                (_, true, _) => Colors.Tan,
                (_, _, _) => e.Appearance.BackColor
            };
    }

    private Task LoadDetailsAsync(PatientOfTraitSpec row)
    {
        var procheOnly = (bool)swtchTimeOnly.EditValue;
        return viewDetails.LoadDataAsync(() => ChifaService.Instance.GetPatientTraitementAsync(row.NumAssure!, row.Rang!, procheOnly));
    }

    private async Task ScheduleReloadDataAsync()
    {
        var cancellationToken = ResetReloadToken();

        try
        {
            await Task.Delay(ReloadDebounceDelay, cancellationToken);
            await LoadDataAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task ReloadDataImmediateAsync()
    {
        var cancellationToken = ResetReloadToken();
        await LoadDataAsync(cancellationToken);
    }

    private CancellationToken ResetReloadToken()
    {
        var cts = new CancellationTokenSource();
        var previous = Interlocked.Exchange(ref _reloadCts, cts);
        previous?.Cancel();
        previous?.Dispose();
        return cts.Token;
    }

    private async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        var lockTaken = false;

        try
        {
            await _reloadLock.WaitAsync(cancellationToken);
            lockTaken = true;

            if (cancellationToken.IsCancellationRequested)
                return;

            Expression<Func<DetailFact, bool>> predicate = f => true;

            if (txtMedic.EditValue is string txt)
                predicate = predicate.And(d => d.Medicament.FullName!.Contains(txt, StringComparison.InvariantCultureIgnoreCase));

            var period = new Period { From = (DateTime?)FromDate.EditValue, To = (DateTime?)ToDate.EditValue };

            await viewFectures.LoadDataAsync(() => ChifaService.Instance.GetPatientsOfTraitSpecAsync(period, predicate),
                cancellationToken);
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
            if (lockTaken)
                _reloadLock.Release();
        }
    }

    private async void swtchTimeOnly_EditValueChangedAsync(object sender, EventArgs e)
    {
        await ScheduleReloadDataAsync();
    }

    private async void txtMedic_EditValueChanged(object sender, EventArgs e)
    {
        await ScheduleReloadDataAsync();
    }

    private void viewFectures_DoubleClick(object sender, EventArgs e)
    {
        GetHistoryOfSelectedPatient();
    }



    private async void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await ReloadDataImmediateAsync();
    }

    private async void FromDate_EditValueChanged(object? sender, EventArgs e) => await ScheduleReloadDataAsync();

    private async void ToDate_EditValueChanged(object? sender, EventArgs e) => await ScheduleReloadDataAsync();

    private void TraitSpecUc_Disposed(object? sender, EventArgs e)
    {
        var previous = Interlocked.Exchange(ref _reloadCts, null);
        previous?.Cancel();
        previous?.Dispose();
        _reloadLock.Dispose();
    }
}

namespace CHIFA.Pro.Views;

public partial class TraitSpecUc : XtraUserControl, INavigable
{
    private readonly ReloadCancellation _reloads = new();

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
        await DateFilter.SetUpAsync(FromDate, ToDate, DateTime.Today.AddYears(-1), calendarOnFrom: true, _reloads, LoadDataAsync);
        await ReloadDataImmediateAsync();
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
        if (viewFectures?.GetRow(e.RowHandle) is PatientOfTraitSpec fc && RowColors.For(fc.TS, fc.LongDuree) is { } color)
            e.Appearance.BackColor = color;
    }

    private async void gridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
        await GetSelectedDetails();
    }

    private void gridView2_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewDetails?.GetRow(e.RowHandle) is TraitDetailsDto fc && RowColors.For(fc.TS, fc.Duree >= 60, DateTime.Now > fc.Prochain) is { } color)
            e.Appearance.BackColor = color;
    }

    private Task LoadDetailsAsync(PatientOfTraitSpec row)
    {
        var procheOnly = (bool)swtchTimeOnly.EditValue;
        var period = CurrentPeriod();
        return viewDetails.LoadDataAsync(ct => ChifaService.Instance.GetPatientTraitementAsync(row.NumAssure!, row.Rang!, procheOnly, period: period, cancellationToken: ct));
    }

    private Period CurrentPeriod() => DateFilter.ToPeriod(FromDate.EditValue, ToDate.EditValue);

    private Task ScheduleReloadDataAsync() => DateFilter.DebounceAsync(_reloads, LoadDataAsync);

    private async Task ReloadDataImmediateAsync()
    {
        var cancellationToken = _reloads.Next();
        await LoadDataAsync(cancellationToken);
    }

    private async Task LoadDataAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        try
        {
            Expression<Func<DetailFact, bool>> predicate = f => true;

            if (txtMedic.EditValue is string txt)
                predicate = predicate.And(d => d.Medicament.FullName!.Contains(txt, StringComparison.InvariantCultureIgnoreCase));

            var period = CurrentPeriod();

            await viewFectures.LoadDataAsync(ct => ChifaService.Instance.GetPatientsOfTraitSpecAsync(period, predicate, ct),
                cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            ex.Log();
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

    private void TraitSpecUc_Disposed(object? sender, EventArgs e)
    {
        _reloads.Stop();
    }
}

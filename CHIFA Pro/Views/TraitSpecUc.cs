namespace CHIFA.Pro.Views;

public partial class TraitSpecUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "TRAITEMENT SPECIFIQUE";
    public Image Image => FrmMain.Image(2);

    public TraitSpecUc()
    {
        InitializeComponent();
        viewFectures.SetOptions();
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
        await LoadData();
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

            FromDate.EditValueChanged += async (_, _) => await LoadData();
            ToDate.EditValueChanged += async (_, _) => await LoadData();
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

    private Task LoadData()
    {
        Expression<Func<DetailFact, bool>> predicate = f => true;

        if (txtMedic.EditValue is string txt)
            predicate = predicate.And(d => d.Medicament.FullName!.Contains(txt, StringComparison.InvariantCultureIgnoreCase));

        var period = new Period { From = (DateTime?)FromDate.EditValue, To = (DateTime?)ToDate.EditValue };

        return viewFectures.LoadDataAsync(() => ChifaService.Instance.GetPatientsOfTraitSpecAsync(period, predicate));
    }

    private async void swtchTimeOnly_EditValueChangedAsync(object sender, EventArgs e)
    {
        await LoadData();
    }

    private async void txtMedic_EditValueChanged(object sender, EventArgs e)
    {
        await LoadData();
    }

    private void viewFectures_DoubleClick(object sender, EventArgs e)
    {
        GetHistoryOfSelectedPatient();
    }



    private async void btnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        await LoadData();
    }
}
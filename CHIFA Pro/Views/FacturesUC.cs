
namespace CHIFA.Pro.uc;

public partial class FacturesUC : XtraUserControl, INavigable
{
    public FacturesUC()
    {
        InitializeComponent();
        viewFactures.SetOptions();
        txtDateFrom.EditValueChanged += async (_, _) => await RefreshData();
        txtDateTo.EditValueChanged += async (_, _) => await RefreshData();
    }

    public string Caption { get; } = "FACTURES";
    public Image Image => frmMain.Image(1);

    private void BtnClearDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        txtDateFrom.EditValue = toDateRepo.MinValue;
        txtDateTo.EditValue = toDateRepo.MaxValue;
    }

    private void BtnHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        OpenHistoryOfSelectedPatient();
    }

    private async void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => await RefreshData();

    private void BtnTraitSpes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => new frmTraitSpec().Show();


    private void GridDetails_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewDetails?.GetRow(e.RowHandle) is FactureDetailDto fc)
        {
            e.Appearance.BackColor = (fc.Ts, fc.DureeTrait >= 60) switch
            {
                (true, _) => Color.LawnGreen,
                (_, true) => Color.Khaki,
                (_, _) => e.Appearance.BackColor
            };
        }
    }

    private void GridFactures_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewFactures?.GetRow(e.RowHandle) is FactureDto row)
        {
            e.Appearance.BackColor = (row.TS, row.LongDuree) switch
            {
                (true, _) => Color.LawnGreen,
                (_, true) => Color.Khaki,
                (_, _) => e.Appearance.BackColor
            };
        }
    }

    private async void GridFactures_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
        if (viewFactures.GetRow(viewFactures.FocusedRowHandle) is FactureDto row)
        {
            await viewDetails.LoadDataAsync(() => DataService.GetFacturDetailsByIdAsync(row.NumFact));
        }
    }

    private async void LastFacturesUC_Load(object sender, EventArgs e)
    {
        await LoadMaxAndMinDates();
        await RefreshData();
    }

    private async Task LoadMaxAndMinDates()
    {
        try
        {
            var (min, max) = await DataService.GetMinAndMaxDatesAsync();

            fromDateRepo.MaxValue = max;
            fromDateRepo.MinValue = min;
            fromDateRepo.TodayDate = min;

            toDateRepo.MaxValue = max;
            toDateRepo.MinValue = min;
            toDateRepo.TodayDate = max;

            txtDateFrom.EditValue = toDateRepo.MinValue;
            txtDateTo.EditValue = toDateRepo.MaxValue;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void OpenHistoryOfSelectedPatient()
    {
        if (viewFactures.GetRow(viewFactures.FocusedRowHandle) is FactureDto row)
        {
            new FrmHistory(row.NumAssure!, row.Rang!).Show();
        }
    }

    private async Task RefreshData()
    {
        try
        {
            var last = (bool)swtchFactures.EditValue;
            var ts = (bool)swtchTS.EditValue;
            var period = new Period { From = (DateTime?)txtDateFrom.EditValue, To = (DateTime?)txtDateTo.EditValue };
            Expression<Func<Facture, bool>> predicate = f => true;

            var txt = (txtMedic.EditValue as string)?.ToUpperInvariant();
            if (!string.IsNullOrEmpty(txt))
            {
                var patterns = txt.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
                foreach (var p in patterns)
                {
                    predicate = predicate.And(f => f.DetailFacts.Any(d => d.Medicament!.FullName!.Contains(p)));
                }
            }
            viewFactures.FocusedRowHandle = 0;
            await factureDtoBindingSource.LoadDataAsync(viewFactures, () => DataService.GetAllFacturesAsync(last, ts,period, predicate));
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void SwtchFactures_EditValueChanged(object s, EventArgs e) => await RefreshData();

    private async void SwtchTS_EditValueChanged(object sender, EventArgs e) => await RefreshData();

    private async void TxtMedic_EditValueChanged(object sender, EventArgs e) => await RefreshData();

    private void ViewFactures_DoubleClick(object sender, EventArgs e)
    {
        OpenHistoryOfSelectedPatient();
    }
}
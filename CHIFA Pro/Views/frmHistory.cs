namespace CHIFA.Pro.Views;

public partial class FrmHistory : XtraForm
{
    private readonly string? _noAssure;
    private readonly string? _rang;
    private bool _isStrict;

    public FrmHistory()
    {
        InitializeComponent();
    }

    public FrmHistory(string noAssure, string rang) : this()
    {
        _noAssure = noAssure;
        _rang = rang;
    }

    private async void FrmHistory_Load(object sender, EventArgs e)
    {
        try
        {
            await LoadHistory();
            await LoadConsumption();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async Task GetSelectedDetails()
    {
        if (gridHistFactures.GetRow(gridHistFactures.FocusedRowHandle) is FactureDto row)
            await gridHistDetails.LoadDataAsync(ct => ChifaService.Instance.GetFactureDetailsByIdAsync(row.NumFact!, ct));
    }

    private void gridConsumption_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (gridConsomption?.GetRow(e.RowHandle) is ConsumptionDto row && RowColors.For(row.TS, row.Duree >= 60) is { } color)
            e.Appearance.BackColor = color;
    }

    private void gridDetails_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (gridHistDetails?.GetRow(e.RowHandle) is FactureDetailDto fc && RowColors.For(fc.Ts, fc.DureeTrait >= 60) is { } color)
            e.Appearance.BackColor = color;
    }

    private void gridHistory_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (gridHistFactures?.GetRow(e.RowHandle) is FactureDto row && RowColors.For(row.TS, row.LongDuree) is { } color)
            e.Appearance.BackColor = color;
    }

    private async void gridHistory_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
        try
        {
            await GetSelectedDetails();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private Task LoadConsumption() => gridConsomption.LoadDataAsync(ct => ChifaService.Instance.LoadConsumptionAsync(_noAssure!, _rang!, _isStrict, ct));

    private async Task LoadHistory()
    {
        await gridHistFactures.LoadDataAsync(ct => ChifaService.Instance.LoadHistoryAsync(_noAssure!, _rang!, ct));
        var beneficiare = await ChifaService.Instance.GetBeneficiareByIdAsync(_noAssure!, _rang!);
        if (!IsDisposed) dataLayoutControl1.DataSource = beneficiare;
    }

    private async void xtraTabControl1_CustomHeaderButtonClick(object sender, DevExpress.XtraTab.ViewInfo.CustomHeaderButtonEventArgs e)
    {
        try
        {
            _isStrict = !_isStrict;
            await LoadConsumption();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}

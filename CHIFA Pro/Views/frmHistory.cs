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
        await LoadHistory();
        await LoadConsumption();
    }

    private async Task GetSelectedDetails()
    {
        if (gridHistFactures.GetRow(gridHistFactures.FocusedRowHandle) is FactureDto row)
            await gridHistDetails.LoadDataAsync(() => DataService.GetFactureDetailsByIdAsync(row.NumFact!));
    }

    private void gridConsumption_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (gridConsomption?.GetRow(e.RowHandle) is ConsumptionDto row)
        {
            e.Appearance.BackColor = (row.TS, row.Duree >= 60) switch
            {
                (true, _) => Color.LawnGreen,
                (_, true) => Color.Khaki,
                (_, _) => e.Appearance.BackColor
            };
        }
    }

    private void gridDetails_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        var fc = gridHistDetails?.GetRow(e.RowHandle) as FactureDetailDto;
        e.Appearance.BackColor = (fc?.Ts, fc?.DureeTrait >= 60) switch
        {
            (true, _) => Color.LawnGreen,
            (_, true) => Color.Khaki,
            (_, _) => e.Appearance.BackColor
        };
    }

    private void gridHistory_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (gridHistFactures?.GetRow(e.RowHandle) is FactureDto row)
        {
            e.Appearance.BackColor = (row.TS, row.LongDuree) switch
            {
                (true, _) => Color.LawnGreen,
                (_, true) => Color.Khaki,
                (_, _) => e.Appearance.BackColor
            };
        }
    }

    private async void gridHistory_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
        await GetSelectedDetails();
    }

    private Task LoadConsumption() => gridConsomption.LoadDataAsync(() => DataService.LoadConsumptionAsync(_noAssure!, _rang!, _isStrict));

    private async Task LoadHistory()
    {
        await gridHistFactures.LoadDataAsync(() => DataService.LoadHistoryAsync(_noAssure!, _rang!));
        dataLayoutControl1.DataSource = await DataService.GetBeneficiareByIdAsync(_noAssure!, _rang!);
    }

    private async void xtraTabControl1_CustomHeaderButtonClick(object sender, DevExpress.XtraTab.ViewInfo.CustomHeaderButtonEventArgs e)
    {
        _isStrict = !_isStrict;
        await LoadConsumption();
    }
}
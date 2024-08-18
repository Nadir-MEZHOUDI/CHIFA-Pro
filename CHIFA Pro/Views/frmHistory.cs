namespace CHIFA.Pro.Forms;

public partial class FrmHistory : XtraForm
{
    private readonly string? noAssure;
    private readonly string? rang;
    private bool isStrict = true;

    public FrmHistory()
    {
        InitializeComponent();
    }

    public FrmHistory(string noAssure, string rang) : this()
    {
        this.noAssure = noAssure;
        this.rang = rang;
    }

    private async void FrmHistory_Load(object sender, EventArgs e)
    {
        await LoadHistory();
        await LoadConsumption();
    }

    private async Task GetSelectedDetails()
    {
        if (gridHistFactures.GetRow(gridHistFactures.FocusedRowHandle) is FactureDto row)
            await gridHistDetails.LoadDataAsync(() => DataService.GetFacturDetailsByIdAsync(row.NumFact));
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

    private Task LoadConsumption() => gridConsomption.LoadDataAsync(() => DataService.LoadConsumptionAsync(noAssure, rang, isStrict));

    private async Task LoadHistory()
    {
        await gridHistFactures.LoadDataAsync(() => DataService.LoadHistoryAsync(noAssure, rang));
        dataLayoutControl1.DataSource = await DataService.GetBeneficiareByIdAsync(noAssure, rang);
    }

    private async void xtraTabControl1_CustomHeaderButtonClick(object sender, DevExpress.XtraTab.ViewInfo.CustomHeaderButtonEventArgs e)
    {
        isStrict = !isStrict;
        await LoadConsumption();
    }
}
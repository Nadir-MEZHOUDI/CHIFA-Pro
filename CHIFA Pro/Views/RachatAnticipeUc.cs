using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

/// <summary>Medicines sold again before the previous treatment ended: factures CNAS / CASNOS are likely to reject.</summary>
public class RachatAnticipeUc : XtraUserControl, INavigable
{
    private static readonly (string Text, int Months)[] Periods = [("1 mois", 1), ("3 mois", 3), ("6 mois", 6), ("12 mois", 12)];

    private readonly GridControl _grid = new() { Dock = DockStyle.Fill };
    private readonly GridView _view;
    private readonly ComboBoxEdit _period = new() { Width = 110 };
    private readonly CheckEdit _openOnly = new() { Text = "Bordereaux non envoyés seulement", Width = 260 };
    private readonly LabelControl _summary = new() { AutoSizeMode = LabelAutoSizeMode.Horizontal };
    private List<EarlyRefillDto> _rows = [];

    public string Caption { get; } = "RACHAT ANTICIPÉ";
    public Image Image => FrmMain.Image(10);

    public RachatAnticipeUc()
    {
        _view = new GridView(_grid);
        _grid.MainView = _view;
        _view.SetOptions();
        _view.OptionsView.ColumnAutoWidth = false;
        _view.HorzScrollVisibility = ScrollVisibility.Auto;
        _view.RowCellStyle += View_RowCellStyle;

        _period.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        _period.Properties.Items.AddRange(Periods.Select(p => p.Text).ToArray());
        _period.SelectedIndex = 1;
        _period.SelectedIndexChanged += async (_, _) => await ReloadAsync();
        _openOnly.CheckedChanged += (_, _) => Bind();

        var export = new SimpleButton { Text = "Exporter", Width = 90 };
        export.Click += (_, _) => _grid.Export("Exporter les rachats anticipés", $"Rachats_Anticipes_{DateTime.Today:yyyyMMdd}");

        var bar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(6, 8, 6, 0), WrapContents = false };
        bar.Controls.AddRange([new LabelControl { Text = "Ventes des derniers :", Padding = new Padding(0, 3, 0, 0) }, _period, _openOnly, export, _summary]);
        _summary.Padding = new Padding(12, 3, 0, 0);

        Controls.Add(_grid);
        Controls.Add(bar);

        Load += async (_, _) => await ReloadAsync();
    }

    private async Task ReloadAsync()
    {
        var from = DateTime.Today.AddMonths(-Periods[Math.Max(_period.SelectedIndex, 0)].Months);
        await _view.LoadDataAsync<EarlyRefillDto>(async ct =>
        {
            _rows = await EarlyRefillService.Instance.GetEarlyRefillsAsync(from, ct);
            return Filtered();
        });
        ShowSummary();
    }

    private void Bind()
    {
        _grid.DataSource = Filtered();
        ShowSummary();
    }

    private List<EarlyRefillDto> Filtered() => _openOnly.Checked ? _rows.Where(r => r.BordereauOuvert).ToList() : _rows;

    private void ShowSummary()
    {
        var rows = Filtered();
        _summary.Text = $"{rows.Select(r => r.NumFact).Distinct().Count():N0} factures à vérifier · " +
                        $"{rows.Count(r => r.BordereauOuvert):N0} lignes encore corrigeables · {rows.Sum(r => r.Montant ?? 0m):N2} DA";
    }

    private void View_RowCellStyle(object sender, RowCellStyleEventArgs e)
    {
        if (_view.GetRow(e.RowHandle) is not EarlyRefillDto row) return;

        if (e.Column.FieldName == nameof(EarlyRefillDto.JoursAvance))
        {
            e.Appearance.ForeColor = Color.Crimson;
            e.Appearance.FontStyleDelta = FontStyle.Bold;
        }

        if (row.BordereauOuvert)
            e.Appearance.BackColor = Color.FromArgb(255, 240, 235);
    }
}

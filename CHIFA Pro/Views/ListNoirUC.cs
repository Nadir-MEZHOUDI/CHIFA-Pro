using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

/// <summary>The pharmacy's patients whose Chifa card is in the CNAS liste noire.</summary>
public partial class ListNoirUc : XtraUserControl,INavigable
{
    private static readonly (Color Back, Color Fore) Current = (Color.FromArgb(254, 226, 226), Color.FromArgb(185, 28, 28));
    private static readonly (Color Back, Color Fore) Old = (Color.FromArgb(255, 237, 213), Color.FromArgb(194, 65, 12));

    public ListNoirUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        gridView1.RowStyle += GridView1_RowStyle;
        gridView1.RowCellStyle += GridView1_RowCellStyle;
    }

    public string Caption { get; } = "LISTE NOIRE";
    public Image Image => FrmMain.Image(9);

    private async void ListNoirUc_Load(object sender, EventArgs e)
    {
        try
        {
            await gridView1.LoadDataAsync(ChifaService.Instance.LoadListeNoirePatientsAsync);
            if (gridView1.Columns[nameof(ListeNoirePatientDto.Statut)] is { } statut)
                statut.Summary.Add(DevExpress.Data.SummaryItemType.Count, nameof(ListeNoirePatientDto.Statut), "{0:N0}");
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void GridView1_RowStyle(object sender, RowStyleEventArgs e)
    {
        if (gridView1.GetRow(e.RowHandle) is ListeNoirePatientDto row)
            e.Appearance.BackColor = (row.CarteActuelle ? Current : Old).Back;
    }

    private void GridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
    {
        if (e.Column.FieldName != nameof(ListeNoirePatientDto.Statut) || gridView1.GetRow(e.RowHandle) is not ListeNoirePatientDto row)
            return;

        e.Appearance.ForeColor = (row.CarteActuelle ? Current : Old).Fore;
        e.Appearance.FontStyleDelta = FontStyle.Bold;
    }
}

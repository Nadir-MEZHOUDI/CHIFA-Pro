using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class ControlMedicalUc : XtraUserControl,INavigable
{
    private static readonly Dictionary<CmStatut, (Color Back, Color Fore)> StatutColors = new()
    {
        [CmStatut.EnAttente] = (Color.FromArgb(226, 232, 240), Color.FromArgb(51, 65, 85)),
        [CmStatut.Accordee] = (Color.FromArgb(220, 252, 231), Color.FromArgb(22, 101, 52)),
        [CmStatut.Signee] = (Color.FromArgb(219, 234, 254), Color.FromArgb(30, 64, 175)),
        [CmStatut.RejetPartiel] = (Color.FromArgb(255, 237, 213), Color.FromArgb(194, 65, 12)),
        [CmStatut.RejetGlobal] = (Color.FromArgb(254, 226, 226), Color.FromArgb(185, 28, 28)),
        [CmStatut.Convoque] = (Color.FromArgb(237, 233, 254), Color.FromArgb(91, 33, 182))
    };

    public ControlMedicalUc()
    {
        InitializeComponent();
        gridView1.SetOptions();
        gridView1.RowStyle += GridView1_RowStyle;
        gridView1.RowCellStyle += GridView1_RowCellStyle;
        gridView1.OptionsDetail.ShowDetailTabs = false;
        AddLignesView();

        Load += async (_, _) =>
        {
            await gridView1.LoadDataAsync(ChifaService.Instance.LoadControlsMedicalAsync);
            if (gridView1.Columns[nameof(ControleMedicalDto.StatutText)] is { } statut)
                statut.Summary.Add(DevExpress.Data.SummaryItemType.Count, nameof(ControleMedicalDto.StatutText), "{0:N0} ordonnances");
        };
    }

    public string Caption { get; } = "CONTRÔLE MÉDICAL";
    public Image Image => FrmMain.Image(10);

    private void GridView1_RowStyle(object sender, RowStyleEventArgs e)
    {
        if (gridView1.GetRow(e.RowHandle) is ControleMedicalDto row && StatutColors.TryGetValue(row.Statut, out var colors))
            e.Appearance.BackColor = colors.Back;
    }

    private void GridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
    {
        if (e.Column.FieldName != nameof(ControleMedicalDto.StatutText)
            || gridView1.GetRow(e.RowHandle) is not ControleMedicalDto row
            || !StatutColors.TryGetValue(row.Statut, out var colors))
            return;

        e.Appearance.ForeColor = colors.Fore;
        e.Appearance.FontStyleDelta = FontStyle.Bold;
    }

    /// <summary>The medicines of each ordonnance, rejected lines in red and partly accorded ones in orange.</summary>
    private void AddLignesView()
    {
        var lignes = new GridView(gridControl);
        lignes.OptionsBehavior.ReadOnly = true;
        lignes.OptionsBehavior.Editable = false;
        lignes.OptionsView.ShowGroupPanel = false;
        // The pattern view holds no data: each expanded row gets its own copy, which raises the event.
        lignes.RowCellStyle += (sender, e) =>
        {
            if (((GridView)sender).GetRow(e.RowHandle) is not ControleMedicalLigneDto ligne) return;
            var colors = ligne.Decision switch
            {
                "Rejeté" => StatutColors[CmStatut.RejetGlobal],
                "Partiel" => StatutColors[CmStatut.RejetPartiel],
                _ => default
            };
            if (colors == default) return;
            e.Appearance.BackColor = colors.Back;
            e.Appearance.ForeColor = colors.Fore;
        };
        gridControl.LevelTree.Nodes.Add(nameof(ControleMedicalDto.Lignes), lignes);
    }
}

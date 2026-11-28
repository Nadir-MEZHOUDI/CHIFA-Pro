using DevExpress.XtraLayout.Utils;

namespace CHIFA.Pro.Views;

public partial class UsersUc : XtraUserControl, INavigable
{
    public UsersUc()
    {
        InitializeComponent();
        HidePasswords();
        gridView1.SetOptions();
        gridView1.CustomColumnDisplayText += (_, e) =>
        {
            if (e.Column == colLASTCHANGE)
                e.DisplayText = AccessRights(gridView1.GetListSourceRowCellValue(e.ListSourceRowIndex, colNAT) as bool?, e.Value as IEnumerable<DroitAcce>);
        };
        Load += async (_, _) => await gridView1.LoadDataAsync(ChifaService.Instance.GetUsersAsync);
    }

    /// <summary>The rights column holds the list of screens a user may open; the grid would show its type name.</summary>
    private static string AccessRights(bool? admin, IEnumerable<DroitAcce>? rights)
    {
        if (admin == true)
            return "Tous (administrateur)";

        var enabled = rights?.Count(d => d.Activer == true) ?? 0;
        return enabled switch
        {
            0 => "Aucun",
            1 => "1 droit",
            _ => $"{enabled} droits"
        };
    }

    public string Caption { get; } = "UTILISATEURS";
    public Image Image => FrmMain.Image(11);

    /// <summary>
    /// Passwords of CHIFA users must never be shown. The service no longer loads them;
    /// the grid column and the detail field are removed too, so they cannot come back
    /// through the column chooser, the find panel or an export.
    /// </summary>
    private void HidePasswords()
    {
        gridView1.Columns.Remove(colPASS);
        PASSTextEdit.DataBindings.Clear();
        ItemForPASS.Visibility = LayoutVisibility.Never;
    }
}

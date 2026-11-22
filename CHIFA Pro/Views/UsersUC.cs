using DevExpress.XtraLayout.Utils;

namespace CHIFA.Pro.Views;

public partial class UsersUc : XtraUserControl, INavigable
{
    public UsersUc()
    {
        InitializeComponent();
        HidePasswords();
        gridView1.SetOptions();
        Load += async (_, _) => await gridView1.LoadDataAsync(ChifaService.Instance.GetUsersAsync);
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

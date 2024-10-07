using CHIFA.Pro.Views;
using DevExpress.Utils;
using DevExpress.XtraTab;

namespace CHIFA.Pro.Helpers;
internal static class NavigationService
{
    private static FrmMain? frmMain;
    private static FrmMain? Main => frmMain ??= Application.OpenForms.OfType<FrmMain>().FirstOrDefault();
    public static void NavigateTo<T>(this object sender) where T : UserControl, INavigable, new()
    {
        if (Main?.tabContainer == null) return;

        var tab = Main.tabContainer.TabPages.FirstOrDefault(x => x.Controls.Cast<UserControl>().Any(c => c.GetType() == typeof(T)));
        if (tab == null)
        {
            var uc = new T { Dock = DockStyle.Fill };
            tab = new XtraTabPage { Text = uc.Caption, Image = uc.Image };
            tab.Controls.Add(uc);
            if (tab.Text.Contains("HOME"))
                tab.ShowCloseButton = DefaultBoolean.False;
            Main.tabContainer.TabPages.Add(tab);
        }
        Main.tabContainer.SelectedTabPage = tab;
    }
}

public interface INavigable
{
    string Caption { get; }
    Image Image { get; }
}
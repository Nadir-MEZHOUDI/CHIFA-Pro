using DevExpress.Utils;
using DevExpress.XtraTab;

namespace CHIFA.Pro.Helpers;
internal static class NavigationService
{
    private static FrmMain? _frmMain;
    private static FrmMain? Main => _frmMain ??= Application.OpenForms.OfType<FrmMain>().FirstOrDefault();
    public static T? NavigateTo<T>(this object sender, Action<T>? configure = null) where T : UserControl, INavigable, new()
    {
        if (Main?.tabContainer == null)
            return null;

        var tab = Main.tabContainer.TabPages.FirstOrDefault(x => x.Controls.Cast<UserControl>().Any(c => c.GetType() == typeof(T)));
        T control;
        if (tab == null)
        {
            control = new T { Dock = DockStyle.Fill };
            tab = new XtraTabPage { Text = control.Caption, Image = control.Image };
            tab.Controls.Add(control);
            if (control is HomeUc || tab.Text.Contains("ACCUEIL", StringComparison.InvariantCultureIgnoreCase) || tab.Text.Contains("HOME", StringComparison.InvariantCultureIgnoreCase))
                tab.ShowCloseButton = DefaultBoolean.False;
            Main.tabContainer.TabPages.Add(tab);
        }
        else
        {
            control = tab.Controls.OfType<T>().First();
        }

        configure?.Invoke(control);
        Main.tabContainer.SelectedTabPage = tab;
        return control;
    }
}

public interface INavigable
{
    string Caption { get; }
    Image Image { get; }
}

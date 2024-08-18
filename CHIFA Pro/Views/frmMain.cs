using CHIFA.Pro.uc;

using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;

namespace CHIFA.Pro;

public partial class frmMain : XtraForm
{
    public frmMain()
    {
        InitializeComponent();
    }
    public static Image Image(int index) => (Application.OpenForms["frmMain"] as frmMain)!.AppImages.ImageSource.Images[index];

    private void accActes_Click(object sender, EventArgs e) => sender.NavigateTo<SpecialitesUc>();

    private void accAssures_Click(object sender, EventArgs e) => sender.NavigateTo<AssuresUc>();

    private void accBordereaux_Click(object sender, EventArgs e) => sender.NavigateTo<BordereauxUc>();

    private void accCenters_Click(object sender, EventArgs e) => sender.NavigateTo<CentersUc>();

    private void accCM_Click(object sender, EventArgs e) => sender.NavigateTo<ControlMedicalUc>();

    private void accFactures_Click(object sender, EventArgs e) => sender.NavigateTo<TraitSpecUc>();

    private void accLastFactures_Click(object sender, EventArgs e) => sender.NavigateTo<FacturesUC>();

    private void accLN_Click(object sender, EventArgs e) => sender.NavigateTo<ListNoirUc>();

    private void accMedicaments_Click(object sender, EventArgs e) => sender.NavigateTo<NomenclaturUc>();

    private void accOfficine_Click(object sender, EventArgs e) => sender.NavigateTo<OfficineUc>();

    private void accordionControlElement2_Click(object sender, EventArgs e) => sender.NavigateTo<FormesUc>();

    private void accParametres_Click(object sender, EventArgs e) => sender.NavigateTo<ParametersUc>();

    private void accStatistics_Click(object sender, EventArgs e) => sender.NavigateTo<StatisticsUc>();

    private void accUsers_Click(object sender, EventArgs e) => sender.NavigateTo<UsersUc>();

    private void acMain_Click(object sender, EventArgs e) => sender.NavigateTo<HomeUc>();

    private void ChangeNameBasedOnDotNetVersion() => Text = $"CHIFA PRO [V : {Application.ProductVersion.Split("+")[0]}] [.NET {Environment.Version}] ( By MEZHOUDI Hadj Nadir )";

    private async void frmMain_Load(object sender, EventArgs e)
    {
        try
        {
            this.NavigateTo<HomeUc>();

            ChangeNameBasedOnDotNetVersion();

            var check = DbChecker.RunServerAsync();

            var load = LoadServerInfo();

            await Task.WhenAll(check, load);

        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async Task LoadServerInfo()
    {
        try
        {
            txtDatabase.Text = AppSettings.Default.DbName;

            var db = new ChifaDb();
            var server = await db.Parametres.FirstOrDefaultAsync();
            if (server == null)
                return;

            txtCodePs.Text = server.CodePs ?? "";
            txtServer.Text = server.PosteTelech;
            txtPharmacie.Text = server?.Nom + " " + server?.Prenom;
            txtIP.Text = XtraHelper.ListLocalIPAddresses();

        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void TabContainer_CloseButtonClick(object sender, EventArgs e)
    {
        var index = tabContainer.SelectedTabPageIndex;
        var page = ((ClosePageButtonEventArgs)e).Page as XtraTabPage;
        if (page is null || page.Text.Contains("Home", StringComparison.InvariantCultureIgnoreCase))        
            return;
        
        tabContainer.SelectedTabPageIndex = index > 1 ? index - 1 : index + 1;
        tabContainer.TabPages.Remove(page, true);
    }
}
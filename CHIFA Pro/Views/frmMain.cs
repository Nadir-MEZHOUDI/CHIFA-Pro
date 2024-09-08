using CHIFA.Pro.uc;

using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;

using Velopack;

namespace CHIFA.Pro;

public partial class frmMain : XtraForm
{
    public frmMain()
    {
        InitializeComponent();
    }
    public static Image Image(int index) => (Application.OpenForms["frmMain"] as frmMain)!.AppImages.ImageSource.Images[index];

    private void accSpecialists_Click(object sender, EventArgs e) => sender.NavigateTo<SpecialitesUc>();

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

    private void acHome_Click(object sender, EventArgs e) => sender.NavigateTo<HomeUc>();

    private void ChangeNameBasedOnDotNetVersion() => Text = $"CHIFA PRO [V : {Application.ProductVersion.Split("+")[0]}] [.NET {Environment.Version}] ( By MEZHOUDI Hadj Nadir )";

    private async void frmMain_Load(object sender, EventArgs e)
    {
        try
        {
            ChangeNameBasedOnDotNetVersion();

            this.NavigateTo<HomeUc>();

            await DbChecker.RunServerAsync();

            var load = LoadServerInfo();

            var update =  UpdateAppAsync();

            await Task.WhenAll(load,update);

        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    public static async Task UpdateAppAsync(bool showMessage=false)
    {
        var mgr = new UpdateManager("https://nadirsmartapp.blob.core.windows.net/chifa-pro");

        if (!mgr.IsInstalled)            return;

        // check for new version
        var newVersion = await mgr.CheckForUpdatesAsync();
        if (newVersion == null)
        {
            if(showMessage)
            {
                var result = XtraMessageBox.Show("No new version available,You are using the latest Version", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return; // no update available
        }

        if (showMessage)
        {
            var result = XtraMessageBox.Show("New version available. Updating now", "Update", MessageBoxButtons.OK, MessageBoxIcon.Information);            
        }

        // download new version
        await mgr.DownloadUpdatesAsync(newVersion);

        // install new version and restart app
        mgr.ApplyUpdatesAndRestart(newVersion);

    }

    private async Task LoadServerInfo()
    {
        try
        {
            txtDatabase.Text = "CHIFA_OFFICINE";
            txtIP.Text  = XtraHelper.GetThis_PC_IP_Address();
            txtServer.Text = ChifaDb.Server;

            var db = new ChifaDb();
            var officine = await db.Parametres.FirstOrDefaultAsync();         

            txtCodePs.Text = officine?.CodePs ?? "";
            txtPharmacie.Text = officine?.Nom + " " + officine?.Prenom;

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
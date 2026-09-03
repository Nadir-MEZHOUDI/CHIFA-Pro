using CHIFA.Pro.Helpers.Settings;

using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using LinqToDB.Async;
using Velopack;

namespace CHIFA.Pro.Views;

public partial class FrmMain : XtraForm
{
    private static FrmMain? _instance;
    public static FrmMain Instance => _instance ??= (Application.OpenForms[nameof(FrmMain)] as FrmMain)!;

    public FrmMain()
    {
        _instance = this;
        InitializeComponent();
        SetStatisticsAccordionImages();
        accordionControl1.Width = 270;
        ChangeTitle();
    }

    public static Image Image(int index)
    {
        try
        {
            var inst = _instance ?? (Application.OpenForms[nameof(FrmMain)] as FrmMain);
            if (inst?.AppImages?.ImageSource?.Images != null && index >= 0 && index < inst.AppImages.ImageSource.Images.Count)
                return inst.AppImages.ImageSource.Images[index];
        }
        catch { }
        return new Bitmap(16, 16);
    }

    

    private void accPsychotropes_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<PsychotropesUc>();
    }

    private void accSpecialists_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<SpecialitesUc>();
    }

    private void accAssures_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<AssuresUc>();
    }

    private void accBordereaux_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<BordereauxUc>();
    }

    private void accCenters_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<CentersUc>();
    }

    private void accCM_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<ControlMedicalUc>();
    }

    private void accFactures_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<TraitSpecUc>();
    }

    private void accLastFactures_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<FacturesUc>();
    }

    private void accLN_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<ListNoirUc>();
    }

    private void accMedicaments_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<NomenclaturUc>();
    }

    private void accOfficine_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<OfficineUc>();
    }

    private void accordionControlElement2_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<FormesUc>();
    }

    private void accParametres_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<ParametersUc>();
    }

    private void NavigateToStatistics(object sender, StatisticsSection section)
    {
        sender.NavigateTo<StatisticsUc>(statistics => statistics.SelectSection(section));
    }

    private void SetStatisticsAccordionImages()
    {
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticsUc));

        accStatisticsBordereaux.ImageOptions.Image = (Image)resources.GetObject("tabBordereauxTable.ImageOptions.Image")!;
        accStatisticsYearly.ImageOptions.Image = (Image)resources.GetObject("tabYearlyTable.ImageOptions.Image")!;
        accStatisticsMonthly.ImageOptions.Image = (Image)resources.GetObject("tabMonthlyTable.ImageOptions.Image")!;
        accStatisticsWeekly.ImageOptions.Image = (Image)resources.GetObject("tabWeeklyTable.ImageOptions.Image")!;
        accStatisticsDaily.ImageOptions.Image = (Image)resources.GetObject("tabDailyTable.ImageOptions.Image")!;
        accStatisticsProducts.ImageOptions.Image = (Image)resources.GetObject("tabProductTable.ImageOptions.Image")!;
        accStatisticsClients.ImageOptions.Image = (Image)resources.GetObject("tabClientsTable.ImageOptions.Image")!;
    }

    private void accStatisticsBordereaux_Click(object sender, EventArgs e)
    {
        NavigateToStatistics(sender, StatisticsSection.Bordereaux);
    }

    private void accStatisticsYearly_Click(object sender, EventArgs e)
    {
        NavigateToStatistics(sender, StatisticsSection.Yearly);
    }

    private void accStatisticsMonthly_Click(object sender, EventArgs e)
    {
        NavigateToStatistics(sender, StatisticsSection.Monthly);
    }

    private void accStatisticsWeekly_Click(object sender, EventArgs e)
    {
        NavigateToStatistics(sender, StatisticsSection.Weekly);
    }

    private void accStatisticsDaily_Click(object sender, EventArgs e)
    {
        NavigateToStatistics(sender, StatisticsSection.Daily);
    }

    private void accStatisticsProducts_Click(object sender, EventArgs e)
    {
        NavigateToStatistics(sender, StatisticsSection.Products);
    }

    private void accStatisticsClients_Click(object sender, EventArgs e)
    {
        NavigateToStatistics(sender, StatisticsSection.Clients);
    }

    private void accUsers_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<UsersUc>();
    }

    private void acHome_Click(object sender, EventArgs e)
    {
        sender.NavigateTo<HomeUc>();
    }

    private void ChangeTitle()
    {
        Text =
            $@"CHIFA PRO [V : {Application.ProductVersion.Split("+")[0]}] [.NET {Environment.Version}] ( By MEZHOUDI Hadj Nadir )";
    }

    private async void frmMain_Load(object sender, EventArgs e)
    {
        try
        {
            if (!await EnsureDatabaseConnectionAsync())
                return;

            if (Environment.GetCommandLineArgs().Contains("minimized"))
            {
                _ = MinimizeApp();//to hide it only
            }

            this.NavigateTo<HomeUc>();
            await Task.Yield();
            await Task.Delay(500);
            _ = LoadServerInfo();
            _ = UpdateAppAsync();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async Task<bool> EnsureDatabaseConnectionAsync()
    {

        if (await DbChecker.CheckDbConnectionAsync())
            return true;

        using var dialog = new DatabaseConnectionPromptForm();
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return false;

        return await DbChecker.CheckDbConnectionAsync();
    }


    public async Task UpdateAppAsync(bool showMessage = false)
    {
        try
        {
            progressBar.Visible = true;
            var mgr = new UpdateManager("https://nadirsmartapp.blob.core.windows.net/chifa-pro");

            if (!mgr.IsInstalled)
            {
                progressBar.Value = 50;
                await Task.Delay(2000);
                return;
            }

            var newVersion = await mgr.CheckForUpdatesAsync();
            if (newVersion == null)
            {
                if (showMessage)
                    await ShowNotification("Vous disposez de la dernière version", "Mise à jour");
            }
            else
            {
                await mgr.DownloadUpdatesAsync(newVersion,
                    i => Invoke(() => progressBar.Value = i));

                mgr.ApplyUpdatesAndRestart(newVersion);
            }
        }
        catch (Exception ex)
        {
            ex.Log(false);
        }
        finally
        {
            progressBar.Visible = false;
        }
    }


    private async Task LoadServerInfo()
    {
        try
        {
            txtServer.Text = ChifaDb.Server;
            txtDatabase.Text = @"CHIFA_OFFICINE";
            txtIP.Text = XtraHelper.GetThis_PC_IP_Address();

            var officine = await ChifaService.Instance.GetFirstOfficineAsync();
            txtCodePs.Text = officine?.CodePs ?? "";
            txtPharmacie.Text = officine?.Nom + @" " + officine?.Prenom;

        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void TabContainer_CloseButtonClick(object sender, EventArgs e)
    {
        try
        {
            var index = tabContainer.SelectedTabPageIndex;
            if (((ClosePageButtonEventArgs)e).Page is not XtraTabPage page ||
                page.Controls.OfType<HomeUc>().Any() ||
                page.Text.Contains("ACCUEIL", StringComparison.InvariantCultureIgnoreCase) ||
                page.Text.Contains("Home", StringComparison.InvariantCultureIgnoreCase))
                return;

            tabContainer.SelectedTabPageIndex = index > 1 ? index - 1 : index + 1;
            tabContainer.TabPages.Remove(page, true);
        }
        catch (Exception ex)
        {
            ex.Log();
        }

    }


    #region Bring Single Instance To Front

    protected override void WndProc(ref Message m)
    {
        try
        {
            if (m.Msg == SingleInstance.WM_SHOWFIRSTINSTANCE)
            {
                Show();

                Notification.Visible = false;

                if (WindowState == FormWindowState.Minimized)
                    WindowState = FormWindowState.Maximized;

                Activate();
            }
        }
        catch (Exception)
        {
            //ignore
        }

        base.WndProc(ref m);
    }

    #endregion

    private void timer_Tick(object sender, EventArgs e)
    {
        txtSpring.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss").ToUpperInvariant();
    }

    private async void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                await MinimizeApp();
            }
            else
            {
                Notification.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async Task MinimizeApp()
    {
        Hide();
        Notification.Visible = true;
        await ShowNotification("Application réduite dans la zone de notification.");
    }

    private void Notification_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        try
        {
            Show();
            WindowState = FormWindowState.Maximized;
            Notification.Visible = false;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    public async Task ShowBordereauxNotifications()
    {
        try
        {
            var db = new ChifaDb();
            var bords = await db.Bordereaus
                .Where(x => x.Etat == 'O')
                .Select(x => new BordNotification
                {
                    NumBord = x.NumBord,
                    Center = x.Center!.Nom,
                    NmbrFact = x.Factures.Count(),
                    Min = x.Factures.Min(f => f.DateFact),
                    Max = x.Factures.Max(f => f.DateFact),
                    Montant = x.Factures.Sum(f => f.MontOff + f.MontMaj + f.MontMajFae)
                })
                .ToListAsync();

            foreach (var bord in bords)
            {
                var message = "";
                if (AppSettings.Default.NotificationOnDays && AppSettings.Default.MaxDays > 0 && bord.Days > AppSettings.Default.MaxDays)
                {
                    message += $"\n a {bord.Days:N0} Jours";
                }

                if (AppSettings.Default.NotificationOnMontant && AppSettings.Default.MaxMontant > 0 && bord.Montant > AppSettings.Default.MaxMontant)
                {
                    message += $"\n a {bord.Montant:N2} DA";
                }

                if (AppSettings.Default.NotificationOnNmbr && AppSettings.Default.MaxNmbr > 0 && bord.NmbrFact > AppSettings.Default.MaxNmbr)
                {
                    message += $"\n a {bord.NmbrFact:N0} Factures";
                }
                if (!string.IsNullOrWhiteSpace(message))
                    await ShowNotification($"Le Bord {bord.NumBord} de {bord.Center} " + message, ToolTipIcon.Warning);

            }
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async Task ShowNotification(string message, string text, ToolTipIcon icon = ToolTipIcon.Info)
    {
        Notification.ShowBalloonTip(5, message, text, icon);
        await Task.Yield();
        await Task.Delay(200);
    }


    private async Task ShowNotification(string message, ToolTipIcon icon = ToolTipIcon.Info)
    {
        await ShowNotification(message, "CHIFA Pro", icon);
    }


    private void menuExit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void menuOpen_Click(object sender, EventArgs e)
    {
        try
        {
            Show();
            WindowState = FormWindowState.Maximized;
            Notification.Visible = false;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}

public record BordNotification
{
    public int Days => (Max - Min)?.Days ?? 0;
    public string? NumBord { get; init; }
    public string? Center { get; init; }
    public int NmbrFact { get; init; }
    public DateTime? Min { get; init; }
    public DateTime? Max { get; init; }
    public decimal? Montant { get; init; }
}

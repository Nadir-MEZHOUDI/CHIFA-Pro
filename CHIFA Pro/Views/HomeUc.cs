namespace CHIFA.Pro.Views;

public partial class HomeUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "ACCUEIL";
    public Image Image => FrmMain.Image(0);

    public HomeUc()
    {
        InitializeComponent();
        // At the app's Segoe UI 10, « STATISTIQUES » and « BORDEREAUX » no longer fit a medium tile.
        tileControl1.AppearanceItem.Normal.Font = new Font("Segoe UI", 9F);
        tileControl1.AppearanceItem.Normal.Options.UseFont = true;
        ApplyAccess();
        AccessGuard.Changed += ApplyAccess;
        Disposed += (_, _) => AccessGuard.Changed -= ApplyAccess;
    }

    private void ApplyAccess()
    {
        itmStatistics.Visible = !AccessGuard.IsHidden(ProtectedItem.Statistics);
        itmBordereaux.Visible = !AccessGuard.IsHidden(ProtectedItem.Bordereaux);

        var montant = !AccessGuard.IsHidden(ProtectedItem.HomeMontant);
        var factures = !AccessGuard.IsHidden(ProtectedItem.HomeFactures);
        var marge = !AccessGuard.IsHidden(ProtectedItem.HomeMarge);

        chartControl1.Series["Montant"].Visible = montant;
        chartControl1.Series["Factures"].Visible = factures;
        foreach (var name in new[] { "Brut", "Marge", "Maj" })
            chartControl1.Series[name].Visible = marge;

        if (chartControl1.Diagram is DevExpress.XtraCharts.XYDiagram diagram)
        {
            diagram.DefaultPane.Visibility = Visibility(montant);
            diagram.Panes[0].Visibility = Visibility(factures);
            diagram.Panes[1].Visibility = Visibility(marge);
            // A hidden pane's axis otherwise moves to a visible pane as an empty 0–1 scale.
            diagram.AxisY.Visibility = montant ? DevExpress.Utils.DefaultBoolean.Default : DevExpress.Utils.DefaultBoolean.False;
            diagram.SecondaryAxesY[0].Visibility = factures ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
            diagram.SecondaryAxesY[1].Visibility = marge ? DevExpress.Utils.DefaultBoolean.True : DevExpress.Utils.DefaultBoolean.False;
        }

        chartControl1.Visible = montant || factures || marge;
    }

    private static DevExpress.XtraCharts.ChartElementVisibility Visibility(bool visible) =>
        visible ? DevExpress.XtraCharts.ChartElementVisibility.Visible : DevExpress.XtraCharts.ChartElementVisibility.Hidden;

    private async Task ReLoadDataAsync()
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            var data = await StatisticsService.Instance.GetThisWeekStatsAsync();
            weekStatBindingSource.DataSource = data;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            // A server that is down must not leave the wait cursor on the home screen.
            if (!IsDisposed) Cursor = Cursors.Default;
        }
    }

    private void itmBordereaux_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<BordereauxUc>();
    }

    private void itmFactures_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<FacturesUc>();

    }

    private void ItmMedicaments_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<NomenclaturUc>();
    }

    private void itmStatistics_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<StatisticsUc>();
    }

    private void itmTraitSpec_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<TraitSpecUc>();
    }

    private void itmAssures_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<AssuresUc>();
    }

    private void ItemSpecialists_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<SpecialitesUc>();
    }

    private void itemFormes_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<FormesUc>();
    }

    private void ItemCenters_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<CentersUc>();
    }

    private void ItemListNoir_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<ListNoirUc>();
    }

    private void itemUsers_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<UsersUc>();
    }

    private void ItemControlMedical_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<ControlMedicalUc>();
    }

    private void ItemOfficine_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<OfficineUc>();
    }

    private async void ItemRefresh_ItemClick(object sender, TileItemEventArgs e)
    {
        try
        {
            await ReLoadDataAsync();
            await FrmMain.Instance.ShowBordereauxNotifications();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void itemUpdate_ItemClick(object sender, TileItemEventArgs e)
    {
        try
        {
            await FrmMain.Instance.Updater.UpdateNowAsync(true);
        }
        catch (Exception ex)
        {

            ex.Log();
        }
    }

    private async void HomeUc_Load(object sender, EventArgs e)
    {
        try
        {
            _ = DbChecker.RunServerAsync();
            _ = ReLoadDataAsync();
            _ = FrmMain.Instance.ShowBordereauxNotifications();
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}


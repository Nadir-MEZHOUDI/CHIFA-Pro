namespace CHIFA.Pro.Views;

public partial class HomeUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "HOME";
    public Image Image => frmMain.Image(0);

    public HomeUc()
    {
        InitializeComponent();
    }

    private async Task ReLoadDataAsync()
    {
        Cursor = Cursors.WaitCursor;
        var data = await Task.Run(StatisticsService.GetThisWeekStatsAsync);
        weekStatBindingSource.DataSource = data ;
        Cursor = Cursors.Default;
    }

    private void itmBordereaux_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<BordereauxUc>();
    }

    private void itmFactures_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<FacturesUc>();

    }

    private void itmMedicaments_ItemClick(object sender, TileItemEventArgs e)
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
        await ReLoadDataAsync();
    }

    private async void itemUpdate_ItemClick(object sender, TileItemEventArgs e)
    {
        var frm = Application.OpenForms.OfType<frmMain>().FirstOrDefault();
        if (frm != null)
            await frm.UpdateAppAsync(true);
    }

    private async void HomeUc_Load(object sender, EventArgs e)
    {
        await ReLoadDataAsync();
    }
}



namespace CHIFA.Pro.uc;

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
        var weekStatService = await WeekStatService.GetWeekStatsAsync();
        chartControl1.DataSource = weekStatBindingSource;
        weekStatBindingSource.DataSource = weekStatService;
        Cursor = Cursors.Default;
    }

    private void itmBordereaux_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<BordereauxUc>();
    }

    private void itmFactures_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<FacturesUC>();

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

    private void itemSpecialetes_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<SpecialitesUc>();
    }

    private void itemFormes_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<FormesUc>();
    }

    private void itemCenters_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<CentersUc>();
    }

    private void itemListNoir_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<ListNoirUc>();
    }

    private void itemUsers_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<UsersUc>();
    }

    private void itemControlMedical_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<ControlMedicalUc>();
    }

    private void itemOfficine_ItemClick(object sender, TileItemEventArgs e)
    {
        this.NavigateTo<OfficineUc>();
    }

    private async void HomeUc_Enter(object sender, EventArgs e)
    {
      await  ReLoadDataAsync();
    }

    private async void HomeUc_Paint(object sender, PaintEventArgs e)
    {
        await ReLoadDataAsync();
    }
}


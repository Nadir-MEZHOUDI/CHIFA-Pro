
namespace CHIFA.Pro.uc;

public partial class HomeUc : XtraUserControl, INavigable
{
    public string Caption { get; } = "HOME";
    public Image Image => frmMain.Image(0);

    public HomeUc()
    {
        InitializeComponent();
        ReLoadDataAsync();
    }

    private void ReLoadDataAsync()
    {
        var weekStatService = WeekStatService.GetWeekStats();
        chartControl1.DataSource = weekStatBindingSource;
        weekStatBindingSource.DataSource = weekStatService;
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

    private void HomeUc_Enter(object sender, EventArgs e)
    {
        ReLoadDataAsync();
    }
}


using CHIFA.Pro.Reports;

namespace CHIFA.Pro.Views;

public partial class FacturesUc : XtraUserControl, INavigable
{
    private readonly ReloadCancellation _reloads = new();
    private string _medicament = "";

    public FacturesUc()
    {
        InitializeComponent();
        viewFactures.SetOptions();
        txtMedic.OnTyping(text =>
        {
            _medicament = text;
            return ScheduleFacturesReloadAsync();
        });
        Disposed += FacturesUc_Disposed;
    }

    public string Caption { get; } = "FACTURES";
    public Image Image => FrmMain.Image(1);

    private void BtnClearDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        txtDateFrom.EditValue = Period.MinDate;
        txtDateTo.EditValue = null; // up to today
    }

    private void BtnHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        OpenHistoryOfSelectedPatient();
    }

    private async void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => await ReloadFacturesImmediateAsync();

    private void BtnTraitSpes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => new FrmTraitSpec().Show();

    private void BtnViewFacture_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => _ = PrintFactureAsync(avecMajoration: true);

    private void BtnPrintFacture_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => _ = PrintFactureAsync(avecMajoration: true);

    private void BtnPrintVerso_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => _ = PrintVersoAsync();

    private void GridDetails_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewDetails?.GetRow(e.RowHandle) is FactureDetailDto fc && RowColors.For(fc.Ts, fc.DureeTrait >= 60) is { } color)
            e.Appearance.BackColor = color;
    }

    private void GridFactures_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewFactures?.GetRow(e.RowHandle) is FactureDto row && RowColors.For(row.TS, row.LongDuree) is { } color)
            e.Appearance.BackColor = color;
    }

    private async void GridFactures_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e) => await LoadSelectedDetailsAsync();

    private async Task LoadSelectedDetailsAsync()
    {
        if (viewFactures.GetRow(viewFactures.FocusedRowHandle) is FactureDto row)
            await viewDetails.LoadDataAsync(ct => ChifaService.Instance.GetFactureDetailsByIdAsync(row.NumFact!, ct));
        else
            viewDetails.GridControl.DataSource = null;
    }

    private async void LastFacturesUC_Load(object sender, EventArgs e)
    {
        await DateFilter.SetUpAsync(txtDateFrom, txtDateTo, DateTime.Today.AddYears(-1), calendarOnFrom: true, _reloads, LoadFacturesAsync);
        _ = ReloadFacturesImmediateAsync();
    }

    private void OpenHistoryOfSelectedPatient()
    {
        if (viewFactures.GetRow(viewFactures.FocusedRowHandle) is FactureDto row)
        {
            new FrmHistory(row.NumAssure!, row.Rang!).Show();
        }
    }

    private Task ScheduleFacturesReloadAsync() => DateFilter.DebounceAsync(_reloads, LoadFacturesAsync);

    private async Task ReloadFacturesImmediateAsync()
    {
        var cancellationToken = _reloads.Next();
        await LoadFacturesAsync(cancellationToken);
    }

    private async Task LoadFacturesAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        try
        {
            var last = (bool)swtchFactures.EditValue;

            var ts = (bool)swtchTS.EditValue;

            var period = DateFilter.ToPeriod(txtDateFrom.EditValue, txtDateTo.EditValue);

            Expression<Func<Facture, bool>> predicate = f => true;

            var txt = _medicament.ToUpperInvariant();

            if (!string.IsNullOrEmpty(txt))
            {
                var patterns = txt.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
                predicate = patterns.Aggregate(predicate, (current, p) => current.And(f => f.DetailFacts.Any(d => d.Medicament.FullName!.Contains(p))));
            }

            if (cancellationToken.IsCancellationRequested)
                return;

            viewFactures.FocusedRowHandle = 0;
            await factureDtoBindingSource.LoadDataAsync(viewFactures,
                ct => ChifaService.Instance.GetAllFacturesAsync(last, ts, period, predicate, ct), cancellationToken);

            // The focus stays on row 0, so FocusedRowChanged does not fire: the details would show the old facture.
            if (!cancellationToken.IsCancellationRequested)
                await LoadSelectedDetailsAsync();
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private async void SwtchFactures_EditValueChanged(object s, EventArgs e) => await ScheduleFacturesReloadAsync();

    private async void SwtchTS_EditValueChanged(object sender, EventArgs e) => await ScheduleFacturesReloadAsync();


    private void ViewFactures_DoubleClick(object sender, EventArgs e)
    {
        OpenHistoryOfSelectedPatient();
    }

    private async Task PrintFactureAsync(bool avecMajoration)
    {
        try
        {
            if (await LoadSelectedFactureForPrintAsync() is not { } facture)
                return;

            if (facture.DetailFacts is null || !facture.DetailFacts.Any())
            {
                XtraMessageBox.Show("Cette facture ne contient aucun détail.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FactureReportService.PrintFacture(facture, avecMajoration);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    private async Task PrintVersoAsync()
    {
        try
        {
            if (await LoadSelectedFactureForPrintAsync() is { } facture)
                FactureReportService.PrintVerso(facture);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>The selected facture with its details, or <c>null</c> once the user is told why. Leaves the wait cursor on.</summary>
    private async Task<Facture?> LoadSelectedFactureForPrintAsync()
    {
        if (viewFactures.GetRow(viewFactures.FocusedRowHandle) is not FactureDto { NumFact: { } numFact } || string.IsNullOrWhiteSpace(numFact))
        {
            XtraMessageBox.Show("Veuillez sélectionner une facture.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return null;
        }

        Cursor = Cursors.WaitCursor;
        var facture = await ChifaService.Instance.GetFactureForPrintAsync(numFact);
        if (facture is null)
            XtraMessageBox.Show($"Facture {numFact} introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        return facture;
    }

    private void FacturesUc_Disposed(object? sender, EventArgs e)
    {
        _reloads.Stop();
    }
}

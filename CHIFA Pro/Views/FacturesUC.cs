using CHIFA.Pro.Reports;

namespace CHIFA.Pro.Views;

public partial class FacturesUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private CancellationTokenSource? _reloadCts;

    public FacturesUc()
    {
        InitializeComponent();
        viewFactures.SetOptions();
        Disposed += FacturesUc_Disposed;
    }

    public string Caption { get; } = "FACTURES";
    public Image Image => FrmMain.Image(1);

    private void BtnClearDate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        txtDateFrom.EditValue = toDateRepo.MinValue;
        txtDateTo.EditValue = toDateRepo.MaxValue;
    }

    private void BtnHistory_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        OpenHistoryOfSelectedPatient();
    }

    private async void BtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => await ReloadFacturesImmediateAsync();

    private void BtnTraitSpes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) => new FrmTraitSpec().Show();

    private void BtnViewFacture_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => _ = ViewFactureAsync();

    private void BtnPrintFacture_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => _ = PrintFactureAsync(avecMajoration: true);

    private void BtnPrintVerso_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => _ = PrintVersoAsync();

    private void BtnDeleteFacture_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        => _ = DeleteFactureAsync();


    private void GridDetails_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewDetails?.GetRow(e.RowHandle) is FactureDetailDto fc)
        {
            e.Appearance.BackColor = (fc.Ts, fc.DureeTrait >= 60) switch
            {
                (true, _) => Color.LawnGreen,
                (_, true) => Color.Khaki,
                (_, _) => e.Appearance.BackColor
            };
        }
    }

    private void GridFactures_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewFactures?.GetRow(e.RowHandle) is FactureDto row)
        {
            e.Appearance.BackColor = (row.TS, row.LongDuree) switch
            {
                (true, _) => Color.LawnGreen,
                (_, true) => Color.Khaki,
                (_, _) => e.Appearance.BackColor
            };
        }
    }

    private async void GridFactures_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {
        if (viewFactures.GetRow(viewFactures.FocusedRowHandle) is FactureDto row)
        {
            await viewDetails.LoadDataAsync(() => ChifaService.Instance.GetFactureDetailsByIdAsync(row.NumFact!));
        }
    }

    private async void LastFacturesUC_Load(object sender, EventArgs e)
    {
        await LoadMaxAndMinDates();
        _ = ReloadFacturesImmediateAsync();
    }

    private async Task LoadMaxAndMinDates()
    {
        try
        {
            await ChifaService.Instance.GetMinAndMaxDatesAsync();
            var lastYear = DateTime.Now.AddYears(-1);
            fromDateRepo.MaxValue = Period.MaxDate;
            fromDateRepo.MinValue = Period.MinDate;
            fromDateRepo.TodayDate = lastYear;

            toDateRepo.MaxValue = Period.MaxDate;
            toDateRepo.MinValue = Period.MinDate;
            toDateRepo.TodayDate = Period.MaxDate;

            txtDateFrom.EditValue = lastYear;
            txtDateTo.EditValue = toDateRepo.MaxValue;

            txtDateFrom.EditValueChanged += TxtDateFrom_EditValueChanged;
            txtDateTo.EditValueChanged += TxtDateTo_EditValueChanged;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void OpenHistoryOfSelectedPatient()
    {
        if (viewFactures.GetRow(viewFactures.FocusedRowHandle) is FactureDto row)
        {
            new FrmHistory(row.NumAssure!, row.Rang!).Show();
        }
    }

    private async Task ScheduleFacturesReloadAsync()
    {
        var cancellationToken = ResetReloadToken();

        try
        {
            await Task.Delay(ReloadDebounceDelay, cancellationToken);
            await LoadFacturesAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task ReloadFacturesImmediateAsync()
    {
        var cancellationToken = ResetReloadToken();
        await LoadFacturesAsync(cancellationToken);
    }

    private CancellationToken ResetReloadToken()
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = new CancellationTokenSource();
        return _reloadCts.Token;
    }

    private async Task LoadFacturesAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        try
        {
            var last = (bool)swtchFactures.EditValue;

            var ts = (bool)swtchTS.EditValue;

            var period = new Period { From = (DateTime?)txtDateFrom.EditValue, To = (DateTime?)txtDateTo.EditValue };

            Expression<Func<Facture, bool>> predicate = f => true;

            var txt = (txtMedic.EditValue as string)?.ToUpperInvariant();

            if (!string.IsNullOrEmpty(txt))
            {
                var patterns = txt.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToArray();
                predicate = patterns.Aggregate(predicate, (current, p) => current.And(f => f.DetailFacts.Any(d => d.Medicament.FullName!.Contains(p))));
            }

            if (cancellationToken.IsCancellationRequested)
                return;

            viewFactures.FocusedRowHandle = 0;
            await factureDtoBindingSource.LoadDataAsync(viewFactures,
                () => ChifaService.Instance.GetAllFacturesAsync(last, ts, period, predicate), cancellationToken);
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

    private async void TxtMedic_EditValueChanged(object sender, EventArgs e) => await ScheduleFacturesReloadAsync();

    private async void TxtDateFrom_EditValueChanged(object? sender, EventArgs e) => await ScheduleFacturesReloadAsync();

    private async void TxtDateTo_EditValueChanged(object? sender, EventArgs e) => await ScheduleFacturesReloadAsync();

    private void ViewFactures_DoubleClick(object sender, EventArgs e)
    {
        OpenHistoryOfSelectedPatient();
    }

    private FactureDto? GetSelectedFactureDto()
        => viewFactures.GetRow(viewFactures.FocusedRowHandle) as FactureDto;

    private async Task ViewFactureAsync()
        => await PrintFactureInternalAsync(avecMajoration: true);

    private async Task PrintFactureAsync(bool avecMajoration)
        => await PrintFactureInternalAsync(avecMajoration);

    private async Task PrintFactureInternalAsync(bool avecMajoration)
    {
        try
        {
            var dto = GetSelectedFactureDto();
            if (dto is null || string.IsNullOrWhiteSpace(dto.NumFact))
            {
                XtraMessageBox.Show("Veuillez sélectionner une facture.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor = Cursors.WaitCursor;
            var facture = await Task.Run(() => ChifaService.Instance.GetFactureForPrint(dto.NumFact!)).ConfigureAwait(true);

            if (facture is null)
            {
                XtraMessageBox.Show($"Facture {dto.NumFact} introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
            var dto = GetSelectedFactureDto();
            if (dto is null || string.IsNullOrWhiteSpace(dto.NumFact))
            {
                XtraMessageBox.Show("Veuillez sélectionner une facture.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor = Cursors.WaitCursor;
            var facture = await Task.Run(() => ChifaService.Instance.GetFactureForPrint(dto.NumFact!)).ConfigureAwait(true);

            if (facture is null)
            {
                XtraMessageBox.Show($"Facture {dto.NumFact} introuvable.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

    private async Task DeleteFactureAsync()
    {
        try
        {
            var dto = GetSelectedFactureDto();
            if (dto is null || string.IsNullOrWhiteSpace(dto.NumFact))
            {
                XtraMessageBox.Show("Veuillez sélectionner une facture.", "Aucune sélection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = XtraMessageBox.Show(
                $"Voulez-vous vraiment supprimer cette facture ?\n\nN° Facture : {dto.NumFact}\nAssuré : {dto.Assure}\nMalade : {dto.Malade}\n\nCette action est irréversible.",
                "Confirmation de suppression",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            Cursor = Cursors.WaitCursor;

            await ChifaService.Instance.DeleteFactureAsync(dto.NumFact!);

            XtraMessageBox.Show($"Facture {dto.NumFact} supprimée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

            await ReloadFacturesImmediateAsync();
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

    private void FacturesUc_Disposed(object? sender, EventArgs e)
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = null;
    }
}

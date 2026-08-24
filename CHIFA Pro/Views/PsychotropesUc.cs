using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class PsychotropesUc : XtraUserControl, INavigable
{
    private static readonly TimeSpan ReloadDebounceDelay = TimeSpan.FromMilliseconds(400);
    private CancellationTokenSource? _reloadCts;
    private readonly Period _period = new();

    public string Caption { get; } = "PSYCHOTROPES";
    public Image Image => FrmMain.Image(8);

    public PsychotropesUc()
    {
        InitializeComponent();
        viewPsychotropes.SetOptions();
        viewPsychotropes.CustomDrawCell += ViewPsychotropes_CustomDrawCell;

        Load += PsychotropesUc_Load;
        Disposed += PsychotropesUc_Disposed;
    }

    private void ViewPsychotropes_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
    {
        if (viewPsychotropes.GetRow(e.RowHandle) is PsychotropeDto row)
        {
            if (row.CategoriePsychotrope != null && row.CategoriePsychotrope.Contains("Abus", StringComparison.OrdinalIgnoreCase))
            {
                if (e.Column.FieldName == "CategoriePsychotrope" || e.Column.FieldName == "MedicamentNom")
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 235, 235);
                    e.Appearance.ForeColor = Color.Crimson;
                }
            }
            else if (row.EstTraitementSpecial)
            {
                e.Appearance.BackColor = Color.FromArgb(235, 255, 235);
            }
            else if (row.DureeTrait >= 60)
            {
                e.Appearance.BackColor = Color.FromArgb(255, 250, 230);
            }
        }
    }

    private async void PsychotropesUc_Load(object? sender, EventArgs e)
    {
        await LoadDateFiltersAsync();
        await ReloadDataAsync(ResetReloadToken());
    }

    private async Task LoadDateFiltersAsync()
    {
        try
        {
            await ChifaService.Instance.GetMinAndMaxDatesAsync();
            var lastYear = DateTime.Today.AddYears(-1);

            repoDateFrom.MinValue = Period.MinDate;
            repoDateFrom.MaxValue = Period.MaxDate;
            repoDateTo.MinValue = Period.MinDate;
            repoDateTo.MaxValue = Period.MaxDate;

            txtDateFrom.EditValue = lastYear;
            txtDateTo.EditValue = Period.MaxDate;

            txtDateFrom.EditValueChanged += TxtDate_EditValueChanged;
            txtDateTo.EditValueChanged += TxtDate_EditValueChanged;
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }

    private void TxtDate_EditValueChanged(object? sender, EventArgs e)
    {
        ScheduleReload();
    }

    private void ScheduleReload()
    {
        var token = ResetReloadToken();

        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(ReloadDebounceDelay, token);
                if (token.IsCancellationRequested) return;

                if (InvokeRequired)
                    Invoke(new Action(async () => await ReloadDataAsync(token)));
                else
                    await ReloadDataAsync(token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { ex.Log(); }
        }, token);
    }

    private CancellationToken ResetReloadToken()
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = new CancellationTokenSource();
        return _reloadCts.Token;
    }

    private async Task ReloadDataAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (cancellationToken.IsCancellationRequested) return;

            _period.From = txtDateFrom.EditValue as DateTime?;
            _period.To = txtDateTo.EditValue as DateTime?;

            Cursor = Cursors.WaitCursor;
            var data = await ScopeService.Instance.GetPsychotropesAsync(_period);

            if (cancellationToken.IsCancellationRequested) return;

            gridPsychotropes.DataSource = data;
            viewPsychotropes.BestFitColumns();

            ConfigureColumns();
            ConfigureSummary();
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

    private void ConfigureColumns()
    {
        if (viewPsychotropes.Columns.Count == 0) return;

        SetColumn("DateDelivrance", "Date Délivrance", 0);
        SetColumn("NumFact", "N° Facture", 1);
        SetColumn("NomAssure", "Assuré", 2);
        SetColumn("NomBeneficiaire", "Bénéficiaire", 3);
        SetColumn("Medecin", "Médecin Prescripteur", 4);
        SetColumn("MedicamentNom", "Médicament", 5);
        SetColumn("Dci", "DCI (Molécule)", 6);
        SetColumn("CodeDci", "Code DCI", 7);
        SetColumn("CategoriePsychotrope", "Classification Réglementaire", 8);
        SetColumn("Dosage", "Dosage", 9);
        SetColumn("Forme", "Forme", 10);
        SetColumn("Quantite", "Quantité", 11);
        SetColumn("DureeTrait", "Durée (Jours)", 12);
        SetColumn("Ppa", "PPA (DA)", 13);
        SetColumn("TarifRef", "Tarif Réf (DA)", 14);

        viewPsychotropes.BestFitColumns();
    }

    private void SetColumn(string fieldName, string caption, int visibleIndex)
    {
        var col = viewPsychotropes.Columns[fieldName];
        if (col != null)
        {
            col.Caption = caption;
            col.VisibleIndex = visibleIndex;
        }
    }

    private void ConfigureSummary()
    {
        viewPsychotropes.OptionsView.ShowFooter = true;
        if (viewPsychotropes.Columns.Count == 0) return;

        var numFactCol = viewPsychotropes.Columns["NumFact"];
        if (numFactCol != null)
        {
            numFactCol.Summary.Clear();
            numFactCol.Summary.Add(DevExpress.Data.SummaryItemType.Count, "NumFact", "Total: {0:N0}");
        }

        var qteCol = viewPsychotropes.Columns["Quantite"];
        if (qteCol != null)
        {
            qteCol.Summary.Clear();
            qteCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Quantite", "Boîtes: {0:N0}");
        }

        var ppaCol = viewPsychotropes.Columns["Ppa"];
        if (ppaCol != null)
        {
            ppaCol.Summary.Clear();
            ppaCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Ppa", "Total: {0:N2} DA");
        }
    }

    private async void BtnRefresh_ItemClick(object sender, ItemClickEventArgs e)
    {
        await ReloadDataAsync(ResetReloadToken());
    }

    private void BtnClearDates_ItemClick(object sender, ItemClickEventArgs e)
    {
        txtDateFrom.EditValue = Period.MinDate;
        txtDateTo.EditValue = Period.MaxDate;
    }

    private void BtnExport_ItemClick(object sender, ItemClickEventArgs e)
    {
        gridPsychotropes.Export("Exporter le Registre des Psychotropes", $"Registre_Psychotropes_{DateTime.Today:yyyyMMdd}");
    }

    private void PsychotropesUc_Disposed(object? sender, EventArgs e)
    {
        _reloadCts?.Cancel();
        _reloadCts?.Dispose();
        _reloadCts = null;
    }
}

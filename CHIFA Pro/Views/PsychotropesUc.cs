using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace CHIFA.Pro.Views;

public partial class PsychotropesUc : XtraUserControl, INavigable
{
    private readonly ReloadCancellation _reloads = new();
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
        await DateFilter.SetUpAsync(txtDateFrom, txtDateTo, DateTime.Today.AddYears(-1), calendarOnFrom: false, _reloads, ReloadDataAsync);
        await ReloadDataAsync(_reloads.Next());
    }

    private async Task ReloadDataAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (cancellationToken.IsCancellationRequested) return;

            _period.From = DateFilter.Value(txtDateFrom.EditValue);
            _period.To = DateFilter.Value(txtDateTo.EditValue);

            Cursor = Cursors.WaitCursor;
            var data = await ScopeService.Instance.GetPsychotropesAsync(_period, cancellationToken: cancellationToken);

            if (cancellationToken.IsCancellationRequested || IsDisposed) return;

            gridPsychotropes.DataSource = data;
            viewPsychotropes.BestFitColumns();

            ConfigureColumns();
            ConfigureSummary();
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
        }
        catch (Exception ex)
        {
            ex.Log();
        }
        finally
        {
            if (!IsDisposed) Cursor = Cursors.Default;
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
        SetColumn("SpecialiteMedecin", "Spécialité", 5);
        SetColumn("MedicamentNom", "Médicament", 6);
        SetColumn("Dci", "DCI (Molécule)", 7);
        SetColumn("CodeDci", "Code DCI", 8);
        SetColumn("CategoriePsychotrope", "Classification Réglementaire", 9);
        SetColumn("Dosage", "Dosage", 10);
        SetColumn("Forme", "Forme", 11);
        SetColumn("Quantite", "Quantité", 12);
        SetColumn("DureeTrait", "Durée (Jours)", 13);
        SetColumn("Ppa", "PPA (DA)", 14);
        SetColumn("TarifRef", "Tarif Réf (DA)", 15);
        SetColumn("Montant", "Montant (DA)", 16);

        // Same value as NumFact: no separate prescription number exists in CHIFA.
        if (viewPsychotropes.Columns["NumeroOrdonnance"] is { } ordonnanceCol)
            ordonnanceCol.Visible = false;

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

        // A sum of unit prices means nothing: total the line amounts instead.
        viewPsychotropes.Columns["Ppa"]?.Summary.Clear();

        var montantCol = viewPsychotropes.Columns["Montant"];
        if (montantCol != null)
        {
            montantCol.Summary.Clear();
            montantCol.Summary.Add(DevExpress.Data.SummaryItemType.Sum, "Montant", "Total: {0:N2} DA");
        }
    }

    private async void BtnRefresh_ItemClick(object sender, ItemClickEventArgs e)
    {
        await ReloadDataAsync(_reloads.Next());
    }

    private void BtnClearDates_ItemClick(object sender, ItemClickEventArgs e)
    {
        txtDateFrom.EditValue = Period.MinDate;
        txtDateTo.EditValue = null; // up to today
    }

    private void BtnExport_ItemClick(object sender, ItemClickEventArgs e)
    {
        gridPsychotropes.Export("Exporter le Registre des Psychotropes", $"Registre_Psychotropes_{DateTime.Today:yyyyMMdd}");
    }

    private void PsychotropesUc_Disposed(object? sender, EventArgs e)
    {
        _reloads.Stop();
    }
}

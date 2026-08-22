using DevExpress.XtraEditors;
using DevExpress.XtraGrid;

namespace CHIFA.Pro.Helpers;

public static class GridExportHelper
{
    private const string ExportFilter = "Fichier Excel (*.xlsx)|*.xlsx|Fichier PDF (*.pdf)|*.pdf";
    private const string SuccessTitle = "Information";
    private const string DefaultSuccessMessage = "Exportation terminée avec succès !";

    public static void Export(this GridControl grid, string title, string defaultFileName,
        string successMessage = DefaultSuccessMessage)
    {
        using var saveDialog = new SaveFileDialog
        {
            Filter = ExportFilter,
            Title = title,
            FileName = defaultFileName,
            OverwritePrompt = true,
            AddExtension = true,
            DefaultExt = "xlsx"
        };

        if (saveDialog.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            if (saveDialog.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                grid.ExportToPdf(saveDialog.FileName);
            else
                grid.ExportToXlsx(saveDialog.FileName);

            XtraMessageBox.Show(successMessage, SuccessTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ex.Log();
        }
    }
}

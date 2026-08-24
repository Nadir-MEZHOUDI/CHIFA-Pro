using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using CHIFA.Pro.Helpers;
using DataModel;
using DevExpress.XtraEditors;
using Microsoft.Reporting.WinForms;
using QRCoder;

namespace CHIFA.Pro.Reports;

public static class FactureReportService
{
    private static Parametre? _officine;
    private static readonly Lock Lock = new();

    private static Parametre? Officine
    {
        get
        {
            if (_officine is null)
            {
                lock (Lock)
                {
                    if (_officine is null)
                    {
                        try
                        {
                            using var db = new ChifaDb();
                            _officine = db.Parametres.FirstOrDefault();
                        }
                        catch (Exception ex)
                        {
                            ex.Log(showMessage: false);
                        }
                    }
                }
            }

            return _officine;
        }
    }

    public static void PrintFacture(Facture facture, bool avecMajoration = true)
    {
        try
        {
            var list = new List<object>();

            foreach (var detail in facture.DetailFacts)
            {
                list.Add(new
                {
                    nom_com = detail.Medicament.NomCom,
                    qte = detail.Qte.ToString(CultureInfo.InvariantCulture),
                    ppa = detail.Ppa.ToString("#,##0.00", CultureInfo.InvariantCulture),
                    montant = detail.Mont.ToString("#,##0.00", CultureInfo.InvariantCulture),
                    montant_as = detail.MontAs?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00",
                    montant_ps = detail.MontPharm?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00"
                });
            }

            var officine = Officine;

            using var preview = new FacturePreviewForm();
            preview.reportViewer1.LocalReport.ReportEmbeddedResource = "CHIFA.Pro.Reports.Facture.rdlc";
            preview.reportViewer1.LocalReport.DataSources.Clear();
            preview.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", list));

            var pOfficine = new ReportParameter("officine", officine is null ? string.Empty : $"{officine.CodePs}   {officine.NomPharmacie}");
            var pAdresse = new ReportParameter("adresse", officine?.Adresse ?? string.Empty);
            var pCentreOfficine = new ReportParameter("centre_officine", facture.CodeCentre ?? string.Empty);
            var pNumFact = new ReportParameter("num_fact", facture.NumFact ?? string.Empty);
            var pDateFact = new ReportParameter("date_fact", facture.DateFact?.ToString("dd/MM/yyyy") ?? string.Empty);
            var pCentreAs = new ReportParameter("centre_as", facture.CodeCentreAs ?? string.Empty);
            var pNomAs = new ReportParameter("nom_as", facture.Assure?.FullName ?? string.Empty);
            var pNumAs = new ReportParameter("num_as", facture.NumAssure ?? string.Empty);
            var pNomBenef = new ReportParameter("nom_benef", facture.Beneficiaire?.FullName ?? string.Empty);
            var pDateSoins = new ReportParameter("date_soins", facture.DateSoin?.ToString("dd/MM/yyyy") ?? string.Empty);
            var pMontTotalFact = new ReportParameter("mont_total_fact", facture.MontFact?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00");
            var pMontTotalAs = new ReportParameter("mont_total_as", facture.MontAs?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00");
            var pMontTotalPs = new ReportParameter("mont_total_ps", facture.MontOff?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00");
            var pMontFae = new ReportParameter("mont_fae", facture.MontMajFae?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00");
            var pTypeMaj = new ReportParameter("type_maj", GetMajStr(facture.TypeMaj));
            var pMontMaj = new ReportParameter("mont_maj", facture.MontMaj?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00");
            var pMontTotalOff = new ReportParameter("mont_total_off", facture.MontGlob?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00");
            var pSansMajoration = new ReportParameter("sans_majoration", avecMajoration ? "false" : "true");

            preview.reportViewer1.LocalReport.SetParameters(
            [
                pOfficine,
                pAdresse,
                pCentreOfficine,
                pNumFact,
                pDateFact,
                pCentreAs,
                pNomAs,
                pNumAs,
                pNomBenef,
                pDateSoins,
                pMontTotalFact,
                pMontTotalAs,
                pMontTotalPs,
                pMontFae,
                pTypeMaj,
                pMontMaj,
                pMontTotalOff,
                pSansMajoration
            ]);

            preview.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            preview.reportViewer1.RefreshReport();
            preview.ShowDialog();
        }
        catch (Exception ex)
        {
            ex.Log();
            XtraMessageBox.Show("Echec d'aperçu de la facture !", "Aperçu facture", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    public static void PrintVerso(Facture facture)
    {
        try
        {
            var list = new List<object>();
            decimal totalQte = 0;
            var text = $"{facture.NumFact};{facture.DateFact?.ToShortDateString()};{facture.NumAssure};{facture.Beneficiaire?.RangAd};{facture.CodeSp};{facture.DateSoin?.ToShortDateString()};";

            foreach (var detail in facture.DetailFacts)
            {
                list.Add(new
                {
                    nom_com = detail.Medicament.NomCom,
                    qte = detail.Qte.ToString(CultureInfo.InvariantCulture),
                    ppa = detail.Ppa.ToString("#,##0.00 DA", CultureInfo.InvariantCulture),
                    montant = detail.Mont.ToString("#,##0.00", CultureInfo.InvariantCulture),
                    montant_as = detail.MontAs?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00",
                    montant_ps = detail.MontPharm?.ToString("#,##0.00", CultureInfo.InvariantCulture) ?? "0.00"
                });
                totalQte += detail.Qte;
                text = string.Concat(text, detail.Medicament.NumEnr, ",", detail.Qte.ToString(CultureInfo.InvariantCulture), ";");
            }

            byte[]? qrBytes = null;
            try
            {
                using var qrImage = QrCodeImage(text);
                using var ms = new MemoryStream();
                qrImage.Save(ms, ImageFormat.Png);
                qrBytes = ms.ToArray();
            }
            catch (Exception ex)
            {
                ex.Log(showMessage: false);
            }

            var dataTable = new DataTable();
            dataTable.Columns.Add("bord_clos", typeof(bool));
            dataTable.Columns.Add("qr_code", typeof(byte[]));

            if (qrBytes is not null && qrBytes.Length > 0)
            {
                dataTable.Rows.Add(true, qrBytes);
            }
            else
            {
                dataTable.Rows.Add(true, DBNull.Value);
            }

            string caisse = facture.CodeCentreAs?.StartsWith("1") == true ? "CNAS"
                : facture.CodeCentreAs?.StartsWith("2") == true ? "CASNOS" : string.Empty;

            using var preview = new FacturePreviewForm();
            preview.reportViewer1.LocalReport.ReportEmbeddedResource = "CHIFA.Pro.Reports.Verso_Ord.rdlc";
            preview.reportViewer1.LocalReport.DataSources.Clear();
            preview.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", list));
            preview.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dataTable));

            var pNumAss = new ReportParameter("num_ass", $"{facture.NumAssure}/{facture.RangAd}");
            var pNumBord = new ReportParameter("num_bord", facture.NumBord ?? string.Empty);
            var pNumFact = new ReportParameter("num_fact", facture.NumFact ?? string.Empty);
            var pCaisse = new ReportParameter("caisse", caisse);
            var pNbVignette = new ReportParameter("nb_vignette", totalQte.ToString(CultureInfo.InvariantCulture));
            var pMontOrd = new ReportParameter("mont_ord", facture.MontFact?.ToString("#,##0.00 DA", CultureInfo.InvariantCulture) ?? "0.00 DA");
            var pMontOff = new ReportParameter("mont_off", facture.MontGlob?.ToString("#,##0.00 DA", CultureInfo.InvariantCulture) ?? "0.00 DA");

            preview.reportViewer1.LocalReport.SetParameters(
            [
                pNumAss,
                pNumBord,
                pNumFact,
                pCaisse,
                pNbVignette,
                pMontOrd,
                pMontOff
            ]);

            preview.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            preview.reportViewer1.RefreshReport();
            preview.ShowDialog();
        }
        catch (Exception ex)
        {
            ex.Log();
            XtraMessageBox.Show("Echec d'aperçu du verso de l'ordonnance !", "Aperçu verso de l'ordonnance", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    private static Image QrCodeImage(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException(nameof(text));
        }

        using var generator = new QRCodeGenerator();
        var data = generator.CreateQrCode(text.Trim(), QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new QRCode(data);
        return qrCode.GetGraphic(20, Color.Black, Color.White, true);
    }

    private static string GetMajStr(int typeMaj)
    {
        var officine = Officine;
        return typeMaj switch
        {
            0 => "Aucune Majoration",
            1 => $"Majoration de {officine?.MontMajSub} DA par Produit Substitué",
            2 => $"Majoration de {officine?.TauxMajInfTr} %",
            3 => $"Majoration de {officine?.TauxMajLocal} % par Produit Local",
            _ => string.Empty
        };
    }

    public static void ClearOfficineCache()
    {
        lock (Lock)
        {
            _officine = null;
        }
    }
}

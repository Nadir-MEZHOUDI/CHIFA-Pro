
namespace CHIFA.Pro.Views
{
    partial class BordereauxUc
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            FormatConditionRuleIconSet formatConditionRuleIconSet1 = new FormatConditionRuleIconSet();
            FormatConditionIconSet formatConditionIconSet1 = new FormatConditionIconSet();
            FormatConditionIconSetIcon formatConditionIconSetIcon1 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon formatConditionIconSetIcon2 = new FormatConditionIconSetIcon();
            FormatConditionIconSetIcon formatConditionIconSetIcon3 = new FormatConditionIconSetIcon();
            colEtat = new DevExpress.XtraGrid.Columns.GridColumn();
            gridBordereaux = new DevExpress.XtraGrid.GridControl();
            bordereauDtoBindingSource = new BindingSource(components);
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colNum = new DevExpress.XtraGrid.Columns.GridColumn();
            colCenter = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateGen = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateExtraction = new DevExpress.XtraGrid.Columns.GridColumn();
            colNmbr = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontOff = new DevExpress.XtraGrid.Columns.GridColumn();
            colMaj = new DevExpress.XtraGrid.Columns.GridColumn();
            colMFAE = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontGlobal = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colTauxRejet = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontantRejete = new DevExpress.XtraGrid.Columns.GridColumn();
            colStatutRejet = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontAss = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)gridBordereaux).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bordereauDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            SuspendLayout();
            // 
            // colEtat
            // 
            colEtat.FieldName = "State";
            colEtat.MinWidth = 15;
            colEtat.Name = "colEtat";
            colEtat.Visible = true;
            colEtat.VisibleIndex = 5;
            colEtat.Width = 68;
            // 
            // gridBordereaux
            // 
            gridBordereaux.DataSource = bordereauDtoBindingSource;
            gridBordereaux.Dock = DockStyle.Fill;
            gridBordereaux.EmbeddedNavigator.Margin = new Padding(4, 3, 4, 3);
            gridBordereaux.Location = new Point(0, 0);
            gridBordereaux.MainView = gridView1;
            gridBordereaux.Margin = new Padding(4, 3, 4, 3);
            gridBordereaux.Name = "gridBordereaux";
            gridBordereaux.Size = new Size(1362, 576);
            gridBordereaux.TabIndex = 0;
            gridBordereaux.ViewCollection.AddRange(new BaseView[] { gridView1 });
            // 
            // bordereauDtoBindingSource
            // 
            bordereauDtoBindingSource.DataSource = typeof(BordereauDto);
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colNum, colCenter, colDateGen, colDateExtraction, colNmbr, colEtat, colMontOff, colMaj, colMFAE, colMontGlobal, gridColumn1, colTauxRejet, colMontantRejete, colStatutRejet, gridColumn3, colMontAss });
            gridView1.DetailHeight = 371;
            gridFormatRule1.Column = colEtat;
            gridFormatRule1.Name = "Format0";
            formatConditionIconSet1.CategoryName = "Symbols";
            formatConditionIconSetIcon1.PredefinedName = "Symbols23_1.png";
            formatConditionIconSetIcon1.Value = new decimal(new int[] { 2, 0, 0, 0 });
            formatConditionIconSetIcon1.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSetIcon2.PredefinedName = "Symbols23_2.png";
            formatConditionIconSetIcon2.Value = new decimal(new int[] { 1, 0, 0, 0 });
            formatConditionIconSetIcon2.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSetIcon3.PredefinedName = "Symbols23_3.png";
            formatConditionIconSetIcon3.ValueComparison = FormatConditionComparisonType.GreaterOrEqual;
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon1);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon2);
            formatConditionIconSet1.Icons.Add(formatConditionIconSetIcon3);
            formatConditionIconSet1.Name = "Symbols3Circled";
            formatConditionIconSet1.ValueType = FormatConditionValueType.Number;
            formatConditionRuleIconSet1.IconSet = formatConditionIconSet1;
            gridFormatRule1.Rule = formatConditionRuleIconSet1;
            gridView1.FormatRules.Add(gridFormatRule1);
            gridView1.GridControl = gridBordereaux;
            gridView1.Name = "gridView1";
            gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            gridView1.OptionsEditForm.PopupEditFormWidth = 581;
            gridView1.OptionsMenu.ShowGroupSummaryEditorItem = true;
            gridView1.OptionsMenu.ShowSummaryItemMode = DevExpress.Utils.DefaultBoolean.True;
            gridView1.OptionsPrint.ExpandAllDetails = true;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsView.ShowFooter = true;
            // 
            // colNum
            // 
            colNum.AppearanceCell.BackColor = Color.FromArgb(255, 224, 192);
            colNum.AppearanceCell.Options.UseBackColor = true;
            colNum.FieldName = "Num";
            colNum.MinWidth = 15;
            colNum.Name = "colNum";
            colNum.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Num", "{0}") });
            colNum.Visible = true;
            colNum.VisibleIndex = 1;
            colNum.Width = 100;
            // 
            // colCenter
            // 
            colCenter.FieldName = "Center";
            colCenter.MinWidth = 15;
            colCenter.Name = "colCenter";
            colCenter.Visible = true;
            colCenter.VisibleIndex = 0;
            colCenter.Width = 64;
            // 
            // colDateGen
            // 
            colDateGen.Caption = "Ouverture";
            colDateGen.DisplayFormat.FormatString = "dd/MM/yyyy";
            colDateGen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colDateGen.FieldName = "FirstFacture";
            colDateGen.MinWidth = 15;
            colDateGen.Name = "colDateGen";
            colDateGen.Visible = true;
            colDateGen.VisibleIndex = 3;
            colDateGen.Width = 100;
            // 
            // colDateExtraction
            // 
            colDateExtraction.AppearanceCell.BackColor = Color.FromArgb(255, 255, 192);
            colDateExtraction.AppearanceCell.Options.UseBackColor = true;
            colDateExtraction.Caption = "Clôture";
            colDateExtraction.DisplayFormat.FormatString = "dd/MM/yyyy";
            colDateExtraction.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colDateExtraction.FieldName = "LastFacture";
            colDateExtraction.MinWidth = 15;
            colDateExtraction.Name = "colDateExtraction";
            colDateExtraction.Visible = true;
            colDateExtraction.VisibleIndex = 4;
            colDateExtraction.Width = 108;
            // 
            // colNmbr
            // 
            colNmbr.FieldName = "Nmbr";
            colNmbr.MinWidth = 15;
            colNmbr.Name = "colNmbr";
            colNmbr.Visible = true;
            colNmbr.VisibleIndex = 2;
            colNmbr.Width = 57;
            // 
            // colMontOff
            // 
            colMontOff.DisplayFormat.FormatString = "n";
            colMontOff.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontOff.FieldName = "MontOff";
            colMontOff.MinWidth = 15;
            colMontOff.Name = "colMontOff";
            colMontOff.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MontOff", "{0:N2}") });
            colMontOff.Visible = true;
            colMontOff.VisibleIndex = 6;
            colMontOff.Width = 60;
            // 
            // colMaj
            // 
            colMaj.DisplayFormat.FormatString = "n";
            colMaj.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMaj.FieldName = "Maj";
            colMaj.MinWidth = 15;
            colMaj.Name = "colMaj";
            colMaj.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Maj", "{0:N2}") });
            colMaj.Visible = true;
            colMaj.VisibleIndex = 7;
            colMaj.Width = 65;
            // 
            // colMFAE
            // 
            colMFAE.DisplayFormat.FormatString = "n";
            colMFAE.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMFAE.FieldName = "MFAE";
            colMFAE.MinWidth = 15;
            colMFAE.Name = "colMFAE";
            colMFAE.Visible = true;
            colMFAE.VisibleIndex = 9;
            colMFAE.Width = 65;
            // 
            // colMontGlobal
            // 
            colMontGlobal.AppearanceCell.BackColor = Color.FromArgb(192, 255, 192);
            colMontGlobal.AppearanceCell.Options.UseBackColor = true;
            colMontGlobal.DisplayFormat.FormatString = "n";
            colMontGlobal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontGlobal.FieldName = "MontGlobal";
            colMontGlobal.MinWidth = 15;
            colMontGlobal.Name = "colMontGlobal";
            colMontGlobal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, DevExpress.Data.SummaryMode.Mixed, "MontGlobal", "{0:N2}") });
            colMontGlobal.Visible = true;
            colMontGlobal.VisibleIndex = 10;
            colMontGlobal.Width = 135;
            // 
            // gridColumn1
            // 
            gridColumn1.FieldName = "Virment";
            gridColumn1.MinWidth = 19;
            gridColumn1.Name = "gridColumn1";
            gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Virment", "{0:N2}") });
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 11;
            gridColumn1.Width = 69;
            // 
            // colTauxRejet
            // 
            colTauxRejet.Caption = "Taux %";
            colTauxRejet.DisplayFormat.FormatString = "p2";
            colTauxRejet.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colTauxRejet.FieldName = "TauxRejet";
            colTauxRejet.MinWidth = 19;
            colTauxRejet.Name = "colTauxRejet";
            colTauxRejet.Visible = true;
            colTauxRejet.VisibleIndex = 13;
            colTauxRejet.Width = 60;
            // 
            // colMontantRejete
            // 
            colMontantRejete.Caption = "Montant Rejeté";
            colMontantRejete.DisplayFormat.FormatString = "n2";
            colMontantRejete.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontantRejete.FieldName = "MontantRejete";
            colMontantRejete.MinWidth = 19;
            colMontantRejete.Name = "colMontantRejete";
            colMontantRejete.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MontantRejete", "{0:N2}") });
            colMontantRejete.Visible = true;
            colMontantRejete.VisibleIndex = 12;
            colMontantRejete.Width = 90;
            // 
            // colStatutRejet
            // 
            colStatutRejet.Caption = "Statut Rejet";
            colStatutRejet.FieldName = "StatutRejet";
            colStatutRejet.MinWidth = 19;
            colStatutRejet.Name = "colStatutRejet";
            colStatutRejet.Visible = true;
            colStatutRejet.VisibleIndex = 14;
            colStatutRejet.Width = 110;
            // 
            // gridColumn3
            // 
            gridColumn3.FieldName = "DepotFtp";
            gridColumn3.Name = "gridColumn3";
            gridColumn3.Visible = true;
            gridColumn3.VisibleIndex = 15;
            // 
            // colMontAss
            // 
            colMontAss.Caption = "Mont Assure";
            colMontAss.DisplayFormat.FormatString = "n2";
            colMontAss.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontAss.FieldName = "MontAss";
            colMontAss.Name = "colMontAss";
            colMontAss.Visible = true;
            colMontAss.VisibleIndex = 8;
            // 
            // BordereauxUc
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridBordereaux);
            Margin = new Padding(4, 3, 4, 3);
            Name = "BordereauxUc";
            Size = new Size(1362, 576);
            ((System.ComponentModel.ISupportInitialize)gridBordereaux).EndInit();
            ((System.ComponentModel.ISupportInitialize)bordereauDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colNum;
        private DevExpress.XtraGrid.Columns.GridColumn colCenter;
        private DevExpress.XtraGrid.Columns.GridColumn colDateGen;
        private DevExpress.XtraGrid.Columns.GridColumn colDateExtraction;
        private DevExpress.XtraGrid.Columns.GridColumn colEtat;
        private DevExpress.XtraGrid.Columns.GridColumn colMaj;
        private DevExpress.XtraGrid.Columns.GridColumn colMontGlobal;
        private DevExpress.XtraGrid.Columns.GridColumn colMontOff;
        private DevExpress.XtraGrid.Columns.GridColumn colNmbr;
        private DevExpress.XtraGrid.Columns.GridColumn colMFAE;
        public DevExpress.XtraGrid.GridControl gridBordereaux;
        private BindingSource bordereauDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn colMontantRejete;
        private DevExpress.XtraGrid.Columns.GridColumn colTauxRejet;
        private DevExpress.XtraGrid.Columns.GridColumn colStatutRejet;
        private DevExpress.XtraGrid.Columns.GridColumn colMontAss;
    }
}

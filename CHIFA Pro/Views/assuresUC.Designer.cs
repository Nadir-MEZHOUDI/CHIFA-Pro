namespace CHIFA.Pro
{
    partial class AssuresUc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssuresUc));
            colRang = new DevExpress.XtraGrid.Columns.GridColumn();
            colFactures = new DevExpress.XtraGrid.Columns.GridColumn();
            gridAssures = new DevExpress.XtraGrid.GridControl();
            beneficiareDtoBindingSource = new BindingSource(components);
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colCenter = new DevExpress.XtraGrid.Columns.GridColumn();
            colNumAssure = new DevExpress.XtraGrid.Columns.GridColumn();
            colBeneficiare = new DevExpress.XtraGrid.Columns.GridColumn();
            colAssure = new DevExpress.XtraGrid.Columns.GridColumn();
            ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            barEditItem2 = new DevExpress.XtraBars.BarEditItem();
            swtchAllFactures = new DevExpress.XtraBars.BarEditItem();
            txtDateFrom = new DevExpress.XtraBars.BarEditItem();
            txtDateTo = new DevExpress.XtraBars.BarEditItem();
            txtMedic = new DevExpress.XtraBars.BarEditItem();
            btnConsumption = new DevExpress.XtraBars.BarButtonItem();
            btnHistory = new DevExpress.XtraBars.BarButtonItem();
            swtchTimeOnly = new DevExpress.XtraBars.BarEditItem();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)gridAssures).BeginInit();
            ((System.ComponentModel.ISupportInitialize)beneficiareDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).BeginInit();
            SuspendLayout();
            // 
            // colRang
            // 
            colRang.AppearanceCell.Options.UseTextOptions = true;
            colRang.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            colRang.FieldName = "Rang";
            colRang.MinWidth = 11;
            colRang.Name = "colRang";
            colRang.Visible = true;
            colRang.VisibleIndex = 4;
            colRang.Width = 46;
            // 
            // colFactures
            // 
            colFactures.FieldName = "Factures";
            colFactures.MinWidth = 11;
            colFactures.Name = "colFactures";
            colFactures.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Factures", "SUM={0:0.##}") });
            colFactures.Width = 40;
            // 
            // gridAssures
            // 
            gridAssures.DataSource = beneficiareDtoBindingSource;
            gridAssures.Dock = DockStyle.Fill;
            gridAssures.EmbeddedNavigator.Margin = new Padding(3, 2, 3, 2);
            gridAssures.Location = new Point(0, 86);
            gridAssures.MainView = gridView1;
            gridAssures.Margin = new Padding(3, 2, 3, 2);
            gridAssures.Name = "gridAssures";
            gridAssures.Size = new Size(1269, 449);
            gridAssures.TabIndex = 0;
            gridAssures.ViewCollection.AddRange(new BaseView[] { gridView1 });
            // 
            // beneficiareDtoBindingSource
            // 
            beneficiareDtoBindingSource.DataSource = typeof(BeneficiareDto);
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colCenter, colNumAssure, colBeneficiare, colAssure, colRang, colFactures });
            gridView1.DetailHeight = 284;
            gridView1.FixedLineWidth = 3;
            gridFormatRule1.Name = "Format0";
            gridFormatRule1.Rule = null;
            gridView1.FormatRules.Add(gridFormatRule1);
            gridView1.GridControl = gridAssures;
            gridView1.Name = "gridView1";
            gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsCustomization.CustomizationFormSearchBoxVisible = true;
            gridView1.OptionsEditForm.PopupEditFormWidth = 436;
            gridView1.OptionsFind.AlwaysVisible = true;
            gridView1.OptionsFind.SearchInPreview = true;
            gridView1.OptionsMenu.ShowGroupSummaryEditorItem = true;
            gridView1.OptionsPrint.ExpandAllDetails = true;
            gridView1.OptionsView.ShowFooter = true;
            gridView1.DoubleClick += gridView1_DoubleClick;
            // 
            // colCenter
            // 
            colCenter.FieldName = "Center";
            colCenter.ImageOptions.Image = (Image)resources.GetObject("colCenter.ImageOptions.Image");
            colCenter.MinWidth = 14;
            colCenter.Name = "colCenter";
            colCenter.Visible = true;
            colCenter.VisibleIndex = 0;
            colCenter.Width = 51;
            // 
            // colNumAssure
            // 
            colNumAssure.AppearanceCell.BackColor = Color.FromArgb(255, 255, 192);
            colNumAssure.AppearanceCell.Options.UseBackColor = true;
            colNumAssure.FieldName = "NumAssure";
            colNumAssure.ImageOptions.Image = (Image)resources.GetObject("colNumAssure.ImageOptions.Image");
            colNumAssure.MinWidth = 11;
            colNumAssure.Name = "colNumAssure";
            colNumAssure.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "NumAssure", "{0}") });
            colNumAssure.Visible = true;
            colNumAssure.VisibleIndex = 1;
            colNumAssure.Width = 110;
            // 
            // colBeneficiare
            // 
            colBeneficiare.AppearanceCell.BackColor = Color.FromArgb(192, 255, 192);
            colBeneficiare.AppearanceCell.Options.UseBackColor = true;
            colBeneficiare.FieldName = "Beneficiare";
            colBeneficiare.ImageOptions.Image = (Image)resources.GetObject("colBeneficiare.ImageOptions.Image");
            colBeneficiare.MinWidth = 11;
            colBeneficiare.Name = "colBeneficiare";
            colBeneficiare.Visible = true;
            colBeneficiare.VisibleIndex = 2;
            colBeneficiare.Width = 174;
            // 
            // colAssure
            // 
            colAssure.AppearanceCell.BackColor = Color.FromArgb(255, 224, 192);
            colAssure.AppearanceCell.Options.UseBackColor = true;
            colAssure.FieldName = "Assure";
            colAssure.ImageOptions.Image = (Image)resources.GetObject("colAssure.ImageOptions.Image");
            colAssure.MinWidth = 11;
            colAssure.Name = "colAssure";
            colAssure.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Assure", "{0}") });
            colAssure.Visible = true;
            colAssure.VisibleIndex = 3;
            colAssure.Width = 178;
            // 
            // ribbonControl1
            // 
            ribbonControl1.AllowMinimizeRibbon = false;
            ribbonControl1.AutoSizeItems = true;
            ribbonControl1.DrawGroupCaptions = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.DrawGroupsBorderMode = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.EmptyAreaImageOptions.ImagePadding = new Padding(18);
            ribbonControl1.ExpandCollapseItem.Id = 0;
            ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbonControl1.ExpandCollapseItem, barEditItem1, barEditItem2, swtchAllFactures, txtDateFrom, txtDateTo, txtMedic, btnConsumption, btnHistory, swtchTimeOnly });
            ribbonControl1.Location = new Point(0, 0);
            ribbonControl1.Margin = new Padding(2);
            ribbonControl1.MaxItemId = 16;
            ribbonControl1.Name = "ribbonControl1";
            ribbonControl1.OptionsMenuMinWidth = 152;
            ribbonControl1.OptionsPageCategories.ShowCaptions = false;
            ribbonControl1.OptionsTouch.ShowTouchUISelectorInSearchMenu = false;
            ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { ribbonPage1 });
            ribbonControl1.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonControlStyle.MacOffice;
            ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            ribbonControl1.ShowQatLocationSelector = false;
            ribbonControl1.ShowToolbarCustomizeItem = false;
            ribbonControl1.Size = new Size(1269, 86);
            ribbonControl1.Toolbar.ShowCustomizeItem = false;
            ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // barEditItem1
            // 
            barEditItem1.Caption = "From:";
            barEditItem1.Edit = null;
            barEditItem1.EditWidth = 100;
            barEditItem1.Id = 4;
            barEditItem1.Name = "barEditItem1";
            // 
            // barEditItem2
            // 
            barEditItem2.Caption = "To   :";
            barEditItem2.Edit = null;
            barEditItem2.EditWidth = 100;
            barEditItem2.Id = 5;
            barEditItem2.Name = "barEditItem2";
            // 
            // swtchAllFactures
            // 
            swtchAllFactures.Edit = null;
            swtchAllFactures.Id = 7;
            swtchAllFactures.ImageOptions.Image = (Image)resources.GetObject("swtchAllFactures.ImageOptions.Image");
            swtchAllFactures.ImageOptions.LargeImage = (Image)resources.GetObject("swtchAllFactures.ImageOptions.LargeImage");
            swtchAllFactures.Name = "swtchAllFactures";
            // 
            // txtDateFrom
            // 
            txtDateFrom.Caption = "From: ";
            txtDateFrom.Edit = null;
            txtDateFrom.EditWidth = 120;
            txtDateFrom.Id = 8;
            txtDateFrom.Name = "txtDateFrom";
            // 
            // txtDateTo
            // 
            txtDateTo.Caption = "To:";
            txtDateTo.Edit = null;
            txtDateTo.EditWidth = 120;
            txtDateTo.Id = 9;
            txtDateTo.Name = "txtDateTo";
            txtDateTo.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            // 
            // txtMedic
            // 
            txtMedic.Caption = "Medicament:";
            txtMedic.Edit = null;
            txtMedic.EditWidth = 150;
            txtMedic.Id = 12;
            txtMedic.Name = "txtMedic";
            // 
            // btnConsumption
            // 
            btnConsumption.Caption = "Consumption";
            btnConsumption.Id = 13;
            btnConsumption.ImageOptions.Image = (Image)resources.GetObject("btnConsumption.ImageOptions.Image");
            btnConsumption.ImageOptions.LargeImage = (Image)resources.GetObject("btnConsumption.ImageOptions.LargeImage");
            btnConsumption.Name = "btnConsumption";
            btnConsumption.ItemClick += btnConsumption_ItemClick;
            // 
            // btnHistory
            // 
            btnHistory.Caption = "History";
            btnHistory.Id = 14;
            btnHistory.ImageOptions.Image = (Image)resources.GetObject("btnHistory.ImageOptions.Image");
            btnHistory.ImageOptions.LargeImage = (Image)resources.GetObject("btnHistory.ImageOptions.LargeImage");
            btnHistory.Name = "btnHistory";
            // 
            // swtchTimeOnly
            // 
            swtchTimeOnly.Edit = null;
            swtchTimeOnly.EditValue = false;
            swtchTimeOnly.Id = 15;
            swtchTimeOnly.Name = "swtchTimeOnly";
            // 
            // ribbonPage1
            // 
            ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroup3 });
            ribbonPage1.Name = "ribbonPage1";
            // 
            // ribbonPageGroup3
            // 
            ribbonPageGroup3.ItemLinks.Add(btnHistory);
            ribbonPageGroup3.ItemLinks.Add(btnConsumption);
            ribbonPageGroup3.Name = "ribbonPageGroup3";
            ribbonPageGroup3.Text = "ribbonPageGroup3";
            // 
            // AssuresUc
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridAssures);
            Controls.Add(ribbonControl1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "AssuresUc";
            Size = new Size(1269, 535);
            ((System.ComponentModel.ISupportInitialize)gridAssures).EndInit();
            ((System.ComponentModel.ISupportInitialize)beneficiareDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridAssures;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colAssure;
        private DevExpress.XtraGrid.Columns.GridColumn colBeneficiare;
        private DevExpress.XtraGrid.Columns.GridColumn colFactures;
        private DevExpress.XtraGrid.Columns.GridColumn colNumAssure;
        private DevExpress.XtraGrid.Columns.GridColumn colRang;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraBars.BarEditItem barEditItem2;
        private DevExpress.XtraBars.BarEditItem swtchAllFactures;
        private DevExpress.XtraBars.BarEditItem txtDateFrom;
        private DevExpress.XtraBars.BarEditItem txtDateTo;
        private DevExpress.XtraBars.BarEditItem txtMedic;
        private DevExpress.XtraBars.BarButtonItem btnConsumption;
        private DevExpress.XtraBars.BarButtonItem btnHistory;
        private DevExpress.XtraBars.BarEditItem swtchTimeOnly;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private BindingSource beneficiareDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colCenter;
    }
}

namespace CHIFA.Pro.Views
{
    partial class ScopeDashboardUc
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScopeDashboardUc));
            ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            txtDateFrom = new DevExpress.XtraBars.BarEditItem();
            repoDateFrom = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            txtDateTo = new DevExpress.XtraBars.BarEditItem();
            repoDateTo = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            btnClearDates = new DevExpress.XtraBars.BarButtonItem();
            btnAllPeriod = new DevExpress.XtraBars.BarButtonItem();
            btnLastYear = new DevExpress.XtraBars.BarButtonItem();
            btn6Months = new DevExpress.XtraBars.BarButtonItem();
            btnThisYear = new DevExpress.XtraBars.BarButtonItem();
            btnThisMonth = new DevExpress.XtraBars.BarButtonItem();
            ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            ribbonPageGroupPeriod = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            PeriodRange = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            panelHeader = new PanelControl();
            lblSubtitle = new LabelControl();
            lblTitle = new LabelControl();
            panelKpis = new PanelControl();
            pnlKpiTaux = new PanelControl();
            lblTauxPriseEnChargeVal = new LabelControl();
            lblTauxPriseEnChargeTitle = new LabelControl();
            pnlKpiCasnos = new PanelControl();
            lblCasnosVal = new LabelControl();
            lblCasnosTitle = new LabelControl();
            pnlKpiCnas = new PanelControl();
            lblCnasVal = new LabelControl();
            lblCnasTitle = new LabelControl();
            pnlKpiBoites = new PanelControl();
            lblBoitesVal = new LabelControl();
            lblBoitesTitle = new LabelControl();
            pnlKpiFactures = new PanelControl();
            lblFactVal = new LabelControl();
            lblFactTitle = new LabelControl();
            pnlKpiCa = new PanelControl();
            lblCaVal = new LabelControl();
            lblCaTitle = new LabelControl();
            splitContainerCharts = new SplitContainerControl();
            chartHourly = new DevExpress.XtraCharts.ChartControl();
            chartTopProducts = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).BeginInit();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelKpis).BeginInit();
            panelKpis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiTaux).BeginInit();
            pnlKpiTaux.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCasnos).BeginInit();
            pnlKpiCasnos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCnas).BeginInit();
            pnlKpiCnas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiBoites).BeginInit();
            pnlKpiBoites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiFactures).BeginInit();
            pnlKpiFactures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCa).BeginInit();
            pnlKpiCa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerCharts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerCharts.Panel1).BeginInit();
            splitContainerCharts.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerCharts.Panel2).BeginInit();
            splitContainerCharts.Panel2.SuspendLayout();
            splitContainerCharts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartHourly).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartTopProducts).BeginInit();
            SuspendLayout();
            // 
            // ribbonControl1
            // 
            ribbonControl1.AllowMinimizeRibbon = false;
            ribbonControl1.AutoSizeItems = true;
            ribbonControl1.DrawGroupCaptions = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.DrawGroupsBorderMode = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.EmptyAreaImageOptions.ImagePadding = new Padding(23, 22, 23, 22);
            ribbonControl1.ExpandCollapseItem.Id = 0;
            ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { ribbonControl1.ExpandCollapseItem, btnRefresh, txtDateFrom, txtDateTo, btnClearDates, btnAllPeriod, btnLastYear, btn6Months, btnThisYear, btnThisMonth });
            ribbonControl1.Location = new Point(0, 0);
            ribbonControl1.Margin = new Padding(4);
            ribbonControl1.MaxItemId = 9;
            ribbonControl1.Name = "ribbonControl1";
            ribbonControl1.OptionsMenuMinWidth = 424;
            ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] { ribbonPage1 });
            ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repoDateFrom, repoDateTo });
            ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.False;
            ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            ribbonControl1.ShowToolbarCustomizeItem = false;
            ribbonControl1.Size = new Size(1479, 134);
            ribbonControl1.Toolbar.ShowCustomizeItem = false;
            ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // btnRefresh
            // 
            btnRefresh.Caption = "Actualiser";
            btnRefresh.Id = 0;
            btnRefresh.ImageOptions.Image = (Image)resources.GetObject("btnRefresh.ImageOptions.Image");
            btnRefresh.Name = "btnRefresh";
            btnRefresh.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            btnRefresh.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            btnRefresh.ItemClick += BtnRefresh_ItemClick;
            // 
            // txtDateFrom
            // 
            txtDateFrom.Caption = "Du :";
            txtDateFrom.Edit = repoDateFrom;
            txtDateFrom.EditWidth = 120;
            txtDateFrom.Id = 1;
            txtDateFrom.Name = "txtDateFrom";
            txtDateFrom.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            // 
            // repoDateFrom
            // 
            repoDateFrom.AutoHeight = false;
            repoDateFrom.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repoDateFrom.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repoDateFrom.Name = "repoDateFrom";
            // 
            // txtDateTo
            // 
            txtDateTo.Caption = "Au :";
            txtDateTo.Edit = repoDateTo;
            txtDateTo.EditWidth = 120;
            txtDateTo.Id = 2;
            txtDateTo.Name = "txtDateTo";
            txtDateTo.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
            // 
            // repoDateTo
            // 
            repoDateTo.AutoHeight = false;
            repoDateTo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repoDateTo.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repoDateTo.Name = "repoDateTo";
            // 
            // btnClearDates
            // 
            btnClearDates.Caption = "Tout l'historique";
            btnClearDates.Id = 3;
            btnClearDates.ImageOptions.Image = (Image)resources.GetObject("btnClearDates.ImageOptions.Image");
            btnClearDates.ImageOptions.LargeImage = (Image)resources.GetObject("btnClearDates.ImageOptions.LargeImage");
            btnClearDates.Name = "btnClearDates";
            btnClearDates.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            btnClearDates.ItemClick += BtnClearDates_ItemClick;
            // 
            // btnAllPeriod
            // 
            btnAllPeriod.Caption = "Toutes";
            btnAllPeriod.Id = 4;
            btnAllPeriod.ImageOptions.LargeImage = (Image)resources.GetObject("btnAllPeriod.ImageOptions.LargeImage");
            btnAllPeriod.Name = "btnAllPeriod";
            btnAllPeriod.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            btnAllPeriod.ItemClick += BtnAllPeriod_ItemClick;
            // 
            // btnLastYear
            // 
            btnLastYear.Caption = "1 An";
            btnLastYear.Id = 5;
            btnLastYear.ImageOptions.LargeImage = (Image)resources.GetObject("btnLastYear.ImageOptions.LargeImage");
            btnLastYear.Name = "btnLastYear";
            btnLastYear.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            btnLastYear.ItemClick += BtnLastYear_ItemClick;
            // 
            // btn6Months
            // 
            btn6Months.Caption = "6 Mois";
            btn6Months.Id = 6;
            btn6Months.ImageOptions.LargeImage = (Image)resources.GetObject("btn6Months.ImageOptions.LargeImage");
            btn6Months.Name = "btn6Months";
            btn6Months.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            btn6Months.ItemClick += Btn6Months_ItemClick;
            // 
            // btnThisYear
            // 
            btnThisYear.Caption = "Cette Année";
            btnThisYear.Id = 7;
            btnThisYear.ImageOptions.LargeImage = (Image)resources.GetObject("btnThisYear.ImageOptions.LargeImage");
            btnThisYear.Name = "btnThisYear";
            btnThisYear.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            btnThisYear.ItemClick += BtnThisYear_ItemClick;
            // 
            // btnThisMonth
            // 
            btnThisMonth.Caption = "Ce Mois";
            btnThisMonth.Id = 8;
            btnThisMonth.ImageOptions.LargeImage = (Image)resources.GetObject("btnThisMonth.ImageOptions.LargeImage");
            btnThisMonth.Name = "btnThisMonth";
            btnThisMonth.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            btnThisMonth.ItemClick += BtnThisMonth_ItemClick;
            // 
            // ribbonPage1
            // 
            ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] { ribbonPageGroupPeriod, PeriodRange });
            ribbonPage1.Name = "ribbonPage1";
            // 
            // ribbonPageGroupPeriod
            // 
            ribbonPageGroupPeriod.ItemLinks.Add(btnRefresh);
            ribbonPageGroupPeriod.ItemLinks.Add(txtDateFrom);
            ribbonPageGroupPeriod.ItemLinks.Add(txtDateTo);
            ribbonPageGroupPeriod.ItemLinks.Add(btnClearDates);
            ribbonPageGroupPeriod.Name = "ribbonPageGroupPeriod";
            ribbonPageGroupPeriod.Text = "Période";
            // 
            // PeriodRange
            // 
            PeriodRange.ItemLinks.Add(btnAllPeriod);
            PeriodRange.ItemLinks.Add(btnLastYear);
            PeriodRange.ItemLinks.Add(btn6Months);
            PeriodRange.ItemLinks.Add(btnThisYear);
            PeriodRange.ItemLinks.Add(btnThisMonth);
            PeriodRange.Name = "PeriodRange";
            PeriodRange.Text = "Périodes";
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 134);
            panelHeader.Margin = new Padding(4);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(15, 10, 15, 10);
            panelHeader.Size = new Size(1479, 64);
            panelHeader.TabIndex = 4;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Appearance.Font = new Font("Segoe UI", 8.25F);
            lblSubtitle.Appearance.ForeColor = Color.Gray;
            lblSubtitle.Appearance.Options.UseFont = true;
            lblSubtitle.Appearance.Options.UseForeColor = true;
            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Location = new Point(17, 40);
            lblSubtitle.Margin = new Padding(4);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(479, 19);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Surveillance en temps réel des indicateurs officine, flux horaires et prescriptions";
            // 
            // lblTitle
            // 
            lblTitle.Appearance.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.Appearance.ForeColor = Color.DarkSlateBlue;
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Appearance.Options.UseForeColor = true;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Location = new Point(17, 12);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(340, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "CHIFA SCOPE - TOUR DE CONTRÔLE";
            // 
            // panelKpis
            // 
            panelKpis.Controls.Add(pnlKpiTaux);
            panelKpis.Controls.Add(pnlKpiCasnos);
            panelKpis.Controls.Add(pnlKpiCnas);
            panelKpis.Controls.Add(pnlKpiBoites);
            panelKpis.Controls.Add(pnlKpiFactures);
            panelKpis.Controls.Add(pnlKpiCa);
            panelKpis.Dock = DockStyle.Top;
            panelKpis.Location = new Point(0, 198);
            panelKpis.Margin = new Padding(4);
            panelKpis.Name = "panelKpis";
            panelKpis.Padding = new Padding(8, 7, 8, 7);
            panelKpis.Size = new Size(1479, 93);
            panelKpis.TabIndex = 5;
            // 
            // pnlKpiTaux
            // 
            pnlKpiTaux.Appearance.BackColor = Color.Honeydew;
            pnlKpiTaux.Appearance.Options.UseBackColor = true;
            pnlKpiTaux.Controls.Add(lblTauxPriseEnChargeVal);
            pnlKpiTaux.Controls.Add(lblTauxPriseEnChargeTitle);
            pnlKpiTaux.Dock = DockStyle.Left;
            pnlKpiTaux.Location = new Point(1154, 9);
            pnlKpiTaux.Margin = new Padding(4);
            pnlKpiTaux.Name = "pnlKpiTaux";
            pnlKpiTaux.Padding = new Padding(10, 7, 10, 7);
            pnlKpiTaux.Size = new Size(244, 75);
            pnlKpiTaux.TabIndex = 5;
            // 
            // lblTauxPriseEnChargeVal
            // 
            lblTauxPriseEnChargeVal.Appearance.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblTauxPriseEnChargeVal.Appearance.ForeColor = Color.SeaGreen;
            lblTauxPriseEnChargeVal.Appearance.Options.UseFont = true;
            lblTauxPriseEnChargeVal.Appearance.Options.UseForeColor = true;
            lblTauxPriseEnChargeVal.Dock = DockStyle.Fill;
            lblTauxPriseEnChargeVal.Location = new Point(12, 24);
            lblTauxPriseEnChargeVal.Margin = new Padding(4);
            lblTauxPriseEnChargeVal.Name = "lblTauxPriseEnChargeVal";
            lblTauxPriseEnChargeVal.Size = new Size(48, 25);
            lblTauxPriseEnChargeVal.TabIndex = 1;
            lblTauxPriseEnChargeVal.Text = "0.0 %";
            // 
            // lblTauxPriseEnChargeTitle
            // 
            lblTauxPriseEnChargeTitle.Appearance.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblTauxPriseEnChargeTitle.Appearance.ForeColor = Color.DarkGreen;
            lblTauxPriseEnChargeTitle.Appearance.Options.UseFont = true;
            lblTauxPriseEnChargeTitle.Appearance.Options.UseForeColor = true;
            lblTauxPriseEnChargeTitle.Dock = DockStyle.Top;
            lblTauxPriseEnChargeTitle.Location = new Point(12, 9);
            lblTauxPriseEnChargeTitle.Margin = new Padding(4);
            lblTauxPriseEnChargeTitle.Name = "lblTauxPriseEnChargeTitle";
            lblTauxPriseEnChargeTitle.Size = new Size(135, 15);
            lblTauxPriseEnChargeTitle.TabIndex = 0;
            lblTauxPriseEnChargeTitle.Text = "TAUX PRISE EN CHARGE";
            // 
            // pnlKpiCasnos
            // 
            pnlKpiCasnos.Appearance.BackColor = Color.FloralWhite;
            pnlKpiCasnos.Appearance.Options.UseBackColor = true;
            pnlKpiCasnos.Controls.Add(lblCasnosVal);
            pnlKpiCasnos.Controls.Add(lblCasnosTitle);
            pnlKpiCasnos.Dock = DockStyle.Left;
            pnlKpiCasnos.Location = new Point(910, 9);
            pnlKpiCasnos.Margin = new Padding(4);
            pnlKpiCasnos.Name = "pnlKpiCasnos";
            pnlKpiCasnos.Padding = new Padding(10, 7, 10, 7);
            pnlKpiCasnos.Size = new Size(244, 75);
            pnlKpiCasnos.TabIndex = 4;
            // 
            // lblCasnosVal
            // 
            lblCasnosVal.Appearance.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCasnosVal.Appearance.ForeColor = Color.DarkGoldenrod;
            lblCasnosVal.Appearance.Options.UseFont = true;
            lblCasnosVal.Appearance.Options.UseForeColor = true;
            lblCasnosVal.Dock = DockStyle.Fill;
            lblCasnosVal.Location = new Point(12, 24);
            lblCasnosVal.Margin = new Padding(4);
            lblCasnosVal.Name = "lblCasnosVal";
            lblCasnosVal.Size = new Size(111, 25);
            lblCasnosVal.TabIndex = 1;
            lblCasnosVal.Text = "0.00 DA (0%)";
            // 
            // lblCasnosTitle
            // 
            lblCasnosTitle.Appearance.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblCasnosTitle.Appearance.ForeColor = Color.DarkOrange;
            lblCasnosTitle.Appearance.Options.UseFont = true;
            lblCasnosTitle.Appearance.Options.UseForeColor = true;
            lblCasnosTitle.Dock = DockStyle.Top;
            lblCasnosTitle.Location = new Point(12, 9);
            lblCasnosTitle.Margin = new Padding(4);
            lblCasnosTitle.Name = "lblCasnosTitle";
            lblCasnosTitle.Size = new Size(128, 15);
            lblCasnosTitle.TabIndex = 0;
            lblCasnosTitle.Text = "PART CASNOS (TOTAL)";
            // 
            // pnlKpiCnas
            // 
            pnlKpiCnas.Appearance.BackColor = Color.GhostWhite;
            pnlKpiCnas.Appearance.Options.UseBackColor = true;
            pnlKpiCnas.Controls.Add(lblCnasVal);
            pnlKpiCnas.Controls.Add(lblCnasTitle);
            pnlKpiCnas.Dock = DockStyle.Left;
            pnlKpiCnas.Location = new Point(666, 9);
            pnlKpiCnas.Margin = new Padding(4);
            pnlKpiCnas.Name = "pnlKpiCnas";
            pnlKpiCnas.Padding = new Padding(10, 7, 10, 7);
            pnlKpiCnas.Size = new Size(244, 75);
            pnlKpiCnas.TabIndex = 3;
            // 
            // lblCnasVal
            // 
            lblCnasVal.Appearance.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCnasVal.Appearance.ForeColor = Color.DarkSlateBlue;
            lblCnasVal.Appearance.Options.UseFont = true;
            lblCnasVal.Appearance.Options.UseForeColor = true;
            lblCnasVal.Dock = DockStyle.Fill;
            lblCnasVal.Location = new Point(12, 24);
            lblCnasVal.Margin = new Padding(4);
            lblCnasVal.Name = "lblCnasVal";
            lblCnasVal.Size = new Size(111, 25);
            lblCnasVal.TabIndex = 1;
            lblCnasVal.Text = "0.00 DA (0%)";
            // 
            // lblCnasTitle
            // 
            lblCnasTitle.Appearance.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblCnasTitle.Appearance.ForeColor = Color.MediumSlateBlue;
            lblCnasTitle.Appearance.Options.UseFont = true;
            lblCnasTitle.Appearance.Options.UseForeColor = true;
            lblCnasTitle.Dock = DockStyle.Top;
            lblCnasTitle.Location = new Point(12, 9);
            lblCnasTitle.Margin = new Padding(4);
            lblCnasTitle.Name = "lblCnasTitle";
            lblCnasTitle.Size = new Size(112, 15);
            lblCnasTitle.TabIndex = 0;
            lblCnasTitle.Text = "PART CNAS (TOTAL)";
            // 
            // pnlKpiBoites
            // 
            pnlKpiBoites.Appearance.BackColor = Color.MintCream;
            pnlKpiBoites.Appearance.Options.UseBackColor = true;
            pnlKpiBoites.Controls.Add(lblBoitesVal);
            pnlKpiBoites.Controls.Add(lblBoitesTitle);
            pnlKpiBoites.Dock = DockStyle.Left;
            pnlKpiBoites.Location = new Point(460, 9);
            pnlKpiBoites.Margin = new Padding(4);
            pnlKpiBoites.Name = "pnlKpiBoites";
            pnlKpiBoites.Padding = new Padding(10, 7, 10, 7);
            pnlKpiBoites.Size = new Size(206, 75);
            pnlKpiBoites.TabIndex = 2;
            // 
            // lblBoitesVal
            // 
            lblBoitesVal.Appearance.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblBoitesVal.Appearance.ForeColor = Color.Teal;
            lblBoitesVal.Appearance.Options.UseFont = true;
            lblBoitesVal.Appearance.Options.UseForeColor = true;
            lblBoitesVal.Dock = DockStyle.Fill;
            lblBoitesVal.Location = new Point(12, 24);
            lblBoitesVal.Margin = new Padding(4);
            lblBoitesVal.Name = "lblBoitesVal";
            lblBoitesVal.Size = new Size(11, 25);
            lblBoitesVal.TabIndex = 1;
            lblBoitesVal.Text = "0";
            // 
            // lblBoitesTitle
            // 
            lblBoitesTitle.Appearance.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblBoitesTitle.Appearance.ForeColor = Color.DarkSlateGray;
            lblBoitesTitle.Appearance.Options.UseFont = true;
            lblBoitesTitle.Appearance.Options.UseForeColor = true;
            lblBoitesTitle.Dock = DockStyle.Top;
            lblBoitesTitle.Location = new Point(12, 9);
            lblBoitesTitle.Margin = new Padding(4);
            lblBoitesTitle.Name = "lblBoitesTitle";
            lblBoitesTitle.Size = new Size(104, 15);
            lblBoitesTitle.TabIndex = 0;
            lblBoitesTitle.Text = "BOÎTES DÉLIVRÉES";
            // 
            // pnlKpiFactures
            // 
            pnlKpiFactures.Appearance.BackColor = Color.WhiteSmoke;
            pnlKpiFactures.Appearance.Options.UseBackColor = true;
            pnlKpiFactures.Controls.Add(lblFactVal);
            pnlKpiFactures.Controls.Add(lblFactTitle);
            pnlKpiFactures.Dock = DockStyle.Left;
            pnlKpiFactures.Location = new Point(254, 9);
            pnlKpiFactures.Margin = new Padding(4);
            pnlKpiFactures.Name = "pnlKpiFactures";
            pnlKpiFactures.Padding = new Padding(10, 7, 10, 7);
            pnlKpiFactures.Size = new Size(206, 75);
            pnlKpiFactures.TabIndex = 1;
            // 
            // lblFactVal
            // 
            lblFactVal.Appearance.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblFactVal.Appearance.ForeColor = Color.DarkGreen;
            lblFactVal.Appearance.Options.UseFont = true;
            lblFactVal.Appearance.Options.UseForeColor = true;
            lblFactVal.Dock = DockStyle.Fill;
            lblFactVal.Location = new Point(12, 24);
            lblFactVal.Margin = new Padding(4);
            lblFactVal.Name = "lblFactVal";
            lblFactVal.Size = new Size(11, 25);
            lblFactVal.TabIndex = 1;
            lblFactVal.Text = "0";
            // 
            // lblFactTitle
            // 
            lblFactTitle.Appearance.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblFactTitle.Appearance.ForeColor = Color.ForestGreen;
            lblFactTitle.Appearance.Options.UseFont = true;
            lblFactTitle.Appearance.Options.UseForeColor = true;
            lblFactTitle.Dock = DockStyle.Top;
            lblFactTitle.Location = new Point(12, 9);
            lblFactTitle.Margin = new Padding(4);
            lblFactTitle.Name = "lblFactTitle";
            lblFactTitle.Size = new Size(98, 15);
            lblFactTitle.TabIndex = 0;
            lblFactTitle.Text = "TOTAL FACTURES";
            // 
            // pnlKpiCa
            // 
            pnlKpiCa.Appearance.BackColor = Color.AliceBlue;
            pnlKpiCa.Appearance.Options.UseBackColor = true;
            pnlKpiCa.Controls.Add(lblCaVal);
            pnlKpiCa.Controls.Add(lblCaTitle);
            pnlKpiCa.Dock = DockStyle.Left;
            pnlKpiCa.Location = new Point(10, 9);
            pnlKpiCa.Margin = new Padding(4);
            pnlKpiCa.Name = "pnlKpiCa";
            pnlKpiCa.Padding = new Padding(10, 7, 10, 7);
            pnlKpiCa.Size = new Size(244, 75);
            pnlKpiCa.TabIndex = 0;
            // 
            // lblCaVal
            // 
            lblCaVal.Appearance.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblCaVal.Appearance.ForeColor = Color.MidnightBlue;
            lblCaVal.Appearance.Options.UseFont = true;
            lblCaVal.Appearance.Options.UseForeColor = true;
            lblCaVal.Dock = DockStyle.Fill;
            lblCaVal.Location = new Point(12, 24);
            lblCaVal.Margin = new Padding(4);
            lblCaVal.Name = "lblCaVal";
            lblCaVal.Size = new Size(70, 25);
            lblCaVal.TabIndex = 1;
            lblCaVal.Text = "0.00 DA";
            // 
            // lblCaTitle
            // 
            lblCaTitle.Appearance.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            lblCaTitle.Appearance.ForeColor = Color.SteelBlue;
            lblCaTitle.Appearance.Options.UseFont = true;
            lblCaTitle.Appearance.Options.UseForeColor = true;
            lblCaTitle.Dock = DockStyle.Top;
            lblCaTitle.Location = new Point(12, 9);
            lblCaTitle.Margin = new Padding(4);
            lblCaTitle.Name = "lblCaTitle";
            lblCaTitle.Size = new Size(115, 15);
            lblCaTitle.TabIndex = 0;
            lblCaTitle.Text = "CHIFFRE D'AFFAIRES";
            // 
            // splitContainerCharts
            // 
            splitContainerCharts.Dock = DockStyle.Fill;
            splitContainerCharts.Location = new Point(0, 291);
            splitContainerCharts.Margin = new Padding(4);
            splitContainerCharts.Name = "splitContainerCharts";
            // 
            // splitContainerCharts.Panel1
            // 
            splitContainerCharts.Panel1.Controls.Add(chartHourly);
            splitContainerCharts.Panel1.Padding = new Padding(8, 7, 8, 7);
            splitContainerCharts.Panel1.Text = "Panel1";
            // 
            // splitContainerCharts.Panel2
            // 
            splitContainerCharts.Panel2.Controls.Add(chartTopProducts);
            splitContainerCharts.Panel2.Padding = new Padding(8, 7, 8, 7);
            splitContainerCharts.Panel2.Text = "Panel2";
            splitContainerCharts.Size = new Size(1479, 574);
            splitContainerCharts.SplitterPosition = 739;
            splitContainerCharts.TabIndex = 6;
            // 
            // chartHourly
            // 
            chartHourly.Dock = DockStyle.Fill;
            chartHourly.Location = new Point(8, 7);
            chartHourly.Margin = new Padding(4);
            chartHourly.Name = "chartHourly";
            chartHourly.Size = new Size(723, 560);
            chartHourly.TabIndex = 0;
            // 
            // chartTopProducts
            // 
            chartTopProducts.Dock = DockStyle.Fill;
            chartTopProducts.Location = new Point(8, 7);
            chartTopProducts.Margin = new Padding(4);
            chartTopProducts.Name = "chartTopProducts";
            chartTopProducts.Size = new Size(718, 560);
            chartTopProducts.TabIndex = 0;
            // 
            // ScopeDashboardUc
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainerCharts);
            Controls.Add(panelKpis);
            Controls.Add(panelHeader);
            Controls.Add(ribbonControl1);
            Margin = new Padding(4);
            Name = "ScopeDashboardUc";
            Size = new Size(1479, 865);
            ((System.ComponentModel.ISupportInitialize)ribbonControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)panelKpis).EndInit();
            panelKpis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlKpiTaux).EndInit();
            pnlKpiTaux.ResumeLayout(false);
            pnlKpiTaux.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCasnos).EndInit();
            pnlKpiCasnos.ResumeLayout(false);
            pnlKpiCasnos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCnas).EndInit();
            pnlKpiCnas.ResumeLayout(false);
            pnlKpiCnas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiBoites).EndInit();
            pnlKpiBoites.ResumeLayout(false);
            pnlKpiBoites.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiFactures).EndInit();
            pnlKpiFactures.ResumeLayout(false);
            pnlKpiFactures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCa).EndInit();
            pnlKpiCa.ResumeLayout(false);
            pnlKpiCa.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerCharts.Panel1).EndInit();
            splitContainerCharts.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerCharts.Panel2).EndInit();
            splitContainerCharts.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerCharts).EndInit();
            splitContainerCharts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartHourly).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartTopProducts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroupPeriod;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarEditItem txtDateFrom;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repoDateFrom;
        private DevExpress.XtraBars.BarEditItem txtDateTo;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repoDateTo;
        private DevExpress.XtraBars.BarButtonItem btnClearDates;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup PeriodRange;
        private DevExpress.XtraBars.BarButtonItem btnAllPeriod;
        private DevExpress.XtraBars.BarButtonItem btnLastYear;
        private DevExpress.XtraBars.BarButtonItem btn6Months;
        private DevExpress.XtraBars.BarButtonItem btnThisYear;
        private DevExpress.XtraBars.BarButtonItem btnThisMonth;
        private DevExpress.XtraEditors.PanelControl panelHeader;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl lblSubtitle;
        private DevExpress.XtraEditors.PanelControl panelKpis;
        private DevExpress.XtraEditors.PanelControl pnlKpiCa;
        private DevExpress.XtraEditors.LabelControl lblCaVal;
        private DevExpress.XtraEditors.LabelControl lblCaTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiFactures;
        private DevExpress.XtraEditors.LabelControl lblFactVal;
        private DevExpress.XtraEditors.LabelControl lblFactTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiBoites;
        private DevExpress.XtraEditors.LabelControl lblBoitesVal;
        private DevExpress.XtraEditors.LabelControl lblBoitesTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiCnas;
        private DevExpress.XtraEditors.LabelControl lblCnasVal;
        private DevExpress.XtraEditors.LabelControl lblCnasTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiCasnos;
        private DevExpress.XtraEditors.LabelControl lblCasnosVal;
        private DevExpress.XtraEditors.LabelControl lblCasnosTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiTaux;
        private DevExpress.XtraEditors.LabelControl lblTauxPriseEnChargeVal;
        private DevExpress.XtraEditors.LabelControl lblTauxPriseEnChargeTitle;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerCharts;
        private DevExpress.XtraCharts.ChartControl chartHourly;
        private DevExpress.XtraCharts.ChartControl chartTopProducts;
    }
}

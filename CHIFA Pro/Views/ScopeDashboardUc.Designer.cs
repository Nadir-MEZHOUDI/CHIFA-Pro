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
            components = new System.ComponentModel.Container();
            barManager1 = new DevExpress.XtraBars.BarManager(components);
            bar1 = new DevExpress.XtraBars.Bar();
            btnRefresh = new DevExpress.XtraBars.BarButtonItem();
            txtDateFrom = new DevExpress.XtraBars.BarEditItem();
            repoDateFrom = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            txtDateTo = new DevExpress.XtraBars.BarEditItem();
            repoDateTo = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            btnClearDates = new DevExpress.XtraBars.BarButtonItem();
            barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            panelHeader = new DevExpress.XtraEditors.PanelControl();
            lblTitle = new DevExpress.XtraEditors.LabelControl();
            lblSubtitle = new DevExpress.XtraEditors.LabelControl();
            panelKpis = new DevExpress.XtraEditors.PanelControl();
            pnlKpiCa = new DevExpress.XtraEditors.PanelControl();
            lblCaVal = new DevExpress.XtraEditors.LabelControl();
            lblCaTitle = new DevExpress.XtraEditors.LabelControl();
            pnlKpiFactures = new DevExpress.XtraEditors.PanelControl();
            lblFactVal = new DevExpress.XtraEditors.LabelControl();
            lblFactTitle = new DevExpress.XtraEditors.LabelControl();
            pnlKpiBoites = new DevExpress.XtraEditors.PanelControl();
            lblBoitesVal = new DevExpress.XtraEditors.LabelControl();
            lblBoitesTitle = new DevExpress.XtraEditors.LabelControl();
            pnlKpiCnas = new DevExpress.XtraEditors.PanelControl();
            lblCnasVal = new DevExpress.XtraEditors.LabelControl();
            lblCnasTitle = new DevExpress.XtraEditors.LabelControl();
            pnlKpiCasnos = new DevExpress.XtraEditors.PanelControl();
            lblCasnosVal = new DevExpress.XtraEditors.LabelControl();
            lblCasnosTitle = new DevExpress.XtraEditors.LabelControl();
            pnlKpiTaux = new DevExpress.XtraEditors.PanelControl();
            lblTauxPriseEnChargeVal = new DevExpress.XtraEditors.LabelControl();
            lblTauxPriseEnChargeTitle = new DevExpress.XtraEditors.LabelControl();
            splitContainerCharts = new DevExpress.XtraEditors.SplitContainerControl();
            chartHourly = new DevExpress.XtraCharts.ChartControl();
            chartTopProducts = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).BeginInit();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelKpis).BeginInit();
            panelKpis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCa).BeginInit();
            pnlKpiCa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiFactures).BeginInit();
            pnlKpiFactures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiBoites).BeginInit();
            pnlKpiBoites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCnas).BeginInit();
            pnlKpiCnas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCasnos).BeginInit();
            pnlKpiCasnos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiTaux).BeginInit();
            pnlKpiTaux.SuspendLayout();
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
            // barManager1
            // 
            barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] { bar1 });
            barManager1.DockControls.Add(barDockControlTop);
            barManager1.DockControls.Add(barDockControlBottom);
            barManager1.DockControls.Add(barDockControlLeft);
            barManager1.DockControls.Add(barDockControlRight);
            barManager1.Form = this;
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { btnRefresh, txtDateFrom, txtDateTo, btnClearDates });
            barManager1.MaxItemId = 4;
            barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repoDateFrom, repoDateTo });
            // 
            // bar1
            // 
            bar1.BarName = "Filtres";
            bar1.DockCol = 0;
            bar1.DockRow = 0;
            bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(txtDateFrom), new DevExpress.XtraBars.LinkPersistInfo(txtDateTo), new DevExpress.XtraBars.LinkPersistInfo(btnRefresh), new DevExpress.XtraBars.LinkPersistInfo(btnClearDates) });
            bar1.OptionsBar.AllowQuickCustomization = false;
            bar1.OptionsBar.DrawDragBorder = false;
            bar1.OptionsBar.UseWholeRow = true;
            bar1.Text = "Filtres";
            // 
            // btnRefresh
            // 
            btnRefresh.Caption = "Actualiser";
            btnRefresh.Id = 0;
            btnRefresh.ImageOptions.Image = FrmMain.Image(1);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
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
            btnClearDates.Name = "btnClearDates";
            btnClearDates.ItemClick += BtnClearDates_ItemClick;
            // 
            // barDockControlTop
            // 
            barDockControlTop.CausesValidation = false;
            barDockControlTop.Dock = DockStyle.Top;
            barDockControlTop.Location = new Point(0, 0);
            barDockControlTop.Manager = barManager1;
            barDockControlTop.Size = new Size(1150, 30);
            // 
            // barDockControlBottom
            // 
            barDockControlBottom.CausesValidation = false;
            barDockControlBottom.Dock = DockStyle.Bottom;
            barDockControlBottom.Location = new Point(0, 700);
            barDockControlBottom.Manager = barManager1;
            barDockControlBottom.Size = new Size(1150, 0);
            // 
            // barDockControlLeft
            // 
            barDockControlLeft.CausesValidation = false;
            barDockControlLeft.Dock = DockStyle.Left;
            barDockControlLeft.Location = new Point(0, 30);
            barDockControlLeft.Manager = barManager1;
            barDockControlLeft.Size = new Size(0, 670);
            // 
            // barDockControlRight
            // 
            barDockControlRight.CausesValidation = false;
            barDockControlRight.Dock = DockStyle.Right;
            barDockControlRight.Location = new Point(1150, 30);
            barDockControlRight.Manager = barManager1;
            barDockControlRight.Size = new Size(0, 670);
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 30);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(12, 8, 12, 8);
            panelHeader.Size = new Size(1150, 52);
            panelHeader.TabIndex = 4;
            // 
            // lblTitle
            // 
            lblTitle.Appearance.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.Appearance.ForeColor = Color.DarkSlateBlue;
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Appearance.Options.UseForeColor = true;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Location = new Point(14, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(295, 21);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "CHIFA SCOPE - TOUR DE CONTRÔLE";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Appearance.Font = new Font("Segoe UI", 8.25F);
            lblSubtitle.Appearance.ForeColor = Color.Gray;
            lblSubtitle.Appearance.Options.UseFont = true;
            lblSubtitle.Appearance.Options.UseForeColor = true;
            lblSubtitle.Dock = DockStyle.Top;
            lblSubtitle.Location = new Point(14, 31);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(420, 13);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Surveillance en temps réel des indicateurs officine, flux horaires et prescriptions";
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
            panelKpis.Location = new Point(0, 82);
            panelKpis.Name = "panelKpis";
            panelKpis.Padding = new Padding(6);
            panelKpis.Size = new Size(1150, 75);
            panelKpis.TabIndex = 5;
            // 
            // pnlKpiCa
            // 
            pnlKpiCa.Appearance.BackColor = Color.AliceBlue;
            pnlKpiCa.Appearance.Options.UseBackColor = true;
            pnlKpiCa.Controls.Add(lblCaVal);
            pnlKpiCa.Controls.Add(lblCaTitle);
            pnlKpiCa.Dock = DockStyle.Left;
            pnlKpiCa.Location = new Point(8, 8);
            pnlKpiCa.Name = "pnlKpiCa";
            pnlKpiCa.Padding = new Padding(8, 6, 8, 6);
            pnlKpiCa.Size = new Size(190, 59);
            pnlKpiCa.TabIndex = 0;
            // 
            // lblCaVal
            // 
            lblCaVal.Appearance.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblCaVal.Appearance.ForeColor = Color.MidnightBlue;
            lblCaVal.Appearance.Options.UseFont = true;
            lblCaVal.Appearance.Options.UseForeColor = true;
            lblCaVal.Dock = DockStyle.Fill;
            lblCaVal.Location = new Point(10, 24);
            lblCaVal.Name = "lblCaVal";
            lblCaVal.Size = new Size(80, 21);
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
            lblCaTitle.Location = new Point(10, 8);
            lblCaTitle.Name = "lblCaTitle";
            lblCaTitle.Size = new Size(120, 12);
            lblCaTitle.TabIndex = 0;
            lblCaTitle.Text = "CHIFFRE D'AFFAIRES";
            // 
            // pnlKpiFactures
            // 
            pnlKpiFactures.Appearance.BackColor = Color.WhiteSmoke;
            pnlKpiFactures.Appearance.Options.UseBackColor = true;
            pnlKpiFactures.Controls.Add(lblFactVal);
            pnlKpiFactures.Controls.Add(lblFactTitle);
            pnlKpiFactures.Dock = DockStyle.Left;
            pnlKpiFactures.Location = new Point(198, 8);
            pnlKpiFactures.Name = "pnlKpiFactures";
            pnlKpiFactures.Padding = new Padding(8, 6, 8, 6);
            pnlKpiFactures.Size = new Size(160, 59);
            pnlKpiFactures.TabIndex = 1;
            // 
            // lblFactVal
            // 
            lblFactVal.Appearance.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblFactVal.Appearance.ForeColor = Color.DarkGreen;
            lblFactVal.Appearance.Options.UseFont = true;
            lblFactVal.Appearance.Options.UseForeColor = true;
            lblFactVal.Dock = DockStyle.Fill;
            lblFactVal.Location = new Point(10, 24);
            lblFactVal.Name = "lblFactVal";
            lblFactVal.Size = new Size(9, 21);
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
            lblFactTitle.Location = new Point(10, 8);
            lblFactTitle.Name = "lblFactTitle";
            lblFactTitle.Size = new Size(95, 12);
            lblFactTitle.TabIndex = 0;
            lblFactTitle.Text = "TOTAL FACTURES";
            // 
            // pnlKpiBoites
            // 
            pnlKpiBoites.Appearance.BackColor = Color.MintCream;
            pnlKpiBoites.Appearance.Options.UseBackColor = true;
            pnlKpiBoites.Controls.Add(lblBoitesVal);
            pnlKpiBoites.Controls.Add(lblBoitesTitle);
            pnlKpiBoites.Dock = DockStyle.Left;
            pnlKpiBoites.Location = new Point(358, 8);
            pnlKpiBoites.Name = "pnlKpiBoites";
            pnlKpiBoites.Padding = new Padding(8, 6, 8, 6);
            pnlKpiBoites.Size = new Size(160, 59);
            pnlKpiBoites.TabIndex = 2;
            // 
            // lblBoitesVal
            // 
            lblBoitesVal.Appearance.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblBoitesVal.Appearance.ForeColor = Color.Teal;
            lblBoitesVal.Appearance.Options.UseFont = true;
            lblBoitesVal.Appearance.Options.UseForeColor = true;
            lblBoitesVal.Dock = DockStyle.Fill;
            lblBoitesVal.Location = new Point(10, 24);
            lblBoitesVal.Name = "lblBoitesVal";
            lblBoitesVal.Size = new Size(9, 21);
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
            lblBoitesTitle.Location = new Point(10, 8);
            lblBoitesTitle.Name = "lblBoitesTitle";
            lblBoitesTitle.Size = new Size(110, 12);
            lblBoitesTitle.TabIndex = 0;
            lblBoitesTitle.Text = "BOÎTES DÉLIVRÉES";
            // 
            // pnlKpiCnas
            // 
            pnlKpiCnas.Appearance.BackColor = Color.GhostWhite;
            pnlKpiCnas.Appearance.Options.UseBackColor = true;
            pnlKpiCnas.Controls.Add(lblCnasVal);
            pnlKpiCnas.Controls.Add(lblCnasTitle);
            pnlKpiCnas.Dock = DockStyle.Left;
            pnlKpiCnas.Location = new Point(518, 8);
            pnlKpiCnas.Name = "pnlKpiCnas";
            pnlKpiCnas.Padding = new Padding(8, 6, 8, 6);
            pnlKpiCnas.Size = new Size(190, 59);
            pnlKpiCnas.TabIndex = 3;
            // 
            // lblCnasVal
            // 
            lblCnasVal.Appearance.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCnasVal.Appearance.ForeColor = Color.DarkSlateBlue;
            lblCnasVal.Appearance.Options.UseFont = true;
            lblCnasVal.Appearance.Options.UseForeColor = true;
            lblCnasVal.Dock = DockStyle.Fill;
            lblCnasVal.Location = new Point(10, 24);
            lblCnasVal.Name = "lblCnasVal";
            lblCnasVal.Size = new Size(76, 20);
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
            lblCnasTitle.Location = new Point(10, 8);
            lblCnasTitle.Name = "lblCnasTitle";
            lblCnasTitle.Size = new Size(95, 12);
            lblCnasTitle.TabIndex = 0;
            lblCnasTitle.Text = "PART CNAS (TOTAL)";
            // 
            // pnlKpiCasnos
            // 
            pnlKpiCasnos.Appearance.BackColor = Color.FloralWhite;
            pnlKpiCasnos.Appearance.Options.UseBackColor = true;
            pnlKpiCasnos.Controls.Add(lblCasnosVal);
            pnlKpiCasnos.Controls.Add(lblCasnosTitle);
            pnlKpiCasnos.Dock = DockStyle.Left;
            pnlKpiCasnos.Location = new Point(708, 8);
            pnlKpiCasnos.Name = "pnlKpiCasnos";
            pnlKpiCasnos.Padding = new Padding(8, 6, 8, 6);
            pnlKpiCasnos.Size = new Size(190, 59);
            pnlKpiCasnos.TabIndex = 4;
            // 
            // lblCasnosVal
            // 
            lblCasnosVal.Appearance.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCasnosVal.Appearance.ForeColor = Color.DarkGoldenrod;
            lblCasnosVal.Appearance.Options.UseFont = true;
            lblCasnosVal.Appearance.Options.UseForeColor = true;
            lblCasnosVal.Dock = DockStyle.Fill;
            lblCasnosVal.Location = new Point(10, 24);
            lblCasnosVal.Name = "lblCasnosVal";
            lblCasnosVal.Size = new Size(76, 20);
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
            lblCasnosTitle.Location = new Point(10, 8);
            lblCasnosTitle.Name = "lblCasnosTitle";
            lblCasnosTitle.Size = new Size(105, 12);
            lblCasnosTitle.TabIndex = 0;
            lblCasnosTitle.Text = "PART CASNOS (TOTAL)";
            // 
            // pnlKpiTaux
            // 
            pnlKpiTaux.Appearance.BackColor = Color.Honeydew;
            pnlKpiTaux.Appearance.Options.UseBackColor = true;
            pnlKpiTaux.Controls.Add(lblTauxPriseEnChargeVal);
            pnlKpiTaux.Controls.Add(lblTauxPriseEnChargeTitle);
            pnlKpiTaux.Dock = DockStyle.Left;
            pnlKpiTaux.Location = new Point(898, 8);
            pnlKpiTaux.Name = "pnlKpiTaux";
            pnlKpiTaux.Padding = new Padding(8, 6, 8, 6);
            pnlKpiTaux.Size = new Size(190, 59);
            pnlKpiTaux.TabIndex = 5;
            // 
            // lblTauxPriseEnChargeVal
            // 
            lblTauxPriseEnChargeVal.Appearance.Font = new Font("Segoe UI", 11.5F, FontStyle.Bold);
            lblTauxPriseEnChargeVal.Appearance.ForeColor = Color.SeaGreen;
            lblTauxPriseEnChargeVal.Appearance.Options.UseFont = true;
            lblTauxPriseEnChargeVal.Appearance.Options.UseForeColor = true;
            lblTauxPriseEnChargeVal.Dock = DockStyle.Fill;
            lblTauxPriseEnChargeVal.Location = new Point(10, 24);
            lblTauxPriseEnChargeVal.Name = "lblTauxPriseEnChargeVal";
            lblTauxPriseEnChargeVal.Size = new Size(39, 21);
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
            lblTauxPriseEnChargeTitle.Location = new Point(10, 8);
            lblTauxPriseEnChargeTitle.Name = "lblTauxPriseEnChargeTitle";
            lblTauxPriseEnChargeTitle.Size = new Size(130, 12);
            lblTauxPriseEnChargeTitle.TabIndex = 0;
            lblTauxPriseEnChargeTitle.Text = "TAUX PRISE EN CHARGE";
            // 
            // splitContainerCharts
            // 
            splitContainerCharts.Dock = DockStyle.Fill;
            splitContainerCharts.Location = new Point(0, 157);
            splitContainerCharts.Name = "splitContainerCharts";
            splitContainerCharts.Panel1.Controls.Add(chartHourly);
            splitContainerCharts.Panel1.Padding = new Padding(6);
            splitContainerCharts.Panel1.Text = "Panel1";
            splitContainerCharts.Panel2.Controls.Add(chartTopProducts);
            splitContainerCharts.Panel2.Padding = new Padding(6);
            splitContainerCharts.Panel2.Text = "Panel2";
            splitContainerCharts.Size = new Size(1150, 543);
            splitContainerCharts.SplitterPosition = 575;
            splitContainerCharts.TabIndex = 6;
            // 
            // chartHourly
            // 
            chartHourly.Dock = DockStyle.Fill;
            chartHourly.Location = new Point(6, 6);
            chartHourly.Name = "chartHourly";
            chartHourly.Size = new Size(563, 531);
            chartHourly.TabIndex = 0;
            // 
            // chartTopProducts
            // 
            chartTopProducts.Dock = DockStyle.Fill;
            chartTopProducts.Location = new Point(6, 6);
            chartTopProducts.Name = "chartTopProducts";
            chartTopProducts.Size = new Size(563, 531);
            chartTopProducts.TabIndex = 0;
            // 
            // ScopeDashboardUc
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainerCharts);
            Controls.Add(panelKpis);
            Controls.Add(panelHeader);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            Name = "ScopeDashboardUc";
            Size = new Size(1150, 700);
            ((System.ComponentModel.ISupportInitialize)barManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)panelKpis).EndInit();
            panelKpis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlKpiCa).EndInit();
            pnlKpiCa.ResumeLayout(false);
            pnlKpiCa.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiFactures).EndInit();
            pnlKpiFactures.ResumeLayout(false);
            pnlKpiFactures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiBoites).EndInit();
            pnlKpiBoites.ResumeLayout(false);
            pnlKpiBoites.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCnas).EndInit();
            pnlKpiCnas.ResumeLayout(false);
            pnlKpiCnas.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCasnos).EndInit();
            pnlKpiCasnos.ResumeLayout(false);
            pnlKpiCasnos.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiTaux).EndInit();
            pnlKpiTaux.ResumeLayout(false);
            pnlKpiTaux.PerformLayout();
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

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarEditItem txtDateFrom;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repoDateFrom;
        private DevExpress.XtraBars.BarEditItem txtDateTo;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repoDateTo;
        private DevExpress.XtraBars.BarButtonItem btnClearDates;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
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

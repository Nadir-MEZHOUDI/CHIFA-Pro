namespace CHIFA.Pro.Views
{
    partial class PrevisionChroniquesUc
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
            btnFilterAll = new DevExpress.XtraBars.BarButtonItem();
            btnFilterOverdue = new DevExpress.XtraBars.BarButtonItem();
            btnFilterThisWeek = new DevExpress.XtraBars.BarButtonItem();
            btnExport = new DevExpress.XtraBars.BarButtonItem();
            barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            panelHeader = new DevExpress.XtraEditors.PanelControl();
            lblSubtitle = new DevExpress.XtraEditors.LabelControl();
            lblTitle = new DevExpress.XtraEditors.LabelControl();
            panelKpis = new DevExpress.XtraEditors.PanelControl();
            pnlKpiCaEstime = new DevExpress.XtraEditors.PanelControl();
            lblCaEstimeVal = new DevExpress.XtraEditors.LabelControl();
            lblCaEstimeTitle = new DevExpress.XtraEditors.LabelControl();
            pnlKpiTotal = new DevExpress.XtraEditors.PanelControl();
            lblTotalVal = new DevExpress.XtraEditors.LabelControl();
            lblTotalTitle = new DevExpress.XtraEditors.LabelControl();
            pnlKpiOverdue = new DevExpress.XtraEditors.PanelControl();
            lblOverdueVal = new DevExpress.XtraEditors.LabelControl();
            lblOverdueTitle = new DevExpress.XtraEditors.LabelControl();
            pnlKpiThisWeek = new DevExpress.XtraEditors.PanelControl();
            lblThisWeekVal = new DevExpress.XtraEditors.LabelControl();
            lblThisWeekTitle = new DevExpress.XtraEditors.LabelControl();
            gridForecast = new DevExpress.XtraGrid.GridControl();
            viewForecast = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).BeginInit();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelKpis).BeginInit();
            panelKpis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiCaEstime).BeginInit();
            pnlKpiCaEstime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiTotal).BeginInit();
            pnlKpiTotal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiOverdue).BeginInit();
            pnlKpiOverdue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiThisWeek).BeginInit();
            pnlKpiThisWeek.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridForecast).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewForecast).BeginInit();
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
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { btnRefresh, btnFilterAll, btnFilterOverdue, btnFilterThisWeek, btnExport });
            barManager1.MaxItemId = 5;
            // 
            // bar1
            // 
            bar1.BarName = "Tools";
            bar1.DockCol = 0;
            bar1.DockRow = 0;
            bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
                new DevExpress.XtraBars.LinkPersistInfo(btnRefresh),
                new DevExpress.XtraBars.LinkPersistInfo(btnFilterAll),
                new DevExpress.XtraBars.LinkPersistInfo(btnFilterOverdue),
                new DevExpress.XtraBars.LinkPersistInfo(btnFilterThisWeek),
                new DevExpress.XtraBars.LinkPersistInfo(btnExport)
            });
            bar1.OptionsBar.AllowQuickCustomization = false;
            bar1.OptionsBar.DrawDragBorder = false;
            bar1.OptionsBar.UseWholeRow = true;
            bar1.Text = "Tools";
            // 
            // btnRefresh
            // 
            btnRefresh.Caption = "Actualiser";
            btnRefresh.Id = 0;
            btnRefresh.Name = "btnRefresh";
            btnRefresh.ItemClick += BtnRefresh_ItemClick;
            // 
            // btnFilterAll
            // 
            btnFilterAll.Caption = "Tous les patients attendus";
            btnFilterAll.Id = 1;
            btnFilterAll.Name = "btnFilterAll";
            btnFilterAll.ItemClick += BtnFilterAll_ItemClick;
            // 
            // btnFilterOverdue
            // 
            btnFilterOverdue.Caption = "En retard (Inobservance)";
            btnFilterOverdue.Id = 2;
            btnFilterOverdue.Name = "btnFilterOverdue";
            btnFilterOverdue.ItemClick += BtnFilterOverdue_ItemClick;
            // 
            // btnFilterThisWeek
            // 
            btnFilterThisWeek.Caption = "Attendus cette semaine (≤ 7j)";
            btnFilterThisWeek.Id = 3;
            btnFilterThisWeek.Name = "btnFilterThisWeek";
            btnFilterThisWeek.ItemClick += BtnFilterThisWeek_ItemClick;
            // 
            // btnExport
            // 
            btnExport.Caption = "Exporter Planning (Excel)";
            btnExport.Id = 4;
            btnExport.Name = "btnExport";
            btnExport.ItemClick += BtnExport_ItemClick;
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 31);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1300, 60);
            panelHeader.TabIndex = 4;
            // 
            // lblTitle
            // 
            lblTitle.Appearance.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Location = new Point(16, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(495, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "PRÉVISION DES CHRONIQUES & REVENUS FUTURS";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Appearance.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitle.Appearance.ForeColor = Color.Gray;
            lblSubtitle.Appearance.Options.UseFont = true;
            lblSubtitle.Appearance.Options.UseForeColor = true;
            lblSubtitle.Location = new Point(16, 35);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(545, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Anticipation des dates de retour des malades chroniques, estimation du CA prévisionnel et alertes de rupture";
            // 
            // panelKpis
            // 
            panelKpis.Controls.Add(pnlKpiCaEstime);
            panelKpis.Controls.Add(pnlKpiThisWeek);
            panelKpis.Controls.Add(pnlKpiOverdue);
            panelKpis.Controls.Add(pnlKpiTotal);
            panelKpis.Dock = DockStyle.Top;
            panelKpis.Location = new Point(0, 91);
            panelKpis.Name = "panelKpis";
            panelKpis.Size = new Size(1300, 85);
            panelKpis.TabIndex = 5;
            // 
            // pnlKpiTotal
            // 
            pnlKpiTotal.Controls.Add(lblTotalVal);
            pnlKpiTotal.Controls.Add(lblTotalTitle);
            pnlKpiTotal.Location = new Point(12, 10);
            pnlKpiTotal.Name = "pnlKpiTotal";
            pnlKpiTotal.Size = new Size(290, 65);
            pnlKpiTotal.TabIndex = 0;
            // 
            // lblTotalTitle
            // 
            lblTotalTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalTitle.Appearance.ForeColor = Color.Gray;
            lblTotalTitle.Appearance.Options.UseFont = true;
            lblTotalTitle.Appearance.Options.UseForeColor = true;
            lblTotalTitle.Location = new Point(12, 8);
            lblTotalTitle.Name = "lblTotalTitle";
            lblTotalTitle.Size = new Size(144, 13);
            lblTotalTitle.TabIndex = 0;
            lblTotalTitle.Text = "TOTAL PATIENTS ATTENDUS";
            // 
            // lblTotalVal
            // 
            lblTotalVal.Appearance.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalVal.Appearance.ForeColor = Color.RoyalBlue;
            lblTotalVal.Appearance.Options.UseFont = true;
            lblTotalVal.Appearance.Options.UseForeColor = true;
            lblTotalVal.Location = new Point(12, 28);
            lblTotalVal.Name = "lblTotalVal";
            lblTotalVal.Size = new Size(11, 25);
            lblTotalVal.TabIndex = 1;
            lblTotalVal.Text = "0";
            // 
            // pnlKpiOverdue
            // 
            pnlKpiOverdue.Controls.Add(lblOverdueVal);
            pnlKpiOverdue.Controls.Add(lblOverdueTitle);
            pnlKpiOverdue.Location = new Point(315, 10);
            pnlKpiOverdue.Name = "pnlKpiOverdue";
            pnlKpiOverdue.Size = new Size(290, 65);
            pnlKpiOverdue.TabIndex = 1;
            // 
            // lblOverdueTitle
            // 
            lblOverdueTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOverdueTitle.Appearance.ForeColor = Color.Gray;
            lblOverdueTitle.Appearance.Options.UseFont = true;
            lblOverdueTitle.Appearance.Options.UseForeColor = true;
            lblOverdueTitle.Location = new Point(12, 8);
            lblOverdueTitle.Name = "lblOverdueTitle";
            lblOverdueTitle.Size = new Size(160, 13);
            lblOverdueTitle.TabIndex = 0;
            lblOverdueTitle.Text = "EN RETARD (INOBSERVANCE)";
            // 
            // lblOverdueVal
            // 
            lblOverdueVal.Appearance.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOverdueVal.Appearance.ForeColor = Color.Crimson;
            lblOverdueVal.Appearance.Options.UseFont = true;
            lblOverdueVal.Appearance.Options.UseForeColor = true;
            lblOverdueVal.Location = new Point(12, 28);
            lblOverdueVal.Name = "lblOverdueVal";
            lblOverdueVal.Size = new Size(11, 25);
            lblOverdueVal.TabIndex = 1;
            lblOverdueVal.Text = "0";
            // 
            // pnlKpiThisWeek
            // 
            pnlKpiThisWeek.Controls.Add(lblThisWeekVal);
            pnlKpiThisWeek.Controls.Add(lblThisWeekTitle);
            pnlKpiThisWeek.Location = new Point(620, 10);
            pnlKpiThisWeek.Name = "pnlKpiThisWeek";
            pnlKpiThisWeek.Size = new Size(290, 65);
            pnlKpiThisWeek.TabIndex = 2;
            // 
            // lblThisWeekTitle
            // 
            lblThisWeekTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblThisWeekTitle.Appearance.ForeColor = Color.Gray;
            lblThisWeekTitle.Appearance.Options.UseFont = true;
            lblThisWeekTitle.Appearance.Options.UseForeColor = true;
            lblThisWeekTitle.Location = new Point(12, 8);
            lblThisWeekTitle.Name = "lblThisWeekTitle";
            lblThisWeekTitle.Size = new Size(149, 13);
            lblThisWeekTitle.TabIndex = 0;
            lblThisWeekTitle.Text = "ATTENDUS CETTE SEMAINE";
            // 
            // lblThisWeekVal
            // 
            lblThisWeekVal.Appearance.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblThisWeekVal.Appearance.ForeColor = Color.DarkOrange;
            lblThisWeekVal.Appearance.Options.UseFont = true;
            lblThisWeekVal.Appearance.Options.UseForeColor = true;
            lblThisWeekVal.Location = new Point(12, 28);
            lblThisWeekVal.Name = "lblThisWeekVal";
            lblThisWeekVal.Size = new Size(11, 25);
            lblThisWeekVal.TabIndex = 1;
            lblThisWeekVal.Text = "0";
            // 
            // pnlKpiCaEstime
            // 
            pnlKpiCaEstime.Controls.Add(lblCaEstimeVal);
            pnlKpiCaEstime.Controls.Add(lblCaEstimeTitle);
            pnlKpiCaEstime.Location = new Point(925, 10);
            pnlKpiCaEstime.Name = "pnlKpiCaEstime";
            pnlKpiCaEstime.Size = new Size(290, 65);
            pnlKpiCaEstime.TabIndex = 3;
            // 
            // lblCaEstimeTitle
            // 
            lblCaEstimeTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCaEstimeTitle.Appearance.ForeColor = Color.Gray;
            lblCaEstimeTitle.Appearance.Options.UseFont = true;
            lblCaEstimeTitle.Appearance.Options.UseForeColor = true;
            lblCaEstimeTitle.Location = new Point(12, 8);
            lblCaEstimeTitle.Name = "lblCaEstimeTitle";
            lblCaEstimeTitle.Size = new Size(168, 13);
            lblCaEstimeTitle.TabIndex = 0;
            lblCaEstimeTitle.Text = "CA PRÉVISIONNEL ESTIMÉ (DA)";
            // 
            // lblCaEstimeVal
            // 
            lblCaEstimeVal.Appearance.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCaEstimeVal.Appearance.ForeColor = Color.SeaGreen;
            lblCaEstimeVal.Appearance.Options.UseFont = true;
            lblCaEstimeVal.Appearance.Options.UseForeColor = true;
            lblCaEstimeVal.Location = new Point(12, 28);
            lblCaEstimeVal.Name = "lblCaEstimeVal";
            lblCaEstimeVal.Size = new Size(68, 25);
            lblCaEstimeVal.TabIndex = 1;
            lblCaEstimeVal.Text = "0.00 DA";
            // 
            // gridForecast
            // 
            gridForecast.Dock = DockStyle.Fill;
            gridForecast.Location = new Point(0, 176);
            gridForecast.MainView = viewForecast;
            gridForecast.Name = "gridForecast";
            gridForecast.Size = new Size(1300, 600);
            gridForecast.TabIndex = 6;
            gridForecast.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { viewForecast });
            // 
            // viewForecast
            // 
            viewForecast.GridControl = gridForecast;
            viewForecast.Name = "viewForecast";
            viewForecast.OptionsBehavior.Editable = false;
            viewForecast.OptionsView.ShowFooter = true;
            // 
            // PrevisionChroniquesUc
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridForecast);
            Controls.Add(panelKpis);
            Controls.Add(panelHeader);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            Name = "PrevisionChroniquesUc";
            Size = new Size(1300, 776);
            ((System.ComponentModel.ISupportInitialize)barManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)panelKpis).EndInit();
            panelKpis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pnlKpiCaEstime).EndInit();
            pnlKpiCaEstime.ResumeLayout(false);
            pnlKpiCaEstime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiTotal).EndInit();
            pnlKpiTotal.ResumeLayout(false);
            pnlKpiTotal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiOverdue).EndInit();
            pnlKpiOverdue.ResumeLayout(false);
            pnlKpiOverdue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiThisWeek).EndInit();
            pnlKpiThisWeek.ResumeLayout(false);
            pnlKpiThisWeek.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridForecast).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewForecast).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnRefresh;
        private DevExpress.XtraBars.BarButtonItem btnFilterAll;
        private DevExpress.XtraBars.BarButtonItem btnFilterOverdue;
        private DevExpress.XtraBars.BarButtonItem btnFilterThisWeek;
        private DevExpress.XtraBars.BarButtonItem btnExport;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelHeader;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl lblSubtitle;
        private DevExpress.XtraEditors.PanelControl panelKpis;
        private DevExpress.XtraEditors.PanelControl pnlKpiTotal;
        private DevExpress.XtraEditors.LabelControl lblTotalVal;
        private DevExpress.XtraEditors.LabelControl lblTotalTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiOverdue;
        private DevExpress.XtraEditors.LabelControl lblOverdueVal;
        private DevExpress.XtraEditors.LabelControl lblOverdueTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiThisWeek;
        private DevExpress.XtraEditors.LabelControl lblThisWeekVal;
        private DevExpress.XtraEditors.LabelControl lblThisWeekTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiCaEstime;
        private DevExpress.XtraEditors.LabelControl lblCaEstimeVal;
        private DevExpress.XtraEditors.LabelControl lblCaEstimeTitle;
        private DevExpress.XtraGrid.GridControl gridForecast;
        private DevExpress.XtraGrid.Views.Grid.GridView viewForecast;
    }
}

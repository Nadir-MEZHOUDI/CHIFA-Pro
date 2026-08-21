namespace CHIFA.Pro.Views
{
    partial class RejetsUc
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
            btnExport = new DevExpress.XtraBars.BarButtonItem();
            barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            panelHeader = new DevExpress.XtraEditors.PanelControl();
            lblSubtitle = new DevExpress.XtraEditors.LabelControl();
            lblTitle = new DevExpress.XtraEditors.LabelControl();
            panelKpis = new DevExpress.XtraEditors.PanelControl();
            pnlTotalRejet = new DevExpress.XtraEditors.PanelControl();
            lblTotalRejetVal = new DevExpress.XtraEditors.LabelControl();
            lblTotalRejetTitle = new DevExpress.XtraEditors.LabelControl();
            pnlTotalFacture = new DevExpress.XtraEditors.PanelControl();
            lblTotalFactureVal = new DevExpress.XtraEditors.LabelControl();
            lblTotalFactureTitle = new DevExpress.XtraEditors.LabelControl();
            pnlTotalVirement = new DevExpress.XtraEditors.PanelControl();
            lblTotalVirementVal = new DevExpress.XtraEditors.LabelControl();
            lblTotalVirementTitle = new DevExpress.XtraEditors.LabelControl();
            gridRejets = new DevExpress.XtraGrid.GridControl();
            viewRejets = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).BeginInit();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelKpis).BeginInit();
            panelKpis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlTotalRejet).BeginInit();
            pnlTotalRejet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlTotalFacture).BeginInit();
            pnlTotalFacture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlTotalVirement).BeginInit();
            pnlTotalVirement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridRejets).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewRejets).BeginInit();
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
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { btnRefresh, txtDateFrom, txtDateTo, btnClearDates, btnExport });
            barManager1.MaxItemId = 5;
            barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repoDateFrom, repoDateTo });
            // 
            // bar1
            // 
            bar1.BarName = "Tools";
            bar1.DockCol = 0;
            bar1.DockRow = 0;
            bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
                new DevExpress.XtraBars.LinkPersistInfo(btnRefresh),
                new DevExpress.XtraBars.LinkPersistInfo(txtDateFrom),
                new DevExpress.XtraBars.LinkPersistInfo(txtDateTo),
                new DevExpress.XtraBars.LinkPersistInfo(btnClearDates),
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
            // txtDateFrom
            // 
            txtDateFrom.Caption = "Du";
            txtDateFrom.Edit = repoDateFrom;
            txtDateFrom.EditWidth = 110;
            txtDateFrom.Id = 1;
            txtDateFrom.Name = "txtDateFrom";
            // 
            // repoDateFrom
            // 
            repoDateFrom.AutoHeight = false;
            repoDateFrom.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
            });
            repoDateFrom.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
            });
            repoDateFrom.Name = "repoDateFrom";
            // 
            // txtDateTo
            // 
            txtDateTo.Caption = "Au";
            txtDateTo.Edit = repoDateTo;
            txtDateTo.EditWidth = 110;
            txtDateTo.Id = 2;
            txtDateTo.Name = "txtDateTo";
            // 
            // repoDateTo
            // 
            repoDateTo.AutoHeight = false;
            repoDateTo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
            });
            repoDateTo.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
                new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)
            });
            repoDateTo.Name = "repoDateTo";
            // 
            // btnClearDates
            // 
            btnClearDates.Caption = "Toutes les dates";
            btnClearDates.Id = 3;
            btnClearDates.Name = "btnClearDates";
            btnClearDates.ItemClick += BtnClearDates_ItemClick;
            // 
            // btnExport
            // 
            btnExport.Caption = "Exporter Rejets (Excel/PDF)";
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
            lblTitle.Size = new Size(420, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "GESTION DES REJETS & SUIVI DU RECOUVREMENT";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Appearance.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitle.Appearance.ForeColor = Color.Gray;
            lblSubtitle.Appearance.Options.UseFont = true;
            lblSubtitle.Appearance.Options.UseForeColor = true;
            lblSubtitle.Location = new Point(16, 35);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(470, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Rapprochement facturé vs viré, détection des écarts CNAS/CASNOS et dossiers de régularisation";
            // 
            // panelKpis
            // 
            panelKpis.Controls.Add(pnlTotalVirement);
            panelKpis.Controls.Add(pnlTotalFacture);
            panelKpis.Controls.Add(pnlTotalRejet);
            panelKpis.Dock = DockStyle.Top;
            panelKpis.Location = new Point(0, 91);
            panelKpis.Name = "panelKpis";
            panelKpis.Size = new Size(1300, 75);
            panelKpis.TabIndex = 5;
            // 
            // pnlTotalRejet
            // 
            pnlTotalRejet.Controls.Add(lblTotalRejetVal);
            pnlTotalRejet.Controls.Add(lblTotalRejetTitle);
            pnlTotalRejet.Location = new Point(12, 8);
            pnlTotalRejet.Name = "pnlTotalRejet";
            pnlTotalRejet.Size = new Size(350, 58);
            pnlTotalRejet.TabIndex = 0;
            // 
            // lblTotalRejetTitle
            // 
            lblTotalRejetTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalRejetTitle.Appearance.ForeColor = Color.Gray;
            lblTotalRejetTitle.Appearance.Options.UseFont = true;
            lblTotalRejetTitle.Appearance.Options.UseForeColor = true;
            lblTotalRejetTitle.Location = new Point(12, 6);
            lblTotalRejetTitle.Name = "lblTotalRejetTitle";
            lblTotalRejetTitle.Size = new Size(185, 13);
            lblTotalRejetTitle.TabIndex = 0;
            lblTotalRejetTitle.Text = "TOTAL REJETÉ / À RECOUVRER (DA)";
            // 
            // lblTotalRejetVal
            // 
            lblTotalRejetVal.Appearance.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalRejetVal.Appearance.ForeColor = Color.Crimson;
            lblTotalRejetVal.Appearance.Options.UseFont = true;
            lblTotalRejetVal.Appearance.Options.UseForeColor = true;
            lblTotalRejetVal.Location = new Point(12, 24);
            lblTotalRejetVal.Name = "lblTotalRejetVal";
            lblTotalRejetVal.Size = new Size(62, 23);
            lblTotalRejetVal.TabIndex = 1;
            lblTotalRejetVal.Text = "0.00 DA";
            // 
            // pnlTotalFacture
            // 
            pnlTotalFacture.Controls.Add(lblTotalFactureVal);
            pnlTotalFacture.Controls.Add(lblTotalFactureTitle);
            pnlTotalFacture.Location = new Point(380, 8);
            pnlTotalFacture.Name = "pnlTotalFacture";
            pnlTotalFacture.Size = new Size(350, 58);
            pnlTotalFacture.TabIndex = 1;
            // 
            // lblTotalFactureTitle
            // 
            lblTotalFactureTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalFactureTitle.Appearance.ForeColor = Color.Gray;
            lblTotalFactureTitle.Appearance.Options.UseFont = true;
            lblTotalFactureTitle.Appearance.Options.UseForeColor = true;
            lblTotalFactureTitle.Location = new Point(12, 6);
            lblTotalFactureTitle.Name = "lblTotalFactureTitle";
            lblTotalFactureTitle.Size = new Size(140, 13);
            lblTotalFactureTitle.TabIndex = 0;
            lblTotalFactureTitle.Text = "TOTAL PART CAISSE FACTURÉ";
            // 
            // lblTotalFactureVal
            // 
            lblTotalFactureVal.Appearance.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalFactureVal.Appearance.ForeColor = Color.RoyalBlue;
            lblTotalFactureVal.Appearance.Options.UseFont = true;
            lblTotalFactureVal.Appearance.Options.UseForeColor = true;
            lblTotalFactureVal.Location = new Point(12, 24);
            lblTotalFactureVal.Name = "lblTotalFactureVal";
            lblTotalFactureVal.Size = new Size(62, 23);
            lblTotalFactureVal.TabIndex = 1;
            lblTotalFactureVal.Text = "0.00 DA";
            // 
            // pnlTotalVirement
            // 
            pnlTotalVirement.Controls.Add(lblTotalVirementVal);
            pnlTotalVirement.Controls.Add(lblTotalVirementTitle);
            pnlTotalVirement.Location = new Point(750, 8);
            pnlTotalVirement.Name = "pnlTotalVirement";
            pnlTotalVirement.Size = new Size(350, 58);
            pnlTotalVirement.TabIndex = 2;
            // 
            // lblTotalVirementTitle
            // 
            lblTotalVirementTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalVirementTitle.Appearance.ForeColor = Color.Gray;
            lblTotalVirementTitle.Appearance.Options.UseFont = true;
            lblTotalVirementTitle.Appearance.Options.UseForeColor = true;
            lblTotalVirementTitle.Location = new Point(12, 6);
            lblTotalVirementTitle.Name = "lblTotalVirementTitle";
            lblTotalVirementTitle.Size = new Size(137, 13);
            lblTotalVirementTitle.TabIndex = 0;
            lblTotalVirementTitle.Text = "TOTAL VIREMENTS REÇUS";
            // 
            // lblTotalVirementVal
            // 
            lblTotalVirementVal.Appearance.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalVirementVal.Appearance.ForeColor = Color.SeaGreen;
            lblTotalVirementVal.Appearance.Options.UseFont = true;
            lblTotalVirementVal.Appearance.Options.UseForeColor = true;
            lblTotalVirementVal.Location = new Point(12, 24);
            lblTotalVirementVal.Name = "lblTotalVirementVal";
            lblTotalVirementVal.Size = new Size(62, 23);
            lblTotalVirementVal.TabIndex = 1;
            lblTotalVirementVal.Text = "0.00 DA";
            // 
            // gridRejets
            // 
            gridRejets.Dock = DockStyle.Fill;
            gridRejets.Location = new Point(0, 166);
            gridRejets.MainView = viewRejets;
            gridRejets.Name = "gridRejets";
            gridRejets.Size = new Size(1300, 610);
            gridRejets.TabIndex = 6;
            gridRejets.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { viewRejets });
            // 
            // viewRejets
            // 
            viewRejets.GridControl = gridRejets;
            viewRejets.Name = "viewRejets";
            viewRejets.OptionsBehavior.Editable = false;
            viewRejets.OptionsView.ShowFooter = true;
            // 
            // RejetsUc
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridRejets);
            Controls.Add(panelKpis);
            Controls.Add(panelHeader);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            Name = "RejetsUc";
            Size = new Size(1300, 776);
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
            ((System.ComponentModel.ISupportInitialize)pnlTotalRejet).EndInit();
            pnlTotalRejet.ResumeLayout(false);
            pnlTotalRejet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlTotalFacture).EndInit();
            pnlTotalFacture.ResumeLayout(false);
            pnlTotalFacture.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlTotalVirement).EndInit();
            pnlTotalVirement.ResumeLayout(false);
            pnlTotalVirement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridRejets).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewRejets).EndInit();
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
        private DevExpress.XtraBars.BarButtonItem btnExport;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelHeader;
        private DevExpress.XtraEditors.LabelControl lblTitle;
        private DevExpress.XtraEditors.LabelControl lblSubtitle;
        private DevExpress.XtraEditors.PanelControl panelKpis;
        private DevExpress.XtraEditors.PanelControl pnlTotalRejet;
        private DevExpress.XtraEditors.LabelControl lblTotalRejetVal;
        private DevExpress.XtraEditors.LabelControl lblTotalRejetTitle;
        private DevExpress.XtraEditors.PanelControl pnlTotalFacture;
        private DevExpress.XtraEditors.LabelControl lblTotalFactureVal;
        private DevExpress.XtraEditors.LabelControl lblTotalFactureTitle;
        private DevExpress.XtraEditors.PanelControl pnlTotalVirement;
        private DevExpress.XtraEditors.LabelControl lblTotalVirementVal;
        private DevExpress.XtraEditors.LabelControl lblTotalVirementTitle;
        private DevExpress.XtraGrid.GridControl gridRejets;
        private DevExpress.XtraGrid.Views.Grid.GridView viewRejets;
    }
}

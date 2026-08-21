namespace CHIFA.Pro.Views
{
    partial class AuditBordereauUc
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
            btnRunAudit = new DevExpress.XtraBars.BarButtonItem();
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
            panelHeader = new PanelControl();
            lblSubtitle = new LabelControl();
            lblTitle = new LabelControl();
            panelKpis = new PanelControl();
            pnlKpiRisque = new PanelControl();
            lblRisqueVal = new LabelControl();
            lblRisqueTitle = new LabelControl();
            pnlKpiAnomalies = new PanelControl();
            lblAnomaliesVal = new LabelControl();
            lblAnomaliesTitle = new LabelControl();
            pnlKpiConformes = new PanelControl();
            lblConformesVal = new LabelControl();
            lblConformesTitle = new LabelControl();
            gridAnomalies = new DevExpress.XtraGrid.GridControl();
            viewAnomalies = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).BeginInit();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelKpis).BeginInit();
            panelKpis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiRisque).BeginInit();
            pnlKpiRisque.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiAnomalies).BeginInit();
            pnlKpiAnomalies.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiConformes).BeginInit();
            pnlKpiConformes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridAnomalies).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewAnomalies).BeginInit();
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
            barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] { btnRunAudit, txtDateFrom, txtDateTo, btnClearDates, btnExport });
            barManager1.MaxItemId = 5;
            barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] { repoDateFrom, repoDateTo });
            // 
            // bar1
            // 
            bar1.BarName = "Tools";
            bar1.DockCol = 0;
            bar1.DockRow = 0;
            bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] { new DevExpress.XtraBars.LinkPersistInfo(btnRunAudit), new DevExpress.XtraBars.LinkPersistInfo(txtDateFrom), new DevExpress.XtraBars.LinkPersistInfo(txtDateTo), new DevExpress.XtraBars.LinkPersistInfo(btnClearDates), new DevExpress.XtraBars.LinkPersistInfo(btnExport) });
            bar1.OptionsBar.AllowQuickCustomization = false;
            bar1.OptionsBar.DrawDragBorder = false;
            bar1.OptionsBar.UseWholeRow = true;
            bar1.Text = "Tools";
            // 
            // btnRunAudit
            // 
            btnRunAudit.Caption = "Lancer l'Audit de Conformité";
            btnRunAudit.Id = 0;
            btnRunAudit.Name = "btnRunAudit";
            btnRunAudit.ItemClick += BtnRunAudit_ItemClick;
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
            repoDateFrom.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repoDateFrom.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
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
            repoDateTo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            repoDateTo.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
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
            btnExport.Caption = "Exporter Rapport d'Audit (Excel/PDF)";
            btnExport.Id = 4;
            btnExport.Name = "btnExport";
            btnExport.ItemClick += BtnExport_ItemClick;
            // 
            // barDockControlTop
            // 
            barDockControlTop.CausesValidation = false;
            barDockControlTop.Dock = DockStyle.Top;
            barDockControlTop.Location = new Point(0, 0);
            barDockControlTop.Manager = barManager1;
            barDockControlTop.Margin = new Padding(4, 4, 4, 4);
            barDockControlTop.Size = new Size(1671, 39);
            // 
            // barDockControlBottom
            // 
            barDockControlBottom.CausesValidation = false;
            barDockControlBottom.Dock = DockStyle.Bottom;
            barDockControlBottom.Location = new Point(0, 1087);
            barDockControlBottom.Manager = barManager1;
            barDockControlBottom.Margin = new Padding(4, 4, 4, 4);
            barDockControlBottom.Size = new Size(1671, 0);
            // 
            // barDockControlLeft
            // 
            barDockControlLeft.CausesValidation = false;
            barDockControlLeft.Dock = DockStyle.Left;
            barDockControlLeft.Location = new Point(0, 39);
            barDockControlLeft.Manager = barManager1;
            barDockControlLeft.Margin = new Padding(4, 4, 4, 4);
            barDockControlLeft.Size = new Size(0, 1048);
            // 
            // barDockControlRight
            // 
            barDockControlRight.CausesValidation = false;
            barDockControlRight.Dock = DockStyle.Right;
            barDockControlRight.Location = new Point(1671, 39);
            barDockControlRight.Manager = barManager1;
            barDockControlRight.Margin = new Padding(4, 4, 4, 4);
            barDockControlRight.Size = new Size(0, 1048);
            // 
            // panelHeader
            // 
            panelHeader.Controls.Add(lblSubtitle);
            panelHeader.Controls.Add(lblTitle);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 39);
            panelHeader.Margin = new Padding(4, 4, 4, 4);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1671, 84);
            panelHeader.TabIndex = 4;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Appearance.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitle.Appearance.ForeColor = Color.Gray;
            lblSubtitle.Appearance.Options.UseFont = true;
            lblSubtitle.Appearance.Options.UseForeColor = true;
            lblSubtitle.Location = new Point(21, 49);
            lblSubtitle.Margin = new Padding(4, 4, 4, 4);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(663, 20);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Détection automatique des anomalies de tarification, dépassements et risques de rejet avant clôture";
            // 
            // lblTitle
            // 
            lblTitle.Appearance.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Location = new Point(21, 12);
            lblTitle.Margin = new Padding(4, 4, 4, 4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(580, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "AUDIT PRÉ-BORDEREAU & CONTRÔLE CONFORMITÉ";
            // 
            // panelKpis
            // 
            panelKpis.Controls.Add(pnlKpiRisque);
            panelKpis.Controls.Add(pnlKpiAnomalies);
            panelKpis.Controls.Add(pnlKpiConformes);
            panelKpis.Dock = DockStyle.Top;
            panelKpis.Location = new Point(0, 123);
            panelKpis.Margin = new Padding(4, 4, 4, 4);
            panelKpis.Name = "panelKpis";
            panelKpis.Size = new Size(1671, 105);
            panelKpis.TabIndex = 5;
            // 
            // pnlKpiRisque
            // 
            pnlKpiRisque.Controls.Add(lblRisqueVal);
            pnlKpiRisque.Controls.Add(lblRisqueTitle);
            pnlKpiRisque.Location = new Point(964, 12);
            pnlKpiRisque.Margin = new Padding(4, 4, 4, 4);
            pnlKpiRisque.Name = "pnlKpiRisque";
            pnlKpiRisque.Size = new Size(450, 81);
            pnlKpiRisque.TabIndex = 2;
            // 
            // lblRisqueVal
            // 
            lblRisqueVal.Appearance.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRisqueVal.Appearance.ForeColor = Color.DarkOrange;
            lblRisqueVal.Appearance.Options.UseFont = true;
            lblRisqueVal.Appearance.Options.UseForeColor = true;
            lblRisqueVal.Location = new Point(15, 34);
            lblRisqueVal.Margin = new Padding(4, 4, 4, 4);
            lblRisqueVal.Name = "lblRisqueVal";
            lblRisqueVal.Size = new Size(82, 30);
            lblRisqueVal.TabIndex = 1;
            lblRisqueVal.Text = "0.00 DA";
            // 
            // lblRisqueTitle
            // 
            lblRisqueTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRisqueTitle.Appearance.ForeColor = Color.Gray;
            lblRisqueTitle.Appearance.Options.UseFont = true;
            lblRisqueTitle.Appearance.Options.UseForeColor = true;
            lblRisqueTitle.Location = new Point(15, 8);
            lblRisqueTitle.Margin = new Padding(4, 4, 4, 4);
            lblRisqueTitle.Name = "lblRisqueTitle";
            lblRisqueTitle.Size = new Size(201, 19);
            lblRisqueTitle.TabIndex = 0;
            lblRisqueTitle.Text = "MONTANT À RISQUE DE REJET";
            // 
            // pnlKpiAnomalies
            // 
            pnlKpiAnomalies.Controls.Add(lblAnomaliesVal);
            pnlKpiAnomalies.Controls.Add(lblAnomaliesTitle);
            pnlKpiAnomalies.Location = new Point(489, 12);
            pnlKpiAnomalies.Margin = new Padding(4, 4, 4, 4);
            pnlKpiAnomalies.Name = "pnlKpiAnomalies";
            pnlKpiAnomalies.Size = new Size(450, 81);
            pnlKpiAnomalies.TabIndex = 1;
            // 
            // lblAnomaliesVal
            // 
            lblAnomaliesVal.Appearance.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAnomaliesVal.Appearance.ForeColor = Color.Crimson;
            lblAnomaliesVal.Appearance.Options.UseFont = true;
            lblAnomaliesVal.Appearance.Options.UseForeColor = true;
            lblAnomaliesVal.Location = new Point(15, 34);
            lblAnomaliesVal.Margin = new Padding(4, 4, 4, 4);
            lblAnomaliesVal.Name = "lblAnomaliesVal";
            lblAnomaliesVal.Size = new Size(13, 30);
            lblAnomaliesVal.TabIndex = 1;
            lblAnomaliesVal.Text = "0";
            // 
            // lblAnomaliesTitle
            // 
            lblAnomaliesTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAnomaliesTitle.Appearance.ForeColor = Color.Gray;
            lblAnomaliesTitle.Appearance.Options.UseFont = true;
            lblAnomaliesTitle.Appearance.Options.UseForeColor = true;
            lblAnomaliesTitle.Location = new Point(15, 8);
            lblAnomaliesTitle.Margin = new Padding(4, 4, 4, 4);
            lblAnomaliesTitle.Name = "lblAnomaliesTitle";
            lblAnomaliesTitle.Size = new Size(192, 19);
            lblAnomaliesTitle.TabIndex = 0;
            lblAnomaliesTitle.Text = "FACTURES AVEC ANOMALIES";
            // 
            // pnlKpiConformes
            // 
            pnlKpiConformes.Controls.Add(lblConformesVal);
            pnlKpiConformes.Controls.Add(lblConformesTitle);
            pnlKpiConformes.Location = new Point(15, 12);
            pnlKpiConformes.Margin = new Padding(4, 4, 4, 4);
            pnlKpiConformes.Name = "pnlKpiConformes";
            pnlKpiConformes.Size = new Size(450, 81);
            pnlKpiConformes.TabIndex = 0;
            // 
            // lblConformesVal
            // 
            lblConformesVal.Appearance.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConformesVal.Appearance.ForeColor = Color.SeaGreen;
            lblConformesVal.Appearance.Options.UseFont = true;
            lblConformesVal.Appearance.Options.UseForeColor = true;
            lblConformesVal.Location = new Point(15, 34);
            lblConformesVal.Margin = new Padding(4, 4, 4, 4);
            lblConformesVal.Name = "lblConformesVal";
            lblConformesVal.Size = new Size(13, 30);
            lblConformesVal.TabIndex = 1;
            lblConformesVal.Text = "0";
            // 
            // lblConformesTitle
            // 
            lblConformesTitle.Appearance.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConformesTitle.Appearance.ForeColor = Color.Gray;
            lblConformesTitle.Appearance.Options.UseFont = true;
            lblConformesTitle.Appearance.Options.UseForeColor = true;
            lblConformesTitle.Location = new Point(15, 8);
            lblConformesTitle.Margin = new Padding(4, 4, 4, 4);
            lblConformesTitle.Name = "lblConformesTitle";
            lblConformesTitle.Size = new Size(158, 19);
            lblConformesTitle.TabIndex = 0;
            lblConformesTitle.Text = "FACTURES CONFORMES";
            // 
            // gridAnomalies
            // 
            gridAnomalies.Dock = DockStyle.Fill;
            gridAnomalies.EmbeddedNavigator.Margin = new Padding(4, 4, 4, 4);
            gridAnomalies.Location = new Point(0, 228);
            gridAnomalies.MainView = viewAnomalies;
            gridAnomalies.Margin = new Padding(4, 4, 4, 4);
            gridAnomalies.Name = "gridAnomalies";
            gridAnomalies.Size = new Size(1671, 859);
            gridAnomalies.TabIndex = 6;
            gridAnomalies.ViewCollection.AddRange(new BaseView[] { viewAnomalies });
            // 
            // viewAnomalies
            // 
            viewAnomalies.DetailHeight = 490;
            viewAnomalies.GridControl = gridAnomalies;
            viewAnomalies.Name = "viewAnomalies";
            viewAnomalies.OptionsBehavior.Editable = false;
            viewAnomalies.OptionsEditForm.PopupEditFormWidth = 1029;
            viewAnomalies.OptionsView.ShowFooter = true;
            // 
            // AuditBordereauUc
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridAnomalies);
            Controls.Add(panelKpis);
            Controls.Add(panelHeader);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            Margin = new Padding(4, 4, 4, 4);
            Name = "AuditBordereauUc";
            Size = new Size(1671, 1087);
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
            ((System.ComponentModel.ISupportInitialize)pnlKpiRisque).EndInit();
            pnlKpiRisque.ResumeLayout(false);
            pnlKpiRisque.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiAnomalies).EndInit();
            pnlKpiAnomalies.ResumeLayout(false);
            pnlKpiAnomalies.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pnlKpiConformes).EndInit();
            pnlKpiConformes.ResumeLayout(false);
            pnlKpiConformes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridAnomalies).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewAnomalies).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarButtonItem btnRunAudit;
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
        private DevExpress.XtraEditors.PanelControl pnlKpiConformes;
        private DevExpress.XtraEditors.LabelControl lblConformesVal;
        private DevExpress.XtraEditors.LabelControl lblConformesTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiAnomalies;
        private DevExpress.XtraEditors.LabelControl lblAnomaliesVal;
        private DevExpress.XtraEditors.LabelControl lblAnomaliesTitle;
        private DevExpress.XtraEditors.PanelControl pnlKpiRisque;
        private DevExpress.XtraEditors.LabelControl lblRisqueVal;
        private DevExpress.XtraEditors.LabelControl lblRisqueTitle;
        private DevExpress.XtraGrid.GridControl gridAnomalies;
        private DevExpress.XtraGrid.Views.Grid.GridView viewAnomalies;
    }
}

namespace CHIFA.Pro.Views
{
    partial class PsychotropesUc
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
            gridPsychotropes = new DevExpress.XtraGrid.GridControl();
            viewPsychotropes = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)barManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo.CalendarTimeProperties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).BeginInit();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridPsychotropes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)viewPsychotropes).BeginInit();
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
            btnExport.Caption = "Exporter Registre (Excel/PDF)";
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
            lblTitle.Size = new Size(475, 25);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "REGISTRE RÉGLEMENTAIRE DES PSYCHOTROPES";
            // 
            // lblSubtitle
            // 
            lblSubtitle.Appearance.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitle.Appearance.ForeColor = Color.Gray;
            lblSubtitle.Appearance.Options.UseFont = true;
            lblSubtitle.Appearance.Options.UseForeColor = true;
            lblSubtitle.Location = new Point(16, 35);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(495, 15);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Suivi des substances vénéneuses, délivrances sous ordonnance sécurisée et traçabilité légale";
            // 
            // gridPsychotropes
            // 
            gridPsychotropes.Dock = DockStyle.Fill;
            gridPsychotropes.Location = new Point(0, 91);
            gridPsychotropes.MainView = viewPsychotropes;
            gridPsychotropes.Name = "gridPsychotropes";
            gridPsychotropes.Size = new Size(1300, 685);
            gridPsychotropes.TabIndex = 5;
            gridPsychotropes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { viewPsychotropes });
            // 
            // viewPsychotropes
            // 
            viewPsychotropes.GridControl = gridPsychotropes;
            viewPsychotropes.Name = "viewPsychotropes";
            viewPsychotropes.OptionsBehavior.Editable = false;
            viewPsychotropes.OptionsView.ShowFooter = true;
            // 
            // PsychotropesUc
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridPsychotropes);
            Controls.Add(panelHeader);
            Controls.Add(barDockControlLeft);
            Controls.Add(barDockControlRight);
            Controls.Add(barDockControlBottom);
            Controls.Add(barDockControlTop);
            Name = "PsychotropesUc";
            Size = new Size(1300, 776);
            ((System.ComponentModel.ISupportInitialize)barManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateFrom).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo.CalendarTimeProperties).EndInit();
            ((System.ComponentModel.ISupportInitialize)repoDateTo).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelHeader).EndInit();
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)gridPsychotropes).EndInit();
            ((System.ComponentModel.ISupportInitialize)viewPsychotropes).EndInit();
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
        private DevExpress.XtraGrid.GridControl gridPsychotropes;
        private DevExpress.XtraGrid.Views.Grid.GridView viewPsychotropes;
    }
}

namespace CHIFA.Pro.Views
{
    partial class NomenclaturUc
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
            gridControl = new DevExpress.XtraGrid.GridControl();
            medicDtoBindingSource = new BindingSource(components);
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colNEnr = new DevExpress.XtraGrid.Columns.GridColumn();
            colNomCommercial = new DevExpress.XtraGrid.Columns.GridColumn();
            colDCI = new DevExpress.XtraGrid.Columns.GridColumn();
            colCOND = new DevExpress.XtraGrid.Columns.GridColumn();
            colDOSAGE = new DevExpress.XtraGrid.Columns.GridColumn();
            colFORME = new DevExpress.XtraGrid.Columns.GridColumn();
            colTR = new DevExpress.XtraGrid.Columns.GridColumn();
            colPAYS = new DevExpress.XtraGrid.Columns.GridColumn();
            colOBS = new DevExpress.XtraGrid.Columns.GridColumn();
            colGeneric = new DevExpress.XtraGrid.Columns.GridColumn();
            colCodeDCI = new DevExpress.XtraGrid.Columns.GridColumn();
            colCodeMedic = new DevExpress.XtraGrid.Columns.GridColumn();
            dockManager1 = new DevExpress.XtraBars.Docking.DockManager(components);
            dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            txtObs = new MemoEdit();
            ((System.ComponentModel.ISupportInitialize)gridControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)medicDtoBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dockManager1).BeginInit();
            dockPanel1.SuspendLayout();
            dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtObs.Properties).BeginInit();
            SuspendLayout();
            // 
            // gridControl
            // 
            gridControl.DataSource = medicDtoBindingSource;
            gridControl.Dock = DockStyle.Fill;
            gridControl.EmbeddedNavigator.Margin = new Padding(1);
            gridControl.Location = new Point(0, 0);
            gridControl.MainView = gridView1;
            gridControl.Margin = new Padding(1);
            gridControl.Name = "gridControl";
            gridControl.Size = new Size(1259, 601);
            gridControl.TabIndex = 1;
            gridControl.ViewCollection.AddRange(new BaseView[] { gridView1 });
            // 
            // medicDtoBindingSource
            // 
            medicDtoBindingSource.DataSource = typeof(MedicDto);
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colNEnr, colNomCommercial, colDCI, colCOND, colDOSAGE, colFORME, colTR, colPAYS, colOBS, colGeneric, colCodeDCI, colCodeMedic });
            gridView1.DetailHeight = 262;
            gridView1.GridControl = gridControl;
            gridView1.Name = "gridView1";
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsEditForm.PopupEditFormWidth = 1029;
            gridView1.OptionsView.ShowFooter = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.CustomDrawCell += gridView1_CustomDrawCell;
            gridView1.FocusedRowChanged += gridView1_FocusedRowChanged;
            // 
            // colNEnr
            // 
            colNEnr.FieldName = "NEnr";
            colNEnr.MinWidth = 17;
            colNEnr.Name = "colNEnr";
            colNEnr.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "NEnr", "{0}") });
            colNEnr.Width = 60;
            // 
            // colNomCommercial
            // 
            colNomCommercial.AppearanceCell.BackColor = Color.FromArgb(192, 255, 192);
            colNomCommercial.AppearanceCell.Options.UseBackColor = true;
            colNomCommercial.FieldName = "NomCommercial";
            colNomCommercial.MinWidth = 17;
            colNomCommercial.Name = "colNomCommercial";
            colNomCommercial.Visible = true;
            colNomCommercial.VisibleIndex = 1;
            colNomCommercial.Width = 284;
            // 
            // colDCI
            // 
            colDCI.FieldName = "DCI";
            colDCI.MinWidth = 17;
            colDCI.Name = "colDCI";
            colDCI.Visible = true;
            colDCI.VisibleIndex = 2;
            colDCI.Width = 235;
            // 
            // colCOND
            // 
            colCOND.FieldName = "COND";
            colCOND.MinWidth = 17;
            colCOND.Name = "colCOND";
            colCOND.Visible = true;
            colCOND.VisibleIndex = 3;
            colCOND.Width = 53;
            // 
            // colDOSAGE
            // 
            colDOSAGE.FieldName = "DOSAGE";
            colDOSAGE.MinWidth = 17;
            colDOSAGE.Name = "colDOSAGE";
            colDOSAGE.Visible = true;
            colDOSAGE.VisibleIndex = 4;
            colDOSAGE.Width = 68;
            // 
            // colFORME
            // 
            colFORME.FieldName = "FORME";
            colFORME.MinWidth = 17;
            colFORME.Name = "colFORME";
            colFORME.Visible = true;
            colFORME.VisibleIndex = 5;
            colFORME.Width = 59;
            // 
            // colTR
            // 
            colTR.FieldName = "TR";
            colTR.MinWidth = 17;
            colTR.Name = "colTR";
            colTR.Visible = true;
            colTR.VisibleIndex = 6;
            colTR.Width = 41;
            // 
            // colPAYS
            // 
            colPAYS.FieldName = "PAYS";
            colPAYS.MinWidth = 17;
            colPAYS.Name = "colPAYS";
            colPAYS.Visible = true;
            colPAYS.VisibleIndex = 7;
            colPAYS.Width = 50;
            // 
            // colOBS
            // 
            colOBS.FieldName = "OBS";
            colOBS.MinWidth = 17;
            colOBS.Name = "colOBS";
            colOBS.Width = 64;
            // 
            // colGeneric
            // 
            colGeneric.FieldName = "Generic";
            colGeneric.MinWidth = 17;
            colGeneric.Name = "colGeneric";
            colGeneric.Visible = true;
            colGeneric.VisibleIndex = 8;
            colGeneric.Width = 67;
            // 
            // colCodeDCI
            // 
            colCodeDCI.FieldName = "CodeDCI";
            colCodeDCI.MinWidth = 22;
            colCodeDCI.Name = "colCodeDCI";
            colCodeDCI.Visible = true;
            colCodeDCI.VisibleIndex = 0;
            colCodeDCI.Width = 85;
            // 
            // colCodeMedic
            // 
            colCodeMedic.FieldName = "CodeMedic";
            colCodeMedic.MinWidth = 22;
            colCodeMedic.Name = "colCodeMedic";
            colCodeMedic.Width = 85;
            // 
            // dockManager1
            // 
            dockManager1.Form = this;
            dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] { dockPanel1 });
            dockManager1.TopZIndexControls.AddRange(new string[] { "DevExpress.XtraBars.BarDockControl", "DevExpress.XtraBars.StandaloneBarDockControl", "System.Windows.Forms.StatusBar", "System.Windows.Forms.MenuStrip", "System.Windows.Forms.StatusStrip", "DevExpress.XtraBars.Ribbon.RibbonStatusBar", "DevExpress.XtraBars.Ribbon.RibbonControl", "DevExpress.XtraBars.Navigation.OfficeNavigationBar", "DevExpress.XtraBars.Navigation.TileNavPane", "DevExpress.XtraBars.TabFormControl", "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl", "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl" });
            // 
            // dockPanel1
            // 
            dockPanel1.Controls.Add(dockPanel1_Container);
            dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            dockPanel1.ID = new Guid("476e82c9-ca1b-43ca-9cc1-51f1c74e4628");
            dockPanel1.Location = new Point(0, 601);
            dockPanel1.Margin = new Padding(1);
            dockPanel1.Name = "dockPanel1";
            dockPanel1.Options.ShowCloseButton = false;
            dockPanel1.OriginalSize = new Size(250, 156);
            dockPanel1.Size = new Size(1259, 156);
            dockPanel1.Text = "Observations";
            // 
            // dockPanel1_Container
            // 
            dockPanel1_Container.Controls.Add(txtObs);
            dockPanel1_Container.Location = new Point(5, 32);
            dockPanel1_Container.Margin = new Padding(1);
            dockPanel1_Container.Name = "dockPanel1_Container";
            dockPanel1_Container.Size = new Size(1249, 119);
            dockPanel1_Container.TabIndex = 0;
            // 
            // txtObs
            // 
            txtObs.Dock = DockStyle.Fill;
            txtObs.Location = new Point(0, 0);
            txtObs.Margin = new Padding(1);
            txtObs.Name = "txtObs";
            txtObs.Properties.Appearance.Options.UseTextOptions = true;
            txtObs.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            txtObs.Properties.ReadOnly = true;
            txtObs.Size = new Size(1249, 119);
            txtObs.TabIndex = 0;
            // 
            // NomenclaturUc
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl);
            Controls.Add(dockPanel1);
            Margin = new Padding(5, 4, 5, 4);
            Name = "NomenclaturUc";
            Size = new Size(1259, 757);
            Load += NomenclaturUc_Load;
            ((System.ComponentModel.ISupportInitialize)gridControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)medicDtoBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dockManager1).EndInit();
            dockPanel1.ResumeLayout(false);
            dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)txtObs.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private MemoEdit txtObs;
        private BindingSource medicDtoBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colNEnr;
        private DevExpress.XtraGrid.Columns.GridColumn colNomCommercial;
        private DevExpress.XtraGrid.Columns.GridColumn colDCI;
        private DevExpress.XtraGrid.Columns.GridColumn colCOND;
        private DevExpress.XtraGrid.Columns.GridColumn colDOSAGE;
        private DevExpress.XtraGrid.Columns.GridColumn colFORME;
        private DevExpress.XtraGrid.Columns.GridColumn colTR;
        private DevExpress.XtraGrid.Columns.GridColumn colPAYS;
        private DevExpress.XtraGrid.Columns.GridColumn colOBS;
        private DevExpress.XtraGrid.Columns.GridColumn colGeneric;
        private DevExpress.XtraGrid.Columns.GridColumn colCodeDCI;
        private DevExpress.XtraGrid.Columns.GridColumn colCodeMedic;
    }
}

namespace CHIFA.Pro.Views
{
    partial class CentersUc
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(CentersUc));
            documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(components);
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            cENTREBindingSource = new BindingSource(components);
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colCodeCp = new DevExpress.XtraGrid.Columns.GridColumn();
            colLibelle = new DevExpress.XtraGrid.Columns.GridColumn();
            colADRESSE = new DevExpress.XtraGrid.Columns.GridColumn();
            colInBor = new DevExpress.XtraGrid.Columns.GridColumn();
            colCONV = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            noDocumentsView1 = new DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView(components);
            tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(components);
            dockManager1 = new DevExpress.XtraBars.Docking.DockManager(components);
            dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            CodeCpTextEdit = new TextEdit();
            LibelleTextEdit = new TextEdit();
            ADRESSETextEdit = new TextEdit();
            btnSave = new SimpleButton();
            btnCancel = new SimpleButton();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            ItemForCodeCp = new DevExpress.XtraLayout.LayoutControlItem();
            ItemForLibelle = new DevExpress.XtraLayout.LayoutControlItem();
            ItemForADRESSE = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)documentManager1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cENTREBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)noDocumentsView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tabbedView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dockManager1).BeginInit();
            dockPanel1.SuspendLayout();
            dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataLayoutControl1).BeginInit();
            dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CodeCpTextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LibelleTextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ADRESSETextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForCodeCp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForLibelle).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForADRESSE).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem3).BeginInit();
            SuspendLayout();
            // 
            // documentManager1
            // 
            documentManager1.ClientControl = gridControl1;
            documentManager1.View = noDocumentsView1;
            documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] { noDocumentsView1, tabbedView1 });
            // 
            // gridControl1
            // 
            gridControl1.DataSource = cENTREBindingSource;
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.EmbeddedNavigator.Margin = new Padding(4, 3, 4, 3);
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = gridView1;
            gridControl1.Margin = new Padding(4, 3, 4, 3);
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new Size(993, 614);
            gridControl1.TabIndex = 1;
            gridControl1.ViewCollection.AddRange(new BaseView[] { gridView1 });
            // 
            // cENTREBindingSource
            // 
            cENTREBindingSource.DataSource = typeof(Centre);
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colCodeCp, colLibelle, colADRESSE, colInBor, colCONV, gridColumn1 });
            gridView1.DetailHeight = 372;
            gridView1.FixedLineWidth = 3;
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 711;
            // 
            // colCodeCp
            // 
            colCodeCp.FieldName = "CodeCentre";
            colCodeCp.ImageOptions.Image = (Image)resources.GetObject("colCodeCp.ImageOptions.Image");
            colCodeCp.Name = "colCodeCp";
            colCodeCp.Visible = true;
            colCodeCp.VisibleIndex = 0;
            colCodeCp.Width = 145;
            // 
            // colLibelle
            // 
            colLibelle.FieldName = "Nom";
            colLibelle.ImageOptions.Image = (Image)resources.GetObject("colLibelle.ImageOptions.Image");
            colLibelle.Name = "colLibelle";
            colLibelle.Visible = true;
            colLibelle.VisibleIndex = 1;
            colLibelle.Width = 380;
            // 
            // colADRESSE
            // 
            colADRESSE.FieldName = "Adresse";
            colADRESSE.ImageOptions.Image = (Image)resources.GetObject("colADRESSE.ImageOptions.Image");
            colADRESSE.Name = "colADRESSE";
            colADRESSE.Visible = true;
            colADRESSE.VisibleIndex = 2;
            colADRESSE.Width = 380;
            // 
            // colInBor
            // 
            colInBor.FieldName = "InBor";
            colInBor.Name = "colInBor";
            colInBor.Width = 76;
            // 
            // colCONV
            // 
            colCONV.FieldName = "CONV";
            colCONV.Name = "colCONV";
            colCONV.Width = 76;
            // 
            // gridColumn1
            // 
            gridColumn1.FieldName = "NumBord";
            gridColumn1.ImageOptions.Image = (Image)resources.GetObject("gridColumn1.ImageOptions.Image");
            gridColumn1.MinWidth = 19;
            gridColumn1.Name = "gridColumn1";
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 3;
            gridColumn1.Width = 84;
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
            dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            dockPanel1.ID = new Guid("f9a5deaf-7656-4d79-8aac-27857c2c561f");
            dockPanel1.Location = new Point(993, 0);
            dockPanel1.Margin = new Padding(4, 3, 4, 3);
            dockPanel1.Name = "dockPanel1";
            dockPanel1.Options.ShowCloseButton = false;
            dockPanel1.OriginalSize = new Size(352, 200);
            dockPanel1.Size = new Size(352, 614);
            dockPanel1.Text = "Details";
            // 
            // dockPanel1_Container
            // 
            dockPanel1_Container.Controls.Add(dataLayoutControl1);
            dockPanel1_Container.Location = new Point(5, 25);
            dockPanel1_Container.Margin = new Padding(4, 3, 4, 3);
            dockPanel1_Container.Name = "dockPanel1_Container";
            dockPanel1_Container.Size = new Size(343, 585);
            dockPanel1_Container.TabIndex = 0;
            // 
            // dataLayoutControl1
            // 
            dataLayoutControl1.Controls.Add(CodeCpTextEdit);
            dataLayoutControl1.Controls.Add(LibelleTextEdit);
            dataLayoutControl1.Controls.Add(ADRESSETextEdit);
            dataLayoutControl1.Controls.Add(btnSave);
            dataLayoutControl1.Controls.Add(btnCancel);
            dataLayoutControl1.DataSource = cENTREBindingSource;
            dataLayoutControl1.Dock = DockStyle.Fill;
            dataLayoutControl1.Location = new Point(0, 0);
            dataLayoutControl1.Margin = new Padding(3, 2, 3, 2);
            dataLayoutControl1.Name = "dataLayoutControl1";
            dataLayoutControl1.Root = Root;
            dataLayoutControl1.Size = new Size(343, 585);
            dataLayoutControl1.TabIndex = 0;
            dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // CodeCpTextEdit
            // 
            CodeCpTextEdit.DataBindings.Add(new Binding("EditValue", cENTREBindingSource, "CodeCentre", true));
            CodeCpTextEdit.Location = new Point(11, 30);
            CodeCpTextEdit.Margin = new Padding(3, 2, 3, 2);
            CodeCpTextEdit.Name = "CodeCpTextEdit";
            CodeCpTextEdit.Size = new Size(321, 24);
            CodeCpTextEdit.StyleController = dataLayoutControl1;
            CodeCpTextEdit.TabIndex = 4;
            // 
            // LibelleTextEdit
            // 
            LibelleTextEdit.DataBindings.Add(new Binding("EditValue", cENTREBindingSource, "Nom", true));
            LibelleTextEdit.Location = new Point(11, 78);
            LibelleTextEdit.Margin = new Padding(3, 2, 3, 2);
            LibelleTextEdit.Name = "LibelleTextEdit";
            LibelleTextEdit.Size = new Size(321, 24);
            LibelleTextEdit.StyleController = dataLayoutControl1;
            LibelleTextEdit.TabIndex = 5;
            // 
            // ADRESSETextEdit
            // 
            ADRESSETextEdit.DataBindings.Add(new Binding("EditValue", cENTREBindingSource, "Adresse", true));
            ADRESSETextEdit.Location = new Point(11, 126);
            ADRESSETextEdit.Margin = new Padding(3, 2, 3, 2);
            ADRESSETextEdit.Name = "ADRESSETextEdit";
            ADRESSETextEdit.Size = new Size(321, 24);
            ADRESSETextEdit.StyleController = dataLayoutControl1;
            ADRESSETextEdit.TabIndex = 6;
            // 
            // btnSave
            // 
            btnSave.ImageOptions.Image = (Image)resources.GetObject("btnSave.ImageOptions.Image");
            btnSave.Location = new Point(11, 189);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(321, 38);
            btnSave.StyleController = dataLayoutControl1;
            btnSave.TabIndex = 7;
            btnSave.Text = "Save";
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.ImageOptions.Image = (Image)resources.GetObject("btnCancel.ImageOptions.Image");
            btnCancel.Location = new Point(11, 249);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            btnCancel.Size = new Size(321, 38);
            btnCancel.StyleController = dataLayoutControl1;
            btnCancel.TabIndex = 8;
            btnCancel.Text = "Cancel";
            btnCancel.Click += btnCancel_Click;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlGroup1 });
            Root.Name = "Root";
            Root.Size = new Size(343, 585);
            Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            layoutControlGroup1.AllowDrawBackground = false;
            layoutControlGroup1.GroupBordersVisible = false;
            layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { ItemForCodeCp, ItemForLibelle, ItemForADRESSE, layoutControlItem1, layoutControlItem2, emptySpaceItem1, emptySpaceItem2, emptySpaceItem3 });
            layoutControlGroup1.Location = new Point(0, 0);
            layoutControlGroup1.Name = "autoGeneratedGroup0";
            layoutControlGroup1.Size = new Size(325, 569);
            // 
            // ItemForCodeCp
            // 
            ItemForCodeCp.Control = CodeCpTextEdit;
            ItemForCodeCp.Location = new Point(0, 0);
            ItemForCodeCp.Name = "ItemForCodeCp";
            ItemForCodeCp.Size = new Size(325, 48);
            ItemForCodeCp.Text = "Code Cp";
            ItemForCodeCp.TextLocation = DevExpress.Utils.Locations.Top;
            ItemForCodeCp.TextSize = new Size(59, 17);
            // 
            // ItemForLibelle
            // 
            ItemForLibelle.Control = LibelleTextEdit;
            ItemForLibelle.Location = new Point(0, 48);
            ItemForLibelle.Name = "ItemForLibelle";
            ItemForLibelle.Size = new Size(325, 48);
            ItemForLibelle.Text = "Libelle";
            ItemForLibelle.TextLocation = DevExpress.Utils.Locations.Top;
            ItemForLibelle.TextSize = new Size(59, 17);
            // 
            // ItemForADRESSE
            // 
            ItemForADRESSE.Control = ADRESSETextEdit;
            ItemForADRESSE.Location = new Point(0, 96);
            ItemForADRESSE.Name = "ItemForADRESSE";
            ItemForADRESSE.Size = new Size(325, 48);
            ItemForADRESSE.Text = "ADRESSE";
            ItemForADRESSE.TextLocation = DevExpress.Utils.Locations.Top;
            ItemForADRESSE.TextSize = new Size(59, 17);
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = btnSave;
            layoutControlItem1.Location = new Point(0, 179);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(325, 42);
            layoutControlItem1.TextSize = new Size(0, 0);
            layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = btnCancel;
            layoutControlItem2.Location = new Point(0, 239);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new Size(325, 42);
            layoutControlItem2.TextSize = new Size(0, 0);
            layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            emptySpaceItem1.AllowHotTrack = false;
            emptySpaceItem1.Location = new Point(0, 281);
            emptySpaceItem1.Name = "emptySpaceItem1";
            emptySpaceItem1.Size = new Size(325, 288);
            emptySpaceItem1.TextSize = new Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            emptySpaceItem2.AllowHotTrack = false;
            emptySpaceItem2.Location = new Point(0, 221);
            emptySpaceItem2.Name = "emptySpaceItem2";
            emptySpaceItem2.Size = new Size(325, 18);
            emptySpaceItem2.TextSize = new Size(0, 0);
            // 
            // emptySpaceItem3
            // 
            emptySpaceItem3.AllowHotTrack = false;
            emptySpaceItem3.Location = new Point(0, 144);
            emptySpaceItem3.Name = "emptySpaceItem3";
            emptySpaceItem3.Size = new Size(325, 35);
            emptySpaceItem3.TextSize = new Size(0, 0);
            // 
            // CentersUc
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Controls.Add(dockPanel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "CentersUc";
            Size = new Size(1345, 614);
            Load += CentersUC_Load;
            ((System.ComponentModel.ISupportInitialize)documentManager1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)cENTREBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)noDocumentsView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)tabbedView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dockManager1).EndInit();
            dockPanel1.ResumeLayout(false);
            dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataLayoutControl1).EndInit();
            dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)CodeCpTextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)LibelleTextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)ADRESSETextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForCodeCp).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForLibelle).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForADRESSE).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem3).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView noDocumentsView1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private BindingSource cENTREBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colCodeCp;
        private DevExpress.XtraGrid.Columns.GridColumn colLibelle;
        private DevExpress.XtraGrid.Columns.GridColumn colADRESSE;
        private DevExpress.XtraGrid.Columns.GridColumn colInBor;
        private DevExpress.XtraGrid.Columns.GridColumn colCONV;
        private TextEdit CodeCpTextEdit;
        private TextEdit LibelleTextEdit;
        private TextEdit ADRESSETextEdit;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem ItemForCodeCp;
        private DevExpress.XtraLayout.LayoutControlItem ItemForLibelle;
        private DevExpress.XtraLayout.LayoutControlItem ItemForADRESSE;
        private SimpleButton btnSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private SimpleButton btnCancel;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}

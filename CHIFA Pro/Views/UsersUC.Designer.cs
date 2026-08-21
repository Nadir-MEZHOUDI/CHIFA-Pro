namespace CHIFA.Pro.Views
{
    partial class UsersUc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UsersUc));
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            UserBindingSource = new BindingSource(components);
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colNOM = new DevExpress.XtraGrid.Columns.GridColumn();
            colPASS = new DevExpress.XtraGrid.Columns.GridColumn();
            colLASTCHANGE = new DevExpress.XtraGrid.Columns.GridColumn();
            colNAT = new DevExpress.XtraGrid.Columns.GridColumn();
            dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            dockManager1 = new DevExpress.XtraBars.Docking.DockManager(components);
            dockPanel2 = new DevExpress.XtraBars.Docking.DockPanel();
            dockPanel2_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            NOMTextEdit = new TextEdit();
            PASSTextEdit = new TextEdit();
            LASTCHANGEDateEdit = new ToggleSwitch();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            ItemForNOM = new DevExpress.XtraLayout.LayoutControlItem();
            ItemForPASS = new DevExpress.XtraLayout.LayoutControlItem();
            ItemForLASTCHANGE = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)UserBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dockManager1).BeginInit();
            dockPanel2.SuspendLayout();
            dockPanel2_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataLayoutControl1).BeginInit();
            dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)NOMTextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PASSTextEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LASTCHANGEDateEdit.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForNOM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForPASS).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ItemForLASTCHANGE).BeginInit();
            SuspendLayout();
            // 
            // gridControl1
            // 
            gridControl1.DataSource = UserBindingSource;
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = gridView1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new Size(802, 505);
            gridControl1.TabIndex = 1;
            gridControl1.ViewCollection.AddRange(new BaseView[] { gridView1 });
            // 
            // UserBindingSource
            // 
            UserBindingSource.DataSource = typeof(Utilisateur);
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colNOM, colPASS, colLASTCHANGE, colNAT });
            gridView1.FixedLineWidth = 3;
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 622;
            gridView1.OptionsView.ShowFooter = true;
            // 
            // colNOM
            // 
            colNOM.FieldName = "NomUtilisateur";
            colNOM.ImageOptions.Image = (Image)resources.GetObject("colNOM.ImageOptions.Image");
            colNOM.MinWidth = 17;
            colNOM.Name = "colNOM";
            colNOM.Visible = true;
            colNOM.VisibleIndex = 0;
            colNOM.Width = 66;
            // 
            // colPASS
            // 
            colPASS.FieldName = "MotPasse";
            colPASS.ImageOptions.Image = (Image)resources.GetObject("colPASS.ImageOptions.Image");
            colPASS.MinWidth = 17;
            colPASS.Name = "colPASS";
            colPASS.Visible = true;
            colPASS.VisibleIndex = 1;
            colPASS.Width = 66;
            // 
            // colLASTCHANGE
            // 
            colLASTCHANGE.FieldName = "DroitAcces";
            colLASTCHANGE.MinWidth = 17;
            colLASTCHANGE.Name = "colLASTCHANGE";
            colLASTCHANGE.Visible = true;
            colLASTCHANGE.VisibleIndex = 2;
            colLASTCHANGE.Width = 66;
            // 
            // colNAT
            // 
            colNAT.FieldName = "Admin";
            colNAT.MinWidth = 17;
            colNAT.Name = "colNAT";
            colNAT.Visible = true;
            colNAT.VisibleIndex = 3;
            colNAT.Width = 66;
            // 
            // dockPanel1
            // 
            dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            dockPanel1.ID = new Guid("a7f51335-81f0-40d5-9a42-ab8f47a8886d");
            dockPanel1.Location = new Point(0, 0);
            dockPanel1.Margin = new Padding(4);
            dockPanel1.Name = "dockPanel1";
            dockPanel1.Options.FloatOnDblClick = false;
            dockPanel1.Options.ShowCloseButton = false;
            dockPanel1.OriginalSize = new Size(510, 200);
            dockPanel1.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            dockPanel1.SavedIndex = 0;
            dockPanel1.Size = new Size(510, 942);
            dockPanel1.Text = "Details";
            // 
            // dockPanel1_Container
            // 
            dockPanel1_Container.Location = new Point(7, 31);
            dockPanel1_Container.Margin = new Padding(4);
            dockPanel1_Container.Name = "dockPanel1_Container";
            dockPanel1_Container.Size = new Size(498, 906);
            dockPanel1_Container.TabIndex = 0;
            // 
            // dockManager1
            // 
            dockManager1.Form = this;
            dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] { dockPanel2 });
            dockManager1.TopZIndexControls.AddRange(new string[] { "DevExpress.XtraBars.BarDockControl", "DevExpress.XtraBars.StandaloneBarDockControl", "System.Windows.Forms.StatusBar", "System.Windows.Forms.MenuStrip", "System.Windows.Forms.StatusStrip", "DevExpress.XtraBars.Ribbon.RibbonStatusBar", "DevExpress.XtraBars.Ribbon.RibbonControl", "DevExpress.XtraBars.Navigation.OfficeNavigationBar", "DevExpress.XtraBars.Navigation.TileNavPane", "DevExpress.XtraBars.TabFormControl", "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl", "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl" });
            // 
            // dockPanel2
            // 
            dockPanel2.Controls.Add(dockPanel2_Container);
            dockPanel2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Right;
            dockPanel2.ID = new Guid("f4c344aa-f93d-4f55-83eb-344905e53fad");
            dockPanel2.Location = new Point(802, 0);
            dockPanel2.Margin = new Padding(2, 2, 2, 2);
            dockPanel2.Name = "dockPanel2";
            dockPanel2.OriginalSize = new Size(300, 200);
            dockPanel2.Size = new Size(233, 505);
            dockPanel2.Text = "Détails";
            // 
            // dockPanel2_Container
            // 
            dockPanel2_Container.Controls.Add(dataLayoutControl1);
            dockPanel2_Container.Location = new Point(6, 32);
            dockPanel2_Container.Margin = new Padding(2, 2, 2, 2);
            dockPanel2_Container.Name = "dockPanel2_Container";
            dockPanel2_Container.Size = new Size(223, 469);
            dockPanel2_Container.TabIndex = 0;
            // 
            // dataLayoutControl1
            // 
            dataLayoutControl1.Controls.Add(NOMTextEdit);
            dataLayoutControl1.Controls.Add(PASSTextEdit);
            dataLayoutControl1.Controls.Add(LASTCHANGEDateEdit);
            dataLayoutControl1.DataSource = UserBindingSource;
            dataLayoutControl1.Dock = DockStyle.Fill;
            dataLayoutControl1.Location = new Point(0, 0);
            dataLayoutControl1.Margin = new Padding(2, 2, 2, 2);
            dataLayoutControl1.Name = "dataLayoutControl1";
            dataLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new Rectangle(1108, 227, 812, 500);
            dataLayoutControl1.Root = Root;
            dataLayoutControl1.Size = new Size(223, 469);
            dataLayoutControl1.TabIndex = 0;
            dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // NOMTextEdit
            // 
            NOMTextEdit.DataBindings.Add(new Binding("EditValue", UserBindingSource, "NomUtilisateur", true));
            NOMTextEdit.Location = new Point(10, 28);
            NOMTextEdit.Margin = new Padding(2, 2, 2, 2);
            NOMTextEdit.Name = "NOMTextEdit";
            NOMTextEdit.Size = new Size(203, 22);
            NOMTextEdit.StyleController = dataLayoutControl1;
            NOMTextEdit.TabIndex = 4;
            // 
            // PASSTextEdit
            // 
            PASSTextEdit.DataBindings.Add(new Binding("EditValue", UserBindingSource, "MotPasse", true));
            PASSTextEdit.Location = new Point(10, 72);
            PASSTextEdit.Margin = new Padding(2, 2, 2, 2);
            PASSTextEdit.Name = "PASSTextEdit";
            PASSTextEdit.Size = new Size(203, 22);
            PASSTextEdit.StyleController = dataLayoutControl1;
            PASSTextEdit.TabIndex = 5;
            // 
            // LASTCHANGEDateEdit
            // 
            LASTCHANGEDateEdit.DataBindings.Add(new Binding("ReadOnly", UserBindingSource, "Admin", true));
            LASTCHANGEDateEdit.EditValue = null;
            LASTCHANGEDateEdit.Location = new Point(10, 116);
            LASTCHANGEDateEdit.Margin = new Padding(2, 2, 2, 2);
            LASTCHANGEDateEdit.Name = "LASTCHANGEDateEdit";
            LASTCHANGEDateEdit.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            LASTCHANGEDateEdit.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            LASTCHANGEDateEdit.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            LASTCHANGEDateEdit.Properties.OffText = "Utilisateur";
            LASTCHANGEDateEdit.Properties.OnText = "Administrateur";
            LASTCHANGEDateEdit.Size = new Size(203, 24);
            LASTCHANGEDateEdit.StyleController = dataLayoutControl1;
            LASTCHANGEDateEdit.TabIndex = 6;
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlGroup1 });
            Root.Name = "Root";
            Root.Size = new Size(223, 469);
            Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            layoutControlGroup1.AllowDrawBackground = false;
            layoutControlGroup1.GroupBordersVisible = false;
            layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { ItemForNOM, ItemForPASS, ItemForLASTCHANGE });
            layoutControlGroup1.Location = new Point(0, 0);
            layoutControlGroup1.Name = "autoGeneratedGroup0";
            layoutControlGroup1.Size = new Size(207, 453);
            // 
            // ItemForNOM
            // 
            ItemForNOM.Control = NOMTextEdit;
            ItemForNOM.Location = new Point(0, 0);
            ItemForNOM.Name = "ItemForNOM";
            ItemForNOM.Size = new Size(207, 44);
            ItemForNOM.Text = "Nom d'utilisateur";
            ItemForNOM.TextLocation = DevExpress.Utils.Locations.Top;
            ItemForNOM.TextSize = new Size(36, 16);
            // 
            // ItemForPASS
            // 
            ItemForPASS.Control = PASSTextEdit;
            ItemForPASS.Location = new Point(0, 44);
            ItemForPASS.Name = "ItemForPASS";
            ItemForPASS.Size = new Size(207, 44);
            ItemForPASS.Text = "Mot de passe : ";
            ItemForPASS.TextLocation = DevExpress.Utils.Locations.Top;
            ItemForPASS.TextSize = new Size(36, 16);
            // 
            // ItemForLASTCHANGE
            // 
            ItemForLASTCHANGE.Control = LASTCHANGEDateEdit;
            ItemForLASTCHANGE.Location = new Point(0, 88);
            ItemForLASTCHANGE.Name = "ItemForLASTCHANGE";
            ItemForLASTCHANGE.Size = new Size(207, 365);
            ItemForLASTCHANGE.Text = "Rôle : ";
            ItemForLASTCHANGE.TextLocation = DevExpress.Utils.Locations.Top;
            ItemForLASTCHANGE.TextSize = new Size(36, 16);
            // 
            // UsersUc
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl1);
            Controls.Add(dockPanel2);
            Name = "UsersUc";
            Size = new Size(1035, 505);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)UserBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dockManager1).EndInit();
            dockPanel2.ResumeLayout(false);
            dockPanel2_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataLayoutControl1).EndInit();
            dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)NOMTextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)PASSTextEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)LASTCHANGEDateEdit.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForNOM).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForPASS).EndInit();
            ((System.ComponentModel.ISupportInitialize)ItemForLASTCHANGE).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel2;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel2_Container;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private TextEdit NOMTextEdit;
        private BindingSource UserBindingSource;
        private TextEdit PASSTextEdit;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem ItemForNOM;
        private DevExpress.XtraLayout.LayoutControlItem ItemForPASS;
        private DevExpress.XtraLayout.LayoutControlItem ItemForLASTCHANGE;
        private DevExpress.XtraGrid.Columns.GridColumn colNOM;
        private DevExpress.XtraGrid.Columns.GridColumn colPASS;
        private DevExpress.XtraGrid.Columns.GridColumn colLASTCHANGE;
        private DevExpress.XtraGrid.Columns.GridColumn colNAT;
        private ToggleSwitch LASTCHANGEDateEdit;
    }
}

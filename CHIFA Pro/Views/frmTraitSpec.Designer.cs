namespace CHIFA.Pro.Views
{
    partial class FrmTraitSpec
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            var serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            var editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTraitSpec));
            var serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            colMalad = new DevExpress.XtraGrid.Columns.GridColumn();
            colMontant = new DevExpress.XtraGrid.Columns.GridColumn();
            xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            patientWithTraitSpecBindingSource = new BindingSource(components);
            gridViewTraitSpec1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colMalade = new DevExpress.XtraGrid.Columns.GridColumn();
            colAssure = new DevExpress.XtraGrid.Columns.GridColumn();
            colNumAssure = new DevExpress.XtraGrid.Columns.GridColumn();
            xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            gridControl2 = new DevExpress.XtraGrid.GridControl();
            traitSpec2BindingSource = new BindingSource(components);
            gridViewTraitSpec2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colMedicament1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colQt1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colDuree1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colDateFact1 = new DevExpress.XtraGrid.Columns.GridColumn();
            colProchain2 = new DevExpress.XtraGrid.Columns.GridColumn();
            xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)patientWithTraitSpecBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewTraitSpec1).BeginInit();
            xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridControl2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)traitSpec2BindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridViewTraitSpec2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).BeginInit();
            xtraTabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // colMalad
            // 
            colMalad.FieldName = "Malad";
            colMalad.MinWidth = 27;
            colMalad.Name = "colMalad";
            colMalad.OptionsColumn.ReadOnly = true;
            colMalad.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Malad", "{0}") });
            colMalad.Visible = true;
            colMalad.VisibleIndex = 0;
            colMalad.Width = 104;
            // 
            // colMontant
            // 
            colMontant.DisplayFormat.FormatString = "n";
            colMontant.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            colMontant.FieldName = "Montant";
            colMontant.MinWidth = 27;
            colMontant.Name = "colMontant";
            colMontant.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Montant", "{0:n}") });
            colMontant.Visible = true;
            colMontant.VisibleIndex = 3;
            colMontant.Width = 104;
            // 
            // xtraTabPage1
            // 
            xtraTabPage1.Controls.Add(gridControl1);
            xtraTabPage1.Name = "xtraTabPage1";
            xtraTabPage1.Size = new Size(1366, 627);
            xtraTabPage1.Text = "Page2";
            // 
            // gridControl1
            // 
            gridControl1.Cursor = Cursors.WaitCursor;
            gridControl1.DataSource = patientWithTraitSpecBindingSource;
            gridControl1.Dock = DockStyle.Fill;
            gridControl1.Location = new Point(0, 0);
            gridControl1.MainView = gridViewTraitSpec1;
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new Size(1366, 627);
            gridControl1.TabIndex = 4;
            gridControl1.ViewCollection.AddRange(new BaseView[] { gridViewTraitSpec1 });
            // 
            // patientWithTraitSpecBindingSource
            // 
            patientWithTraitSpecBindingSource.DataSource = typeof(PatientWithTraitSpec);
            // 
            // gridViewTraitSpec1
            // 
            gridViewTraitSpec1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colMalade, colAssure, colNumAssure });
            gridViewTraitSpec1.DetailHeight = 418;
            gridViewTraitSpec1.GridControl = gridControl1;
            gridViewTraitSpec1.Name = "gridViewTraitSpec1";
            gridViewTraitSpec1.OptionsDetail.ShowDetailTabs = false;
            gridViewTraitSpec1.CustomDrawCell += gridView1_CustomDrawCell;
            // 
            // colMalade
            // 
            colMalade.AppearanceCell.Options.UseFont = true;
            colMalade.FieldName = "Malade";
            colMalade.MinWidth = 35;
            colMalade.Name = "colMalade";
            colMalade.Visible = true;
            colMalade.VisibleIndex = 1;
            colMalade.Width = 127;
            // 
            // colAssure
            // 
            colAssure.FieldName = "Assure";
            colAssure.MinWidth = 35;
            colAssure.Name = "colAssure";
            colAssure.Visible = true;
            colAssure.VisibleIndex = 2;
            colAssure.Width = 127;
            // 
            // colNumAssure
            // 
            colNumAssure.FieldName = "NumAssure";
            colNumAssure.MinWidth = 35;
            colNumAssure.Name = "colNumAssure";
            colNumAssure.Visible = true;
            colNumAssure.VisibleIndex = 0;
            colNumAssure.Width = 127;
            // 
            // xtraTabPage2
            // 
            xtraTabPage2.Controls.Add(gridControl2);
            xtraTabPage2.Name = "xtraTabPage2";
            xtraTabPage2.Size = new Size(1366, 627);
            xtraTabPage2.Text = "Page1";
            // 
            // gridControl2
            // 
            gridControl2.DataSource = traitSpec2BindingSource;
            gridControl2.Dock = DockStyle.Fill;
            gridControl2.Location = new Point(0, 0);
            gridControl2.MainView = gridViewTraitSpec2;
            gridControl2.Name = "gridControl2";
            gridControl2.Size = new Size(1366, 627);
            gridControl2.TabIndex = 0;
            gridControl2.ViewCollection.AddRange(new BaseView[] { gridViewTraitSpec2 });
            // 
            // traitSpec2BindingSource
            // 
            traitSpec2BindingSource.DataSource = typeof(TraitSpec2);
            // 
            // gridViewTraitSpec2
            // 
            gridViewTraitSpec2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colMalad, colMedicament1, colQt1, colDuree1, colMontant, colDateFact1, colProchain2 });
            gridViewTraitSpec2.DetailHeight = 382;
            gridViewTraitSpec2.GridControl = gridControl2;
            gridViewTraitSpec2.GroupCount = 1;
            gridViewTraitSpec2.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "Malad", null, "({0:N})"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "Malad", colMalad, "{0:n}"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Montant", null, "(Montant:{0:n})"), new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Montant", colMontant, "{0:n}") });
            gridViewTraitSpec2.Name = "gridViewTraitSpec2";
            gridViewTraitSpec2.OptionsBehavior.AutoExpandAllGroups = true;
            gridViewTraitSpec2.OptionsBehavior.Editable = false;
            gridViewTraitSpec2.OptionsDetail.ShowDetailTabs = false;
            gridViewTraitSpec2.OptionsView.ShowFooter = true;
            gridViewTraitSpec2.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] { new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colMalad, DevExpress.Data.ColumnSortOrder.Ascending) });
            gridViewTraitSpec2.CustomDrawCell += gridView2_CustomDrawCell;
            gridViewTraitSpec2.CustomDrawGroupRow += gridView2_CustomDrawGroupRow;
            gridViewTraitSpec2.DoubleClick += gridView2_DoubleClick;
            // 
            // colMedicament1
            // 
            colMedicament1.FieldName = "Medicament";
            colMedicament1.MinWidth = 27;
            colMedicament1.Name = "colMedicament1";
            colMedicament1.Visible = true;
            colMedicament1.VisibleIndex = 0;
            colMedicament1.Width = 104;
            // 
            // colQt1
            // 
            colQt1.FieldName = "Qt";
            colQt1.MinWidth = 27;
            colQt1.Name = "colQt1";
            colQt1.Visible = true;
            colQt1.VisibleIndex = 2;
            colQt1.Width = 104;
            // 
            // colDuree1
            // 
            colDuree1.FieldName = "Duree";
            colDuree1.MinWidth = 27;
            colDuree1.Name = "colDuree1";
            colDuree1.Visible = true;
            colDuree1.VisibleIndex = 1;
            colDuree1.Width = 104;
            // 
            // colDateFact1
            // 
            colDateFact1.DisplayFormat.FormatString = "dd/MM/yyyy";
            colDateFact1.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colDateFact1.FieldName = "DateFact";
            colDateFact1.MinWidth = 27;
            colDateFact1.Name = "colDateFact1";
            colDateFact1.Visible = true;
            colDateFact1.VisibleIndex = 4;
            colDateFact1.Width = 104;
            // 
            // colProchain2
            // 
            colProchain2.DisplayFormat.FormatString = "dd/MM/yyyy";
            colProchain2.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            colProchain2.FieldName = "Prochain";
            colProchain2.MinWidth = 27;
            colProchain2.Name = "colProchain2";
            colProchain2.OptionsColumn.ReadOnly = true;
            colProchain2.Visible = true;
            colProchain2.VisibleIndex = 5;
            colProchain2.Width = 104;
            // 
            // xtraTabControl1
            // 
            serializableAppearanceObject1.Options.UseImage = true;
            editorButtonImageOptions2.Image = (Image)resources.GetObject("editorButtonImageOptions2.Image");
            xtraTabControl1.CustomHeaderButtons.AddRange(new DevExpress.XtraTab.Buttons.CustomHeaderButton[] { new DevExpress.XtraTab.Buttons.CustomHeaderButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "S", -1, true, true, editorButtonImageOptions1, serializableAppearanceObject1, "", null, null), new DevExpress.XtraTab.Buttons.CustomHeaderButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "History", -1, true, true, editorButtonImageOptions2, serializableAppearanceObject2, "", null, null) });
            xtraTabControl1.Dock = DockStyle.Fill;
            xtraTabControl1.Location = new Point(0, 0);
            xtraTabControl1.Name = "xtraTabControl1";
            xtraTabControl1.SelectedTabPage = xtraTabPage2;
            xtraTabControl1.Size = new Size(1372, 659);
            xtraTabControl1.TabIndex = 5;
            xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] { xtraTabPage2, xtraTabPage1 });
            // 
            // frmTraitSpec
            // 
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1372, 659);
            Controls.Add(xtraTabControl1);
            Font = new Font("Tahoma", 12F);
            IconOptions.LargeImage = (Image)resources.GetObject("frmTraitSpec.IconOptions.LargeImage");
            Name = "FrmTraitSpec";
            Text = "Trait Spec";
            WindowState = FormWindowState.Maximized;
            Load += frmTraitSpec_Load;
            xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)patientWithTraitSpecBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewTraitSpec1).EndInit();
            xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridControl2).EndInit();
            ((System.ComponentModel.ISupportInitialize)traitSpec2BindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridViewTraitSpec2).EndInit();
            ((System.ComponentModel.ISupportInitialize)xtraTabControl1).EndInit();
            xtraTabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTraitSpec1;
        private DevExpress.XtraGrid.Columns.GridColumn colMalade;
        private DevExpress.XtraGrid.Columns.GridColumn colAssure;
        private DevExpress.XtraGrid.Columns.GridColumn colNumAssure;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewTraitSpec2;
        private DevExpress.XtraGrid.Columns.GridColumn colMalad;
        private DevExpress.XtraGrid.Columns.GridColumn colMedicament1;
        private DevExpress.XtraGrid.Columns.GridColumn colQt1;
        private DevExpress.XtraGrid.Columns.GridColumn colDuree1;
        private DevExpress.XtraGrid.Columns.GridColumn colMontant;
        private DevExpress.XtraGrid.Columns.GridColumn colDateFact1;
        private DevExpress.XtraGrid.Columns.GridColumn colProchain2;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private BindingSource patientWithTraitSpecBindingSource;
        private BindingSource traitSpec2BindingSource;
    }
}
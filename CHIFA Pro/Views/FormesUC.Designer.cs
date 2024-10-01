namespace CHIFA.Pro.Views
{
    partial class FormesUc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormesUc));
            gridControl = new DevExpress.XtraGrid.GridControl();
            fORMEBindingSource = new BindingSource(components);
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colFormeID = new DevExpress.XtraGrid.Columns.GridColumn();
            colLibelle = new DevExpress.XtraGrid.Columns.GridColumn();
            colLibelleCourt = new DevExpress.XtraGrid.Columns.GridColumn();
            colMedicaments = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)gridControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)fORMEBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            SuspendLayout();
            // 
            // gridControl
            // 
            gridControl.DataSource = fORMEBindingSource;
            gridControl.Dock = DockStyle.Fill;
            gridControl.Location = new Point(0, 0);
            gridControl.MainView = gridView1;
            gridControl.Name = "gridControl";
            gridControl.Size = new Size(1369, 662);
            gridControl.TabIndex = 0;
            gridControl.ViewCollection.AddRange(new BaseView[] { gridView1 });
            // 
            // fORMEBindingSource
            // 
            fORMEBindingSource.DataSource = typeof(Forme);
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colFormeID, colLibelle, colLibelleCourt, colMedicaments });
            gridView1.CustomizationFormBounds = new Rectangle(997, 429, 230, 255);
            gridView1.FixedLineWidth = 3;
            gridView1.GridControl = gridControl;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 509;
            gridView1.OptionsView.ShowFooter = true;
            // 
            // colFormeID
            // 
            colFormeID.FieldName = "CodeForme";
            colFormeID.ImageOptions.Image = (Image)resources.GetObject("colFormeID.ImageOptions.Image");
            colFormeID.MinWidth = 17;
            colFormeID.Name = "colFormeID";
            colFormeID.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] { new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "FormeID", "{0}") });
            colFormeID.Visible = true;
            colFormeID.VisibleIndex = 0;
            colFormeID.Width = 69;
            // 
            // colLibelle
            // 
            colLibelle.FieldName = "Libelle";
            colLibelle.ImageOptions.Image = (Image)resources.GetObject("colLibelle.ImageOptions.Image");
            colLibelle.MinWidth = 17;
            colLibelle.Name = "colLibelle";
            colLibelle.Visible = true;
            colLibelle.VisibleIndex = 1;
            colLibelle.Width = 66;
            // 
            // colLibelleCourt
            // 
            colLibelleCourt.FieldName = "LibelleCourt";
            colLibelleCourt.ImageOptions.Image = (Image)resources.GetObject("colLibelleCourt.ImageOptions.Image");
            colLibelleCourt.MinWidth = 17;
            colLibelleCourt.Name = "colLibelleCourt";
            colLibelleCourt.Visible = true;
            colLibelleCourt.VisibleIndex = 2;
            colLibelleCourt.Width = 85;
            // 
            // colMedicaments
            // 
            colMedicaments.FieldName = "Medicaments";
            colMedicaments.MinWidth = 17;
            colMedicaments.Name = "colMedicaments";
            colMedicaments.Width = 66;
            // 
            // FormesUc
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl);
            Name = "FormesUc";
            Size = new Size(1369, 662);
            ((System.ComponentModel.ISupportInitialize)gridControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)fORMEBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private BindingSource fORMEBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colFormeID;
        private DevExpress.XtraGrid.Columns.GridColumn colLibelle;
        private DevExpress.XtraGrid.Columns.GridColumn colLibelleCourt;
        private DevExpress.XtraGrid.Columns.GridColumn colMedicaments;
    }
}

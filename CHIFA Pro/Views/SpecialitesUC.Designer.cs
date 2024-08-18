using DataModel;

namespace CHIFA.Pro.Others
{
    partial class SpecialitesUc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpecialitesUc));
            gridControl = new DevExpress.XtraGrid.GridControl();
            sPECIALITEBindingSource = new BindingSource(components);
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            colLibelle = new DevExpress.XtraGrid.Columns.GridColumn();
            colSpID = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)gridControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)sPECIALITEBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            SuspendLayout();
            // 
            // gridControl
            // 
            gridControl.DataSource = sPECIALITEBindingSource;
            gridControl.Dock = DockStyle.Fill;
            gridControl.Location = new Point(0, 0);
            gridControl.MainView = gridView1;
            gridControl.Name = "gridControl";
            gridControl.Size = new Size(1664, 777);
            gridControl.TabIndex = 0;
            gridControl.ViewCollection.AddRange(new BaseView[] { gridView1 });
            // 
            // sPECIALITEBindingSource
            // 
            sPECIALITEBindingSource.DataSource = typeof(Specialite);
            // 
            // gridView1
            // 
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { colLibelle, colSpID });
            gridView1.CustomizationFormBounds = new Rectangle(1149, 459, 230, 255);
            gridView1.FixedLineWidth = 3;
            gridView1.GridControl = gridControl;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 509;
            gridView1.OptionsView.ShowFooter = true;
            // 
            // colLibelle
            // 
            colLibelle.FieldName = "Libelle";
            colLibelle.ImageOptions.Image = (Image)resources.GetObject("colLibelle.ImageOptions.Image");
            colLibelle.MinWidth = 17;
            colLibelle.Name = "colLibelle";
            colLibelle.Visible = true;
            colLibelle.VisibleIndex = 1;
            colLibelle.Width = 839;
            // 
            // colSpID
            // 
            colSpID.FieldName = "CodeSp";
            colSpID.ImageOptions.Image = (Image)resources.GetObject("colSpID.ImageOptions.Image");
            colSpID.MinWidth = 17;
            colSpID.Name = "colSpID";
            colSpID.Visible = true;
            colSpID.VisibleIndex = 0;
            colSpID.Width = 139;
            // 
            // SpecialitesUc
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl);
            Name = "SpecialitesUc";
            Size = new Size(1664, 777);
            ((System.ComponentModel.ISupportInitialize)gridControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)sPECIALITEBindingSource).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colLibelle;
        private BindingSource sPECIALITEBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colSpID;
    }
}

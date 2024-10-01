namespace CHIFA.Pro.Views
{
    partial class ControlMedicalUc
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
            gridControl = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)gridControl).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            SuspendLayout();
            // 
            // gridControl
            // 
            gridControl.Dock = DockStyle.Fill;
            gridControl.Location = new Point(0, 0);
            gridControl.MainView = gridView1;
            gridControl.Name = "gridControl";
            gridControl.Size = new Size(1319, 593);
            gridControl.TabIndex = 0;
            gridControl.ViewCollection.AddRange(new BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.DetailHeight = 372;
            gridView1.FixedLineWidth = 3;
            gridView1.GridControl = gridControl;
            gridView1.HorzScrollVisibility = ScrollVisibility.Always;
            gridView1.Name = "gridView1";
            gridView1.OptionsEditForm.PopupEditFormWidth = 640;
            gridView1.OptionsView.ColumnAutoWidth = false;
            // 
            // ControlMedicalUc
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(gridControl);
            Name = "ControlMedicalUc";
            Size = new Size(1319, 593);
            ((System.ComponentModel.ISupportInitialize)gridControl).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
    }
}

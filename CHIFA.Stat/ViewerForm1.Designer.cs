namespace CHIFA.Stat
{
    partial class ViewerForm1
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
            dashboardViewer = new DevExpress.DashboardWin.DashboardViewer(components);
            ((System.ComponentModel.ISupportInitialize)dashboardViewer).BeginInit();
            SuspendLayout();
            // 
            // dashboardViewer
            // 
            dashboardViewer.AsyncMode = true;
            dashboardViewer.DashboardSource = new Uri("D:\\Projects\\2023\\CHIFA Pro\\CHIFA.Stat\\dashs\\Global.xml", UriKind.Absolute);
            dashboardViewer.Dock = DockStyle.Fill;
            dashboardViewer.Location = new Point(0, 0);
            dashboardViewer.Name = "dashboardViewer";
            dashboardViewer.Size = new Size(986, 628);
            dashboardViewer.TabIndex = 0;
            dashboardViewer.UseNeutralFilterMode = true;
            // 
            // ViewerForm1
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(986, 628);
            Controls.Add(dashboardViewer);
            Name = "ViewerForm1";
            Text = "Dashboard Viewer";
            Load += ViewerForm1_Load;
            ((System.ComponentModel.ISupportInitialize)dashboardViewer).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DevExpress.DashboardWin.DashboardViewer dashboardViewer;
    }
}


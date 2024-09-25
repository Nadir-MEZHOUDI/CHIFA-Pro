namespace CHIFA.Pro.Others
{
    partial class ParametersUc
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ParametersUc));
            groupControl1 = new GroupControl();
            layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            txtChifaPath = new TextEdit();
            btnSave = new SimpleButton();
            txtServerName = new ComboBoxEdit();
            btnBackup = new SimpleButton();
            btnRestor = new SimpleButton();
            simpleButton1 = new SimpleButton();
            btnTest = new SimpleButton();
            rbtServer = new CheckEdit();
            rbtClient = new CheckEdit();
            layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            btnBrowse = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem3 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            Root = new DevExpress.XtraLayout.LayoutControlGroup();
            ((System.ComponentModel.ISupportInitialize)groupControl1).BeginInit();
            groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layoutControl2).BeginInit();
            layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)txtChifaPath.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtServerName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rbtServer.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)rbtClient.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem9).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnBrowse).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Root).BeginInit();
            SuspendLayout();
            // 
            // groupControl1
            // 
            groupControl1.AutoSize = true;
            groupControl1.CaptionImageOptions.Image = (Image)resources.GetObject("groupControl1.CaptionImageOptions.Image");
            groupControl1.Controls.Add(layoutControl2);
            groupControl1.Controls.Add(layoutControl1);
            groupControl1.Dock = DockStyle.Fill;
            groupControl1.Location = new Point(0, 0);
            groupControl1.Name = "groupControl1";
            groupControl1.Size = new Size(691, 326);
            groupControl1.TabIndex = 1;
            groupControl1.Text = "CHIFA Server";
            // 
            // layoutControl2
            // 
            layoutControl2.Controls.Add(txtChifaPath);
            layoutControl2.Controls.Add(btnSave);
            layoutControl2.Controls.Add(txtServerName);
            layoutControl2.Controls.Add(btnBackup);
            layoutControl2.Controls.Add(btnRestor);
            layoutControl2.Controls.Add(simpleButton1);
            layoutControl2.Controls.Add(btnTest);
            layoutControl2.Controls.Add(rbtServer);
            layoutControl2.Controls.Add(rbtClient);
            layoutControl2.Dock = DockStyle.Fill;
            layoutControl2.Location = new Point(2, 39);
            layoutControl2.Margin = new Padding(2);
            layoutControl2.Name = "layoutControl2";
            layoutControl2.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new Rectangle(944, 0, 812, 796);
            layoutControl2.Root = layoutControlGroup1;
            layoutControl2.Size = new Size(687, 285);
            layoutControl2.TabIndex = 1;
            layoutControl2.Text = "layoutControl2";
            // 
            // txtChifaPath
            // 
            txtChifaPath.Location = new Point(230, 46);
            txtChifaPath.Margin = new Padding(2);
            txtChifaPath.Name = "txtChifaPath";
            txtChifaPath.Size = new Size(372, 24);
            txtChifaPath.StyleController = layoutControl2;
            txtChifaPath.TabIndex = 0;
            txtChifaPath.EditValueChanged += txtChifaPath_EditValueChanged;
            // 
            // btnSave
            // 
            btnSave.ImageOptions.Image = (Image)resources.GetObject("btnSave.ImageOptions.Image");
            btnSave.Location = new Point(466, 102);
            btnSave.Margin = new Padding(2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(180, 38);
            btnSave.StyleController = layoutControl2;
            btnSave.TabIndex = 5;
            btnSave.Text = "Save Settings";
            btnSave.Click += btnSave_Click;
            // 
            // txtServerName
            // 
            txtServerName.EditValue = "localhost";
            txtServerName.Location = new Point(230, 74);
            txtServerName.Margin = new Padding(2);
            txtServerName.Name = "txtServerName";
            txtServerName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            txtServerName.Properties.Items.AddRange(new object[] { "localhost" });
            txtServerName.Size = new Size(416, 24);
            txtServerName.StyleController = layoutControl2;
            txtServerName.TabIndex = 3;
            // 
            // btnBackup
            // 
            btnBackup.ImageOptions.Image = (Image)resources.GetObject("btnBackup.ImageOptions.Image");
            btnBackup.Location = new Point(24, 200);
            btnBackup.Margin = new Padding(2);
            btnBackup.Name = "btnBackup";
            btnBackup.ShowFocusRectangle = DevExpress.Utils.DefaultBoolean.False;
            btnBackup.Size = new Size(622, 38);
            btnBackup.StyleController = layoutControl2;
            btnBackup.TabIndex = 6;
            btnBackup.Text = "Backup";
            btnBackup.Click += btnBackup_Click;
            // 
            // btnRestor
            // 
            btnRestor.ImageOptions.Image = (Image)resources.GetObject("btnRestor.ImageOptions.Image");
            btnRestor.Location = new Point(24, 242);
            btnRestor.Margin = new Padding(2);
            btnRestor.Name = "btnRestor";
            btnRestor.Size = new Size(622, 38);
            btnRestor.StyleController = layoutControl2;
            btnRestor.TabIndex = 7;
            btnRestor.Text = "Restore";
            btnRestor.Click += btnRestore_Click;
            // 
            // simpleButton1
            // 
            simpleButton1.ImageOptions.Image = (Image)resources.GetObject("simpleButton1.ImageOptions.Image");
            simpleButton1.Location = new Point(606, 46);
            simpleButton1.Margin = new Padding(2);
            simpleButton1.Name = "simpleButton1";
            simpleButton1.Size = new Size(40, 24);
            simpleButton1.StyleController = layoutControl2;
            simpleButton1.TabIndex = 2;
            simpleButton1.Text = "...";
            simpleButton1.Click += btnBrowse_Click;
            // 
            // btnTest
            // 
            btnTest.ImageOptions.Image = (Image)resources.GetObject("btnTest.ImageOptions.Image");
            btnTest.Location = new Point(230, 102);
            btnTest.Margin = new Padding(2);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(134, 38);
            btnTest.StyleController = layoutControl2;
            btnTest.TabIndex = 4;
            btnTest.Text = "Test";
            btnTest.Click += btnTest_Click;
            // 
            // rbtServer
            // 
            rbtServer.EditValue = true;
            rbtServer.Location = new Point(24, 46);
            rbtServer.Margin = new Padding(2);
            rbtServer.Name = "rbtServer";
            rbtServer.Properties.Caption = "Server:";
            rbtServer.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.Radio;
            rbtServer.Properties.RadioGroupIndex = 0;
            rbtServer.Size = new Size(114, 21);
            rbtServer.StyleController = layoutControl2;
            rbtServer.TabIndex = 1;
            rbtServer.CheckedChanged += rbtServer_CheckedChanged;
            // 
            // rbtClient
            // 
            rbtClient.Location = new Point(24, 74);
            rbtClient.Margin = new Padding(2);
            rbtClient.Name = "rbtClient";
            rbtClient.Properties.Caption = "Client:";
            rbtClient.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.Radio;
            rbtClient.Properties.RadioGroupIndex = 0;
            rbtClient.Size = new Size(114, 21);
            rbtClient.StyleController = layoutControl2;
            rbtClient.TabIndex = 1;
            rbtClient.TabStop = false;
            rbtClient.CheckedChanged += rbtClient_CheckedChanged;
            // 
            // layoutControlGroup1
            // 
            layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            layoutControlGroup1.GroupBordersVisible = false;
            layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlGroup2, layoutControlGroup3 });
            layoutControlGroup1.Name = "Root";
            layoutControlGroup1.Size = new Size(670, 304);
            layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem6, layoutControlItem7 });
            layoutControlGroup2.Location = new Point(0, 154);
            layoutControlGroup2.Name = "layoutControlGroup2";
            layoutControlGroup2.Size = new Size(650, 130);
            layoutControlGroup2.Text = "Backup && Restore";
            // 
            // layoutControlItem6
            // 
            layoutControlItem6.Control = btnBackup;
            layoutControlItem6.Location = new Point(0, 0);
            layoutControlItem6.Name = "layoutControlItem6";
            layoutControlItem6.Size = new Size(626, 42);
            layoutControlItem6.TextSize = new Size(0, 0);
            layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem7
            // 
            layoutControlItem7.Control = btnRestor;
            layoutControlItem7.Location = new Point(0, 42);
            layoutControlItem7.Name = "layoutControlItem7";
            layoutControlItem7.Size = new Size(626, 42);
            layoutControlItem7.TextSize = new Size(0, 0);
            layoutControlItem7.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
            layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { layoutControlItem4, layoutControlItem9, layoutControlItem5, layoutControlItem1, btnBrowse, emptySpaceItem3, layoutControlItem2, emptySpaceItem1, layoutControlItem3, emptySpaceItem2 });
            layoutControlGroup3.Location = new Point(0, 0);
            layoutControlGroup3.Name = "layoutControlGroup3";
            layoutControlGroup3.Size = new Size(650, 154);
            layoutControlGroup3.Text = "CHIFA INFO";
            // 
            // layoutControlItem4
            // 
            layoutControlItem4.Control = rbtClient;
            layoutControlItem4.Location = new Point(0, 28);
            layoutControlItem4.Name = "layoutControlItem4";
            layoutControlItem4.Size = new Size(118, 28);
            layoutControlItem4.TextSize = new Size(0, 0);
            layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            layoutControlItem9.Control = rbtServer;
            layoutControlItem9.Location = new Point(0, 0);
            layoutControlItem9.Name = "layoutControlItem9";
            layoutControlItem9.Size = new Size(118, 28);
            layoutControlItem9.TextSize = new Size(0, 0);
            layoutControlItem9.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            layoutControlItem5.Control = txtServerName;
            layoutControlItem5.Location = new Point(118, 28);
            layoutControlItem5.Name = "layoutControlItem5";
            layoutControlItem5.Size = new Size(508, 28);
            layoutControlItem5.Text = "Server Name:";
            layoutControlItem5.TextSize = new Size(84, 17);
            // 
            // layoutControlItem1
            // 
            layoutControlItem1.Control = txtChifaPath;
            layoutControlItem1.Location = new Point(118, 0);
            layoutControlItem1.Name = "layoutControlItem1";
            layoutControlItem1.Size = new Size(464, 28);
            layoutControlItem1.Text = "CHIFA Path:";
            layoutControlItem1.TextSize = new Size(84, 17);
            // 
            // btnBrowse
            // 
            btnBrowse.Control = simpleButton1;
            btnBrowse.Location = new Point(582, 0);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(44, 28);
            btnBrowse.TextSize = new Size(0, 0);
            btnBrowse.TextVisible = false;
            // 
            // emptySpaceItem3
            // 
            emptySpaceItem3.AllowHotTrack = false;
            emptySpaceItem3.Location = new Point(0, 98);
            emptySpaceItem3.Name = "emptySpaceItem3";
            emptySpaceItem3.Size = new Size(626, 10);
            emptySpaceItem3.TextSize = new Size(0, 0);
            // 
            // layoutControlItem2
            // 
            layoutControlItem2.Control = btnSave;
            layoutControlItem2.Location = new Point(442, 56);
            layoutControlItem2.Name = "layoutControlItem2";
            layoutControlItem2.Size = new Size(184, 42);
            layoutControlItem2.TextSize = new Size(0, 0);
            layoutControlItem2.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            emptySpaceItem1.AllowHotTrack = false;
            emptySpaceItem1.Location = new Point(344, 56);
            emptySpaceItem1.Name = "emptySpaceItem1";
            emptySpaceItem1.Size = new Size(98, 42);
            emptySpaceItem1.TextSize = new Size(0, 0);
            // 
            // layoutControlItem3
            // 
            layoutControlItem3.Control = btnTest;
            layoutControlItem3.Location = new Point(206, 56);
            layoutControlItem3.Name = "layoutControlItem3";
            layoutControlItem3.Size = new Size(138, 42);
            layoutControlItem3.TextSize = new Size(0, 0);
            layoutControlItem3.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            emptySpaceItem2.AllowHotTrack = false;
            emptySpaceItem2.Location = new Point(0, 56);
            emptySpaceItem2.Name = "emptySpaceItem2";
            emptySpaceItem2.Size = new Size(206, 42);
            emptySpaceItem2.TextSize = new Size(0, 0);
            // 
            // layoutControl1
            // 
            layoutControl1.Location = new Point(66, 17);
            layoutControl1.Margin = new Padding(2);
            layoutControl1.Name = "layoutControl1";
            layoutControl1.Root = Root;
            layoutControl1.Size = new Size(2, 2);
            layoutControl1.TabIndex = 0;
            layoutControl1.Text = "layoutControl1";
            // 
            // Root
            // 
            Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            Root.GroupBordersVisible = false;
            Root.Name = "Root";
            Root.Size = new Size(20, 20);
            Root.TextVisible = false;
            // 
            // ParametersUc
            // 
            AutoScaleDimensions = new SizeF(8F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupControl1);
            Name = "ParametersUc";
            Size = new Size(691, 326);
            Load += ParametersUc_Load;
            ((System.ComponentModel.ISupportInitialize)groupControl1).EndInit();
            groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layoutControl2).EndInit();
            layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)txtChifaPath.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtServerName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)rbtServer.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)rbtClient.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem6).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem7).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlGroup3).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem4).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem9).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem5).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnBrowse).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem1).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlItem3).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptySpaceItem2).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Root).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupControl groupControl1;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private TextEdit txtChifaPath;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private SimpleButton btnSave;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private ComboBoxEdit txtServerName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private SimpleButton btnBackup;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private SimpleButton btnRestor;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private SimpleButton simpleButton1;
        private DevExpress.XtraLayout.LayoutControlItem btnBrowse;
        private SimpleButton btnTest;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private CheckEdit rbtServer;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private CheckEdit rbtClient;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem3;
    }
}

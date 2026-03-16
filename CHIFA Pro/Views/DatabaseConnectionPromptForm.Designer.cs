namespace CHIFA.Pro.Views
{
    partial class DatabaseConnectionPromptForm : XtraForm
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

        private void InitializeComponent()
        {
            var parameterResources = new System.ComponentModel.ComponentResourceManager(typeof(ParametersUc));
            lblMessage = new LabelControl();
            lblServer = new LabelControl();
            lblPort = new LabelControl();
            _txtServer = new TextEdit();
            _spnPort = new SpinEdit();
            _btnTest = new SimpleButton();
            _btnSave = new SimpleButton();
            btnClose = new SimpleButton();
            layoutControlMain = new DevExpress.XtraLayout.LayoutControl();
            rootGroup = new DevExpress.XtraLayout.LayoutControlGroup();
            lciMessage = new DevExpress.XtraLayout.LayoutControlItem();
            lciServerLabel = new DevExpress.XtraLayout.LayoutControlItem();
            lciServer = new DevExpress.XtraLayout.LayoutControlItem();
            lciPortLabel = new DevExpress.XtraLayout.LayoutControlItem();
            lciPort = new DevExpress.XtraLayout.LayoutControlItem();
            lciTest = new DevExpress.XtraLayout.LayoutControlItem();
            lciSave = new DevExpress.XtraLayout.LayoutControlItem();
            lciClose = new DevExpress.XtraLayout.LayoutControlItem();
            emptyBetweenButtons = new DevExpress.XtraLayout.EmptySpaceItem();
            emptyBottom = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)_txtServer.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_spnPort.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlMain).BeginInit();
            layoutControlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rootGroup).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lciMessage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lciServerLabel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lciServer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lciPortLabel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lciPort).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lciTest).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lciSave).BeginInit();
            ((System.ComponentModel.ISupportInitialize)lciClose).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptyBetweenButtons).BeginInit();
            ((System.ComponentModel.ISupportInitialize)emptyBottom).BeginInit();
            SuspendLayout();
            // 
            // lblMessage
            // 
            lblMessage.AutoSizeMode = LabelAutoSizeMode.Vertical;
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(425, 42);
            lblMessage.Text = "Connexion a la base de donnees impossible.\r\nVeuillez saisir le serveur et le port, puis tester la connexion.";
            // 
            // lblServer
            // 
            lblServer.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(64, 21);
            lblServer.Text = "Serveur :";
            // 
            // lblPort
            // 
            lblPort.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(41, 21);
            lblPort.Text = "Port :";
            // 
            // _txtServer
            // 
            _txtServer.Name = "_txtServer";
            _txtServer.Properties.NullValuePrompt = "localhost";
            _txtServer.Properties.NullValuePromptShowForEmptyValue = true;
            _txtServer.Size = new Size(344, 28);
            _txtServer.StyleController = layoutControlMain;
            _txtServer.TabIndex = 2;
            // 
            // _spnPort
            // 
            _spnPort.EditValue = new decimal(new int[] { 5432, 0, 0, 0 });
            _spnPort.Name = "_spnPort";
            _spnPort.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            _spnPort.Properties.IsFloatValue = false;
            _spnPort.Properties.MaskSettings.Set("mask", "d");
            _spnPort.Properties.MaxValue = new decimal(new int[] { 65535, 0, 0, 0 });
            _spnPort.Properties.MinValue = new decimal(new int[] { 1, 0, 0, 0 });
            _spnPort.Size = new Size(344, 28);
            _spnPort.StyleController = layoutControlMain;
            _spnPort.TabIndex = 4;
            // 
            // _btnTest
            // 
            _btnTest.ImageOptions.Image = (Image)parameterResources.GetObject("btnTest.ImageOptions.Image");
            _btnTest.Name = "_btnTest";
            _btnTest.Size = new Size(135, 38);
            _btnTest.StyleController = layoutControlMain;
            _btnTest.TabIndex = 5;
            _btnTest.Text = "Tester";
            _btnTest.Click += BtnTest_Click;
            // 
            // _btnSave
            // 
            _btnSave.ImageOptions.Image = (Image)parameterResources.GetObject("btnSave.ImageOptions.Image");
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(136, 38);
            _btnSave.StyleController = layoutControlMain;
            _btnSave.TabIndex = 6;
            _btnSave.Text = "Enregistrer";
            _btnSave.Click += BtnSave_Click;
            // 
            // btnClose
            // 
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.ImageOptions.Image = (Image)parameterResources.GetObject("btnRestor.ImageOptions.Image");
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(136, 38);
            btnClose.StyleController = layoutControlMain;
            btnClose.TabIndex = 7;
            btnClose.Text = "Annuler";
            // 
            // layoutControlMain
            // 
            layoutControlMain.Controls.Add(lblMessage);
            layoutControlMain.Controls.Add(lblServer);
            layoutControlMain.Controls.Add(lblPort);
            layoutControlMain.Controls.Add(_txtServer);
            layoutControlMain.Controls.Add(_spnPort);
            layoutControlMain.Controls.Add(_btnTest);
            layoutControlMain.Controls.Add(_btnSave);
            layoutControlMain.Controls.Add(btnClose);
            layoutControlMain.Dock = DockStyle.Fill;
            layoutControlMain.Location = new Point(0, 0);
            layoutControlMain.Name = "layoutControlMain";
            layoutControlMain.Root = rootGroup;
            layoutControlMain.Size = new Size(474, 241);
            layoutControlMain.TabIndex = 0;
            layoutControlMain.Text = "layoutControlMain";
            // 
            // rootGroup
            // 
            rootGroup.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            rootGroup.GroupBordersVisible = false;
            rootGroup.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] { lciMessage, lciServerLabel, lciServer, lciPortLabel, lciPort, lciTest, lciSave, lciClose, emptyBetweenButtons, emptyBottom });
            rootGroup.Name = "rootGroup";
            rootGroup.Size = new Size(474, 241);
            rootGroup.TextVisible = false;
            // 
            // lciMessage
            // 
            lciMessage.Control = lblMessage;
            lciMessage.Location = new Point(0, 0);
            lciMessage.Name = "lciMessage";
            lciMessage.Size = new Size(454, 46);
            lciMessage.TextSize = new Size(0, 0);
            lciMessage.TextVisible = false;
            // 
            // lciServerLabel
            // 
            lciServerLabel.Control = lblServer;
            lciServerLabel.Location = new Point(0, 46);
            lciServerLabel.Name = "lciServerLabel";
            lciServerLabel.Size = new Size(98, 32);
            lciServerLabel.TextSize = new Size(0, 0);
            lciServerLabel.TextVisible = false;
            // 
            // lciServer
            // 
            lciServer.Control = _txtServer;
            lciServer.Location = new Point(98, 46);
            lciServer.Name = "lciServer";
            lciServer.Size = new Size(356, 32);
            lciServer.TextSize = new Size(0, 0);
            lciServer.TextVisible = false;
            // 
            // lciPortLabel
            // 
            lciPortLabel.Control = lblPort;
            lciPortLabel.Location = new Point(0, 78);
            lciPortLabel.Name = "lciPortLabel";
            lciPortLabel.Size = new Size(98, 32);
            lciPortLabel.TextSize = new Size(0, 0);
            lciPortLabel.TextVisible = false;
            // 
            // lciPort
            // 
            lciPort.Control = _spnPort;
            lciPort.Location = new Point(98, 78);
            lciPort.Name = "lciPort";
            lciPort.Size = new Size(356, 32);
            lciPort.TextSize = new Size(0, 0);
            lciPort.TextVisible = false;
            // 
            // lciTest
            // 
            lciTest.Control = _btnTest;
            lciTest.Location = new Point(0, 127);
            lciTest.Name = "lciTest";
            lciTest.Size = new Size(139, 42);
            lciTest.TextSize = new Size(0, 0);
            lciTest.TextVisible = false;
            // 
            // lciSave
            // 
            lciSave.Control = _btnSave;
            lciSave.Location = new Point(139, 127);
            lciSave.Name = "lciSave";
            lciSave.Size = new Size(140, 42);
            lciSave.TextSize = new Size(0, 0);
            lciSave.TextVisible = false;
            // 
            // lciClose
            // 
            lciClose.Control = btnClose;
            lciClose.Location = new Point(279, 127);
            lciClose.Name = "lciClose";
            lciClose.Size = new Size(140, 42);
            lciClose.TextSize = new Size(0, 0);
            lciClose.TextVisible = false;
            // 
            // emptyBetweenButtons
            // 
            emptyBetweenButtons.AllowHotTrack = false;
            emptyBetweenButtons.Location = new Point(419, 127);
            emptyBetweenButtons.Name = "emptyBetweenButtons";
            emptyBetweenButtons.Size = new Size(35, 42);
            emptyBetweenButtons.TextSize = new Size(0, 0);
            // 
            // emptyBottom
            // 
            emptyBottom.AllowHotTrack = false;
            emptyBottom.Location = new Point(0, 110);
            emptyBottom.Name = "emptyBottom";
            emptyBottom.Size = new Size(454, 17);
            emptyBottom.TextSize = new Size(0, 0);
            // 
            // DatabaseConnectionPromptForm
            // 
            AcceptButton = _btnSave;
            AutoScaleDimensions = new SizeF(8F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnClose;
            ClientSize = new Size(474, 241);
            Controls.Add(layoutControlMain);
            Font = new Font("Tahoma", 10.2F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            IconOptions.ShowIcon = false;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DatabaseConnectionPromptForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Parametres de connexion a la base de donnees";
            ((System.ComponentModel.ISupportInitialize)_txtServer.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_spnPort.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)layoutControlMain).EndInit();
            layoutControlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)rootGroup).EndInit();
            ((System.ComponentModel.ISupportInitialize)lciMessage).EndInit();
            ((System.ComponentModel.ISupportInitialize)lciServerLabel).EndInit();
            ((System.ComponentModel.ISupportInitialize)lciServer).EndInit();
            ((System.ComponentModel.ISupportInitialize)lciPortLabel).EndInit();
            ((System.ComponentModel.ISupportInitialize)lciPort).EndInit();
            ((System.ComponentModel.ISupportInitialize)lciTest).EndInit();
            ((System.ComponentModel.ISupportInitialize)lciSave).EndInit();
            ((System.ComponentModel.ISupportInitialize)lciClose).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptyBetweenButtons).EndInit();
            ((System.ComponentModel.ISupportInitialize)emptyBottom).EndInit();
            ResumeLayout(false);
        }

        private LabelControl lblMessage;
        private LabelControl lblServer;
        private LabelControl lblPort;
        private TextEdit _txtServer;
        private SpinEdit _spnPort;
        private SimpleButton _btnTest;
        private SimpleButton _btnSave;
        private SimpleButton btnClose;
        private DevExpress.XtraLayout.LayoutControl layoutControlMain;
        private DevExpress.XtraLayout.LayoutControlGroup rootGroup;
        private DevExpress.XtraLayout.LayoutControlItem lciMessage;
        private DevExpress.XtraLayout.LayoutControlItem lciServerLabel;
        private DevExpress.XtraLayout.LayoutControlItem lciServer;
        private DevExpress.XtraLayout.LayoutControlItem lciPortLabel;
        private DevExpress.XtraLayout.LayoutControlItem lciPort;
        private DevExpress.XtraLayout.LayoutControlItem lciTest;
        private DevExpress.XtraLayout.LayoutControlItem lciSave;
        private DevExpress.XtraLayout.LayoutControlItem lciClose;
        private DevExpress.XtraLayout.EmptySpaceItem emptyBetweenButtons;
        private DevExpress.XtraLayout.EmptySpaceItem emptyBottom;
    }
}

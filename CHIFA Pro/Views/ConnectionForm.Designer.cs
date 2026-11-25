namespace CHIFA.Pro.Views
{
    partial class ConnectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionForm));
            lblTitle = new LabelControl();
            lblIntro = new LabelControl();
            _rbLocal = new CheckEdit();
            _dropZone = new PanelControl();
            _lblDrop = new LabelControl();
            _btnBrowse = new SimpleButton();
            _rbRemote = new CheckEdit();
            _lblServer = new LabelControl();
            _cboServer = new ComboBoxEdit();
            _lblStatus = new LabelControl();
            _btnTest = new SimpleButton();
            _btnSave = new SimpleButton();
            _btnCancel = new SimpleButton();
            ((System.ComponentModel.ISupportInitialize)_rbLocal.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_dropZone).BeginInit();
            _dropZone.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_rbRemote.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_cboServer.Properties).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Appearance.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Location = new Point(30, 20);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(395, 31);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Connexion à la base CHIFA_OFFICINE";
            // 
            // lblIntro
            // 
            lblIntro.Appearance.ForeColor = Color.DimGray;
            lblIntro.Appearance.Options.UseForeColor = true;
            lblIntro.Appearance.Options.UseTextOptions = true;
            lblIntro.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            lblIntro.AutoSizeMode = LabelAutoSizeMode.None;
            lblIntro.Location = new Point(30, 62);
            lblIntro.Margin = new Padding(4);
            lblIntro.Name = "lblIntro";
            lblIntro.Size = new Size(690, 28);
            lblIntro.TabIndex = 1;
            lblIntro.Text = "SmartCHIFA lit les données de CHIFA_OFFICINE. Indiquez où se trouve sa base de données.";
            // 
            // _rbLocal
            // 
            _rbLocal.Location = new Point(28, 105);
            _rbLocal.Margin = new Padding(4);
            _rbLocal.Name = "_rbLocal";
            _rbLocal.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _rbLocal.Properties.Appearance.Options.UseFont = true;
            _rbLocal.Properties.Caption = "CHIFA_OFFICINE est installé sur ce poste (serveur)";
            _rbLocal.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            _rbLocal.Properties.RadioGroupIndex = 1;
            _rbLocal.Size = new Size(695, 25);
            _rbLocal.TabIndex = 2;
            _rbLocal.TabStop = false;
            _rbLocal.CheckedChanged += Mode_CheckedChanged;
            // 
            // _dropZone
            // 
            _dropZone.AllowDrop = true;
            _dropZone.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            _dropZone.Controls.Add(_lblDrop);
            _dropZone.Location = new Point(58, 142);
            _dropZone.Margin = new Padding(4);
            _dropZone.Name = "_dropZone";
            _dropZone.Size = new Size(500, 138);
            _dropZone.TabIndex = 3;
            _dropZone.DragDrop += OnDragDrop;
            _dropZone.DragEnter += OnDragEnter;
            // 
            // _lblDrop
            // 
            _lblDrop.AllowDrop = true;
            _lblDrop.Appearance.ForeColor = Color.DimGray;
            _lblDrop.Appearance.Options.UseForeColor = true;
            _lblDrop.Appearance.Options.UseTextOptions = true;
            _lblDrop.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            _lblDrop.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            _lblDrop.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblDrop.AutoSizeMode = LabelAutoSizeMode.None;
            _lblDrop.Dock = DockStyle.Fill;
            _lblDrop.Location = new Point(2, 2);
            _lblDrop.Margin = new Padding(4);
            _lblDrop.Name = "_lblDrop";
            _lblDrop.Size = new Size(496, 134);
            _lblDrop.TabIndex = 0;
            _lblDrop.Text = "Glissez ici l'icône CHIFA_OFFICINE\r\ndepuis le Bureau";
            _lblDrop.DragDrop += OnDragDrop;
            _lblDrop.DragEnter += OnDragEnter;
            // 
            // _btnBrowse
            // 
            _btnBrowse.Appearance.Font = new Font("Tahoma", 10F);
            _btnBrowse.Appearance.Options.UseFont = true;
            _btnBrowse.ImageOptions.Image = (Image)resources.GetObject("_btnBrowse.ImageOptions.Image");
            _btnBrowse.Location = new Point(570, 142);
            _btnBrowse.Margin = new Padding(4);
            _btnBrowse.Name = "_btnBrowse";
            _btnBrowse.Size = new Size(150, 40);
            _btnBrowse.TabIndex = 4;
            _btnBrowse.Text = "Parcourir…";
            _btnBrowse.Click += BtnBrowse_Click;
            // 
            // _rbRemote
            // 
            _rbRemote.Location = new Point(28, 295);
            _rbRemote.Margin = new Padding(4);
            _rbRemote.Name = "_rbRemote";
            _rbRemote.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _rbRemote.Properties.Appearance.Options.UseFont = true;
            _rbRemote.Properties.Caption = "La base de données est sur un autre poste";
            _rbRemote.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            _rbRemote.Properties.RadioGroupIndex = 1;
            _rbRemote.Size = new Size(695, 25);
            _rbRemote.TabIndex = 5;
            _rbRemote.TabStop = false;
            _rbRemote.CheckedChanged += Mode_CheckedChanged;
            // 
            // _lblServer
            // 
            _lblServer.Appearance.Font = new Font("Tahoma", 10F);
            _lblServer.Appearance.Options.UseFont = true;
            _lblServer.Location = new Point(60, 340);
            _lblServer.Margin = new Padding(4);
            _lblServer.Name = "_lblServer";
            _lblServer.Size = new Size(67, 21);
            _lblServer.TabIndex = 6;
            _lblServer.Text = "Serveur :";
            // 
            // _cboServer
            // 
            _cboServer.Location = new Point(162, 335);
            _cboServer.Margin = new Padding(4);
            _cboServer.Name = "_cboServer";
            _cboServer.Properties.Appearance.Font = new Font("Tahoma", 12F);
            _cboServer.Properties.Appearance.Options.UseFont = true;
            _cboServer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            _cboServer.Properties.NullValuePrompt = "Nom ou adresse IP du poste serveur";
            _cboServer.Size = new Size(395, 30);
            _cboServer.TabIndex = 7;
            // 
            // _lblStatus
            // 
            _lblStatus.Appearance.Options.UseTextOptions = true;
            _lblStatus.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
            _lblStatus.Location = new Point(30, 390);
            _lblStatus.Margin = new Padding(4);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(690, 68);
            _lblStatus.TabIndex = 8;
            // 
            // _btnTest
            // 
            _btnTest.Appearance.Font = new Font("Tahoma", 10F);
            _btnTest.Appearance.Options.UseFont = true;
            _btnTest.ImageOptions.Image = (Image)resources.GetObject("_btnTest.ImageOptions.Image");
            _btnTest.Location = new Point(245, 475);
            _btnTest.Margin = new Padding(4);
            _btnTest.Name = "_btnTest";
            _btnTest.Size = new Size(150, 40);
            _btnTest.TabIndex = 9;
            _btnTest.Text = "Tester";
            _btnTest.Click += BtnTest_Click;
            // 
            // _btnSave
            // 
            _btnSave.Appearance.Font = new Font("Tahoma", 10F);
            _btnSave.Appearance.Options.UseFont = true;
            _btnSave.ImageOptions.Image = (Image)resources.GetObject("_btnSave.ImageOptions.Image");
            _btnSave.Location = new Point(408, 475);
            _btnSave.Margin = new Padding(4);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(150, 40);
            _btnSave.TabIndex = 10;
            _btnSave.Text = "Enregistrer";
            _btnSave.Click += BtnSave_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.Appearance.Font = new Font("Tahoma", 10F);
            _btnCancel.Appearance.Options.UseFont = true;
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.ImageOptions.Image = (Image)resources.GetObject("_btnCancel.ImageOptions.Image");
            _btnCancel.Location = new Point(570, 475);
            _btnCancel.Margin = new Padding(4);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(150, 40);
            _btnCancel.TabIndex = 11;
            _btnCancel.Text = "Annuler";
            // 
            // ConnectionForm
            // 
            AcceptButton = _btnSave;
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnCancel;
            ClientSize = new Size(734, 526);
            Controls.Add(lblTitle);
            Controls.Add(lblIntro);
            Controls.Add(_rbLocal);
            Controls.Add(_dropZone);
            Controls.Add(_btnBrowse);
            Controls.Add(_rbRemote);
            Controls.Add(_lblServer);
            Controls.Add(_cboServer);
            Controls.Add(_lblStatus);
            Controls.Add(_btnTest);
            Controls.Add(_btnSave);
            Controls.Add(_btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            IconOptions.Icon = (Icon)resources.GetObject("ConnectionForm.IconOptions.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectionForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Base de données CHIFA_OFFICINE";
            DragDrop += OnDragDrop;
            DragEnter += OnDragEnter;
            ((System.ComponentModel.ISupportInitialize)_rbLocal.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_dropZone).EndInit();
            _dropZone.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_rbRemote.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_cboServer.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private LabelControl lblTitle;
        private LabelControl lblIntro;
        private CheckEdit _rbLocal;
        private PanelControl _dropZone;
        private LabelControl _lblDrop;
        private SimpleButton _btnBrowse;
        private CheckEdit _rbRemote;
        private LabelControl _lblServer;
        private ComboBoxEdit _cboServer;
        private LabelControl _lblStatus;
        private SimpleButton _btnTest;
        private SimpleButton _btnSave;
        private SimpleButton _btnCancel;
    }
}

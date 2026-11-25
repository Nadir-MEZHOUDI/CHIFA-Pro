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
            lblTitle.Location = new Point(24, 16);
            lblTitle.Name = "lblTitle";
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
            lblIntro.Location = new Point(24, 50);
            lblIntro.Name = "lblIntro";
            lblIntro.Size = new Size(552, 22);
            lblIntro.TabIndex = 1;
            lblIntro.Text = "SmartCHIFA lit les données de CHIFA_OFFICINE. Indiquez où se trouve sa base de données.";
            //
            // _rbLocal
            //
            _rbLocal.Location = new Point(22, 84);
            _rbLocal.Name = "_rbLocal";
            _rbLocal.Properties.Caption = "CHIFA_OFFICINE est installé sur ce poste (serveur)";
            _rbLocal.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            _rbLocal.Properties.RadioGroupIndex = 1;
            _rbLocal.Size = new Size(556, 24);
            _rbLocal.TabIndex = 2;
            _rbLocal.CheckedChanged += Mode_CheckedChanged;
            //
            // _dropZone
            //
            _dropZone.AllowDrop = true;
            _dropZone.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            _dropZone.Controls.Add(_lblDrop);
            _dropZone.Location = new Point(46, 114);
            _dropZone.Name = "_dropZone";
            _dropZone.Size = new Size(400, 110);
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
            _lblDrop.Location = new Point(1, 1);
            _lblDrop.Name = "_lblDrop";
            _lblDrop.Size = new Size(398, 108);
            _lblDrop.TabIndex = 0;
            _lblDrop.Text = "Glissez ici l'icône CHIFA_OFFICINE\r\ndepuis le Bureau";
            _lblDrop.DragDrop += OnDragDrop;
            _lblDrop.DragEnter += OnDragEnter;
            //
            // _btnBrowse
            //
            _btnBrowse.Location = new Point(456, 114);
            _btnBrowse.Name = "_btnBrowse";
            _btnBrowse.Size = new Size(120, 32);
            _btnBrowse.TabIndex = 4;
            _btnBrowse.Text = "Parcourir…";
            _btnBrowse.Click += BtnBrowse_Click;
            //
            // _rbRemote
            //
            _rbRemote.Location = new Point(22, 236);
            _rbRemote.Name = "_rbRemote";
            _rbRemote.Properties.Caption = "La base de données est sur un autre poste";
            _rbRemote.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.Radio;
            _rbRemote.Properties.RadioGroupIndex = 1;
            _rbRemote.Size = new Size(556, 24);
            _rbRemote.TabIndex = 5;
            _rbRemote.CheckedChanged += Mode_CheckedChanged;
            //
            // _lblServer
            //
            _lblServer.Location = new Point(48, 272);
            _lblServer.Name = "_lblServer";
            _lblServer.TabIndex = 6;
            _lblServer.Text = "Serveur :";
            //
            // _cboServer
            //
            _cboServer.Location = new Point(130, 268);
            _cboServer.Name = "_cboServer";
            _cboServer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            _cboServer.Properties.NullValuePrompt = "Nom ou adresse IP du poste serveur";
            _cboServer.Size = new Size(316, 26);
            _cboServer.TabIndex = 7;
            //
            // _lblStatus
            //
            _lblStatus.Appearance.Options.UseTextOptions = true;
            _lblStatus.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
            _lblStatus.Location = new Point(24, 312);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(552, 54);
            _lblStatus.TabIndex = 8;
            //
            // _btnTest
            //
            _btnTest.Location = new Point(196, 380);
            _btnTest.Name = "_btnTest";
            _btnTest.Size = new Size(120, 32);
            _btnTest.TabIndex = 9;
            _btnTest.Text = "Tester";
            _btnTest.Click += BtnTest_Click;
            //
            // _btnSave
            //
            _btnSave.Location = new Point(326, 380);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(120, 32);
            _btnSave.TabIndex = 10;
            _btnSave.Text = "Enregistrer";
            _btnSave.Click += BtnSave_Click;
            //
            // _btnCancel
            //
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.Location = new Point(456, 380);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(120, 32);
            _btnCancel.TabIndex = 11;
            _btnCancel.Text = "Annuler";
            //
            // ConnectionForm
            //
            AcceptButton = _btnSave;
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnCancel;
            ClientSize = new Size(600, 430);
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

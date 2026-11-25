namespace CHIFA.Pro.Mobile
{
    partial class MobileLinkForm
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
            components = new System.ComponentModel.Container();
            lblTitle = new LabelControl();
            _lblIntro = new LabelControl();
            _qr = new PictureEdit();
            _lblCode = new LabelControl();
            _lblExpiry = new LabelControl();
            _lblStatus = new LabelControl();
            _btnNewCode = new SimpleButton();
            lblDevices = new LabelControl();
            _lstDevices = new ListBoxControl();
            _btnRemove = new SimpleButton();
            _lblService = new LabelControl();
            _btnService = new SimpleButton();
            _btnClose = new SimpleButton();
            _clock = new System.Windows.Forms.Timer(components);
            _devicesPoll = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)_qr.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_lstDevices).BeginInit();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.Appearance.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Lier un téléphone";
            //
            // _lblIntro
            //
            _lblIntro.Appearance.Font = new Font("Segoe UI", 9.75F);
            _lblIntro.Appearance.ForeColor = Color.DimGray;
            _lblIntro.Appearance.Options.UseFont = true;
            _lblIntro.Appearance.Options.UseForeColor = true;
            _lblIntro.Appearance.Options.UseTextOptions = true;
            _lblIntro.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblIntro.AutoSizeMode = LabelAutoSizeMode.None;
            _lblIntro.Location = new Point(24, 56);
            _lblIntro.Name = "_lblIntro";
            _lblIntro.Size = new Size(592, 42);
            _lblIntro.TabIndex = 1;
            _lblIntro.Text = "1. Sur le téléphone, scannez le QR code ou ouvrez l'adresse du site\r\n2. Saisissez le code ci-dessous. Il ne sert qu'une fois : le téléphone reste ensuite connecté.";
            //
            // _qr
            //
            _qr.Location = new Point(24, 110);
            _qr.Name = "_qr";
            _qr.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            _qr.Properties.ReadOnly = true;
            _qr.Properties.ShowMenu = false;
            _qr.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            _qr.Size = new Size(190, 190);
            _qr.TabIndex = 2;
            //
            // _lblCode
            //
            _lblCode.Appearance.Font = new Font("Consolas", 34F, FontStyle.Bold);
            _lblCode.Appearance.ForeColor = Color.FromArgb(14, 124, 102);
            _lblCode.Appearance.Options.UseFont = true;
            _lblCode.Appearance.Options.UseForeColor = true;
            _lblCode.AutoSizeMode = LabelAutoSizeMode.None;
            _lblCode.Location = new Point(236, 120);
            _lblCode.Name = "_lblCode";
            _lblCode.Size = new Size(380, 64);
            _lblCode.TabIndex = 3;
            //
            // _lblExpiry
            //
            _lblExpiry.Appearance.Font = new Font("Segoe UI", 9.75F);
            _lblExpiry.Appearance.Options.UseFont = true;
            _lblExpiry.AutoSizeMode = LabelAutoSizeMode.None;
            _lblExpiry.Location = new Point(240, 186);
            _lblExpiry.Name = "_lblExpiry";
            _lblExpiry.Size = new Size(376, 22);
            _lblExpiry.TabIndex = 4;
            //
            // _lblStatus
            //
            _lblStatus.Appearance.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
            _lblStatus.Appearance.Options.UseFont = true;
            _lblStatus.Appearance.Options.UseTextOptions = true;
            _lblStatus.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            _lblStatus.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
            _lblStatus.Location = new Point(240, 214);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(376, 44);
            _lblStatus.TabIndex = 5;
            //
            // _btnNewCode
            //
            _btnNewCode.Location = new Point(240, 264);
            _btnNewCode.Name = "_btnNewCode";
            _btnNewCode.Size = new Size(160, 32);
            _btnNewCode.TabIndex = 6;
            _btnNewCode.Text = "Nouveau code";
            _btnNewCode.Click += BtnNewCode_Click;
            //
            // lblDevices
            //
            lblDevices.Appearance.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblDevices.Appearance.Options.UseFont = true;
            lblDevices.Location = new Point(24, 320);
            lblDevices.Name = "lblDevices";
            lblDevices.TabIndex = 7;
            lblDevices.Text = "Téléphones liés";
            //
            // _lstDevices
            //
            _lstDevices.Appearance.Font = new Font("Segoe UI", 9.75F);
            _lstDevices.Appearance.Options.UseFont = true;
            _lstDevices.Location = new Point(24, 346);
            _lstDevices.Name = "_lstDevices";
            _lstDevices.Size = new Size(592, 130);
            _lstDevices.TabIndex = 8;
            _lstDevices.SelectedIndexChanged += LstDevices_SelectedIndexChanged;
            //
            // _btnRemove
            //
            _btnRemove.Enabled = false;
            _btnRemove.Location = new Point(24, 484);
            _btnRemove.Name = "_btnRemove";
            _btnRemove.Size = new Size(190, 30);
            _btnRemove.TabIndex = 9;
            _btnRemove.Text = "Retirer ce téléphone";
            _btnRemove.Click += BtnRemove_Click;
            //
            // _lblService
            //
            _lblService.Appearance.Font = new Font("Segoe UI", 9.5F);
            _lblService.Appearance.Options.UseFont = true;
            _lblService.Appearance.Options.UseTextOptions = true;
            _lblService.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            _lblService.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblService.AutoSizeMode = LabelAutoSizeMode.None;
            _lblService.Location = new Point(24, 524);
            _lblService.Name = "_lblService";
            _lblService.Size = new Size(420, 40);
            _lblService.TabIndex = 10;
            //
            // _btnService
            //
            _btnService.Location = new Point(456, 528);
            _btnService.Name = "_btnService";
            _btnService.Size = new Size(160, 30);
            _btnService.TabIndex = 11;
            _btnService.Text = "Installer le service";
            _btnService.Click += BtnService_Click;
            //
            // _btnClose
            //
            _btnClose.Location = new Point(486, 562);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(130, 30);
            _btnClose.TabIndex = 12;
            _btnClose.Text = "Fermer";
            _btnClose.Click += BtnClose_Click;
            //
            // _clock
            //
            _clock.Interval = 1000;
            _clock.Tick += Clock_Tick;
            //
            // _devicesPoll
            //
            _devicesPoll.Interval = 5000;
            _devicesPoll.Tick += DevicesPoll_Tick;
            //
            // MobileLinkForm
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnClose;
            ClientSize = new Size(640, 600);
            Controls.Add(lblTitle);
            Controls.Add(_lblIntro);
            Controls.Add(_qr);
            Controls.Add(_lblCode);
            Controls.Add(_lblExpiry);
            Controls.Add(_lblStatus);
            Controls.Add(_btnNewCode);
            Controls.Add(lblDevices);
            Controls.Add(_lstDevices);
            Controls.Add(_btnRemove);
            Controls.Add(_lblService);
            Controls.Add(_btnService);
            Controls.Add(_btnClose);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MobileLinkForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Application mobile";
            Shown += MobileLinkForm_Shown;
            ((System.ComponentModel.ISupportInitialize)_qr.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_lstDevices).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private LabelControl lblTitle;
        private LabelControl _lblIntro;
        private PictureEdit _qr;
        private LabelControl _lblCode;
        private LabelControl _lblExpiry;
        private LabelControl _lblStatus;
        private SimpleButton _btnNewCode;
        private LabelControl lblDevices;
        private ListBoxControl _lstDevices;
        private SimpleButton _btnRemove;
        private LabelControl _lblService;
        private SimpleButton _btnService;
        private SimpleButton _btnClose;
        private System.Windows.Forms.Timer _clock;
        private System.Windows.Forms.Timer _devicesPoll;
    }
}

namespace CHIFA.Pro.Views
{
    partial class LicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
            lblTitle = new LabelControl();
            _lblStatus = new LabelControl();
            _lblDetails = new LabelControl();
            _btnRequest = new SimpleButton();
            _lblSerial = new LabelControl();
            _txtSerial = new TextEdit();
            _btnActivate = new SimpleButton();
            _btnRefresh = new SimpleButton();
            _btnClose = new SimpleButton();
            _pollTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)_txtSerial.Properties).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Appearance.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Location = new Point(30, 22);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(238, 35);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Licence SmartCHIFA";
            // 
            // _lblStatus
            // 
            _lblStatus.Appearance.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            _lblStatus.Appearance.Options.UseFont = true;
            _lblStatus.Appearance.Options.UseTextOptions = true;
            _lblStatus.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblStatus.AutoSizeMode = LabelAutoSizeMode.None;
            _lblStatus.Location = new Point(30, 72);
            _lblStatus.Margin = new Padding(4);
            _lblStatus.Name = "_lblStatus";
            _lblStatus.Size = new Size(665, 55);
            _lblStatus.TabIndex = 1;
            // 
            // _lblDetails
            // 
            _lblDetails.Appearance.Font = new Font("Segoe UI", 9.5F);
            _lblDetails.Appearance.ForeColor = Color.DimGray;
            _lblDetails.Appearance.Options.UseFont = true;
            _lblDetails.Appearance.Options.UseForeColor = true;
            _lblDetails.Appearance.Options.UseTextOptions = true;
            _lblDetails.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            _lblDetails.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblDetails.AutoSizeMode = LabelAutoSizeMode.None;
            _lblDetails.Location = new Point(30, 135);
            _lblDetails.Margin = new Padding(4);
            _lblDetails.Name = "_lblDetails";
            _lblDetails.Size = new Size(665, 125);
            _lblDetails.TabIndex = 2;
            // 
            // _btnRequest
            // 
            _btnRequest.Appearance.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _btnRequest.Appearance.Options.UseFont = true;
            _btnRequest.Location = new Point(30, 265);
            _btnRequest.Margin = new Padding(4);
            _btnRequest.Name = "_btnRequest";
            _btnRequest.Size = new Size(665, 40);
            _btnRequest.TabIndex = 3;
            _btnRequest.Text = "Demander une licence (sans numéro de série)";
            _btnRequest.Click += BtnRequest_Click;
            // 
            // _lblSerial
            // 
            _lblSerial.Appearance.Font = new Font("Microsoft Sans Serif", 10.2F);
            _lblSerial.Appearance.Options.UseFont = true;
            _lblSerial.Location = new Point(14, 314);
            _lblSerial.Margin = new Padding(4);
            _lblSerial.Name = "_lblSerial";
            _lblSerial.Size = new Size(277, 20);
            _lblSerial.TabIndex = 4;
            _lblSerial.Text = "Vous avez déjà un numéro de série ?";
            // 
            // _txtSerial
            // 
            _txtSerial.Location = new Point(30, 342);
            _txtSerial.Margin = new Padding(4);
            _txtSerial.Name = "_txtSerial";
            _txtSerial.Properties.Appearance.Font = new Font("Microsoft Sans Serif", 12F);
            _txtSerial.Properties.Appearance.Options.UseFont = true;
            _txtSerial.Properties.CharacterCasing = CharacterCasing.Upper;
            _txtSerial.Properties.MaxLength = 40;
            _txtSerial.Properties.NullValuePrompt = "XXXXX-XXXXX-XXXXX-XXXXX-XXXXX";
            _txtSerial.Size = new Size(488, 32);
            _txtSerial.TabIndex = 5;
            _txtSerial.ToolTip = "Un numéro de série ne fonctionne que sur un seul ordinateur.";
            // 
            // _btnActivate
            // 
            _btnActivate.Appearance.Font = new Font("Microsoft Sans Serif", 10.2F);
            _btnActivate.Appearance.Options.UseFont = true;
            _btnActivate.ImageOptions.Image = (Image)resources.GetObject("_btnActivate.ImageOptions.Image");
            _btnActivate.Location = new Point(532, 341);
            _btnActivate.Margin = new Padding(4);
            _btnActivate.Name = "_btnActivate";
            _btnActivate.Size = new Size(162, 40);
            _btnActivate.TabIndex = 6;
            _btnActivate.Text = "Activer";
            _btnActivate.Click += BtnActivate_Click;
            // 
            // _btnRefresh
            // 
            _btnRefresh.Appearance.Font = new Font("Microsoft Sans Serif", 10.2F);
            _btnRefresh.Appearance.Options.UseFont = true;
            _btnRefresh.ImageOptions.Image = (Image)resources.GetObject("_btnRefresh.ImageOptions.Image");
            _btnRefresh.Location = new Point(30, 402);
            _btnRefresh.Margin = new Padding(4);
            _btnRefresh.Name = "_btnRefresh";
            _btnRefresh.Size = new Size(188, 38);
            _btnRefresh.TabIndex = 7;
            _btnRefresh.Text = "Vérifier en ligne";
            _btnRefresh.Click += BtnRefresh_Click;
            // 
            // _btnClose
            // 
            _btnClose.Appearance.Font = new Font("Microsoft Sans Serif", 10.2F);
            _btnClose.Appearance.Options.UseFont = true;
            _btnClose.ImageOptions.Image = (Image)resources.GetObject("_btnClose.ImageOptions.Image");
            _btnClose.Location = new Point(532, 402);
            _btnClose.Margin = new Padding(4);
            _btnClose.Name = "_btnClose";
            _btnClose.Size = new Size(162, 38);
            _btnClose.TabIndex = 8;
            _btnClose.Text = "Fermer";
            _btnClose.Click += BtnClose_Click;
            // 
            // _pollTimer
            // 
            _pollTimer.Interval = 15000;
            _pollTimer.Tick += PollTimer_Tick;
            // 
            // LicenseForm
            // 
            AcceptButton = _btnActivate;
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(709, 451);
            Controls.Add(lblTitle);
            Controls.Add(_lblStatus);
            Controls.Add(_lblDetails);
            Controls.Add(_btnRequest);
            Controls.Add(_lblSerial);
            Controls.Add(_txtSerial);
            Controls.Add(_btnActivate);
            Controls.Add(_btnRefresh);
            Controls.Add(_btnClose);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            IconOptions.Icon = (Icon)resources.GetObject("LicenseForm.IconOptions.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LicenseForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Licence SmartCHIFA";
            ((System.ComponentModel.ISupportInitialize)_txtSerial.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private LabelControl lblTitle;
        private LabelControl _lblStatus;
        private LabelControl _lblDetails;
        private SimpleButton _btnRequest;
        private LabelControl _lblSerial;
        private TextEdit _txtSerial;
        private SimpleButton _btnActivate;
        private SimpleButton _btnRefresh;
        private SimpleButton _btnClose;
        private System.Windows.Forms.Timer _pollTimer;
    }
}

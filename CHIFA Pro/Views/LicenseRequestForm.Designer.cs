namespace CHIFA.Pro.Views
{
    partial class LicenseRequestForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseRequestForm));
            lblTitle = new LabelControl();
            lblIntro = new LabelControl();
            lblFullName = new LabelControl();
            _txtFullName = new TextEdit();
            lblPhone = new LabelControl();
            _txtPhone = new TextEdit();
            lblPharmacy = new LabelControl();
            _txtPharmacy = new TextEdit();
            lblEmail = new LabelControl();
            _txtEmail = new TextEdit();
            lblWilaya = new LabelControl();
            _cboWilaya = new ComboBoxEdit();
            lblAddress = new LabelControl();
            _txtAddress = new TextEdit();
            lblCodePs = new LabelControl();
            _txtCodePs = new TextEdit();
            lblMessage = new LabelControl();
            _txtMessage = new MemoEdit();
            _lblError = new LabelControl();
            _btnSend = new SimpleButton();
            _btnCancel = new SimpleButton();
            ((System.ComponentModel.ISupportInitialize)_txtFullName.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_txtPhone.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_txtPharmacy.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_txtEmail.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_cboWilaya.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_txtAddress.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_txtCodePs.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_txtMessage.Properties).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Appearance.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Location = new Point(30, 20);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(237, 31);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Demander une licence";
            // 
            // lblIntro
            // 
            lblIntro.Appearance.Font = new Font("Tahoma", 10F);
            lblIntro.Appearance.ForeColor = Color.DimGray;
            lblIntro.Appearance.Options.UseFont = true;
            lblIntro.Appearance.Options.UseForeColor = true;
            lblIntro.Appearance.Options.UseTextOptions = true;
            lblIntro.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            lblIntro.AutoSizeMode = LabelAutoSizeMode.None;
            lblIntro.Location = new Point(30, 62);
            lblIntro.Margin = new Padding(4);
            lblIntro.Name = "lblIntro";
            lblIntro.Size = new Size(690, 50);
            lblIntro.TabIndex = 1;
            lblIntro.Text = resources.GetString("lblIntro.Text");
            // 
            // lblFullName
            // 
            lblFullName.Appearance.Font = new Font("Tahoma", 10F);
            lblFullName.Appearance.Options.UseFont = true;
            lblFullName.Location = new Point(30, 132);
            lblFullName.Margin = new Padding(4);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(243, 21);
            lblFullName.TabIndex = 2;
            lblFullName.Text = "Nom et prénom du pharmacien *";
            // 
            // _txtFullName
            // 
            _txtFullName.Location = new Point(288, 125);
            _txtFullName.Margin = new Padding(4);
            _txtFullName.Name = "_txtFullName";
            _txtFullName.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtFullName.Properties.Appearance.Options.UseFont = true;
            _txtFullName.Properties.MaxLength = 160;
            _txtFullName.Size = new Size(432, 28);
            _txtFullName.TabIndex = 3;
            // 
            // lblPhone
            // 
            lblPhone.Appearance.Font = new Font("Tahoma", 10F);
            lblPhone.Appearance.Options.UseFont = true;
            lblPhone.Location = new Point(30, 178);
            lblPhone.Margin = new Padding(4);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(235, 21);
            lblPhone.TabIndex = 4;
            lblPhone.Text = "Téléphone * (pour le paiement)";
            // 
            // _txtPhone
            // 
            _txtPhone.Location = new Point(288, 170);
            _txtPhone.Margin = new Padding(4);
            _txtPhone.Name = "_txtPhone";
            _txtPhone.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtPhone.Properties.Appearance.Options.UseFont = true;
            _txtPhone.Properties.MaxLength = 40;
            _txtPhone.Properties.NullValuePrompt = "05 / 06 / 07 XX XX XX XX";
            _txtPhone.Size = new Size(432, 28);
            _txtPhone.TabIndex = 5;
            // 
            // lblPharmacy
            // 
            lblPharmacy.Appearance.Font = new Font("Tahoma", 10F);
            lblPharmacy.Appearance.Options.UseFont = true;
            lblPharmacy.Location = new Point(30, 222);
            lblPharmacy.Margin = new Padding(4);
            lblPharmacy.Name = "lblPharmacy";
            lblPharmacy.Size = new Size(77, 21);
            lblPharmacy.TabIndex = 6;
            lblPharmacy.Text = "Pharmacie";
            // 
            // _txtPharmacy
            // 
            _txtPharmacy.Location = new Point(288, 215);
            _txtPharmacy.Margin = new Padding(4);
            _txtPharmacy.Name = "_txtPharmacy";
            _txtPharmacy.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtPharmacy.Properties.Appearance.Options.UseFont = true;
            _txtPharmacy.Properties.MaxLength = 160;
            _txtPharmacy.Size = new Size(432, 28);
            _txtPharmacy.TabIndex = 7;
            // 
            // lblEmail
            // 
            lblEmail.Appearance.Font = new Font("Tahoma", 10F);
            lblEmail.Appearance.Options.UseFont = true;
            lblEmail.Location = new Point(30, 268);
            lblEmail.Margin = new Padding(4);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(41, 21);
            lblEmail.TabIndex = 8;
            lblEmail.Text = "Email";
            // 
            // _txtEmail
            // 
            _txtEmail.Location = new Point(288, 260);
            _txtEmail.Margin = new Padding(4);
            _txtEmail.Name = "_txtEmail";
            _txtEmail.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtEmail.Properties.Appearance.Options.UseFont = true;
            _txtEmail.Properties.MaxLength = 256;
            _txtEmail.Size = new Size(432, 28);
            _txtEmail.TabIndex = 9;
            // 
            // lblWilaya
            // 
            lblWilaya.Appearance.Font = new Font("Tahoma", 10F);
            lblWilaya.Appearance.Options.UseFont = true;
            lblWilaya.Location = new Point(30, 312);
            lblWilaya.Margin = new Padding(4);
            lblWilaya.Name = "lblWilaya";
            lblWilaya.Size = new Size(49, 21);
            lblWilaya.TabIndex = 10;
            lblWilaya.Text = "Wilaya";
            // 
            // _cboWilaya
            // 
            _cboWilaya.Location = new Point(288, 305);
            _cboWilaya.Margin = new Padding(4);
            _cboWilaya.Name = "_cboWilaya";
            _cboWilaya.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _cboWilaya.Properties.Appearance.Options.UseFont = true;
            _cboWilaya.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            _cboWilaya.Properties.Items.AddRange(new object[] { "01 Adrar", "02 Chlef", "03 Laghouat", "04 Oum El Bouaghi", "05 Batna", "06 Béjaïa", "07 Biskra", "08 Béchar", "09 Blida", "10 Bouira", "11 Tamanrasset", "12 Tébessa", "13 Tlemcen", "14 Tiaret", "15 Tizi Ouzou", "16 Alger", "17 Djelfa", "18 Jijel", "19 Sétif", "20 Saïda", "21 Skikda", "22 Sidi Bel Abbès", "23 Annaba", "24 Guelma", "25 Constantine", "26 Médéa", "27 Mostaganem", "28 M'Sila", "29 Mascara", "30 Ouargla", "31 Oran", "32 El Bayadh", "33 Illizi", "34 Bordj Bou Arréridj", "35 Boumerdès", "36 El Tarf", "37 Tindouf", "38 Tissemsilt", "39 El Oued", "40 Khenchela", "41 Souk Ahras", "42 Tipaza", "43 Mila", "44 Aïn Defla", "45 Naâma", "46 Aïn Témouchent", "47 Ghardaïa", "48 Relizane", "49 Timimoun", "50 Bordj Badji Mokhtar", "51 Ouled Djellal", "52 Béni Abbès", "53 In Salah", "54 In Guezzam", "55 Touggourt", "56 Djanet", "57 El M'Ghair", "58 El Meniaa" });
            _cboWilaya.Properties.MaxLength = 64;
            _cboWilaya.Size = new Size(432, 28);
            _cboWilaya.TabIndex = 11;
            // 
            // lblAddress
            // 
            lblAddress.Appearance.Font = new Font("Tahoma", 10F);
            lblAddress.Appearance.Options.UseFont = true;
            lblAddress.Location = new Point(30, 358);
            lblAddress.Margin = new Padding(4);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(60, 21);
            lblAddress.TabIndex = 12;
            lblAddress.Text = "Adresse";
            // 
            // _txtAddress
            // 
            _txtAddress.Location = new Point(288, 350);
            _txtAddress.Margin = new Padding(4);
            _txtAddress.Name = "_txtAddress";
            _txtAddress.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtAddress.Properties.Appearance.Options.UseFont = true;
            _txtAddress.Properties.MaxLength = 256;
            _txtAddress.Size = new Size(432, 28);
            _txtAddress.TabIndex = 13;
            // 
            // lblCodePs
            // 
            lblCodePs.Appearance.Font = new Font("Tahoma", 10F);
            lblCodePs.Appearance.Options.UseFont = true;
            lblCodePs.Location = new Point(30, 402);
            lblCodePs.Margin = new Padding(4);
            lblCodePs.Name = "lblCodePs";
            lblCodePs.Size = new Size(60, 21);
            lblCodePs.TabIndex = 14;
            lblCodePs.Text = "Code PS";
            // 
            // _txtCodePs
            // 
            _txtCodePs.Location = new Point(288, 395);
            _txtCodePs.Margin = new Padding(4);
            _txtCodePs.Name = "_txtCodePs";
            _txtCodePs.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtCodePs.Properties.Appearance.Options.UseFont = true;
            _txtCodePs.Properties.MaxLength = 64;
            _txtCodePs.Size = new Size(432, 28);
            _txtCodePs.TabIndex = 15;
            // 
            // lblMessage
            // 
            lblMessage.Appearance.Font = new Font("Tahoma", 10F);
            lblMessage.Appearance.Options.UseFont = true;
            lblMessage.Location = new Point(30, 448);
            lblMessage.Margin = new Padding(4);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(149, 21);
            lblMessage.TabIndex = 16;
            lblMessage.Text = "Message (facultatif)";
            // 
            // _txtMessage
            // 
            _txtMessage.Location = new Point(288, 440);
            _txtMessage.Margin = new Padding(4);
            _txtMessage.Name = "_txtMessage";
            _txtMessage.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtMessage.Properties.Appearance.Options.UseFont = true;
            _txtMessage.Properties.MaxLength = 1000;
            _txtMessage.Size = new Size(432, 68);
            _txtMessage.TabIndex = 17;
            // 
            // _lblError
            // 
            _lblError.Appearance.Font = new Font("Tahoma", 10F);
            _lblError.Appearance.ForeColor = Color.Crimson;
            _lblError.Appearance.Options.UseFont = true;
            _lblError.Appearance.Options.UseForeColor = true;
            _lblError.Appearance.Options.UseTextOptions = true;
            _lblError.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblError.AutoSizeMode = LabelAutoSizeMode.None;
            _lblError.Location = new Point(30, 520);
            _lblError.Margin = new Padding(4);
            _lblError.Name = "_lblError";
            _lblError.Size = new Size(690, 42);
            _lblError.TabIndex = 18;
            // 
            // _btnSend
            // 
            _btnSend.Appearance.Font = new Font("Tahoma", 10F);
            _btnSend.Appearance.Options.UseFont = true;
            _btnSend.ImageOptions.Image = (Image)resources.GetObject("_btnSend.ImageOptions.Image");
            _btnSend.Location = new Point(395, 592);
            _btnSend.Margin = new Padding(4);
            _btnSend.Name = "_btnSend";
            _btnSend.Size = new Size(200, 40);
            _btnSend.TabIndex = 19;
            _btnSend.Text = "Envoyer la demande";
            _btnSend.Click += BtnSend_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.Appearance.Font = new Font("Tahoma", 10F);
            _btnCancel.Appearance.Options.UseFont = true;
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.ImageOptions.Image = (Image)resources.GetObject("_btnCancel.ImageOptions.Image");
            _btnCancel.Location = new Point(608, 592);
            _btnCancel.Margin = new Padding(4);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(112, 40);
            _btnCancel.TabIndex = 20;
            _btnCancel.Text = "Annuler";
            // 
            // LicenseRequestForm
            // 
            AcceptButton = _btnSend;
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnCancel;
            ClientSize = new Size(734, 661);
            Controls.Add(lblTitle);
            Controls.Add(lblIntro);
            Controls.Add(lblFullName);
            Controls.Add(_txtFullName);
            Controls.Add(lblPhone);
            Controls.Add(_txtPhone);
            Controls.Add(lblPharmacy);
            Controls.Add(_txtPharmacy);
            Controls.Add(lblEmail);
            Controls.Add(_txtEmail);
            Controls.Add(lblWilaya);
            Controls.Add(_cboWilaya);
            Controls.Add(lblAddress);
            Controls.Add(_txtAddress);
            Controls.Add(lblCodePs);
            Controls.Add(_txtCodePs);
            Controls.Add(lblMessage);
            Controls.Add(_txtMessage);
            Controls.Add(_lblError);
            Controls.Add(_btnSend);
            Controls.Add(_btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            IconOptions.Icon = (Icon)resources.GetObject("LicenseRequestForm.IconOptions.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LicenseRequestForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Demande de licence SmartCHIFA";
            Shown += LicenseRequestForm_Shown;
            ((System.ComponentModel.ISupportInitialize)_txtFullName.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_txtPhone.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_txtPharmacy.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_txtEmail.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_cboWilaya.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_txtAddress.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_txtCodePs.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_txtMessage.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private LabelControl lblTitle;
        private LabelControl lblIntro;
        private LabelControl lblFullName;
        private TextEdit _txtFullName;
        private LabelControl lblPhone;
        private TextEdit _txtPhone;
        private LabelControl lblPharmacy;
        private TextEdit _txtPharmacy;
        private LabelControl lblEmail;
        private TextEdit _txtEmail;
        private LabelControl lblWilaya;
        private ComboBoxEdit _cboWilaya;
        private LabelControl lblAddress;
        private TextEdit _txtAddress;
        private LabelControl lblCodePs;
        private TextEdit _txtCodePs;
        private LabelControl lblMessage;
        private MemoEdit _txtMessage;
        private LabelControl _lblError;
        private SimpleButton _btnSend;
        private SimpleButton _btnCancel;
    }
}

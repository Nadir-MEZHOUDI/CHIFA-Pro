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
            lblTitle.Location = new Point(24, 16);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Demander une licence";
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
            lblIntro.Size = new Size(552, 40);
            lblIntro.TabIndex = 1;
            lblIntro.Text = "Vérifiez vos informations puis envoyez la demande. SmartApp vous appelle pour le paiement de l'abonnement ; dès qu'il est validé, SmartCHIFA s'active tout seul sur ce poste, sans numéro de série à saisir.";
            //
            // lblFullName
            //
            lblFullName.Location = new Point(24, 106);
            lblFullName.Name = "lblFullName";
            lblFullName.TabIndex = 2;
            lblFullName.Text = "Nom et prénom du pharmacien *";
            //
            // _txtFullName
            //
            _txtFullName.Location = new Point(230, 100);
            _txtFullName.Name = "_txtFullName";
            _txtFullName.Properties.MaxLength = 160;
            _txtFullName.Size = new Size(346, 26);
            _txtFullName.TabIndex = 3;
            //
            // lblPhone
            //
            lblPhone.Location = new Point(24, 142);
            lblPhone.Name = "lblPhone";
            lblPhone.TabIndex = 4;
            lblPhone.Text = "Téléphone * (pour le paiement)";
            //
            // _txtPhone
            //
            _txtPhone.Location = new Point(230, 136);
            _txtPhone.Name = "_txtPhone";
            _txtPhone.Properties.MaxLength = 40;
            _txtPhone.Properties.NullValuePrompt = "05 / 06 / 07 XX XX XX XX";
            _txtPhone.Size = new Size(346, 26);
            _txtPhone.TabIndex = 5;
            //
            // lblPharmacy
            //
            lblPharmacy.Location = new Point(24, 178);
            lblPharmacy.Name = "lblPharmacy";
            lblPharmacy.TabIndex = 6;
            lblPharmacy.Text = "Pharmacie";
            //
            // _txtPharmacy
            //
            _txtPharmacy.Location = new Point(230, 172);
            _txtPharmacy.Name = "_txtPharmacy";
            _txtPharmacy.Properties.MaxLength = 160;
            _txtPharmacy.Size = new Size(346, 26);
            _txtPharmacy.TabIndex = 7;
            //
            // lblEmail
            //
            lblEmail.Location = new Point(24, 214);
            lblEmail.Name = "lblEmail";
            lblEmail.TabIndex = 8;
            lblEmail.Text = "Email";
            //
            // _txtEmail
            //
            _txtEmail.Location = new Point(230, 208);
            _txtEmail.Name = "_txtEmail";
            _txtEmail.Properties.MaxLength = 256;
            _txtEmail.Size = new Size(346, 26);
            _txtEmail.TabIndex = 9;
            //
            // lblWilaya
            //
            lblWilaya.Location = new Point(24, 250);
            lblWilaya.Name = "lblWilaya";
            lblWilaya.TabIndex = 10;
            lblWilaya.Text = "Wilaya";
            //
            // _cboWilaya
            //
            _cboWilaya.Location = new Point(230, 244);
            _cboWilaya.Name = "_cboWilaya";
            _cboWilaya.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            _cboWilaya.Properties.Items.AddRange(new object[] {
                "01 Adrar", "02 Chlef", "03 Laghouat", "04 Oum El Bouaghi", "05 Batna", "06 Béjaïa", "07 Biskra", "08 Béchar",
                "09 Blida", "10 Bouira", "11 Tamanrasset", "12 Tébessa", "13 Tlemcen", "14 Tiaret", "15 Tizi Ouzou", "16 Alger",
                "17 Djelfa", "18 Jijel", "19 Sétif", "20 Saïda", "21 Skikda", "22 Sidi Bel Abbès", "23 Annaba", "24 Guelma",
                "25 Constantine", "26 Médéa", "27 Mostaganem", "28 M'Sila", "29 Mascara", "30 Ouargla", "31 Oran", "32 El Bayadh",
                "33 Illizi", "34 Bordj Bou Arréridj", "35 Boumerdès", "36 El Tarf", "37 Tindouf", "38 Tissemsilt", "39 El Oued",
                "40 Khenchela", "41 Souk Ahras", "42 Tipaza", "43 Mila", "44 Aïn Defla", "45 Naâma", "46 Aïn Témouchent",
                "47 Ghardaïa", "48 Relizane", "49 Timimoun", "50 Bordj Badji Mokhtar", "51 Ouled Djellal", "52 Béni Abbès",
                "53 In Salah", "54 In Guezzam", "55 Touggourt", "56 Djanet", "57 El M'Ghair", "58 El Meniaa" });
            _cboWilaya.Properties.MaxLength = 64;
            _cboWilaya.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            _cboWilaya.Size = new Size(346, 26);
            _cboWilaya.TabIndex = 11;
            //
            // lblAddress
            //
            lblAddress.Location = new Point(24, 286);
            lblAddress.Name = "lblAddress";
            lblAddress.TabIndex = 12;
            lblAddress.Text = "Adresse";
            //
            // _txtAddress
            //
            _txtAddress.Location = new Point(230, 280);
            _txtAddress.Name = "_txtAddress";
            _txtAddress.Properties.MaxLength = 256;
            _txtAddress.Size = new Size(346, 26);
            _txtAddress.TabIndex = 13;
            //
            // lblCodePs
            //
            lblCodePs.Location = new Point(24, 322);
            lblCodePs.Name = "lblCodePs";
            lblCodePs.TabIndex = 14;
            lblCodePs.Text = "Code PS";
            //
            // _txtCodePs
            //
            _txtCodePs.Location = new Point(230, 316);
            _txtCodePs.Name = "_txtCodePs";
            _txtCodePs.Properties.MaxLength = 64;
            _txtCodePs.Size = new Size(346, 26);
            _txtCodePs.TabIndex = 15;
            //
            // lblMessage
            //
            lblMessage.Location = new Point(24, 358);
            lblMessage.Name = "lblMessage";
            lblMessage.TabIndex = 16;
            lblMessage.Text = "Message (facultatif)";
            //
            // _txtMessage
            //
            _txtMessage.Location = new Point(230, 352);
            _txtMessage.Name = "_txtMessage";
            _txtMessage.Properties.MaxLength = 1000;
            _txtMessage.Size = new Size(346, 54);
            _txtMessage.TabIndex = 17;
            //
            // _lblError
            //
            _lblError.Appearance.ForeColor = Color.Crimson;
            _lblError.Appearance.Options.UseForeColor = true;
            _lblError.Appearance.Options.UseTextOptions = true;
            _lblError.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblError.AutoSizeMode = LabelAutoSizeMode.None;
            _lblError.Location = new Point(24, 416);
            _lblError.Name = "_lblError";
            _lblError.Size = new Size(552, 34);
            _lblError.TabIndex = 18;
            //
            // _btnSend
            //
            _btnSend.Location = new Point(316, 474);
            _btnSend.Name = "_btnSend";
            _btnSend.Size = new Size(160, 32);
            _btnSend.TabIndex = 19;
            _btnSend.Text = "Envoyer la demande";
            _btnSend.Click += BtnSend_Click;
            //
            // _btnCancel
            //
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.Location = new Point(486, 474);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(90, 32);
            _btnCancel.TabIndex = 20;
            _btnCancel.Text = "Annuler";
            //
            // LicenseRequestForm
            //
            AcceptButton = _btnSend;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnCancel;
            ClientSize = new Size(600, 520);
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

namespace CHIFA.Pro.Views
{
    partial class SecurityForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecurityForm));
            lblTitle = new LabelControl();
            lblIntro = new LabelControl();
            grpScreens = new GroupControl();
            chkStatistics = new CheckEdit();
            chkBordereaux = new CheckEdit();
            chkMobile = new CheckEdit();
            chkLicense = new CheckEdit();
            grpHome = new GroupControl();
            chkHomeFactures = new CheckEdit();
            chkHomeMontant = new CheckEdit();
            chkHomeMarge = new CheckEdit();
            grpPassword = new GroupControl();
            lblNew = new LabelControl();
            _txtNew = new TextEdit();
            lblConfirm = new LabelControl();
            _txtConfirm = new TextEdit();
            _lblError = new LabelControl();
            _btnSave = new SimpleButton();
            _btnCancel = new SimpleButton();
            ((System.ComponentModel.ISupportInitialize)grpScreens).BeginInit();
            grpScreens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chkStatistics.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkBordereaux.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkMobile.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkLicense.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grpHome).BeginInit();
            grpHome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chkHomeFactures.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkHomeMontant.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chkHomeMarge.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)grpPassword).BeginInit();
            grpPassword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_txtNew.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_txtConfirm.Properties).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Appearance.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Location = new Point(30, 20);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(205, 31);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Données protégées";
            // 
            // lblIntro
            // 
            lblIntro.Appearance.ForeColor = Color.DimGray;
            lblIntro.Appearance.Options.UseForeColor = true;
            lblIntro.Appearance.Options.UseTextOptions = true;
            lblIntro.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            lblIntro.AutoSizeMode = LabelAutoSizeMode.None;
            lblIntro.Location = new Point(9, 62);
            lblIntro.Margin = new Padding(4);
            lblIntro.Name = "lblIntro";
            lblIntro.Size = new Size(590, 25);
            lblIntro.TabIndex = 1;
            lblIntro.Text = "Les éléments cochés sont masqués tant que l'administrateur n'est pas connecté.";
            // 
            // grpScreens
            // 
            grpScreens.Controls.Add(chkStatistics);
            grpScreens.Controls.Add(chkBordereaux);
            grpScreens.Controls.Add(chkMobile);
            grpScreens.Controls.Add(chkLicense);
            grpScreens.Location = new Point(30, 100);
            grpScreens.Margin = new Padding(4);
            grpScreens.Name = "grpScreens";
            grpScreens.Size = new Size(590, 185);
            grpScreens.TabIndex = 2;
            grpScreens.Text = "Écrans";
            // 
            // chkStatistics
            // 
            chkStatistics.Location = new Point(18, 40);
            chkStatistics.Margin = new Padding(4);
            chkStatistics.Name = "chkStatistics";
            chkStatistics.Properties.Appearance.Font = new Font("Tahoma", 10F);
            chkStatistics.Properties.Appearance.Options.UseFont = true;
            chkStatistics.Properties.Caption = "Statistiques";
            chkStatistics.Size = new Size(550, 25);
            chkStatistics.TabIndex = 0;
            // 
            // chkBordereaux
            // 
            chkBordereaux.Location = new Point(18, 75);
            chkBordereaux.Margin = new Padding(4);
            chkBordereaux.Name = "chkBordereaux";
            chkBordereaux.Properties.Appearance.Font = new Font("Tahoma", 10F);
            chkBordereaux.Properties.Appearance.Options.UseFont = true;
            chkBordereaux.Properties.Caption = "Bordereaux";
            chkBordereaux.Size = new Size(550, 25);
            chkBordereaux.TabIndex = 1;
            // 
            // chkMobile
            // 
            chkMobile.Location = new Point(18, 110);
            chkMobile.Margin = new Padding(4);
            chkMobile.Name = "chkMobile";
            chkMobile.Properties.Appearance.Font = new Font("Tahoma", 10F);
            chkMobile.Properties.Appearance.Options.UseFont = true;
            chkMobile.Properties.Caption = "Application mobile (liaison du téléphone)";
            chkMobile.Size = new Size(550, 25);
            chkMobile.TabIndex = 2;
            // 
            // chkLicense
            // 
            chkLicense.Location = new Point(18, 145);
            chkLicense.Margin = new Padding(4);
            chkLicense.Name = "chkLicense";
            chkLicense.Properties.Appearance.Font = new Font("Tahoma", 10F);
            chkLicense.Properties.Appearance.Options.UseFont = true;
            chkLicense.Properties.Caption = "Licence";
            chkLicense.Size = new Size(550, 25);
            chkLicense.TabIndex = 3;
            // 
            // grpHome
            // 
            grpHome.Controls.Add(chkHomeFactures);
            grpHome.Controls.Add(chkHomeMontant);
            grpHome.Controls.Add(chkHomeMarge);
            grpHome.Location = new Point(30, 298);
            grpHome.Margin = new Padding(4);
            grpHome.Name = "grpHome";
            grpHome.Size = new Size(590, 150);
            grpHome.TabIndex = 3;
            grpHome.Text = "Écran d'accueil";
            // 
            // chkHomeFactures
            // 
            chkHomeFactures.Location = new Point(18, 40);
            chkHomeFactures.Margin = new Padding(4);
            chkHomeFactures.Name = "chkHomeFactures";
            chkHomeFactures.Properties.Appearance.Font = new Font("Tahoma", 10F);
            chkHomeFactures.Properties.Appearance.Options.UseFont = true;
            chkHomeFactures.Properties.Caption = "Nombre de factures";
            chkHomeFactures.Size = new Size(550, 25);
            chkHomeFactures.TabIndex = 0;
            // 
            // chkHomeMontant
            // 
            chkHomeMontant.Location = new Point(18, 75);
            chkHomeMontant.Margin = new Padding(4);
            chkHomeMontant.Name = "chkHomeMontant";
            chkHomeMontant.Properties.Appearance.Font = new Font("Tahoma", 10F);
            chkHomeMontant.Properties.Appearance.Options.UseFont = true;
            chkHomeMontant.Properties.Caption = "Chiffre d'affaires (montant par jour)";
            chkHomeMontant.Size = new Size(550, 25);
            chkHomeMontant.TabIndex = 1;
            // 
            // chkHomeMarge
            // 
            chkHomeMarge.Location = new Point(18, 110);
            chkHomeMarge.Margin = new Padding(4);
            chkHomeMarge.Name = "chkHomeMarge";
            chkHomeMarge.Properties.Appearance.Font = new Font("Tahoma", 10F);
            chkHomeMarge.Properties.Appearance.Options.UseFont = true;
            chkHomeMarge.Properties.Caption = "Marge bénéficiaire (majoration)";
            chkHomeMarge.Size = new Size(550, 25);
            chkHomeMarge.TabIndex = 2;
            // 
            // grpPassword
            // 
            grpPassword.Controls.Add(lblNew);
            grpPassword.Controls.Add(_txtNew);
            grpPassword.Controls.Add(lblConfirm);
            grpPassword.Controls.Add(_txtConfirm);
            grpPassword.Location = new Point(30, 460);
            grpPassword.Margin = new Padding(4);
            grpPassword.Name = "grpPassword";
            grpPassword.Size = new Size(590, 155);
            grpPassword.TabIndex = 4;
            grpPassword.Text = "Mot de passe administrateur";
            // 
            // lblNew
            // 
            lblNew.Appearance.Font = new Font("Tahoma", 10F);
            lblNew.Appearance.Options.UseFont = true;
            lblNew.Location = new Point(13, 40);
            lblNew.Margin = new Padding(4);
            lblNew.Name = "lblNew";
            lblNew.Size = new Size(75, 21);
            lblNew.TabIndex = 0;
            lblNew.Text = "Nouveau :";
            // 
            // _txtNew
            // 
            _txtNew.Location = new Point(131, 35);
            _txtNew.Margin = new Padding(4);
            _txtNew.Name = "_txtNew";
            _txtNew.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtNew.Properties.Appearance.Options.UseFont = true;
            _txtNew.Properties.NullValuePrompt = "Laisser vide pour ne pas changer";
            _txtNew.Properties.UseSystemPasswordChar = true;
            _txtNew.Size = new Size(425, 28);
            _txtNew.TabIndex = 1;
            // 
            // lblConfirm
            // 
            lblConfirm.Appearance.Font = new Font("Tahoma", 10F);
            lblConfirm.Appearance.Options.UseFont = true;
            lblConfirm.Location = new Point(13, 85);
            lblConfirm.Margin = new Padding(4);
            lblConfirm.Name = "lblConfirm";
            lblConfirm.Size = new Size(83, 21);
            lblConfirm.TabIndex = 2;
            lblConfirm.Text = "Confirmer :";
            // 
            // _txtConfirm
            // 
            _txtConfirm.Location = new Point(131, 80);
            _txtConfirm.Margin = new Padding(4);
            _txtConfirm.Name = "_txtConfirm";
            _txtConfirm.Properties.Appearance.Font = new Font("Tahoma", 10F);
            _txtConfirm.Properties.Appearance.Options.UseFont = true;
            _txtConfirm.Properties.UseSystemPasswordChar = true;
            _txtConfirm.Size = new Size(425, 28);
            _txtConfirm.TabIndex = 3;
            // 
            // _lblError
            // 
            _lblError.Appearance.ForeColor = Color.Firebrick;
            _lblError.Appearance.Options.UseForeColor = true;
            _lblError.Location = new Point(30, 635);
            _lblError.Margin = new Padding(4);
            _lblError.Name = "_lblError";
            _lblError.Size = new Size(0, 16);
            _lblError.TabIndex = 5;
            // 
            // _btnSave
            // 
            _btnSave.Appearance.Font = new Font("Tahoma", 10F);
            _btnSave.Appearance.Options.UseFont = true;
            _btnSave.ImageOptions.Image = (Image)resources.GetObject("_btnSave.ImageOptions.Image");
            _btnSave.Location = new Point(332, 623);
            _btnSave.Margin = new Padding(4);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(138, 40);
            _btnSave.TabIndex = 6;
            _btnSave.Text = "Enregistrer";
            _btnSave.Click += BtnSave_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.Appearance.Font = new Font("Tahoma", 10F);
            _btnCancel.Appearance.Options.UseFont = true;
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.ImageOptions.Image = (Image)resources.GetObject("_btnCancel.ImageOptions.Image");
            _btnCancel.Location = new Point(482, 623);
            _btnCancel.Margin = new Padding(4);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(138, 40);
            _btnCancel.TabIndex = 7;
            _btnCancel.Text = "Annuler";
            // 
            // SecurityForm
            // 
            AcceptButton = _btnSave;
            Appearance.Options.UseFont = true;
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnCancel;
            ClientSize = new Size(642, 676);
            Controls.Add(lblTitle);
            Controls.Add(lblIntro);
            Controls.Add(grpScreens);
            Controls.Add(grpHome);
            Controls.Add(grpPassword);
            Controls.Add(_lblError);
            Controls.Add(_btnSave);
            Controls.Add(_btnCancel);
            Font = new Font("Tahoma", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            IconOptions.Image = (Image)resources.GetObject("SecurityForm.IconOptions.Image");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SecurityForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sécurité";
            ((System.ComponentModel.ISupportInitialize)grpScreens).EndInit();
            grpScreens.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chkStatistics.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkBordereaux.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkMobile.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkLicense.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)grpHome).EndInit();
            grpHome.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chkHomeFactures.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkHomeMontant.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chkHomeMarge.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)grpPassword).EndInit();
            grpPassword.ResumeLayout(false);
            grpPassword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_txtNew.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)_txtConfirm.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private LabelControl lblTitle;
        private LabelControl lblIntro;
        private GroupControl grpScreens;
        private CheckEdit chkStatistics;
        private CheckEdit chkBordereaux;
        private CheckEdit chkMobile;
        private CheckEdit chkLicense;
        private GroupControl grpHome;
        private CheckEdit chkHomeFactures;
        private CheckEdit chkHomeMontant;
        private CheckEdit chkHomeMarge;
        private GroupControl grpPassword;
        private LabelControl lblNew;
        private TextEdit _txtNew;
        private LabelControl lblConfirm;
        private TextEdit _txtConfirm;
        private LabelControl _lblError;
        private SimpleButton _btnSave;
        private SimpleButton _btnCancel;
    }
}

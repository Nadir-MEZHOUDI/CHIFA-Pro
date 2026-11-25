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
            lblTitle.Location = new Point(24, 16);
            lblTitle.Name = "lblTitle";
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
            lblIntro.Location = new Point(24, 50);
            lblIntro.Name = "lblIntro";
            lblIntro.Size = new Size(472, 20);
            lblIntro.TabIndex = 1;
            lblIntro.Text = "Les éléments cochés sont masqués tant que l'administrateur n'est pas connecté.";
            //
            // grpScreens
            //
            grpScreens.Controls.Add(chkStatistics);
            grpScreens.Controls.Add(chkBordereaux);
            grpScreens.Controls.Add(chkMobile);
            grpScreens.Controls.Add(chkLicense);
            grpScreens.Location = new Point(24, 80);
            grpScreens.Name = "grpScreens";
            grpScreens.Size = new Size(472, 148);
            grpScreens.TabIndex = 2;
            grpScreens.Text = "Écrans";
            //
            // chkStatistics
            //
            chkStatistics.Location = new Point(14, 32);
            chkStatistics.Name = "chkStatistics";
            chkStatistics.Properties.Caption = "Statistiques";
            chkStatistics.Size = new Size(440, 24);
            chkStatistics.TabIndex = 0;
            //
            // chkBordereaux
            //
            chkBordereaux.Location = new Point(14, 60);
            chkBordereaux.Name = "chkBordereaux";
            chkBordereaux.Properties.Caption = "Bordereaux";
            chkBordereaux.Size = new Size(440, 24);
            chkBordereaux.TabIndex = 1;
            //
            // chkMobile
            //
            chkMobile.Location = new Point(14, 88);
            chkMobile.Name = "chkMobile";
            chkMobile.Properties.Caption = "Application mobile (liaison du téléphone)";
            chkMobile.Size = new Size(440, 24);
            chkMobile.TabIndex = 2;
            //
            // chkLicense
            //
            chkLicense.Location = new Point(14, 116);
            chkLicense.Name = "chkLicense";
            chkLicense.Properties.Caption = "Licence";
            chkLicense.Size = new Size(440, 24);
            chkLicense.TabIndex = 3;
            //
            // grpHome
            //
            grpHome.Controls.Add(chkHomeFactures);
            grpHome.Controls.Add(chkHomeMontant);
            grpHome.Controls.Add(chkHomeMarge);
            grpHome.Location = new Point(24, 238);
            grpHome.Name = "grpHome";
            grpHome.Size = new Size(472, 120);
            grpHome.TabIndex = 3;
            grpHome.Text = "Écran d'accueil";
            //
            // chkHomeFactures
            //
            chkHomeFactures.Location = new Point(14, 32);
            chkHomeFactures.Name = "chkHomeFactures";
            chkHomeFactures.Properties.Caption = "Nombre de factures";
            chkHomeFactures.Size = new Size(440, 24);
            chkHomeFactures.TabIndex = 0;
            //
            // chkHomeMontant
            //
            chkHomeMontant.Location = new Point(14, 60);
            chkHomeMontant.Name = "chkHomeMontant";
            chkHomeMontant.Properties.Caption = "Chiffre d'affaires (montant par jour)";
            chkHomeMontant.Size = new Size(440, 24);
            chkHomeMontant.TabIndex = 1;
            //
            // chkHomeMarge
            //
            chkHomeMarge.Location = new Point(14, 88);
            chkHomeMarge.Name = "chkHomeMarge";
            chkHomeMarge.Properties.Caption = "Marge bénéficiaire (majoration)";
            chkHomeMarge.Size = new Size(440, 24);
            chkHomeMarge.TabIndex = 2;
            //
            // grpPassword
            //
            grpPassword.Controls.Add(lblNew);
            grpPassword.Controls.Add(_txtNew);
            grpPassword.Controls.Add(lblConfirm);
            grpPassword.Controls.Add(_txtConfirm);
            grpPassword.Location = new Point(24, 368);
            grpPassword.Name = "grpPassword";
            grpPassword.Size = new Size(472, 130);
            grpPassword.TabIndex = 4;
            grpPassword.Text = "Mot de passe administrateur";
            //
            // lblNew
            //
            lblNew.Location = new Point(16, 40);
            lblNew.Name = "lblNew";
            lblNew.TabIndex = 0;
            lblNew.Text = "Nouveau :";
            //
            // _txtNew
            //
            _txtNew.Location = new Point(110, 36);
            _txtNew.Name = "_txtNew";
            _txtNew.Properties.NullValuePrompt = "Laisser vide pour ne pas changer";
            _txtNew.Properties.UseSystemPasswordChar = true;
            _txtNew.Size = new Size(340, 26);
            _txtNew.TabIndex = 1;
            //
            // lblConfirm
            //
            lblConfirm.Location = new Point(16, 76);
            lblConfirm.Name = "lblConfirm";
            lblConfirm.TabIndex = 2;
            lblConfirm.Text = "Confirmer :";
            //
            // _txtConfirm
            //
            _txtConfirm.Location = new Point(110, 72);
            _txtConfirm.Name = "_txtConfirm";
            _txtConfirm.Properties.UseSystemPasswordChar = true;
            _txtConfirm.Size = new Size(340, 26);
            _txtConfirm.TabIndex = 3;
            //
            // _lblError
            //
            _lblError.Appearance.ForeColor = Color.Firebrick;
            _lblError.Appearance.Options.UseForeColor = true;
            _lblError.Location = new Point(24, 508);
            _lblError.Name = "_lblError";
            _lblError.TabIndex = 5;
            //
            // _btnSave
            //
            _btnSave.Location = new Point(266, 542);
            _btnSave.Name = "_btnSave";
            _btnSave.Size = new Size(110, 32);
            _btnSave.TabIndex = 6;
            _btnSave.Text = "Enregistrer";
            _btnSave.Click += BtnSave_Click;
            //
            // _btnCancel
            //
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.Location = new Point(386, 542);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(110, 32);
            _btnCancel.TabIndex = 7;
            _btnCancel.Text = "Annuler";
            //
            // SecurityForm
            //
            AcceptButton = _btnSave;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnCancel;
            ClientSize = new Size(520, 590);
            Controls.Add(lblTitle);
            Controls.Add(lblIntro);
            Controls.Add(grpScreens);
            Controls.Add(grpHome);
            Controls.Add(grpPassword);
            Controls.Add(_lblError);
            Controls.Add(_btnSave);
            Controls.Add(_btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
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

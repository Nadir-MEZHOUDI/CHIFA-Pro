namespace CHIFA.Pro.Views
{
    partial class LoginForm
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
            _txtPassword = new TextEdit();
            _lblHint = new LabelControl();
            _lblError = new LabelControl();
            _btnOk = new SimpleButton();
            _btnCancel = new SimpleButton();
            ((System.ComponentModel.ISupportInitialize)_txtPassword.Properties).BeginInit();
            SuspendLayout();
            //
            // lblTitle
            //
            lblTitle.Appearance.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.Appearance.Options.UseFont = true;
            lblTitle.Location = new Point(24, 18);
            lblTitle.Name = "lblTitle";
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Mot de passe administrateur";
            //
            // _txtPassword
            //
            _txtPassword.Location = new Point(24, 56);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.Properties.UseSystemPasswordChar = true;
            _txtPassword.Size = new Size(372, 28);
            _txtPassword.TabIndex = 1;
            //
            // _lblHint
            //
            _lblHint.Appearance.ForeColor = Color.DimGray;
            _lblHint.Appearance.Options.UseForeColor = true;
            _lblHint.Appearance.Options.UseTextOptions = true;
            _lblHint.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblHint.AutoSizeMode = LabelAutoSizeMode.None;
            _lblHint.Location = new Point(24, 92);
            _lblHint.Name = "_lblHint";
            _lblHint.Size = new Size(372, 20);
            _lblHint.TabIndex = 2;
            //
            // _lblError
            //
            _lblError.Appearance.ForeColor = Color.Firebrick;
            _lblError.Appearance.Options.UseForeColor = true;
            _lblError.Location = new Point(24, 116);
            _lblError.Name = "_lblError";
            _lblError.TabIndex = 3;
            //
            // _btnOk
            //
            _btnOk.Location = new Point(170, 150);
            _btnOk.Name = "_btnOk";
            _btnOk.Size = new Size(110, 32);
            _btnOk.TabIndex = 4;
            _btnOk.Text = "Connexion";
            _btnOk.Click += BtnOk_Click;
            //
            // _btnCancel
            //
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.Location = new Point(286, 150);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(110, 32);
            _btnCancel.TabIndex = 5;
            _btnCancel.Text = "Annuler";
            //
            // LoginForm
            //
            AcceptButton = _btnOk;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnCancel;
            ClientSize = new Size(420, 200);
            Controls.Add(lblTitle);
            Controls.Add(_txtPassword);
            Controls.Add(_lblHint);
            Controls.Add(_lblError);
            Controls.Add(_btnOk);
            Controls.Add(_btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Connexion administrateur";
            Shown += LoginForm_Shown;
            ((System.ComponentModel.ISupportInitialize)_txtPassword.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private LabelControl lblTitle;
        private TextEdit _txtPassword;
        private LabelControl _lblHint;
        private LabelControl _lblError;
        private SimpleButton _btnOk;
        private SimpleButton _btnCancel;
    }
}

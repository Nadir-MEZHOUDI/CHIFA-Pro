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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
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
            lblTitle.Location = new Point(11, 7);
            lblTitle.Margin = new Padding(4);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(272, 28);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Mot de passe administrateur";
            // 
            // _txtPassword
            // 
            _txtPassword.Location = new Point(30, 48);
            _txtPassword.Margin = new Padding(4);
            _txtPassword.Name = "_txtPassword";
            _txtPassword.Properties.Appearance.Font = new Font("Tahoma", 12F);
            _txtPassword.Properties.Appearance.Options.UseFont = true;
            _txtPassword.Properties.UseSystemPasswordChar = true;
            _txtPassword.Size = new Size(465, 30);
            _txtPassword.TabIndex = 1;
            // 
            // _lblHint
            // 
            _lblHint.Appearance.ForeColor = Color.DimGray;
            _lblHint.Appearance.Options.UseForeColor = true;
            _lblHint.Appearance.Options.UseTextOptions = true;
            _lblHint.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            _lblHint.AutoSizeMode = LabelAutoSizeMode.None;
            _lblHint.LineVisible = true;
            _lblHint.Location = new Point(30, 76);
            _lblHint.Margin = new Padding(4);
            _lblHint.Name = "_lblHint";
            _lblHint.Size = new Size(465, 56);
            _lblHint.TabIndex = 2;
            // 
            // _lblError
            // 
            _lblError.Appearance.ForeColor = Color.Firebrick;
            _lblError.Appearance.Options.UseForeColor = true;
            _lblError.Location = new Point(30, 145);
            _lblError.Margin = new Padding(4);
            _lblError.Name = "_lblError";
            _lblError.Size = new Size(0, 16);
            _lblError.TabIndex = 3;
            // 
            // _btnOk
            // 
            _btnOk.Appearance.Font = new Font("Tahoma", 10F);
            _btnOk.Appearance.Options.UseFont = true;
            _btnOk.ImageOptions.Image = (Image)resources.GetObject("_btnOk.ImageOptions.Image");
            _btnOk.Location = new Point(218, 145);
            _btnOk.Margin = new Padding(4);
            _btnOk.Name = "_btnOk";
            _btnOk.Size = new Size(138, 38);
            _btnOk.TabIndex = 4;
            _btnOk.Text = "Connexion";
            _btnOk.Click += BtnOk_Click;
            // 
            // _btnCancel
            // 
            _btnCancel.Appearance.Font = new Font("Tahoma", 10F);
            _btnCancel.Appearance.Options.UseFont = true;
            _btnCancel.DialogResult = DialogResult.Cancel;
            _btnCancel.ImageOptions.Image = (Image)resources.GetObject("_btnCancel.ImageOptions.Image");
            _btnCancel.Location = new Point(364, 145);
            _btnCancel.Margin = new Padding(4);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(138, 38);
            _btnCancel.TabIndex = 5;
            _btnCancel.Text = "Annuler";
            // 
            // LoginForm
            // 
            AcceptButton = _btnOk;
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = _btnCancel;
            ClientSize = new Size(509, 191);
            Controls.Add(lblTitle);
            Controls.Add(_txtPassword);
            Controls.Add(_lblHint);
            Controls.Add(_lblError);
            Controls.Add(_btnOk);
            Controls.Add(_btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            IconOptions.Image = (Image)resources.GetObject("LoginForm.IconOptions.Image");
            Margin = new Padding(4);
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

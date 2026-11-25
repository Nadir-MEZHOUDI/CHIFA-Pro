namespace CHIFA.Pro.Views;

public sealed partial class LoginForm : XtraForm
{
    public LoginForm()
    {
        InitializeComponent();
        _lblHint.Text = AccessGuard.IsDefaultPassword
            ? $"Mot de passe par défaut : {AccessGuard.DefaultPassword} (à changer dans « Sécurité »)"
            : string.Empty;
    }

    private void LoginForm_Shown(object? sender, EventArgs e) => _txtPassword.Focus();

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (AccessGuard.Login(_txtPassword.Text))
        {
            DialogResult = DialogResult.OK;
            return;
        }

        _lblError.Text = "Mot de passe incorrect.";
        _txtPassword.SelectAll();
        _txtPassword.Focus();
    }
}

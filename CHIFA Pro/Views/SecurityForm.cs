using CHIFA.Pro.Helpers.Settings;

namespace CHIFA.Pro.Views;

/// <summary>Administrator only: which screens and home figures are hidden while logged out, and the password.</summary>
public sealed partial class SecurityForm : XtraForm
{
    private readonly Dictionary<ProtectedItem, CheckEdit> _checks;

    public SecurityForm()
    {
        InitializeComponent();

        _checks = new()
        {
            [ProtectedItem.Statistics] = chkStatistics,
            [ProtectedItem.Bordereaux] = chkBordereaux,
            [ProtectedItem.Mobile] = chkMobile,
            [ProtectedItem.License] = chkLicense,
            [ProtectedItem.HomeFactures] = chkHomeFactures,
            [ProtectedItem.HomeMontant] = chkHomeMontant,
            [ProtectedItem.HomeMarge] = chkHomeMarge
        };
        foreach (var (item, check) in _checks)
            check.Checked = SecuritySettings.Default.IsHidden(item);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var newPassword = _txtNew.Text;
        if (newPassword.Length > 0)
        {
            if (newPassword.Length < 4)
            {
                _lblError.Text = "Le mot de passe doit contenir au moins 4 caractères.";
                return;
            }

            if (newPassword != _txtConfirm.Text)
            {
                _lblError.Text = "La confirmation ne correspond pas au nouveau mot de passe.";
                return;
            }

            AccessGuard.ChangePassword(newPassword);
        }

        foreach (var (item, check) in _checks)
            SecuritySettings.Default.SetHidden(item, check.Checked);
        AccessGuard.SaveSettings();

        DialogResult = DialogResult.OK;
    }
}

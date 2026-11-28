using CHIFA.Services.Helpers;

namespace CHIFA.Tests.Helpers;

public class PharmacyLicenseIdTests
{
    [Fact]
    public void Same_code_ps_gives_the_same_id_whatever_the_spaces_or_case()
    {
        var id = PharmacyLicenseId.FromCodePs("16a 0123");

        Assert.Equal(id, PharmacyLicenseId.FromCodePs(" 16A0123 "));
        Assert.StartsWith("site-", id);
        Assert.Equal(37, id!.Length);
        Assert.True(PharmacyLicenseId.IsPharmacy(id));
    }

    [Fact]
    public void Formula_never_changes_because_activated_pharmacies_are_bound_to_it()
    {
        Assert.Equal("site-6954855f84214123b16b70f4a25477b9", PharmacyLicenseId.FromCodePs("12345"));
    }

    [Fact]
    public void Other_code_ps_or_empty_code()
    {
        Assert.NotEqual(PharmacyLicenseId.FromCodePs("12345"), PharmacyLicenseId.FromCodePs("12346"));
        Assert.Null(PharmacyLicenseId.FromCodePs("  "));
        Assert.Null(PharmacyLicenseId.FromCodePs(null));
        Assert.False(PharmacyLicenseId.IsPharmacy("0f3a9c"));
    }
}

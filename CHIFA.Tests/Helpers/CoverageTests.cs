using CHIFA.Services.Helpers;

namespace CHIFA.Tests.Helpers;

public class CoverageTests
{
    [Theory]
    [InlineData('1', 80)]
    [InlineData('2', 100)]
    [InlineData('0', null)]
    [InlineData(null, null)]
    public void Facture_taux_code_is_a_percent(char? code, int? percent) =>
        Coverage.Percent(code).Should().Be(percent);

    [Theory]
    [InlineData('2', true)]
    [InlineData('3', true)]
    [InlineData('1', false)]
    [InlineData('0', false)]
    [InlineData(null, false)]
    public void Only_ordonnance_types_2_and_3_prove_a_chronic_patient(char? typeOrd, bool chronic) =>
        Coverage.IsChronicOrdonnance(typeOrd).Should().Be(chronic);

    [Fact]
    public void Rate_is_the_last_factures_and_a_change_is_flagged()
    {
        var c = Coverage.From(
        [
            new CoverageGroup('1', new DateTime(2026, 11, 20), false),
            new CoverageGroup('2', new DateTime(2025, 3, 1), true)
        ], attestationMc: false);

        c.Should().Be(new PatientCoverage(80, true, true));
        c.TauxLibelle.Should().Be("80 % (dernière facture)");
    }

    [Fact]
    public void A_single_rate_is_shown_plainly()
    {
        var c = Coverage.From([new CoverageGroup('2', new DateTime(2026, 1, 1), false)], attestationMc: false);

        c.Should().Be(new PatientCoverage(100, false, false));
        c.TauxLibelle.Should().Be("100 %");
    }

    [Fact]
    public void The_mc_attestation_alone_makes_a_chronic_patient()
    {
        Coverage.From([new CoverageGroup('1', DateTime.Today, false)], attestationMc: true).Mc.Should().BeTrue();
        Coverage.From([], attestationMc: true).Should().Be(new PatientCoverage(null, false, true));
    }

    [Fact]
    public void Unknown_rate_has_no_label() =>
        Coverage.Label(null, true).Should().BeNull();
}

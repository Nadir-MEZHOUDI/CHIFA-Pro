using CHIFA.Services.Dtos;

namespace CHIFA.Tests.Dtos;

public class RejetDtoTests
{
    private static RejetDto Sent(decimal partCaisse, decimal maj, decimal fae, decimal virement, int daysAgo = 10) => new()
    {
        NumBord = "B1",
        MontantPartCaisse = partCaisse,
        MontantMajorations = maj,
        MontantFae = fae,
        MontantVirement = virement,
        DateBord = DateTime.Today.AddDays(-daysAgo),
        EstOuvert = false
    };

    [Fact]
    public void AmountDue_IncludesMajorationsAndFae()
        => Sent(10_000m, 1_500m, 250m, 0m).MontantFacture.Should().Be(11_750m);

    [Fact]
    public void FullyPaid_IncludingMajAndFae_IsNotARejection()
    {
        // Before: compared with part caisse only -> MAJ/FAE hid real rejections or looked like overpayment.
        var r = Sent(10_000m, 1_500m, 250m, virement: 11_750m);
        r.MontantRejete.Should().Be(0m);
        r.TauxRejet.Should().Be(0m);
        r.StatutRejet.Should().Be("Sans rejet");
        r.MotifPrincipal.Should().BeNull();
    }

    [Fact]
    public void RejectionSmallerThanMajAndFae_IsNowDetected()
    {
        // Virement covers the part caisse but not all of MAJ + FAE: 1 000 DA missing.
        var r = Sent(10_000m, 1_500m, 250m, virement: 10_750m);
        r.MontantRejete.Should().Be(1_000m);
        r.StatutRejet.Should().Be("Rejet partiel");
        r.TauxRejet.Should().Be(Math.Round(1_000m / 11_750m * 100m, 2));
    }

    [Fact]
    public void NoVirementYet_IsWaiting_NotTotalRejection()
    {
        var r = Sent(10_000m, 1_500m, 250m, virement: 0m, daysAgo: 12);
        r.EstPaye.Should().BeFalse();
        r.MontantRejete.Should().Be(0m);
        r.TauxRejet.Should().Be(0m);
        r.MontantEnAttente.Should().Be(11_750m);
        r.StatutRejet.Should().Be("En attente de virement (12 j)");
    }

    [Fact]
    public void OpenBordereau_IsNeitherRejectedNorWaiting()
    {
        var r = Sent(5_000m, 0m, 0m, virement: 0m);
        r.EstOuvert = true;
        r.MontantRejete.Should().Be(0m);
        r.MontantEnAttente.Should().Be(0m);
        r.StatutRejet.Should().Be("Bordereau ouvert");
    }

    [Fact]
    public void RoundingDifference_BelowTolerance_IsIgnored()
        => Sent(10_000m, 0m, 0m, virement: 9_999.40m).MontantRejete.Should().Be(0m);

    [Fact]
    public void Overpayment_IsNotARejection()
        => Sent(10_000m, 0m, 0m, virement: 10_500m).StatutRejet.Should().Be("Sans rejet");
}

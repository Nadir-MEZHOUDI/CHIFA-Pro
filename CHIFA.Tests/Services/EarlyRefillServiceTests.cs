using CHIFA.Services.DataServices;
using CHIFA.Services.Helpers;
using Xunit.Abstractions;
using Sale = CHIFA.Services.DataServices.EarlyRefillService.RefillSale;

namespace CHIFA.Tests.Services;

public class EarlyRefillDetectTests
{
    private static readonly DateTime From = new(2024, 1, 1);

    private static Sale Line(string fact, string date, decimal duree, string dci = "DCI1", string assure = "A1", string rang = "00") => new()
    {
        NumFact = fact, NumAssure = assure, Rang = rang, Key = dci, Date = DateTime.Parse(date), Duree = duree, Qte = 1, Montant = 100
    };

    [Fact]
    public void RenewalBeforeEnd_IsFlaggedWithDaysEarly()
    {
        var rows = EarlyRefillService.Detect([Line("F1", "2024-03-01", 90), Line("F2", "2024-04-01", 90)], From, 5);

        var row = rows.Should().ContainSingle().Subject;
        row.NumFact.Should().Be("F2");
        row.NumFactPrecedente.Should().Be("F1");
        row.FinTraitement.Should().Be(new DateTime(2024, 5, 30));
        row.JoursAvance.Should().Be(59);
    }

    [Fact]
    public void RenewalWithinTolerance_OrAfterEnd_IsNotFlagged()
    {
        EarlyRefillService.Detect([Line("F1", "2024-03-01", 30), Line("F2", "2024-03-27", 30), Line("F3", "2024-05-01", 30)], From, 5)
            .Should().BeEmpty();
    }

    [Fact]
    public void OtherDci_OtherBeneficiary_AndLinesOfSameFacture_AreNotCompared()
    {
        var rows = EarlyRefillService.Detect(
        [
            Line("F1", "2024-03-01", 30), Line("F1", "2024-03-01", 30),
            Line("F2", "2024-03-05", 30, dci: "DCI2"),
            Line("F3", "2024-03-05", 30, rang: "01"),
            Line("F4", "2024-03-05", 30, assure: "A2")
        ], From, 5);

        rows.Should().BeEmpty();
    }

    [Fact]
    public void SalesBeforeThePeriod_AreOnlyUsedAsPrevious()
    {
        var rows = EarlyRefillService.Detect([Line("F1", "2023-11-01", 90), Line("F2", "2023-12-01", 90), Line("F3", "2024-01-10", 90)], From, 5);

        var row = rows.Should().ContainSingle().Subject;
        row.NumFact.Should().Be("F3");
        row.NumFactPrecedente.Should().Be("F2");
    }

    [Fact]
    public void ImplausibleDuration_IsIgnored()
    {
        EarlyRefillService.Detect([Line("F1", "2024-01-05", 810), Line("F2", "2024-03-01", 80)], From, 5)
            .Should().BeEmpty();
    }

    [Fact]
    public void SameDayDuplicate_IsFlaggedForTheWholeDuration()
    {
        var row = EarlyRefillService.Detect([Line("F1", "2024-02-01", 30), Line("F2", "2024-02-01", 30)], From, 5)
            .Should().ContainSingle().Subject;
        row.JoursAvance.Should().Be(30);
    }
}

[Trait("Category", "DbFunctional")]
public class EarlyRefillServiceTests(ITestOutputHelper output)
{
    [Fact]
    public async Task GetEarlyRefillsAsync_ReturnsNamedRows()
    {
        using var w = new TimeWatch(output: output);
        var rows = await EarlyRefillService.Instance.GetEarlyRefillsAsync(DateTime.Today.AddYears(-2));

        output.WriteLine($"{rows.Count} early refills");
        foreach (var r in rows.Take(5))
            output.WriteLine($"{r.NumFact} {r.DateVente:d} {r.Medicament} fin {r.FinTraitement:d} -{r.JoursAvance}j bord {r.Bordereau} ouvert={r.BordereauOuvert}");

        rows.Should().OnlyContain(r => r.JoursAvance > MedicalThresholds.EarlyRefillToleranceDays);
        rows.Should().OnlyContain(r => r.Medicament != null && r.Malade != null);
    }
}

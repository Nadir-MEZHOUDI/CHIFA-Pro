using CHIFA.Services.Dtos;

namespace CHIFA.Tests.Dtos;

public class FactureDtoTests
{
    [Fact]
    public void JourSemaine_WhenDateIsSet_ReturnsFrenchDayInUpperCase()
    {
        var dto = new FactureDto { DateFact = new DateTime(2026, 6, 10) };

        dto.JourSemaine.Should().Be("MERCREDI");
    }

    [Fact]
    public void JourSemaine_WhenDateIsNull_ReturnsEmpty()
    {
        var dto = new FactureDto { DateFact = null };

        dto.JourSemaine.Should().BeEmpty();
    }

    [Fact]
    public void Time_WhenDateIsSet_ReturnsShortTimeString()
    {
        var dto = new FactureDto { DateFact = new DateTime(2026, 6, 10, 14, 30, 0) };

        dto.Time.Should().Be("14:30");
    }

    [Fact]
    public void Time_WhenDateIsNull_ReturnsEmpty()
    {
        var dto = new FactureDto { DateFact = null };

        dto.Time.Should().BeEmpty();
    }

    [Fact]
    public void Equals_TwoDtosWithSameNumFact_AreEqual()
    {
        var a = new FactureDto { NumFact = "F001" };
        var b = new FactureDto { NumFact = "F001" };

        a.Equals(b).Should().BeTrue();
        a.GetHashCode().Should().Be(b.GetHashCode());
    }

    [Fact]
    public void Equals_TwoDtosWithDifferentNumFact_AreNotEqual()
    {
        var a = new FactureDto { NumFact = "F001" };
        var b = new FactureDto { NumFact = "F002" };

        a.Equals(b).Should().BeFalse();
    }
}

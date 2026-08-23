using CHIFA.Services.Helpers;
using CHIFA.Services.Dtos;

namespace CHIFA.Tests.Helpers;

public class MedicalThresholdsTests : IDisposable
{
    public void Dispose() { }

    [Fact]
    public void Constants_HaveExpectedValues()
    {
        using var w = new TimeWatch();
        MedicalThresholds.HighPriceThreshold.Should().Be(1000m);
        MedicalThresholds.HighQuantityThreshold.Should().Be(3);
        MedicalThresholds.MediumTreatmentDurationDays.Should().Be(30);
        MedicalThresholds.LongTreatmentDurationDays.Should().Be(60);
        MedicalThresholds.VeryLongTreatmentDurationDays.Should().Be(80);
    }
}

public class PeriodTests : IDisposable
{
    public void Dispose() { }

    [Fact]
    public void Default_From_IsTwoYearsAgo()
    {
        using var w = new TimeWatch();
        var p = new Period();
        var expected = DateTime.Today.AddYears(-2);
        p.From.Should().BeCloseTo(expected, TimeSpan.FromDays(1));
    }

    [Fact]
    public void Default_To_IsToday()
    {
        using var w = new TimeWatch();
        var p = new Period();
        p.To.Should().Be(DateTime.Today);
    }

    [Fact]
    public void CanSetCustomRange()
    {
        using var w = new TimeWatch();
        var p = new Period { From = new DateTime(2020, 1, 1), To = new DateTime(2020, 12, 31) };
        p.From.Should().Be(new DateTime(2020, 1, 1));
        p.To.Should().Be(new DateTime(2020, 12, 31));
    }
}

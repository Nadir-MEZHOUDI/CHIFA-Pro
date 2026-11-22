using System.Linq.Expressions;
using CHIFA.Services.DataServices;
using CHIFA.Services.Dtos;
using DataModel;

namespace CHIFA.Tests.Helpers;

/// <summary>
/// The period filter must include both bounds as whole days: a facture at 23:59 on the "To" day,
/// or at 00:00 on the "From" day, is inside the period.
/// </summary>
public class SetPeriodTests
{
    private static readonly Period August = new() { From = new DateTime(2026, 8, 1), To = new DateTime(2026, 8, 31) };

    private static bool Matches(Period period, DateTime? dateFact)
        => ((Expression<Func<Facture, bool>>?)null).SetPeriod(period).Compile()(new Facture { DateFact = dateFact });

    [Fact]
    public void Period_Bounds_AreWholeDays()
    {
        var p = new Period { From = new DateTime(2026, 8, 1, 15, 30, 0), To = new DateTime(2026, 8, 31, 9, 0, 0) };
        p.Start.Should().Be(new DateTime(2026, 8, 1));
        p.EndExclusive.Should().Be(new DateTime(2026, 9, 1));
    }

    [Theory]
    [InlineData("2026-08-01 00:00:00", true)]  // first instant of the From day
    [InlineData("2026-08-31 23:59:59", true)]  // last facture of the To day (was excluded before)
    [InlineData("2026-08-31 00:00:00", true)]
    [InlineData("2026-07-31 23:59:59", false)]
    [InlineData("2026-09-01 00:00:00", false)]
    public void Facture_BothBoundsInclusive(string date, bool expected)
        => Matches(August, DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture)).Should().Be(expected);

    [Fact]
    public void Facture_ToEqualToLastFactureTime_IncludesThatFacture()
    {
        // The "Au" editor used to be filled with the exact time of the last facture, which excluded it.
        var last = new DateTime(2026, 11, 22, 18, 42, 7);
        Matches(new Period { From = new DateTime(2026, 11, 1), To = last }, last).Should().BeTrue();
    }

    [Fact]
    public void Facture_OpenEndedPeriod_IncludesFutureAndToday()
    {
        var p = new Period { From = new DateTime(2026, 1, 1), To = null };
        Matches(p, DateTime.Now).Should().BeTrue();
        Matches(p, DateTime.Today.AddDays(30)).Should().BeTrue();
    }

    [Fact]
    public void Facture_WithoutDate_IsExcluded()
        => Matches(August, null).Should().BeFalse();

    [Theory]
    [InlineData("2026-08-31 23:59:59", true)]
    [InlineData("2026-09-01 00:00:00", false)]
    public void DetailFact_UsesFactureDate_Inclusive(string date, bool expected)
    {
        var detail = new DetailFact { Facture = new Facture { DateFact = DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture) } };
        ((Expression<Func<DetailFact, bool>>?)null).SetPeriod(August).Compile()(detail).Should().Be(expected);
    }

    [Theory]
    [InlineData("2026-08-31 16:00:00", true)]
    [InlineData("2026-09-01 00:00:00", false)]
    public void Bordereau_UsesExtractDate_Inclusive(string date, bool expected)
    {
        var bord = new Bordereau { DateExtract = DateTime.Parse(date, System.Globalization.CultureInfo.InvariantCulture) };
        ((Expression<Func<Bordereau, bool>>?)null).SetPeriod(August).Compile()(bord).Should().Be(expected);
    }
}

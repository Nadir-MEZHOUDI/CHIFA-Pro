using CHIFA.Contract.Helpers;

namespace CHIFA.Tests.Helpers;

public class ToDecimalExtensionsTests
{
    [Theory]
    [InlineData(null, 0)]
    [InlineData(0.0, 0)]
    [InlineData(1.234, 1.23)]
    [InlineData(-1.234, -1.23)]
    public void Decimal_Nullable_RoundsToTwoDecimals(double? input, double expected)
    {
        decimal? value = input.HasValue ? (decimal)input.Value : null;

        value.ToDecimal().Should().Be((decimal)expected);
    }

    [Fact]
    public void Decimal_Nullable_NullInputReturnsZero()
    {
        ((decimal?)null).ToDecimal().Should().Be(0m);
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData(0, 0)]
    [InlineData(5, 5)]
    [InlineData(100, 100)]
    public void Int_Nullable_ReturnsDecimalCast(int? input, int expected)
    {
        input.ToDecimal().Should().Be((decimal)expected);
    }

    [Fact]
    public void Double_Nullable_RoundsToTwoDecimals()
    {
        double? value = 1.236;

        value.ToDecimal().Should().Be(1.24m);
    }

    [Fact]
    public void Decimal_NonNullable_RoundsToTwoDecimals()
    {
        1.2345m.ToDecimal().Should().Be(1.23m);
    }

    [Fact]
    public void Int_NonNullable_ReturnsDecimalCast()
    {
        42.ToDecimal().Should().Be(42m);
    }
}

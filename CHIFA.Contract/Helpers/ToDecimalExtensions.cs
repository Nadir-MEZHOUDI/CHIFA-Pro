namespace CHIFA.Contract.Helpers;

public static class ToDecimalExtensions
{
    public static decimal ToDecimal(this decimal? value) => Math.Round(value ?? 0, 2);
    public static decimal ToDecimal(this int? value) => Math.Round((decimal)(value ?? 0), 2);
    public static decimal ToDecimal(this double? value) => Math.Round((decimal)(value ?? 0), 2);

    public static decimal ToDecimal(this decimal value) => Math.Round(value, 2);
    public static decimal ToDecimal(this int value) => Math.Round((decimal)value, 2);
    public static decimal ToDecimal(this double value) => Math.Round((decimal)value, 2);
}
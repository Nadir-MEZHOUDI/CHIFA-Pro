using System.Globalization;

namespace CHIFA.Mobile;

/// <summary>French number and date formats without loading ICU data (the app runs with invariant globalization).</summary>
public static class Fmt
{
    private static readonly NumberFormatInfo French = new()
    {
        NumberGroupSeparator = " ",
        NumberDecimalSeparator = ",",
        PercentGroupSeparator = " ",
        PercentDecimalSeparator = ","
    };

    public static string Money(decimal? value) => value is { } v ? $"{v.ToString("N2", French)} DA" : "—";

    public static string Number(decimal? value) => value is { } v ? v.ToString("N0", French) : "—";

    public static string Percent(decimal value) => $"{value.ToString("N1", French)} %";

    public static string Date(DateTime? value) => value?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) ?? "—";

    public static string Stamp(DateTime? value) => value?.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) ?? "—";

    public static string Ago(DateTime? utc)
    {
        if (utc is not { } value)
            return "jamais";

        var elapsed = System.DateTime.UtcNow - value.ToUniversalTime();
        return elapsed.TotalMinutes switch
        {
            < 1 => "à l'instant",
            < 60 => $"il y a {(int)elapsed.TotalMinutes} min",
            < 1440 => $"il y a {(int)elapsed.TotalHours} h",
            _ => $"le {Date(value.ToLocalTime())}"
        };
    }
}

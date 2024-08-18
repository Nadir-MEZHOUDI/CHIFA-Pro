using System.Text.RegularExpressions;

namespace CHIFA.DAL.Helpers;
public static partial class StringHelpers
{
    public static bool IsNullOrEmpty(this string? str) => string.IsNullOrEmpty(str);
    public static string TrimAll(this string? str) => str is null ? string.Empty : MyRegex().Replace(str, " ");
    public static string TrimMe(this string? str) => str is null ? string.Empty : str.Trim();

    [GeneratedRegex("\\s+")]
    private static partial Regex MyRegex();
}


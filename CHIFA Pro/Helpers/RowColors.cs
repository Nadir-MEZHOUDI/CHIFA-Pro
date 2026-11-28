namespace CHIFA.Pro.Helpers;

/// <summary>Grid row colours of the facture and treatment screens.</summary>
public static class RowColors
{
    public static readonly Color Green = Color.LawnGreen;
    public static readonly Color Red = Color.FromArgb(251, 157, 140);
    public static readonly Color Tan = Color.Khaki;

    /// <summary>
    /// Red when the next delivery date is past, then green for a traitement spécifique, then tan for a long treatment;
    /// <c>null</c> keeps the grid's own colour.
    /// </summary>
    public static Color? For(bool? ts, bool? longDuree, bool overdue = false) => (overdue, ts == true, longDuree == true) switch
    {
        (true, _, _) => Red,
        (_, true, _) => Green,
        (_, _, true) => Tan,
        _ => null
    };
}

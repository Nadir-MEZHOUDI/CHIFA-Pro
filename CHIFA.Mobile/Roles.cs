namespace CHIFA.Mobile;

/// <summary>What a linked phone may see; the gateway enforces it, the app only hides what would be refused.</summary>
public static class Roles
{
    public const string Owner = "owner";
    public const string Staff = "staff";

    public static string Label(string? role) => role == Staff ? "Employé" : "Titulaire";
}

namespace CHIFA.Mobile.Layout;

/// <summary>
/// Every page of the menu. <see cref="InTabBar"/> ones are also in the phone's bottom bar. <see cref="OwnerOnly"/> ones
/// are hidden on an employee's phone (the gateway refuses them anyway).
/// </summary>
public sealed record NavItem(string Href, string Label, string Icon, bool InTabBar = false, string? Short = null, bool OwnerOnly = false)
{
    public static readonly IReadOnlyList<NavItem> All =
    [
        new("", "Accueil", "M3 12l9-9 9 9M5 10v10h5v-6h4v6h5V10", InTabBar: true),
        new("statistiques", "Statistiques", "M3 3v18h18M7 16v-4M12 16V8M17 16V5", OwnerOnly: true),
        new("factures", "Factures", "M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8zM14 2v6h6M8 13h8M8 17h8", InTabBar: true),
        new("patients", "Patients", "M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8zM22 21v-2a4 4 0 0 0-3-3.87M16 3.13a4 4 0 0 1 0 7.75", InTabBar: true),
        new("relances", "Chroniques à relancer", "M8 2v4M16 2v4M3 10h18M5 4h14a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V6a2 2 0 0 1 2-2zM12 13v3l2 1.5"),
        new("medicaments", "Médicaments", "M10.5 20.5 3.5 13.5a5 5 0 0 1 7-7l7 7a5 5 0 0 1-7 7zM8.5 8.5l7 7", InTabBar: true, Short: "Médic."),
        new("bordereaux", "Bordereaux", "M3 7h18M3 12h18M3 17h18", OwnerOnly: true),
        new("rejets", "Rejets et paiements", "M12 9v4M12 17h.01M10.3 3.9 1.8 18a2 2 0 0 0 1.7 3h17a2 2 0 0 0 1.7-3L13.7 3.9a2 2 0 0 0-3.4 0z", InTabBar: true, Short: "Rejets", OwnerOnly: true),
        new("psychotropes", "Psychotropes", "M9 3h6M10 3v6l-5 9a2 2 0 0 0 1.8 3h10.4a2 2 0 0 0 1.8-3l-5-9V3M7 15h10"),
        new("notifications", "Notifications", "M18 8a6 6 0 0 0-12 0c0 7-3 9-3 9h18s-3-2-3-9M13.7 21a2 2 0 0 1-3.4 0", OwnerOnly: true),
        new("abonnement", "Abonnement et appareils", "M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10zM9 12l2 2 4-4", OwnerOnly: true),
        new("assistance", "Assistance et contact", "M3 18v-6a9 9 0 0 1 18 0v6M21 19a2 2 0 0 1-2 2h-1v-6h3zM3 19a2 2 0 0 0 2 2h1v-6H3z")
    ];

    /// <summary>Pages reached from another page rather than the menu, refused to an employee's phone too.</summary>
    public static readonly string[] OwnerOnlyPages = ["avis"];

    public static bool IsOwnerOnly(string page) =>
        OwnerOnlyPages.Contains(page) || All.Any(i => i.OwnerOnly && i.Href == page);

    /// <summary>What this phone may open.</summary>
    public static IEnumerable<NavItem> For(bool isStaff) => isStaff ? All.Where(i => !i.OwnerOnly) : All;
}

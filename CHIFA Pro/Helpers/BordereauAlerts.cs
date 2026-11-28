using CHIFA.Pro.Helpers.Settings;
using LinqToDB.Async;

namespace CHIFA.Pro.Helpers;

/// <summary>Open bordereaux past the limits set in « Paramètres » (days, amount, number of factures).</summary>
internal static class BordereauAlerts
{
    public static async Task<List<string>> GetMessagesAsync()
    {
        // Undisposed, the connection never went back to the pool: one leaked server connection per refresh.
        await using var db = new ChifaDb();
        var bords = await db.Bordereaus
            .Where(x => x.Etat == 'O')
            .Select(x => new BordNotification
            {
                NumBord = x.NumBord,
                Center = x.Center!.Nom,
                NmbrFact = x.Factures.Count(),
                Min = x.Factures.Min(f => f.DateFact),
                Max = x.Factures.Max(f => f.DateFact),
                Montant = x.Factures.Sum(f => f.MontOff + f.MontMaj + f.MontMajFae)
            })
            .ToListAsync();

        var messages = new List<string>();
        foreach (var bord in bords)
        {
            var message = "";
            if (AppSettings.Default.NotificationOnDays && AppSettings.Default.MaxDays > 0 && bord.Days > AppSettings.Default.MaxDays)
            {
                message += $"\n a {bord.Days:N0} Jours";
            }

            if (AppSettings.Default.NotificationOnMontant && AppSettings.Default.MaxMontant > 0 && bord.Montant > AppSettings.Default.MaxMontant)
            {
                message += $"\n a {bord.Montant:N2} DA";
            }

            if (AppSettings.Default.NotificationOnNmbr && AppSettings.Default.MaxNmbr > 0 && bord.NmbrFact > AppSettings.Default.MaxNmbr)
            {
                message += $"\n a {bord.NmbrFact:N0} Factures";
            }

            if (!string.IsNullOrWhiteSpace(message))
                messages.Add($"Le Bord {bord.NumBord} de {bord.Center} " + message);
        }

        return messages;
    }
}

public record BordNotification
{
    public int Days => (Max - Min)?.Days ?? 0;
    public string? NumBord { get; init; }
    public string? Center { get; init; }
    public int NmbrFact { get; init; }
    public DateTime? Min { get; init; }
    public DateTime? Max { get; init; }
    public decimal? Montant { get; init; }
}

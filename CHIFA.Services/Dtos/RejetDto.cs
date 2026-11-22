namespace CHIFA.Services.Dtos;

/// <summary>
/// Reconciliation of one bordereau: what the caisse owes vs the virement received.
/// The amount due is the same as on the Bordereaux page: part caisse + majorations + FAE.
/// A bordereau without virement is "waiting for payment", not "rejected": CHIFA has no way
/// to tell a total rejection from a payment that has simply not arrived yet.
/// </summary>
public class RejetDto
{
    /// <summary>Differences below this amount (DA) are rounding, not rejections.</summary>
    public const decimal ToleranceArrondi = 1m;

    public string? NumBord { get; set; }
    public string? Centre { get; set; }

    /// <summary>Closing date of the bordereau (FTP deposit date when it has none).</summary>
    public DateTime? DateBord { get; set; }

    public DateTime? DateDepotFtp { get; set; }
    public int NombreFactures { get; set; }

    public decimal MontantPartCaisse { get; set; }
    public decimal MontantMajorations { get; set; }
    public decimal MontantFae { get; set; }

    /// <summary>Total due by the caisse: part caisse + majorations + FAE.</summary>
    public decimal MontantFacture => MontantPartCaisse + MontantMajorations + MontantFae;

    public decimal MontantVirement { get; set; }

    /// <summary>Bordereau still open in CHIFA (not sent yet).</summary>
    public bool EstOuvert { get; set; }

    /// <summary>A virement has been received for this bordereau.</summary>
    public bool EstPaye => !EstOuvert && MontantVirement > 0;

    public decimal MontantRejete
    {
        get
        {
            if (!EstPaye) return 0m;
            var ecart = MontantFacture - MontantVirement;
            return ecart >= ToleranceArrondi ? ecart : 0m;
        }
    }

    /// <summary>Amount still expected (sent bordereau without virement yet).</summary>
    public decimal MontantEnAttente => !EstOuvert && !EstPaye ? MontantFacture : 0m;

    public decimal TauxRejet => EstPaye && MontantFacture > 0 ? Math.Round(MontantRejete / MontantFacture * 100m, 2) : 0m;

    /// <summary>Days since the bordereau was closed/sent, for unpaid follow-up.</summary>
    public int? JoursAttente => DateBord.HasValue ? (int)(DateTime.Today - DateBord.Value.Date).TotalDays : null;

    public string StatutRejet => (EstOuvert, EstPaye, MontantRejete > 0) switch
    {
        (true, _, _) => "Bordereau ouvert",
        (_, false, _) => JoursAttente is { } jours ? $"En attente de virement ({jours} j)" : "En attente de virement",
        (_, true, true) => "Rejet partiel",
        _ => "Sans rejet"
    };

    public string? MotifPrincipal => MontantRejete > 0 ? "Écart de virement bordereau" : null;

    public string EtatBordereau { get; set; } = string.Empty;
}

public class RejetDetailDto
{
    public string? NumFact { get; set; }
    public string? NumAssure { get; set; }
    public string? NomAssure { get; set; }
    public DateTime? DateFact { get; set; }
    public decimal MontantFact { get; set; }
    public decimal MontantCaisse { get; set; }
    public string? MotifRejet { get; set; }
    public string Statut { get; set; } = "En Cours";
}

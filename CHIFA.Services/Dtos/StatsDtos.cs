namespace CHIFA.Services.Dtos;

/// <summary>One day of the home chart. <see cref="Marge"/> is the same estimate as the desktop home page.</summary>
public record DayStatDto(DateTime Date, int Factures, decimal Montant, decimal Maj, decimal Marge);

/// <summary>
/// One row of a statistics tab, whatever its kind (a day, a month, a bordereau, a medicine, a patient).
/// Amounts follow the desktop statistics page: <see cref="Marge"/> is estimated, there is no purchase price.
/// </summary>
public record StatRowDto
{
    public required string Label { get; init; }
    public string? Short { get; init; }
    public string? Detail { get; init; }
    public DateTime? Date { get; init; }

    /// <summary>N° d'enregistrement for a medicine, N° d'assuré for a patient.</summary>
    public string? Key { get; init; }
    public string? Rang { get; init; }

    public int Factures { get; init; }
    public decimal Quantite { get; init; }
    public decimal Montant { get; init; }
    public decimal Caisse { get; init; }
    public decimal Maj { get; init; }
    public decimal Marge { get; init; }
    public decimal Brut => Marge + Maj;

    public decimal? Virement { get; init; }
    public decimal? Ecart { get; init; }

    /// <summary>Bordereaux only: false when CHIFA never receives the caisse's virements (CASNOS).</summary>
    public bool PaiementSuivi { get; init; } = true;
}

/// <param name="Count">Rows over the whole period; <see cref="Rows"/> may hold only the first ones.</param>
/// <param name="Totals">Sums over all rows, not only the returned ones.</param>
public record StatsDto(string Kind, int Count, List<StatRowDto> Rows, StatRowDto Totals);

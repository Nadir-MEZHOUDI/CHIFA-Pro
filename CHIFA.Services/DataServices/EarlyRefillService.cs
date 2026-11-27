using LinqToDB.Async;

namespace CHIFA.Services.DataServices;

/// <summary>
/// Finds medicines sold again to the same beneficiary while the previous treatment of the same DCI was still running:
/// the factures CNAS / CASNOS are likely to reject.
/// </summary>
public class EarlyRefillService
{
    public static EarlyRefillService Instance { get; } = new();

    /// <summary>Early renewals among the sales made since <paramref name="from"/>, the most days early first.</summary>
    public async ValueTask<List<EarlyRefillDto>> GetEarlyRefillsAsync(DateTime from, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var lookback = from.Date.AddDays(-MedicalThresholds.MaxTreatmentDurationDays);

        var sales = await db.DetailFacts
            .Where(d => d.Facture.DateFact >= lookback && d.DureeTrait > 0 && d.DureeTrait <= MedicalThresholds.MaxTreatmentDurationDays
                        && d.Facture.NumAssure != null)
            .Select(d => new RefillSale
            {
                NumFact = d.Facture.NumFact,
                NumAssure = d.Facture.NumAssure,
                Rang = d.Facture.RangAd,
                Key = d.Medicament.CodeDci ?? d.NumEnr,
                Date = d.Facture.DateFact,
                Duree = d.DureeTrait,
                Qte = d.Qte,
                Montant = d.MontPharm
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var flagged = Detect(sales, from, MedicalThresholds.EarlyRefillToleranceDays);
        if (flagged.Count == 0) return [];

        var factIds = flagged.Select(f => f.NumFact).Distinct().ToList();

        var factures = await db.Factures
            .Where(f => factIds.Contains(f.NumFact))
            .Select(f => new
            {
                f.NumFact,
                Assure = f.Assure.FullName,
                Malade = f.Beneficiaire.FullName,
                Specialite = f.Specialite!.Libelle,
                f.NumBord,
                Ouvert = f.NumBord == null || f.Bordereau!.Etat == 'O'
            })
            .ToDictionaryAsync(f => f.NumFact, cancellationToken)
            .ConfigureAwait(false);

        var medicaments = await db.DetailFacts
            .Where(d => factIds.Contains(d.Facture.NumFact))
            .Select(d => new { d.Facture.NumFact, Key = d.Medicament.CodeDci ?? d.NumEnr, Nom = d.Medicament.FullName() })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var names = medicaments
            .GroupBy(m => (m.NumFact, m.Key))
            .ToDictionary(g => g.Key, g => string.Join(" + ", g.Select(m => m.Nom.Trim()).Distinct()));

        foreach (var row in flagged)
        {
            if (row.NumFact != null && factures.TryGetValue(row.NumFact, out var f))
            {
                row.Assure = f.Assure;
                row.Malade = f.Malade;
                row.Specialite = f.Specialite;
                row.Bordereau = f.NumBord;
                row.BordereauOuvert = f.Ouvert;
            }

            row.Medicament = names.GetValueOrDefault((row.NumFact!, row.CodeDci!));
        }

        return flagged;
    }

    /// <summary>
    /// Per beneficiary and DCI, compares each sale with the end of the treatments already sold
    /// (sale date + duration). Lines of one facture count as one sale.
    /// </summary>
    internal static List<EarlyRefillDto> Detect(IEnumerable<RefillSale> lines, DateTime from, int toleranceDays)
    {
        var result = new List<EarlyRefillDto>();

        var groups = lines
            .Where(l => l.NumFact != null && l.NumAssure != null && l.Key != null && l.Date.HasValue
                        && l.Duree is > 0 and <= MedicalThresholds.MaxTreatmentDurationDays)
            .GroupBy(l => (l.NumAssure, l.Rang, l.Key));

        foreach (var group in groups)
        {
            var sales = group
                .GroupBy(l => l.NumFact)
                .Select(g => new
                {
                    NumFact = g.Key,
                    Date = g.Min(l => l.Date!.Value).Date,
                    Duree = g.Max(l => l.Duree ?? 0m),
                    Qte = g.Sum(l => l.Qte ?? 0m),
                    Montant = g.Sum(l => l.Montant ?? 0m)
                })
                .OrderBy(s => s.Date)
                .ThenBy(s => s.NumFact, StringComparer.Ordinal)
                .ToList();

            var covering = sales[0];
            var coveredUntil = covering.Date.AddDays((double)covering.Duree);

            foreach (var sale in sales.Skip(1))
            {
                var daysEarly = (coveredUntil - sale.Date).Days;
                if (sale.Date >= from.Date && daysEarly > toleranceDays)
                    result.Add(new EarlyRefillDto
                    {
                        NumAssure = group.Key.NumAssure,
                        Rang = group.Key.Rang,
                        CodeDci = group.Key.Key,
                        NumFact = sale.NumFact,
                        DateVente = sale.Date,
                        Qte = sale.Qte,
                        Duree = sale.Duree,
                        Montant = sale.Montant,
                        NumFactPrecedente = covering.NumFact,
                        DateVentePrecedente = covering.Date,
                        DureePrecedente = covering.Duree,
                        FinTraitement = coveredUntil,
                        JoursAvance = daysEarly
                    });

                var end = sale.Date.AddDays((double)sale.Duree);
                if (end > coveredUntil)
                {
                    covering = sale;
                    coveredUntil = end;
                }
            }
        }

        return result
            .OrderByDescending(r => r.JoursAvance)
            .ThenByDescending(r => r.DateVente)
            .ToList();
    }

    internal class RefillSale
    {
        public string? NumFact { get; init; }
        public string? NumAssure { get; init; }
        public string? Rang { get; init; }
        public string? Key { get; init; }
        public DateTime? Date { get; init; }
        public decimal? Duree { get; init; }
        public decimal? Qte { get; init; }
        public decimal? Montant { get; init; }
    }
}

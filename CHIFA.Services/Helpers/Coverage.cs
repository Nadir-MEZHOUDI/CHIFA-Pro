using LinqToDB.Async;
using LinqToDB.Mapping;

namespace CHIFA.Services.Helpers;

/// <summary>A patient's CNAS/CASNOS coverage: the rate of their last facture and whether they are a chronic patient.</summary>
public sealed record PatientCoverage(int? Taux, bool TauxVarie, bool Mc)
{
    public string? TauxLibelle => Coverage.Label(Taux, TauxVarie);
}

/// <summary>The patient's factures at one rate.</summary>
public readonly record struct CoverageGroup(char? Taux, DateTime? Derniere, bool Mc);

public static class Coverage
{
    private static readonly Func<char?, int?> PercentCompiled = PercentExpression().Compile();
    private static readonly Func<char?, bool> ChronicCompiled = IsChronicOrdonnanceExpression().Compile();

    /// <summary><c>facture.taux</c>: '1' is 80 %, '2' is 100 %, as CHIFA_OFFICINE displays it.</summary>
    [ExpressionMethod(nameof(PercentExpression))]
    public static int? Percent(char? taux) => PercentCompiled(taux);

    private static Expression<Func<char?, int?>> PercentExpression() =>
        taux => taux == '2' ? 100 : taux == '1' ? 80 : null;

    /// <summary>
    /// <c>facture.type_ord</c>: CHIFA_OFFICINE writes 2 or 3 only when the card carries a chronic-disease (ALD) list.
    /// 1 is written for both chronic and other patients, so it proves nothing.
    /// </summary>
    [ExpressionMethod(nameof(IsChronicOrdonnanceExpression))]
    public static bool IsChronicOrdonnance(char? typeOrd) => ChronicCompiled(typeOrd);

    private static Expression<Func<char?, bool>> IsChronicOrdonnanceExpression() =>
        typeOrd => typeOrd == '2' || typeOrd == '3';

    public static string? Label(int? taux, bool? varie) =>
        taux is null ? null : varie == true ? $"{taux} % (dernière facture)" : $"{taux} %";

    /// <summary>
    /// The rate is the one of the most recent facture; <see cref="PatientCoverage.TauxVarie"/> tells whether older
    /// ones differ. The patient is chronic when CNAS's MC attestation says so or a facture was a chronic ordonnance.
    /// </summary>
    public static PatientCoverage From(IEnumerable<CoverageGroup> groups, bool attestationMc)
    {
        var list = groups.ToList();
        var last = list.OrderByDescending(g => g.Derniere ?? DateTime.MinValue).FirstOrDefault();
        var rates = list.Select(g => Percent(g.Taux)).Where(t => t is not null).Distinct().Count();
        return new PatientCoverage(
            list.Count == 0 ? null : Percent(last.Taux),
            rates > 1,
            attestationMc || list.Any(g => g.Mc));
    }

    /// <summary>Coverage of the patients holding these insured numbers, or of everyone when null.</summary>
    public static async Task<Dictionary<(string NumAssure, string Rang), PatientCoverage>> LoadAsync(
        ChifaDb db, IReadOnlyCollection<string>? numAssures, CancellationToken cancellationToken = default)
    {
        var factures = db.Factures.Where(f => f.NumAssure != null && f.RangAd != null);
        var attestations = db.AttestationMcs.Where(a => a.Mc == true);
        if (numAssures is not null)
        {
            if (numAssures.Count == 0)
                return [];
            factures = factures.Where(f => numAssures.Contains(f.NumAssure));
            attestations = attestations.Where(a => numAssures.Contains(a.NumAssure));
        }

        var groups = await (
                from f in factures
                group f by new { f.NumAssure, f.RangAd, f.Taux } into g
                select new
                {
                    g.Key.NumAssure,
                    g.Key.RangAd,
                    g.Key.Taux,
                    Derniere = g.Max(f => f.DateFact),
                    Mc = g.Max(f => IsChronicOrdonnance(f.TypeOrd) ? 1 : 0)
                })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var attested = (await attestations
                .Select(a => new { a.NumAssure, a.RangAd })
                .Distinct()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false))
            .Select(a => (a.NumAssure, a.RangAd))
            .ToHashSet();

        var result = groups
            .GroupBy(g => (g.NumAssure!, g.RangAd!))
            .ToDictionary(
                p => p.Key,
                p => From(p.Select(g => new CoverageGroup(g.Taux, g.Derniere, g.Mc == 1)), attested.Contains(p.Key)));
        foreach (var key in attested)
            result.TryAdd(key, new PatientCoverage(null, false, true));
        return result;
    }
}

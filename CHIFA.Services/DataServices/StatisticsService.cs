using LinqToDB.Async;

namespace CHIFA.Services.DataServices;

public class StatisticsService
{
    private static StatisticsService? _instance;
    public static StatisticsService Instance => _instance ??= new StatisticsService();

    public readonly Period Period = new();

    public async ValueTask<ScopeDashboardDto> GetScopeDashboardAsync(Period? period = null, CancellationToken cancellationToken = default)
    {
        var activePeriod = period ?? Period;
        await using var db = new ChifaDb();

        Expression<Func<Facture, bool>> predicate = f => f.DateFact != null;
        predicate = predicate.SetPeriod(activePeriod);

        var facturesQuery = db.Factures.Where(predicate);

        var kpis = await facturesQuery
            .GroupBy(_ => 1)
            .Select(g => new
            {
                TotalFact = g.Sum(f => f.MontFact) ?? 0m,
                TotalCaisse = g.Sum(f => f.MontOff) ?? 0m,
                TotalAssure = g.Sum(f => f.MontAs) ?? 0m,
                TotalMaj = g.Sum(f => f.MontMaj) ?? 0m,
                CountFact = g.Count(),
                DistinctAssures = g.CountExt(f => f.NumAssure, Sql.AggregateModifier.Distinct)
            })
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);

        var totalBoxes = await db.DetailFacts
            .Where(d => d.Facture.DateFact != null)
            .Where(d => (activePeriod.Start == null || d.Facture.DateFact >= activePeriod.Start) &&
                        (activePeriod.EndExclusive == null || d.Facture.DateFact < activePeriod.EndExclusive))
            .SumAsync(d => (decimal?)d.Qte, cancellationToken)
            .ConfigureAwait(false) ?? 0m;

        var hourlyRaw = await facturesQuery
            .GroupBy(f => f.DateFact!.Value.Hour)
            .Select(g => new
            {
                Hour = g.Key,
                Count = g.Count(),
                Total = g.Sum(f => f.MontFact) ?? 0m
            })
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var hourlyList = Enumerable.Range(8, 14)
            .Select(h =>
            {
                var match = hourlyRaw.FirstOrDefault(x => x.Hour == h);
                return new HourlyActivityDto
                {
                    Heure = h,
                    NombreFactures = match?.Count ?? 0,
                    MontantTotal = match?.Total ?? 0m
                };
            })
            .ToList();

        var centersRaw = await facturesQuery
            .GroupBy(f => f.Center!.Nom)
            .Select(g => new CenterSummaryDto
            {
                Centre = g.Key ?? "Inconnu",
                NombreFactures = g.Count(),
                MontantTotal = g.Sum(f => f.MontFact) ?? 0m,
                MontantCaisse = g.Sum(f => f.MontOff) ?? 0m,
                MontantMaj = g.Sum(f => f.MontMaj) ?? 0m
            })
            .OrderByDescending(c => c.MontantTotal)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var topProducts = await db.DetailFacts
            .Where(d => d.Facture.DateFact != null)
            .Where(d => (activePeriod.Start == null || d.Facture.DateFact >= activePeriod.Start) &&
                        (activePeriod.EndExclusive == null || d.Facture.DateFact < activePeriod.EndExclusive))
            .GroupBy(d => new { d.NumEnr, Nom = d.Medicament.NomCom, Dci = d.Medicament.NomDci })
            .Select(g => new TopProductDto
            {
                CodeEnr = g.Key.NumEnr ?? "",
                Designation = g.Key.Nom ?? "Non défini",
                Dci = g.Key.Dci ?? "",
                QuantiteTotale = g.Sum(d => d.Qte),
                MontantTotal = g.Sum(d => d.MontPharm) ?? 0m
            })
            .OrderByDescending(p => p.QuantiteTotale)
            .Take(10)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new ScopeDashboardDto
        {
            MontantTotalFact = kpis?.TotalFact ?? 0m,
            MontantTotalCaisse = kpis?.TotalCaisse ?? 0m,
            MontantTotalAssure = kpis?.TotalAssure ?? 0m,
            MontantTotalMajoration = kpis?.TotalMaj ?? 0m,
            NombreFactures = kpis?.CountFact ?? 0,
            NombreAssuresUniques = kpis?.DistinctAssures ?? 0,
            NombreBoites = totalBoxes,
            ActiviteHoraire = hourlyList,
            RepartitionParCaisse = centersRaw,
            TopMedicaments = topProducts
        };
    }

    public async ValueTask<List<ThisWeekStat>> GetThisWeekStatsAsync()
    {
        await using var db = new ChifaDb();
        var firstDate = DateTime.Today.AddDays(-8);
        var weekStats = await db.Factures
                .Where(f => f.DateFact >= firstDate)
                .GroupBy(f => f.DateFact!.Value.Date)
                .Select(g => new ThisWeekStat
                {
                    Date = g.Key,
                    Count = g.Count(),

                    Montant = g.Sum(f => f.MontFact),
                    Maj = g.Sum(f => f.MontMaj)
                })
                .ToListAsync()
                .ConfigureAwait(false)
            ;

        return Enumerable.Range(0, 8)
            .Select(x => DateTime.Today.AddDays(-x))
            .ToDictionary(d => d, d => weekStats.Find(ws => ws.Date == d))
            .Select(kv => kv.Value ?? new ThisWeekStat(kv.Key))
            .OrderByDescending(ws => ws.Date)
            .ToList();
    }

    public async Task<List<DayStatDto>> GetRecentDaysAsync() =>
        (await GetThisWeekStatsAsync().ConfigureAwait(false))
        .Select(d => new DayStatDto(d.Date, d.Count, d.Montant ?? 0, d.Maj ?? 0, d.Marge ?? 0))
        .ToList();

    /// <summary>Rows per kind; the long kinds keep only their most recent or biggest rows.</summary>
    private static readonly Dictionary<string, int> MaxRows = new() { ["jours"] = 400, ["semaines"] = 260, ["produits"] = 100, ["clients"] = 100 };

    private static readonly string[] Months = ["", "Janv.", "Févr.", "Mars", "Avr.", "Mai", "Juin", "Juil.", "Août", "Sept.", "Oct.", "Nov.", "Déc."];

    /// <summary>One tab of the phone statistics page. Time rows come oldest first, the others biggest first.</summary>
    public async Task<StatsDto> GetStatsAsync(string kind, Period period)
    {
        IEnumerable<StatRowDto> rows = kind switch
        {
            "jours" => (await DailyAsync(period: period).ConfigureAwait(false)).Select(d => new StatRowDto
            {
                Label = D(d.DateTime, "dd/MM/yyyy"), Short = D(d.DateTime, "dd/MM"), Date = d.DateTime,
                Factures = d.Factures, Montant = d.MontantFact ?? 0, Caisse = d.MontantOff ?? 0, Maj = d.MontantMaj ?? 0, Marge = d.Marge ?? 0
            }).OrderBy(r => r.Date),
            "semaines" => (await WeeklyAsync(period: period).ConfigureAwait(false)).Select(w => new StatRowDto
            {
                Label = $"Semaine du {D(w.DateDebut, "dd/MM/yyyy")}", Short = D(w.DateDebut, "dd/MM"), Date = w.DateDebut,
                Factures = w.Factures, Montant = w.MontantFact ?? 0, Caisse = w.MontantOff ?? 0, Maj = w.MontantMaj ?? 0, Marge = w.Marge ?? 0
            }).OrderBy(r => r.Date),
            "mois" => (await MonthlyAsync(period: period).ConfigureAwait(false)).Select(m =>
            {
                int.TryParse(m.Month, out var month);
                int.TryParse(m.Year, out var year);
                return new StatRowDto
                {
                    Label = $"{Months[month]} {year}", Short = $"{Months[month].TrimEnd('.')} {year % 100:00}", Date = new DateTime(year, Math.Max(1, month), 1),
                    Factures = m.Factures, Montant = m.MontantFact ?? 0, Caisse = m.MontantOff ?? 0, Maj = m.MontantMaj ?? 0, Marge = m.Marge ?? 0
                };
            }).OrderBy(r => r.Date),
            "annees" => (await YearlyAsync(period).ConfigureAwait(false)).Select(y => new StatRowDto
            {
                Label = $"{y.Year}", Short = $"{y.Year}", Date = new DateTime(y.Year ?? 1, 1, 1),
                Factures = y.Factures, Montant = y.MontantFact ?? 0, Caisse = y.MontantOff ?? 0, Maj = y.MontantMaj ?? 0, Marge = y.Marge ?? 0
            }).OrderBy(r => r.Date),
            "bordereaux" => (await BordereauxAsync(period: period).ConfigureAwait(false)).Select(b => new StatRowDto
            {
                Label = $"Bordereau N° {b.Num}", Short = b.Num, Date = b.DateFin,
                Detail = $"{b.Center} · {D(b.DateDebut, "dd/MM")} → {D(b.DateFin, "dd/MM/yyyy")}",
                Factures = b.Factures, Montant = b.MontantFact ?? 0, Caisse = b.MontantOff ?? 0, Maj = b.MontantMaj ?? 0, Marge = b.Marge ?? 0,
                Virement = b.Virement, Ecart = b.Ecart, PaiementSuivi = BordereauDto.EstPaiementSuivi(b.Center)
            }).OrderBy(r => r.Date),
            "produits" => (await ProductsAsync(period: period).ConfigureAwait(false)).Select(p => new StatRowDto
            {
                Label = p.Produits.Trim(), Detail = p.Dci, Key = p.NumEnr,
                Quantite = p.Qt, Montant = p.Total, Maj = p.Maj ?? 0, Marge = p.Marge ?? 0
            }).OrderByDescending(r => r.Quantite),
            "clients" => (await ByClientAsync(period: period).ConfigureAwait(false)).Select(c => new StatRowDto
            {
                Label = string.IsNullOrWhiteSpace(c.Malade) ? "—" : c.Malade.Trim(), Detail = c.NumAssure, Key = c.NumAssure, Rang = c.Rang,
                Factures = c.Factures, Quantite = c.Boites, Montant = c.MantFact ?? 0, Caisse = c.MontOff ?? 0, Maj = c.MontMaj ?? 0, Marge = c.Marge ?? 0
            }).OrderByDescending(r => r.Montant),
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, "Unknown statistics kind")
        };

        var all = rows.ToList();
        var count = all.Count;
        var totals = new StatRowDto
        {
            Label = "Total",
            Factures = all.Sum(r => r.Factures),
            Quantite = all.Sum(r => r.Quantite),
            Montant = all.Sum(r => r.Montant),
            Caisse = all.Sum(r => r.Caisse),
            Maj = all.Sum(r => r.Maj),
            Marge = all.Sum(r => r.Marge),
            Virement = kind == "bordereaux" ? all.Sum(r => r.Virement ?? 0) : null,
            Ecart = kind == "bordereaux" ? all.Sum(r => r.Ecart ?? 0) : null
        };

        if (MaxRows.TryGetValue(kind, out var max) && all.Count > max)
            all = all[0].Date is null ? all.Take(max).ToList() : all.TakeLast(max).ToList();

        return new StatsDto(kind, count, all, totals);
    }

    private static string D(DateTime? date, string format) =>
        date?.ToString(format, System.Globalization.CultureInfo.InvariantCulture) ?? "";

    public async ValueTask<IEnumerable<BordStatDto>> BordereauxAsync(Expression<Func<Bordereau, bool>>? predicate = null, Period? period = null, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var start = (period ?? Period).Start;
        var end = (period ?? Period).EndExclusive;

        // Same totals as the Bordereaux grid, in one GROUP BY over facture (see ChifaService.FactureTotals).
        var query = await (
                from x in db.Bordereaus.Where(predicate ?? (_ => true))
                from t in ChifaService.FactureTotals(db).LeftJoin(t => t.NumBord == x.NumBord)
                where x.Etat == 'C' && t.LastFacture != null &&
                      (start == null || t.LastFacture >= start) && (end == null || t.LastFacture < end)
                select new BordStatDto
                {
                    MontantOff = t.MontOff,
                    MontantFact = t.MontFact,
                    MontantAss = t.MontAss,
                    MontantMaj = t.Maj,
                    MontantFE = t.Fae,
                    Factures = t.Nmbr,
                    DateDebut = t.FirstFacture,
                    DateFin = t.LastFacture,
                    Center = x.Center!.Nom,
                    Num = x.NumBord,
                    Virement = x.MontVir
                })
            .OrderBy(x => x.DateFin)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return query;
    }

    public async ValueTask<IEnumerable<YearlyStat>> YearlyAsync(Period? period = null, CancellationToken cancellationToken = default)
    {
        Expression<Func<Facture, bool>> predicate = x => true;

        await using var db = new ChifaDb();
        var list = await db.Factures
            .Where(predicate.SetPeriod(period ?? Period))
            .GroupBy(x => x.DateFact!.Value.Date.Year)
            .Select(x => new YearlyStat
            {
                Year = x.Key,
                MontantFact = x.Sum(f => f.MontFact),
                MontantAss = x.Sum(f => f.MontAs),
                MontantOff = x.Sum(f => f.MontOff),
                MontantMaj = x.Sum(f => f.MontMaj),
                MontantFE = x.Sum(f => f.MontMajFae),
                Factures = x.Count(),
                DateDebut = x.Min(f => f.DateFact),
                DateFin = x.Max(f => f.DateFact),
                Assureis = x.CountExt(f => f.NumAssure, Sql.AggregateModifier.Distinct),
                Beneficiaires = x.CountExt(f => f.NumAssure + "|" + f.RangAd, Sql.AggregateModifier.Distinct)
            })
            .OrderBy(x => x.Year)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return list;
    }

    public async ValueTask<IEnumerable<MonthlyStat>> MonthlyAsync(Expression<Func<Facture, bool>>? predicate = null, Period? period = null, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var monthlyRows = await db.Factures
            .Where(predicate.SetPeriod(period ?? Period))
            .GroupBy(x => new { Year = x.DateFact!.Value.Year, Month = x.DateFact.Value.Month })
            .Select(x => new
            {
                Year = x.Key.Year,
                Month = x.Key.Month,
                MontantFact = x.Sum(f => f.MontFact),
                MontantAss = x.Sum(f => f.MontAs),
                MontantOff = x.Sum(f => f.MontOff),
                MontantMaj = x.Sum(f => f.MontMaj),
                MontantFE = x.Sum(f => f.MontMajFae),
                Factures = x.Count(),
                DateDebut = x.Min(f => f.DateFact),
                DateFin = x.Max(f => f.DateFact),
                Assureis = x.CountExt(f => f.NumAssure, Sql.AggregateModifier.Distinct),
                Beneficiaires = x.CountExt(f => f.NumAssure + "|" + f.RangAd, Sql.AggregateModifier.Distinct)
            })
            .OrderBy(x => x.DateDebut)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var list = monthlyRows
            .Select(x => new MonthlyStat
            {
                Year = x.Year.ToString(),
                Month = x.Month.ToString(),
                MontantFact = x.MontantFact,
                MontantAss = x.MontantAss,
                MontantOff = x.MontantOff,
                MontantMaj = x.MontantMaj,
                MontantFE = x.MontantFE,
                Factures = x.Factures,
                DateDebut = x.DateDebut,
                DateFin = x.DateFin,
                Assureis = x.Assureis,
                Beneficiaires = x.Beneficiaires
            })
            .ToList();

        return list;
    }

    public async ValueTask<IEnumerable<WeeklyStat>> WeeklyAsync(Expression<Func<Facture, bool>>? predicate = null, Period? period = null, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var weeklyRows = await db.Factures
            .Where(predicate.SetPeriod(period ?? Period))
            .GroupBy(x => x.DateFact!.Value.Date.AddDays(-(int)x.DateFact.Value.Date.DayOfWeek))
            .Select(x => new
            {
                DateDebut = x.Key,
                MontantFact = x.Sum(f => f.MontFact),
                MontantAss = x.Sum(f => f.MontAs),
                MontantMaj = x.Sum(f => f.MontMaj),
                MontantOff = x.Sum(f => f.MontOff),
                MontantFE = x.Sum(f => f.MontMajFae),
                Factures = x.Count(),
                Assureis = x.CountExt(f => f.NumAssure, Sql.AggregateModifier.Distinct),
                Beneficiaires = x.CountExt(f => f.NumAssure + "|" + f.RangAd, Sql.AggregateModifier.Distinct)
            })
            .OrderByDescending(x => x.DateDebut)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var query = weeklyRows
            .Select(x => new WeeklyStat
            {
                DateDebut = x.DateDebut,
                StartDate = x.DateDebut.ToString("yyyy-MM-dd"),
                MontantFact = x.MontantFact,
                MontantAss = x.MontantAss,
                MontantMaj = x.MontantMaj,
                MontantOff = x.MontantOff,
                MontantFE = x.MontantFE,
                Factures = x.Factures,
                Assureis = x.Assureis,
                Beneficiaires = x.Beneficiaires
            })
            .ToList();

        return query;
    }

    public async ValueTask<IEnumerable<DailyStat>> DailyAsync(Expression<Func<Facture, bool>>? predicate = null, Period? period = null, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var query = await db.Factures
            .Where(predicate.SetPeriod(period ?? Period))
            .GroupBy(x => x.DateFact!.Value.Date)
            .Select(g => new DailyStat
            {
                DateTime = g.Key,
                MontantFact = g.Sum(f => f.MontFact),
                MontantAss = g.Sum(f => f.MontAs),
                MontantMaj = g.Sum(f => f.MontMaj),
                MontantOff = g.Sum(f => f.MontOff),
                MontantFE = g.Sum(f => f.MontMajFae),
                Factures = g.Count(),
                Assureis = g.CountExt(f => f.NumAssure, Sql.AggregateModifier.Distinct),
                Beneficiaires = g.CountExt(f => f.NumAssure + "|" + f.RangAd, Sql.AggregateModifier.Distinct)
            })
            .OrderByDescending(x => x.DateTime)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return query;
    }

    public async ValueTask<IEnumerable<ClientsStat>> ByClientAsync(Expression<Func<Facture, bool>>? predicate = null, Period? period = null, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var factures = db.Factures.Where(predicate.SetPeriod(period ?? Period));

        var list = await factures
            .GroupBy(x => new { x.NumAssure, x.RangAd })
            .Select(x => new
            {
                x.Key.NumAssure,
                x.Key.RangAd,
                Malade = x.Max(f => f.Beneficiaire.FullName),
                MantFact = x.Sum(f => f.MontFact),
                MontMaj = x.Sum(f => f.MontMaj),
                MontAss = x.Sum(f => f.MontAs),
                MontOff = x.Sum(f => f.MontOff),
                MontFE = x.Sum(f => f.MontMajFae),
                Factures = x.Count()
            })
            .OrderByDescending(x => x.MantFact)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        // Boxes in a second grouped query: as a per-client subquery it rescanned facture for each client.
        var boites = await (
                from f in factures
                join d in db.DetailFacts on f.NumFact equals d.NumFact
                group d by new { f.NumAssure, f.RangAd } into g
                select new { g.Key.NumAssure, g.Key.RangAd, Qte = g.Sum(d => d.Qte) })
            .ToDictionaryAsync(x => ((string?)x.NumAssure, (string?)x.RangAd), x => x.Qte, cancellationToken)
            .ConfigureAwait(false);

        return list.Select(x => new ClientsStat
            {
                NumAssure = x.NumAssure!,
                Rang = x.RangAd,
                Malade = x.Malade,
                MantFact = x.MantFact,
                MontMaj = x.MontMaj,
                MontAss = x.MontAss,
                MontOff = x.MontOff,
                MontFE = x.MontFE,
                Boites = boites.GetValueOrDefault((x.NumAssure, x.RangAd)),
                Factures = x.Factures
            })
            .ToList();
    }
    public async ValueTask<IEnumerable<ProductStat>> ProductsAsync(Expression<Func<DetailFact, bool>>? predicate = null, Period? period = null, CancellationToken cancellationToken = default)
    {
        await using var db = new ChifaDb();
        var query = await db.DetailFacts
            .Where(predicate.SetPeriod(period ?? Period))
            .GroupBy(x => x.NumEnr)
            .Select(x => new ProductStat
            {
                NomCom = x.Max(m => m.Medicament.NomCom),
                CodeDci = x.Max(m => m.Medicament.CodeDci),
                Dci = x.Max(m => m.Medicament.NomDci),
                Condition = x.Max(m => m.Medicament.Conditionnement),
                Dosage = x.Max(m => m.Medicament.Dosage),
                Qt = x.Sum(m => m.Qte),
                Prix = x.Max(m => m.Ppa),
                Montant = x.Sum(m => m.Mont),
                MontAss = x.Sum(m => m.MontAs),
                NumEnr = x.Key,
                Local = x.Max(m => m.Local),
                Maj = x.Sum(m => m.MajLocal)
            })
            .OrderByDescending(x => x.Qt)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return query;
    }
}

public static class PredicateExtensions
{
    // All overloads filter whole days, both bounds inclusive: DateFact >= From.Date AND DateFact < To.Date + 1 day.
    // Bounds are captured as local values so each query gets the dates as they were when it was built.

    public static Expression<Func<Facture, bool>> SetPeriod(this Expression<Func<Facture, bool>>? predicate, Period? period = default)
    {
        period ??= new Period();
        predicate ??= _ => true;

        predicate = predicate.And(x => x.DateFact != null);

        if (period.Start is { } start)
            predicate = predicate.And(x => x.DateFact >= start);

        if (period.EndExclusive is { } end)
            predicate = predicate.And(x => x.DateFact < end);

        return predicate;
    }

    public static Expression<Func<DetailFact, bool>> SetPeriod(this Expression<Func<DetailFact, bool>>? predicate, Period? period = default)
    {
        period ??= new Period();
        predicate ??= _ => true;

        predicate = predicate.And(x => x.Facture.DateFact.HasValue);

        if (period.Start is { } start)
            predicate = predicate.And(x => x.Facture.DateFact >= start);

        if (period.EndExclusive is { } end)
            predicate = predicate.And(x => x.Facture.DateFact < end);

        return predicate;
    }

    public static Expression<Func<Bordereau, bool>> SetPeriod(this Expression<Func<Bordereau, bool>>? predicate,
        Period? period = default)
    {
        period ??= new Period();
        predicate ??= _ => true;

        predicate = predicate.And(x => x.DateExtract != null);

        if (period.Start is { } start)
            predicate = predicate.And(x => x.DateExtract >= start);

        if (period.EndExclusive is { } end)
            predicate = predicate.And(x => x.DateExtract < end);

        return predicate;
    }

    public static string FullName(this Medicament? m)
    {
        return $"{m?.NomCom} {m?.Dosage} {m?.Conditionnement}";
    }
}

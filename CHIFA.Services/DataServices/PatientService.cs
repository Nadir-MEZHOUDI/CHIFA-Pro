using LinqToDB.Async;

namespace CHIFA.Services.DataServices;

/// <summary>Patients and medicines as the phone app browses them: searched and paged in the database.</summary>
public class PatientService
{
    public static PatientService Instance { get; } = new();

    private const int MaxPatientsPerMedicament = 200;
    private const int RecentFactures = 30;

    /// <summary>
    /// Patients with at least one facture, last visit first. The text matches the name (every word) or,
    /// when it is all digits, the start of the insured number.
    /// </summary>
    public async ValueTask<(int Total, List<PatientRowDto> Items)> SearchPatientsAsync(string? text, int skip, int take)
    {
        await using var db = new ChifaDb();

        var beneficiaires = db.Beneficiaires.AsQueryable();
        var term = text?.Trim();
        if (!string.IsNullOrEmpty(term))
        {
            if (term.All(char.IsDigit))
                beneficiaires = beneficiaires.Where(b => b.NumAssure.StartsWith(term));
            else
                foreach (var word in Words(term))
                    beneficiaires = beneficiaires.Where(b => (b.Nom + " " + b.Prenom).ToUpper().Contains(word));
        }

        var visits =
            from f in db.Factures
            where f.NumAssure != null && f.RangAd != null
            group f by new { f.NumAssure, f.RangAd } into g
            select new { g.Key.NumAssure, g.Key.RangAd, Derniere = g.Max(f => f.DateFact), Nombre = g.Count() };

        var query =
            from b in beneficiaires
            join v in visits on new { N = b.NumAssure, R = b.RangAd } equals new { N = v.NumAssure!, R = v.RangAd! }
            select new PatientRowDto
            {
                NumAssure = b.NumAssure,
                Rang = b.RangAd,
                Nom = b.FullName,
                Assure = b.Assure!.FullName,
                DerniereVisite = v.Derniere,
                NombreFactures = v.Nombre
            };

        var total = await query.CountAsync().ConfigureAwait(false);
        var items = await query
            .OrderByDescending(p => p.DerniereVisite)
            .ThenBy(p => p.NumAssure)
            .ThenBy(p => p.Rang)
            .Skip(skip)
            .Take(take)
            .ToListAsync()
            .ConfigureAwait(false);

        var coverage = await Coverage.LoadAsync(db, items.Select(p => p.NumAssure).Distinct().ToList()).ConfigureAwait(false);
        foreach (var p in items)
            (p.Taux, p.TauxVarie, p.Mc) = Of(coverage, p.NumAssure, p.Rang);
        return (total, items);
    }

    private static (int? Taux, bool TauxVarie, bool Mc) Of(
        Dictionary<(string NumAssure, string Rang), PatientCoverage> coverage, string numAssure, string rang) =>
        coverage.TryGetValue((numAssure, rang), out var c) ? (c.Taux, c.TauxVarie, c.Mc) : (null, false, false);

    public async ValueTask<PatientDetailDto?> GetPatientAsync(string numAssure, string rang)
    {
        await using var db = new ChifaDb();

        var patient = await db.Beneficiaires
            .Where(b => b.NumAssure == numAssure && b.RangAd == rang)
            .Select(b => new PatientDetailDto
            {
                NumAssure = b.NumAssure,
                Rang = b.RangAd,
                Nom = b.FullName,
                Assure = b.Assure!.FullName
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (patient is null)
            return null;

        patient.Famille = await db.Beneficiaires
            .Where(b => b.NumAssure == numAssure)
            .OrderBy(b => b.RangAd)
            .Select(b => new FamilyMemberDto { Rang = b.RangAd, Nom = b.FullName })
            .ToListAsync()
            .ConfigureAwait(false);

        var factures = db.Factures.Where(f => f.NumAssure == numAssure && f.RangAd == rang);
        var totals = await factures
            .GroupBy(f => 1)
            .Select(g => new
            {
                Nombre = g.Count(),
                Montant = g.Sum(f => f.MontFact) ?? 0m,
                Premiere = g.Min(f => f.DateFact),
                Derniere = g.Max(f => f.DateFact)
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (totals is not null)
        {
            patient.NombreFactures = totals.Nombre;
            patient.MontantTotal = totals.Montant;
            patient.PremiereVisite = totals.Premiere;
            patient.DerniereVisite = totals.Derniere;
        }

        var last = await factures
            .OrderByDescending(f => f.DateFact)
            .Select(f => new { Centre = f.Center!.Nom, f.DateFinDroit, f.DateFinDroitBenef })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        patient.Centre = last?.Centre;
        patient.DateFinDroit = last?.DateFinDroitBenef ?? last?.DateFinDroit;

        var coverage = await Coverage.LoadAsync(db, [numAssure]).ConfigureAwait(false);
        (patient.Taux, patient.TauxVarie, patient.Mc) = Of(coverage, numAssure, rang);

        patient.Traitements = (await ChifaService.Instance.LoadConsumptionAsync(numAssure, rang, distinct: false).ConfigureAwait(false))
            .OfType<ConsumptionGroupedDto>()
            .Select(c => new TreatmentDto
            {
                NumEnr = c.NEnrg,
                Medicament = c.Medicament,
                DerniereDate = c.Date,
                Quantite = c.Qt,
                Duree = c.Duree,
                Prochain = c.Duree > 0 ? c.Prochain : null,
                Ts = c.TS ?? false,
                Medecin = c.Medecin,
                Delivrances = Math.Max(1, c.Historic?.Count ?? 0)
            })
            .OrderByDescending(t => t.DerniereDate)
            .ToList();

        patient.Factures = (await ChifaService.Instance.LoadHistoryAsync(numAssure, rang).ConfigureAwait(false))
            .Take(RecentFactures)
            .ToList();

        return patient;
    }

    /// <summary>
    /// Medicines dispensed in the period, most boxes first. The text matches the trade name or the DCI (every word).
    /// </summary>
    public async ValueTask<(int Total, List<MedicRowDto> Items)> SearchMedicamentsAsync(string? text, Period period, int skip, int take)
    {
        await using var db = new ChifaDb();

        var lines = Lines(db, period);
        var term = text?.Trim();
        if (!string.IsNullOrEmpty(term))
            foreach (var word in Words(term))
                lines = lines.Where(d => (d.Medicament.NomCom + " " + d.Medicament.NomDci).ToUpper().Contains(word));

        var sold =
            from d in lines
            select new { d.NumEnr, d.Qte, d.Ppa, d.NumFact, Patient = d.Facture.NumAssure + "-" + d.Facture.RangAd };

        var byMedicament =
            from d in sold
            group d by d.NumEnr into g
            select new
            {
                NumEnr = g.Key,
                Quantite = g.Sum(x => x.Qte),
                Montant = g.Sum(x => x.Qte * x.Ppa),
                Factures = g.CountExt(x => x.NumFact, Sql.AggregateModifier.Distinct),
                Patients = g.CountExt(x => x.Patient, Sql.AggregateModifier.Distinct)
            };

        var query =
            from s in byMedicament
            join m in db.Medicaments on s.NumEnr equals m.NumEnr
            select new MedicRowDto
            {
                NumEnr = m.NumEnr,
                Nom = m.FullName,
                Dci = m.NomDci,
                Laboratoire = m.Laboratoire,
                Quantite = s.Quantite,
                Montant = s.Montant,
                Factures = s.Factures,
                Patients = s.Patients
            };

        var total = await query.CountAsync().ConfigureAwait(false);
        var items = await query
            .OrderByDescending(m => m.Quantite)
            .ThenBy(m => m.Nom)
            .Skip(skip)
            .Take(take)
            .ToListAsync()
            .ConfigureAwait(false);
        return (total, items);
    }

    public async ValueTask<MedicDetailDto?> GetMedicamentAsync(string numEnr, Period period)
    {
        await using var db = new ChifaDb();

        var medic = await db.Medicaments
            .Where(m => m.NumEnr == numEnr)
            .Select(m => new MedicDetailDto
            {
                NumEnr = m.NumEnr,
                Nom = m.FullName,
                Dci = m.NomDci,
                Laboratoire = m.Laboratoire,
                Forme = m.Forme.LibelleCourt,
                Pays = m.Pays,
                TarifRef = m.TarifRef,
                Generique = m.Generic == 'O',
                Psychotrope = m.Tableau != null && m.Tableau != ' '
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (medic is null)
            return null;

        var lines =
            from d in Lines(db, period)
            where d.NumEnr == numEnr
            select new { d.Qte, d.Ppa, d.NumFact, d.Facture.NumAssure, d.Facture.RangAd, d.Facture.DateFact };

        var totals = await lines
            .GroupBy(d => 1)
            .Select(g => new
            {
                Quantite = g.Sum(x => x.Qte),
                Montant = g.Sum(x => x.Qte * x.Ppa),
                Factures = g.CountExt(x => x.NumFact, Sql.AggregateModifier.Distinct)
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (totals is null)
            return medic;

        medic.Quantite = totals.Quantite;
        medic.Montant = totals.Montant;
        medic.Factures = totals.Factures;

        medic.ParMois = await lines
            .GroupBy(d => new { d.DateFact!.Value.Year, d.DateFact.Value.Month })
            .Select(g => new MonthQuantityDto { Annee = g.Key.Year, Mois = g.Key.Month, Quantite = g.Sum(x => x.Qte) })
            .OrderBy(m => m.Annee).ThenBy(m => m.Mois)
            .ToListAsync()
            .ConfigureAwait(false);

        var byPatient =
            from d in lines
            where d.NumAssure != null && d.RangAd != null
            group d by new { d.NumAssure, d.RangAd } into g
            select new
            {
                g.Key.NumAssure,
                g.Key.RangAd,
                Quantite = g.Sum(x => x.Qte),
                Factures = g.CountExt(x => x.NumFact, Sql.AggregateModifier.Distinct),
                Derniere = g.Max(x => x.DateFact)
            };

        var patients =
            from p in byPatient
            from b in db.Beneficiaires.LeftJoin(b => b.NumAssure == p.NumAssure && b.RangAd == p.RangAd)
            select new MedicPatientDto
            {
                NumAssure = p.NumAssure!,
                Rang = p.RangAd!,
                Nom = b.FullName,
                Quantite = p.Quantite,
                Factures = p.Factures,
                DerniereDate = p.Derniere
            };

        medic.NombrePatients = await byPatient.CountAsync().ConfigureAwait(false);
        medic.Patients = await patients
            .OrderByDescending(p => p.DerniereDate)
            .Take(MaxPatientsPerMedicament)
            .ToListAsync()
            .ConfigureAwait(false);
        return medic;
    }

    public async ValueTask<FactureFullDto?> GetFactureAsync(string numFact)
    {
        await using var db = new ChifaDb();
        var facture = await db.Factures
            .Where(f => f.NumFact == numFact)
            .Select(f => new FactureFullDto
            {
                NumFact = f.NumFact,
                DateFact = f.DateFact,
                DateSoin = f.DateSoin,
                NumAssure = f.NumAssure,
                Rang = f.RangAd,
                Malade = f.Beneficiaire.FullName,
                Assure = f.Assure.FullName,
                Centre = f.Center!.Nom,
                Specialite = f.Specialite!.Libelle,
                Prescripteur = f.Prescripteur,
                Bordereau = f.NumBord,
                MontFact = f.MontFact,
                MontOff = f.MontOff,
                MontAss = f.MontAs,
                Majoration = f.MontMaj,
                Taux = Coverage.Percent(f.Taux),
                Mc = Coverage.IsChronicOrdonnance(f.TypeOrd)
            })
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);
        if (facture is null)
            return null;

        facture.Lignes = (await ChifaService.Instance.GetFactureDetailsByIdAsync(numFact).ConfigureAwait(false)).ToList();
        return facture;
    }

    /// <summary>Chronic patients late for their next treatment or expected within a week, most urgent first.</summary>
    public async ValueTask<List<ForecastRefillDto>> GetPatientsToRemindAsync()
    {
        var forecast = await ScopeService.Instance.GetChronicForecastsAsync().ConfigureAwait(false);
        return forecast.Details
            .Where(f => f.JoursRestants <= 7)
            .OrderBy(f => f.JoursRestants)
            .ToList();
    }

    private static IQueryable<DetailFact> Lines(ChifaDb db, Period period)
    {
        var start = period.Start;
        var end = period.EndExclusive;
        return db.DetailFacts.Where(d =>
            (start == null || d.Facture.DateFact >= start) && (end == null || d.Facture.DateFact < end));
    }

    private static IEnumerable<string> Words(string text) =>
        text.ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Take(5);
}

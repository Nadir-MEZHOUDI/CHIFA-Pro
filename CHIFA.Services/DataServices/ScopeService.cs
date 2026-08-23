using LinqToDB.Async;

namespace CHIFA.Services.DataServices;

public class ScopeService
{
    private static ScopeService? _instance;
    public static ScopeService Instance => _instance ??= new ScopeService();

    public readonly Period Period = new();

    public async ValueTask<ScopeDashboardDto> GetScopeDashboardAsync(Period? period = null)
    {
        var activePeriod = period ?? Period;
        await using var db = new ChifaDb();

        Expression<Func<Facture, bool>> predicate = f => f.DateFact != null;
        predicate = predicate.SetPeriod(activePeriod);

        var facturesQuery = db.Factures.Where(predicate);

        // Core Aggregations
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
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        // Total Boxes from Details
        var totalBoxes = await db.DetailFacts
            .Where(d => d.Facture.DateFact != null)
            .Where(d => (activePeriod.From == null || d.Facture.DateFact >= activePeriod.From) &&
                        (activePeriod.To == null || d.Facture.DateFact <= activePeriod.To))
            .SumAsync(d => (decimal?)d.Qte)
            .ConfigureAwait(false) ?? 0m;

        // Hourly Activity
        var hourlyRaw = await facturesQuery
            .GroupBy(f => f.DateFact!.Value.Hour)
            .Select(g => new
            {
                Hour = g.Key,
                Count = g.Count(),
                Total = g.Sum(f => f.MontFact) ?? 0m
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var hourlyList = Enumerable.Range(8, 14) // 08:00 to 22:00
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

        // Repartition par Centre (CNAS, CASNOS, etc.)
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
            .ToListAsync()
            .ConfigureAwait(false);

        // Top 10 Medicaments
        var topProducts = await db.DetailFacts
            .Where(d => d.Facture.DateFact != null)
            .Where(d => (activePeriod.From == null || d.Facture.DateFact >= activePeriod.From) &&
                        (activePeriod.To == null || d.Facture.DateFact <= activePeriod.To))
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
            .ToListAsync()
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

    public async ValueTask<IEnumerable<RejetDto>> GetRejetsAsync(Period? period = null)
    {
        var activePeriod = period ?? Period;
        await using var db = new ChifaDb();

        var bordereaux = await db.Bordereaus
            .Where(b => (activePeriod.From == null || b.DateCloture >= activePeriod.From || b.DateDepotFtp >= activePeriod.From) &&
                        (activePeriod.To == null || b.DateCloture <= activePeriod.To || b.DateDepotFtp <= activePeriod.To))
            .Select(b => new
            {
                b.NumBord,
                Center = b.Center!.Nom,
                b.DateCloture,
                b.DateDepotFtp,
                b.MontVir,
                Etat = b.Etat.ToString(),
                NombreFactures = b.Factures.Count(),
                MontantFacture = b.Factures.Sum(f => f.MontOff) ?? 0m
            })
            .ToListAsync()
            .ConfigureAwait(false);

        return bordereaux
            .Select(b => new RejetDto
            {
                NumBord = b.NumBord,
                Centre = b.Center ?? "Inconnu",
                DateBord = b.DateCloture ?? b.DateDepotFtp,
                DateVirement = b.DateDepotFtp,
                NombreFactures = b.NombreFactures,
                MontantFacture = b.MontantFacture,
                MontantVirement = b.MontVir ?? 0m,
                EtatBordereau = b.Etat ?? ""
            })
            .OrderByDescending(r => r.MontantRejete)
            .ThenByDescending(r => r.DateBord)
            .ToList();
    }

    public async ValueTask<IEnumerable<PsychotropeDto>> GetPsychotropesAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null)
    {
        var activePeriod = period ?? Period;
        await using var db = new ChifaDb();

        var explicitCodes = PsychotropesHelper.ExplicitDciCodes;
        var excludedCodes = PsychotropesHelper.ExcludedDciCodes.ToList();

        Expression<Func<DetailFact, bool>> basePredicate = d =>
            (
                d.Medicament.Tableau == 'B' ||
                explicitCodes.Contains(d.Medicament.CodeDci!) ||
                (
                    d.Medicament.CodeDci != null &&
                    (d.Medicament.CodeDci.StartsWith("16B") ||
                     d.Medicament.CodeDci.StartsWith("16C") ||
                     d.Medicament.CodeDci.StartsWith("15A") ||
                     d.Medicament.CodeDci.StartsWith("15D") ||
                     d.Medicament.CodeDci.StartsWith("03F") ||
                     d.Medicament.CodeDci.StartsWith("16A") ||
                     d.Medicament.CodeDci.StartsWith("16D") ||
                     d.Medicament.CodeDci.StartsWith("16E")) &&
                    !excludedCodes.Contains(d.Medicament.CodeDci!)
                )
            ) &&
            (activePeriod.From == null || d.Facture.DateFact >= activePeriod.From) &&
            (activePeriod.To == null || d.Facture.DateFact <= activePeriod.To);

        if (predicate != null)
            basePredicate = basePredicate.And(predicate);

        var list = await db.DetailFacts
            .Where(basePredicate)
            .Select(d => new
            {
                NumFact = d.Facture.NumFact,
                DateDelivrance = d.Facture.DateFact,
                NumAssure = d.Facture.NumAssure,
                NomAssure = d.Facture.Assure.FullName,
                NomBeneficiaire = d.Facture.Beneficiaire.FullName,
                Rang = d.Facture.RangAd,
                Medecin = d.Facture.Specialite!.Libelle,
                SpecialiteMedecin = d.Facture.Specialite.Libelle,
                CodeEnr = d.NumEnr,
                CodeDci = d.Medicament.CodeDci,
                Tableau = d.Medicament.Tableau != null ? d.Medicament.Tableau.ToString() : null,
                MedicamentNom = d.Medicament.NomCom,
                Dci = d.Medicament.NomDci,
                Dosage = d.Medicament.Dosage,
                Forme = d.Medicament.Forme.LibelleCourt,
                Quantite = d.Qte,
                DureeTrait = d.DureeTrait,
                Ppa = d.Ppa,
                TarifRef = d.TarifRef,
                NumeroOrdonnance = d.Facture.NumFact,
                EstTraitementSpecial = d.Ts == true
            })
            .OrderByDescending(p => p.DateDelivrance)
            .Take(1000)
            .ToListAsync()
            .ConfigureAwait(false);

        return list.Select(d => new PsychotropeDto
        {
            NumFact = d.NumFact,
            DateDelivrance = d.DateDelivrance,
            NumAssure = d.NumAssure,
            NomAssure = d.NomAssure,
            NomBeneficiaire = d.NomBeneficiaire,
            Rang = d.Rang,
            Medecin = d.Medecin,
            SpecialiteMedecin = d.SpecialiteMedecin,
            CodeEnr = d.CodeEnr,
            CodeDci = d.CodeDci,
            CategoriePsychotrope = PsychotropesHelper.GetCategorie(d.CodeDci),
            Tableau = d.Tableau,
            MedicamentNom = d.MedicamentNom,
            Dci = d.Dci,
            Dosage = d.Dosage,
            Forme = d.Forme,
            Quantite = d.Quantite,
            DureeTrait = d.DureeTrait,
            Ppa = d.Ppa,
            TarifRef = d.TarifRef,
            NumeroOrdonnance = d.NumeroOrdonnance,
            EstTraitementSpecial = d.EstTraitementSpecial
        }).ToList();
    }

    public async ValueTask<ForecastSummaryDto> GetChronicForecastsAsync(int lookbackMonths = 6, int forecastDays = 30)
    {
        await using var db = new ChifaDb();
        var lookbackDate = DateTime.Today.AddMonths(-lookbackMonths);
        var maxForecastDate = DateTime.Today.AddDays(forecastDays);

        var chronicDetails = await db.DetailFacts
            .Where(d => d.Facture.DateFact >= lookbackDate && (d.DureeTrait >= 20 || d.Ts == true))
            .Select(d => new
            {
                d.Facture.NumAssure,
                Rang = d.Facture.RangAd,
                NomAssure = d.Facture.Assure.FullName,
                NomBenef = d.Facture.Beneficiaire.FullName,
                DateFact = d.Facture.DateFact,
                Duree = (int)(d.DureeTrait ?? 30m),
                d.MontPharm,
                MedicNom = d.Medicament.NomCom,
                Specialite = d.Facture.Specialite!.Libelle
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var groupedPatients = chronicDetails
            .Where(x => x.NumAssure != null && x.DateFact.HasValue)
            .GroupBy(x => new { x.NumAssure, x.Rang })
            .Select(g =>
            {
                var latestFacture = g.OrderByDescending(x => x.DateFact).First();
                var lastDate = latestFacture.DateFact!.Value;
                var estimatedDuration = latestFacture.Duree <= 0 ? 30 : latestFacture.Duree;
                var estimatedReturn = lastDate.Date.AddDays(estimatedDuration);

                var avgAmount = g.GroupBy(x => x.DateFact).Average(grp => grp.Sum(i => i.MontPharm) ?? 0m);
                var medList = string.Join(", ", g.Select(x => x.MedicNom).Where(n => !string.IsNullOrEmpty(n)).Distinct().Take(3));

                return new ForecastRefillDto
                {
                    NumAssure = g.Key.NumAssure,
                    Rang = g.Key.Rang,
                    NomAssure = latestFacture.NomAssure,
                    NomBeneficiaire = latestFacture.NomBenef,
                    DateDerniereVisite = lastDate,
                    DureeDernierTraitement = estimatedDuration,
                    DatePrevisionnelleRetour = estimatedReturn,
                    MontantMoyenHabituel = Math.Round(avgAmount, 2),
                    NombreTraitementsPrevus = g.Select(x => x.MedicNom).Distinct().Count(),
                    TraitementHabituelResume = medList,
                    MedecinHabituel = latestFacture.Specialite
                };
            })
            .Where(f => f.DatePrevisionnelleRetour.HasValue &&
                        f.DatePrevisionnelleRetour.Value.Date >= DateTime.Today.AddDays(-15) &&
                        f.DatePrevisionnelleRetour.Value.Date <= maxForecastDate)
            .OrderBy(f => f.DatePrevisionnelleRetour)
            .ToList();

        var totalForecastAmount = groupedPatients
            .Where(f => f.DatePrevisionnelleRetour >= DateTime.Today)
            .Sum(f => f.MontantMoyenHabituel);

        return new ForecastSummaryDto
        {
            TotalPatientsAttendus = groupedPatients.Count,
            PatientsEnRetard = groupedPatients.Count(f => f.JoursRestants < 0),
            PatientsCetteSemaine = groupedPatients.Count(f => f.JoursRestants >= 0 && f.JoursRestants <= 7),
            EstimationCaPrevisionnel = Math.Round(totalForecastAmount, 2),
            Details = groupedPatients
        };
    }

    public async ValueTask<AuditReportDto> AuditBordereauAsync(string? numBord = null, Period? period = null)
    {
        var activePeriod = period ?? Period;
        await using var db = new ChifaDb();

        var query = db.Factures.AsQueryable();
        if (!string.IsNullOrEmpty(numBord))
            query = query.Where(f => f.NumBord == numBord);
        else if (activePeriod.From != null || activePeriod.To != null)
            query = query.Where(f => (activePeriod.From == null || f.DateFact >= activePeriod.From) &&
                                     (activePeriod.To == null || f.DateFact <= activePeriod.To));

        query = query.Where(f => f.Bordereau != null && f.Bordereau.Etat == 'O');

        var facturesWithDetails = await query
            .Select(f => new
            {
                f.NumFact,
                f.NumBord,
                f.DateFact,
                f.DateSoin,
                f.NumAssure,
                NomAssure = f.Assure.FullName,
                f.MontFact,
                Details = f.DetailFacts.Select(d => new
                {
                    d.NumEnr,
                    MedicName = d.Medicament.NomCom,
                    d.Ppa,
                    d.TarifRef,
                    d.Qte,
                    d.DureeTrait,
                    d.Ts
                }).ToList()
            })
            .ToListAsync()
            .ConfigureAwait(false);

        var anomalies = new List<AuditAnomalyDto>();

        foreach (var f in facturesWithDetails)
        {
            if (f.Details.Count == 0)
            {
                anomalies.Add(new AuditAnomalyDto
                {
                    NumFact = f.NumFact,
                    NumBord = f.NumBord,
                    DateFact = f.DateFact,
                    NumAssure = f.NumAssure,
                    NomAssure = f.NomAssure,
                    TypeAnomalie = "Facture Orpheline",
                    Description = "La facture ne contient aucune ligne de médicament délivré.",
                    Severite = AnomalySeverity.Critique,
                    ActionRecommandee = "Supprimer ou ré-enregistrer les détails de la facture",
                    MontantImpacte = f.MontFact
                });
                continue;
            }

            if (f.DateSoin.HasValue && f.DateFact.HasValue && f.DateSoin.Value > f.DateFact.Value)
            {
                anomalies.Add(new AuditAnomalyDto
                {
                    NumFact = f.NumFact,
                    NumBord = f.NumBord,
                    DateFact = f.DateFact,
                    NumAssure = f.NumAssure,
                    NomAssure = f.NomAssure,
                    TypeAnomalie = "Incohérence Date de Soin",
                    Description = $"Date de soin ({f.DateSoin.Value:dd/MM/yyyy}) postérieure à la date de facturation ({f.DateFact.Value:dd/MM/yyyy}).",
                    Severite = AnomalySeverity.Critique,
                    ActionRecommandee = "Corriger la date de soin pour respecter la chronologie",
                    MontantImpacte = f.MontFact
                });
            }

            foreach (var d in f.Details)
            {
                if (d.Qte > 4 && (d.DureeTrait == null || d.DureeTrait <= 30) && d.Ts != true)
                {
                    anomalies.Add(new AuditAnomalyDto
                    {
                        NumFact = f.NumFact,
                        NumBord = f.NumBord,
                        DateFact = f.DateFact,
                        NomProduit = d.MedicName,
                        NumAssure = f.NumAssure,
                        NomAssure = f.NomAssure,
                        TypeAnomalie = "Quantité Élevée sans Mention TS",
                        Description = $"Délivrance de {d.Qte} boîtes pour 30 jours sans statut Traitement Spécial.",
                        Severite = AnomalySeverity.Avertissement,
                        ActionRecommandee = "Vérifier la mention Traitement Spécial ou ajuster la posologie",
                        MontantImpacte = d.Ppa * d.Qte
                    });
                }
            }
        }

        var totalCritiques = anomalies.Count(a => a.Severite == AnomalySeverity.Critique);
        var totalAvertissements = anomalies.Count(a => a.Severite == AnomalySeverity.Avertissement);
        var montantRisque = anomalies.Sum(a => a.MontantImpacte ?? 0m);

        return new AuditReportDto
        {
            TotalFacturesAuditees = facturesWithDetails.Count,
            FacturesConformes = facturesWithDetails.Count - anomalies.Select(a => a.NumFact).Distinct().Count(),
            FacturesAvecAnomalies = anomalies.Select(a => a.NumFact).Distinct().Count(),
            TotalAnomaliesCritiques = totalCritiques,
            TotalAvertissements = totalAvertissements,
            MontantRisqueRejet = montantRisque,
            Anomalies = anomalies.OrderByDescending(a => a.Severite).ToList()
        };
    }

    public async ValueTask<IEnumerable<FactureDto>> GetRecentFacturesLiveAsync(int limit = 50)
    {
        await using var db = new ChifaDb();
        return await db.Factures
            .OrderByDescending(f => f.DateFact)
            .Take(limit)
            .Select(f => new FactureDto
            {
                NumFact = f.NumFact,
                NumAssure = f.NumAssure,
                DateFact = f.DateFact,
                DateSoin = f.DateSoin,
                Malade = f.Beneficiaire.FullName,
                Assure = f.Assure.FullName,
                MontFact = f.MontFact,
                MontAss = f.MontAs,
                MontOff = f.MontOff,
                Majoration = f.MontMaj,
                Bordereau = f.NumBord,
                Specialite = f.Specialite!.Libelle,
                Centre = f.Center!.Nom,
                Rang = f.RangAd,
                TS = f.DetailFacts.Any(m => m.Ts == true),
                LongDuree = f.DetailFacts.Any(m => m.DureeTrait >= MedicalThresholds.LongTreatmentDurationDays)
            })
            .ToListAsync()
            .ConfigureAwait(false);
    }
}

using CHIFA.Services.DataServices;
using CHIFA.Services.Dtos;
using Xunit.Abstractions;

namespace CHIFA.Tests.Services;

[Trait("Category", "DbFunctional")]
public class PatientServiceTests(ITestOutputHelper output)
{
    private readonly PatientService _svc = PatientService.Instance;
    private static readonly Period AllTime = new() { From = new DateTime(2000, 1, 1) };

    private TimeWatch StartWatch() => new(output: output);

    [Fact]
    public async Task Recent_patients_come_last_visit_first()
    {
        using var w = StartWatch();
        var (total, items) = await _svc.SearchPatientsAsync(null, 0, 20);

        total.Should().BeGreaterThan(0);
        items.Should().HaveCountLessThanOrEqualTo(20);
        items.Select(p => p.DerniereVisite).Should().BeInDescendingOrder();
        items.Should().OnlyContain(p => p.NombreFactures > 0);
    }

    [Fact]
    public async Task Search_by_name_matches_every_word()
    {
        var (_, recent) = await _svc.SearchPatientsAsync(null, 0, 1);
        var someone = recent.Single();
        var firstWord = someone.Nom!.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];

        using var w = StartWatch();
        var (total, items) = await _svc.SearchPatientsAsync(firstWord.ToLowerInvariant(), 0, 200);

        total.Should().BeGreaterThan(0);
        items.Should().OnlyContain(p => p.Nom!.Contains(firstWord, StringComparison.OrdinalIgnoreCase));
        items.Should().Contain(p => p.NumAssure == someone.NumAssure && p.Rang == someone.Rang);
    }

    [Fact]
    public async Task Search_by_number_matches_the_start_of_the_insured_number()
    {
        var (_, recent) = await _svc.SearchPatientsAsync(null, 0, 1);
        var prefix = recent.Single().NumAssure[..6];

        var (_, items) = await _svc.SearchPatientsAsync(prefix, 0, 50);

        items.Should().NotBeEmpty().And.OnlyContain(p => p.NumAssure.StartsWith(prefix));
    }

    [Fact]
    public async Task Patient_detail_has_history_and_treatments()
    {
        var (_, recent) = await _svc.SearchPatientsAsync(null, 0, 1);
        var row = recent.Single();

        using var w = StartWatch();
        var patient = await _svc.GetPatientAsync(row.NumAssure, row.Rang);

        patient.Should().NotBeNull();
        patient!.NombreFactures.Should().Be(row.NombreFactures);
        patient.Factures.Should().NotBeEmpty();
        patient.Famille.Should().Contain(m => m.Rang == row.Rang);
        patient.Traitements.Should().NotBeEmpty();
        output.WriteLine($"{patient.Nom}: {patient.Traitements.Count} traitements, fin de droits {patient.DateFinDroit:d}");
    }

    [Fact]
    public async Task Patient_rate_is_the_one_of_their_last_facture()
    {
        var (_, rows) = await _svc.SearchPatientsAsync(null, 0, 30);
        rows.Should().OnlyContain(p => p.Taux == 80 || p.Taux == 100);
        rows.Should().OnlyContain(p => p.Mc != null && p.TauxVarie != null);

        var row = rows[0];
        var patient = await _svc.GetPatientAsync(row.NumAssure, row.Rang);
        var last = await _svc.GetFactureAsync(patient!.Factures[0].NumFact!);

        patient.Taux.Should().Be(row.Taux).And.Be(last!.Taux);
        patient.Mc.Should().Be(row.Mc);
        if (patient.Factures.Select(f => f.Taux).Distinct().Count() > 1)
            patient.TauxVarie.Should().BeTrue();
        output.WriteLine($"{patient.Nom}: {patient.Taux} %, varie {patient.TauxVarie}, MC {patient.Mc}");
    }

    [Fact]
    public async Task Unknown_patient_is_null()
    {
        (await _svc.GetPatientAsync("000000000000", "99")).Should().BeNull();
    }

    [Fact]
    public async Task Medicines_are_ranked_by_boxes_and_searchable()
    {
        using var w = StartWatch();
        var (total, items) = await _svc.SearchMedicamentsAsync(null, AllTime, 0, 20);

        total.Should().BeGreaterThan(0);
        items.Select(m => m.Quantite).Should().BeInDescendingOrder();
        items.Should().OnlyContain(m => m.Patients > 0 && m.Factures > 0 && m.Patients <= m.Factures);

        var top = items[0];
        var word = (top.Dci ?? top.Nom)!.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
        var (_, found) = await _svc.SearchMedicamentsAsync(word.ToLowerInvariant(), AllTime, 0, 200);
        found.Should().Contain(m => m.NumEnr == top.NumEnr);
    }

    [Fact]
    public async Task Medicine_detail_lists_its_patients()
    {
        var (_, items) = await _svc.SearchMedicamentsAsync(null, AllTime, 0, 1);
        var top = items.Single();

        using var w = StartWatch();
        var medic = await _svc.GetMedicamentAsync(top.NumEnr, AllTime);

        medic.Should().NotBeNull();
        medic!.Quantite.Should().Be(top.Quantite);
        medic.NombrePatients.Should().Be(top.Patients);
        medic.Patients.Should().NotBeEmpty();
        medic.Patients.Select(p => p.DerniereDate).Should().BeInDescendingOrder();
        medic.ParMois.Sum(m => m.Quantite).Should().Be(top.Quantite);
    }

    [Fact]
    public async Task Facture_detail_has_its_lines()
    {
        var recent = (await ScopeService.Instance.GetRecentFacturesLiveAsync(1)).Single();

        var facture = await _svc.GetFactureAsync(recent.NumFact!);

        facture.Should().NotBeNull();
        facture!.MontFact.Should().Be(recent.MontFact);
        facture.Lignes.Should().NotBeEmpty();
        (await _svc.GetFactureAsync("ZZZZZZZZ")).Should().BeNull();
    }

    [Fact]
    public async Task Patients_to_remind_are_due_within_a_week()
    {
        var reminders = await _svc.GetPatientsToRemindAsync();

        reminders.Should().OnlyContain(r => r.JoursRestants <= 7);
        reminders.Select(r => r.JoursRestants).Should().BeInAscendingOrder();
    }
}

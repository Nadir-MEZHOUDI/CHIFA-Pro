using CHIFA.Contract.Dtos;
using DataModel;

namespace CHIFA.Contract.Grpc;

public interface IChifaService
{
    Period Period { get; }

    ValueTask<IEnumerable<BordereauDto>> GetAllBordereauxAsync(Expression<Func<Bordereau, bool>>? predicate = default);

    ValueTask<IEnumerable<FactureDto>> GetAllFacturesAsync(bool? last=false, bool? ts=false, Period? period = null, Expression<Func<Facture, bool>>? predicate = default);

    ValueTask<IEnumerable<BeneficiareDto>> GetBeneficiaresAsync();
    ValueTask<BeneficiareDto?> GetBeneficiareByIdAsync(string num, string rang);
    ValueTask<IEnumerable<Centre>> GetCentersAsync();
    ValueTask<IEnumerable<FactureDetailDto>> GetFactureDetailsByIdAsync(string id);
    ValueTask<Parametre?> GetFirstOfficineAsync();
    ValueTask<IEnumerable<Forme>> GetFormesAsync();

    ValueTask<IEnumerable<MedicDto>> GetMedicamentsAsync(Expression<Func<Medicament, bool>>? predicate = default);

    ValueTask<string?> GetMedicObsAsync(string nEnr);

    ValueTask<IEnumerable<PatientOfTraitSpec>> GetPatientsOfTraitSpecAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = default);

    ValueTask<IEnumerable<TraitDetailsDto>> GetPatientTraitementAsync(string noAssure, string rang, bool proche, Expression<Func<DetailFact?, bool>>? predicate = default);
    ValueTask<IEnumerable<Utilisateur>> GetUsersAsync();
    ValueTask<IEnumerable<ListeNoire>> LoadAllListNoirAsync();

    ValueTask<IEnumerable<ConsumptionDto>> LoadConsumptionAsync(string noAssure, string rang, bool distinct);

    ValueTask<IEnumerable<FactureCm>> LoadControlsMedicalAsync();
    ValueTask<IEnumerable<FactureDto>> LoadHistoryAsync(string noAssure, string rang);
    ValueTask<IEnumerable<TraitSpec2>> PatientsWithTraitSpec2Async(Expression<Func<DetailFact, bool>>? predicate = default);

    ValueTask<IEnumerable<PatientWithTraitSpec>> PatientsWithTraitSpecAsync(Expression<Func<DetailFact, bool>>? predicate = default);

    ValueTask<IEnumerable<Specialite>> SpecialitesAsync();
    ValueTask UpdateCenter(Centre center);
    ValueTask GetMinAndMaxDatesAsync();
}
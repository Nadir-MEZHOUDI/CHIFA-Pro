using CHIFA.Stat.Dtos;

namespace CHIFA.Stat.ViewModels;

public abstract class DashVM
{
    public List<BordStat> Bordereaux => ChifaStatService.Instance.Bordereaux.Where(BordsCondition).ToList();
    public List<MedicStat> Medicaments => ChifaStatService.Instance.Medicaments.Where(MedicsCondition).ToList();
    public List<FactureStat> Factures => ChifaStatService.Instance.Factures.Where(FacturesCondition).ToList();

    public virtual Func<FactureStat, bool> FacturesCondition { get; } = x => true;
    public virtual Func<BordStat, bool> BordsCondition { get; } = x => true;
    public virtual Func<MedicStat, bool> MedicsCondition { get; } = x => true;

}

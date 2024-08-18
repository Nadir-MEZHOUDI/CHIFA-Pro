using CHIFA.Stat.Dtos;

namespace CHIFA.Stat.ViewModels;
public class GlobalDashVM : DashVM
{
    public override Func<FactureStat, bool> FacturesCondition { get; } = x => true;
    public override Func<BordStat, bool> BordsCondition { get; } = x => true;
    public override Func<MedicStat, bool> MedicsCondition { get; } = x => true;

}

using CHIFA.Stat.Dtos;

namespace CHIFA.Stat.ViewModels;
public class WeekDashVM : DashVM
{
    public override Func<FactureStat, bool> FacturesCondition { get; } = x => x.DateFact.GetValueOrDefault().Date >= ChifaStatService.LastDate.AddDays(-8);
    public override Func<BordStat, bool> BordsCondition { get; } = x => x.MaxDate.GetValueOrDefault().Date >= ChifaStatService.LastDate.AddDays(-8);
    public override Func<MedicStat, bool> MedicsCondition { get; } = x => x.DateFact.GetValueOrDefault().Date >= ChifaStatService.LastDate.AddDays(-8);

}

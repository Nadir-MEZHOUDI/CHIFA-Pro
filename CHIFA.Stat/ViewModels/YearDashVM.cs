using CHIFA.Stat.Dtos;

namespace CHIFA.Stat.ViewModels;
public class YearDashVM : DashVM
{
    public static int Year { get; set; } = DateTime.Now.Year;
    public override Func<FactureStat, bool> FacturesCondition { get; } = x => x.DateFact.GetValueOrDefault().Year == Year;
    public override Func<BordStat, bool> BordsCondition { get; } = x => x.MaxDate.GetValueOrDefault().Year == Year;
    public override Func<MedicStat, bool> MedicsCondition { get; } = x => x.DateFact.GetValueOrDefault().Year == Year;

}

using CHIFA.Stat.Dtos;

namespace CHIFA.Stat.ViewModels;
public class MonthDashVM : DashVM
{
    public static int Year { get; set; } = DateTime.Now.Year;
    public static int Month { get; set; } = DateTime.Now.Month;
    public override Func<FactureStat, bool> FacturesCondition { get; } = x => x.DateFact.GetValueOrDefault().Year == Year && x.DateFact.GetValueOrDefault().Month == Month;
    public override Func<BordStat, bool> BordsCondition { get; } = x => x.MaxDate.GetValueOrDefault().Year == Year && x.MaxDate.GetValueOrDefault().Month == Month;
    public override Func<MedicStat, bool> MedicsCondition { get; } = x => x.DateFact.GetValueOrDefault().Year == Year && x.DateFact.GetValueOrDefault().Month == Month;

}

using System.ServiceModel;
using CHIFA.Contract.Statistics;
using DataModel;

namespace CHIFA.Contract.Grpc;

[ServiceContract]
public interface IStatisticsService
{
    [OperationContract]
    ValueTask<List<ThisWeekStat>> GetThisWeekStatsAsync();

    [OperationContract]
    ValueTask<IEnumerable<BordStatDto>> BordereauxAsync(Expression<Func<Bordereau, bool>>? predicate = null);

    [OperationContract]
    ValueTask<IEnumerable<YearlyStat>> YearlyAsync();

    [OperationContract]
    ValueTask<IEnumerable<MonthlyStat>> MonthlyAsync(Expression<Func<Facture, bool>>? predicate = null);

    [OperationContract]
    ValueTask<IEnumerable<WeeklyStat>> WeeklyAsync(Expression<Func<Facture, bool>>? predicate = null);

    [OperationContract]
    ValueTask<IEnumerable<DailyStat>> DailyAsync(Expression<Func<Facture, bool>>? predicate = null);

    [OperationContract]
    ValueTask<IEnumerable<ClientsStat>> ByClientAsync(Expression<Func<Facture, bool>>? predicate = null);

    [OperationContract]
    ValueTask<IEnumerable<ProductStat>> ProductsAsync(Expression<Func<DetailFact, bool>>? predicate = null);
}
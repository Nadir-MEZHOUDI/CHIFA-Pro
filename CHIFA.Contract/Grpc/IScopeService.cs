using System.ServiceModel;
using CHIFA.Contract.Dtos;
using DataModel;

namespace CHIFA.Contract.Grpc;

[ServiceContract]
public interface IScopeService
{
    [OperationContract]
    ValueTask<ScopeDashboardDto> GetScopeDashboardAsync(Period? period = null);

    [OperationContract]
    ValueTask<IEnumerable<RejetDto>> GetRejetsAsync(Period? period = null);

    [OperationContract]
    ValueTask<IEnumerable<PsychotropeDto>> GetPsychotropesAsync(Period? period = null, Expression<Func<DetailFact, bool>>? predicate = null);

    [OperationContract]
    ValueTask<ForecastSummaryDto> GetChronicForecastsAsync(int lookbackMonths = 6, int forecastDays = 30);

    [OperationContract]
    ValueTask<AuditReportDto> AuditBordereauAsync(string? numBord = null, Period? period = null);

    [OperationContract]
    ValueTask<IEnumerable<FactureDto>> GetRecentFacturesLiveAsync(int limit = 50);
}

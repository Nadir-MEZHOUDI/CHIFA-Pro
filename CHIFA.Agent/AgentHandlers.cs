using System.Reflection;
using CHIFA.Contract;
using CHIFA.Services.DataServices;
using CHIFA.Services.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Npgsql;

namespace CHIFA.Agent;

/// <summary>
/// Answers the gateway with the same services CHIFA.Pro uses. Only the methods registered here can be called.
/// </summary>
public class AgentHandlers
{
    private static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "0";

    private readonly ILogger<AgentHandlers> logger;
    private readonly WatchlistStore _watchlist;
    private readonly AlertTracker _alerts;

    public AgentHandlers(ILogger<AgentHandlers> logger)
    {
        this.logger = logger;
        _watchlist = new WatchlistStore(logger);
        _alerts = new AlertTracker(logger, _watchlist);
    }

    public void Register(HubConnection connection)
    {
        connection.On(AgentMethods.GetInfo, () => RunAsync(AgentMethods.GetInfo, GetInfoAsync));

        connection.On<PeriodQuery, ScopeDashboardDto>(AgentMethods.GetDashboard, q =>
            RunAsync(AgentMethods.GetDashboard, async () => await StatisticsService.Instance.GetScopeDashboardAsync(ToPeriod(q))));

        connection.On<PageQuery, Page<RejetDto>>(AgentMethods.GetRejets, q =>
            RunAsync(AgentMethods.GetRejets, async () =>
                ToPage((await ScopeService.Instance.GetRejetsAsync(ToPeriod(q))).OrderByDescending(r => r.DateBord), q)));

        connection.On<PageQuery, Page<BordereauDto>>(AgentMethods.GetBordereaux, q =>
            RunAsync(AgentMethods.GetBordereaux, async () =>
                ToPage((await ChifaService.Instance.GetAllBordereauxAsync(period: ToPeriod(q))).OrderByDescending(b => b.DateOuverture), q)));

        connection.On<string, BordereauDetailDto?>(AgentMethods.GetBordereau, numBord =>
            RunAsync(AgentMethods.GetBordereau, async () => await ChifaService.Instance.GetBordereauAsync(numBord)));

        connection.On<PageQuery, Page<PsychotropeDto>>(AgentMethods.GetPsychotropes, q =>
            RunAsync(AgentMethods.GetPsychotropes, async () =>
                ToPage((await ScopeService.Instance.GetPsychotropesAsync(ToPeriod(q))).OrderByDescending(p => p.DateDelivrance), q)));

        connection.On<PageQuery, IEnumerable<FactureDto>>(AgentMethods.GetRecentFactures, q =>
            RunAsync(AgentMethods.GetRecentFactures, async () => await ScopeService.Instance.GetRecentFacturesLiveAsync(q.SafeTake, q.Text)));

        connection.On<string, FactureFullDto?>(AgentMethods.GetFacture, numFact =>
            RunAsync(AgentMethods.GetFacture, async () => await PatientService.Instance.GetFactureAsync(numFact)));

        connection.On<PageQuery, Page<PatientRowDto>>(AgentMethods.SearchPatients, q =>
            RunAsync(AgentMethods.SearchPatients, async () =>
            {
                var (total, items) = await PatientService.Instance.SearchPatientsAsync(q.Text, q.SafeSkip, q.SafeTake);
                return new Page<PatientRowDto>(total, items);
            }));

        connection.On<PatientKey, PatientDetailDto?>(AgentMethods.GetPatient, key =>
            RunAsync(AgentMethods.GetPatient, async () => await PatientService.Instance.GetPatientAsync(key.NumAssure, key.Rang)));

        connection.On<List<ForecastRefillDto>>(AgentMethods.GetPatientsToRemind, () =>
            RunAsync(AgentMethods.GetPatientsToRemind, async () => await PatientService.Instance.GetPatientsToRemindAsync()));

        connection.On<PageQuery, Page<MedicRowDto>>(AgentMethods.SearchMedicaments, q =>
            RunAsync(AgentMethods.SearchMedicaments, async () =>
            {
                var (total, items) = await PatientService.Instance.SearchMedicamentsAsync(q.Text, ToPeriod(q), q.SafeSkip, q.SafeTake);
                return new Page<MedicRowDto>(total, items);
            }));

        connection.On<MedicamentQuery, MedicDetailDto?>(AgentMethods.GetMedicament, q =>
            RunAsync(AgentMethods.GetMedicament, async () => await PatientService.Instance.GetMedicamentAsync(q.NumEnr, ToPeriod(q.From, q.To))));

        connection.On<List<DayStatDto>>(AgentMethods.GetRecentDays, () =>
            RunAsync(AgentMethods.GetRecentDays, StatisticsService.Instance.GetRecentDaysAsync));

        connection.On<StatsQuery, StatsDto>(AgentMethods.GetStats, q =>
            RunAsync(AgentMethods.GetStats, () => StatisticsService.Instance.GetStatsAsync(q.Kind, ToPeriod(q.From, q.To))));

        connection.On(AgentMethods.GetAlerts, () => RunAsync(AgentMethods.GetAlerts, _alerts.GetNewAlertsAsync));

        connection.On(AgentMethods.GetWatchlist, () => RunAsync(AgentMethods.GetWatchlist, _watchlist.LoadAsync));

        connection.On<Watchlist, Watchlist>(AgentMethods.SaveWatchlist, w =>
            RunAsync(AgentMethods.SaveWatchlist, async () =>
            {
                try
                {
                    return await _watchlist.SaveAsync(w);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    logger.LogError(ex, "Could not save {Path}", WatchlistStore.Path);
                    throw new HubException("Impossible d'enregistrer la surveillance sur l'ordinateur de la pharmacie.");
                }
            }));
    }

    private static async Task<AgentInfo> GetInfoAsync()
    {
        var officine = await ChifaService.Instance.GetFirstOfficineAsync();
        return new AgentInfo(officine?.NomPharmacie, Version, DateTime.Now);
    }

    public static Period ToPeriod(PeriodQuery query) => ToPeriod(query.From, query.To);

    public static Period ToPeriod(PageQuery query) => ToPeriod(query.From, query.To);

    private static Period ToPeriod(DateTime? from, DateTime? to)
    {
        var period = new Period { To = to };
        if (from.HasValue)
            period.From = from;
        return period;
    }

    public static Page<T> ToPage<T>(IEnumerable<T> items, PageQuery query)
    {
        var all = items as IReadOnlyCollection<T> ?? items.ToList();
        return new Page<T>(all.Count, all.Skip(query.SafeSkip).Take(query.SafeTake).ToList());
    }

    /// <summary>
    /// The SignalR client sends an exception's message to the gateway as is. Log the real error here
    /// and send a message that is safe to show on the phone.
    /// </summary>
    private async Task<T> RunAsync<T>(string method, Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (HubException)
        {
            throw;
        }
        catch (Exception ex) when (ex is NpgsqlException or TimeoutException)
        {
            logger.LogError(ex, "{Method}: CHIFA database unavailable", method);
            throw new HubException("La base de données CHIFA ne répond pas sur le poste de la pharmacie.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Method} failed", method);
            throw new HubException("Erreur interne de l'agent CHIFA.");
        }
    }
}

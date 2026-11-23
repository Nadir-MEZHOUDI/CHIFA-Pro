using System.Net;
using CHIFA.Contract;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace CHIFA.Agent;

/// <summary>
/// Keeps one outbound connection to the gateway, so the pharmacy needs no open port.
/// Short drops are handled by SignalR's automatic reconnect; this loop restarts the connection
/// when it is fully closed (gateway down at startup, key rejected, reconnect aborted).
/// </summary>
public class AgentWorker(IOptionsMonitor<AgentOptions> options, AgentHandlers handlers, ILogger<AgentWorker> logger) : BackgroundService
{
    private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(5);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var connection = BuildConnection();
        long failedStarts = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            var wait = CheckInterval;
            if (connection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await connection.StartAsync(stoppingToken);
                    logger.LogInformation("Connected to the gateway {Url}", options.CurrentValue.GatewayUrl);
                    failedStarts = 0;
                }
                catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    logger.LogError("The gateway rejected the agent key. Check Agent:AgentKey in appsettings.json.");
                    wait = TimeSpan.FromMinutes(1);
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    wait = ForeverRetryPolicy.DelayFor(failedStarts++);
                    logger.LogWarning("Gateway unreachable ({Error}), next try in {Delay:N0} s", ex.Message, wait.TotalSeconds);
                }
            }

            try
            {
                await Task.Delay(wait, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        await connection.StopAsync(CancellationToken.None);
    }

    private HubConnection BuildConnection()
    {
        var url = new Uri(new Uri(options.CurrentValue.GatewayUrl), AgentHub.Route);
        var connection = new HubConnectionBuilder()
            // Read on every (re)connection: a key replaced by SmartCHIFA is picked up without a restart.
            .WithUrl(url, o => o.AccessTokenProvider = () => Task.FromResult<string?>(options.CurrentValue.AgentKey))
            .WithAutomaticReconnect(new ForeverRetryPolicy())
            .WithKeepAliveInterval(TimeSpan.FromSeconds(15))
            .WithServerTimeout(TimeSpan.FromSeconds(30))
            .Build();

        handlers.Register(connection);

        connection.Reconnecting += ex =>
        {
            logger.LogWarning("Connection lost ({Error}), reconnecting...", ex?.Message);
            return Task.CompletedTask;
        };
        connection.Reconnected += _ =>
        {
            logger.LogInformation("Reconnected to the gateway");
            return Task.CompletedTask;
        };
        connection.Closed += ex =>
        {
            logger.LogWarning("Connection closed ({Error})", ex?.Message);
            return Task.CompletedTask;
        };

        return connection;
    }
}

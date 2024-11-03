using CHIFA.DAL.DataServices;
using Grpc.Core;
using ProtoBuf.Grpc.Server;
using Serilog;
using SmartApp.Bridge;

namespace CHIFA.Server.Helpers;

public class GrpcServer(ILogger logger, BridgeServer bridgeServer) : IDisposable
{
    private Grpc.Core.Server? Server { get; set; } = new();

    public async Task StartAsync()
    {
        await bridgeServer.StartAsync();

        await Stop();
        Server = new Grpc.Core.Server();

        Server.Services.AddCodeFirst(new ChifaService());
        Server.Services.AddCodeFirst(new StatisticsService());

        Server.Ports.Add(new ServerPort("0.0.0.0", (int)bridgeServer.Port!, ServerCredentials.Insecure));

        Server.Start();
        logger.Information("Server started...");
    }

    public async Task Stop()
    {
        try
        {
            // ReSharper disable once ArrangeNullCheckingPattern
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (Server is { })
            {
                await Server.ShutdownAsync();
                await Server.KillAsync();
            }
        }
        catch (Exception)
        {
            //ignored
        }
    }

    public void Dispose()
    {
        try
        {
            _ = Server?.KillAsync();
            Server = null!;
        }
        catch (Exception)
        {
            //ignored
        }
    }
}

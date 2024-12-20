using CHIFA.DAL.DataServices;
using Grpc.Core;
using ProtoBuf.Grpc.Server;
using Serilog;
using SmartApp.Bridge;

namespace CHIFA.Server.Helpers;

public class GrpcServer(ILogger logger, BridgeServer bridgeServer) : IDisposable
{
    private Grpc.Core.Server? _server;
    private readonly SemaphoreSlim _serverLock = new(1, 1);
    private readonly int _maxRetryAttempts = 3;
    private readonly int _retryDelayMs = 1000;
    private bool _disposed;

    public async Task StartAsync()
    {
        await _serverLock.WaitAsync();
        try
        {
            await bridgeServer.StartAsync();
            await Stop();

            for (int attempt = 1; attempt <= _maxRetryAttempts; attempt++)
            {
                try
                {
                    _server = new Grpc.Core.Server();
                    _server.Services.AddCodeFirst(new ChifaService());
                    _server.Services.AddCodeFirst(new StatisticsService());
                    _server.Ports.Add(new ServerPort("0.0.0.0", (int)bridgeServer.Port!, ServerCredentials.Insecure));
                    _server.Start();
                    
                    logger.Information("Server started successfully on attempt {Attempt}", attempt);
                    break;
                }
                catch (Exception ex) when (attempt < _maxRetryAttempts)
                {
                    logger.Warning(ex, "Failed to start server on attempt {Attempt}. Retrying...", attempt);
                    await Task.Delay(_retryDelayMs * attempt);
                }
            }
        }
        finally
        {
            _serverLock.Release();
        }
    }

    public async Task Stop()
    {
        await _serverLock.WaitAsync();
        try
        {
            if (_server != null)
            {
                try
                {
                    await _server.ShutdownAsync();
                    await _server.KillAsync();
                    logger.Information("Server stopped successfully");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Error stopping server");
                }
                finally
                {
                    _server = null;
                }
            }
        }
        finally
        {
            _serverLock.Release();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) 
            return;
        if (disposing)
        {
            _serverLock.Dispose();
            _ = Stop().ConfigureAwait(false);
        }
        _disposed = true;
    }
}

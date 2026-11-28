using CHIFA.Agent;
using DataModel;
using Npgsql;

var builder = Host.CreateApplicationBuilder(args);

// As a Windows service the working directory is System32: read appsettings.json next to the exe.
builder.Configuration.SetBasePath(AppContext.BaseDirectory);
builder.Configuration.AddJsonFile(AgentOptions.SharedSettingsPath, optional: true, reloadOnChange: true);
builder.Services.AddWindowsService(o => o.ServiceName = "CHIFA.Agent");

builder.Services.AddOptions<AgentOptions>().BindConfiguration(AgentOptions.Section).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddSingleton<AgentHandlers>();
builder.Services.AddHostedService<AgentWorker>();
builder.Services.AddOptions<ChifaDbOptions>().BindConfiguration(ChifaDbOptions.Section);
builder.Services.AddHostedService<ChifaServerStarter>();

ConfigureChifaDb(builder.Configuration.GetSection(ChifaDbOptions.Section).Get<ChifaDbOptions>() ?? new ChifaDbOptions());

await builder.Build().RunAsync();

// Same pool and timeout limits as SmartCHIFA: the agent shares the pharmacy's PostgreSQL server with it.
static void ConfigureChifaDb(ChifaDbOptions options)
{
    var connectionString = !string.IsNullOrWhiteSpace(options.ConnectionString)
        ? options.ConnectionString
        : new NpgsqlConnectionStringBuilder(ChifaDb.ConString) { Host = options.Host, Port = options.Port }.ConnectionString;

    ChifaDb.ConString = CHIFA.Services.Helpers.DatabaseDefaults.ForDesktop(connectionString);
}

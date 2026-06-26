using System.Data;
using DataModel;
using Npgsql;

namespace CHIFA.Benchmarks;

public static class DbConfig
{
    public static string ConnectionString { get; } = Build();

    static DbConfig()
    {
        // CHIFA.DAL's ChifaDb reads from ChifaDb.ConString at construction time
        ChifaDb.ConString = ConnectionString;
    }

    private static string Build()
    {
        var host = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_HOST") ?? "localhost";
        var port = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_PORT") ?? "5432";
        var user = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_USER") ?? "pharm";
        var pwd = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_PWD") ?? "REDACTED";
        var db = Environment.GetEnvironmentVariable("CHIFA_TEST_DB_NAME") ?? "CHIFA_OFFICINE";
        return $"Server={host};Port={port};User Id={user};Password={pwd};Database={db};Timeout=15;Command Timeout=120;Pooling=true;Maximum Pool Size=20;";
    }

    public static IDbConnection OpenDapper()
    {
        var c = new NpgsqlConnection(ConnectionString);
        c.Open();
        return c;
    }
}

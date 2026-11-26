using System.Net.Sockets;
using Npgsql;

namespace CHIFA.Services.Helpers;

public enum DbErrorKind
{
    /// <summary>Not a database availability problem: a real error worth reporting.</summary>
    None,

    /// <summary>The server is stopped, unreachable on the network, or shutting down.</summary>
    Unreachable,

    /// <summary>The server refuses new work: too many clients, out of memory or disk.</summary>
    Busy,

    /// <summary>The server or the network did not answer in time.</summary>
    Timeout
}

/// <summary>
/// Tells database availability problems (server down, overloaded, slow network) apart from bugs,
/// so the desktop app can show one notice instead of reporting every failed screen to telemetry.
/// </summary>
public static class DbErrors
{
    public static bool IsTransient(Exception? ex) => Classify(ex) != DbErrorKind.None;

    public static DbErrorKind Classify(Exception? ex)
    {
        var fallback = DbErrorKind.None;
        var fromNpgsql = false;
        for (var current = ex; current is not null; current = Next(current))
        {
            if (current is PostgresException pg)
                return FromSqlState(pg.SqlState);

            if (current is NpgsqlException npgsql)
            {
                fromNpgsql = true;
                // e.g. "Exception while reading from stream" over a connection the server reset.
                if (npgsql.IsTransient) fallback = DbErrorKind.Unreachable;
                continue;
            }

            // A timeout or socket error outside Npgsql (licensing, updates...) is not a database problem.
            if (!fromNpgsql) continue;

            var kind = current switch
            {
                TimeoutException => DbErrorKind.Timeout,
                SocketException { SocketErrorCode: SocketError.TimedOut } => DbErrorKind.Timeout,
                SocketException => DbErrorKind.Unreachable,
                _ => DbErrorKind.None
            };
            if (kind != DbErrorKind.None) return kind;
        }

        return fallback;
    }

    public static string UserMessage(DbErrorKind kind) => kind switch
    {
        DbErrorKind.Busy =>
            "Le serveur de base de données est saturé (trop de connexions ouvertes).\n" +
            "Fermez les programmes CHIFA inutilisés sur les autres postes puis réessayez.",
        DbErrorKind.Timeout =>
            "Le serveur de base de données met trop de temps à répondre.\n" +
            "Réessayez dans un instant ou réduisez la période affichée.",
        _ =>
            "Impossible de joindre le serveur de base de données.\n" +
            "Vérifiez que le poste serveur est allumé et que PostgreSQL est démarré, puis réessayez."
    };

    private static Exception? Next(Exception ex) =>
        ex is AggregateException { InnerExceptions.Count: 1 } aggregate ? aggregate.InnerExceptions[0] : ex.InnerException;

    private static DbErrorKind FromSqlState(string sqlState) => sqlState switch
    {
        // Class 53: insufficient resources (53300 = too many clients already).
        _ when sqlState.StartsWith("53", StringComparison.Ordinal) => DbErrorKind.Busy,
        PostgresErrorCodes.QueryCanceled => DbErrorKind.Timeout,
        PostgresErrorCodes.AdminShutdown or
        PostgresErrorCodes.CrashShutdown or
        PostgresErrorCodes.CannotConnectNow => DbErrorKind.Unreachable,
        // Class 08: connection exception.
        _ when sqlState.StartsWith("08", StringComparison.Ordinal) => DbErrorKind.Unreachable,
        _ => DbErrorKind.None
    };
}

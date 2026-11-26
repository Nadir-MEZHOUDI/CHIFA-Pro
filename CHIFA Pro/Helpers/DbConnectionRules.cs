using System.IO;
using System.Net.Sockets;
using Npgsql;

namespace CHIFA.Pro.Helpers;

/// <summary>
/// Pure rules about the database host and connection failures, kept free of WinForms
/// so CHIFA.Tests can compile this file directly.
/// </summary>
public static class DbConnectionRules
{
    /// <summary>
    /// Trims the server name typed by the user and rejects values Npgsql cannot use as a host,
    /// such as a shared folder path (\\100.107.7.123\D\CHIFA_OFFICINE).
    /// </summary>
    public static bool TryNormalizeHost(string? input, out string host, out string? error)
    {
        host = input?.Trim() ?? string.Empty;
        error = null;

        if (host.Length == 0)
        {
            error = "Saisissez le nom ou l'adresse IP du poste serveur.";
            return false;
        }

        if (host.IndexOfAny(['\\', '/']) >= 0)
        {
            var suggestion = host.Split(['\\', '/'], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            error = $"« {host} » est un chemin de dossier, pas un nom de poste. " +
                    "Saisissez seulement le nom ou l'adresse IP du poste serveur" +
                    (string.IsNullOrWhiteSpace(suggestion) || suggestion.Contains(':') ? "." : $", par exemple « {suggestion} ».");
            return false;
        }

        if (host.Any(char.IsWhiteSpace))
        {
            error = $"« {host} » n'est pas un nom de poste valide : il ne doit pas contenir d'espace.";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Server unreachable, unknown host, timeout, wrong password, server starting...:
    /// configuration or network problems on the customer's side, not defects worth reporting to telemetry.
    /// </summary>
    public static bool IsExpectedConnectionError(Exception ex) => ex switch
    {
        PostgresException pg => pg.SqlState is
            PostgresErrorCodes.InvalidPassword or
            PostgresErrorCodes.InvalidAuthorizationSpecification or
            PostgresErrorCodes.InvalidCatalogName or
            PostgresErrorCodes.CannotConnectNow or
            PostgresErrorCodes.TooManyConnections or
            PostgresErrorCodes.AdminShutdown,
        NpgsqlException { InnerException: SocketException or TimeoutException or IOException } => true,
        NpgsqlException npgsql => npgsql.IsTransient,
        SocketException or TimeoutException => true,
        _ => false
    };
}

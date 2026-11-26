using System.IO;
using System.Net.Sockets;
using CHIFA.Services.Helpers;
using Npgsql;

namespace CHIFA.Tests.Helpers;

/// <summary>The exception shapes come from the production telemetry groups they reproduce.</summary>
public class DbErrorsTests
{
    private static PostgresException Postgres(string sqlState) => new("message", "FATAL", "FATAL", sqlState);

    [Fact]
    public void ConnectionRefused_IsUnreachable() // facafe8b, 94c2a5b8
    {
        var ex = new NpgsqlException("Failed to connect to 127.0.0.1:5432", new SocketException((int)SocketError.ConnectionRefused));

        DbErrors.Classify(ex).Should().Be(DbErrorKind.Unreachable);
    }

    [Fact]
    public void ConnectTimeout_IsTimeout() // 3a64ac1e, 23d57f06
    {
        var ex = new NpgsqlException("Failed to connect to 192.168.1.36:5432", new TimeoutException("Timeout during connection attempt"));

        DbErrors.Classify(ex).Should().Be(DbErrorKind.Timeout);
    }

    [Fact]
    public void ReadTimeout_IsTimeout() // 5ea8af73, 8be8b38d
    {
        var ex = new NpgsqlException("Exception while reading from stream", new TimeoutException("Timeout during reading attempt"));

        DbErrors.Classify(ex).Should().Be(DbErrorKind.Timeout);
    }

    [Fact]
    public void TooManyClients_IsBusy() // c0169d97
    {
        DbErrors.Classify(Postgres(PostgresErrorCodes.TooManyConnections)).Should().Be(DbErrorKind.Busy);
    }

    [Theory]
    [InlineData(PostgresErrorCodes.AdminShutdown)]
    [InlineData(PostgresErrorCodes.CannotConnectNow)]
    [InlineData(PostgresErrorCodes.ConnectionFailure)]
    public void ServerShuttingDownOrStarting_IsUnreachable(string sqlState)
    {
        DbErrors.Classify(Postgres(sqlState)).Should().Be(DbErrorKind.Unreachable);
    }

    [Fact]
    public void ConnectionResetWhileReading_IsUnreachable()
    {
        var ex = new NpgsqlException("Exception while reading from stream", new IOException("Unable to read data from the transport connection"));

        DbErrors.Classify(ex).Should().Be(DbErrorKind.Unreachable);
    }

    [Fact]
    public void WrappedInAggregate_IsStillClassified() // 19e3ce40: unobserved task
    {
        var ex = new AggregateException(new NpgsqlException("Failed to connect", new SocketException((int)SocketError.HostUnreachable)));

        DbErrors.IsTransient(ex).Should().BeTrue();
    }

    [Theory]
    [InlineData(PostgresErrorCodes.UndefinedTable)]
    [InlineData(PostgresErrorCodes.SyntaxError)]
    [InlineData(PostgresErrorCodes.InvalidPassword)]
    public void SqlBugsAndConfiguration_AreNotTransient(string sqlState)
    {
        DbErrors.IsTransient(Postgres(sqlState)).Should().BeFalse();
    }

    [Fact]
    public void TimeoutOutsideNpgsql_IsNotADatabaseProblem()
    {
        DbErrors.IsTransient(new TimeoutException()).Should().BeFalse();
        DbErrors.IsTransient(new HttpRequestException("x", new SocketException((int)SocketError.TimedOut))).Should().BeFalse();
    }

    [Fact]
    public void OrdinaryBugs_AreNotTransient()
    {
        DbErrors.IsTransient(new NullReferenceException()).Should().BeFalse();
        DbErrors.IsTransient(new InvalidCastException()).Should().BeFalse();
        DbErrors.IsTransient(null).Should().BeFalse();
    }

    [Theory]
    [InlineData(DbErrorKind.Unreachable)]
    [InlineData(DbErrorKind.Busy)]
    [InlineData(DbErrorKind.Timeout)]
    public void EveryKind_HasAMessage(DbErrorKind kind)
    {
        DbErrors.UserMessage(kind).Should().NotBeNullOrWhiteSpace();
    }
}

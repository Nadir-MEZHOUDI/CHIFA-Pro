using System.Net.Sockets;
using CHIFA.Pro.Helpers;
using Npgsql;

namespace CHIFA.Tests.Helpers;

public class DbConnectionRulesTests
{
    [Theory]
    [InlineData("localhost", "localhost")]
    [InlineData("  192.168.1.36 ", "192.168.1.36")]
    [InlineData("PC-SERVEUR", "PC-SERVEUR")]
    [InlineData("192.168.1.36:5432", "192.168.1.36:5432")]
    public void ValidHost_IsTrimmedAndAccepted(string input, string expected)
    {
        DbConnectionRules.TryNormalizeHost(input, out var host, out var error).Should().BeTrue();
        host.Should().Be(expected);
        error.Should().BeNull();
    }

    [Fact]
    public void UncPath_IsRejected_WithTheHostAsSuggestion()
    {
        DbConnectionRules.TryNormalizeHost(@"\\100.107.7.123\D\CHIFA_OFFICINE", out _, out var error).Should().BeFalse();
        error.Should().Contain("chemin de dossier").And.Contain("« 100.107.7.123 »");
    }

    [Theory]
    [InlineData(@"D:\CHIFA_OFFICINE")]
    [InlineData("//serveur/partage")]
    [InlineData("PC SERVEUR")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void InvalidHost_IsRejected(string? input)
    {
        DbConnectionRules.TryNormalizeHost(input, out _, out var error).Should().BeFalse();
        error.Should().NotBeNullOrWhiteSpace();
    }

    public static TheoryData<Exception> ExpectedErrors =>
    [
        new NpgsqlException("Failed to connect to 192.168.1.36:5432", new TimeoutException("Timeout during connection attempt")),
        new NpgsqlException("Hôte inconnu.", new SocketException(11001)),
        new NpgsqlException(@"Failed to connect to \\100.107.7.123\.s.PGSQL.5432", new SocketException(10022)),
        new PostgresException("authentification par mot de passe échouée", "FATAL", "FATAL", PostgresErrorCodes.InvalidPassword),
        new PostgresException("le système de bases de données se lance", "FATAL", "FATAL", PostgresErrorCodes.CannotConnectNow),
        new SocketException(10061),
    ];

    [Theory]
    [MemberData(nameof(ExpectedErrors))]
    public void ConnectivityAndConfigErrors_AreExpected(Exception ex) =>
        DbConnectionRules.IsExpectedConnectionError(ex).Should().BeTrue();

    [Fact]
    public void OtherErrors_AreReported()
    {
        DbConnectionRules.IsExpectedConnectionError(new InvalidOperationException()).Should().BeFalse();
        DbConnectionRules.IsExpectedConnectionError(new PostgresException("syntax error", "ERROR", "ERROR", PostgresErrorCodes.SyntaxError)).Should().BeFalse();
    }
}

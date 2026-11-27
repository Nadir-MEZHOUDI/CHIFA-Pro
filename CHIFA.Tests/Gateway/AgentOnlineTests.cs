using CHIFA.Gateway.Features.Agents;

namespace CHIFA.Tests.Gateway;

public class AgentOnlineTests
{
    private static readonly DateTime Start = new(2026, 11, 27, 8, 0, 0, DateTimeKind.Utc);
    private static readonly AgentOnlineRules Rules = new(
        MinOffline: TimeSpan.FromMinutes(5),
        Cooldown: TimeSpan.FromMinutes(60),
        StartupGrace: TimeSpan.FromMinutes(3),
        StableDelay: TimeSpan.FromSeconds(30));

    private readonly Guid _pharmacy = Guid.NewGuid();
    private readonly AgentOnlineTracker _tracker = new(Start, Rules);

    private static DateTime At(double minutes) => Start.AddMinutes(minutes);

    /// <summary>Connects at <paramref name="minutes"/> and runs the check scheduled for that connect.</summary>
    private TimeSpan? Reconnect(double minutes, DateTime? lastSeenAt = null)
    {
        _tracker.Connected(_pharmacy, At(minutes), lastSeenAt);
        return _tracker.CheckBackOnline(_pharmacy, At(minutes) + Rules.StableDelay);
    }

    private void Online(double minutes = 1) => Reconnect(minutes).Should().BeNull();

    [Fact]
    public void Short_drop_sends_nothing()
    {
        Online();
        _tracker.Disconnected(_pharmacy, At(10));

        Reconnect(11).Should().BeNull();
    }

    [Fact]
    public void Outage_then_reconnect_sends_once_with_its_length()
    {
        Online();
        _tracker.Disconnected(_pharmacy, At(10));

        Reconnect(145).Should().Be(TimeSpan.FromMinutes(135));
    }

    [Fact]
    public void Handover_to_a_newer_agent_sends_nothing()
    {
        Online();

        // The Windows service connects; the registry drops the app's connection, which is not reported as a disconnect.
        Reconnect(30).Should().BeNull();
    }

    [Fact]
    public void Handover_with_the_old_agent_leaving_first_sends_nothing()
    {
        Online();
        _tracker.Disconnected(_pharmacy, At(30));

        Reconnect(30.1).Should().BeNull();
    }

    [Fact]
    public void Reconnects_right_after_the_gateway_started_send_nothing()
    {
        // Offline since yesterday as far as the database knows, but every agent reconnects after a deploy.
        Reconnect(0.5, lastSeenAt: Start.AddDays(-1)).Should().BeNull();
    }

    [Fact]
    public void Pharmacy_offline_across_a_gateway_restart_is_reported_from_its_last_seen()
    {
        Reconnect(60, lastSeenAt: Start.AddMinutes(-30)).Should().Be(TimeSpan.FromMinutes(90));
    }

    [Fact]
    public void First_connection_ever_sends_nothing()
    {
        Reconnect(60, lastSeenAt: null).Should().BeNull();
    }

    [Fact]
    public void At_most_one_notification_per_cooldown()
    {
        Online();
        _tracker.Disconnected(_pharmacy, At(10));
        Reconnect(20).Should().NotBeNull();

        _tracker.Disconnected(_pharmacy, At(25));
        Reconnect(40).Should().BeNull();

        _tracker.Disconnected(_pharmacy, At(50));
        Reconnect(90).Should().Be(TimeSpan.FromMinutes(40));
    }

    [Fact]
    public void Flapping_reconnect_waits_until_stable_and_keeps_the_outage_start()
    {
        Online();
        _tracker.Disconnected(_pharmacy, At(10));

        _tracker.Connected(_pharmacy, At(60), null);
        _tracker.Disconnected(_pharmacy, At(60.1));
        _tracker.CheckBackOnline(_pharmacy, At(60) + Rules.StableDelay).Should().BeNull();

        Reconnect(61).Should().Be(TimeSpan.FromMinutes(51));
    }

    [Fact]
    public void Check_of_a_replaced_connection_defers_to_the_newer_one()
    {
        Online();
        _tracker.Disconnected(_pharmacy, At(10));

        _tracker.Connected(_pharmacy, At(60), null);
        _tracker.Connected(_pharmacy, At(60.4), null);
        _tracker.CheckBackOnline(_pharmacy, At(60) + Rules.StableDelay).Should().BeNull();

        _tracker.CheckBackOnline(_pharmacy, At(60.4) + Rules.StableDelay).Should().NotBeNull();
    }

    [Theory]
    [InlineData(0.2, "1 min")]
    [InlineData(45, "45 min")]
    [InlineData(120, "2 h")]
    [InlineData(135, "2 h 15")]
    [InlineData(1440, "1 j")]
    [InlineData(1440 * 3 + 250, "3 j 4 h")]
    public void Duration_is_written_in_french(double minutes, string expected)
    {
        AgentOnlineTracker.FormatDuration(TimeSpan.FromMinutes(minutes)).Should().Be(expected);
    }
}

using CHIFA.Agent;
using CHIFA.Contract;
using Microsoft.AspNetCore.SignalR.Client;

namespace CHIFA.Tests.Gateway;

public class AgentTests
{
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 2)]
    [InlineData(4, 30)]
    [InlineData(5, 60)]
    [InlineData(10_000, 60)]
    public void Retry_policy_never_gives_up_and_waits_at_most_a_minute(long previousRetries, int expectedSeconds)
    {
        var delay = new ForeverRetryPolicy().NextRetryDelay(new RetryContext { PreviousRetryCount = previousRetries });

        delay.Should().NotBeNull();
        delay!.Value.Should().BeGreaterThanOrEqualTo(TimeSpan.FromSeconds(expectedSeconds))
            .And.BeLessThan(TimeSpan.FromSeconds(expectedSeconds + 1));
    }

    [Theory]
    [InlineData(-5, 10_000, 0, PageQuery.MaxTake)]
    [InlineData(20, 0, 20, 1)]
    [InlineData(20, 25, 20, 25)]
    public void Page_query_is_clamped_before_reaching_the_database(int skip, int take, int expectedSkip, int expectedTake)
    {
        var query = new PageQuery(null, null, Skip: skip, Take: take);

        query.SafeSkip.Should().Be(expectedSkip);
        query.SafeTake.Should().Be(expectedTake);
    }

    [Fact]
    public void Missing_from_keeps_the_default_period_start()
    {
        var period = AgentHandlers.ToPeriod(new PeriodQuery(null, new DateTime(2026, 11, 22)));

        period.From.Should().Be(DateTime.Today.AddYears(-2));
        period.To.Should().Be(new DateTime(2026, 11, 22));
    }
}

namespace CHIFA.Tests.Services;

internal static class Pages
{
    /// <summary>Reads every page, plus one past the end; each must carry the full total.</summary>
    public static async Task<List<T>> ReadAllAsync<T>(Func<int, int, ValueTask<(int Total, List<T> Items)>> page, int expectedTotal, int take)
    {
        var all = new List<T>();
        for (var skip = 0; skip < expectedTotal; skip += take)
        {
            var (total, items) = await page(skip, take);
            total.Should().Be(expectedTotal);
            items.Should().HaveCount(Math.Min(take, expectedTotal - skip));
            all.AddRange(items);
        }

        var (pastTotal, pastItems) = await page(expectedTotal + take, take);
        pastTotal.Should().Be(expectedTotal);
        pastItems.Should().BeEmpty();
        return all;
    }
}

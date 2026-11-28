using LinqToDB.Async;

namespace CHIFA.Services.Helpers;

public static class PagingExtensions
{
    /// <summary>
    /// One page of an ordered query and the total row count, in one round trip: COUNT(*) OVER () is
    /// computed before OFFSET/LIMIT, so the filter (often a GROUP BY over facture) runs once.
    /// </summary>
    public static async ValueTask<(int Total, List<T> Items)> ToPageAsync<T>(this IQueryable<T> ordered, int skip, int take,
        CancellationToken cancellationToken = default)
    {
        var rows = await ordered
            .Select(x => new { Item = x, Total = Sql.Ext.Count().Over().ToValue() })
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        if (rows.Count > 0)
            return (rows[0].Total, rows.Select(r => r.Item).ToList());

        // A page past the end returns no row to carry the total.
        var total = skip == 0 ? 0 : await ordered.CountAsync(cancellationToken).ConfigureAwait(false);
        return (total, []);
    }
}

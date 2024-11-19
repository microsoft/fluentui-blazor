using Microsoft.OData.Client;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.ODataAdapter;

internal class ODataAsyncQueryExecutor : IAsyncQueryExecutor
{
    public bool IsSupported<T>(IQueryable<T> queryable) => queryable.Provider is DataServiceQueryProvider;

    public async Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        => (int)(await ExecuteAsync(((DataServiceQuery<T>)queryable.Take(0)).IncludeCount(), cancellationToken)).Count;

    public async Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        => [.. await ExecuteAsync(queryable, cancellationToken)];

    private static async Task<QueryOperationResponse<T>> ExecuteAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
    {
        return (QueryOperationResponse<T>)await ((DataServiceQuery<T>)queryable).ExecuteAsync(cancellationToken);
    }
}

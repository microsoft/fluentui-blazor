// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Runtime.ExceptionServices;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.OData.Client;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.ODataAdapter;

/// <summary>
/// An <see cref="IAsyncQueryExecutor"/> implementation for Microsoft.OData.Client.
/// </summary>
internal class ODataAsyncQueryExecutor : IAsyncQueryExecutor
{
    /// <inheritdoc />
    public bool IsSupported<T>(IQueryable<T> queryable) => queryable.Provider is DataServiceQueryProvider;

    /// <inheritdoc />
    public async Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
    {
        var response = await ExecuteAsync(((DataServiceQuery<T>)queryable.Take(0)).IncludeCount(), cancellationToken);
        return response is null ? default : checked((int)response.Count);
    }

    /// <inheritdoc />
    public async Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
    {
        var response = await ExecuteAsync(queryable, cancellationToken);
        return response is null ? default! : [.. response];
    }

    private static async Task<QueryOperationResponse<T>> ExecuteAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            return (QueryOperationResponse<T>)await ((DataServiceQuery<T>)queryable).ExecuteAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            return default!;
        }
        catch (DataServiceQueryException ex) when (ex.InnerException is OperationCanceledException oce)
        {
            ExceptionDispatchInfo.Capture(oce).Throw();
            throw; // unreachable; satisfies compiler
        }
    }
}

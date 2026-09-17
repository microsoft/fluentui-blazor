// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter;

/// <summary>
/// An <see cref="IAsyncQueryExecutor"/> implementation for Entity Framework Core.
/// </summary>
internal class EntityFrameworkAsyncQueryExecutor(Func<Exception, bool>? ignoreException = null) : IAsyncQueryExecutor, IDisposable
{
    private readonly SemaphoreSlim _lock = new(1);

    /// <inheritdoc />
    public bool IsSupported<T>(IQueryable<T> queryable)
        => queryable.Provider is IAsyncQueryProvider;

    /// <inheritdoc />
    /// <inheritdoc />
    public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        => ExecuteAsync(() => queryable.CountAsync(cancellationToken), cancellationToken);

    /// <inheritdoc />
    public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        => ExecuteAsync(() => queryable.ToArrayAsync(cancellationToken), cancellationToken);

    private async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken)
    {
        try
        {
            await _lock.WaitAsync(cancellationToken);
            try
            {
                return await operation();
            }
            finally
            {
                _lock.Release();
            }
        }
        catch (ObjectDisposedException)
        {
            return typeof(TResult).IsArray
                 ? (TResult)(object)Array.CreateInstance(typeof(TResult).GetElementType()!, 0)
                 : default!;
        }
        catch (Exception ex) when (ignoreException?.Invoke(ex) == true)
        {
            return typeof(TResult).IsArray
                 ? (TResult)(object)Array.CreateInstance(typeof(TResult).GetElementType()!, 0)
                 : default!;
        }
    }

    void IDisposable.Dispose() => _lock.Dispose();
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
    public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        => ExecuteAsync(() => queryable.CountAsync(cancellationToken));

    /// <inheritdoc />
    public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        => ExecuteAsync(() => queryable.ToArrayAsync(cancellationToken));

    private async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation)
    {
        try
        {
            await _lock.WaitAsync();

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
            return default!;
        }
        catch (Exception ex) when (ignoreException?.Invoke(ex) == true)
        {
            return default!;
        }
    }

    void IDisposable.Dispose() => _lock.Dispose();
}

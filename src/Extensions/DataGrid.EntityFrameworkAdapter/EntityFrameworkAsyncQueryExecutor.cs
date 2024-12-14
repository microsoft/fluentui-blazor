using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter;

internal class EntityFrameworkAsyncQueryExecutor : IAsyncQueryExecutor, IEntityFrameworkAsyncQueryExecutor, IDisposable
{
    private readonly SemaphoreSlim _lock = new(1);

    public bool IsSupported<T>(IQueryable<T> queryable)
        => queryable.Provider is IAsyncQueryProvider;

    public Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        => ExecuteAsync(() => queryable.CountAsync(cancellationToken));

    public Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken)
        => ExecuteAsync(() => queryable.ToArrayAsync(cancellationToken));

    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation)
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
    }

    void IDisposable.Dispose() => _lock.Dispose();
}

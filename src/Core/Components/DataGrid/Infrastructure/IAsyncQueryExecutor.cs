namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

/// <summary>
/// Provides methods for asynchronous evaluation of queries against an <see cref="IQueryable{T}" />.
/// </summary>
public interface IAsyncQueryExecutor
{
    /// <summary>
    /// Determines whether the <see cref="IQueryable{T}" /> is supported by this <see cref="IAsyncQueryExecutor"/> type.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    /// <param name="queryable">An <see cref="IQueryable{T}" /> instance.</param>
    /// <returns>True if this <see cref="IAsyncQueryExecutor"/> instance can perform asynchronous queries for the supplied <paramref name="queryable"/>, otherwise false.</returns>
    bool IsSupported<T>(IQueryable<T> queryable);

    /// <summary>
    /// Asynchronously counts the items in the <see cref="IQueryable{T}" />, if supported.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    /// <param name="queryable">An <see cref="IQueryable{T}" /> instance.</param>
    /// <param name="cancellationToken">An <see cref="CancellationToken" /> instance.</param>
    /// <returns>The number of items in <paramref name="queryable"/>.</returns>.
    Task<int> CountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously materializes the <see cref="IQueryable{T}" /> as an array, if supported.
    /// </summary>
    /// <typeparam name="T">The data type.</typeparam>
    /// <param name="cancellationToken">An <see cref="CancellationToken" /> instance.</param>
    /// <param name="queryable">An <see cref="IQueryable{T}" /> instance.</param>
    /// <returns>The items in the <paramref name="queryable"/>.</returns>.
    Task<T[]> ToArrayAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);
}

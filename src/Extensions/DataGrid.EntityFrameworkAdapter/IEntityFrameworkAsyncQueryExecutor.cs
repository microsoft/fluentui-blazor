namespace Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

/// <summary>
/// Defines methods for executing asynchronous operations serially within an Entity Framework database context.
/// </summary>
public interface IEntityFrameworkAsyncQueryExecutor
{
    /// <summary>
    /// Schedules an asynchronous operation to be executed serially against the database context associated with the grid.
    /// </summary>
    /// <typeparam name="TResult">The type of the result returned by the operation.</typeparam>
    /// <param name="operation">A function representing the asynchronous operation to execute.</param>
    /// <returns>A task representing the result of the operation.</returns>
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation);
}
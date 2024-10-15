using Microsoft.FluentUI.AspNetCore.Components.DataGrid.OData.Client;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to configure <see cref="IAsyncQueryExecutor"/> on a <see cref="IServiceCollection"/>.
/// </summary>
public static class ODataAdapterServiceCollectionExtensions
{
    /// <summary>
    /// Registers an OData aware implementation of <see cref="IAsyncQueryExecutor"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    public static void AddDataGridODataAdapter(this IServiceCollection services)
    {
        services.AddScoped<IAsyncQueryExecutor, ODataAsyncQueryExecutor>();
    }
}

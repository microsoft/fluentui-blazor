// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// Provides extension methods to configure <see cref="IAsyncQueryExecutor"/> on a <see cref="IServiceCollection"/>.
/// </summary>
public static class EntityFrameworkAdapterServiceCollectionExtensions
{
    /// <summary>
    /// Registers an Entity Framework aware implementation of <see cref="IAsyncQueryExecutor"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="ignoreException">A function to determine whether to ignore exceptions.</param>
    public static void AddDataGridEntityFrameworkAdapter(this IServiceCollection services, Func<Exception, bool>? ignoreException = null)
    {
        services.AddScoped<IAsyncQueryExecutor, EntityFrameworkAsyncQueryExecutor>(sp => new EntityFrameworkAsyncQueryExecutor(ignoreException));
    }
}

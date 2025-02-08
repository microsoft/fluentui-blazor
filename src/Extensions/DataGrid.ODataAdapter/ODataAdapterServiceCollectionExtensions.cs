// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.DataGrid.Infrastructure;
using Microsoft.FluentUI.AspNetCore.Components.DataGrid.ODataAdapter;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

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

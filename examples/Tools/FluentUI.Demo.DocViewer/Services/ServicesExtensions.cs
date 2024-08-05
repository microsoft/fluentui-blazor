// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace FluentUI.Demo.DocViewer.Services;

/// <summary>
/// Extension methods for the services.
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// Add the DocViewer services to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IServiceCollection AddDocViewer(this IServiceCollection services, Action<DocViewerOptions> options)
    {
        var configuration = new DocViewerOptions();
        options.Invoke(configuration);

        services.AddScoped<StaticAssetService>();
        services.AddScoped<DocViewerService>(factory =>
        {
            return new DocViewerService(configuration);
        });

        return services;
    }
}

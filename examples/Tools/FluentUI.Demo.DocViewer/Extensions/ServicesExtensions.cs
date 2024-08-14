// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Components.ConsoleLog;
using FluentUI.Demo.DocViewer.Services;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace FluentUI.Demo.DocViewer;
#pragma warning restore IDE0130 // Namespace does not match folder structure

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

        // Add Console Log Service
        if (configuration.EnableConsoleLogProvider)
        {
            services.AddScoped<ConsoleLogService>();
        }

        // Add DocViewer Service
        services.AddScoped<DocViewerService>(factory =>
        {
            return new DocViewerService(configuration);
        });

        return services;
    }
}

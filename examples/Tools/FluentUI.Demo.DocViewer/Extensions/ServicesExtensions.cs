// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using FluentUI.Demo.DocViewer.Components.ConsoleLog;
using FluentUI.Demo.DocViewer.Models;
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

    /// <summary>
    /// Load the summaries from the CodeComments.json file.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="jsonFile"></param>
    /// <returns></returns>
    public static async Task<ApiDocSummary> LoadSummariesAsync(this HttpClient httpClient, string jsonFile)
    {
        // Read CodeComments.json
        try
        {
            var json = await httpClient.GetStringAsync(jsonFile);
            return new ApiDocSummary()
            {
                Items = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json)
            };
        }
        catch (Exception ex)
        {
            return new ApiDocSummary()
            {
                Items = new Dictionary<string, Dictionary<string, string>>
                {
                    ["ERROR"] = new Dictionary<string, string> { [$"{jsonFile} cannot be loaded"] = ex.Message },
                }
            };
        }
    }
}

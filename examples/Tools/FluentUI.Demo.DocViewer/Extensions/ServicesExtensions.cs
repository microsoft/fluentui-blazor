// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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
    /// Load the summaries from one or more api-comments.json files.
    /// When multiple files are provided, their contents are merged into a single <see cref="ApiDocSummary.Items"/> dictionary.
    /// If the same key exists in several files, entries from later files are merged into the earlier ones
    /// (inner keys from later files override the previous ones).
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="jsonFiles">One or more json files to load.</param>
    /// <returns></returns>
    public static async Task<ApiDocSummary> LoadSummariesAsync(this HttpClient httpClient, params string[] jsonFiles)
    {
        var summary = new ApiDocSummary()
        {
            Items = new Dictionary<string, Dictionary<string, string>>(),
        };

        if (jsonFiles is null || jsonFiles.Length == 0)
        {
            return summary;
        }

        foreach (var jsonFile in jsonFiles)
        {
            try
            {
                var json = await httpClient.GetStringAsync(jsonFile);
                var items = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

                if (items is null)
                {
                    continue;
                }

                foreach (var (key, value) in items)
                {
                    if (summary.Items.TryGetValue(key, out var existing))
                    {
                        foreach (var (innerKey, innerValue) in value)
                        {
                            existing[innerKey] = innerValue;
                        }
                    }
                    else
                    {
                        summary.Items[key] = new Dictionary<string, string>(value);
                    }
                }
            }
            catch (Exception ex)
            {
                if (!summary.Items.TryGetValue("ERROR", out var errors))
                {
                    errors = new Dictionary<string, string>();
                    summary.Items["ERROR"] = errors;
                }

                errors[$"{jsonFile} cannot be loaded"] = ex.Message;
            }
        }

        return summary;
    }
}

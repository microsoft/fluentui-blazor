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
    /// Discovers all API documentation JSON files via an index file and merges them into
    /// a single <see cref="ApiDocSummary"/>.
    /// The index file (default: <c>/api-index.json</c>) is a JSON array of filenames,
    /// e.g. <c>["api-comments.json","api-comments-charts.json"]</c>.
    /// It is generated automatically at build time from all <c>api-*.json</c> files in <c>wwwroot</c>.
    /// </summary>
    /// <param name="httpClient"></param>
    /// <param name="indexUrl">URL of the JSON index file listing all api-*.json filenames.</param>
    /// <returns></returns>
    public static async Task<ApiDocSummary> LoadSummariesAsync(this HttpClient httpClient, string indexUrl = "/api-index.json")
    {
        var merged = new Dictionary<string, Dictionary<string, string>>();

        // Step 1: fetch the index to discover which api-*.json files exist
        IEnumerable<string> jsonFiles;
        try
        {
            var indexJson = await httpClient.GetStringAsync(indexUrl);
            jsonFiles = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<string>>(indexJson)
                        ?? [];
        }
        catch (Exception ex)
        {
            return new ApiDocSummary
            {
                Items = new Dictionary<string, Dictionary<string, string>>
                {
                    ["ERROR"] = new Dictionary<string, string> { [$"{indexUrl} cannot be loaded"] = ex.Message }
                }
            };
        }

        // Step 2: load and merge each discovered file
        foreach (var fileName in jsonFiles)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                continue;
            }

            var trimmedFileName = fileName.Trim();

            // Support both bare filenames ("api-comments.json") and absolute paths ("/api-comments.json")
            var fileUrl = trimmedFileName.StartsWith('/') ? trimmedFileName : $"/{trimmedFileName}";

            try
            {
                var json = await httpClient.GetStringAsync(fileUrl);
                var items = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

                if (items is not null)
                {
                    foreach (var (key, value) in items)
                    {
                        if (merged.TryGetValue(key, out var existing))
                        {
                            foreach (var (memberKey, memberValue) in value)
                            {
                                existing[memberKey] = memberValue;
                            }
                        }
                        else
                        {
                            merged[key] = value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                merged[$"ERROR:{fileUrl}"] = new Dictionary<string, string> { [$"{fileUrl} cannot be loaded"] = ex.Message };
            }
        }

        return new ApiDocSummary { Items = merged };
    }
}

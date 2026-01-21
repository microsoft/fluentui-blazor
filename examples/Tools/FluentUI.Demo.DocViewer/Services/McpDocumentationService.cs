// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;
using FluentUI.Demo.DocViewer.Models.Mcp;

namespace FluentUI.Demo.DocViewer.Services;

/// <summary>
/// Service to load and cache MCP documentation.
/// This is a static service designed to be used for the lifetime of the application.
/// </summary>
public static class McpDocumentationService
{
    private static McpDocumentation? _cachedDocumentation;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    /// Gets the cached MCP documentation, or null if not loaded.
    /// </summary>
    public static McpDocumentation? Cached => _cachedDocumentation;

    /// <summary>
    /// Loads the MCP documentation from the specified URL.
    /// Uses SemaphoreSlim for thread-safe async operations.
    /// </summary>
    /// <param name="httpClient">The HTTP client to use.</param>
    /// <param name="url">The URL of the MCP documentation JSON file.</param>
    /// <returns>The loaded MCP documentation, or null if loading fails.</returns>
    public static async Task<McpDocumentation?> LoadAsync(HttpClient httpClient, string url)
    {
        if (_cachedDocumentation is not null)
        {
            return _cachedDocumentation;
        }

        await _semaphore.WaitAsync();
        try
        {
            if (_cachedDocumentation is not null)
            {
                return _cachedDocumentation;
            }

            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                _cachedDocumentation = await response.Content.ReadFromJsonAsync<McpDocumentation>(options).ConfigureAwait(false);
            }
        }
        catch
        {
            // Silently fail and return null
        }
        finally
        {
            _semaphore.Release();
        }

        return _cachedDocumentation;
    }

    /// <summary>
    /// Gets tools filtered by class name.
    /// </summary>
    /// <param name="filter">Optional class name filter.</param>
    /// <returns>List of matching tools.</returns>
    public static IEnumerable<McpTool> GetTools(string? filter = null)
    {
        if (_cachedDocumentation is null)
        {
            return [];
        }

        var tools = _cachedDocumentation.Tools.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            tools = tools.Where(t =>
                t.ClassName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                t.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }

        return tools.OrderBy(t => t.ClassName).ThenBy(t => t.Name);
    }

    /// <summary>
    /// Gets resources filtered by class name.
    /// </summary>
    /// <param name="filter">Optional class name filter.</param>
    /// <returns>List of matching resources.</returns>
    public static IEnumerable<McpResource> GetResources(string? filter = null)
    {
        if (_cachedDocumentation is null)
        {
            return [];
        }

        var resources = _cachedDocumentation.Resources.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            resources = resources.Where(r =>
                r.ClassName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                r.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }

        return resources.OrderBy(r => r.ClassName).ThenBy(r => r.Name);
    }

    /// <summary>
    /// Gets prompts filtered by class name.
    /// </summary>
    /// <param name="filter">Optional class name filter.</param>
    /// <returns>List of matching prompts.</returns>
    public static IEnumerable<McpPrompt> GetPrompts(string? filter = null)
    {
        if (_cachedDocumentation is null)
        {
            return [];
        }

        var prompts = _cachedDocumentation.Prompts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(filter))
        {
            prompts = prompts.Where(p =>
                p.ClassName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                p.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }

        return prompts.OrderBy(p => p.ClassName).ThenBy(p => p.Name);
    }
}

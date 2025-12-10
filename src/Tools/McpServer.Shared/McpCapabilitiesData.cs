// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared.Models;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Shared;

/// <summary>
/// Provides access to MCP capabilities data discovered via reflection.
/// This class dynamically discovers tools, prompts, and resources from the MCP Server assembly.
/// </summary>
public static class McpCapabilitiesData
{
    private static readonly object _lock = new();
    private static McpSummary? _cachedSummary;
    private static Assembly? _mcpServerAssembly;
    private static Func<McpSummary>? _summaryProvider;

    /// <summary>
    /// Initializes the capabilities data with the MCP Server assembly.
    /// Use this when running in a context where the MCP Server assembly is available.
    /// </summary>
    /// <param name="mcpServerAssembly">The assembly containing MCP tools, prompts, and resources.</param>
    public static void Initialize(Assembly mcpServerAssembly)
    {
        lock (_lock)
        {
            _mcpServerAssembly = mcpServerAssembly;
            _summaryProvider = null;
            _cachedSummary = null;
        }
    }

    /// <summary>
    /// Initializes the capabilities data with a custom summary provider.
    /// Use this for scenarios like WebAssembly where reflection on the server assembly isn't possible.
    /// </summary>
    /// <param name="summaryProvider">A function that provides the MCP summary.</param>
    public static void Initialize(Func<McpSummary> summaryProvider)
    {
        lock (_lock)
        {
            _summaryProvider = summaryProvider;
            _mcpServerAssembly = null;
            _cachedSummary = null;
        }
    }

    /// <summary>
    /// Gets all MCP tools available in the server.
    /// </summary>
    public static IReadOnlyList<McpToolInfo> Tools => GetSummary().Tools;

    /// <summary>
    /// Gets all MCP prompts available in the server.
    /// </summary>
    public static IReadOnlyList<McpPromptInfo> Prompts => GetSummary().Prompts;

    /// <summary>
    /// Gets all MCP resources available in the server.
    /// </summary>
    public static IReadOnlyList<McpResourceInfo> Resources => GetSummary().Resources;

    /// <summary>
    /// Gets a summary of all MCP capabilities.
    /// </summary>
    public static McpSummary GetSummary()
    {
        lock (_lock)
        {
            if (_cachedSummary is not null)
            {
                return _cachedSummary;
            }

            // Try custom provider first
            if (_summaryProvider is not null)
            {
                _cachedSummary = _summaryProvider();
                return _cachedSummary;
            }

            // Try to use provided assembly
            if (_mcpServerAssembly is not null)
            {
                _cachedSummary = McpReflectionService.GetSummary(_mcpServerAssembly);
                return _cachedSummary;
            }

            // Try to find the MCP Server assembly by name
            _mcpServerAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "FluentUI.Mcp.Server");

            if (_mcpServerAssembly is not null)
            {
                _cachedSummary = McpReflectionService.GetSummary(_mcpServerAssembly);
                return _cachedSummary;
            }

            // Return empty summary if nothing available
            return new McpSummary([], [], []);
        }
    }

    /// <summary>
    /// Clears the cached summary, forcing re-discovery on next access.
    /// </summary>
    public static void ClearCache()
    {
        lock (_lock)
        {
            _cachedSummary = null;
        }
    }

    /// <summary>
    /// Gets whether the capabilities data has been initialized.
    /// </summary>
    public static bool IsInitialized
    {
        get
        {
            lock (_lock)
            {
                return _mcpServerAssembly is not null || _summaryProvider is not null;
            }
        }
    }
}

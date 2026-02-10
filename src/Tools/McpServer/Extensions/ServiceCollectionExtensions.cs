// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Extensions;

/// <summary>
/// Extension methods for configuring the MCP server services.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures logging for MCP stdio transport (logs to stderr).
    /// </summary>
    public static IHostApplicationBuilder ConfigureMcpLogging(this IHostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole(options =>
        {
            options.LogToStandardErrorThreshold = LogLevel.Trace;
        });

        return builder;
    }

    /// <summary>
    /// Adds the Fluent UI documentation service.
    /// Uses pre-generated JSON documentation for fast, dependency-free access.
    /// </summary>
    public static IServiceCollection AddFluentUIDocumentation(this IServiceCollection services)
    {
        // Try to find external JSON documentation file (for development)
        var externalJsonPath = JsonDocumentationFinder.Find();

        // Component documentation service using pre-generated JSON
        // Falls back to embedded resource if no external file is found
        services.AddSingleton(_ => new FluentUIDocumentationService(externalJsonPath));

        // Component documentation service (usage guides and Razor examples from Demo.Client)
        services.AddSingleton<ComponentDocumentationService>();

        // Documentation service
        // Excludes the 'mcp' folder
        services.AddSingleton(_ => new DocumentationService(["mcp"]));

        return services;
    }

    /// <summary>
    /// Adds the MCP server with stdio transport, tools, resources, and prompts.
    /// </summary>
    /// <remarks>
    /// <para><strong>Tools</strong> (model-controlled): For dynamic queries like search, details by name.</para>
    /// <para><strong>Resources</strong> (user-controlled): For static context like component lists, categories.</para>
    /// <para><strong>Prompts</strong> (user-controlled): Pre-defined prompt templates for common tasks.</para>
    /// </remarks>
    public static IServiceCollection AddFluentUIMcpServer(this IServiceCollection services)
    {
        services
            .AddMcpServer()
            .WithStdioServerTransport()
            .WithToolsFromAssembly(Assembly.GetExecutingAssembly())
            .WithResourcesFromAssembly(Assembly.GetExecutingAssembly())
            .WithPromptsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}

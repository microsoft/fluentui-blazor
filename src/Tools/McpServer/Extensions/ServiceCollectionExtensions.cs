// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Resources;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Extensions;

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
    /// </summary>
    public static IServiceCollection AddFluentUIDocumentation(this IServiceCollection services)
    {
        // Component documentation service
        services.AddSingleton(_ =>
        {
            var componentsAssembly = typeof(Microsoft.FluentUI.AspNetCore.Components._Imports).Assembly;
            var xmlPath = XmlDocumentationFinder.Find();

            return new FluentUIDocumentationService(componentsAssembly, xmlPath);
        });

        // Documentation guides service (Installation, Migration, Styles, etc.)
        services.AddSingleton<DocumentationGuideService>();

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
            .WithResources<FluentUIResources>()
            .WithResources<ComponentResources>()
            .WithResources<GuideResources>()
            // Component prompts
            .WithPrompts<CreateComponentPrompt>()
            .WithPrompts<ExplainComponentPrompt>()
            .WithPrompts<CompareComponentsPrompt>()
            // Form & data prompts
            .WithPrompts<CreateFormPrompt>()
            .WithPrompts<CreateDataGridPrompt>()
            .WithPrompts<CreateDialogPrompt>()
            .WithPrompts<CreateDrawerPrompt>()
            // Migration & setup prompts
            .WithPrompts<MigrateToV5Prompt>()
            .WithPrompts<SetupProjectPrompt>()
            .WithPrompts<ConfigureThemingPrompt>()
            .WithPrompts<ConfigureLocalizationPrompt>();

        return services;
    }
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tools;

/// <summary>
/// MCP tools for accessing documentation.
/// </summary>
[McpServerToolType]
public class GetDocumentationTools
{
    private readonly DocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetDocumentationTools"/> class.
    /// </summary>
    /// <param name="documentationService">The documentation service.</param>
    public GetDocumentationTools(DocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Lists all available documentation topics.
    /// </summary>
    /// <returns>A formatted string listing all available documentation topics.</returns>
    [McpServerTool]
    [Description("Lists all available GetStarted documentation topics for Fluent UI Blazor. Use this to discover installation guides, migration information, localization, and styling documentation.")]
    public string ListDocumentation()
    {
        var docs = _documentationService.GetAllDocumentation();

        if (docs.Count == 0)
        {
            return "No GetStarted documentation found.";
        }

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Fluent UI Blazor - GetStarted Documentation ({docs.Count} topics)");
        sb.AppendLine();

        foreach (var doc in docs)
        {
            var icon = !string.IsNullOrEmpty(doc.Icon) ? $" 📄" : "";
            sb.AppendLine(CultureInfo.InvariantCulture, $"- **{doc.Title}**{icon}: {TruncateSummary(doc.Summary, 100)}");
        }

        sb.AppendLine();
        sb.AppendLine("Use `GetDocumentationTopic(topicName)` to get detailed information about a specific topic.");

        return sb.ToString();
    }

    /// <summary>
    /// Gets detailed documentation for a specific GetStarted topic.
    /// </summary>
    /// <param name="topicName">The name or route of the topic (e.g., 'Installation', 'Localization', 'Styles', 'MigrationV5').</param>
    /// <returns>The full documentation content for the specified topic.</returns>
    [McpServerTool]
    [Description("Get documentation for a specific GetStarted topic. Use this to get full installation guides, migration instructions, localization setup, or styling documentation.")]
    public string GetDocumentationTopic(
        [Description("The name or route of the topic (e.g., 'Installation', 'Localization', 'Styles', 'Migrating to v5', 'Default Values').")]
        string topicName)
    {
        if (string.IsNullOrWhiteSpace(topicName))
        {
            return "Please provide a topic name. Use ListDocumentation() to see available topics.";
        }

        var doc = _documentationService.GetDocumentation(topicName);

        if (doc == null)
        {
            var availableTopics = _documentationService.GetTopics();
            var topicList = string.Join(", ", availableTopics.Take(5));
            return $"Topic '{topicName}' was not found. Available topics include: {topicList}. Use ListDocumentation() for the complete list.";
        }

        return doc.Content;
    }

    /// <summary>
    /// Searches for GetStarted documentation by keyword.
    /// </summary>
    /// <param name="searchTerm">The keyword to search for in documentation titles and content.</param>
    /// <returns>A formatted string listing matching documentation topics.</returns>
    [McpServerTool]
    [Description("Searches documentation by keyword. Use this to find specific information about installation, configuration, migration, or styling.")]
    public string SearchDocumentation(
        [Description("The keyword to search for (e.g., 'install', 'migrate', 'localize', 'style', 'reboot', 'service').")]
        string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return "Please provide a search term.";
        }

        var docs = _documentationService.SearchDocumentation(searchTerm);

        if (docs.Count == 0)
        {
            return $"No documentation found matching '{searchTerm}'. Try different keywords or use ListDocumentation() to see all available topics.";
        }

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Search Results for '{searchTerm}' ({docs.Count} found)");
        sb.AppendLine();

        foreach (var doc in docs)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"## {doc.Title}");
            sb.AppendLine(doc.Summary);
            sb.AppendLine();
        }

        sb.AppendLine("Use `GetDocumentationTopic(topicName)` to get the full documentation for a specific topic.");

        return sb.ToString();
    }

    /// <summary>
    /// Gets migration documentation for upgrading to Fluent UI Blazor v5.
    /// </summary>
    /// <returns>A formatted string with migration documentation.</returns>
    [McpServerTool]
    [Description("Gets migration documentation for upgrading to Fluent UI Blazor v5. Use this to understand breaking changes, component updates, and migration steps.")]
    public string GetMigrationGuide()
    {
        var migrationDocs = _documentationService.GetMigrationDocumentation();

        if (migrationDocs.Count == 0)
        {
            return "No migration documentation found.";
        }

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor - Migration Guide to v5");
        sb.AppendLine();
        sb.AppendLine("This guide covers the changes and migration steps for upgrading to Fluent UI Blazor v5.");
        sb.AppendLine();

        // First, show the main migration document if available
        var mainMigration = migrationDocs.FirstOrDefault(d =>
            d.Title.Contains("Migrating to v5", StringComparison.OrdinalIgnoreCase) ||
            d.FileName.Equals("MigrationVersion5", StringComparison.OrdinalIgnoreCase));

        if (mainMigration != null)
        {
            sb.AppendLine("## Overview");
            sb.AppendLine();
            sb.AppendLine(mainMigration.Content);
            sb.AppendLine();
        }

        // List component-specific migration guides
        var componentMigrations = migrationDocs
            .Where(d => d != mainMigration && d.FileName.StartsWith("Migration", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (componentMigrations.Count > 0)
        {
            sb.AppendLine("## Component-Specific Migration Guides");
            sb.AppendLine();

            foreach (var doc in componentMigrations)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"- **{doc.Title}**: {TruncateSummary(doc.Summary, 80)}");
            }

            sb.AppendLine();
            sb.AppendLine("Use `GetDocumentationTopic(topicName)` to get detailed migration information for a specific component.");
        }

        return sb.ToString();
    }

    private static string TruncateSummary(string summary, int maxLength)
    {
        if (string.IsNullOrEmpty(summary))
        {
            return string.Empty;
        }

        if (summary.Length <= maxLength)
        {
            return summary;
        }

        return summary[..(maxLength - 3)] + "...";
    }
}

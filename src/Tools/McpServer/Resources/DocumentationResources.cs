// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Resources;

/// <summary>
/// MCP Resources providing static documentation content for Fluent UI Blazor.
/// These resources are user-selected and provide context for the LLM.
/// </summary>
[McpServerResourceType]
public class DocumentationResources
{
    private readonly DocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationResources"/> class.
    /// </summary>
    public DocumentationResources(DocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Gets the complete list of all documentation topics.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://documentation",
        Name = "documentation",
        Title = "Fluent UI Blazor - Documentation",
        MimeType = "text/markdown")]
    [Description("Complete list of all documentation topics including installation, configuration, migration, and styling guides.")]
    public string GetAllTopics()
    {
        var docs = _documentationService.GetAllDocumentation();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor - Documentation");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Total: {docs.Count} documentation topics");
        sb.AppendLine();
        sb.AppendLine("Available topics (use `fluentui://documentation/{topic}` to access):");
        sb.AppendLine();

        foreach (var doc in docs)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"## {doc.Title}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"**URI:** `fluentui://documentation/{doc.FileName.ToLowerInvariant()}`");

            if (!string.IsNullOrEmpty(doc.Route))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"**Route:** {doc.Route}");
            }

            sb.AppendLine();
            sb.AppendLine(doc.Summary);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets documentation for a specific documentation topic by name.
    /// </summary>
    /// <param name="topic">The topic name (e.g., 'installation', 'localization', 'styles', 'migrationversion5', 'defaultvalues').</param>
    [McpServerResource(
        UriTemplate = "fluentui://documentation/{topic}",
        Name = "documentation-topic",
        Title = "Topic Documentation",
        MimeType = "text/markdown")]
    [Description("Detailed documentation for a specific documentation topic. Use topic names like 'installation', 'localization', 'styles', 'migrationversion5', 'defaultvalues'.")]
    public string GetTopic(string topic)
    {
        var doc = _documentationService.GetDocumentation(topic);

        if (doc == null)
        {
            var availableTopics = _documentationService.GetTopics();
            var topicList = string.Join(", ", availableTopics.Take(10).Select(t => $"'{t}'"));

            return $"# Topic Not Found\n\nTopic '{topic}' was not found.\n\n" +
                   $"Available topics include: {topicList}\n\n" +
                   "Use `fluentui://documentation` to see all available topics.";
        }

        return doc.Content;
    }

    /// <summary>
    /// Gets the complete migration guide documentation.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://documentation/migration",
        Name = "migration",
        Title = "Fluent UI Blazor - Migration Guide to v5",
        MimeType = "text/markdown")]
    [Description("Complete migration guide for upgrading to Fluent UI Blazor v5, including all breaking changes and component updates.")]
    public string GetMigrationGuide()
    {
        var migrationDocs = _documentationService.GetMigrationDocumentation();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor - Migration Guide to v5");
        sb.AppendLine();

        // Get the main migration document first
        var mainDoc = _documentationService.GetDocumentation("Migrating to v5") ??
                      _documentationService.GetDocumentation("MigrationVersion5");

        if (mainDoc != null)
        {
            sb.AppendLine(mainDoc.Content);
            sb.AppendLine();
        }

        // Add component-specific migration info
        var componentMigrations = migrationDocs
            .Where(d => d.FileName.StartsWith("Migration", StringComparison.OrdinalIgnoreCase) &&
                       !d.FileName.Equals("MigrationVersion5", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (componentMigrations.Count > 0)
        {
            sb.AppendLine("## Component-Specific Migrations");
            sb.AppendLine();

            foreach (var doc in componentMigrations)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"### {doc.Title}");
                sb.AppendLine();
                sb.AppendLine(doc.Content);
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}

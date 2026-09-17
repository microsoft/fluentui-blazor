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
/// MCP tools for migrating from Fluent UI Blazor v4 to v5.
/// Provides targeted, component-level migration guidance for AI agents.
/// </summary>
[McpServerToolType]
public class MigrationTools
{
    private readonly MigrationService _migrationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationTools"/> class.
    /// </summary>
    /// <param name="migrationService">The migration service.</param>
    public MigrationTools(MigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    /// <summary>
    /// Gets the general migration overview for upgrading from v4 to v5.
    /// </summary>
    /// <returns>A formatted string with general migration information.</returns>
    [McpServerTool]
    [Description("Gets the general migration overview for upgrading from Fluent UI Blazor v4 to v5. " +
                 "Use this to understand the overall migration process, including CSS bundling changes, " +
                 "JavaScript interop updates, ToAttributeValue() behavior changes, and general guidelines. " +
                 "Call this FIRST before migrating specific components.")]
    public string GetMigrationOverview()
    {
        var overview = _migrationService.GetMigrationOverview();

        if (overview == null)
        {
            return "No general migration documentation found.";
        }

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor - Migration Overview (v4 → v5)");
        sb.AppendLine();
        sb.AppendLine(overview.Content);
        sb.AppendLine();

        // Append a hint about available component migrations
        var componentNames = _migrationService.GetAvailableComponentNames();
        if (componentNames.Count > 0)
        {
            sb.AppendLine("---");
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"**{componentNames.Count} component-specific migration guides are available.**");
            sb.AppendLine("Use `ListComponentMigrations()` to see all available components, ");
            sb.AppendLine("or `GetComponentMigration(componentName)` to get migration details for a specific component.");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Lists all available component migration guides with summaries.
    /// </summary>
    /// <returns>A formatted string listing all component migrations.</returns>
    [McpServerTool]
    [Description("Lists all available component-specific migration guides for upgrading from Fluent UI Blazor v4 to v5. " +
                 "Returns a list of components that have migration documentation, with a brief summary for each. " +
                 "Use this to discover which components have breaking changes or require migration steps.")]
    public string ListComponentMigrations()
    {
        var migrations = _migrationService.GetAllComponentMigrations();

        if (migrations.Count == 0)
        {
            return "No component migration guides found.";
        }

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Component Migration Guides ({migrations.Count} available)");
        sb.AppendLine();
        sb.AppendLine("The following components have migration guides for upgrading from v4 to v5:");
        sb.AppendLine();

        foreach (var migration in migrations)
        {
            var componentName = MigrationService.ExtractComponentName(migration.FileName);
            var summary = TruncateSummary(migration.Summary, 120);
            sb.AppendLine(CultureInfo.InvariantCulture, $"- **{componentName}**: {summary}");
        }

        sb.AppendLine();
        sb.AppendLine("Use `GetComponentMigration(componentName)` to get the full migration guide for a specific component.");
        sb.AppendLine("Example: `GetComponentMigration(\"FluentButton\")` or `GetComponentMigration(\"Button\")`");

        return sb.ToString();
    }

    /// <summary>
    /// Gets detailed migration documentation for a specific component.
    /// </summary>
    /// <param name="componentName">The component name (e.g., "FluentButton", "Button", "DataGrid").</param>
    /// <returns>The full migration documentation for the specified component.</returns>
    [McpServerTool]
    [Description("Gets the detailed migration guide for a specific Fluent UI Blazor component when upgrading from v4 to v5. " +
                 "Returns the complete documentation including renamed properties, removed properties, new properties, " +
                 "appearance changes, and code examples. Supports both 'FluentButton' and 'Button' naming conventions.")]
    public string GetComponentMigration(
        [Description("The component name to get migration details for (e.g., 'FluentButton', 'Button', 'DataGrid', 'FluentSelect'). " +
                     "Both prefixed (FluentXxx) and unprefixed (Xxx) names are supported.")]
        string componentName)
    {
        if (string.IsNullOrWhiteSpace(componentName))
        {
            return "Please provide a component name. Use `ListComponentMigrations()` to see available components.";
        }

        var migration = _migrationService.GetComponentMigration(componentName);

        if (migration == null)
        {
            var availableNames = _migrationService.GetAvailableComponentNames();
            var namesList = string.Join(", ", availableNames.Take(10));
            return $"No migration guide found for component '{componentName}'.\n\n" +
                   $"Available components: {namesList}\n\n" +
                   "Use `ListComponentMigrations()` to see all available migration guides.";
        }

        return migration.Content;
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

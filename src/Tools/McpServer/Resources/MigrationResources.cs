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
/// MCP Resources providing migration documentation for upgrading Fluent UI Blazor v4 to v5.
/// These resources are user-selected and provide context for the LLM.
/// </summary>
[McpServerResourceType]
public class MigrationResources
{
    private readonly MigrationService _migrationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationResources"/> class.
    /// </summary>
    public MigrationResources(MigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    /// <summary>
    /// Gets the general migration overview.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://migration/overview",
        Name = "migration-overview",
        Title = "Fluent UI Blazor - Migration Overview (v4 → v5)",
        MimeType = "text/markdown")]
    [Description("General migration overview for upgrading from Fluent UI Blazor v4 to v5, including CSS bundling, JavaScript interop, and general guidelines.")]
    public string GetMigrationOverview()
    {
        var overview = _migrationService.GetMigrationOverview();

        if (overview == null)
        {
            return "# Migration Overview\n\nNo general migration documentation found.";
        }

        return overview.Content;
    }

    /// <summary>
    /// Gets the list of all available component migration guides.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://migration/components",
        Name = "migration-components",
        Title = "Fluent UI Blazor - Component Migration Guides",
        MimeType = "text/markdown")]
    [Description("List of all available component-specific migration guides for upgrading from Fluent UI Blazor v4 to v5.")]
    public string GetComponentMigrationList()
    {
        var migrations = _migrationService.GetAllComponentMigrations();

        var sb = new StringBuilder();
        sb.AppendLine("# Component Migration Guides");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Total: {migrations.Count} component migration guides");
        sb.AppendLine();
        sb.AppendLine("Available components (use `fluentui://migration/components/{name}` to access):");
        sb.AppendLine();

        foreach (var migration in migrations)
        {
            var componentName = MigrationService.ExtractComponentName(migration.FileName);
            sb.AppendLine(CultureInfo.InvariantCulture, $"- **{componentName}** — URI: `fluentui://migration/components/{componentName.ToLowerInvariant()}`");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets the migration guide for a specific component.
    /// </summary>
    /// <param name="component">The component name (e.g., "fluentbutton", "button", "datagrid").</param>
    [McpServerResource(
        UriTemplate = "fluentui://migration/components/{component}",
        Name = "migration-component",
        Title = "Component Migration Guide",
        MimeType = "text/markdown")]
    [Description("Detailed migration guide for a specific Fluent UI Blazor component when upgrading from v4 to v5.")]
    public string GetComponentMigration(string component)
    {
        var migration = _migrationService.GetComponentMigration(component);

        if (migration == null)
        {
            var availableNames = _migrationService.GetAvailableComponentNames();
            var namesList = string.Join(", ", availableNames.Take(10).Select(n => $"'{n}'"));

            return $"# Component Not Found\n\nNo migration guide found for '{component}'.\n\n" +
                   $"Available components: {namesList}\n\n" +
                   "Use `fluentui://migration/components` to see all available migration guides.";
        }

        return migration.Content;
    }
}

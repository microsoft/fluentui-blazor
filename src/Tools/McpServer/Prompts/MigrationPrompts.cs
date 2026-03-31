// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.AI;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

/// <summary>
/// MCP prompts for migrating to Fluent UI Blazor v5.
/// </summary>
[McpServerPromptType]
public class MigrationPrompts
{
    private readonly MigrationService _migrationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationPrompts"/> class.
    /// </summary>
    public MigrationPrompts(MigrationService migrationService)
    {
        _migrationService = migrationService;
    }

    /// <summary>
    /// Generates a prompt to help migrate from v4 to v5 of Fluent UI Blazor.
    /// </summary>
    /// <param name="componentToMigrate">Specific component to focus on, or empty for general migration.</param>
    /// <param name="includeBreakingChanges">Whether to emphasize breaking changes.</param>
    [McpServerPrompt(Name = "migrate_to_v5")]
    [Description("Generates guidance for migrating from Fluent UI Blazor v4 to v5, including breaking changes and component updates.")]
    public ChatMessage MigrateToV5(
        [Description("Specific component to focus on (e.g., 'DataGrid', 'Button'), or leave empty for general migration")]
        string componentToMigrate = "",
        [Description("Whether to emphasize breaking changes (default: true)")]
        bool includeBreakingChanges = true)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Migrate to Fluent UI Blazor v5");
        sb.AppendLine();

        // Include general migration overview
        AppendMigrationOverview(sb);

        if (includeBreakingChanges)
        {
            AppendBreakingChanges(sb);
        }

        // Include component-specific migration if requested
        if (!string.IsNullOrWhiteSpace(componentToMigrate))
        {
            AppendComponentMigration(sb, componentToMigrate);
        }
        else
        {
            AppendAvailableComponentMigrations(sb);
        }

        AppendMigrationSteps(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private void AppendMigrationOverview(StringBuilder sb)
    {
        var overview = _migrationService.GetMigrationOverview();
        if (overview != null)
        {
            sb.AppendLine("## General Migration Guidelines");
            sb.AppendLine();
            sb.AppendLine(overview.Content);
            sb.AppendLine();
        }
    }

    private static void AppendBreakingChanges(StringBuilder sb)
    {
        sb.AppendLine("## Key Breaking Changes");
        sb.AppendLine();
        sb.AppendLine("- **Package name change** - Check for any renamed packages");
        sb.AppendLine("- **API changes** - Some component parameters may have been renamed or removed");
        sb.AppendLine("- **Styling changes** - Scoped CSS bundling is now disabled, `::deep` is no longer needed");
        sb.AppendLine("- **JavaScript interop** - JS files migrated to TypeScript with new namespace conventions");
        sb.AppendLine("- **ToAttributeValue()** - Now returns `[Description]` value as-is or lowercase enum name");
        sb.AppendLine();
    }

    private void AppendComponentMigration(StringBuilder sb, string componentToMigrate)
    {
        var migration = _migrationService.GetComponentMigration(componentToMigrate);
        if (migration != null)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"## {componentToMigrate} Migration Details");
            sb.AppendLine();
            sb.AppendLine(migration.Content);
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"## {componentToMigrate} Migration");
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"No specific migration guide found for `{componentToMigrate}`.");
            sb.AppendLine("This component may not have breaking changes, or it may be a new component in v5.");
            sb.AppendLine();

            AppendAvailableComponentMigrations(sb);
        }
    }

    private void AppendAvailableComponentMigrations(StringBuilder sb)
    {
        var componentNames = _migrationService.GetAvailableComponentNames();
        if (componentNames.Count > 0)
        {
            sb.AppendLine("## Available Component Migrations");
            sb.AppendLine();
            sb.AppendLine("The following components have specific migration guides:");
            sb.AppendLine();

            foreach (var name in componentNames)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"- {name}");
            }

            sb.AppendLine();
        }
    }

    private static void AppendMigrationSteps(StringBuilder sb)
    {
        sb.AppendLine("## Migration Steps");
        sb.AppendLine();
        sb.AppendLine("1. **Update packages** to v5 versions");
        sb.AppendLine("2. **Review general migration guidelines** above for cross-cutting changes");
        sb.AppendLine("3. **Update component usage** based on component-specific migration guides");
        sb.AppendLine("4. **Test thoroughly** in development environment");
        sb.AppendLine("5. **Update styles** — remove `::deep` selectors, update scoped CSS references");
        sb.AppendLine();
        sb.AppendLine("Please provide specific migration guidance based on the requirements.");
    }
}

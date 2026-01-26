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
    private readonly DocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationPrompts"/> class.
    /// </summary>
    public MigrationPrompts(DocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Generates a prompt to help migrate from v4 to v5 of Fluent UI Blazor.
    /// </summary>
    /// <param name="componentToMigrate">Specific component to focus on, or empty for general migration.</param>
    /// <param name="includeBreakingChanges">Whether to emphasize breaking changes.</param>
    [McpServerPrompt(Name = "migrate_to_v5")]
    [Description("Generates guidance for migrating from Fluent UI Blazor v4 to v5, including breaking changes and component updates.")]
    public ChatMessage MigrateToV5(
        [Description("Specific component to focus on (e.g., 'DataGrid', 'Dialog'), or leave empty for general migration")]
        string componentToMigrate = "",
        [Description("Whether to emphasize breaking changes (default: true)")]
        bool includeBreakingChanges = true)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Migrate to Fluent UI Blazor v5");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(componentToMigrate))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"## Focus: {componentToMigrate} Migration");
            sb.AppendLine();
        }

        AppendMigrationDocumentation(sb);

        if (includeBreakingChanges)
        {
            AppendBreakingChanges(sb);
        }

        AppendComponentMigration(sb, componentToMigrate);
        AppendMigrationSteps(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private void AppendMigrationDocumentation(StringBuilder sb)
    {
        var migrationDoc = _documentationService.GetDocumentation("MigrationVersion5");
        if (migrationDoc != null)
        {
            sb.AppendLine("## Migration Documentation");
            sb.AppendLine();
            sb.AppendLine(migrationDoc.Summary ?? "See the migration guide for details.");
            sb.AppendLine();
        }
    }

    private static void AppendBreakingChanges(StringBuilder sb)
    {
        sb.AppendLine("## Key Breaking Changes");
        sb.AppendLine();
        sb.AppendLine("- **Package name change** - Check for any renamed packages");
        sb.AppendLine("- **API changes** - Some component parameters may have been renamed");
        sb.AppendLine("- **Styling changes** - CSS class names and design tokens may differ");
        sb.AppendLine("- **Component restructuring** - Some components may have been split or merged");
        sb.AppendLine();
    }

    private static void AppendComponentMigration(StringBuilder sb, string componentToMigrate)
    {
        if (!string.IsNullOrWhiteSpace(componentToMigrate))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"### {componentToMigrate}-Specific Migration");
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"Focus on changes specific to the `{componentToMigrate}` component.");
            sb.AppendLine();
        }
    }

    private static void AppendMigrationSteps(StringBuilder sb)
    {
        sb.AppendLine("## Migration Steps");
        sb.AppendLine();
        sb.AppendLine("1. **Update packages** to v5 versions");
        sb.AppendLine("2. **Review breaking changes** in release notes");
        sb.AppendLine("3. **Update component usage** based on API changes");
        sb.AppendLine("4. **Test thoroughly** in development environment");
        sb.AppendLine("5. **Update styles** if using custom CSS");
        sb.AppendLine();
        sb.AppendLine("Please provide specific migration guidance based on the requirements.");
    }
}

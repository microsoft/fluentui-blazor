// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using FluentUI.Mcp.Server.Services;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace FluentUI.Mcp.Server.Prompts;

/// <summary>
/// MCP Prompt for migrating Fluent UI Blazor code from v4 to v5.
/// </summary>
[McpServerPromptType]
public class MigrateToV5Prompt
{
    private readonly DocumentationGuideService _guideService;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrateToV5Prompt"/> class.
    /// </summary>
    public MigrateToV5Prompt(DocumentationGuideService guideService)
    {
        _guideService = guideService;
    }

    [McpServerPrompt(Name = "migrate_to_v5")]
    [Description("Get guidance for migrating Fluent UI Blazor code from v4 to v5.")]
    public ChatMessage MigrateToV5(
        [Description("The code or component to migrate (paste your existing v4 code here)")] string existingCode,
        [Description("Optional: specific components or features you're migrating")] string? focus = null)
    {
        var migrationGuide = _guideService.GetFullMigrationGuide();

        var sb = new StringBuilder();
        sb.AppendLine("# Migrate Fluent UI Blazor Code from v4 to v5");
        sb.AppendLine();

        sb.AppendLine("## Migration Guide Reference");
        sb.AppendLine();

        // Include relevant parts of migration guide
        if (!string.IsNullOrEmpty(migrationGuide) && migrationGuide.Length > 100)
        {
            // Truncate if too long, but keep key sections
            if (migrationGuide.Length > 3000)
            {
                sb.AppendLine(migrationGuide[..3000]);
                sb.AppendLine("...(truncated, use GetGuide('migration') for full guide)");
            }
            else
            {
                sb.AppendLine(migrationGuide);
            }
        }
        else
        {
            sb.AppendLine("Migration guide not available. Key changes in v5 include:");
            sb.AppendLine("- Color enum changes");
            sb.AppendLine("- Component API updates");
            sb.AppendLine("- New default values system");
        }

        sb.AppendLine();
        sb.AppendLine("## Code to Migrate");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine(existingCode);
        sb.AppendLine("```");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(focus))
        {
            sb.AppendLine("## Focus Areas");
            sb.AppendLine();
            sb.AppendLine(focus);
            sb.AppendLine();
        }

        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine("Migrate this Fluent UI Blazor code from v4 to v5:");
        sb.AppendLine("1. Identify any deprecated or changed APIs");
        sb.AppendLine("2. Update component names and parameters as needed");
        sb.AppendLine("3. Apply new v5 patterns and best practices");
        sb.AppendLine("4. Explain each change you make");
        sb.AppendLine("5. Highlight any breaking changes that require attention");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

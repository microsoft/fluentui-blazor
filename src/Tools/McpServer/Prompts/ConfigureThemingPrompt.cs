// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;

/// <summary>
/// MCP Prompt for configuring theming in Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class ConfigureThemingPrompt
{
    private readonly DocumentationGuideService _guideService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureThemingPrompt"/> class.
    /// </summary>
    public ConfigureThemingPrompt(DocumentationGuideService guideService)
    {
        _guideService = guideService;
    }

    [McpServerPrompt(Name = "configure_theming")]
    [Description("Get guidance for configuring theming and styles in Fluent UI Blazor.")]
    public ChatMessage ConfigureTheming(
        [Description("Theme requirements: 'dark', 'light', 'custom', or 'dynamic'")] string themeType,
        [Description("Optional: specific colors or design tokens to customize")] string? customizations = null)
    {
        var stylesGuide = _guideService.GetGuideContent("styles");

        var sb = new StringBuilder();
        sb.AppendLine($"# Configure {themeType.ToUpperInvariant()} Theme in Fluent UI Blazor");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(stylesGuide) && stylesGuide.Length > 100)
        {
            sb.AppendLine("## Styles Guide Reference");
            sb.AppendLine();

            if (stylesGuide.Length > 2500)
            {
                sb.AppendLine(stylesGuide[..2500]);
                sb.AppendLine("...(truncated)");
            }
            else
            {
                sb.AppendLine(stylesGuide);
            }

            sb.AppendLine();
        }

        sb.AppendLine("## Theme Requirements");
        sb.AppendLine();
        sb.AppendLine($"- **Theme Type**: {themeType}");

        if (!string.IsNullOrEmpty(customizations))
        {
            sb.AppendLine($"- **Customizations**: {customizations}");
        }

        sb.AppendLine();
        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine($"Provide complete guidance for implementing {themeType} theming in Fluent UI Blazor:");
        sb.AppendLine("1. Theme provider setup");
        sb.AppendLine("2. Design token configuration");
        sb.AppendLine("3. CSS variable customization");
        sb.AppendLine("4. Dark/light mode switching (if applicable)");
        sb.AppendLine("5. Component-specific styling");

        if (!string.IsNullOrEmpty(customizations))
        {
            sb.AppendLine($"6. Custom requirements: {customizations}");
        }

        sb.AppendLine();
        sb.AppendLine("Include working code examples for the theme configuration.");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

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
/// MCP Prompt for configuring localization in Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class ConfigureLocalizationPrompt
{
    private readonly DocumentationGuideService _guideService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureLocalizationPrompt"/> class.
    /// </summary>
    public ConfigureLocalizationPrompt(DocumentationGuideService guideService)
    {
        _guideService = guideService;
    }

    [McpServerPrompt(Name = "configure_localization")]
    [Description("Get guidance for implementing localization in Fluent UI Blazor.")]
    public ChatMessage ConfigureLocalization(
        [Description("Target languages (comma-separated, e.g., 'French,German,Spanish')")] string languages)
    {
        var localizationGuide = _guideService.GetGuideContent("localization");

        var sb = new StringBuilder();
        sb.AppendLine("# Configure Localization in Fluent UI Blazor");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(localizationGuide) && localizationGuide.Length > 100)
        {
            sb.AppendLine("## Localization Guide Reference");
            sb.AppendLine();
            sb.AppendLine(localizationGuide);
            sb.AppendLine();
        }

        sb.AppendLine("## Localization Requirements");
        sb.AppendLine();
        sb.AppendLine($"- **Target Languages**: {languages}");
        sb.AppendLine();

        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine("Provide complete guidance for implementing localization:");
        sb.AppendLine("1. IFluentLocalizer implementation");
        sb.AppendLine("2. Resource file setup for target languages");
        sb.AppendLine("3. Service registration");
        sb.AppendLine("4. Culture switching implementation");
        sb.AppendLine("5. Component text localization");
        sb.AppendLine();
        sb.AppendLine($"Include examples for these languages: {languages}");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

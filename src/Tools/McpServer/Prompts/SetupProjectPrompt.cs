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
/// MCP Prompt for setting up a new Fluent UI Blazor project.
/// </summary>
[McpServerPromptType]
public class SetupProjectPrompt
{
    private readonly DocumentationGuideService _guideService;
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetupProjectPrompt"/> class.
    /// </summary>
    public SetupProjectPrompt(DocumentationGuideService guideService, FluentUIDocumentationService documentationService)
    {
        _guideService = guideService;
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "setup_project")]
    [Description("Get step-by-step guidance for setting up a new Fluent UI Blazor project with the correct package version.")]
    public ChatMessage SetupProject(
        [Description("The type of Blazor project: 'server', 'wasm', 'hybrid', or 'maui'")] string projectType,
        [Description("Optional: specific features to include (e.g., 'icons', 'datagrid', 'charts')")] string? features = null)
    {
        var installGuide = _guideService.GetGuideContent("installation");
        var componentsVersion = _documentationService.ComponentsVersion;
        var mcpServerVersion = FluentUIDocumentationService.McpServerVersion;

        var sb = new StringBuilder();
        sb.AppendLine($"# Set Up a Fluent UI Blazor {projectType.ToUpperInvariant()} Project");
        sb.AppendLine();

        // Version compatibility information
        sb.AppendLine("## Version Information");
        sb.AppendLine();
        sb.AppendLine($"- **MCP Server Version**: {mcpServerVersion}");
        sb.AppendLine($"- **Compatible Components Version**: {componentsVersion}");
        sb.AppendLine();
        sb.AppendLine("> ⚠️ **Important**: For optimal compatibility with this MCP Server, install the matching version of the NuGet package.");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(installGuide) && installGuide.Length > 100)
        {
            sb.AppendLine("## Installation Guide Reference");
            sb.AppendLine();

            if (installGuide.Length > 2000)
            {
                sb.AppendLine(installGuide[..2000]);
                sb.AppendLine("...(truncated)");
            }
            else
            {
                sb.AppendLine(installGuide);
            }

            sb.AppendLine();
        }

        sb.AppendLine("## Project Configuration");
        sb.AppendLine();
        sb.AppendLine($"- **Project Type**: Blazor {projectType}");

        if (!string.IsNullOrEmpty(features))
        {
            sb.AppendLine($"- **Features**: {features}");
        }

        sb.AppendLine();
        sb.AppendLine("## Required NuGet Packages");
        sb.AppendLine();
        sb.AppendLine("```shell");
        sb.AppendLine($"dotnet add package Microsoft.FluentUI.AspNetCore.Components --version {componentsVersion}");

        if (!string.IsNullOrEmpty(features))
        {
            if (features.Contains("icon", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($"dotnet add package Microsoft.FluentUI.AspNetCore.Components.Icons --version {componentsVersion}");
            }

            if (features.Contains("emoji", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($"dotnet add package Microsoft.FluentUI.AspNetCore.Components.Emoji --version {componentsVersion}");
            }
        }

        sb.AppendLine("```");
        sb.AppendLine();

        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine($"Provide complete step-by-step instructions for setting up a Blazor {projectType} project with Fluent UI Blazor:");
        sb.AppendLine("1. Project creation commands");
        sb.AppendLine($"2. NuGet package installation (use version **{componentsVersion}**)");
        sb.AppendLine("3. Program.cs configuration");
        sb.AppendLine("4. _Imports.razor setup");
        sb.AppendLine("5. Layout configuration");
        sb.AppendLine("6. CSS and JavaScript references");

        if (!string.IsNullOrEmpty(features))
        {
            sb.AppendLine($"7. Configuration for requested features: {features}");
        }

        sb.AppendLine();
        sb.AppendLine("Include complete code examples for each step.");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

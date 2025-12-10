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
/// MCP Prompt for generating Fluent UI Blazor drawer components.
/// </summary>
[McpServerPromptType]
public class CreateDrawerPrompt
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDrawerPrompt"/> class.
    /// </summary>
    public CreateDrawerPrompt(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "create_drawer")]
    [Description("Generate a Fluent UI Blazor drawer/panel component.")]
    public ChatMessage CreateDrawer(
        [Description("The purpose of the drawer (e.g., 'navigation menu', 'settings panel', 'details view', 'filters')")] string purpose,
        [Description("Optional: position of the drawer ('start', 'end', 'top', 'bottom')")] string? position = null,
        [Description("Optional: content requirements (e.g., 'navigation links', 'form fields', 'list of items')")] string? content = null)
    {
        var drawerDetails = _documentationService.GetComponentDetails("FluentDrawer");

        var sb = new StringBuilder();
        sb.AppendLine("# Create a Fluent UI Blazor Drawer");
        sb.AppendLine();

        if (drawerDetails != null)
        {
            sb.AppendLine("## FluentDrawer Component");
            sb.AppendLine();
            sb.AppendLine($"{drawerDetails.Component.Summary}");
            sb.AppendLine();

            sb.AppendLine("### Key Parameters");
            sb.AppendLine();

            var keyParams = new[] { "Position", "Title", "Width", "Dismissible", "Modal", "Open" };
            foreach (var param in drawerDetails.Parameters.Where(p => keyParams.Contains(p.Name)))
            {
                sb.AppendLine($"- `{param.Name}` ({param.Type}): {param.Description}");
            }

            sb.AppendLine();
        }

        sb.AppendLine("## Drawer Requirements");
        sb.AppendLine();
        sb.AppendLine($"**Purpose**: {purpose}");

        if (!string.IsNullOrEmpty(position))
        {
            sb.AppendLine($"**Position**: {position}");
        }

        if (!string.IsNullOrEmpty(content))
        {
            sb.AppendLine($"**Content**: {content}");
        }

        sb.AppendLine();
        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine("Generate a complete FluentDrawer implementation that includes:");
        sb.AppendLine("1. Drawer component with appropriate configuration");
        sb.AppendLine("2. Header with title and close button");
        sb.AppendLine("3. Body content");
        sb.AppendLine("4. Open/close state management");
        sb.AppendLine("5. Trigger button or mechanism to open the drawer");

        if (!string.IsNullOrEmpty(position))
        {
            sb.AppendLine($"6. Position set to: {position}");
        }

        sb.AppendLine();
        sb.AppendLine("Include both the Razor markup and the code-behind with proper state handling.");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

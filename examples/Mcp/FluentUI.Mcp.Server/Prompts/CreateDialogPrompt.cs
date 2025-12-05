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
/// MCP Prompt for generating Fluent UI Blazor dialog components.
/// </summary>
[McpServerPromptType]
public class CreateDialogPrompt
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDialogPrompt"/> class.
    /// </summary>
    public CreateDialogPrompt(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "create_dialog")]
    [Description("Generate a Fluent UI Blazor dialog/modal component.")]
    public ChatMessage CreateDialog(
        [Description("The purpose of the dialog (e.g., 'confirm delete', 'edit user', 'display details')")] string purpose,
        [Description("Optional: content requirements (e.g., 'form with name and email', 'warning message')")] string? content = null)
    {
        var dialogDetails = _documentationService.GetComponentDetails("FluentDialog");

        var sb = new StringBuilder();
        sb.AppendLine("# Create a Fluent UI Blazor Dialog");
        sb.AppendLine();

        if (dialogDetails != null)
        {
            sb.AppendLine("## FluentDialog Component");
            sb.AppendLine();
            sb.AppendLine($"{dialogDetails.Component.Summary}");
            sb.AppendLine();
        }

        sb.AppendLine("## Dialog Requirements");
        sb.AppendLine();
        sb.AppendLine($"**Purpose**: {purpose}");

        if (!string.IsNullOrEmpty(content))
        {
            sb.AppendLine($"**Content**: {content}");
        }

        sb.AppendLine();
        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine("Generate a complete FluentDialog implementation that includes:");
        sb.AppendLine("1. Dialog component with appropriate title");
        sb.AppendLine("2. Dialog content/body");
        sb.AppendLine("3. Action buttons (confirm, cancel, etc.)");
        sb.AppendLine("4. Service-based dialog opening/closing");
        sb.AppendLine("5. Result handling");
        sb.AppendLine("6. Proper async/await patterns");
        sb.AppendLine();
        sb.AppendLine("Show how to open the dialog from a parent component.");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

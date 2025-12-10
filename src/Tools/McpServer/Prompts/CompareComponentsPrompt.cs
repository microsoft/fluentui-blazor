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
/// MCP Prompt for comparing Fluent UI Blazor components.
/// </summary>
[McpServerPromptType]
public class CompareComponentsPrompt
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompareComponentsPrompt"/> class.
    /// </summary>
    public CompareComponentsPrompt(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "compare_components")]
    [Description("Compare two or more Fluent UI Blazor components to understand their differences.")]
    public ChatMessage CompareComponents(
        [Description("Comma-separated list of component names to compare (e.g., 'FluentButton,FluentAnchor')")] string componentNames)
    {
        var names = componentNames.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var sb = new StringBuilder();
        sb.AppendLine("# Component Comparison");
        sb.AppendLine();

        foreach (var name in names)
        {
            var details = _documentationService.GetComponentDetails(name);
            if (details != null)
            {
                sb.AppendLine($"## {details.Component.Name}");
                sb.AppendLine();
                sb.AppendLine($"- **Category**: {details.Component.Category}");
                sb.AppendLine($"- **Summary**: {details.Component.Summary}");
                sb.AppendLine($"- **Parameters**: {details.Parameters.Count}");
                sb.AppendLine($"- **Events**: {details.Events.Count}");
                sb.AppendLine();
            }
            else
            {
                sb.AppendLine($"## {name}");
                sb.AppendLine();
                sb.AppendLine("Component not found.");
                sb.AppendLine();
            }
        }

        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine("Compare these Fluent UI Blazor components and explain:");
        sb.AppendLine("1. The key differences between them");
        sb.AppendLine("2. When to use each component");
        sb.AppendLine("3. Any shared functionality or parameters");
        sb.AppendLine("4. Performance or accessibility considerations");
        sb.AppendLine("5. Example scenarios where each would be the best choice");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

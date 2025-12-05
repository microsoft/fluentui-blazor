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
/// MCP Prompt for explaining Fluent UI Blazor components.
/// </summary>
[McpServerPromptType]
public class ExplainComponentPrompt
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExplainComponentPrompt"/> class.
    /// </summary>
    public ExplainComponentPrompt(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "explain_component")]
    [Description("Get a detailed explanation of a Fluent UI Blazor component and its usage.")]
    public ChatMessage ExplainComponent(
        [Description("The component name to explain")] string componentName)
    {
        var details = _documentationService.GetComponentDetails(componentName);

        var sb = new StringBuilder();
        sb.AppendLine($"# Explain {componentName}");
        sb.AppendLine();

        if (details != null)
        {
            sb.AppendLine("## Component Details");
            sb.AppendLine();
            sb.AppendLine($"- **Name**: {details.Component.Name}");
            sb.AppendLine($"- **Category**: {details.Component.Category}");
            sb.AppendLine($"- **Summary**: {details.Component.Summary}");

            if (details.Component.IsGeneric)
            {
                sb.AppendLine("- **Generic**: Yes (requires type parameter)");
            }

            sb.AppendLine();
            sb.AppendLine($"**Parameters**: {details.Parameters.Count}");
            sb.AppendLine($"**Events**: {details.Events.Count}");
            sb.AppendLine($"**Methods**: {details.Methods.Count}");
            sb.AppendLine();
        }

        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine($"Provide a comprehensive explanation of the {componentName} component that includes:");
        sb.AppendLine("1. What the component is used for");
        sb.AppendLine("2. Common use cases and scenarios");
        sb.AppendLine("3. Key parameters and their purposes");
        sb.AppendLine("4. Best practices for using the component");
        sb.AppendLine("5. Common pitfalls to avoid");
        sb.AppendLine("6. A simple example demonstrating basic usage");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

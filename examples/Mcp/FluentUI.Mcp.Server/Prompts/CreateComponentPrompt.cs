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
/// MCP Prompt for generating Fluent UI Blazor component code.
/// </summary>
[McpServerPromptType]
public class CreateComponentPrompt
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateComponentPrompt"/> class.
    /// </summary>
    public CreateComponentPrompt(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "create_component")]
    [Description("Generate code for a Fluent UI Blazor component with specified configuration.")]
    public ChatMessage CreateComponent(
        [Description("The component name (e.g., 'FluentButton', 'FluentDataGrid', 'FluentTextField')")] string componentName,
        [Description("Optional: specific requirements or configuration for the component")] string? requirements = null)
    {
        var details = _documentationService.GetComponentDetails(componentName);

        var sb = new StringBuilder();
        sb.AppendLine($"# Create a {componentName} Component");
        sb.AppendLine();

        if (details != null)
        {
            sb.AppendLine("## Component Information");
            sb.AppendLine();
            sb.AppendLine($"**{details.Component.Name}**: {details.Component.Summary}");
            sb.AppendLine();

            if (details.Parameters.Count > 0)
            {
                sb.AppendLine("## Available Parameters");
                sb.AppendLine();

                foreach (var param in details.Parameters.Take(15))
                {
                    var defaultInfo = !string.IsNullOrEmpty(param.DefaultValue) ? $" (default: {param.DefaultValue})" : "";
                    sb.AppendLine($"- `{param.Name}` ({param.Type}){defaultInfo}: {param.Description}");
                }

                if (details.Parameters.Count > 15)
                {
                    sb.AppendLine($"- ... and {details.Parameters.Count - 15} more parameters");
                }

                sb.AppendLine();
            }

            if (details.Events.Count > 0)
            {
                sb.AppendLine("## Available Events");
                sb.AppendLine();

                foreach (var evt in details.Events.Take(5))
                {
                    sb.AppendLine($"- `{evt.Name}` ({evt.Type}): {evt.Description}");
                }

                sb.AppendLine();
            }
        }
        else
        {
            sb.AppendLine($"Component '{componentName}' not found in documentation.");
            sb.AppendLine("Please check the component name and try again.");
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(requirements))
        {
            sb.AppendLine("## Requirements");
            sb.AppendLine();
            sb.AppendLine(requirements);
            sb.AppendLine();
        }

        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine($"Generate a complete, working Blazor code example for the {componentName} component that:");
        sb.AppendLine("1. Uses proper Fluent UI Blazor syntax");
        sb.AppendLine("2. Includes necessary @using statements");
        sb.AppendLine("3. Shows best practices for the component");
        sb.AppendLine("4. Includes event handlers if applicable");

        if (!string.IsNullOrEmpty(requirements))
        {
            sb.AppendLine($"5. Meets these specific requirements: {requirements}");
        }

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}

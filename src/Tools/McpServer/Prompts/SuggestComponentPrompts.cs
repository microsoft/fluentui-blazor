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
/// MCP prompts for suggesting components based on use cases.
/// </summary>
[McpServerPromptType]
public class SuggestComponentPrompts
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SuggestComponentPrompts"/> class.
    /// </summary>
    public SuggestComponentPrompts(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Suggests appropriate Fluent UI Blazor components for a given use case.
    /// </summary>
    /// <param name="useCase">Description of what the user wants to achieve.</param>
    /// <param name="context">Additional context about the project or requirements.</param>
    [McpServerPrompt(Name = "suggest_components")]
    [Description("Suggests the best Fluent UI Blazor components for a specific use case or feature implementation.")]
    public ChatMessage SuggestComponents(
        [Description("What you want to achieve (e.g., 'display a list of items', 'create a settings page', 'show notifications')")]
        string useCase,
        [Description("Additional context about your project or specific requirements")]
        string context = "")
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Suggest Fluent UI Blazor Components");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"**Goal:** {useCase}");

        if (!string.IsNullOrWhiteSpace(context))
        {
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Context:** {context}");
        }

        sb.AppendLine();
        AppendAvailableComponents(sb);
        AppendRequest(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private void AppendAvailableComponents(StringBuilder sb)
    {
        sb.AppendLine("## Available Components by Category");
        sb.AppendLine();

        var components = _documentationService.GetAllComponents();
        var categorized = components
            .GroupBy(c => c.Category, StringComparer.OrdinalIgnoreCase)
            .OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

        foreach (var category in categorized)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"### {category.Key}");

            foreach (var component in category.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"- **{component.Name}**: {component.Summary ?? "No description"}");
            }

            sb.AppendLine();
        }
    }

    private static void AppendRequest(StringBuilder sb)
    {
        sb.AppendLine("## Request");
        sb.AppendLine();
        sb.AppendLine("Based on the goal and available components, please suggest:");
        sb.AppendLine();
        sb.AppendLine("1. The most suitable components for this use case");
        sb.AppendLine("2. How to combine them effectively");
        sb.AppendLine("3. A code example demonstrating the implementation");
        sb.AppendLine("4. Any alternative approaches to consider");
    }
}

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
/// MCP prompts for explaining Fluent UI Blazor components.
/// </summary>
[McpServerPromptType]
public class ExplainComponentPrompts
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExplainComponentPrompts"/> class.
    /// </summary>
    public ExplainComponentPrompts(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Generates a prompt to explain a specific Fluent UI Blazor component.
    /// </summary>
    /// <param name="componentName">The name of the component to explain.</param>
    /// <param name="includeExamples">Whether to include usage examples.</param>
    /// <param name="detailLevel">Level of detail: 'basic', 'intermediate', or 'advanced'.</param>
    [McpServerPrompt(Name = "explain_component")]
    [Description("Generates a detailed explanation of a specific Fluent UI Blazor component.")]
    public ChatMessage ExplainComponent(
        [Description("The name of the component (e.g., 'FluentButton', 'FluentDataGrid')")]
        string componentName,
        [Description("Whether to include usage examples (default: true)")]
        bool includeExamples = true,
        [Description("Level of detail: 'basic', 'intermediate', or 'advanced' (default: 'intermediate')")]
        string detailLevel = "intermediate")
    {
        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Explain: {componentName}");
        sb.AppendLine();

        AppendComponentInfo(sb, componentName);
        AppendDetailInstructions(sb, detailLevel, includeExamples);
        AppendParameters(sb, componentName, detailLevel);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private void AppendComponentInfo(StringBuilder sb, string componentName)
    {
        var details = _documentationService.GetComponentDetails(componentName);

        if (details != null)
        {
            sb.AppendLine("## Component Information");
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Name:** {details.Component.Name}");
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Category:** {details.Component.Category}");

            if (!string.IsNullOrEmpty(details.Component.Summary))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"**Summary:** {details.Component.Summary}");
            }

            if (details.Component.IsGeneric)
            {
                sb.AppendLine("**Generic:** Yes (requires type parameter)");
            }

            sb.AppendLine();
        }
    }

    private static void AppendDetailInstructions(StringBuilder sb, string detailLevel, bool includeExamples)
    {
        sb.AppendLine(CultureInfo.InvariantCulture, $"## Provide a **{detailLevel}** level explanation covering:");
        sb.AppendLine();

        switch (detailLevel.ToLowerInvariant())
        {
            case "basic":
                sb.AppendLine("1. What the component is and its purpose");
                sb.AppendLine("2. When to use this component");
                sb.AppendLine("3. Essential parameters");
                break;

            case "advanced":
                sb.AppendLine("1. Detailed purpose and design philosophy");
                sb.AppendLine("2. Complete parameter reference");
                sb.AppendLine("3. All events and callbacks");
                sb.AppendLine("4. Styling and theming options");
                sb.AppendLine("5. Accessibility considerations");
                sb.AppendLine("6. Performance tips");
                break;

            default:
                sb.AppendLine("1. What the component is and its purpose");
                sb.AppendLine("2. Key parameters and their uses");
                sb.AppendLine("3. Available events and callbacks");
                sb.AppendLine("4. Best practices");
                break;
        }

        if (includeExamples)
        {
            sb.AppendLine();
            sb.AppendLine("Include code examples for basic and common usage patterns.");
        }

        sb.AppendLine();
    }

    private void AppendParameters(StringBuilder sb, string componentName, string detailLevel)
    {
        var details = _documentationService.GetComponentDetails(componentName);

        if (details == null || details.Parameters.Count == 0)
        {
            return;
        }

        sb.AppendLine("## Available Parameters");
        sb.AppendLine();

        var parametersToShow = detailLevel.Equals("basic", StringComparison.OrdinalIgnoreCase)
            ? details.Parameters.Take(5)
            : details.Parameters;

        foreach (var param in parametersToShow)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"- **{param.Name}** (`{param.Type}`)");
        }

        if (detailLevel.Equals("basic", StringComparison.OrdinalIgnoreCase) && details.Parameters.Count > 5)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"- ... and {details.Parameters.Count - 5} more");
        }
    }
}

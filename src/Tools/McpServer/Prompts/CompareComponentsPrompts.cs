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
/// MCP prompts for comparing Fluent UI Blazor components.
/// </summary>
[McpServerPromptType]
public class CompareComponentsPrompts
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompareComponentsPrompts"/> class.
    /// </summary>
    public CompareComponentsPrompts(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Generates a prompt to compare two or more Fluent UI Blazor components.
    /// </summary>
    /// <param name="components">Comma-separated list of components to compare.</param>
    /// <param name="comparisonFocus">What aspect to focus the comparison on.</param>
    [McpServerPrompt(Name = "compare_components")]
    [Description("Compares two or more Fluent UI Blazor components to help choose the right one for your needs.")]
    public ChatMessage CompareComponents(
        [Description("Comma-separated list of components to compare (e.g., 'FluentSelect,FluentCombobox,FluentAutocomplete')")]
        string components,
        [Description("Focus of comparison: 'features', 'performance', 'accessibility', or 'all' (default: 'all')")]
        string comparisonFocus = "all")
    {
        var componentList = components.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var sb = new StringBuilder();
        sb.AppendLine("# Compare Fluent UI Blazor Components");
        sb.AppendLine();

        AppendComponentsToCompare(sb, componentList);
        AppendComponentDetails(sb, componentList);
        AppendComparisonFocus(sb, comparisonFocus);
        AppendRequest(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendComponentsToCompare(StringBuilder sb, string[] componentList)
    {
        sb.AppendLine("## Components to Compare");
        sb.AppendLine();

        foreach (var component in componentList)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"- {component}");
        }

        sb.AppendLine();
    }

    private void AppendComponentDetails(StringBuilder sb, string[] componentList)
    {
        sb.AppendLine("## Component Details");
        sb.AppendLine();

        foreach (var componentName in componentList)
        {
            var details = _documentationService.GetComponentDetails(componentName.Trim());

            if (details != null)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"### {details.Component.Name}");
                sb.AppendLine();

                if (!string.IsNullOrEmpty(details.Component.Summary))
                {
                    sb.AppendLine(details.Component.Summary);
                }

                sb.AppendLine(CultureInfo.InvariantCulture, $"- **Category:** {details.Component.Category}");
                sb.AppendLine(CultureInfo.InvariantCulture, $"- **Parameters:** {details.Parameters.Count}");
                sb.AppendLine(CultureInfo.InvariantCulture, $"- **Events:** {details.Events.Count}");

                if (details.Component.IsGeneric)
                {
                    sb.AppendLine("- **Generic:** Yes");
                }

                sb.AppendLine();
            }
            else
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"### {componentName}");
                sb.AppendLine("(Component details not found in documentation)");
                sb.AppendLine();
            }
        }
    }

    private static void AppendComparisonFocus(StringBuilder sb, string comparisonFocus)
    {
        sb.AppendLine("## Comparison Focus");
        sb.AppendLine();

        if (string.Equals(comparisonFocus, "FEATURES", StringComparison.OrdinalIgnoreCase))
        {
            sb.AppendLine("Focus on: **Feature set and capabilities**");
        }
        else if (string.Equals(comparisonFocus, "PERFORMANCE", StringComparison.OrdinalIgnoreCase))
        {
            sb.AppendLine("Focus on: **Performance characteristics**");
        }
        else if (string.Equals(comparisonFocus, "ACCESSIBILITY", StringComparison.OrdinalIgnoreCase))
        {
            sb.AppendLine("Focus on: **Accessibility features**");
        }
        else
        {
            sb.AppendLine("Focus on: **All aspects** (features, performance, accessibility, use cases)");
        }

        sb.AppendLine();
    }

    private static void AppendRequest(StringBuilder sb)
    {
        sb.AppendLine("## Please Provide");
        sb.AppendLine();
        sb.AppendLine("1. A comparison table highlighting key differences");
        sb.AppendLine("2. When to use each component");
        sb.AppendLine("3. Pros and cons of each");
        sb.AppendLine("4. Code examples showing similar functionality");
        sb.AppendLine("5. Recommendation based on common use cases");
    }
}

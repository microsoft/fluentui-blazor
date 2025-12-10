// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Tools;

/// <summary>
/// MCP tools for getting detailed component documentation.
/// </summary>
[McpServerToolType]
public class ComponentDetailTools
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentDetailTools"/> class.
    /// </summary>
    /// <param name="documentationService">The documentation service.</param>
    public ComponentDetailTools(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerTool]
    [Description("Gets detailed documentation for a specific Fluent UI Blazor component, including all its parameters, properties, events, and methods.")]
    public string GetComponentDetails(
        [Description("The name of the component (e.g., 'FluentButton', 'FluentDataGrid', 'FluentTextField'). You can omit the 'Fluent' prefix.")]
        string componentName)
    {
        var details = _documentationService.GetComponentDetails(componentName);

        if (details == null)
        {
            return $"Component '{componentName}' not found. Use ListComponents() to see all available components.";
        }

        var sb = new StringBuilder();

        AppendComponentHeader(sb, details);
        AppendParameters(sb, details);
        AppendEvents(sb, details);
        AppendMethods(sb, details);

        return sb.ToString();
    }

    [McpServerTool]
    [Description("Gets a usage example for a specific Fluent UI Blazor component showing basic and common usage patterns.")]
    public string GetComponentExample(
        [Description("The name of the component (e.g., 'FluentButton', 'FluentDataGrid', 'FluentTextField').")]
        string componentName)
    {
        var details = _documentationService.GetComponentDetails(componentName);

        if (details == null)
        {
            return $"Component '{componentName}' not found. Use ListComponents() to see all available components.";
        }

        var sb = new StringBuilder();

        AppendBasicExample(sb, details);
        AppendEventHandlingExample(sb, details);
        AppendCommonParameters(sb, details);

        return sb.ToString();
    }

    private static void AppendComponentHeader(StringBuilder sb, Models.ComponentDetails details)
    {
        sb.AppendLine($"# {details.Component.Name}");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(details.Component.Summary))
        {
            sb.AppendLine(details.Component.Summary);
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(details.Component.BaseClass))
        {
            sb.AppendLine($"**Base Class:** {details.Component.BaseClass}");
        }

        if (details.Component.IsGeneric)
        {
            sb.AppendLine("**Generic Component:** Yes (requires type parameter)");
        }

        sb.AppendLine($"**Category:** {details.Component.Category}");
        sb.AppendLine();
    }

    private static void AppendParameters(StringBuilder sb, Models.ComponentDetails details)
    {
        if (details.Parameters.Count == 0)
        {
            return;
        }

        sb.AppendLine("## Parameters");
        sb.AppendLine();
        sb.AppendLine("| Name | Type | Default | Description |");
        sb.AppendLine("|------|------|---------|-------------|");

        foreach (var param in details.Parameters)
        {
            var defaultValue = param.DefaultValue ?? "-";
            var description = ToolOutputHelper.TruncateSummary(param.Description, 80);
            var enumHint = param.EnumValues.Length > 0
                ? $" Values: {string.Join(", ", param.EnumValues.Take(5))}{(param.EnumValues.Length > 5 ? "..." : "")}"
                : "";
            sb.AppendLine($"| {param.Name} | `{param.Type}` | {defaultValue} | {description}{enumHint} |");
        }

        sb.AppendLine();
    }

    private static void AppendEvents(StringBuilder sb, Models.ComponentDetails details)
    {
        if (details.Events.Count == 0)
        {
            return;
        }

        sb.AppendLine("## Events");
        sb.AppendLine();
        sb.AppendLine("| Name | Type | Description |");
        sb.AppendLine("|------|------|-------------|");

        foreach (var evt in details.Events)
        {
            sb.AppendLine($"| {evt.Name} | `{evt.Type}` | {ToolOutputHelper.TruncateSummary(evt.Description, 80)} |");
        }

        sb.AppendLine();
    }

    private static void AppendMethods(StringBuilder sb, Models.ComponentDetails details)
    {
        if (details.Methods.Count == 0)
        {
            return;
        }

        sb.AppendLine("## Methods");
        sb.AppendLine();

        foreach (var method in details.Methods)
        {
            sb.AppendLine($"### {method.Name}");
            sb.AppendLine();
            sb.AppendLine($"```csharp");
            sb.AppendLine($"{method.Signature}");
            sb.AppendLine($"```");

            if (!string.IsNullOrEmpty(method.Description))
            {
                sb.AppendLine();
                sb.AppendLine(method.Description);
            }

            sb.AppendLine();
        }
    }

    private static void AppendBasicExample(StringBuilder sb, Models.ComponentDetails details)
    {
        sb.AppendLine($"# {details.Component.Name} Usage Examples");
        sb.AppendLine();
        sb.AppendLine("## Basic Usage");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.Append($"<{details.Component.Name}");

        if (details.Component.IsGeneric)
        {
            sb.Append(" TItem=\"YourItemType\"");
        }

        var commonParams = details.Parameters
            .Where(p => !p.IsInherited && ToolOutputHelper.IsCommonExampleParam(p.Name))
            .Take(3)
            .ToList();

        foreach (var param in commonParams)
        {
            sb.Append($" {param.Name}=\"{ToolOutputHelper.GetExampleValue(param)}\"");
        }

        var hasChildContent = details.Parameters.Any(p =>
            p.Name.Equals("ChildContent", StringComparison.OrdinalIgnoreCase) ||
            p.Type.Contains("RenderFragment"));

        if (hasChildContent)
        {
            sb.AppendLine(">");
            sb.AppendLine("    <!-- Your content here -->");
            sb.AppendLine($"</{details.Component.Name}>");
        }
        else
        {
            sb.AppendLine(" />");
        }

        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendEventHandlingExample(StringBuilder sb, Models.ComponentDetails details)
    {
        if (details.Events.Count == 0)
        {
            return;
        }

        sb.AppendLine("## With Event Handling");
        sb.AppendLine();
        sb.AppendLine("```razor");
        sb.AppendLine("@code {");

        foreach (var evt in details.Events.Take(2))
        {
            var eventType = ToolOutputHelper.ExtractEventType(evt.Type);
            sb.AppendLine($"    private void On{evt.Name.Replace("On", "")}({eventType} args)");
            sb.AppendLine("    {");
            sb.AppendLine("        // Handle event");
            sb.AppendLine("    }");
        }

        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendCommonParameters(StringBuilder sb, Models.ComponentDetails details)
    {
        sb.AppendLine("## Common Parameters");
        sb.AppendLine();

        var importantParams = details.Parameters
            .Where(p => !p.IsInherited)
            .Take(5);

        foreach (var param in importantParams)
        {
            sb.AppendLine($"- `{param.Name}` ({param.Type}): {ToolOutputHelper.TruncateSummary(param.Description, 60)}");
        }
    }
}

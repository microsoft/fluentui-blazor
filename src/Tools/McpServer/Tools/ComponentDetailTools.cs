// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.FluentUI.AspNetCore.McpServer.Helpers;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tools;

/// <summary>
/// MCP tools for getting detailed component documentation.
/// </summary>
[McpServerToolType]
public class ComponentDetailTools
{
    private readonly FluentUIDocumentationService _documentationService;
    private readonly ComponentDocumentationService _componentDocService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentDetailTools"/> class.
    /// </summary>
    /// <param name="documentationService">The documentation service.</param>
    /// <param name="componentDocService">The component documentation service.</param>
    public ComponentDetailTools(FluentUIDocumentationService documentationService, ComponentDocumentationService componentDocService)
    {
        _documentationService = documentationService;
        _componentDocService = componentDocService;
    }

    /// <summary>
    /// Gets detailed documentation for a specific Fluent UI Blazor component, including all its parameters, properties, events, and methods.
    /// </summary>
    /// <param name="componentName">The name of the component (e.g., 'FluentButton', 'FluentDataGrid', 'FluentTextField'). You can omit the 'Fluent' prefix.</param>
    /// <returns>
    /// A string containing the detailed documentation for the specified component, or a message indicating that the component was not found.
    /// </returns>
    [McpServerTool]
    [Description("Gets detailed documentation for a specific Fluent UI Blazor component, including usage guide, code examples, parameters, events, and methods.")]
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
        AppendUsageDocumentation(sb, componentName);
        AppendParameters(sb, details);
        AppendEvents(sb, details);
        AppendMethods(sb, details);

        return sb.ToString();
    }

    private void AppendUsageDocumentation(StringBuilder sb, string componentName)
    {
        var usageDocs = _componentDocService.GetComponentDocumentation(componentName);
        if (string.IsNullOrEmpty(usageDocs))
        {
            return;
        }

        sb.AppendLine("## Usage Guide");
        sb.AppendLine();
        sb.AppendLine(usageDocs);
        sb.AppendLine();
    }

    private static void AppendComponentHeader(StringBuilder sb, Models.ComponentDetails details)
    {
        sb.AppendLine(CultureInfo.InvariantCulture, $"# {details.Component.Name}");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(details.Component.Summary))
        {
            sb.AppendLine(details.Component.Summary);
            sb.AppendLine();
        }

        if (!string.IsNullOrEmpty(details.Component.BaseClass))
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Base Class:** {details.Component.BaseClass}");
        }

        if (details.Component.IsGeneric)
        {
            sb.AppendLine("**Generic Component:** Yes (requires type parameter)");
        }

        sb.AppendLine(CultureInfo.InvariantCulture, $"**Category:** {details.Component.Category}");
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
            sb.AppendLine(CultureInfo.InvariantCulture, $"| {param.Name} | `{param.Type}` | {defaultValue} | {description}{enumHint} |");
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
            sb.AppendLine(CultureInfo.InvariantCulture, $"| {evt.Name} | `{evt.Type}` | {ToolOutputHelper.TruncateSummary(evt.Description, 80)} |");
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
            sb.AppendLine(CultureInfo.InvariantCulture, $"### {method.Name}");
            sb.AppendLine();
            sb.AppendLine($"```csharp");
            sb.AppendLine(CultureInfo.InvariantCulture, $"{method.Signature}");
            sb.AppendLine($"```");

            if (!string.IsNullOrEmpty(method.Description))
            {
                sb.AppendLine();
                sb.AppendLine(method.Description);
            }

            sb.AppendLine();
        }
    }
}

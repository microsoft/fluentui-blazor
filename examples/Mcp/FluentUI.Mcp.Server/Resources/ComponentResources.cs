// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using FluentUI.Mcp.Server.Services;
using FluentUI.Mcp.Server.Tools;
using ModelContextProtocol.Server;

namespace FluentUI.Mcp.Server.Resources;

/// <summary>
/// MCP Resource templates for accessing specific component documentation.
/// These are templated resources that accept parameters.
/// </summary>
[McpServerResourceType]
public class ComponentResources
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentResources"/> class.
    /// </summary>
    public ComponentResources(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Gets detailed documentation for a specific component.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://component/{name}",
        Name = "component",
        Title = "Component Documentation",
        MimeType = "text/markdown")]
    [Description("Detailed documentation for a specific Fluent UI Blazor component including parameters, events, and methods.")]
    public string GetComponent(string name)
    {
        var details = _documentationService.GetComponentDetails(name);

        if (details == null)
        {
            return $"# Component Not Found\n\nComponent '{name}' was not found. Check the components list for available components.";
        }

        var sb = new StringBuilder();

        // Header
        sb.AppendLine($"# {details.Component.Name}");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(details.Component.Summary))
        {
            sb.AppendLine(details.Component.Summary);
            sb.AppendLine();
        }

        sb.AppendLine($"**Category:** {details.Component.Category}");

        if (!string.IsNullOrEmpty(details.Component.BaseClass))
        {
            sb.AppendLine($"**Base Class:** {details.Component.BaseClass}");
        }

        if (details.Component.IsGeneric)
        {
            sb.AppendLine("**Generic Component:** Yes (requires type parameter)");
        }

        sb.AppendLine();

        // Parameters
        if (details.Parameters.Count > 0)
        {
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

        // Events
        if (details.Events.Count > 0)
        {
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

        // Methods
        if (details.Methods.Count > 0)
        {
            sb.AppendLine("## Methods");
            sb.AppendLine();

            foreach (var method in details.Methods)
            {
                sb.AppendLine($"### {method.Name}");
                sb.AppendLine();
                sb.AppendLine("```csharp");
                sb.AppendLine(method.Signature);
                sb.AppendLine("```");

                if (!string.IsNullOrEmpty(method.Description))
                {
                    sb.AppendLine();
                    sb.AppendLine(method.Description);
                }

                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets components in a specific category.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://category/{name}",
        Name = "category",
        Title = "Components by Category",
        MimeType = "text/markdown")]
    [Description("List of all components in a specific category.")]
    public string GetCategory(string name)
    {
        var components = _documentationService.GetComponentsByCategory(name);

        if (components.Count == 0)
        {
            return $"# Category Not Found\n\nNo components found in category '{name}'.";
        }

        var sb = new StringBuilder();
        sb.AppendLine($"# {name} Components");
        sb.AppendLine();
        sb.AppendLine($"Total: {components.Count} components");
        sb.AppendLine();

        foreach (var component in components.OrderBy(c => c.Name))
        {
            var genericIndicator = component.IsGeneric ? "<T>" : "";
            sb.AppendLine($"## {component.Name}{genericIndicator}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(component.Summary))
            {
                sb.AppendLine(component.Summary);
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets detailed information about a specific enum.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://enum/{name}",
        Name = "enum",
        Title = "Enum Type Details",
        MimeType = "text/markdown")]
    [Description("Detailed information about a specific enum type including all values.")]
    public string GetEnum(string name)
    {
        var enumInfo = _documentationService.GetEnumDetails(name);

        if (enumInfo == null)
        {
            return $"# Enum Not Found\n\nEnum '{name}' was not found. Check the enums list for available enum types.";
        }

        var sb = new StringBuilder();
        sb.AppendLine($"# {enumInfo.Name}");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(enumInfo.Description))
        {
            sb.AppendLine(enumInfo.Description);
            sb.AppendLine();
        }

        sb.AppendLine("## Values");
        sb.AppendLine();
        sb.AppendLine("| Name | Value | Description |");
        sb.AppendLine("|------|-------|-------------|");

        foreach (var value in enumInfo.Values)
        {
            sb.AppendLine($"| {value.Name} | {value.Value} | {value.Description} |");
        }

        return sb.ToString();
    }
}

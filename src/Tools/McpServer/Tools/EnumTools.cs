// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tools;

/// <summary>
/// MCP tools for accessing Fluent UI Blazor enum documentation.
/// </summary>
[McpServerToolType]
public class EnumTools
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumTools"/> class.
    /// </summary>
    /// <param name="documentationService">The documentation service.</param>
    public EnumTools(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Gets information about a specific enum type used in Fluent UI Blazor components, including all possible values.
    /// </summary>
    /// <param name="enumName">The name of the enum type (e.g., 'Appearance', 'Color', 'Size', 'Orientation').</param>
    /// <param name="filter">Optional: Filter to show only values matching this search term.</param>
    /// <returns>
    /// A string containing the enum's name, description, and a table of its values and their descriptions.
    /// If the enum is not found, returns a message indicating so.
    /// </returns>
    [McpServerTool]
    [Description("Gets information about a specific enum type used in Fluent UI Blazor components, including all possible values.")]
    public string GetEnumValues(
        [Description("The name of the enum type (e.g., 'Appearance', 'Color', 'Size', 'Orientation').")]
        string enumName,
        [Description("Optional: Filter to show only values matching this search term.")]
        string? filter = null)
    {
        var enumInfo = _documentationService.GetEnumDetails(enumName);

        if (enumInfo == null)
        {
            return $"Enum '{enumName}' not found. Use ListEnums() to see all available enums.";
        }

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# {enumInfo.Name}");
        sb.AppendLine();

        if (!string.IsNullOrEmpty(enumInfo.Description))
        {
            sb.AppendLine(enumInfo.Description);
            sb.AppendLine();
        }

        var values = enumInfo.Values.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(filter))
        {
            values = values.Where(v =>
                v.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                v.Description.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }

        var valuesList = values.ToList();
        if (valuesList.Count == 0)
        {
            return $"No values found matching filter '{filter}' in enum '{enumName}'.";
        }

        sb.AppendLine("## Values");
        sb.AppendLine();
        sb.AppendLine("| Name | Value | Description |");
        sb.AppendLine("|------|-------|-------------|");

        foreach (var value in valuesList)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"| {value.Name} | {value.Value} | {value.Description} |");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Lists all enum types used by a specific Fluent UI Blazor component, showing which property/parameter uses each enum.
    /// </summary>
    /// <param name="componentName">
    /// The name of the component (e.g., 'FluentButton', 'FluentDataGrid', 'FluentTextField'). You can omit the 'Fluent' prefix.
    /// </param>
    /// <returns>
    /// A string containing a list of enum types used by the specified component, including which property or parameter uses each enum.
    /// If the component is not found, returns a message indicating so.
    /// </returns>
    [McpServerTool]
    [Description("Lists all enum types used by a specific Fluent UI Blazor component, showing which property/parameter uses each enum.")]
    public string GetComponentEnums(
        [Description("The name of the component (e.g., 'FluentButton', 'FluentDataGrid', 'FluentTextField'). You can omit the 'Fluent' prefix.")] 
        string componentName)
    {
        var enumsByProperty = _documentationService.GetEnumsForComponent(componentName);

        if (enumsByProperty.Count == 0)
        {
            var details = _documentationService.GetComponentDetails(componentName);
            if (details == null)
            {
                return $"Component '{componentName}' not found. Use ListComponents() to see all available components.";
            }

            return $"No enum types found for component '{componentName}'.";
        }

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Enum Types for {componentName}");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Found {enumsByProperty.Count} enum type(s) used by this component:");
        sb.AppendLine();

        foreach (var kvp in enumsByProperty.OrderBy(k => k.Key))
        {
            var propertyName = kvp.Key;
            var enumInfo = kvp.Value;

            sb.AppendLine(CultureInfo.InvariantCulture, $"## {propertyName} → {enumInfo.Name}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(enumInfo.Description))
            {
                sb.AppendLine(enumInfo.Description);
                sb.AppendLine();
            }

            sb.AppendLine("| Value | Description |");
            sb.AppendLine("|-------|-------------|");

            foreach (var value in enumInfo.Values.Take(10))
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"| {value.Name} | {value.Description} |");
            }

            if (enumInfo.Values.Count > 10)
            {
                sb.AppendLine(CultureInfo.InvariantCulture, $"| ... | *{enumInfo.Values.Count - 10} more values* |");
            }

            sb.AppendLine();
        }

        sb.AppendLine("Use `GetEnumValues(enumName: \"EnumName\")` to see all values for a specific enum.");

        return sb.ToString();
    }
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.FluentUI.AspNetCore.McpServer.Services;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Resources;

/// <summary>
/// MCP Resources providing static documentation content for Fluent UI Blazor components.
/// These resources are user-selected and provide context for the LLM.
/// </summary>
[McpServerResourceType]
public class FluentUIResources
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentUIResources"/> class.
    /// </summary>
    public FluentUIResources(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    /// <summary>
    /// Gets the complete list of all Fluent UI Blazor components.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://components",
        Name = "components",
        Title = "All Fluent UI Blazor Components",
        MimeType = "text/markdown")]
    [Description("Complete list of all available Fluent UI Blazor components organized by category.")]
    public string GetAllComponents()
    {
        var components = _documentationService.GetAllComponents();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor Components");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Total: {components.Count} components");
        sb.AppendLine();

        var groupedByCategory = components.GroupBy(c => c.Category, StringComparer.OrdinalIgnoreCase).OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

        foreach (var group in groupedByCategory)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"## {group.Key}");
            sb.AppendLine();

            foreach (var component in group.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase))
            {
                var genericIndicator = component.IsGeneric ? "<T>" : "";
                sb.AppendLine(CultureInfo.InvariantCulture, $"- **{component.Name}{genericIndicator}**: {component.Summary}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets all enum types available in the library.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://enums",
        Name = "enums",
        Title = "All Enum Types",
        MimeType = "text/markdown")]
    [Description("Complete list of all enum types used in Fluent UI Blazor components.")]
    public string GetAllEnums()
    {
        var enums = _documentationService.GetAllEnums();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor Enum Types");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Total: {enums.Count} enums");
        sb.AppendLine();

        foreach (var enumInfo in enums)
        {
            var values = string.Join(", ", enumInfo.Values.Select(v => v.Name));
            sb.AppendLine(CultureInfo.InvariantCulture, $"## {enumInfo.Name}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(enumInfo.Description))
            {
                sb.AppendLine(enumInfo.Description);
                sb.AppendLine();
            }

            sb.AppendLine(CultureInfo.InvariantCulture, $"**Values:** {values}");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}

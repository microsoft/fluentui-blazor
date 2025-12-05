// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using FluentUI.Mcp.Server.Services;
using ModelContextProtocol.Server;

namespace FluentUI.Mcp.Server.Resources;

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
        sb.AppendLine($"Total: {components.Count} components");
        sb.AppendLine();

        var groupedByCategory = components.GroupBy(c => c.Category).OrderBy(g => g.Key);

        foreach (var group in groupedByCategory)
        {
            sb.AppendLine($"## {group.Key}");
            sb.AppendLine();

            foreach (var component in group.OrderBy(c => c.Name))
            {
                var genericIndicator = component.IsGeneric ? "<T>" : "";
                sb.AppendLine($"- **{component.Name}{genericIndicator}**: {component.Summary}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Gets all component categories with their counts.
    /// </summary>
    [McpServerResource(
        UriTemplate = "fluentui://categories",
        Name = "categories",
        Title = "Component Categories",
        MimeType = "text/markdown")]
    [Description("List of all Fluent UI Blazor component categories with component counts.")]
    public string GetCategories()
    {
        var categories = _documentationService.GetCategories();
        var allComponents = _documentationService.GetAllComponents();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor Component Categories");
        sb.AppendLine();

        foreach (var category in categories)
        {
            var count = allComponents.Count(c => c.Category == category);
            var componentNames = string.Join(", ", allComponents
                .Where(c => c.Category == category)
                .OrderBy(c => c.Name)
                .Select(c => c.Name));

            sb.AppendLine($"## {category} ({count} components)");
            sb.AppendLine();
            sb.AppendLine(componentNames);
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
        sb.AppendLine($"Total: {enums.Count} enums");
        sb.AppendLine();

        foreach (var enumInfo in enums)
        {
            var values = string.Join(", ", enumInfo.Values.Select(v => v.Name));
            sb.AppendLine($"## {enumInfo.Name}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(enumInfo.Description))
            {
                sb.AppendLine(enumInfo.Description);
                sb.AppendLine();
            }

            sb.AppendLine($"**Values:** {values}");
            sb.AppendLine();
        }

        return sb.ToString();
    }
}

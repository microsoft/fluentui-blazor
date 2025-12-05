// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using FluentUI.Mcp.Server.Services;
using ModelContextProtocol.Server;

namespace FluentUI.Mcp.Server.Tools;

/// <summary>
/// MCP tools for listing and searching Fluent UI Blazor components.
/// </summary>
[McpServerToolType]
public class ComponentListTools
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentListTools"/> class.
    /// </summary>
    /// <param name="documentationService">The documentation service.</param>
    public ComponentListTools(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerTool]
    [Description("Lists all available Fluent UI Blazor components with their names and brief descriptions. Use this to discover what components are available in the library.")]
    public string ListComponents(
        [Description("Optional: Filter components by category (e.g., 'Button', 'Input', 'Dialog', 'DataGrid', 'Layout', 'Menu', 'Navigation', 'Card', 'Icon')")]
        string? category = null)
    {
        var components = string.IsNullOrEmpty(category)
            ? _documentationService.GetAllComponents()
            : _documentationService.GetComponentsByCategory(category);

        if (components.Count == 0)
        {
            return category != null
                ? $"No components found in category '{category}'. Use ListCategories() to see available categories."
                : "No components found.";
        }

        var sb = new StringBuilder();
        sb.AppendLine($"# Fluent UI Blazor Components ({components.Count} found)");
        sb.AppendLine();

        var groupedByCategory = components.GroupBy(c => c.Category).OrderBy(g => g.Key);

        foreach (var group in groupedByCategory)
        {
            sb.AppendLine($"## {group.Key}");
            sb.AppendLine();

            foreach (var component in group.OrderBy(c => c.Name))
            {
                var genericIndicator = component.IsGeneric ? "<T>" : "";
                sb.AppendLine($"- **{component.Name}{genericIndicator}**: {ToolOutputHelper.TruncateSummary(component.Summary, 100)}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    [McpServerTool]
    [Description("Searches for Fluent UI Blazor components by name or description. Use this when you're looking for a component that does something specific.")]
    public string SearchComponents(
        [Description("The term to search for in component names and descriptions (e.g., 'button', 'grid', 'input', 'dialog').")]
        string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return "Please provide a search term.";
        }

        var components = _documentationService.SearchComponents(searchTerm);

        if (components.Count == 0)
        {
            return $"No components found matching '{searchTerm}'. Try a different search term or use ListComponents() to see all components.";
        }

        var sb = new StringBuilder();
        sb.AppendLine($"# Search Results for '{searchTerm}' ({components.Count} found)");
        sb.AppendLine();

        foreach (var component in components)
        {
            var genericIndicator = component.IsGeneric ? "<T>" : "";
            sb.AppendLine($"## {component.Name}{genericIndicator}");
            sb.AppendLine();
            sb.AppendLine($"**Category:** {component.Category}");
            sb.AppendLine();

            if (!string.IsNullOrEmpty(component.Summary))
            {
                sb.AppendLine(component.Summary);
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    [McpServerTool]
    [Description("Lists all available component categories to help navigate the Fluent UI Blazor library.")]
    public string ListCategories()
    {
        var categories = _documentationService.GetCategories();
        var allComponents = _documentationService.GetAllComponents();

        var sb = new StringBuilder();
        sb.AppendLine("# Fluent UI Blazor Component Categories");
        sb.AppendLine();

        foreach (var category in categories)
        {
            var count = allComponents.Count(c => c.Category == category);
            sb.AppendLine($"- **{category}** ({count} components)");
        }

        sb.AppendLine();
        sb.AppendLine("Use `ListComponents(category: \"CategoryName\")` to see components in a specific category.");

        return sb.ToString();
    }
}

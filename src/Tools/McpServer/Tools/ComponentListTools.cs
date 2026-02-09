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
/// MCP tools for listing and searching Fluent UI Blazor components.
/// </summary>
[McpServerToolType]
public class ComponentListTools
{
    private readonly FluentUIDocumentationService _documentationService;
    private readonly ComponentDocumentationService _componentDocService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentListTools"/> class.
    /// </summary>
    /// <param name="documentationService">The documentation service.</param>
    /// <param name="componentDocService">The component documentation service.</param>
    public ComponentListTools(FluentUIDocumentationService documentationService, ComponentDocumentationService componentDocService)
    {
        _documentationService = documentationService;
        _componentDocService = componentDocService;
    }

    /// <summary>
    /// Lists all available Fluent UI Blazor components with their names and brief descriptions.
    /// Optionally filters the components by category.
    /// </summary>
    /// <param name="category">Optional: Filter components by category (e.g., 'Button', 'Input', 'Dialog', 'DataGrid', 'Layout', 'Menu', 'Navigation', 'Card', 'Icon').</param>
    /// <returns>
    /// A formatted string listing the available Fluent UI Blazor components, grouped by category.
    /// If a category is specified, only components in that category are listed.
    /// </returns>
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
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Fluent UI Blazor Components ({components.Count} found)");
        sb.AppendLine();

        var groupedByCategory = components.GroupBy(c => c.Category, StringComparer.OrdinalIgnoreCase).OrderBy(g => g.Key, StringComparer.OrdinalIgnoreCase);

        foreach (var group in groupedByCategory)
        {
            sb.AppendLine(CultureInfo.InvariantCulture, $"## {group.Key}");
            sb.AppendLine();

            foreach (var component in group.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase))
            {
                var genericIndicator = component.IsGeneric ? "<T>" : "";
                sb.AppendLine(CultureInfo.InvariantCulture, $"- **{component.Name}{genericIndicator}**: {ToolOutputHelper.TruncateSummary(component.Summary, 100)}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>
    /// Searches for Fluent UI Blazor components by name or description.
    /// Use this when you're looking for a component that does something specific.
    /// </summary>
    /// <param name="searchTerm">The term to search for in component names and descriptions (e.g., 'button', 'grid', 'input', 'dialog').</param>
    /// <returns>
    /// A formatted string listing the matching Fluent UI Blazor components, including their names, categories, and summaries.
    /// If no components match, a message indicating no results are found is returned.
    /// </returns>
    [McpServerTool]
    [Description("Searches for Fluent UI Blazor components by name, description, or documentation content. Use this when you're looking for a component that does something specific.")]
    public string SearchComponents(
        [Description("The term to search for in component names, descriptions, and documentation (e.g., 'button', 'grid', 'input', 'dialog', 'loading', 'modal').")]
        string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return "Please provide a search term.";
        }

        var allResults = MergeSearchResults(searchTerm);

        if (allResults.Count == 0)
        {
            return $"No components found matching '{searchTerm}'. Try a different search term or use ListComponents() to see all components.";
        }

        var sb = new StringBuilder();
        sb.AppendLine(CultureInfo.InvariantCulture, $"# Search Results for '{searchTerm}' ({allResults.Count} found)");
        sb.AppendLine();

        foreach (var component in allResults)
        {
            var genericIndicator = component.IsGeneric ? "<T>" : "";
            sb.AppendLine(CultureInfo.InvariantCulture, $"## {component.Name}{genericIndicator}");
            sb.AppendLine();
            sb.AppendLine(CultureInfo.InvariantCulture, $"**Category:** {component.Category}");
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
    /// Merges search results from API documentation and component documentation content.
    /// </summary>
    private List<Models.ComponentInfo> MergeSearchResults(string searchTerm)
    {
        var components = _documentationService.SearchComponents(searchTerm);
        var docMatches = _componentDocService.SearchDocumentation(searchTerm);
        var existingNames = new HashSet<string>(components.Select(c => c.Name), StringComparer.OrdinalIgnoreCase);
        var allComponents = _documentationService.GetAllComponents();

        // Build a lookup dictionary for O(1) name resolution instead of O(N) scans
        var componentLookup = allComponents
            .SelectMany(c => new[]
            {
                (Key: c.Name, Value: c),
                (Key: c.Name.StartsWith("Fluent", StringComparison.OrdinalIgnoreCase) ? c.Name["Fluent".Length..] : $"Fluent{c.Name}", Value: c)
            })
            .GroupBy(x => x.Key, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().Value, StringComparer.OrdinalIgnoreCase);

        var additionalComponents = docMatches
            .Where(docName => !existingNames.Contains(docName))
            .Select(docName => componentLookup.GetValueOrDefault(docName))
            .Where(match => match != null && existingNames.Add(match.Name))
            .Select(match => match!)
            .ToList();

        // Sort the combined results by name for stable output
        return [.. components.Concat(additionalComponents).OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase)];
    }
}

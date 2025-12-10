// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq;
using System.Reflection;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

/// <summary>
/// Service for extracting documentation from the Fluent UI Blazor components assembly.
/// </summary>
public class FluentUIDocumentationService
{
    private readonly Assembly _componentsAssembly;
    private readonly ComponentInfoFactory _componentFactory;
    private readonly EnumInfoFactory _enumFactory;
    private readonly Dictionary<string, ComponentInfo> _componentCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, EnumInfo> _enumCache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentUIDocumentationService"/> class.
    /// </summary>
    /// <param name="componentsAssembly">The Fluent UI components assembly.</param>
    /// <param name="xmlDocumentationPath">Optional path to the XML documentation file.</param>
    public FluentUIDocumentationService(Assembly componentsAssembly, string? xmlDocumentationPath = null)
    {
        _componentsAssembly = componentsAssembly;

        var xmlReader = new XmlDocumentationReader(xmlDocumentationPath);
        _componentFactory = new ComponentInfoFactory(xmlReader);
        _enumFactory = new EnumInfoFactory(xmlReader);

        InitializeCache();
    }

    /// <summary>
    /// Initializes the component and enum caches.
    /// </summary>
    private void InitializeCache()
    {
        foreach (var type in _componentsAssembly.GetTypes().Where(ComponentCategoryHelper.IsValidComponentType))
        {
            var componentInfo = _componentFactory.CreateComponentInfo(type);
            _componentCache[componentInfo.Name] = componentInfo;
        }

        foreach (var type in _componentsAssembly.GetTypes().Where(t => t.IsEnum && t.IsPublic))
        {
            var enumInfo = _enumFactory.CreateEnumInfo(type);
            _enumCache[enumInfo.Name] = enumInfo;
        }
    }

    /// <summary>
    /// Gets all available components.
    /// </summary>
    /// <returns>A list of all components.</returns>
    public IReadOnlyList<ComponentInfo> GetAllComponents()
    {
        return _componentCache.Values.OrderBy(c => c.Name).ToList();
    }

    /// <summary>
    /// Gets components filtered by category.
    /// </summary>
    /// <param name="category">The category to filter by.</param>
    /// <returns>A list of components in the specified category.</returns>
    public IReadOnlyList<ComponentInfo> GetComponentsByCategory(string category)
    {
        return _componentCache.Values
            .Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.Name)
            .ToList();
    }

    /// <summary>
    /// Searches for components by name or description.
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <returns>A list of matching components.</returns>
    public IReadOnlyList<ComponentInfo> SearchComponents(string searchTerm)
    {
        var lowerSearch = searchTerm.ToLowerInvariant();
        return _componentCache.Values
            .Where(c => c.Name.Contains(lowerSearch, StringComparison.OrdinalIgnoreCase) ||
                        c.Summary.Contains(lowerSearch, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.Name)
            .ToList();
    }

    /// <summary>
    /// Gets detailed information about a specific component.
    /// </summary>
    /// <param name="componentName">The name of the component.</param>
    /// <returns>Detailed component information, or null if not found.</returns>
    public ComponentDetails? GetComponentDetails(string componentName)
    {
        if (!_componentCache.TryGetValue(componentName, out var componentInfo))
        {
            // Try to find with "Fluent" prefix
            if (!_componentCache.TryGetValue($"Fluent{componentName}", out componentInfo))
            {
                return null;
            }
        }

        var type = _componentsAssembly.GetType(componentInfo.FullName);
        if (type == null)
        {
            return null;
        }

        return _componentFactory.CreateComponentDetails(type, componentInfo);
    }

    /// <summary>
    /// Gets all available enums.
    /// </summary>
    /// <returns>A list of all enums.</returns>
    public IReadOnlyList<EnumInfo> GetAllEnums()
    {
        return _enumCache.Values.OrderBy(e => e.Name).ToList();
    }

    /// <summary>
    /// Gets detailed information about a specific enum.
    /// </summary>
    /// <param name="enumName">The name of the enum.</param>
    /// <returns>Enum information, or null if not found.</returns>
    public EnumInfo? GetEnumDetails(string enumName)
    {
        _enumCache.TryGetValue(enumName, out var enumInfo);
        return enumInfo;
    }

    /// <summary>
    /// Gets all enums used by a specific component.
    /// </summary>
    /// <param name="componentName">The name of the component.</param>
    /// <returns>A dictionary of property names to their enum info.</returns>
    public Dictionary<string, EnumInfo> GetEnumsForComponent(string componentName)
    {
        var result = new Dictionary<string, EnumInfo>(StringComparer.OrdinalIgnoreCase);
        var details = GetComponentDetails(componentName);

        if (details == null)
        {
            return result;
        }

        // Get enums from parameters
        foreach (var param in details.Parameters)
        {
            var enumInfo = FindEnumForType(param.Type);
            if (enumInfo != null && !result.ContainsKey(param.Name))
            {
                result[param.Name] = enumInfo;
            }
        }

        // Get enums from properties
        foreach (var prop in details.Properties)
        {
            var enumInfo = FindEnumForType(prop.Type);
            if (enumInfo != null && !result.ContainsKey(prop.Name))
            {
                result[prop.Name] = enumInfo;
            }
        }

        return result;
    }

    /// <summary>
    /// Finds an enum info by type name (handles nullable types).
    /// </summary>
    private EnumInfo? FindEnumForType(string typeName)
    {
        // Remove nullable suffix
        var cleanTypeName = typeName.TrimEnd('?');

        // Try direct match
        if (_enumCache.TryGetValue(cleanTypeName, out var enumInfo))
        {
            return enumInfo;
        }

        // Try to find by partial match (e.g., "Appearance" in the cache)
        var match = _enumCache.Values.FirstOrDefault(e =>
            e.Name.Equals(cleanTypeName, StringComparison.OrdinalIgnoreCase) ||
            e.FullName.EndsWith($".{cleanTypeName}", StringComparison.OrdinalIgnoreCase));

        return match;
    }

    /// <summary>
    /// Gets all available categories.
    /// </summary>
    /// <returns>A list of all categories.</returns>
    public IReadOnlyList<string> GetCategories()
    {
        return _componentCache.Values
            .Select(c => c.Category)
            .Where(c => !string.IsNullOrEmpty(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}

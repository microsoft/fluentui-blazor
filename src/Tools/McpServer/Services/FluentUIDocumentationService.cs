// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Models;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Services;

/// <summary>
/// Service for providing Fluent UI Blazor component documentation.
/// Uses pre-generated JSON data for fast, dependency-free access.
/// </summary>
public class FluentUIDocumentationService
{
    private readonly JsonDocumentationReader _reader;
    private readonly Dictionary<string, ComponentInfo> _componentCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, EnumInfo> _enumCache = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentUIDocumentationService"/> class.
    /// </summary>
    /// <param name="jsonDocumentationPath">Optional path to the JSON documentation file. If null, uses embedded resource.</param>
    public FluentUIDocumentationService(string? jsonDocumentationPath = null)
    {
        _reader = new JsonDocumentationReader(jsonDocumentationPath);
        InitializeCache();
    }

    /// <summary>
    /// Initializes the component and enum caches from JSON data.
    /// </summary>
    private void InitializeCache()
    {
        foreach (var componentInfo in _reader.GetAllComponents().Select(ConvertToComponentInfo))
        {
            _componentCache[componentInfo.Name] = componentInfo;
        }

        foreach (var enumInfo in _reader.GetAllEnums().Select(ConvertToEnumInfo))
        {
            _enumCache[enumInfo.Name] = enumInfo;
        }
    }

    /// <summary>
    /// Gets all available components.
    /// </summary>
    /// <returns>A list of all components.</returns>
    public IReadOnlyList<ComponentInfo> GetAllComponents()
    {
        return [.. _componentCache.Values.OrderBy(c => c.Name)];
    }

    /// <summary>
    /// Gets components filtered by category.
    /// </summary>
    /// <param name="category">The category to filter by.</param>
    /// <returns>A list of components in the specified category.</returns>
    public IReadOnlyList<ComponentInfo> GetComponentsByCategory(string category)
    {
        return [.. _componentCache.Values
            .Where(c => c.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.Name)];
    }

    /// <summary>
    /// Searches for components by name or description.
    /// </summary>
    /// <param name="searchTerm">The search term.</param>
    /// <returns>A list of matching components.</returns>
    public IReadOnlyList<ComponentInfo> SearchComponents(string searchTerm)
    {
        var lowerSearch = searchTerm.ToLowerInvariant();
        return [.. _componentCache.Values
            .Where(c => c.Name.Contains(lowerSearch, StringComparison.OrdinalIgnoreCase) ||
                        c.Summary.Contains(lowerSearch, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.Name)];
    }

    /// <summary>
    /// Gets detailed information about a specific component.
    /// </summary>
    /// <param name="componentName">The name of the component.</param>
    /// <returns>Detailed component information, or null if not found.</returns>
    public ComponentDetails? GetComponentDetails(string componentName)
    {
        var jsonComponent = _reader.GetComponent(componentName);
        if (jsonComponent == null)
        {
            return null;
        }

        if (!_componentCache.TryGetValue(jsonComponent.Name, out var componentInfo))
        {
            return null;
        }

        return ConvertToComponentDetails(jsonComponent, componentInfo);
    }

    /// <summary>
    /// Gets all available enums.
    /// </summary>
    /// <returns>A list of all enums.</returns>
    public IReadOnlyList<EnumInfo> GetAllEnums()
    {
        return [.. _enumCache.Values.OrderBy(e => e.Name)];
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

        // Try to find by partial match
        var match = _enumCache.Values.FirstOrDefault(e =>
            e.Name.Equals(cleanTypeName, StringComparison.OrdinalIgnoreCase) ||
            e.FullName.EndsWith($".{cleanTypeName}", StringComparison.OrdinalIgnoreCase));

        return match;
    }

    /// <summary>
    /// Converts JSON component info to ComponentInfo.
    /// </summary>
    private static ComponentInfo ConvertToComponentInfo(JsonComponentInfo json)
    {
        return new ComponentInfo
        {
            Name = json.Name,
            FullName = json.FullName,
            Summary = json.Summary,
            Category = json.Category,
            IsGeneric = json.IsGeneric,
            BaseClass = json.BaseClass
        };
    }

    /// <summary>
    /// Converts JSON enum info to EnumInfo.
    /// </summary>
    private static EnumInfo ConvertToEnumInfo(JsonEnumInfo json)
    {
        return new EnumInfo
        {
            Name = json.Name,
            FullName = json.FullName,
            Description = json.Description,
            Values = [.. json.Values.Select(v => new EnumValueInfo
            {
                Name = v.Name,
                Value = v.Value,
                Description = v.Description
            })]
        };
    }

    /// <summary>
    /// Converts JSON component to ComponentDetails.
    /// </summary>
    private static ComponentDetails ConvertToComponentDetails(JsonComponentInfo json, ComponentInfo componentInfo)
    {
        var properties = json.Properties.Select(p => new Models.PropertyInfo
        {
            Name = p.Name,
            Type = p.Type,
            Description = p.Description,
            IsParameter = p.IsParameter,
            IsInherited = p.IsInherited,
            DefaultValue = p.DefaultValue,
            EnumValues = p.EnumValues
        }).ToList();

        var events = json.Events.Select(e => new Models.EventInfo
        {
            Name = e.Name,
            Type = e.Type,
            Description = e.Description,
            IsInherited = e.IsInherited
        }).ToList();

        var methods = json.Methods.Select(m => new Models.MethodInfo
        {
            Name = m.Name,
            ReturnType = m.ReturnType,
            Description = m.Description,
            Parameters = m.Parameters,
            IsInherited = m.IsInherited
        }).ToList();

        return new ComponentDetails
        {
            Component = componentInfo,
            Parameters = [.. properties.Where(p => p.IsParameter).OrderBy(p => p.Name)],
            Properties = [.. properties.OrderBy(p => p.Name)],
            Events = [.. events.OrderBy(e => e.Name)],
            Methods = [.. methods.OrderBy(m => m.Name)]
        };
    }
}

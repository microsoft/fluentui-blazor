// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

/// <summary>
/// Helper class for component categorization.
/// </summary>
internal static class ComponentCategoryHelper
{
    /// <summary>
    /// Determines the category of a component based on its namespace or name.
    /// </summary>
    public static string DetermineCategory(Type type)
    {
        var ns = type.Namespace ?? string.Empty;
        var name = type.Name;

        // Extract category from namespace
        var categoryFromNamespace = GetCategoryFromNamespace(ns);
        if (categoryFromNamespace != null)
        {
            return categoryFromNamespace;
        }

        // Categorize by component name patterns
        return GetCategoryFromName(name);
    }

    /// <summary>
    /// Tries to extract category from namespace.
    /// </summary>
    private static string? GetCategoryFromNamespace(string ns)
    {
        if (ns.Contains(".Components.", StringComparison.OrdinalIgnoreCase))
        {
            var parts = ns.Split('.');
            var componentsIndex = Array.IndexOf(parts, "Components");
            if (componentsIndex >= 0 && componentsIndex < parts.Length - 1)
            {
                return parts[componentsIndex + 1];
            }
        }

        return null;
    }

    /// <summary>
    /// Gets category from component name patterns.
    /// </summary>
    private static string GetCategoryFromName(string name)
    {
        if (name.Contains("Button", StringComparison.OrdinalIgnoreCase))
        {
            return "Button";
        }

        if (name.Contains("Input", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("TextField", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("TextArea", StringComparison.OrdinalIgnoreCase))
        {
            return "Input";
        }

        if (name.Contains("Dialog", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Modal", StringComparison.OrdinalIgnoreCase))
        {
            return "Dialog";
        }

        if (name.Contains("Menu", StringComparison.OrdinalIgnoreCase))
        {
            return "Menu";
        }

        if (name.Contains("Nav", StringComparison.OrdinalIgnoreCase))
        {
            return "Navigation";
        }

        if (name.Contains("Grid", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Table", StringComparison.OrdinalIgnoreCase))
        {
            return "DataGrid";
        }

        if (name.Contains("Card", StringComparison.OrdinalIgnoreCase))
        {
            return "Card";
        }

        if (name.Contains("Icon", StringComparison.OrdinalIgnoreCase))
        {
            return "Icon";
        }

        if (name.Contains("Layout", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Stack", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Splitter", StringComparison.OrdinalIgnoreCase))
        {
            return "Layout";
        }

        return "Components";
    }

    /// <summary>
    /// Checks if a type is a valid FluentUI Blazor component type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is a valid FluentUI Blazor component; otherwise, false.</returns>
    /// <remarks>
    /// We use IFluentComponentBase instead of ComponentBase or IComponent because:
    /// <list type="number">
    /// <item>All FluentUI Blazor components implement IFluentComponentBase (either directly or through FluentComponentBase/FluentInputBase)</item>
    /// <item>This ensures we only document components that are part of the FluentUI Blazor library</item>
    /// <item>IFluentComponentBase provides the common properties (Id, Class, Style, etc.) that are specific to FluentUI components</item>
    /// <item>This excludes generic Blazor components or third-party components that might be in the assembly</item>
    /// </list>
    /// </remarks>
    public static bool IsValidComponentType(Type type)
    {
        return type != null &&
               type.IsPublic &&
               !type.IsAbstract &&
               !type.IsInterface &&
               type.IsClass &&
               !ExcludedTypes.Contains(type.Name) &&
               !type.Name.Contains('<') &&
               !type.Name.Contains('>') &&
               !type.Name.EndsWith("_g") &&
               typeof(IFluentComponentBase).IsAssignableFrom(type);
    }

    /// <summary>
    /// Types to exclude from the documentation.
    /// </summary>
    private static readonly HashSet<string> ExcludedTypes =
    [
        "TypeInference",
        "InternalListContext`1",
        "SpacingGenerator",
        "FluentLocalizerInternal",
        "FluentJSModule",
        "FluentServiceProviderException`1",
    ];
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;

/// <summary>
/// Helper class for type-related operations.
/// </summary>
internal static partial class TypeHelper
{
    /// <summary>
    /// Gets a friendly type name.
    /// </summary>
    public static string GetTypeName(Type type)
    {
        if (type == typeof(void))
        {
            return "void";
        }

        if (type == typeof(string))
        {
            return "string";
        }

        if (type == typeof(int))
        {
            return "int";
        }

        if (type == typeof(bool))
        {
            return "bool";
        }

        if (type == typeof(double))
        {
            return "double";
        }

        if (type == typeof(float))
        {
            return "float";
        }

        if (type == typeof(decimal))
        {
            return "decimal";
        }

        if (type == typeof(object))
        {
            return "object";
        }

        // Nullable types
        var nullableType = Nullable.GetUnderlyingType(type);
        if (nullableType != null)
        {
            return GetTypeName(nullableType) + "?";
        }

        // Generic types
        if (type.IsGenericType)
        {
            return GetGenericTypeName(type);
        }

        return type.Name;
    }

    /// <summary>
    /// Gets the name of a generic type.
    /// </summary>
    private static string GetGenericTypeName(Type type)
    {
        var genericDef = type.GetGenericTypeDefinition();
        var genericArgs = type.GetGenericArguments();

        // EventCallback<T>
        if (genericDef == typeof(Microsoft.AspNetCore.Components.EventCallback<>))
        {
            return $"EventCallback<{GetTypeName(genericArgs[0])}>";
        }

        // Other generics
        var baseName = type.Name;
        var tickIndex = baseName.IndexOf('`');
        if (tickIndex > 0)
        {
            baseName = baseName[..tickIndex];
        }

        return $"{baseName}<{string.Join(", ", genericArgs.Select(GetTypeName))}>";
    }

    /// <summary>
    /// Gets the enum values for a type.
    /// </summary>
    public static string[] GetEnumValues(Type type)
    {
        var actualType = Nullable.GetUnderlyingType(type) ?? type;
        if (actualType.IsEnum)
        {
            return Enum.GetNames(actualType);
        }

        return [];
    }

    /// <summary>
    /// Checks if a type is an EventCallback.
    /// </summary>
    public static bool IsEventCallback(Type type)
    {
        return type == typeof(Microsoft.AspNetCore.Components.EventCallback) ||
               (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Microsoft.AspNetCore.Components.EventCallback<>));
    }

    /// <summary>
    /// Cleans up the summary text.
    /// </summary>
    public static string CleanSummary(string? summary)
    {
        if (string.IsNullOrWhiteSpace(summary))
        {
            return string.Empty;
        }

        // Remove XML tags and normalize whitespace
        var cleaned = XmlTagRegex().Replace(summary, " ");
        cleaned = WhitespaceRegex().Replace(cleaned, " ");
        return cleaned.Trim();
    }

    [GeneratedRegex(@"<[^>]+>")]
    private static partial Regex XmlTagRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
internal static class AdditionalAttributesExtensions
{
    /// <summary>
    /// Returns the value of the additional attribute with the specified name.
    /// </summary>
    /// <param name="attributes">Additional attributes</param>
    /// <param name="name">Name of the attribute</param>
    /// <param name="defaultValue">Default value to return if the attribute is not found</param>
    /// <returns>The value of the attribute, or the default value if not found</returns>
    public static object? GetValueOrDefault(this IReadOnlyDictionary<string, object>? attributes, string name, object? defaultValue = null)
    {
        // Returns the found attribute value
        if (attributes is not null &&
            attributes.TryGetValue(name, out var value))
        {
            return value;
        }

        // Or default value if not found
        return defaultValue;
    }

    /// <summary>
    /// Returns the value of the additional attribute with the specified name if it is not found.
    /// </summary>
    /// <param name="attributes">Additional attributes</param>
    /// <param name="name">Name of the attribute</param>
    /// <param name="value">Value to return if the attribute is not found</param>
    /// <param name="when">Condition to check, to return the value</param>
    /// <returns>null if the attribute is found, or if the condition is not met</returns>
    public static object? GetValueIfNoAdditionalAttribute(this IReadOnlyDictionary<string, object>? attributes, string name, object? value, Func<bool> when)
    {
        // Returns null if the attribute is found
        if (attributes is not null &&
            attributes.ContainsKey(name))
        {
            return null;
        }

        // Or the value if not found and the condition is met
        return when() ? value : null;
    }
}

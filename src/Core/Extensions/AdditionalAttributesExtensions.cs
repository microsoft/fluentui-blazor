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
    public static AdditionalAttributeCondition GetValueIfNoAdditionalAttribute(this IReadOnlyDictionary<string, object>? attributes, string name, object? value, Func<bool>? when = null)
    {
        return new AdditionalAttributeCondition(attributes).GetValueIfNoAdditionalAttribute(name, value, when);
    }

    /// <summary>
    /// Returns the value of the additional attribute with the specified name if it is not found.
    /// </summary>
    internal class AdditionalAttributeCondition
    {
        private readonly IReadOnlyDictionary<string, object>? _additionalAttributes;
        private readonly List<AdditionalAttributeConditionItem> _listOfConditions = [];

        /// <summary />
        internal AdditionalAttributeCondition(IReadOnlyDictionary<string, object>? attributes)
        {
            _additionalAttributes = attributes;
        }

        /// <summary>
        /// Returns the value of the additional attribute with the specified name if it is not found.
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value to return if the attribute is not found</param>
        /// <param name="when">Condition to check, to return the value</param>
        /// <returns>null if the attribute is found, or if the condition is not met</returns>
        public AdditionalAttributeCondition GetValueIfNoAdditionalAttribute(string name, object? value, Func<bool>? when = null)
        {
            var item = new AdditionalAttributeConditionItem(name, value, when is null ? () => true : when);
            _listOfConditions.Add(item);
            return this;
        }

        /// <summary>
        /// Returns the value of the additional attribute with the specified name if it is not found.
        /// </summary>
        /// <returns></returns>
        public object? Build()
        {
            foreach (var item in _listOfConditions)
            {
                if (_additionalAttributes is not null &&
                    _additionalAttributes.ContainsKey(item.Name))
                {
                    return null;
                }

                if (item.When())
                {
                    return item.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the value of the additional attribute with the specified name if it is not found.
        /// </summary>
        /// <returns></returns>
        public override string? ToString()
        {
            return Build()?.ToString() ?? null;
        }

        private record AdditionalAttributeConditionItem(string Name, object? Value, Func<bool> When);
    }
}

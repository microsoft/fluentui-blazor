// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Attribute used to mark external static properties as default values for component parameters.
/// The attribute takes the component type name as a parameter, and at component initialization,
/// properties with matching defaults (by property name and component type) are assigned the default value if they are unset.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class FluentDefaultAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentDefaultAttribute"/> class.
    /// </summary>
    /// <param name="componentTypeName">The name of the component type that this default value applies to.</param>
    public FluentDefaultAttribute(string componentTypeName)
    {
        ComponentTypeName = componentTypeName ?? throw new ArgumentNullException(nameof(componentTypeName));
    }

    /// <summary>
    /// Gets the name of the component type that this default value applies to.
    /// </summary>
    public string ComponentTypeName { get; }

    /// <summary>
    /// Gets or sets the name of the component parameter this default value applies to.
    /// If not specified, the property name is used as the parameter name.
    /// This allows multiple properties to map to the same parameter name for different components.
    /// </summary>
    public string? ParameterName { get; set; }
}
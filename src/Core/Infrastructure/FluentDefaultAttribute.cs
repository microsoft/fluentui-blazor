// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies that a static class provides default values for a FluentUI component.
/// The property names in the defaults class must match the component parameter names exactly.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class FluentDefaultAttribute : Attribute
{
    /// <summary>
    /// Gets the component type name that this defaults class applies to.
    /// </summary>
    public string ComponentType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentDefaultAttribute"/> class.
    /// </summary>
    /// <param name="componentType">The component type name (e.g., "FluentButton").</param>
    public FluentDefaultAttribute(string componentType)
    {
        ComponentType = componentType ?? throw new ArgumentNullException(nameof(componentType));
    }
}
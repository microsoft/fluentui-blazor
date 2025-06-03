// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Attribute used to mark external static properties as default values for component parameters.
/// The attribute takes the component type name as a parameter, and at component initialization,
/// properties with matching defaults (by property name and component type) are assigned the default value if they are unset.
/// 
/// For Blazor WASM trimming compatibility:
/// - Static classes containing FluentDefault properties should be preserved with [DynamicDependency] attributes
/// - Consider using targeted assembly/namespace scanning instead of full AppDomain scanning
/// - Component types referenced in FluentDefault attributes should be preserved
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class FluentDefaultAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentDefaultAttribute"/> class.
    /// </summary>
    /// <param name="componentTypeName">The name of the component type that this default value applies to.</param>
    public FluentDefaultAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] string componentTypeName)
    {
        ComponentTypeName = componentTypeName ?? throw new ArgumentNullException(nameof(componentTypeName));
    }

    /// <summary>
    /// Gets the name of the component type that this default value applies to.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public string ComponentTypeName { get; }

    /// <summary>
    /// Gets or sets the name of the component parameter this default value applies to.
    /// If not specified, the property name is used as the parameter name.
    /// This allows multiple properties to map to the same parameter name for different components.
    /// </summary>
    public string? ParameterName { get; set; }
}
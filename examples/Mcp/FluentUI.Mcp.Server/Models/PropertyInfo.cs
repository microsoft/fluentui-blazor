// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Mcp.Server.Models;

/// <summary>
/// Represents a property of a component.
/// </summary>
public record PropertyInfo
{
    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the type of the property.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Gets the description of the property.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets whether this property is a [Parameter].
    /// </summary>
    public bool IsParameter { get; init; }

    /// <summary>
    /// Gets whether this property is inherited.
    /// </summary>
    public bool IsInherited { get; init; }

    /// <summary>
    /// Gets the default value of the property.
    /// </summary>
    public string? DefaultValue { get; init; }

    /// <summary>
    /// Gets the enum values if this property is an enum type.
    /// </summary>
    public string[] EnumValues { get; init; } = [];
}

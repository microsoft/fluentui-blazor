// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.McpDocumentation;

/// <summary>
/// Represents a property of a component.
/// </summary>
public class McpPropertyInfo
{
    /// <summary>
    /// Gets or sets the name of the property.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of the property.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the property.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether this property is a [Parameter].
    /// </summary>
    public bool IsParameter { get; set; }

    /// <summary>
    /// Gets or sets whether this property is inherited.
    /// </summary>
    public bool IsInherited { get; set; }

    /// <summary>
    /// Gets or sets the default value of the property.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the enum values if this property is an enum type.
    /// </summary>
    public string[] EnumValues { get; set; } = [];
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Represents a property of a component.
/// </summary>
public class PropertyInfo
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
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets whether this property is a [Parameter].
    /// </summary>
    public bool IsParameter { get; set; }

    /// <summary>
    /// Gets or sets whether this property is inherited.
    /// Only serialized when true to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsInherited { get; set; }

    /// <summary>
    /// Gets or sets the default value of the property.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the enum values if this property is an enum type.
    /// Only serialized when not empty to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string[]? EnumValues { get; set; }
}

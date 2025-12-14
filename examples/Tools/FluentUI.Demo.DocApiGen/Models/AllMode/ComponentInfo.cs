// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Represents a Fluent UI component with its full documentation.
/// </summary>
public class ComponentInfo
{
    /// <summary>
    /// Gets or sets the name of the component.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full type name of the component.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a brief description of the component.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Summary { get; set; }

    /// <summary>
    /// Gets or sets the category of the component.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets whether the component is a generic type.
    /// Only serialized when true to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsGeneric { get; set; }

    /// <summary>
    /// Gets or sets the base class name.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BaseClass { get; set; }

    /// <summary>
    /// Gets or sets the list of properties.
    /// Only serialized when not empty to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<Models.AllMode.PropertyInfo>? Properties { get; set; }

    /// <summary>
    /// Gets or sets the list of events.
    /// Only serialized when not empty to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<Models.AllMode.EventInfo>? Events { get; set; }

    /// <summary>
    /// Gets or sets the list of methods.
    /// Only serialized when not empty to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<Models.AllMode.MethodInfo>? Methods { get; set; }
}

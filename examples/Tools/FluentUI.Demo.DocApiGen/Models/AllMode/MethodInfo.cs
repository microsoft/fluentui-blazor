// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Represents a method of a component.
/// </summary>
public class MethodInfo
{
    /// <summary>
    /// Gets or sets the name of the method.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the return type of the method.
    /// </summary>
    public string ReturnType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the method.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the parameters of the method.
    /// Only serialized when not empty to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string[]? Parameters { get; set; }

    /// <summary>
    /// Gets or sets whether this method is inherited.
    /// Only serialized when true to reduce JSON size.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsInherited { get; set; }
}

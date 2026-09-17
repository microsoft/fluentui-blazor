// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Represents an enum value.
/// </summary>
public class EnumValueInfo
{
    /// <summary>
    /// Gets or sets the name of the enum value.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the numeric value.
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Gets or sets the description of the enum value.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Description { get; set; }
}

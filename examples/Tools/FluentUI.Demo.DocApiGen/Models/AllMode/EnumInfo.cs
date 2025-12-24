// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace FluentUI.Demo.DocApiGen.Models.AllMode;

/// <summary>
/// Represents an enum type.
/// </summary>
public class EnumInfo
{
    /// <summary>
    /// Gets or sets the name of the enum.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the full type name of the enum.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the enum.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the enum values.
    /// </summary>
    public List<EnumValueInfo> Values { get; set; } = [];
}

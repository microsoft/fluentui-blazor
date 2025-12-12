// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen.Models.McpDocumentation;

/// <summary>
/// Represents an enum type.
/// </summary>
public class McpEnumInfo
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
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the enum values.
    /// </summary>
    public List<McpEnumValueInfo> Values { get; set; } = [];
}

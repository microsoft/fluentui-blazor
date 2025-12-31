// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.McpServer.Models;

/// <summary>
/// Represents an enum type.
/// </summary>
public record EnumInfo
{
    /// <summary>
    /// Gets the name of the enum.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the full type name of the enum.
    /// </summary>
    public required string FullName { get; init; }

    /// <summary>
    /// Gets the description of the enum.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets the enum values.
    /// </summary>
    public IReadOnlyList<EnumValueInfo> Values { get; init; } = [];
}

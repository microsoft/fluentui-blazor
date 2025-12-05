// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Mcp.Server.Models;

/// <summary>
/// Represents an enum value.
/// </summary>
public record EnumValueInfo
{
    /// <summary>
    /// Gets the name of the enum value.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the numeric value.
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Gets the description of the enum value.
    /// </summary>
    public string Description { get; init; } = string.Empty;
}

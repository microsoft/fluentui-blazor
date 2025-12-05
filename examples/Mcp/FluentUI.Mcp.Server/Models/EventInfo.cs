// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Mcp.Server.Models;

/// <summary>
/// Represents an event of a component.
/// </summary>
public record EventInfo
{
    /// <summary>
    /// Gets the name of the event.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Gets the type of the event callback.
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    /// Gets the description of the event.
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Gets whether this event is inherited.
    /// </summary>
    public bool IsInherited { get; init; }
}

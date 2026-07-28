// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents an item that may be subject to overflow handling.
/// </summary>
public record OverflowItem
{
    /// <summary>
    /// Gets the unique identifier of the overflow item.
    /// </summary>
    public string? Id { get; init; }

    /// <summary>
    /// Gets a value indicating whether the item is in overflow.
    /// </summary>
    public bool Overflow { get; init; }

    /// <summary>
    /// Gets the text associated with the overflow item.
    /// </summary>
    public string? Text { get; init; }

    /// <summary>
    /// Gets the overflow behavior of the item.
    /// </summary>
    public OverflowBehavior? Behavior { get; init; }

    /// <summary>
    /// Gets the index of the overflow item.
    /// </summary>
    public int Index { get; init; }
}


// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents the current overflow state.
/// </summary>
internal record OverflowState
{
    /// <summary>
    /// Gets the items that are currently in overflow.
    /// </summary>
    public OverflowItem[]? OverflowItems { get; init; }

    /// <summary>
    /// Gets the count of items in overflow.
    /// </summary>
    public int OverflowCount { get; init; }

    /// <summary>
    /// Gets the index of the first item in overflow.
    /// </summary>
    public int FirstOverflowIndex { get; init; }

    /// <summary>
    /// Gets the ordered item identifiers.
    /// </summary>
    public string[]? OrderedItemIds { get; init; }
}


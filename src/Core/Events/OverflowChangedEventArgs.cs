// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentOverflow overflow change event.
/// </summary>
public class OverflowChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets or sets the ID of the overflow component.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the rendered overflow items included in the event payload.
    /// </summary>
    public IReadOnlyList<OverflowChangedItem>? Items { get; set; }

    /// <summary>
    /// Gets or sets the total number of items currently in overflow.
    /// </summary>
    public int OverflowCount { get; set; }

    /// <summary>
    /// Gets or sets the index of the first overflowed item in DOM order.
    /// </summary>
    public int FirstOverflowIndex { get; set; } = -1;
}

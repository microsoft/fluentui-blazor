// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents one overflow item state passed by the overflowchange custom event.
/// </summary>
public class OverflowChangedItem
{
    /// <summary>
    /// Gets or sets the item identifier.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets whether the item is currently in overflow.
    /// </summary>
    public bool Overflow { get; set; }

    /// <summary>
    /// Gets or sets the item text.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Gets or sets the fixed behavior.
    /// </summary>
    public string? Fixed { get; set; }

    /// <summary>
    /// Gets or sets the item index.
    /// </summary>
    public int Index { get; set; }
}

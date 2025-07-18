// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Determines the horizontal alignment of the content within the <see cref="FluentStack"/>.
/// </summary>
public enum HorizontalAlignment
{
    /// <summary>
    /// The content is aligned to the left.
    /// </summary>
    Left,

    /// <summary>
    /// The content is aligned to the start.
    /// </summary>
    Start,

    /// <summary>
    /// The content is center aligned.
    /// </summary>
    Center,

    /// <summary>
    /// The content is aligned to the right.
    /// </summary>
    Right,

    /// <summary>
    /// The content is aligned to the end.
    /// </summary>
    End,

    /// <summary>
    /// The content is stretched to fill the available space.
    /// </summary>
    Stretch,

    /// <summary>
    /// The items are evenly distributed within the alignment container along the main axis.
    /// </summary>
    SpaceBetween,
}

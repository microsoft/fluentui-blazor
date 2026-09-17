// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Determines the horizontal alignment of the content within the <see cref="FluentStack"/>.
/// </summary>
public enum HorizontalAlignment
{
    /// <summary>
    /// The content is aligned to the left.
    /// Use <see cref="Start"/> for better RTL support.
    /// </summary>
    [Description("flex-start")]
    Left,

    /// <summary>
    /// The content is aligned to the start.
    /// </summary>
    [Description("flex-start")]
    Start,

    /// <summary>
    /// The content is center aligned.
    /// </summary>
    [Description("center")]
    Center,

    /// <summary>
    /// The content is aligned to the right.
    /// Use <see cref="End"/> for better RTL support.
    /// </summary>
    [Description("flex-end")]
    Right,

    /// <summary>
    /// The content is aligned to the end.
    /// </summary>
    [Description("flex-end")]
    End,

    /// <summary>
    /// The content is stretched to fill the available space.
    /// </summary>
    [Description("stretch")]
    Stretch,

    /// <summary>
    /// The items are evenly distributed within the alignment container along the main axis.
    /// </summary>
    [Description("space-between")]
    SpaceBetween,

    /// <summary>
    /// Aligns content at the baseline of the container.
    /// </summary>
    [Description("baseline")]
    Baseline,
}

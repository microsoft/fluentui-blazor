namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Determines the vertical alignment of the content within the <see cref="FluentStack"/>.
/// </summary>
public enum VerticalAlignment
{
    /// <summary>
    /// The content is aligned to the top.
    /// </summary>
    Top,

    /// <summary>
    /// The content is center aligned.
    /// </summary>
    Center,

    /// <summary>
    /// The content is aligned to the bottom
    /// </summary>
    Bottom,

    /// <summary>
    /// The content is stretched to fill the available space.
    /// </summary>
    Stretch,

    /// <summary>
    /// The items are evenly distributed within the alignment container along the main axis.
    /// </summary>
    SpaceBetween,
}

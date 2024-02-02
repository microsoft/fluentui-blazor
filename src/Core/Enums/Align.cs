using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes alignment for a <see cref="FluentDataGrid{TGridItem}"/> column, <see cref="FluentOverlay"/> content, etc.
/// </summary>
public enum Align
{
    /// <summary>
    /// Aligns the content against at the start of the container.
    /// </summary>
    [Description("flex-start")]
    Start,

    /// <summary>
    /// Aligns the content at the center of the container.
    /// </summary>
    [Description("center")]
    Center,

    /// <summary>
    /// Aligns the content at the end of the container.
    /// </summary>
    [Description("flex-end")]
    End,

    /// <summary>
    /// Aligns content to strech to the container.
    /// </summary>
    [Description("stretch")]
    Stretch,

    /// <summary>
    /// Aligns content at the baseline of the container.
    /// </summary>
    [Description("baseline")]
    Baseline

    ///// <summary>
    ///// Justifies the content against the left of the container.
    ///// </summary>
    //Left,

    ///// <summary>
    ///// Justifies the content at the right of the container.
    ///// </summary>
    //Right,
}

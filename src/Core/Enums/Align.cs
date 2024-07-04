using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes alignment for a <see cref="FluentDataGrid{TGridItem}"/> column, <see cref="FluentOverlay"/> content, etc.
/// </summary>
public enum Align
{
    /// <summary>
    /// Aligns the content against at the start of the container.
    /// </summary>
    [Display(Name = "flex-start")]
    Start,

    /// <summary>
    /// Aligns the content at the center of the container.
    /// </summary>
    [Display(Name = "center")]
    Center,

    /// <summary>
    /// Aligns the content at the end of the container.
    /// </summary>
    [Display(Name = "flex-end")]
    End,

    /// <summary>
    /// Aligns content to strech to the container.
    /// </summary>
    [Display(Name = "stretch")]
    Stretch,

    /// <summary>
    /// Aligns content at the baseline of the container.
    /// </summary>
    [Display(Name = "baseline")]
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

using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the alignment of items along the main axis of a flex container.
/// </summary>
public enum JustifyContent
{
    /// <summary>
    /// Justify content flex-start.
    /// </summary>
    [Display(Name = "flex-start")]
    FlexStart,

    /// <summary>
    /// Justify content center.
    /// </summary>
    [Display(Name = "center")]
    Center,

    /// <summary>
    /// Justify content flex-end.
    /// </summary>
    [Display(Name = "flex-end")]
    FlexEnd,

    /// <summary>
    /// Justify content space-between.
    /// </summary>
    [Display(Name = "space-between")]
    SpaceBetween,

    /// <summary>
    /// Justify content space-around.
    /// </summary>
    [Display(Name = "space-around")]
    SpaceAround,

    /// <summary>
    /// Justify content space-evenly.
    /// </summary>
    [Display(Name = "space-evenly")]
    SpaceEvenly
}

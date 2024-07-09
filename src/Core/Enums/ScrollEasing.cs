using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The easing function to use when scrolling.
/// </summary>
public enum ScrollEasing
{
    /// <summary>
    /// Linear easing.
    /// </summary>
    Linear,

    /// <summary>
    /// Ease in.
    /// </summary>
    [Display(Name = "ease-in")]
    EaseIn,

    /// <summary>
    /// Ease out.
    /// </summary>
    [Display(Name = "ease-out")]
    EaseOut,

    /// <summary>
    /// Ease in then out.
    /// </summary>
    [Display(Name = "ease-in-out")]
    EaseInOut
}

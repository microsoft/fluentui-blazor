using System.ComponentModel;

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
    [Description("ease-in")]
    EaseIn,

    /// <summary>
    /// Ease out.
    /// </summary>
    [Description("ease-out")]
    EaseOut,

    /// <summary>
    /// Ease in then out.
    /// </summary>
    [Description("ease-in-out")]
    EaseInOut
}

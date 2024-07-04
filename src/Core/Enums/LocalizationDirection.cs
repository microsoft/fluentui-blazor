using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The (reading) direction of objects in the UI.
/// </summary>
public enum LocalizationDirection
{
    /// <summary>
    /// Left to right.
    /// </summary>
    // TODO: #vNext: Remove this value in the next major version.
    [Obsolete("This value has been replaced with 'LeftToRight' and will be deleted in the next major version.")]
    [Display(Name = "ltr")]
    ltr,

    /// <summary>
    /// Right to left.
    /// </summary>
    // TODO: #vNext: Remove this value in the next major version.
    [Obsolete("This value has been replaced with 'RightToLeft' and will be deleted in the next major version.")]
    [Display(Name = "rtl")]
    rtl,

    /// <summary>
    /// Left to right.
    /// </summary>
    [Display(Name = "ltr")]
    LeftToRight,

    /// <summary>
    /// Right to left.
    /// </summary>
    [Display(Name = "rtl")]
    RightToLeft
}

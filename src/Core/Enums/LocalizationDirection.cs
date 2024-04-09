using System.ComponentModel;

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
    [Description("ltr")]
    ltr,

    /// <summary>
    /// Right to left.
    /// </summary>
    // TODO: #vNext: Remove this value in the next major version.
    [Obsolete("This value has been replaced with 'RightToLeft' and will be deleted in the next major version.")]
    [Description("rtl")]
    rtl,

    /// <summary>
    /// Left to right.
    /// </summary>
    [Description("ltr")]
    LeftToRight,

    /// <summary>
    /// Right to left.
    /// </summary>
    [Description("rtl")]
    RightToLeft
}

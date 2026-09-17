// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Values to define the scaling behavior of a FluentAnchoredRegion component on a particular axis.
/// </summary>
[Obsolete("This enum is not supported anymore and will be removed in a future release.")]

public enum AxisScalingMode
{
    /// <summary>
    /// The axis will scale to the content.
    /// </summary>
    Content,

    /// <summary>
    /// The axis will scale to the content or the anchor, whichever is larger.
    /// </summary>
    Fill,

    /// <summary>
    /// The axis will scale to the anchor.
    /// </summary>
    Anchor,
}

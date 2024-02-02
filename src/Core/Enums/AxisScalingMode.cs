namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Values to define the scaling behavior of a <see cref="FluentAnchoredRegion"/> component on a particular axis.
/// </summary>
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
    Anchor
}

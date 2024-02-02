namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
///  Defines if a <see cref="FluentAnchoredRegion"/> component updates its position automatically. 
/// </summary>
public enum AutoUpdateMode
{
    /// <summary>
    /// The position only updates when the anchor resizes 
    /// </summary>
    Anchor,

    /// <summary>
    /// The position is updated when:
    /// - update() is called
    /// - the anchor resizes
    /// - the window resizes
    /// - the viewport resizes
    /// - any scroll event in the document
    /// This is the default setting
    /// </summary>
    Auto
}

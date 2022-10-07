namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
///  Defines if a <see cref="FluentAnchoredRegion"/> component updates its position automatically. 
/// </summary>
public enum AutoUpdateMode
{
    /// <summary>
    /// The position only updates when the anchor resizes (default)
    /// </summary>
    Anchor,

    /// <summary>
    /// The position is updated when:
    /// - update() is called
    /// - the anchor resizes
    /// - the window resizes
    /// - the viewport resizes
    /// - any scroll event in the document
    /// </summary>
    Auto
}
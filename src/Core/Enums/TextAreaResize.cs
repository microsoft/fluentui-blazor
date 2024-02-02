namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the direction in which a <see cref="FluentTextArea"/> can resize.
/// </summary>
public enum TextAreaResize
{
    /// <summary>
    /// The textarea can only resize horizontally.
    /// </summary>
    Horizontal,

    /// <summary>
    /// The textarea can only resize vertically.
    /// </summary>
    Vertical,

    /// <summary>
    /// The textarea can resize both horizontally and vertically.
    /// </summary>
    Both
}

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Values to define arrow keys available for navigation in the group of elements.
/// </summary>
public enum ArrowNavigationGroupMode
{
    /// <summary>
    /// Default, focus can be moved to the previous using Up/Left and to the next using Down/Right buttons.
    /// </summary>
    Both,

    /// <summary>
    /// Focus can be moved using only Up/Down buttons.
    /// </summary>
    Vertical,

    /// <summary>
    /// Focus can be moved using only Left/Right buttons.
    /// </summary>
    Horizontal,

    /// <summary>
    /// Focus can be moved using arrow keys in two dimensions, movement depends on the visual placement of elements.
    /// </summary>
    Grid,

    /// <summary>
    /// Same as <see cref="Grid"/> and also allows linear movement.
    /// </summary>
    GridLinear
}
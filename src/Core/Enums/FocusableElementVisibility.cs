namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Focusable element visibility states.
/// Values to define focus behavior when it enters container element.
/// </summary>
public enum FocusableElementVisibility
{
    /// <summary>
    /// Element is not in the viewport or less that 25% of it is visible.
    /// </summary>
    Invisible,

    /// <summary>
    /// At least 25% of element is visible
    /// </summary>
    PartiallyVisible,

    /// <summary>
    /// 75% of element or more is visible.
    /// </summary>
    Visible
}
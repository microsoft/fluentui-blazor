namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Parameters for tracking focus using data attributes
/// </summary>
public sealed class FocusTrackingParameters
{
    /// <summary>
    /// Adds 'data-fui-focus-visible' attribute to element when it has focus.
    /// </summary>
    public bool FocusVisible { get; set; }

    /// <summary>
    /// Adds 'data-fui-focus-within' attribute to element when it or one of its child elements is focused.
    /// </summary>
    public bool FocusWithin { get; set; }
}
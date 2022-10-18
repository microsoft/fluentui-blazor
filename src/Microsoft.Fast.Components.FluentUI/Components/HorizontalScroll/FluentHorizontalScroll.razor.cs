using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentHorizontalScroll : FluentComponentBase
{
    /// <summary>
    /// Description: Scroll speed in pixels per second
    /// </summary>
    [Parameter]
    public int Speed { get; set; } = 600;

    /// <summary>
    /// The CSS time value for the scroll transition duration. Overrides the `speed` attribute.
    /// </summary>
    [Parameter]
    public string? Duration { get; set; }

    /// <summary>
    /// Attribute used for easing, defaults to ease-in-out
    /// </summary>
    [Parameter]
    public ScrollEasing? Easing { get; set; } = ScrollEasing.EaseInOut;

    /// <summary>
    /// Attribute to hide flippers from assistive technology
    /// </summary>
    [Parameter]
    public bool? FlippersHiddenFromAt { get; set; }

    /// <summary>
    /// View: default | mobile
    /// </summary>
    [Parameter]
    public HorizontalScrollView? View { get; set; } = FluentUI.HorizontalScrollView.Default;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
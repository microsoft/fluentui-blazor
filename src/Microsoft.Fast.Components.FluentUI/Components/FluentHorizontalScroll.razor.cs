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
    /// Description: Scroll easing
    /// Possible values: linear, ease-in, ease-out or ease-in-out
    /// </summary>
    [Parameter]
    public string? Easing { get; set; }
}
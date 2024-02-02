namespace Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;

/// <summary>
/// Global options for tooltips.
/// </summary>
public class TooltipGlobalOptions
{
    /// <summary>
    /// Default delay (in milliseconds).
    /// </summary>
    public const int DefaultDelay = 300;

    /// <summary>
    /// Gets or sets the delay (in milliseconds). Default is 300.
    /// </summary>
    public int Delay { get; set; } = DefaultDelay;

    /// <summary>
    /// Gets or sets the default tooltip's position.
    /// See <see cref="TooltipPosition"/>
    /// </summary>
    public TooltipPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the default maximum width of tooltip panel.
    /// Default is 500px.
    /// </summary>
    public string? MaxWidth { get; set; } = "500px";

    /// <summary>
    /// Gets or sets whether the horizontal viewport is locked
    /// </summary>
    public bool HorizontalViewportLock { get; set; } = false;

    /// <summary>
    /// Gets or sets whether the vertical viewport is locked
    /// </summary>
    public bool VerticalViewportLock { get; set; } = false;
}

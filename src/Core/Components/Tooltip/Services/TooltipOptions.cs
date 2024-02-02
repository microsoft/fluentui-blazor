using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;

/// <summary>
/// Options for a tooltip.
/// </summary>
public class TooltipOptions
{
    /// <summary>
    /// Gets or sets the unique identifier of the tooltip.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the anchor identifier of the tooltip.
    /// </summary>
    public string Anchor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tooltip content.
    /// </summary>
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the tooltip panel width.
    /// </summary>
    public string? MaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the delay (in milliseconds). 
    /// Default is 300.
    /// </summary>
    [Parameter]
    public int? Delay { get; set; } = TooltipGlobalOptions.DefaultDelay;

    /// <summary>
    /// Gets or sets the tooltip's position. See <see cref="TooltipPosition"/>.
    /// </summary>
    [Parameter]
    public TooltipPosition? Position { get; set; }

    /// <summary>
    /// Callback for when the tooltip is dismissed.
    /// </summary>  
    [Parameter]
    public EventCallback<EventArgs> OnDismissed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tooltip is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }
}

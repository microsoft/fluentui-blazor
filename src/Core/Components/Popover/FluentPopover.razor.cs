using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentPopover : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// Gets or sets the id of the component the popover is positioned relative to
    /// </summary>
    [Parameter]
    public string AnchorId { get; set; } = string.Empty;

    /// <summary>
    /// The default horizontal position of the region relative to the anchor element
    /// Default is unset. See <seealso cref="HorizontalPosition"/>
    /// </summary>
    [Parameter]
    public HorizontalPosition? HorizontalPosition { get; set; } = FluentUI.HorizontalPosition.Unset;

    /// <summary>
    /// Gets or sets popover opened state
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// Callback for when open state changes
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Contents of the header part of the popover
    /// </summary>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    /// Contents of the body part of the popover
    /// </summary>
    [Parameter]
    public RenderFragment? Body { get; set; }

    /// <summary>
    /// Contents of the footer part of the popover
    /// </summary>
    [Parameter]
    public RenderFragment? Footer { get; set; }

    protected override void OnParametersSet()
    {
        if (Header is null && Body is null && Footer is null)
        {
            throw new ArgumentException("At least one of Header, Body or Footer must be set.");
        }
    }

    protected virtual async Task CloseAsync(MouseEventArgs e)
    {
        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
    }
}

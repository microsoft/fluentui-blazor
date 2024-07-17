using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentPopover : FluentComponentBase
{
    private FluentAnchoredRegion AnchoredRegion = default!;

    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary>
    /// Gets or sets the id of the component the popover is positioned relative to.
    /// </summary>
    [Parameter]
    public string AnchorId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default horizontal position of the region relative to the anchor element.
    /// Default is unset. See <seealso cref="HorizontalPosition"/>
    /// </summary>
    [Parameter]
    public HorizontalPosition? HorizontalPosition { get; set; } = AspNetCore.Components.HorizontalPosition.Unset;

    /// <summary>
    /// Gets or sets a value indicating whether the region overlaps the anchor on the horizontal axis.
    /// Default is true which places the region aligned with the anchor element.
    /// </summary>
    [Parameter]
    public bool HorizontalInset { get; set; } = true;

    /// <summary>
    /// Gets or sets the default vertical position of the region relative to the anchor element.
    /// Default is unset. See <seealso cref="VerticalPosition"/>
    /// </summary>
    [Parameter]
    public VerticalPosition? VerticalPosition { get; set; } = AspNetCore.Components.VerticalPosition.Bottom;

    /// <summary>
    /// How short the space allocated to the default position has to be before the tallest area is selected for layout.
    /// </summary>
    [Parameter]
    public int VerticalThreshold { get; set; } = 0;

    /// <summary>
    /// How narrow the space allocated to the default position has to be before the widest area is selected for layout.
    /// </summary>
    [Parameter]
    public int HorizontalThreshold { get; set; } = 0;

    /// <summary>
    /// Gets or sets a value indicating whether the region is positioned using css "position: fixed".
    /// Otherwise the region uses "position: absolute".
    /// Fixed placement allows the region to break out of parent containers.
    /// </summary>
    [Parameter]
    public bool? FixedPlacement { get; set; }

    /// <summary>
    /// Gets or sets popover opened state.
    /// </summary>
    [Parameter]
    public bool Open { get; set; }

    /// <summary>
    /// Callback for when open state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Gets or sets the content of the header part of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? Header { get; set; }

    /// <summary>
    /// Gets or sets the content of the body part of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? Body { get; set; }

    /// <summary>
    /// Gets or sets the content of the footer part of the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? Footer { get; set; }

    /// <summary>
    /// Gets or sets whether the element should receive the focus when the component is loaded.
    /// If this is the case, the user cannot navigate to other elements of the page while the Popup is open.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool AutoFocus { get; set; } = true;

    /// <summary>
    /// Gets or sets the keys that can be used to close the popover.
    /// By default, Escape
    /// </summary>
    [Parameter]
    public KeyCode[]? CloseKeys { get; set; } = new[] { KeyCode.Escape };

    /// <summary />
    private KeyCode[] CloseAndTabKeys => CloseKeys?.Any() == true ? CloseKeys.Union(new[] { KeyCode.Tab }).ToArray() : new[] { KeyCode.Tab };

    /// <summary />
    protected override void OnInitialized()
    {
        if (CloseKeys != null && CloseKeys.Any() && string.IsNullOrEmpty(Id))
        {
            Id = Identifier.NewId();
        }
    }

    /// <summary />
    protected override void OnParametersSet()
    {
        if (Header is null && Body is null && Footer is null)
        {
            throw new ArgumentException("At least one of Header, Body or Footer must be set.");
        }
    }

    /// <summary />
    protected virtual async Task CloseAsync()
    {
        Open = false;
        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(Open);
        }
        await AnchoredRegion.FocusToOriginalElementAsync();
    }

    /// <summary />
    protected virtual async Task CloseOnKeyAsync(FluentKeyCodeEventArgs e)
    {
        if (CloseKeys != null && CloseKeys.Contains(e.Key))
        {
            await CloseAsync();
        }

        if (AutoFocus && e.Key == KeyCode.Tab)
        {
            await AnchoredRegion.FocusToNextElementAsync();
        }
    }
}

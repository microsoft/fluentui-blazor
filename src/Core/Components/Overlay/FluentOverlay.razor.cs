// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverlay : FluentComponentBase, IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Overlay/FluentOverlay.razor.js";

    private string? _color;
    private int _r, _g, _b;

    private const string DEFAULT_NEUTRAL_COLOR = "#808080";

    private DotNetObjectReference<FluentOverlay>? _dotNetHelper;

    /// <summary />
    public FluentOverlay(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-overlay")
        .AddClass("prevent-scroll", PreventScroll)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("cursor", "auto", () => Transparent)
        .AddStyle("background-color", string.Create(CultureInfo.InvariantCulture, $"rgba({_r}, {_g}, {_b}, {Opacity})"), () => !Transparent)
        .AddStyle("cursor", "default", () => !Transparent)
        .AddStyle("position", FullScreen ? "fixed" : "absolute")
        .AddStyle("display", "flex")
        .AddStyle("align-items", Alignment.ToAttributeValue())
        .AddStyle("justify-content", Justification.ToAttributeValue())
        .AddStyle("pointer-events", "none", () => Interactive)
        .AddStyle("z-index", ZIndex.Overlay.ToString(CultureInfo.InvariantCulture))
        .Build();

    /// <summary />
    protected string? StyleContentValue => new StyleBuilder()
        .AddStyle("pointer-events", "auto", () => Interactive)
        .Build();

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; }

    /// <summary>
    /// Callback for when overlay visibility changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    /// <summary>
    /// Callback for when the overlay is closed.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClose { get; set; }

    /// <summary>
    /// Gets or set if the overlay is transparent.
    /// </summary>
    [Parameter]
    public bool Transparent { get; set; } = true;

    /// <summary>
    /// Gets or sets the opacity of the overlay.
    /// Default is 0.4.
    /// </summary>
    [Parameter]
    public double? Opacity { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the content to a <see cref="HorizontalAlignment"/> value.
    /// Defaults to Align.Center.
    /// </summary>
    [Parameter]
    public HorizontalAlignment Alignment { get; set; } = HorizontalAlignment.Center;

    /// <summary>
    /// Gets or sets the justification of the content to a <see cref="AspNetCore.Components.JustifyContent"/> value.
    /// Defaults to JustifyContent.Center.
    /// </summary>
    [Parameter]
    public JustifyContent Justification { get; set; } = JustifyContent.Center;

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is shown full screen or bound to the containing element.
    /// </summary>
    [Parameter]
    public bool FullScreen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is interactive, except for the element with the specified <see cref="InteractiveExceptId"/>.
    /// In other words, the elements below the overlay remain usable (mouse-over, click) and the overlay will closed when clicked.
    /// </summary>
    [Parameter]
    public bool Interactive { get; set; }

    /// <summary>
    /// Gets or sets the HTML identifier of the element that is not interactive when the overlay is shown.
    /// This property is ignored if <see cref="Interactive"/> is false.
    /// </summary>
    [Parameter]
    public string? InteractiveExceptId { get; set; }

    /// <summary>
    /// Gets of sets a value indicating if the overlay can be dismissed by clicking on it.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool Dismissable { get; set; } = true;

    /// <summary>
    /// Gets or sets the background color.
    /// Needs to be formatted as an HTML hex color string (#rrggbb or #rgb)
    /// Default NeutralBaseColor token value (#808080).
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary />
    [Parameter]
    public bool PreventScroll { get; set; }

    /// <summary />
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            _dotNetHelper ??= DotNetObjectReference.Create(this);
            var jsModule = await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            if (Interactive)
            {
                var containerId = FullScreen ? null : Id;
                await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Overlay.Initialize", _dotNetHelper, containerId, InteractiveExceptId);
            }
        }
    }

    /// <summary />
    protected override async Task OnParametersSetAsync()
    {
        if (!Transparent && Opacity is null)
        {
            Opacity = 0.4;
        }

        if (Opacity > 0)
        {
            Transparent = false;
        }

        BackgroundColor ??= DEFAULT_NEUTRAL_COLOR;//GlobalState.NeutralColor ?? DEFAULT_NEUTRAL_COLOR;

        if (!CheckRGBString().IsMatch(BackgroundColor))
        {
            throw new ArgumentException("BackgroundColor must be a valid HTML hex color string (#rrggbb or #rgb)");
        }

        _color = BackgroundColor[1..];

        if (_color.Length == 6)
        {
            _r = int.Parse(_color[..2], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            _g = int.Parse(_color[2..4], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            _b = int.Parse(_color[4..], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }
        else
        {
            _r = int.Parse(_color[0..1], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            _g = int.Parse(_color[1..2], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            _b = int.Parse(_color[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }
    }

    /// <summary />
    [JSInvokable]
    public async Task OnCloseInteractiveAsync(MouseEventArgs e)
    {
        if (!Dismissable || !Visible)
        {
            return;
        }

        // Remove the document.removeEventListener
        //await InvokeOverlayDisposeAsync();

        // Close the overlay
        await OnCloseInternalHandlerAsync(e);
    }

    /// <summary />
    public async Task OnCloseHandlerAsync(MouseEventArgs e)
    {
        if (!Dismissable || !Visible || Interactive)
        {
            return;
        }

        // Close the overlay
        await OnCloseInternalHandlerAsync(e);
    }

    private async Task OnCloseInternalHandlerAsync(MouseEventArgs e)
    {
        Visible = false;

        if (VisibleChanged.HasDelegate)
        {
            await VisibleChanged.InvokeAsync(Visible);
        }

        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync(e);
        }
    }

    /// <summary />
    protected override async ValueTask DisposeAsync(IJSObjectReference jsModule)
    {
        if (Interactive)
        {
            await jsModule.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Overlay.overlayDispose", InteractiveExceptId);
        }
    }

    [GeneratedRegex("^(?:#(?:[a-fA-F0-9]{6}|[a-fA-F0-9]{3}))", RegexOptions.None, matchTimeoutMilliseconds: 1000)] //Add timeout to prevent ReDoS
    private static partial Regex CheckRGBString();
}

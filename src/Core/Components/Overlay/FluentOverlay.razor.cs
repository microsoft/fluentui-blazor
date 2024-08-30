using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverlay : IAsyncDisposable
{
    private readonly string _defaultId = Identifier.NewId();
    private string? _color = null;
    private int _r, _g, _b;

    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Overlay/FluentOverlay.razor.js";
    private DotNetObjectReference<FluentOverlay>? _dotNetHelper = null;

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? _jsModule { get; set; }

    /// <summary />
    protected string? ClassValue => new CssBuilder("fluent-overlay")
        .AddClass("prevent-scroll", PreventScroll)
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("cursor", "auto", () => Transparent)
        .AddStyle("background-color", $"rgba({_r}, {_g}, {_b}, {Opacity.ToString()!.Replace(',', '.')})", () => !Transparent)
        .AddStyle("cursor", "default", () => !Transparent)
        .AddStyle("position", FullScreen ? "fixed" : "absolute")
        .AddStyle("display", "flex")
        .AddStyle("align-items", Alignment.ToAttributeValue())
        .AddStyle("justify-content", Justification.ToAttributeValue())
        .AddStyle("pointer-events", "none", () => Interactive)
        .AddStyle("z-index", $"{ZIndex.Overlay}")
        .Build();

    /// <summary />
    protected string? StyleContentValue => new StyleBuilder()
        .AddStyle("pointer-events", "auto", () => Interactive)
        .Build();

    /// <summary>
    /// Gets or sets the unique identifier of the overlay.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = false;

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
    /// </summary>
    [Parameter]
    public double? Opacity { get; set; }

    /// <summary>
    /// Gets or sets the alignment of the content to a <see cref="AspNetCore.Components.Align"/> value.
    /// Defaults to Align.Center.
    /// </summary>
    [Parameter]
    public Align Alignment { get; set; } = Align.Center;

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
    public bool FullScreen { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the overlay is interactive, except for the element with the specified <see cref="InteractiveExceptId"/>.
    /// In other words, the elements below the overlay remain usable (mouse-over, click) and the overlay will closed when clicked.
    /// </summary>
    [Parameter]
    public bool Interactive { get; set; } = false;

    /// <summary>
    /// Gets or sets the HTML identifier of the element that is not interactive when the overlay is shown.
    /// This property is ignored if <see cref="Interactive"/> is false.
    /// </summary>
    [Parameter]
    public string? InteractiveExceptId { get; set; } = null;

    /// <summary>
    /// Gets of sets a value indicating if the overlay can be dismissed by clicking on it.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool Dismissable { get; set; } = true;

    /// <summary>
    /// Gets or sets the background color. 
    /// Needs to be formatted as an HTML hex color string (#rrggbb or #rgb).
    /// Default is '#ffffff'.
    /// </summary>
    [Parameter]
    public string BackgroundColor { get; set; } = "#ffffff";

    [Parameter]
    public bool PreventScroll { get; set; } = false;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (Interactive)
        {
            if (string.IsNullOrEmpty(Id))
            {
                Id = _defaultId;
            }

            // Add a document.addEventListener when Visible is true
            if (Visible)
            {
                await InvokeOverlayInitializeAsync();
            }

            // Remove a document.addEventListener when Visible is false
            else
            {
                await InvokeOverlayDisposeAsync();
            }
        }

        if (!Transparent && Opacity == 0)
        {
            Opacity = 0.4;
        }

        if (Opacity > 0)
        {
            Transparent = false;
        }

        if (!string.IsNullOrWhiteSpace(BackgroundColor))
        {

#if NET7_0_OR_GREATER
            if (!CheckRGBString().IsMatch(BackgroundColor))
#else
            if (!Regex.IsMatch(BackgroundColor, "^(?:#([a-fA-F0-9]{6}|[a-fA-F0-9]{3}))"))
#endif
                throw new ArgumentException("BackgroundColor must be a valid HTML hex color string (#rrggbb or #rgb).");
            else
            {
                _color = BackgroundColor[1..];
            }

            if (_color.Length == 6)
            {
                _r = int.Parse(_color[..2], NumberStyles.HexNumber);
                _g = int.Parse(_color[2..4], NumberStyles.HexNumber);
                _b = int.Parse(_color[4..], NumberStyles.HexNumber);
            }
            else
            {
                _r = int.Parse(_color[0..1], NumberStyles.HexNumber);
                _g = int.Parse(_color[1..2], NumberStyles.HexNumber);
                _b = int.Parse(_color[2..], NumberStyles.HexNumber);
            }
        }
    }

    [JSInvokable]
    public async Task OnCloseInteractiveAsync(MouseEventArgs e)
    {
        if (!Dismissable || !Visible)
        {
            return;
        }

        // Remove the document.removeEventListener
        await InvokeOverlayDisposeAsync();

        // Close the overlay
        await OnCloseInternalHandlerAsync(e);
    }

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

    /// <summary>
    /// Disposes the overlay.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        await InvokeOverlayDisposeAsync();

        if (_jsModule != null)
        {
            await _jsModule.DisposeAsync();
        }
    }

    /// <summary />
    private async Task InvokeOverlayInitializeAsync()
    {
        _dotNetHelper ??= DotNetObjectReference.Create(this);
        _jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));

        var containerId = FullScreen ? null : Id;
        await _jsModule.InvokeVoidAsync("overlayInitialize", _dotNetHelper, containerId, InteractiveExceptId);
    }

    /// <summary />
    private async Task InvokeOverlayDisposeAsync()
    {
        if (_jsModule != null && Interactive)
        {
            await _jsModule.InvokeVoidAsync("overlayDispose", InteractiveExceptId);
        }
    }

#if NET7_0_OR_GREATER
    [GeneratedRegex("^(?:#(?:[a-fA-F0-9]{6}|[a-fA-F0-9]{3}))")]
    private static partial Regex CheckRGBString();
#endif

}

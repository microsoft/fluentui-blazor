using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentOverlay : IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.Fast.Components.FluentUI/Components/Overlay/FluentOverlay.razor.js";
    
    private string? _color = null;
    private int _r, _g, _b;

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle("cursor", "auto", () => Transparent)
        .AddStyle("background-color", $"rgba({_r}, {_g}, {_b}, {Opacity.ToString()!.Replace(',', '.')})", () => !Transparent)
        .AddStyle("cursor", "default", () => !Transparent)
        .AddStyle("position", "fixed", () => FullScreen)
        .AddStyle("position", "absolute", () => !FullScreen)
        .AddStyle("z-index", $"{ZIndex.Overlay}")
        .Build();

    /// <summary />
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary />
    private IJSObjectReference Module { get; set; } = default!;

    /// <summary>
    /// Gets or sets if the overlay is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = false;

    /// <summary>
    /// Callback for when overlay visisbility changes
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
    /// Gets or sets the alignment of the content to a <see cref="FluentUI.Align"/> value.
    /// Defaults to Align.Center.
    /// </summary>
    [Parameter]
    public Align Alignment { get; set; } = Align.Center;

    /// <summary>
    /// Gets or sets the justification of the content to a <see cref="FluentUI.JustifyContent"/> value.
    /// Defaults to JustifyContent.Center.
    /// </summary>
    [Parameter]
    public JustifyContent Justification { get; set; } = JustifyContent.Center;

    /// <summary>
    /// Gets or sets if the overlay is shown full screen or bound to the containing element.
    /// </summary>
    [Parameter]
    public bool FullScreen { get; set; } = false;

    [Parameter]
    public bool Dismissable { get; set; } = true;

    /// <summary>
    /// Gets or sets background color. 
    /// Needs to be formatted as an HTML hex color string (#rrggbb or #rgb)
    /// Default is '#ffffff'
    /// </summary>
    [Parameter]
    public string BackgroundColor { get; set; } = "#ffffff";

    [Parameter]
    public bool PreventScroll { get; set; } = false;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Module = await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
            if (Visible)
            {
                await Module.InvokeVoidAsync("disableBodyScroll");
            }
        }
    }

    protected override async Task OnParametersSetAsync()
    {
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
                _color = BackgroundColor[1..];

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

        if (Visible && Module is not null)
        {
            await Module.InvokeVoidAsync("disableBodyScroll");
        }
    }

    protected async Task OnCloseHandlerAsync(MouseEventArgs e)
    {
        if (!Dismissable)
        {
            return;
        }

        Visible = false;

        if (VisibleChanged.HasDelegate)
        {
            await VisibleChanged.InvokeAsync(Visible);
        }

        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync(e);
        }

        await Module.InvokeVoidAsync("enableBodyScroll");

        return;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (Module is not null)
            {
                await Module!.InvokeVoidAsync("enableBodyScroll");
                await Module.DisposeAsync();
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

#if NET7_0_OR_GREATER
    [GeneratedRegex("^(?:#(?:[a-fA-F0-9]{6}|[a-fA-F0-9]{3}))")]
    private static partial Regex CheckRGBString();
#endif

}

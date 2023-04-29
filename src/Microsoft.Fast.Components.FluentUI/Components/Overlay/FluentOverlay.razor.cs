using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentOverlay
{
    private string? _color = null;

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
    /// Value comes from the <see cref="FluentUI.Color"/> enumeration. Defaults to Fill.
    /// </summary>
    [Parameter]
    public Color BackgroundColor { get; set; } = Color.Fill;

    /// <summary>
    /// Gets or sets the background color to a custom value.
    /// Needs to be formatted as an HTML hex color string (#rrggbb or #rgb) or CSS variable.
    /// ⚠️ Only available when Color is set to Color.Custom.
    /// </summary>
    [Parameter]
    public string? CustomBackgroundColor { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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

        return;
    }

    protected override void OnParametersSet()
    {
        if (!Transparent && Opacity == 0)
        {
            Opacity = 0.4;
        }

        if (Opacity > 0)
        {
            Transparent = false;
        }

        if (BackgroundColor != Color.Custom)
        {
            if (CustomBackgroundColor != null)
                throw new ArgumentException("CustomBackgroundColor can only be used when BackgroundColor is set to Color.Custom. ");
            else
                _color = BackgroundColor.ToAttributeValue();
        }
        else
        {
            if (CustomBackgroundColor is null)
                throw new ArgumentException("CustomBackgroundColor must be set when BackgroundColor is set to Color.Custom. ");
            else
            {
#if NET7_0_OR_GREATER
                if (!CheckRGBString().IsMatch(CustomBackgroundColor))
#else
                if (!Regex.IsMatch(CustomBackgroundColor, "^(?:#([a-fA-F0-9]{6}|[a-fA-F0-9]{3}))|var\\(--.*\\)$"))
#endif
                    throw new ArgumentException("CustomBackgroundColor must be a valid HTML hex color string (#rrggbb or #rgb) or CSS variable. ");
                else
                    _color = CustomBackgroundColor;
            }
        }
    }

#if NET7_0_OR_GREATER
    [GeneratedRegex("^(?:#(?:[a-fA-F0-9]{6}|[a-fA-F0-9]{3}))|var\\(--.*\\)$")]
    private static partial Regex CheckRGBString();
#endif

}

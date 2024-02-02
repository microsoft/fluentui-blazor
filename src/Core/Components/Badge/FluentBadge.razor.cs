using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentBadge : FluentComponentBase
{
    public FluentBadge()
    {
        Id = Identifier.NewId();
    }

    protected string? ClassValue => new CssBuilder(Class)
         .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("cursor", "pointer", () => OnClick.HasDelegate)
        .AddStyle($"--badge-fill-{Fill}", BackgroundColor, () => !string.IsNullOrEmpty(BackgroundColor))
        .AddStyle($"--badge-color-{Fill}", Color, () => !string.IsNullOrEmpty(Color))
        .Build();

    private string? InternalStyleValue => new StyleBuilder()
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .AddStyle("width: 100%; display: flex; align-items: center; justify-content: center; white-space: nowrap;")
        .Build();

    /// <summary>
    /// Gets or sets the color.
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the background color.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the background color based on fill value.
    /// </summary>
    [Parameter]
    public string? Fill { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the badge is rendered circular.
    /// </summary>
    [Parameter]
    public bool Circular { get; set; } = false;

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="AspNetCore.Components.Appearance"/>
    /// Possible values are Accent, Neutral (default) or Lightweight
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; } = AspNetCore.Components.Appearance.Neutral;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the component.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the tooltip to display when hovering over the <see cref="DismissIcon"/> icon.
    /// </summary>
    [Parameter]
    public string? DismissTitle { get; set; }

    /// <summary>
    /// Gets or sets the icon to be displayed when the badge is cancellable.
    /// By default, a small cross icon is displayed.
    /// </summary>
    [Parameter]
    public Icon? DismissIcon { get; set; }

    /// <summary>
    /// Event callback for when the badge is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Event callback for when the badge <see cref="DismissIcon"/> icon is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnDismissClick { get; set; }

    protected override void OnParametersSet()
    {
        if (Appearance != AspNetCore.Components.Appearance.Accent &&
            Appearance != AspNetCore.Components.Appearance.Lightweight &&
            Appearance != AspNetCore.Components.Appearance.Neutral)
        {
            throw new ArgumentException("FluentBadge Appearance needs to be one of Accent, Lightweight or Neutral.");
        }
    }

    protected virtual async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }

    protected virtual async Task OnDismissClickHandlerAsync(MouseEventArgs e)
    {
        if (OnDismissClick.HasDelegate)
        {
            await OnDismissClick.InvokeAsync(e);
        }
    }

}

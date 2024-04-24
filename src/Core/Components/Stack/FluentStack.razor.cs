using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentStack : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("stack-horizontal", () => Orientation == Orientation.Horizontal)
        .AddClass("stack-vertical", () => Orientation == Orientation.Vertical)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("align-items", GetHorizontalAlignment(), () => Orientation == Orientation.Vertical)
        .AddStyle("justify-content", GetVerticalAlignment(), () => Orientation == Orientation.Vertical)

        .AddStyle("justify-content", GetHorizontalAlignment(), () => Orientation == Orientation.Horizontal)
        .AddStyle("align-items", GetVerticalAlignment(), () => Orientation == Orientation.Horizontal)

        .AddStyle("column-gap", $"{HorizontalGap}px", () => HorizontalGap.HasValue)
        .AddStyle("row-gap", $"{VerticalGap}px", () => VerticalGap.HasValue)
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("flex-wrap", "wrap", () => Wrap)

        .Build();

    /// <summary>
    /// Gets or sets the horizontal alignment of the components in the stack. 
    /// </summary>
    [Parameter]
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;

    /// <summary>
    /// Gets or sets the vertical alignment of the components in the stack.
    /// </summary>
    [Parameter]
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;

    /// <summary>
    /// Gets or sets the orientation of the stacked components. 
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the width of the stack as a percentage string (default = 100%).
    /// </summary>
    [Parameter]
    public string? Width { get; set; } = "100%";

    /// <summary>
    /// Gets or sets a value indicating whether the stack wraps.
    /// </summary>
    [Parameter]
    public bool Wrap { get; set; } = false;

    /// <summary>
    /// Gets or sets the gap between horizontally stacked components (in pixels).
    /// Default is 10 pixels.
    /// </summary>
    [Parameter]
    public int? HorizontalGap { get; set; } = 10;

    /// <summary>
    /// Gets or sets the gap between vertically stacked components (in pixels).
    /// Default is 10 pixels.
    /// </summary>
    [Parameter]
    public int? VerticalGap { get; set; } = 10;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string GetHorizontalAlignment()
    {
        return HorizontalAlignment switch
        {
            HorizontalAlignment.Left => "start",
            HorizontalAlignment.Start => "start",
            HorizontalAlignment.Center => "center",
            HorizontalAlignment.Right => "end",
            HorizontalAlignment.End => "end",
            _ => "start",
        };
    }

    private string GetVerticalAlignment()
    {
        return VerticalAlignment switch
        {
            VerticalAlignment.Top => "start",
            VerticalAlignment.Center => "center",
            VerticalAlignment.Bottom => "end",
            _ => "start",
        };
    }
}

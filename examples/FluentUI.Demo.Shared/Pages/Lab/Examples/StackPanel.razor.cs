using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;

/// <summary>
/// Determines the horizontal alignment of the content within the <see cref="FluentStackPanel"/>.
/// </summary>
public enum StackPanelHorizontalAlignment
{
    /// <summary>
    /// The content is aligned to the left.
    /// </summary>
    Left,

    /// <summary>
    /// The content is center aligned.
    /// </summary>
    Center,

    /// <summary>
    /// The content is aligned to the right.
    /// </summary>
    Right,
}

/// <summary>
/// Determines the vertical alignment of the content within the <see cref="FluentStackPanel"/>.
/// </summary>
public enum StackPanelVerticalAlignment
{
    /// <summary>
    /// The content is aligned to the top.
    /// </summary>
    Top,

    /// <summary>
    /// The content is center aligned.
    /// </summary>
    Center,

    /// <summary>
    /// The content is aligned to the bottom
    /// </summary>
    Bottom,
}

public partial class StackPanel : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("stack-horizontal", () => Orientation == Orientation.Horizontal)
        .AddClass("stack-vertical", () => Orientation == Orientation.Vertical)
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("align-items", GetHorizontalAlignment(), () => Orientation == Orientation.Vertical)
        .AddStyle("justify-content", GetVerticalAlignment(), () => Orientation == Orientation.Vertical)

        .AddStyle("justify-content", GetHorizontalAlignment(), () => Orientation == Orientation.Horizontal)
        .AddStyle("align-items", GetVerticalAlignment(), () => Orientation == Orientation.Horizontal)

        .AddStyle("gap", $"{Gap}px", () => Gap.HasValue)
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("flex-wrap", "wrap", () => Wrap)

        .AddStyle(Style)
        .Build();

    /// <summary>
    /// The horizontal alignment of the stack panel
    /// </summary>
    [Parameter]
    public StackPanelHorizontalAlignment HorizontalAlignment { get; set; } = StackPanelHorizontalAlignment.Left;

    /// <summary>
    /// The vertical alignment of the stack panel
    /// </summary>
    [Parameter]
    public StackPanelVerticalAlignment VerticalAlignment { get; set; } = StackPanelVerticalAlignment.Top;

    /// <summary>
    /// The orientation of the stack panel
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// The width of the stack panel as a string (default = 100%)
    /// </summary>
    [Parameter]
    public string? Width { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the stack panel to wrap
    /// </summary>
    [Parameter]
    public bool Wrap { get; set; } = false;

    /// <summary>
    /// Gets or sets the gap between components in the stack panel
    /// </summary>
    [Parameter]
    public int? Gap { get; set; } = 10;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string GetHorizontalAlignment()
    {
        return HorizontalAlignment switch
        {
            StackPanelHorizontalAlignment.Left => "start",
            StackPanelHorizontalAlignment.Center => "center",
            StackPanelHorizontalAlignment.Right => "end",
            _ => "start",
        };
    }

    private string GetVerticalAlignment()
    {
        return VerticalAlignment switch
        {
            StackPanelVerticalAlignment.Top => "start",
            StackPanelVerticalAlignment.Center => "center",
            StackPanelVerticalAlignment.Bottom => "end",
            _ => "start",
        };
    }
}

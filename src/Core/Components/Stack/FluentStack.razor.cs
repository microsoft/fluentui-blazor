// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A container-type component that can be used to arrange its child components in a horizontal or vertical stack.
/// </summary>
public partial class FluentStack : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-stack-horizontal", () => Orientation == Orientation.Horizontal)
        .AddClass("fluent-stack-vertical", () => Orientation == Orientation.Vertical)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("justify-content", GetVerticalAlignment(), () => Orientation == Orientation.Vertical)
        .AddStyle("align-items", GetHorizontalAlignment(), () => Orientation == Orientation.Vertical)

        .AddStyle("justify-content", GetHorizontalAlignment(), () => Orientation == Orientation.Horizontal)
        .AddStyle("align-items", GetVerticalAlignment(), () => Orientation == Orientation.Horizontal)

        .AddStyle("column-gap", HorizontalGap.AddMissingPx(), () => !string.IsNullOrEmpty(HorizontalGap))
        .AddStyle("row-gap", VerticalGap.AddMissingPx(), () => !string.IsNullOrEmpty(VerticalGap))
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .AddStyle("flex-wrap", "wrap", () => Wrap)

        .Build();

    /// <summary>
    /// Gets or sets the horizontal alignment of the components in the stack.
    /// Default is <see cref="HorizontalAlignment.Left"/>
    /// </summary>
    [Parameter]
    public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;

    /// <summary>
    /// Gets or sets the vertical alignment of the components in the stack.
    /// Default is <see cref="VerticalAlignment.Top"/>
    /// </summary>
    [Parameter]
    public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;

    /// <summary>
    /// Gets or sets the orientation of the stacked components.
    /// Default is <see cref="Orientation.Horizontal"/>.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary>
    /// Gets or sets the width of the stack as a percentage string (default = 100%).
    /// </summary>
    [Parameter]
    public string? Width { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the height of the stack.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the stack wraps.
    /// </summary>
    [Parameter]
    public bool Wrap { get; set; } = false;

    /// <summary>
    /// Gets or sets the gap between horizontally stacked components.
    /// Default is `var(--spacingHorizontalM)` (12px).
    /// See the CSS <see href="https://developer.mozilla.org/docs/Web/CSS/row-gap">row-gap</see> property.
    /// </summary>
    [Parameter]
    public string? HorizontalGap { get; set; } = "var(--spacingHorizontalM)";

    /// <summary>
    /// Gets or sets the gap between vertically stacked components.
    /// Default is `var(--spacingVerticalM)` (12px).
    /// See the CSS <see href="https://developer.mozilla.org/docs/Web/CSS/column-gap">column-gap</see> property.
    /// </summary>
    [Parameter]
    public string? VerticalGap { get; set; } = "var(--spacingVerticalM)";

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
            HorizontalAlignment.Stretch => "stretch",
            HorizontalAlignment.SpaceBetween => Orientation == Orientation.Vertical ? "start" : "space-between",
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
            VerticalAlignment.Stretch => "stretch",
            VerticalAlignment.SpaceBetween => Orientation == Orientation.Horizontal ? "start" : "space-between",
            _ => "start",
        };
    }
}

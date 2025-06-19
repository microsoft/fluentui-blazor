// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a skeleton loading component that provides visual placeholders for content while it is being loaded.
/// </summary>
/// <remarks>The <see cref="FluentSkeleton"/> class is designed to display skeleton shapes, such as rectangles or
/// circles,  to indicate loading states in a user interface. It supports customization of size, shape, and shimmer
/// effects. Use this component to improve perceived performance by showing placeholders for content that is being
/// fetched or processed.
/// </remarks>
public partial class FluentSkeleton : FluentComponentBase
{
    /// <summary />
    public FluentSkeleton(LibraryConfiguration configuration) : base(configuration)
    {

    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-skeleton")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("background-color", "transparent", when: ChildContent is not null || Pattern is not null)
        .AddStyle("width", Width, when: Circular == false)
        .AddStyle("height", Height, when: Circular == false)
        .AddStyle("width", GetCircularSize(), when: Circular == true)
        .AddStyle("height", GetCircularSize(), when: Circular == true)
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the skeleton component.
    /// </summary>
    [Parameter]
    public RenderFragment<FluentSkeleton>? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the loading effect is visible.
    /// </summary>
    [Parameter]
    public bool Shimmer { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the skeleton in a circular mode.
    /// </summary>
    [Parameter]
    public bool Circular { get; set; }

    /// <summary>
    /// Gets or sets the width of the element. Default is "100%".
    /// </summary>
    [Parameter]
    public string? Width { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the height of the component. Default is "48px".
    /// </summary>
    [Parameter]
    public string? Height { get; set; } = "48px";

    /// <summary>
    /// Gets or sets the predefined skeleton pattern used to define the structure or layout of the component.
    /// You can customize the skeleton's appearance by using <see cref="ChildContent" />
    /// </summary>
    [Parameter]
    public SkeletonPattern? Pattern { get; set; }

    /// <summary>
    /// Generates a circular element with the specified radius.
    /// </summary>
    /// <returns>A <see cref="MarkupString"/> containing the HTML markup for a styled circular element.</returns>
    public MarkupString DrawCircle(string radius)
    {
        var style = new StyleBuilder()
            .AddStyle("background-color", "var(--fluentSkeletonBackground)")
            .AddStyle("border-radius", "50%")
            .AddStyle("min-width", radius)
            .AddStyle("min-height", radius)
            .AddStyle("max-width", radius)
            .AddStyle("max-height", radius)
            .AddStyle("margin", "var(--spacingVerticalXS) var(--spacingHorizontalXS)")
            .Build();

        return (MarkupString)$"<div style=\"{style}\" />";
    }

    /// <summary>
    /// Generates a rectangle element with the specified width and height.
    /// </summary>
    /// <param name="width">The width of the rectangle, specified as a CSS-compatible value (e.g., "100px", "50%").</param>
    /// <param name="height">The height of the rectangle, specified as a CSS-compatible value (e.g., "100px", "50%").</param>
    /// <returns>A <see cref="MarkupString"/> containing the HTML representation of a rectangle styled with the specified dimensions.</returns>
    public MarkupString DrawRectangle(string width, string height)
    {
        var style = new StyleBuilder()
           .AddStyle("background-color", "var(--fluentSkeletonBackground)")
           .AddStyle("border-radius", "var(--borderRadiusMedium);")
           .AddStyle("width", width)
           .AddStyle("height", height)
           .AddStyle("margin", "var(--spacingVerticalXS) var(--spacingHorizontalXS)")
           .Build();

        return (MarkupString)$"<div style=\"{style}\" />";
    }

    /// <summary />
    private string GetCircularSize()
    {
        if (!Circular)
        {
            return string.Empty;
        }

        if (string.IsNullOrEmpty(Width) && !string.IsNullOrEmpty(Height))
        {
            return Height;
        }

        if (!string.IsNullOrEmpty(Width) && string.IsNullOrEmpty(Height))
        {
            return Width;
        }

        if (!string.IsNullOrEmpty(Width) && !string.IsNullOrEmpty(Height))
        {
            return $"min({Width}, {Height})";
        }

        return "48px"; // Default size for circular skeleton
    }
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Visual representation of content being loaded or processed.
/// </summary>
public partial class FluentRatingDisplay : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the color of the rating display items.
    /// </summary>
    [Parameter]
    public RatingDisplayColor? Color { get; set; }

    /// <summary>
    /// Gets or sets the compact mode.
    /// Renders a single filled star, with the value written next to it.
    /// </summary>
    [Parameter]
    public bool? Compact { get; set; }

    /// <summary>
    /// Gets or sets the number of ratings represented by the rating value.
    /// This will be formatted with a thousands separator (if applicable) and displayed next to the value.
    /// </summary>
    [Parameter]
    public double? Count { get; set; }

    /// <summary>
    /// Gets or sets the max value of the rating.
    /// This controls the number of rating items displayed. Must be a whole number greater than 1
    /// </summary>
    [Parameter]
    public int? Max { get; set; }

    /// <summary>
    /// Gets or sets the size of the rating items
    /// </summary>
    [Parameter]
    public RatingSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the shape
    /// </summary>
    [Parameter]
    public Icon? Shape { get; set; }

    /// <summary>
    /// Gets or sets the value of the rating
    /// </summary>
    [Parameter]
    public double? Value { get; set; }
}

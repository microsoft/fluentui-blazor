// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Displays a read-only star rating, such as an average product score or user review summary.
/// </summary>
public partial class FluentRatingDisplay : FluentComponentBase, ITooltipComponent
{
    /// <summary />
    public FluentRatingDisplay(LibraryConfiguration configuration) : base(configuration) { }

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
    /// Gets or sets a value indicating whether compact mode is enabled.
    /// When <see langword="true"/>, renders a single filled star with the numeric value next to it.
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
    /// Gets or sets the maximum number of rating items displayed (e.g., <c>Max="5"</c>).
    /// Must be a whole number greater than 1. See also <see cref="Value"/>.
    /// </summary>
    [Parameter]
    public byte? Max { get; set; }

    /// <summary>
    /// Gets or sets the size of the rating items (e.g., <c>Size="RatingSize.Small"</c>).
    /// </summary>
    [Parameter]
    public RatingSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the icon used for each rating item (e.g., <c>Shape="new MyIcons.Star()"</c>).
    /// Defaults to a star icon.
    /// </summary>
    [Parameter]
    public Icon? Shape { get; set; }

    /// <summary>
    /// Gets or sets the current rating value (e.g., <c>Value="3.5"</c>).
    /// Must be between 0 and <see cref="Max"/>.
    /// </summary>
    [Parameter]
    public double? Value { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }
}

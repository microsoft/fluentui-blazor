// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentBadge component is a visual indicator that communicates a status or description of an associated component.
/// It uses short text, color, and icons for quick recognition and is placed near the relavant content.
/// </summary>
public partial class FluentCounterBadge : FluentBadge, IFluentComponentBase
{
    private int? GetCount() => ShowWhen?.Invoke(Count) == true ? Count : null;

    /// <summary>
    ///  Gets or sets the badge's dot state.
    /// </summary>
    [Parameter]
    public bool Dot { get; set; }

    /// <summary>
    /// Gets or sets the badge's show-zero state.
    /// </summary>
    [Parameter]
    public bool ShowZero { get; set; }

    /// <summary>
    /// Gets or sets if the badge displays the count based on the specified lambda expression.
    /// By default the badge only shows a count when it's not equal to 0.
    /// For example, to show the count on the badge when the count greater than 4, use ShowWhen=@(Count => Count > 4)
    /// </summary>
    [Parameter]
    public Func<int?, bool>? ShowWhen { get; set; } = Count => Count != 0;

    /// <summary>
    /// Gets or sets the badge's count.
    /// The default value is `null`. Internally the component uses 0 as its default value.
    /// With ShowZero being false by default, the default result will be an empty counter badge
    /// </summary>
    [Parameter]
    public int? Count { get; set; }

    /// <summary>
    /// Gets or sets the badge's overflow count.
    /// The default value is `null`. Internally the component uses 99 as its default value.
    /// </summary>
    [Parameter]
    public int? OverflowCount { get; set; }

    /// <summary />
    protected override void OnParametersSet()
    {
        if (!string.IsNullOrWhiteSpace(BackgroundColor) && Color is not null)
        {
            throw new ArgumentException("When setting BackgroundColor, Color must not be set.");
        }

        if (Appearance == BadgeAppearance.Outline || Appearance == BadgeAppearance.Tint)
        {
            throw new ArgumentException("FluentCounterBadge does not support Outline or Tint appearance.");
        }

        if (Shape == BadgeShape.Square)
        {
            throw new ArgumentException("FluentCounterBadge does not support Square shape.");
        }
    }
}

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
    private int _internalCount;

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
    /// By default the badge shows the count when it is not equal to 0.
    /// For example, to show the count on the badge when the count greater than 4, use ShowWhen=@(Count => Count > 4)
    /// </summary>
    [Parameter]
    public Func<int, bool>? ShowWhen { get; set; }

    /// <summary>
    /// Gets or sets the badge's count.
    /// </summary>
    [Parameter]
    public int Count { get; set; }

    /// <summary>
    /// Gets or sets the badge's overflow count.
    /// Default is 99
    /// </summary>
    [Parameter]
    public int OverflowCount { get; set; } = 99;

    /// <summary />
    protected override void OnParametersSet()
    {
        if (!string.IsNullOrWhiteSpace(BackgroundColor) && Color is not null)
        {
            throw new ArgumentException("When setting BackgroundColor, Color must not be set.");
        }

        if (Appearance == BadgeAppearance.Outline || Appearance == BadgeAppearance.Tint)
        {
            throw new ArgumentException("CounterBadge does not support Outline or Tint appearance.");
        }

        if (Shape == BadgeShape.Square)
        {
            throw new ArgumentException("CounterBadge does not support Square shape.");
        }

        if (ShowWhen is not null && !ShowWhen.Invoke(Count))
        {
            _internalCount = 0;
        }
        else
        {
            _internalCount = Count;
        }
    }
}

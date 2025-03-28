// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A spinner alerts a user that content is being loaded or processed and they should wait for the activity to complete.
/// </summary>
public partial class FluentSpinner : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the visibility of the component
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets whether the spinner should be shown in an inverted color scheme (i.e. light spinner on dark background)
    /// </summary>
    [Parameter]
    public bool AppearanceInverted { get; set; }

    /// <summary>
    /// Gets or sets the size of the spinner. Default is <see cref="SpinnerSize.Medium"/>.
    /// </summary>
    [Parameter]
    public SpinnerSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the stroke width of the progress bar.
    /// If not set, the default theme stroke width is used.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release. Use Size property instead.")]
    public ProgressStroke? Stroke { get; set; }

#pragma warning disable CS0618

    private SpinnerSize? GetSize()
    {
        if (Size == null && Stroke != null)
        {
            return Stroke switch
            {
                ProgressStroke.Small => SpinnerSize.Small,
                ProgressStroke.Normal => SpinnerSize.Medium,
                ProgressStroke.Large => SpinnerSize.Large,
                _ => null
            };
        }

        return Size;
    }

#pragma warning restore CS0618
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A spinner alerts a user that content is being loaded or processed and they should wait for the activity to complete.
/// </summary>
public partial class FluentSpinner : FluentComponentBase, ITooltipComponent
{
    /// <summary />
    public FluentSpinner(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("visibility", "hidden", () => Visible == false)
        .Build();

    /// <summary>
    /// Gets or sets the visibility of the component.
    /// If `true` (default), the component is visible.
    /// If `false`, the component is hidden.
    /// If `null`, the component is hidden and not rendered.
    /// </summary>
    [Parameter]
    public bool? Visible { get; set; } = true;

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

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

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

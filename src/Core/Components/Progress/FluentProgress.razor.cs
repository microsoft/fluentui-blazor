using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentProgress : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .Build();

    public FluentProgress()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the minimum value.
    /// </summary>
    [Parameter]
    public int? Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum value.
    /// </summary>
    [Parameter]
    public int? Max { get; set; }

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    [Parameter]
    public int? Value { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the component
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the component width.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the progress element is paused.
    /// </summary>
    [Parameter]
    public bool? Paused { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the stroke width of the progress bar. If not set, the default theme stroke width is used.
    /// </summary>
    [Parameter]
    public ProgressStroke Stroke { get; set; } = ProgressStroke.Normal;

    /// <summary>
    /// Gets or sets the color to be used for the progress bar. If not set, the default theme color is used.
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the color to be used for the background. If not set, the default theme color is used.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    private (int BarHeight, int BackgroundHeight, string DefaultBackgroundColor) StrokeDetails => Stroke switch
    {
        ProgressStroke.Small => (2, 1, "--neutral-stroke-strong-rest"),
        ProgressStroke.Normal => (3, 1, "--neutral-stroke-strong-rest"),
        ProgressStroke.Large => (9, 6, "--neutral-stroke-rest"),
        _ => throw new NotImplementedException(),
    };
    private string StyleProgress => $"height: calc((var(--stroke-width) * {StrokeDetails.BackgroundHeight}) * 1px); " +
                                    $"background-color: {(string.IsNullOrEmpty(BackgroundColor) ? $"var({StrokeDetails.DefaultBackgroundColor})" : BackgroundColor)};";

    private string StyleProgressIndicator => $"height: calc((var(--stroke-width) * {StrokeDetails.BarHeight}) * 1px); " +
                                             $"background-color: {(string.IsNullOrEmpty(Color) ? "var(--accent-fill-rest)" : Color)};";

    private string StyleIndeterminate => $"--stroke-width: {((double)StrokeDetails.BarHeight / 3d).ToString("0.00")}; " +
                                         $"height: calc((1 * {StrokeDetails.BackgroundHeight}) * 1px); " +
                                         $"background-color: {(string.IsNullOrEmpty(BackgroundColor) ? $"var({StrokeDetails.DefaultBackgroundColor})" : BackgroundColor)};";

    private string StyleIndeterminateIndicator => $"background-color: {(string.IsNullOrEmpty(Color) ? "var(--accent-fill-rest)" : Color)};";
}

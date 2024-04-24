using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentProgressRing : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("height", Width, () => !string.IsNullOrEmpty(Width))
        .Build();

    public FluentProgressRing()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the minimum value 
    /// </summary>
    [Parameter]
    public int? Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum value 
    /// </summary>
    [Parameter]
    public int? Max { get; set; }

    /// <summary>
    /// Gets or sets the current value .
    /// </summary>
    [Parameter]
    public int? Value { get; set; }

    [Parameter]
    public bool Visible { get; set; } = true;

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
    /// Gets or sets the component width and height.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the stroke width of the progress ring. If not set, the default theme stroke width is used.
    /// </summary>
    [Parameter]
    public ProgressStroke Stroke { get; set; } = ProgressStroke.Normal;

    /// <summary>
    /// Gets or sets the color to be used for the progress ring. If not set, the default theme color is used.
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    private (int Width, int Radius, int Dashoffset) StrokeDetails => Stroke switch
    {
        ProgressStroke.Small => (1, 7, 0),
        ProgressStroke.Normal => (2, 7, 0),
        ProgressStroke.Large => (4, 6, (int)((0.066 * (Value ?? 0)) + 0.22)),
        _ => throw new NotImplementedException(),
    };

    private string StyleBackground => $"stroke-width: {StrokeDetails.Width}px; " +
                                      $"r: {StrokeDetails.Radius}px;";

    private string StyleIndicator => $"stroke-width: {StrokeDetails.Width}px; " +
                                     $"r: {StrokeDetails.Radius}px; " +
                                     $"stroke-dashoffset: {StrokeDetails.Dashoffset}px; " +
                                     $"stroke: {(string.IsNullOrEmpty(Color) ? "var(--accent-fill-rest)" : Color)};";
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Visual representation of content being loaded or processed.
/// </summary>
public partial class FluentProgressBar : FluentComponentBase, ITooltipComponent
{
    private readonly string _defaultId = Identifier.NewId();

    /// <summary />
    public FluentProgressBar(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("visibility", "hidden", () => Visible == false)
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("background-color", BackgroundColor, () => !string.IsNullOrEmpty(BackgroundColor))
        .Build();

    /// <summary>
    /// Gets or sets the minimum value (e.g., <c>Min="0"</c>).
    /// See also <see cref="Max"/>.
    /// </summary>
    [Parameter]
    public int? Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum value (e.g., <c>Max="100"</c>).
    /// The progress bar will be full when <see cref="Value"/> equals this.
    /// See also <see cref="Min"/>.
    /// </summary>
    [Parameter]
    public int? Max { get; set; }

    /// <summary>
    /// Gets or sets the shape of the progress bar: rounded or square.
    /// </summary>
    [Parameter]
    public ProgressShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the current progress value (e.g., <c>Value="75"</c>).
    /// If <see langword="null"/> (default), the bar displays an indeterminate state.
    /// Must be between <see cref="Min"/> and <see cref="Max"/>.
    /// </summary>
    [Parameter]
    public int? Value { get; set; }

    /// <summary>
    /// Gets or sets the visibility of the component.
    /// If `true` (default), the component is visible.
    /// If `false`, the component is hidden.
    /// If `null`, the component is hidden and not rendered.
    /// </summary>
    [Parameter]
    public bool? Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the component width.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the validation state of the progress bar: Success, Warning or Error.
    /// </summary>
    [Parameter]
    public ProgressState? State { get; set; }

    /// <summary>
    /// Gets or sets the background color of the progress bar.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the color of the progress bar indicator (e.g., <c>Color="#0078d4"</c>).
    /// Ignored when <see cref="State"/> is set.
    /// </summary>
    [Parameter]
    public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the stroke width of the progress bar.
    /// If not set, the default theme stroke width is used.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported anymore and will be removed in a future release. Use Thickness property instead.")]
    public ProgressStroke? Stroke { get; set; }

    /// <summary>
    /// Gets or sets the visual thickness of the progress bar track (e.g., <c>Thickness="ProgressThickness.Large"</c>).
    /// If not set, the default theme thickness is used.
    /// </summary>
    [Parameter]
    public ProgressThickness? Thickness { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    /// <summary />
    private string? GetThicknessAttribute()
    {
        var value = Thickness ?? StrokeToThickness();
        return value?.ToAttributeValue();
    }

    /// <summary />
    private string? GetId() => HasCustomStyle ? (Id ?? _defaultId) : Id;

    /// <summary />
    private MarkupStringSanitized? CustomStyle() =>
        HasCustomStyle
        ? new MarkupStringSanitized($"<style>#{GetId()}::part(indicator) {{ background-color: {Color}; }}</style>", LibraryConfiguration)
        : null;

    /// <summary />
    private bool HasCustomStyle => !string.IsNullOrEmpty(Color) && State is null;

    /// <summary />
    private ProgressThickness? StrokeToThickness()
    {
#pragma warning disable CS0618
        return Stroke switch
        {
            ProgressStroke.Small => ProgressThickness.Medium,
            ProgressStroke.Normal => ProgressThickness.Medium,
            ProgressStroke.Large => ProgressThickness.Large,
            _ => null
        };
#pragma warning restore CS0618
    }
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("background-color", BackgroundColor, () => !string.IsNullOrEmpty(BackgroundColor))
        .Build();

    /// <summary>
    /// Gets or sets the minimum value.
    /// </summary>
    [Parameter]
    public int? Min { get; set; }

    /// <summary>
    /// Gets or sets the maximum value.
    /// The FluentProgressBar bar will be full when value equals <see cref="Max"/>.
    /// </summary>
    [Parameter]
    public int? Max { get; set; }

    /// <summary>
    /// Gets or sets the shape of the progress bar: rounded or square.
    /// </summary>
    [Parameter]
    public ProgressShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the current value.
    /// If `null` (default), the FluentProgressBar will display an indeterminate state.
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
    /// Gets or sets the color of the progress bar.
    /// This property is not used when the <see cref="State"/> property is set.
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
    /// Gets or sets the stroke width of the progress bar.
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
    private MarkupString? CustomStyle() =>
        HasCustomStyle
        ? new MarkupString($"<style>#{GetId()}::part(indicator) {{ background-color: {Color}; }}</style>")
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

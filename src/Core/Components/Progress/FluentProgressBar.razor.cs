// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
public partial class FluentProgressBar : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
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
    /// Gets or sets the stroke width of the progress bar.
    /// If not set, the default theme stroke width is used.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported in FluentUI components and will be removed in a future release. Use Thickness property.")]
    public ProgressStroke? Stroke { get; set; }

    /// <summary>
    /// Gets or sets the stroke width of the progress bar.
    /// </summary>
    [Parameter]
    public ProgressThickness? Thickness { get; set; }

    /// <summary />
    private string? GetThicknessAttribute()
    {
        var value = Thickness ?? StrokeToThickness();
        return value?.ToAttributeValue();
    }

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

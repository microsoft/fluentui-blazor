// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
public partial class FluentProgress : FluentComponentBase
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
    /// Gets or sets a value indicating whether the progress element is paused.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported in FluentUI components and will be removed in a future release.")]
    public bool? Paused { get; set; }

    /// <summary>
    /// Gets or sets the component width.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the stroke width of the progress bar.
    /// If not set, the default theme stroke width is used.
    /// </summary>
    [Parameter]
    [Obsolete("This property is not supported in FluentUI components and will be removed in a future release. Use Thickness property.")]
    public ProgressStroke Stroke { get; set; } = ProgressStroke.Normal;

    /// <summary>
    /// Gets or sets the stroke width of the progress bar.
    /// If not set, the default theme stroke width is used.
    /// </summary>
    [Parameter]
    public ProgressThickness? Thickness { get; set; }
}

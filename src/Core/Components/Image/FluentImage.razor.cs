// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The Fluent Image component is used to display an image, providing various options for styling and layout.
/// </summary>
public partial class FluentImage : FluentComponentBase
{
    /// <summary/>
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary/>
    protected string? ImageClassValue => new CssBuilder("fluent-image-item")
        .Build();

    /// <summary/>
    protected string? ImageStyleValue => new StyleBuilder()
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// Gets or sets the height of the image, applies only to the parameter.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the width of the image, applies only to the parameter.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the source link for the image, applies only to the parameter.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// Gets or sets the alternate text for the image.
    /// </summary>
    [Parameter]
    public string? AlternateText { get; set; }

    /// <summary>
    /// Gets or sets the image html component.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    /// Gets or sets the border surrounding image.
    /// </summary>
    [Parameter]
    public bool Bordered { get; set; }

    /// <summary>
    /// Gets or sets the argument ‘block’ so that the image's width will expand to fill the available container space.
    /// </summary>
    [Parameter]
    public bool Block { get; set; }

    /// <summary>
    /// Gets or sets a box shadow to further separate the image from the background.
    /// </summary>
    [Parameter]
    public bool Shadow { get; set; }

    /// <summary>
    /// Gets or sets the image shape <see cref="ImageShape"/>
    /// </summary>
    [Parameter]
    public ImageShape? Shape { get; set; }

    /// <summary>
    /// Gets or sets the fit of the image, determines how it will be scaled and positioned within its parent container.
    /// </summary>
    [Parameter]
    public ImageFit? Fit { get; set; }
}


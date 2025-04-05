// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

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
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// The height of the image.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// The width of the image.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// The source link for the image.
    /// </summary>
    [Parameter]
    public string? Source { get; set; }

    /// <summary>
    /// The alternate text for the image.
    /// </summary>
    [Parameter]
    public string? AlternateText { get; set; }

    /// <summary>
    /// The image html component
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    /// Border surrounding image
    /// </summary>
    [Parameter]
    public bool Bordered { get; set; }

    /// <summary>
    /// An image can use the argument ‘block’ so that it’s width will expand to fill the available container space.
    /// </summary>
    [Parameter]
    public bool Block { get; set; }

    /// <summary>
    /// Apply an optional box shadow to further separate the image from the background.
    /// </summary>
    [Parameter]
    public bool Shadow { get; set; }

    /// <summary>
    /// The image shape <see cref="ImageShape"/>
    /// </summary>
    [Parameter]
    public ImageShape? Shape { get; set; }

    /// <summary>
    /// Determines how the image will be scaled and positioned within its parent container.
    /// </summary>
    [Parameter]
    public ImageFit? Fit { get; set; }
}


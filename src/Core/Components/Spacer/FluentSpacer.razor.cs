// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Spacer component, used to create space between elements.
/// </summary>
public partial class FluentSpacer : FluentComponentBase
{
    /// <summary/>
    public string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("flex-grow", "1", when: () => (string.IsNullOrEmpty(Height) && Orientation == Orientation.Vertical) ||
                                                (string.IsNullOrEmpty(Width) && Orientation == Orientation.Horizontal))
        .AddStyle("width", Width, when: () => !string.IsNullOrEmpty(Width) && Orientation == Orientation.Horizontal)
        .AddStyle("height", Height, when: () => !string.IsNullOrEmpty(Height) && Orientation == Orientation.Vertical)
        .Build();

    /// <summary>
    /// Gets or sets the height of the spacer when the orientation is vertical.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the size of the spacer when the orientation is horizontal.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Specify the orientation of the parent container, which can be either horizontal or vertical.
    /// </summary>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;
}

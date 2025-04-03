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
    /// <summary>
    /// Gets or sets the size of the spacer, depending on the orientation.
    /// </summary>
    [Parameter]
    public string? Size { get; set; }

    /// <summary/>
    [Parameter]
    public Orientation Orientation { get; set; } = Orientation.Horizontal;

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("flex-grow", "1", when: () => string.IsNullOrEmpty(Size))
        .AddStyle("width", Size, when: () => !string.IsNullOrEmpty(Size) && Orientation == Orientation.Horizontal)
        .AddStyle("height", Size, when: () => !string.IsNullOrEmpty(Size) && Orientation == Orientation.Vertical)
        .Build();
}


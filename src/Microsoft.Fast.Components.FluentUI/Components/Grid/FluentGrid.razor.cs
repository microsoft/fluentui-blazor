// --------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// --------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The grid component helps keeping layout consistent across various screen resolutions and sizes.
/// PowerGrid comes with a 12-point grid system and contains 5 types of breakpoints
/// that are used for specific screen sizes.
/// </summary>
public partial class FluentGrid : FluentComponentBase
{
    /// <summary>
    /// Distance between flexbox items, using a multiple of 4px.
    /// Only values from 0 to 10 are possible.
    /// </summary>
    [Parameter]
    public int Spacing { get; set; } = 3;

    /// <summary>
    /// Defines how the browser distributes space between and around content items.
    /// </summary>
    [Parameter]
    public JustifyContent Justify { get; set; } = JustifyContent.FlexStart;

    /// <summary>
    /// Child content of component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass($"fluent-grid")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .AddStyle("justify-content", Justify.ToAttributeValue())
        .Build();
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentContent
{
    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Class"/>
    /// </summary>
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-content")
        .Build();

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Style"/>
    /// </summary>
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("grid-row", Area, () => !string.IsNullOrEmpty(Area))
        .AddStyle("grid-column-start", Area, () => !string.IsNullOrEmpty(Area))
        .AddStyle("grid-column-end", Area, () => !string.IsNullOrEmpty(Area))   // Set "aside" and aside.margin-right=24px to display the scrollbar at right
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle("position", "sticky", () => Sticky)
        .Build();

    /// <summary />
    [CascadingParameter]
    protected FluentLayout? Layout { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public string? Area { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool Sticky { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

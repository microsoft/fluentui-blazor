// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentFooter
{
    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Class"/>
    /// </summary>
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-footer")
        .Build();

    /// <summary>
    /// <inheritdoc cref="FluentComponentBase.Style"/>
    /// </summary>
    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("height", Height, () => !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary />
    [CascadingParameter]
    protected FluentLayout? Layout { get; set; }

    /// <summary>
    /// Gets or sets the height of the header (in pixels).
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

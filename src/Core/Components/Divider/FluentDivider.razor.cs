// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentText component, codifies Fluent's opinions on typography to make them easy to use and standardize across products.
/// </summary>
public partial class FluentDivider : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the alignment of the content.
    /// </summary>
    public DividerAlignContent? AlignContent { get; set; }

    /// <summary>
    /// Gets or sets the appearance of the content.
    /// </summary>
    public DividerAppearance? Appearance { get; set; }

    /// <summary>
    /// Adds padding to the beginning and end of the divider.
    /// </summary>
    public bool? Inset { get; set; }

    /// <summary>
    /// A divider can be horizontal (default) or vertical.
    /// </summary>
    public bool? Vertical { get; set; }

    /// <summary>
    /// Gets or sets the content to be shown.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? Orientation
        => Vertical.HasValue
            ? Vertical.Value
                ? "vertical"
                : "horizontal"
            : null;
}

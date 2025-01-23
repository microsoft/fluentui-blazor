// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentLabel component is used to display a label for an input component. Normally it is positioned above the component
/// </summary>
public partial class FluentLabel
{

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets whether the label show a required marking (red star).
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the size of the label.
    /// </summary>
    [Parameter]
    public LabelSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the weight of the label text.
    /// </summary>
    [Parameter]
    public LabelWeight? Weight { get; set; }

    /// <summary>
    /// Gets or sets the disabled state of the label.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
///
/// </summary>
public partial class FluentImage : FluentComponentBase
{
    /// <summary/>
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary/>
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    public bool Bordered { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    public bool Block { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    public bool Shadow { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    public ImageShape? Shape { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Parameter]
    public ImageSize? Size { get; set; }
}


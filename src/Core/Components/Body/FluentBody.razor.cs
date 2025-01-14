// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// FluentBody component.
/// </summary>
public partial class FluentBody
{
    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-body")
        .Build();

    /// <summary>
    /// Gets or sets a value indicating whether to use reboot styles.
    /// </summary>
    [Parameter]
    public bool UseReboot { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to add the Blazor script: _framework/blazor.web.js
    /// </summary>
    [Parameter]
    public bool AddBlazorScript { get; set; } = false;

    /// <summary>
    /// Gets or sets the child content of the FluentBody.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The MenuList displays a list of MenuItem options.
/// </summary>
public partial class FluentMenuList : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Raised when a FluentMenuItem is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MenuItemEventArgs> OnClick { get; set; }

    /// <summary>
    /// Raised when a FluentMenuItem's Checked state changes.
    /// </summary>
    [Parameter]
    public EventCallback<MenuItemEventArgs> OnCheckedChanged { get; set; }

    internal async Task NotifyCheckedChangedAsync(MenuItemEventArgs args)
    {
        if (OnCheckedChanged.HasDelegate)
        {
            await OnCheckedChanged.InvokeAsync(args);
        }
    }

    internal async Task NotifyClickedAsync(MenuItemEventArgs args)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(args);
        }
    }
}

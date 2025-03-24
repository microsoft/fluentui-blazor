// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Menu component for handling menus and menu items in a user interface.
/// </summary>
public partial class FluentMenu : FluentComponentBase
{
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets whether the menu opens on hover.
    /// </summary>
    [Parameter]
    public bool? OpenOnHover { get; set; }

    /// <summary>
    /// Gets or sets whether the menu opens on right click.
    /// Removes all other menu open interactions.
    /// </summary>
    [Parameter]
    public bool? OpenOnContext { get; set; }

    /// <summary>
    /// Gets or sets whether the menu when scrolling outside of it.
    /// </summary>
    [Parameter]
    public bool? CloseOnScroll { get; set; }

    /// <summary>
    /// Gets or sets whether the menu stays open when an item is clicked.
    /// </summary>
    [Parameter]
    public bool? PersistOnItemClick { get; set; }

    /// <summary>
    /// Gets or sets the menu trigger.
    /// </summary>
    [Parameter]
    public RenderFragment? Trigger { get; set; }

    /// <summary>
    /// Gets or sets the menu's submenus.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}

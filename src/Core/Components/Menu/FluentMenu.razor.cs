// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Menu component for handling menus and menu items in a user interface.
/// </summary>
public partial class FluentMenu : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = FluentJSModule.JAVASCRIPT_ROOT + "Menu/FluentMenu.razor.js";

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("--menu-max-height", Height, Height != null)
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
    /// Gets or sets the id of the menu trigger.
    /// </summary>
    [Parameter]
    public string? Trigger { get; set; }

    /// <summary>
    /// Gets or sets the max height of the menu, e.g. 300px
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the menu's submenus.
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

    /// <summary>
    /// Constructs a new instance of <see cref="FluentMenu"/>.
    /// Sets the Id to a new random value
    /// </summary>
    public FluentMenu()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Import the JavaScript module
            await JSModule.ImportJavaScriptModuleAsync(JAVASCRIPT_FILE);

            if (Trigger != null)
            {
                await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Menu.Initialize", Id, Trigger);
            }
        }
    }

    /// <summary>
    /// Close the menu.
    /// </summary>
    public async Task CloseMenuAsync()
    {
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Menu.CloseMenu", Id);
    }

    /// <summary>
    /// Open the menu.
    /// </summary>
    public async Task OpenMenuAsync()
    {
        await JSModule.ObjectReference.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Menu.OpenMenu", Id);
    }

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

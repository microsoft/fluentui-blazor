// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Menu component for handling menus and menu items in a user interface.
/// </summary>
public partial class FluentMenu : FluentComponentBase, ITooltipComponent
{
    /// <summary>
    /// Constructs a new instance of <see cref="FluentMenu"/>.
    /// Sets the Id to a new random value
    /// </summary>
    public FluentMenu(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("--menu-max-height", Height, when: !string.IsNullOrEmpty(Height))
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

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    /// <summary />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (Trigger != null)
            {
                await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Menu.Initialize", Id, Trigger);
            }
        }
    }

    /// <summary>
    /// Close the menu.
    /// </summary>
    public async Task CloseMenuAsync()
    {
        await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Menu.CloseMenu", Id);
    }

    /// <summary>
    /// Open the menu.
    /// </summary>
    public Task OpenMenuAsync()
    {
        return OpenMenuAsync(targetId: null, targetOffsetLeft: 0, targetOffsetTop: 0);
    }

    /// <summary>
    /// Open the menu.
    /// </summary>
    /// <param name="targetId">The id of the element to anchor the menu to. If null, it will open relative to the trigger.</param>
    /// <param name="targetOffsetLeft">The left offset from the target element to open the menu. Default is 0.</param>
    /// <param name="targetOffsetTop">The top offset from the target element to open the menu. Default is 0.</param>
    public Task OpenMenuAsync(string? targetId = null, int targetOffsetLeft = 0, int targetOffsetTop = 0)
    {
        return JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Components.Menu.OpenMenu", Id, targetId, targetOffsetLeft, targetOffsetTop).AsTask();
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

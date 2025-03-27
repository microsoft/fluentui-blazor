// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Menu list item which is displayed in a MenuList component.
/// </summary>
public partial class FluentMenuItem : FluentComponentBase
{
    private bool _emptyContent => ChildContent is null && Label is null;
    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the owning FluentMenu.
    /// </summary>
    [CascadingParameter]
    public FluentMenu? Menu { get; set; } = default!;

    /// <summary>
    /// Gets or sets the owning FluentMenuList.
    /// </summary>
    [CascadingParameter]
    public FluentMenuList? MenuList { get; set; } = default!;

    /// <summary>
    /// Gets or sets the role of the menu item.
    /// </summary>
    [Parameter]
    public MenuItemRole? Role { get; set; }

    /// <summary>
    /// Gets or sets the checked state of the menu item.
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }

    /// <summary>
    /// Event raised for checkbox and radio menuitems
    /// </summary>
    [Parameter]
    public EventCallback<bool?> CheckedChanged { get; set; }

    /// <summary>
    /// Gets or sets the menu item's disabled state.
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of button content.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the end of button content.
    /// </summary>
    [Parameter]
    public Icon? IconEnd { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed as the indication of a submenu.
    /// </summary>
    [Parameter]
    public Icon? IconSubmenu { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed as the indicator of being checked .
    /// </summary>
    [Parameter]
    public Icon? IconIndicator { get; set; }

    /// <summary>
    ///  Gets or sets the content to be rendered inside the menu item.
    ///  This can be used as an alternative to specifying the content as a child component of the button.
    ///  If both are specified, both will be rendered.
    ///  </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the list of sub-menu items.
    /// </summary>
    [Parameter]
    public RenderFragment? MenuItems { get; set; }

    /// <summary>
    /// Event raised when the user click on this item.
    /// </summary>
    [Parameter]
    public EventCallback<MenuItemEventArgs> OnClick { get; set; }

    /// <summary />
    internal async Task OnChangeHandlerAsync(MenuItemEventArgs args)
    {
        args.Item = this;

        if (args.Checked is null)
        {
            // It is just a click on a menu item
            if (OnClick.HasDelegate)
            {
                await OnClick.InvokeAsync(args);
            }

            if (Menu is not null)
            {
                await Menu.NotifyClickedAsync(args);
            }
        }
        else
        {
            // The checked state of menu item with a checkbox or radio role has changed.
            // In case of a radio item, the event will fired twice. One time for the unchecked item and
            // once for the checked item
            if (Role == MenuItemRole.Checkbox || Role == MenuItemRole.Radio)
            {

                if (CheckedChanged.HasDelegate)
                {
                    await CheckedChanged.InvokeAsync(args.Checked.Value);
                }

                if (MenuList is not null)
                {
                    await MenuList.NotifyCheckedChangedAsync(args);
                }

                if (Menu is not null)
                {
                    await Menu.NotifyCheckedChangedAsync(args);
                }
            }
        }
    }
}

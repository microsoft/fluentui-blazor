using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenuItem : FluentComponentBase, IDisposable
{
    /// <summary>
    /// Gets or sets the owning FluentMenu.
    /// </summary>
    [CascadingParameter]
    public FluentMenu Owner { get; set; } = default!;

    /// <summary>
    /// Gets or sets the menu item label.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the expanded state of the element.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets the role of the element.
    /// </summary>
    [Parameter]
    public MenuItemRole? Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is checked.
    /// </summary>
    [Parameter]
    public bool Checked { get; set; }

    /// <summary>
    /// Gets or sets a value indicates whether the FluentMenu should remain open after an action.
    /// </summary>
    [Parameter]
    public bool KeepOpen { get; set; }

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
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Event raised for checkbox and radio menuitems
    /// </summary>
    [Parameter]
    public EventCallback<bool> CheckedChanged { get; set; }

    public FluentMenuItem()
    {
        Id = Identifier.NewId();
    }

    protected override void OnInitialized()
    {
        Owner?.Register(this);
    }

    protected async Task OnClickHandlerAsync(MouseEventArgs ev)
    {
        if (Disabled)
        {
            return;
        }

        if (Owner != null && !KeepOpen)
        {
            await Owner.CloseAsync();
        }

        await OnClick.InvokeAsync(ev);
    }

    protected async Task OnChangeHandlerAsync(ChangeEventArgs ev)
    {
        // fluent-menu-item v2 does not pass the checked state as a parameter when emitting
        // the change event so we need to capture the state from the html element using javascript.
        // The value is passed in v3 so javscript lookup won't be necessary. 
        if (Owner != null && Role is MenuItemRole.MenuItemCheckbox or MenuItemRole.MenuItemRadio)
        {
            var isChecked = await Owner.IsCheckedAsync(this);
            Checked = isChecked;

            await CheckedChanged.InvokeAsync(Checked);

            if (Role == MenuItemRole.MenuItemCheckbox || (Role == MenuItemRole.MenuItemRadio && isChecked))
            {
                await Owner.NotifyCheckedChangedAsync(this);
            }
        }
    }

    protected string? GetRole()
    {
        if (Role is not null)
        {
            return Role.ToAttributeValue();
        }
        else
            if (Checked)
        {
            return "menuitemcheckbox";
        }

        return null;
    }

    public void Dispose() => Owner?.Unregister(this);
}

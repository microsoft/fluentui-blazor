using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentMenuItem : FluentComponentBase, IDisposable
{
    private bool _previousChecked = false;

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

    protected override async Task OnParametersSetAsync()
    {
        if (_previousChecked != Checked)
        {
            _previousChecked = Checked;
            await SetCheckedAsync(Checked);
        }
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

        if (Owner != null && (Role == MenuItemRole.MenuItemCheckbox || Role == MenuItemRole.MenuItemRadio))
        {
            await SetCheckedAsync(!Checked);
        }

        await OnClick.InvokeAsync(ev);
    }

    internal async Task SetCheckedAsync(bool value)
    {
        if (Checked == value)
        {
            return;
        }

        Checked = value;

        if (Owner != null && (Role == MenuItemRole.MenuItemCheckbox || Role == MenuItemRole.MenuItemRadio))
        {
            await Owner.NotifyCheckedChangedAsync(this);
        }
        await CheckedChanged.InvokeAsync(Checked);
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

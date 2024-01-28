using Microsoft.AspNetCore.Components;

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
    public EventCallback OnClick { get; set; }

    public FluentMenuItem()
    {
        Id = Identifier.NewId();
    }

    protected override void OnInitialized()
    {
        Owner?.Register(this);
    }

    /// <summary />
    protected async Task OnClickHandlerAsync()
    {
        if (Disabled)
        {
            return;
        }

        if (Owner != null)
        {
            await Owner.CloseAsync();
        }

        await OnClick.InvokeAsync();
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

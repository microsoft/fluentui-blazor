using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Base class for <see cref="FluentNavMenuGroup"/> and <see cref="FluentNavMenuItemBase"/>.
/// </summary>
public abstract class FluentNavMenuItemBase : FluentComponentBase, IDisposable
{
    private bool _disposed;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether the link is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the destination of the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the icon to display with the link
    /// Use a constant value from the <see cref="FluentIcon{Icon}" /> class 
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Called when the user attempts to execute the default action of a menu item.
    /// </summary>
    [Parameter]
    public EventCallback<NavMenuActionArgs> OnAction { get; set; }

    /// <summary>
    /// Gets or sets if the item is selected.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Event callback for when <see cref="Selected"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    /// <summary>
    /// Gets or sets the text of the link.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width of the link (in pixels).
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    [CascadingParameter]
    protected FluentNavMenu NavMenu { get; private set; } = default!;

    [CascadingParameter(Name = "NavMenuExpanded")]
    protected bool NavMenuExpanded { get; private set; }

    [CascadingParameter]
    internal INavMenuItemsOwner Owner { get; private set; } = null!;

    [CascadingParameter(Name = "NavMenuItemSiblingHasIcon")]
    protected bool SiblingHasIcon { get; private set; }

    [Inject]
    protected NavigationManager NavigationManager { get; private set; } = null!;

    /// <summary>
    /// Returns <see langword="true"/> if the item has an <see cref="Icon"/> set.
    /// </summary>
    public bool HasIcon => Icon != null;

    /// <summary>
    /// The tree item associated with this menu item.
    /// </summary>
    protected internal FluentTreeItem TreeItem { get; set; } = null!;


    void IDisposable.Dispose()
    {
        // Do not change this code. Put clean-up code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Called when the user attempts to action a menu item
    /// </summary>
    /// <param name="args">Information about the menu item</param>
    /// <returns></returns>
    protected internal virtual async ValueTask ExecuteAsync(NavMenuActionArgs args)
    {
        if (OnAction.HasDelegate)
        {
            await OnAction.InvokeAsync(args);
            if (args.Handled)
            {
                return;
            }
        }
        if (!string.IsNullOrEmpty(Href))
        {
            NavigationManager.NavigateTo(Href);
            args.SetHandled();
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Owner.Register(this);
        NavMenu.Register(this);
    }

    protected internal async Task SetSelectedAsync(bool value)
    {
        if (value == Selected)
        {
            return;
        }

        Selected = value;
        if (SelectedChanged.HasDelegate)
        {
            await SelectedChanged.InvokeAsync(value);
        }
        StateHasChanged();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            Owner.Unregister(this);
            NavMenu.Unregister(this);
        }

        _disposed = true;
    }
}

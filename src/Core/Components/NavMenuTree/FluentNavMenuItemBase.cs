using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
    /// Gets or sets a value indicating whether the item is selected.
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
#pragma warning disable CS0618 // Type or member is obsolete
    protected FluentNavMenuTree NavMenu { get; private set; } = default!;
#pragma warning restore CS0618 // Type or member is obsolete

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
    /// Gets or sets the tree item associated with this menu item.
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
            args.SetHandled();
            if (args.ReNavigate || NeedsNavigation())
            {
                NavigationManager.NavigateTo(Href);
            }
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

    private bool NeedsNavigation()
    {
        if (!string.IsNullOrEmpty(Href) && Href != "/")
        {
            // If the current page is the same as the Href, don't navigate
            if (new Uri(NavigationManager.Uri).LocalPath.Equals(Href, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            // If the local path starts with this Href (with an added "/"), don't navigate 
            // Example local path: https://.../Panel/Panel2 starts with Href: https://.../Panel + "/"  
            // Extra "/" is needed to avoid a match on http://.../Panel for https://.../Panels 
            if (new Uri(NavigationManager.Uri).LocalPath.StartsWith(Href + "/", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
        }

        Uri baseUri = new(NavigationManager.BaseUri);
        Uri currentUri = new(NavigationManager.Uri);
        if (Uri.TryCreate(baseUri, Href, out Uri? comparisonUri) && comparisonUri.Equals(currentUri))
        {
            return false;
        }

        return true;
    }

}

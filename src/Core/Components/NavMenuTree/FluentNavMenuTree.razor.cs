using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

//[Obsolete("This component has been replaced with the FluentNavMenu and will be removed in a future version.")]
public partial class FluentNavMenuTree : FluentComponentBase, INavMenuItemsOwner, IDisposable
{
    private const string WIDTH_COLLAPSED_MENU = "40px";
    private bool _disposed;
    private bool HasChildIcons => ((INavMenuItemsOwner)this).HasChildIcons;
    private readonly Dictionary<string, FluentNavMenuItemBase> _allItems = [];
    private readonly List<FluentNavMenuItemBase> _childItems = [];
    private readonly string _expandCollapseTreeItemId = Identifier.NewId();
    private FluentTreeItem? _currentlySelectedTreeItem;
    private FluentTreeItem? _previousSuccessfullySelectedTreeItem;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu")
        .AddClass("collapsed", Collapsed)
        .AddClass("navmenu-parent-element")
        .Build();

    protected string? StyleValue => new StyleBuilder(Style)
        .AddStyle("width", $"{Width}px", () => Expanded && Width.HasValue)
        .AddStyle("width", WIDTH_COLLAPSED_MENU, () => !Expanded)
        .AddStyle("min-width", WIDTH_COLLAPSED_MENU, () => !Expanded)
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered for the expander icon when the menu is collapsible. 
    /// The default icon will be used if this is not specified.
    /// </summary>
    [Parameter]
    public RenderFragment? ExpanderContent { get; set; }

    /// <summary>
    /// Gets or sets the title of the navigation menu.
    /// Default to "Navigation menu".
    /// </summary>
    [Parameter]
    public string? Title { get; set; } = "Navigation menu";

    /// <summary>
    /// Gets or sets the width of the menu (in pixels).
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets whether or not the menu can be collapsed.
    /// </summary>
    [Parameter]
    public bool Collapsible { get; set; }

    /// <inheritdoc/>
    [Parameter]
    public bool Expanded { get; set; } = true;

    /// <summary>
    /// Event callback for when the <see cref="Expanded"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Called when the user attempts to execute the default action of a menu item.
    /// </summary>
    [Parameter]
    public EventCallback<NavMenuActionArgs> OnAction { get; set; }

    /// <summary>
    /// If set to <see langword="true"/> then the tree will expand when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallyExpanded { get; set; }

    /// <summary>
    /// If true, the menu will re-navigate to the current page when the user clicks on the currently selected menu item.
    /// </summary>
    [Parameter]
    public bool ReNavigate { get; set; } = false;

    /// <inheritdoc/>
    public bool Collapsed => !Expanded;

    /// <summary>
    /// Navigation manager
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; private set; } = null!;

    public FluentNavMenuTree()
    {
        Id = Identifier.NewId();
    }

    /// <inheritdoc/>
    IEnumerable<FluentNavMenuItemBase> INavMenuItemsOwner.GetChildItems() => _childItems;

    /// <inheritdoc/>
    void INavMenuItemsOwner.Register(FluentNavMenuItemBase child)
    {
        _childItems.Add(child);
        StateHasChanged();
    }

    /// <inheritdoc/>
    void INavMenuItemsOwner.Unregister(FluentNavMenuItemBase child)
    {
        _childItems.Remove(child);
        StateHasChanged();
    }

    void IDisposable.Dispose()
    {
        // Do not change this code. Put clean-up code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    internal void Register(FluentNavMenuItemBase item)
    {
        _allItems[item.Id!] = item;
        StateHasChanged();
    }

    internal async Task MenuItemExpandedChangedAsync(INavMenuItemsOwner menuItem)
    {
        if (menuItem.Id == _expandCollapseTreeItemId)
        {
            return;
        }

        if (menuItem.Expanded && !Expanded)
        {
            await SetExpandedAsync(value: true);
        }
    }

    internal void Unregister(FluentNavMenuItemBase item)
    {
        _allItems.Remove(item.Id!);
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        NavigationManager.LocationChanged += HandleNavigationManagerLocationChanged;

        if (InitiallyExpanded)
        {
            await SetExpandedAsync(true);
        }
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            SelectMenuItemForCurrentUrl();
        }
    }

    private void SelectMenuItemForCurrentUrl()
    {
        HandleNavigationManagerLocationChanged(null, new LocationChangedEventArgs(NavigationManager.Uri, isNavigationIntercepted: false));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            NavigationManager.LocationChanged -= HandleNavigationManagerLocationChanged;
            _allItems.Clear();
            _childItems.Clear();
        }

        _disposed = true;
    }

    private Task ToggleCollapsedAsync() => SetExpandedAsync(!Expanded);

    private async void HandleNavigationManagerLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        FluentNavMenuItemBase? menuItem = null;

        var localPath = new Uri(NavigationManager.Uri).LocalPath;
        if (string.IsNullOrEmpty(localPath))
        {
            localPath = "/";
        }

        if (localPath == "/")
        {
            if (_allItems.Count > 0)
            {
                menuItem = _allItems.Values.ElementAt(0);
            }
        }
        else
        {
            // This will match the first item that has a Href that matches the current URL exactly
            menuItem = _allItems.Values
                .Where(x => !string.IsNullOrEmpty(x.Href))
                .FirstOrDefault(x => x.Href != "/" && localPath.Equals(x.Href!, StringComparison.InvariantCultureIgnoreCase));

            // If not found, try to match the first item that has a Href (ending in a "/") that starts with the current URL 
            // URL: https://.../Panel/Panel2 starts with Href: https://.../Panel + "/"  
            // Extra "/" is needed to avoid matching https://.../Panels with https://.../Panel
            menuItem ??= _allItems.Values
                .Where(x => !string.IsNullOrEmpty(x.Href))
                .FirstOrDefault(x => x.Href != "/" && localPath.StartsWith(x.Href! + "/", StringComparison.InvariantCultureIgnoreCase));
        }
        if (menuItem is not null)
        {
            _currentlySelectedTreeItem = menuItem.TreeItem;
            _previousSuccessfullySelectedTreeItem = menuItem.TreeItem;
            await _currentlySelectedTreeItem.SetSelectedAsync(true);
        }
    }

    private async Task HandleExpandCollapseKeyDownAsync(KeyboardEventArgs args)
    {
        Task handler = args.Code switch
        {
            "Enter" => SetExpandedAsync(true),
            "ArrowRight" => SetExpandedAsync(true),
            "ArrowLeft" => SetExpandedAsync(false),
            _ => Task.CompletedTask
        };
        await handler;
    }

    private async Task SetExpandedAsync(bool value)
    {
        if (value == Expanded)
        {
            return;
        }

        Expanded = value;
        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(value);
        }

        StateHasChanged();
    }

    private async Task HandleCurrentSelectedChangedAsync(FluentTreeItem? treeItem)
    {
        // If an already activated menu item is clicked again, then it will
        // match the previously selected one but have Selected == false.
        // In this case, the user has indicated they wish to re-trigger
        // its action. For the case of a simple navigation, this will be the same
        // page and therefore do nothing.
        // But for a nav menu with custom actions like showing a dialog etc, it will
        // re-trigger and repeat that action.

        // itemWasClickedWhilstAlreadySelected will never be true as treeItem is null when the treeItem is clicked again
        // left the code in for now but this should probably be removed
        //bool itemWasClickedWhilstAlreadySelected = treeItem?.Selected == false && treeItem == _previousSuccessfullySelectedTreeItem;
        //if (itemWasClickedWhilstAlreadySelected)
        //{
        //    await TryActivateMenuItemAsync(treeItem);
        //    return;
        //}

        if (treeItem is null && _previousSuccessfullySelectedTreeItem is not null && ReNavigate)
        {
            await TryActivateMenuItemAsync(_previousSuccessfullySelectedTreeItem, true);
            return;
        }
        // If the user has selected a different item, then it will not match the previously
        // selected item, and it will have Selected == true.
        // So try to activate the new one instead of the old one.
        // If it succeeds then keep it selected, if it fails then revert to the last successfully selected
        // tree item. This prevents the user from selecting an item with no Href or custom action.
        if (treeItem?.Selected == true && _allItems.TryGetValue(treeItem.Id!, out _))
        {
            var activated = await TryActivateMenuItemAsync(treeItem);
            if (activated)
            {
                _currentlySelectedTreeItem = treeItem;
                _previousSuccessfullySelectedTreeItem = treeItem;
            }
            else
            {
                _currentlySelectedTreeItem = _previousSuccessfullySelectedTreeItem;
            }
        }

        // At this point we have either
        // 1) Succeeded,
        // 2) Failed and reverted to a previously successful item without re-executing its action,
        // 3) Failed and have no previously successful item to revert to.
        // If we have no currently selected item then we fall back to selecting whichever matches the current
        // URI.
        if (_currentlySelectedTreeItem is null)
        {
            SelectMenuItemForCurrentUrl();
        }

        // Now we need to ensure the currently selected item has Selected=true, and
        // the previous has Selected=false
        if (_currentlySelectedTreeItem?.Selected == false)
        {
            await _currentlySelectedTreeItem.SetSelectedAsync(true);
        }

        if (_currentlySelectedTreeItem?.Selected == true)
        {
            if (_previousSuccessfullySelectedTreeItem?.Selected == true && _previousSuccessfullySelectedTreeItem != _currentlySelectedTreeItem)
            {
                await _previousSuccessfullySelectedTreeItem.SetSelectedAsync(false);
            }
            _previousSuccessfullySelectedTreeItem = _currentlySelectedTreeItem;
        }

        // If we still don't have a currently selected item, then make sure the invalid one
        // the user tried to select is not selected.
        if (treeItem?.Selected == true && treeItem != _currentlySelectedTreeItem)
        {
            await treeItem.SetSelectedAsync(false);
        }
    }

    private async ValueTask<bool> TryActivateMenuItemAsync(FluentTreeItem? treeItem, bool renavigate = false)
    {
        if (treeItem is null)
        {
            return false;
        }

        if (!_allItems.TryGetValue(treeItem.Id!, out FluentNavMenuItemBase? menuItem))
        {
            return false;
        }

        NavMenuActionArgs? actionArgs = new(target: menuItem, renavigate: renavigate);
        if (OnAction.HasDelegate)
        {
            await OnAction.InvokeAsync(actionArgs);
        }

        if (!actionArgs.Handled)
        {
            await menuItem.ExecuteAsync(actionArgs);
        }

        if (actionArgs.Handled)
        {
            await menuItem.SetSelectedAsync(true);
        }

        return actionArgs.Handled;
    }

}

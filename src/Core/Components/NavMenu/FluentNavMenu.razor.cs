using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenu : FluentComponentBase, INavMenuItemsOwner, IDisposable
{
    private const string WIDTH_COLLAPSED_MENU = "40px";
    private bool _disposed;
    private bool _hasChildIcons => ((INavMenuItemsOwner)this).HasChildIcons;
    private readonly Dictionary<string, FluentNavMenuItemBase> _allItems = new();
    private readonly List<FluentNavMenuItemBase> _childItems = new();
    private readonly string _expandCollapseTreeItemId = Identifier.NewId();
    private FluentTreeItem? _currentlySelectedTreeItem;
    private FluentTreeItem? _previousSuccessfullySelectedTreeItem;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu")
        .AddClass("collapsed", Collapsed)
        .AddClass("navmenu-parent-element")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Expanded && Width.HasValue)
        .AddStyle("width", WIDTH_COLLAPSED_MENU, () => !Expanded)
        .AddStyle("min-width", WIDTH_COLLAPSED_MENU, () => !Expanded)
        .AddStyle(Style)
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered for the expander icon
    /// when the menu is collapsible.  The default icon will be used if
    /// this is not specified.
    /// </summary>
    [Parameter]
    public RenderFragment? ExpanderContent { get; set; }

    /// <summary>
    /// Gets or sets the title of the navigation menu
    /// Default to "Navigation menu"
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
    /// If set to <see langword="true"/> then the tree will
    /// expand when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallyExpanded { get; set; }

    /// <inheritdoc/>
    public bool Collapsed => !Expanded;

    /// <summary>
    /// Navigation manager
    /// </summary>
    [Inject]
    protected NavigationManager NavigationManager { get; private set; } = null!;

    public FluentNavMenu()
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
        await base.OnInitializedAsync();

        NavigationManager.LocationChanged += HandleNavigationManagerLocationChanged;

        if (InitiallyExpanded)
        {
            await SetExpandedAsync(true);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
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

    private void HandleNavigationManagerLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        string localPath = new Uri(NavigationManager.Uri).LocalPath;
        if (string.IsNullOrEmpty(localPath))
            localPath = "/";

        FluentNavMenuItemBase? menuItem = _allItems.Values
            .FirstOrDefault(x => x.Href == localPath);

        if (menuItem is not null)
        {
            _currentlySelectedTreeItem = menuItem.TreeItem;
            _previousSuccessfullySelectedTreeItem = menuItem.TreeItem;
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
        // If an already activated menu item is clicked again, then re-trigger
        // its action. For the case of a simple navigation, this will be the same
        // page and therefore do nothing.
        // But for a nav menu with custom actions like showing a dialog etc, it will
        // re-trigger and repeat that action.
        bool itemWasClickedWhilstAlreadySelected =
            treeItem?.Selected == false && treeItem == _previousSuccessfullySelectedTreeItem;
        if (itemWasClickedWhilstAlreadySelected)
        {
            await TryActivateMenuItemAsync(treeItem);
            return;
        }

        // Otherwise, the user selected a different tree item. So try to activate that one instead.
        // If it succeeds then keep it selected, if it fails then revert to the last successfully selected
        // tree item. This prevents the user from selecting an item with no Href or custom action.
        if (treeItem?.Selected == true && _allItems.TryGetValue(treeItem.Id!, out FluentNavMenuItemBase? menuItem))
        {
            bool activated = await TryActivateMenuItemAsync(treeItem);
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

        // At this point we have either succeeded, failed and reverted to a previously successful item
        // without re-executing its action, or failed and have no previously successful item to revert to.
        // If we have no currently selected item then we fall back to selecting whichever matches the current
        // URI.
        if (_currentlySelectedTreeItem is null)
        {
            SelectMenuItemForCurrentUrl();
        }

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

        // If we still don't have a currently selected item, then make sure the one
        // the user tried to select is not selected.
        if (treeItem?.Selected == true && treeItem != _currentlySelectedTreeItem)
        {
            await treeItem.SetSelectedAsync(false);
        }
    }

    private async ValueTask<bool> TryActivateMenuItemAsync(FluentTreeItem? treeItem)
    {
        if (treeItem is null)
        {
            return false;
        }

        if (!_allItems.TryGetValue(treeItem.Id!, out FluentNavMenuItemBase? menuItem))
        {
            return false;
        }

        var actionArgs = new NavMenuActionArgs(target: menuItem);
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

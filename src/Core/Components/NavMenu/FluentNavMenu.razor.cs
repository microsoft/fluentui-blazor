using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenu : FluentComponentBase, INavMenuItemsOwner, IDisposable
{
    private const string WIDTH_COLLAPSED_MENU = "40px";
    private readonly string _expandCollapseTreeItemId = Identifier.NewId();
    private readonly Dictionary<string, FluentNavMenuItem> _allItems = new();
    private readonly List<FluentNavMenuItem> _childItems = new();
    private FluentTreeItem? _selectedTreeItem;
    private FluentTreeItem? _previouslyDeselectedTreeItem;
    private bool _hasRendered;

    private bool HasChildIcons => ((INavMenuItemsOwner)this).HasChildIcons;

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
    /// Gets or sets the content to be rendered for the navigation icon
    /// when the menu is collapsible.  The default icon will be used if
    /// this is not specified.
    /// </summary>
    [Parameter]
    public RenderFragment? NavigationIconContent { get; set; }

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
    /// Gets or sets whether the menu can be collapsed.
    /// </summary>
    [Parameter]
    public bool Collapsible { get; set; } = true;

    /// <summary>
    /// Returns <see langword="true"/> if the nav menu item is expanded,
    /// and <see langword="false"/> if collapsed.
    /// </summary>    [Parameter]
    public bool Expanded { get; set; } = true;

    /// <summary>
    /// Event callback for when the <see cref="Expanded"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Called whenever a contained <see cref="FluentNavMenuGroup"/> is selected or unselected.
    /// </summary>
    [Parameter]
    public EventCallback<FluentNavMenuGroup> OnGroupSelected { get; set; }

    /// <summary>
    /// Called whever a contained <see cref="FluentNavMenuLink"/> is selected or unselected.
    /// </summary>
    [Parameter]
    public EventCallback<FluentNavMenuLink> OnLinkSelected { get; set; }


    /// <summary>
    /// If set to <see langword="true"/> then the tree will
    /// expand when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallyExpanded { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the tree item is collapsed,
    /// and <see langword="false"/> if expanded.
    /// </summary>
    public bool Collapsed => !Expanded;

    public FluentNavMenu()
    {
        Id = Identifier.NewId();
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

    IEnumerable<FluentNavMenuItem> INavMenuItemsOwner.GetChildItems() => _childItems;

    void INavMenuItemsOwner.Register(FluentNavMenuItem child)
    {
        _allItems.Add(child.Id!, child);
        StateHasChanged();
    }

    void INavMenuItemsOwner.Unregister(FluentNavMenuItem child)
    {
        _allItems.Remove(child.Id!);
        StateHasChanged();
    }

    void IDisposable.Dispose()
    {
        NavigationManager.LocationChanged -= HandleNavigationManagerLocationChanged;
        _allItems.Clear();
    }

    internal void Register(FluentNavMenuItem item)
    {
        _allItems[item.Id!] = item;
        StateHasChanged();
    }

    internal void Unregister(FluentNavMenuItem item)
    {
        _allItems.Remove(item.Id!);
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (firstRender)
        {
            _hasRendered = true;
            HandleNavigationManagerLocationChanged(null, new LocationChangedEventArgs(NavigationManager.Uri, isNavigationIntercepted: false));
        }

        bool hasSelectedItem = _selectedTreeItem is not null && _selectedTreeItem.Selected;
        if (!hasSelectedItem && _hasRendered && _previouslyDeselectedTreeItem is not null)
        {
            await _previouslyDeselectedTreeItem.SetSelectedAsync(true);
            StateHasChanged();
        }
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

    private Task ToggleCollapsedAsync() => SetExpandedAsync(!Expanded);

    private void HandleNavigationManagerLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        string localPath = new Uri(NavigationManager.Uri).LocalPath;
        if (string.IsNullOrEmpty(localPath))
            localPath = "/";

        FluentNavMenuItem? menuItem = _allItems.Values
            .FirstOrDefault(x => x.Href == localPath);

        if (menuItem is not null)
        {
            _selectedTreeItem = menuItem.TreeItem;
            _previouslyDeselectedTreeItem = _selectedTreeItem;
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

    private void HandleCurrentSelectedChanged(FluentTreeItem? treeItem)
    {
        if (treeItem?.Selected != true)
        {
            _previouslyDeselectedTreeItem = _selectedTreeItem;
            _selectedTreeItem = null;
            return;
        }

        if (!_allItems.TryGetValue(treeItem.Id!, out FluentNavMenuItem? menuItem))
        {
            return;
        }

        if (string.IsNullOrEmpty(menuItem.Href))
        {
            return;
        }

        string localPath = new Uri(NavigationManager.Uri).LocalPath;
        localPath = localPath == "" ? "/" : localPath;

        if (string.Equals(localPath, menuItem.Href, StringComparison.InvariantCultureIgnoreCase))
        {
            return;
        }

        _selectedTreeItem = treeItem;
        NavigationManager.NavigateTo(menuItem.Href);
    }
}

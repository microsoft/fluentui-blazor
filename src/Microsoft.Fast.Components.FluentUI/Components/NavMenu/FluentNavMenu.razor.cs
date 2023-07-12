using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenu : FluentComponentBase, INavMenuParentElement, IDisposable
{
    private const string WIDTH_COLLAPSED_MENU = "40px";
    private readonly string _expandCollapseTreeItemId = Identifier.NewId();
    private readonly Dictionary<string, INavMenuChildElement> _childElements = new();
    private FluentTreeItem? _selectedTreeItem;

    private bool HasChildIcons => ((INavMenuParentElement)this).HasChildIcons;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu")
        .AddClass("collapsed", !Expanded)
        .AddClass("navmenu-parent-element")
        .AddClass("navmenu-element")
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

    internal async Task MenuItemExpandedChangedAsync(INavMenuParentElement menuItem)
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

    IEnumerable<INavMenuChildElement> INavMenuParentElement.GetChildElements() => _childElements.Values;

    void INavMenuParentElement.Register(INavMenuChildElement child)
    {
        _childElements.Add(child.Id!, child);
        StateHasChanged();
    }

    void INavMenuParentElement.Unregister(INavMenuChildElement child)
    {
        _childElements.Remove(child.Id!);
        StateHasChanged();
    }

    void IDisposable.Dispose()
    {
        NavigationManager.LocationChanged -= HandleNavigationManagerLocationChanged;
        _childElements.Clear();
    }

    internal void Register(INavMenuChildElement element)
    {
        _childElements[element.Id!] = element;
    }

    internal void Unregister(INavMenuChildElement element)
    {
        _childElements.Remove(element.Id!);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
        if (firstRender)
        {
            //HandleNavigationManagerLocationChanged(null, new LocationChangedEventArgs(NavigationManager.Uri, isNavigationIntercepted: false));
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

        INavMenuChildElement? menuItem = _childElements.Values
            .FirstOrDefault(x => x.Href == localPath);

        if (menuItem is not null)
        {
            _selectedTreeItem = menuItem.TreeItem;
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
        //if (treeItem?.Selected != true)
        //{
        //    _selectedTreeItem = null;
        //    return;
        //}

        //if (!_childElements.TryGetValue(treeItem.Id!, out INavMenuChildElement? menuItem))
        //{
        //    return;
        //}

        //if (string.IsNullOrEmpty(menuItem.Href))
        //{
        //    return;
        //}

        //string localPath = new Uri(NavigationManager.Uri).LocalPath;
        //localPath = localPath == "" ? "/" : localPath;

        //if (string.Equals(localPath, menuItem.Href, StringComparison.InvariantCultureIgnoreCase))
        //{
        //    return;
        //}

        //_selectedTreeItem = treeItem;
        //NavigationManager.NavigateTo(menuItem.Href);
    }
}

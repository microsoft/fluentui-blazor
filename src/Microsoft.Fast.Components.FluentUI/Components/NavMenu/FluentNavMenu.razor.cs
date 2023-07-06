using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenu : FluentComponentBase, INavMenuItemsHolder, IDisposable
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private const string WIDTH_COLLAPSED_MENU = "40px";
    private string _prevHref = "/";
    private readonly string _expandCollapseTreeItemId = Identifier.NewId();

    private bool HasChildIcons => ((INavMenuItemsHolder)this).HasChildIcons;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu")
        .AddClass("collapsed", !Expanded)
        .AddClass("navmenu-item-holder")
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
    /// Gets or sets whether the menu is collapsed.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; } = true;

    /// <summary>
    /// Event callback for when the menu is collapsed status changed.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Event callback for when group/link is expanded
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnExpanded { get; set; }

    public FluentNavMenu()
    {
        Id = Identifier.NewId();
    }

    private bool Collapsed => !Expanded;

    private readonly List<INavMenuItem> _navMenuItems = new();

    private Task ToggleCollapsedAsync() => 
        Expanded
        ? CollapseAsync()
        : ExpandAsync();

    /// <summary>
    /// Ensures the <see cref="FluentNavMenu"/> is collasped.
    /// </summary>
    /// <returns></returns>
    public async Task CollapseAsync()
    {
        if (Collapsed || !Collapsible)
            return;

        Expanded = false;

        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(Expanded);
        }

        StateHasChanged();
    }


    /// <summary>
    /// Ensures the <see cref="FluentNavMenu"/> is expanded.
    /// </summary>
    /// <returns></returns>
    public async Task ExpandAsync()
    {
        if (Expanded)
            return;

        Expanded = true;

        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(Expanded);
        }

        if (OnExpanded.HasDelegate)
        {
            await OnExpanded.InvokeAsync(Expanded);
        }
        StateHasChanged();
    }

    private async Task HandleExpandCollapseKeyDownAsync(KeyboardEventArgs args)
    {
        Task handler = args.Code switch
        {
            "Enter" => ExpandAsync(),
            "ArrowRight" => ExpandAsync(),
            "ArrowLeft" => CollapseAsync(),
            _ => Task.CompletedTask
        };
        await handler;
    }

    private void HandleSelectedChange(FluentTreeItem treeItem)
    {
        if (treeItem.Selected && treeItem.Id != _expandCollapseTreeItemId)
        {
            INavMenuItem? menuItem = ((INavMenuItemsHolder)this).GetItemById(treeItem.Id);
            string? href = menuItem?.Href;
            if (!string.IsNullOrWhiteSpace(href) && href != _prevHref)
            {
                _prevHref = href;
                NavigationManager.NavigateTo(href);
            }
        }
    }

    void IDisposable.Dispose()
    {
        _navMenuItems.Clear();
    }

    internal async Task GroupExpandedAsync(FluentNavMenuGroup group)
    {
        if (Collapsed)
            await ExpandAsync();
    }

    void INavMenuItemsHolder.AddItem(INavMenuItem item)
    {
        _navMenuItems.Add(item);
        StateHasChanged();
    }

    void INavMenuItemsHolder.RemoveItem(INavMenuItem item)
    {
        _navMenuItems.Remove(item);
        StateHasChanged();
    }

    IEnumerable<INavMenuItem> INavMenuItemsHolder.GetItems() => _navMenuItems;
}

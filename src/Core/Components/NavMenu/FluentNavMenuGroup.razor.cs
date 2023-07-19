using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenuGroup : FluentNavMenuItemBase, INavMenuItemsOwner, IDisposable
{
    private readonly List<FluentNavMenuItemBase> _childItems = new();
    private bool HasChildIcons => ((INavMenuItemsOwner)this).HasChildIcons;
    private bool Visible => NavMenu.Expanded || HasIcon;

    /// <summary>
    /// Returns <see langword="true"/> if the group is expanded,
    /// and <see langword="false"/> if collapsed.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets a callback that is triggered whenever <see cref="Expanded"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// If set to <see langword="true"/> then the tree will
    /// expand when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallyExpanded { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if the group is collapsed,
    /// and <see langword="false"/> if expanded.
    /// </summary>
    public bool Collapsed => !Expanded;

    public FluentNavMenuGroup()
    {
        Id = Identifier.NewId();
    }

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-parent-element")
        .AddClass("navmenu-group")
        .AddClass("navmenu-child-element")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (InitiallyExpanded && Collapsed)
        {
            Expanded = true;
            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(true);
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _childItems.Clear();
        }
    }

    void INavMenuItemsOwner.Register(FluentNavMenuItemBase child)
    {
        _childItems.Add(child);
        StateHasChanged();
    }

    void INavMenuItemsOwner.Unregister(FluentNavMenuItemBase child)
    {
        _childItems.Remove(child);
        StateHasChanged();
    }

    IEnumerable<FluentNavMenuItemBase> INavMenuItemsOwner.GetChildItems() => _childItems;

    protected internal override async ValueTask ExecuteAsync(NavMenuActionArgs args)
    {
        await base.ExecuteAsync(args);

        bool shouldExpand = Collapsed || NavMenu.Collapsed;
        if (shouldExpand)
        {
            await SetExpandedAsync(true);
        }
    }

    // Always render a group's child items when the nav menu is expanded.
    // Otherwise, only groups directly parented by the nav menu should render
    // their child items; this is so the web components know the item has
    // children and will allow the arrow keys to expand the group.
    private bool GetShouldRenderChildContent() => NavMenu.Expanded || Owner == NavMenu;

    private Task ToggleCollapsedAsync() => SetExpandedAsync(!Expanded);

    private async Task SetExpandedAsync(bool value)
    {
        if (value != Expanded)
        {
            Expanded = value;
            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(value);
            }
        }

        await NavMenu.MenuItemExpandedChangedAsync(this);
    }

    private async Task HandleSelectedChangedAsync(bool value)
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
    }
}

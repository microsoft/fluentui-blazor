using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenuGroup : FluentNavMenuItemBase, INavMenuItemsOwner, IDisposable
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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

    private readonly List<FluentNavMenuItemBase> _childItems = new();
    private bool HasChildIcons => ((INavMenuItemsOwner)this).HasChildIcons;

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

    private bool Visible => NavMenu.Expanded || HasIcon;

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

    private Task ToggleCollapsedAsync() => HandleExpandedChangedAsync(!Expanded);

    private async Task HandleExpandedChangedAsync(bool value)
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

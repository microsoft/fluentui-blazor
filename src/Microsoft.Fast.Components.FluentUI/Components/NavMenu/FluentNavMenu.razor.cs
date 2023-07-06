using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenu : FluentComponentBase, IDisposable
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private const string WIDTH_COLLAPSED_MENU = "40px";
    private readonly List<FluentNavMenuLink> _links = new();
    private readonly List<FluentNavMenuGroup> _groups = new();
    private string _prevHref = "/";
    private readonly string _expandCollapseTreeItemId = Identifier.NewId();

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu")
        .AddClass("collapsed", !Expanded)
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

    internal bool HasSubMenu => _groups.Any();

    internal bool HasIcons => _links.Any(i => i.HasIcon) || _groups.Any(g => g.HasIcon);

    internal async Task CollapsibleClickAsync()
    {
        if (Collapsible)
        {
            Expanded = !Expanded;
            await InvokeAsync(StateHasChanged);

            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(Expanded);
            }

            if (OnExpanded.HasDelegate)
            {
                await OnExpanded.InvokeAsync(Expanded);
            }
        }
    }

    internal async Task HandleSelectedChangeAsync(FluentTreeItem treeItem)
    {
        if (treeItem.Id == _expandCollapseTreeItemId)
        {
            await CollapsibleClickAsync();
            return;
        }
        string? href = _links.FirstOrDefault(x => x.Id == treeItem.Id)?.Href;
        if (string.IsNullOrWhiteSpace(href))
        {
            href = _groups.FirstOrDefault(x => x.Id == treeItem.Id)?.Href;
        }
        if (!string.IsNullOrWhiteSpace(href) && href != _prevHref)
        {
            _prevHref = href;
            NavigationManager.NavigateTo(href);
        }
    }

    internal void AddNavMenuLink(FluentNavMenuLink link)
    {
        _links.Add(link);
    }

    internal void RemoveNavMenuLink(FluentNavMenuLink link)
    {
        _links.Remove(link);
    }

    internal void AddNavMenuGroup(FluentNavMenuGroup group)
    {
        _groups.Add(group);
    }

    internal void RemoveNavMenuGroup(FluentNavMenuGroup group)
    {
        _groups.Remove(group);
    }

    public void Dispose()
    {
        _links.Clear();
        _groups.Clear();
    }

}

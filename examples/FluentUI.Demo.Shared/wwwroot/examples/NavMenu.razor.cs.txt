using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;

public partial class NavMenu : FluentComponentBase
{
    internal readonly List<NavMenuLink> _links = new();
    internal readonly List<NavMenuGroup> _groups = new();

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", Width, () => Collapsed && !string.IsNullOrEmpty(Width))
        .AddStyle("width", "35px", () => !Collapsed)
        //.AddStyle("min-width", "unset", () => !Collapsed)
        .AddStyle(Style)
        .Build();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private IJSObjectReference Module { get; set; } = default!;

    [Parameter]
    public string? Title { get; set; } = "Navigation menu";

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public bool Collapsible { get; set; } = true;

    [Parameter]
    [CascadingParameter()]
    public bool Collapsed { get; set; } = true;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    internal bool HasSubMenu => _groups.Any();

    internal bool HasIcons => _links.Any(i => !String.IsNullOrWhiteSpace(i.Icon));

    protected void Collapsible_Click(MouseEventArgs e)
    {
        if (Collapsible)
        {
            Collapsed = !Collapsed;
        }
    }

    internal int AddNavLink(NavMenuLink link)
    {
        _links.Add(link);
        return _links.Count;
    }

    internal int AddNavGroup(NavMenuGroup group)
    {
        _groups.Add(group);
        return _groups.Count;
    }

    private void HandleCurrentSelectedChanged(FluentTreeItem item)
    {
        string? Href = _links.FirstOrDefault(i => i.Selected)?.Href;
        if (!String.IsNullOrEmpty(Href))
            NavigationManager.NavigateTo(Href);
    }

    internal void SelectOnlyThisLink(NavMenuLink? selectedLink)
    {
        if (selectedLink != null)
        {
            foreach (var link in _links)
            {
                if (link == selectedLink)
                {
                    link.SetSelected(true);
                }
                else
                {
                    link.SetSelected(false);
                }
            }
        }
    }

    internal void SelectOnlyThisGroup(NavMenuGroup? selectedGroup)
    {
        if (selectedGroup != null)
        {
            foreach (var group in _groups)
            {
                if (group == selectedGroup)
                {
                    group.SetSelected(true);
                }
                else
                {
                    group.SetSelected(false);
                }
            }
        }
    }
}

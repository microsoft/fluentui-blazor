using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;

public partial class NavMenu : FluentComponentBase
{
    // Remeber to replace the path to the colocated JS file with your own project's path
    // or Razor Class Library's path.
    //private const string JAVASCRIPT_FILE = "./_content/FluentUI.Demo.Shared/Pages/Lab/NavMenu/NavMenu.razor.js";
    private const string WIDTH_COLLAPSED_MENU = "40px";
    private readonly List<NavMenuLink> _links = new();
    private readonly List<NavMenuGroup> _groups = new();

    private FluentTreeItem? currentSelected;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Expanded && Width.HasValue)
        .AddStyle("width", WIDTH_COLLAPSED_MENU, () => !Expanded)
        .AddStyle(Style)
        .Build();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    //[Inject]
    //private IJSRuntime JS { get; set; } = default!;

    //private IJSObjectReference Module { get; set; } = default!;

    /// <summary>
    /// Gets or sets a reasonably unique ID 
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Identifier.NewId();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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

    internal bool HasSubMenu => _groups.Any();

    internal bool HasIcons => _links.Any(i => !string.IsNullOrWhiteSpace(i.Icon));

    internal async Task CollapsibleClickAsync(MouseEventArgs e)
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

    internal void AddNavMenuLink(NavMenuLink link)
    {
        _links.Add(link);
        //return _links.Count;
    }

    internal void AddNavMenuGroup(NavMenuGroup group)
    {
        _groups.Add(group);
        //return _groups.Count;
    }

    internal void NavigateTo(string href)
    {
        NavigationManager.NavigateTo(href);
    }
}

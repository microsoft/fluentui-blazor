using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Shared;

public partial class FluentNavMenu : FluentComponentBase
{
    private const string JAVASCRIPT_FILE = "./_content/FluentUI.Demo.Shared/Pages/Lab/Examples/FluentNavMenu.razor.js";
    private const string WIDTH_COLLAPSED_MENU = "40px";
    internal readonly List<FluentNavLink> _links = new();
    internal readonly List<FluentNavGroup> _groups = new();

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-navmenu")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", Width, () => Expanded && !string.IsNullOrEmpty(Width))
        .AddStyle("width", WIDTH_COLLAPSED_MENU, () => !Expanded)
        .AddStyle(Style)
        .Build();

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    private IJSObjectReference Module { get; set; } = default!;

    /// <summary>
    /// Gets a reasonably unique ID (ex. 8c8113da982d75a)
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Identifier.NewId();

    [Parameter]
    public string? Title { get; set; } //= FluentNavigationResource.NavigationMenuTitle;

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public bool Collapsible { get; set; } = true;

    [Parameter]
    [CascadingParameter()]
    public bool Expanded { get; set; } = true;

    internal bool HasSubMenu => _groups.Any();

    internal bool HasIcons => _links.Any(i => !String.IsNullOrWhiteSpace(i.Icon));

    protected void Collapsible_Click(MouseEventArgs e)
    {
        if (Collapsible)
        {
            Expanded = !Expanded;
        }
    }

    internal int AddNavLink(FluentNavLink link)
    {
        _links.Add(link);
        StateHasChanged();
        return _links.Count;
    }

    internal int AddNavGroup(FluentNavGroup group)
    {
        _groups.Add(group);
        return _groups.Count;
    }

    internal async Task SelectOnlyThisLinkAsync(FluentNavLink? selectedLink)
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

        // Select the correct link (with property Selected = true)
        // Must use Javascript because the Click and Select events are
        // automatically managed by 'fluent-tree-item' component.
        if (Module == null)
        {
            Module = await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        }
        var selectedLinkId = _links?.FirstOrDefault(i => i.Selected)?.Id ?? "";
        await Module.InvokeVoidAsync("selectOnlyThisLink", this.Id, selectedLinkId);
    }
}

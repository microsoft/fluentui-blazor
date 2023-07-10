using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenu : FluentComponentBase, INavMenuParentElement, IDisposable
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private const string WIDTH_COLLAPSED_MENU = "40px";
    private string _prevHref = "/";
    private readonly string _expandCollapseTreeItemId = Identifier.NewId();
    private readonly Dictionary<string, INavMenuChildElement> _childElements = new();

    private bool HasChildIcons => ((INavMenuParentElement)this).HasChildIcons;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu")
        .AddClass("collapsed", !Expanded)
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
    /// Gets or sets whether the menu is expanded.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; } = true;

    /// <summary>
    /// Event callback for when the <see cref="Expanded"/> property changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// Called whenever a contained <see cref="FluentNavMenuGroup"/> is expanded or collapsed.
    /// </summary>
    [Parameter]
    public EventCallback<FluentNavMenuGroup> OnGroupExpandedChange { get; set; }

    /// <summary>
    /// Called whenever a contained <see cref="FluentNavMenuGroup"/> is selected or unselected.
    /// </summary>
    [Parameter]
    public EventCallback<FluentNavMenuGroup> OnGroupSelectedChange { get; set; }

    /// <summary>
    /// Called whever a contained <see cref="FluentNavMenuLink"/> is selected or unselected.
    /// </summary>
    [Parameter]
    public EventCallback<FluentNavMenuLink> OnLinkSelectedChange { get; set; }


    /// <summary>
    /// If set to <see langword="true"/> then the tree will
    /// expand when it is created.
    /// </summary>
    [Parameter]
    public bool InitiallyExpanded { get; set; }

    public bool Collapsed => !Expanded;

    public FluentNavMenu()
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Sets if the menu is expanded or not.
    /// </summary>
    /// <param name="expanded">Whether or not the menu should be expanded.</param>
    /// <param name="forceChangedEvent">Trigger a <see cref="ExpandedChanged"/> event even if the value hasn't changed.</param>
    /// <returns></returns>
    public async Task SetExpandedAsync(bool expanded, bool forceChangedEvent = false)
    {
        bool changesRequired = forceChangedEvent || expanded != Expanded;

        if (!changesRequired)
            return;

        Expanded = expanded;
        if (ExpandedChanged.HasDelegate)
        {
            await ExpandedChanged.InvokeAsync(expanded);
        }

        StateHasChanged();
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


    private Task ToggleCollapsedAsync() => SetExpandedAsync(!Expanded);

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        if (InitiallyExpanded)
        {
            Expanded = true;
            if (ExpandedChanged.HasDelegate)
            {
                await ExpandedChanged.InvokeAsync(true);
            }
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

    private async Task HandleTreeItemExpandedChangedAsync(FluentTreeItem treeItem)
    {
        if (treeItem.Expanded && !Expanded)
        {
            await SetExpandedAsync(expanded: true);
        }

        if (OnGroupExpandedChange.HasDelegate && _childElements.TryGetValue(treeItem.Id!, out INavMenuChildElement? child))
        {
            await OnGroupExpandedChange.InvokeAsync((FluentNavMenuGroup)child);
        }
    }

    private async Task HandleTreeItemSelectedChangedAsync(FluentTreeItem treeItem)
    {
        if (treeItem.Id == _expandCollapseTreeItemId)
        {
            return;
        }

        if (!_childElements.TryGetValue(treeItem.Id!, out INavMenuChildElement? child))
        {
            return;
        }

        if (child is FluentNavMenuGroup group)
        {
            await HandleGroupSelectedChangeAsync(group);
        }
        else if (child is FluentNavMenuLink link)
        {
            await HandleLinkSelectedChangeAsync(link);
        }
    }

    private async Task HandleLinkSelectedChangeAsync(FluentNavMenuLink link)
    {
        if (OnLinkSelectedChange.HasDelegate)
        {
            await OnLinkSelectedChange.InvokeAsync(link);
        }
        Navigate(link.Href);
    }

    private async Task HandleGroupSelectedChangeAsync(FluentNavMenuGroup group)
    {
        if (OnGroupExpandedChange.HasDelegate)
        {
            await OnGroupExpandedChange.InvokeAsync(group);
        }
        if (AutoExpandOnGroupExpanded && group.Selected)
        {
            await group.SetExpandedAsync(expanded: true);
            await SetExpandedAsync(expanded: true);
        }
    }

    private void Navigate(string? href)
    {
        if (!string.IsNullOrWhiteSpace(href) && href != _prevHref)
        {
            _prevHref = href;
            NavigationManager.NavigateTo(href);
        }
    }
}

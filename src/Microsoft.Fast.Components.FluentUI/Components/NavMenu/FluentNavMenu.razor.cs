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

    private readonly List<INavMenuChildElement> _childElements = new();

    private Task ToggleCollapsedAsync() => 
        Expanded
        ? CollapseAsync()
        : ExpandAsync();

    /// <summary>
    /// Ensures the <see cref="FluentNavMenu"/> is collapsed.
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

        StateHasChanged();
    }

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
            INavMenuChildElement? menuItem = ((INavMenuParentElement)this).FindElementById(treeItem.Id);
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
        _childElements.Clear();
    }

    void INavMenuParentElement.Register(INavMenuChildElement child)
    {
        _childElements.Add(child);
        StateHasChanged();
    }

    void INavMenuParentElement.Unregister(INavMenuChildElement child)
    {
        _childElements.Remove(child);
        StateHasChanged();
    }

    IEnumerable<INavMenuChildElement> INavMenuParentElement.GetChildElements() => _childElements;
}

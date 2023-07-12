using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNavMenuLink : FluentComponentBase, INavMenuChildElement, IDisposable
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the destination of the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the icon to display with the link
    /// Use a constant value from the <see cref="FluentIcon{Icon}" /> class 
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// Gets or sets whether the link is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the target of the link.
    /// </summary>
    [Parameter]
    public string? Target { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text of the link.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width of the link (in pixels).
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets if the item is selected.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Event callback for when <see cref="Selected"/> changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    [CascadingParameter]
    private FluentNavMenu NavMenu { get; set; } = default!;

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    [CascadingParameter]
    private INavMenuParentElement Owner { get; set; } = null!;

    [CascadingParameter(Name = "NavMenuItemSiblingHasIcon")]
    private bool SiblingHasIcon { get; set; }

    public bool HasIcon => Icon != null;

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-link")
        .AddClass("navmenu-element")
        .AddClass("navmenu-child-element")
        .AddClass("navmenu-current-location selected", () => Selected)
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    private FluentTreeItem _treeItem = null!;

    public FluentNavMenuLink()
    {
        Id = Identifier.NewId();
    }

    FluentTreeItem INavMenuChildElement.TreeItem => _treeItem;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Owner.Register(this);
        NavMenu.Register(this);
    }

    void IDisposable.Dispose()
    {
        Owner.Unregister(this);
        NavMenu.Unregister(this);
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

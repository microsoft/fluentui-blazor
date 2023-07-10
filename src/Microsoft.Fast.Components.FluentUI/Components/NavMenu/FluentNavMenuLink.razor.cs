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
    /// Gets or sets the content to be rendered for the icon.
    /// </summary>
    [Parameter]
    public RenderFragment? IconContent { get; set; }

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
    /// Gets or sets whether the link is selected.
    /// </summary>
    [Parameter]
    public bool Selected { get; set; } = false;

    /// <summary>
    /// Callback function for when the selected state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    /// <summary>
    /// Callback function for when the link is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

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

    [CascadingParameter]
    private FluentNavMenu NavMenu { get; set; } = default!;

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    [CascadingParameter]
    private INavMenuParentElement Owner { get; set; } = null!;

    [CascadingParameter(Name = "NavMenuItemSiblingHasIcon")]
    private bool SiblingHasIcon { get; set; }

    protected string? ClassValue => new CssBuilder(Class)
       .AddClass("navmenu-link")
       .AddClass("navmenu-child-element")
       .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    public bool HasIcon => Icon != null || IconContent is not null;

    public FluentNavMenuLink()
    {
        Id = Identifier.NewId();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Owner.Register(this);
        NavMenu.Register(this);

        if (!string.IsNullOrEmpty(Href) && (new Uri(NavigationManager.Uri).LocalPath) == Href)
            Selected = true;
    }


    /// <summary>
    /// Dispose of this navmenu link.
    /// </summary>
    void IDisposable.Dispose()
    {
        Owner.Unregister(this);
        NavMenu.Unregister(this);
    }
}

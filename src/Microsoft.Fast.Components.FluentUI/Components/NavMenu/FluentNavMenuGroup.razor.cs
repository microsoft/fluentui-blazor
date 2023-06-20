using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;


namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentNavMenuGroup : FluentComponentBase
{
    private bool _expanded = false;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

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
    /// Gets or sets the content to be rendered for the icon when
    /// the menu is expanded. If not set, then <see cref="ExpandedIcon"/> will
    /// take next priority, followed by <see cref="FluentNavMenuGroup.IconContent"/>
    /// and then <see cref="FluentNavMenuGroup.Icon"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ExpandedIconContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered for the icon when
    /// the menu is expanded. <see cref="FluentNavMenuGroup.ExpandedIconContent"/> will take
    /// precedence over this setting. If not set, then <see cref="IconContent"/> will
    /// take next priority, followed <see cref="FluentNavMenuGroup.Icon"/>
    /// and then <see cref="FluentNavMenuGroup.Icon"/>.
    /// </summary>
    [Parameter]
    public string ExpandedIcon { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content to be rendered for the icon when
    /// the menu is collapsed. If not set, then <see cref="Icon"/> will
    /// take next priority.
    /// </summary>
    [Parameter]
    public RenderFragment? IconContent { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered for the icon when
    /// the menu is collapsed. <see cref="FluentNavMenuGroup.IconContent"/> will take
    /// precedence over this setting. If not set, no icon will be displayed
    /// </summary>
    [Parameter]
    public string Icon { get; set; }

    /// <summary>
    /// Gets or sets whether the menu group is disabled
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }
    
   
    /// <summary>
    /// Gets or sets whether the menu group is expanded
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets whether the menu group is selected
    /// </summary>
    [Parameter]
    public bool Selected { get; set; }

    /// <summary>
    /// Gets or sets the text of the menu group
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width of the menu group
    /// </summary>
    [Parameter]
    public int? Width { get; set; }
    
    /// <summary>
    /// Callback function for when the menu group is expanded
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnExpandedChanged { get; set; }
 

    [CascadingParameter(Name = "NavMenu")]
    public FluentNavMenu NavMenu { get; set; } = default!;

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    public FluentNavMenuGroup()
    {
        Id = Identifier.NewId();
    }

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-group")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    internal bool HasIcon => HasCollapsedIcon || HasExpandedIcon;
    private bool HasCollapsedIcon => IconContent is not null || !string.IsNullOrWhiteSpace(Icon);
    private bool HasExpandedIcon => ExpandedIconContent is not null || !string.IsNullOrWhiteSpace(ExpandedIcon);

    protected override void OnParametersSet()
    {
        NavMenu.AddNavMenuGroup(this);

        if (_expanded != Expanded)
        {
            _expanded = Expanded;
            if (OnExpandedChanged.HasDelegate)
                OnExpandedChanged.InvokeAsync(_expanded);
        }
    }



    /// <summary />
    internal async Task ExpandMenu()
    {
        if (Disabled)
            return;

        // Expand the Menu Group if the user click on it
        if (!NavMenuExpanded)
        {
            Selected = true;
            await NavMenu.CollapsibleClickAsync();
        }
    }

    internal void HandleIconClick()
    {
        if (!Disabled)
            Selected = true;
    }
}

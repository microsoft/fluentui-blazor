using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;
public partial class NavMenuGroup : FluentComponentBase
{
    private bool _expanded = false;

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
    /// Icon displayed when this item is collasped.
    /// </summary>
    [Parameter]
    public string? IconCollapsed { get; set; } = FluentIcons.ChevronRight;

    /// <summary>
    /// Icon displayed when this item is expanded.
    /// </summary>
    [Parameter]
    public string? IconExpanded { get; set; } = FluentIcons.ChevronDown;

    /// <summary>
    /// Icon displayed only when the <see cref="NavMenu.Expanded"/> is false.
    /// </summary>
    [Parameter]
    public string IconNavMenuCollapsed { get; set; } = FluentIcons.MoreHorizontal;

    /// <summary>
    /// Gets or sets a unique identifier.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Identifier.NewId();

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

    [CascadingParameter(Name = "NavMenu")]
    public NavMenu NavMenu { get; set; } = default!;

    /// <summary>
    /// Callback function for when the menu group is clicked
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Callback function for when the menu group is expanded
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnExpandedChanged { get; set; }

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

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-group")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        NavMenu.AddNavGroup(this);
        base.OnInitialized();
    }

    protected override void OnParametersSet()
    {
        if (_expanded != Expanded)
        {
            _expanded = Expanded;
            if (OnExpandedChanged.HasDelegate)
                OnExpandedChanged.InvokeAsync(_expanded); //.SafeFireAndForget();
        }

        base.OnParametersSet();
    }

    internal void UpdateProperties(bool? expanded)
    {
        if (expanded != null)
        {
            Expanded = expanded.Value;
        }
    }

    /// <summary />
   

    protected async Task OnExpandHandlerAsync(MouseEventArgs e)
    {
        if (Disabled)
            return;

        // Expand the Menu Group if the user click on it
        if (!NavMenuExpanded)
            await NavMenu.CollapsibleClickAsync(new MouseEventArgs());

    }

    protected async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (Disabled)
            return;

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(e);

        if (!string.IsNullOrEmpty(Href))
            NavigationManager.NavigateTo(Href);
    }


    internal async Task ExpandMenu()
    {
        if (Disabled)
            return;

        // Expand the Menu Group if the user click on it
        if (!NavMenuExpanded)
            await NavMenu.CollapsibleClickAsync(new MouseEventArgs());
    }
}

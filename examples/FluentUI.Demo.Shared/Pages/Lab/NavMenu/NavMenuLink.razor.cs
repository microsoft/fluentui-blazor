using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;

public partial class NavMenuLink : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("navmenu-link", () => Owner.HasSubMenu && Owner.HasIcons)
        .AddClass("navmenu-link-nogroup", () => !Owner.HasSubMenu && Owner.HasIcons)
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [CascadingParameter]
    public NavMenu Owner { get; set; } = default!;

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
    /// Gets or sets the name of the icon to display with the link
    /// </summary>
    [Parameter]
    public string Icon { get; set; } = "";

    /// <summary>
    /// Gets or sets the width of the link (in pixels).
    /// </summary>
    [Parameter]
    public int? Width { get; set; }

    /// <summary>
    /// Gets or sets the text of the link.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = "";

    /// <summary>
    /// Gets or sets the destination of the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; } = "";

    /// <summary>
    /// Gets orsets the target of the link.
    /// </summary>
    [Parameter]
    public string? Target { get; set; } = "";

    [CascadingParameter(Name = "NavMenuCollapsed")]
    private bool NavMenuCollapsed { get; set; }

    /// <summary>
    /// Callback function for when the link is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void OnInitialized()
    {
        //Owner.Register(this);
        Owner.AddNavLink(this);
    }
    private bool HasIcon => !string.IsNullOrWhiteSpace(Icon);

    protected async Task OnClickHandler(MouseEventArgs e)
    {
        if (Disabled)
            return;

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(e);

        if (!string.IsNullOrEmpty(Href))
            NavigationManager.NavigateTo(Href);

        Owner.SelectOnlyThisLink(this);
    }

    protected async Task OnKeypressHandler(KeyboardEventArgs e)
    {
        if (e.Code == "Space" || e.Code == "Enter")
        {
            await OnClickHandler(new MouseEventArgs());
        }
    }

    internal void SetSelected(bool value)
    {
        Selected = value;
    }
}

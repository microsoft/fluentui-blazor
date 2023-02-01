using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

// Remember to replace the namespace below with your own project's namespace..
namespace FluentUI.Demo.Shared;

public partial class NavMenuLink : FluentComponentBase
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
    /// Gets or sets the name of the icon to display with the link
    /// </summary>
    [Parameter]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a reasonably unique ID.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Identifier.NewId();

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

    [CascadingParameter(Name = "NavMenu")]
    public NavMenu NavMenu { get; set; } = default!;

    /// <summary>
    /// Callback function for when the link is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets orsets the target of the link.
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

    protected string? ClassValue => new CssBuilder(Class)
       .AddClass("navmenu-link", () => NavMenu.HasSubMenu && NavMenu.HasIcons)
       .AddClass("navmenu-link-nogroup", () => !NavMenu.HasSubMenu && NavMenu.HasIcons)
       .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", $"{Width}px", () => Width.HasValue)
        .AddStyle(Style)
        .Build();

    private bool HasIcon => !string.IsNullOrWhiteSpace(Icon);

    //[CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    internal void SetSelected(bool value)
    {
        Selected = value;
    }

    protected async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (Disabled)
            return;

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(e);

        if (!string.IsNullOrEmpty(Href))
            NavMenu.NavigateTo(Href);
    }

    protected override void OnInitialized()
    {
        NavMenuExpanded = NavMenu.Expanded;
        NavMenu.AddNavMenuLink(this);
    }
}

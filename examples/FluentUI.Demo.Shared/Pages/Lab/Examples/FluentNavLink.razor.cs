using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

public partial class FluentNavLink : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-navmenu-link", () => NavMenu.HasSubMenu && NavMenu.HasIcons)
        .AddClass("fluent-navmenu-link-nogroup", () => !NavMenu.HasSubMenu && NavMenu.HasIcons)
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle(Style)
        .Build();

    [CascadingParameter]
    public FluentNavMenu NavMenu { get; set; } = default!;

    protected override void OnInitialized()
    {
        NavMenu.AddNavLink(this);
        base.OnInitialized();
    }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets a reasonably unique ID (ex. 8c8113da982d75a)
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Identifier.NewId();

    [Parameter]
    public bool Disabled { get; set; } = false;

    [Parameter]
    public bool Selected { get; set; } = false;

    [Parameter]
    public EventCallback<bool> SelectedChanged { get; set; }

    [Parameter]
    public string Icon { get; set; } = "";

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string Text { get; set; } = "";

    [Parameter]
    public string? Href { get; set; } = "";

    [Parameter]
    public string? Target { get; set; } = "";

    [CascadingParameter(Name = "NavMenuExpanded")]
    private bool NavMenuExpanded { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    private bool HasIcon => !string.IsNullOrWhiteSpace(Icon);

    protected async Task OnClickHandler(MouseEventArgs e)
    {
        if (Disabled)
            return;

        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync(e);

        if (!String.IsNullOrEmpty(Href))
            NavigationManager.NavigateTo(Href);

        await NavMenu.SelectOnlyThisLinkAsync(this);
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

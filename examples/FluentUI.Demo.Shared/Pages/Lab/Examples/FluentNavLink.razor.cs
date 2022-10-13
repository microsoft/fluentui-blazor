using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace FluentUI.Demo.Shared;

public partial class FluentNavLink : FluentComponentBase
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-navmenu-link", () => Owner.HasSubMenu && Owner.HasIcons)
        .AddClass("fluent-navmenu-link-nogroup", () => !Owner.HasSubMenu && Owner.HasIcons)
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle("width", Width, () => !string.IsNullOrEmpty(Width))
        .AddStyle(Style)
        .Build();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [CascadingParameter]
    public FluentNavMenu Owner { get; set; } = default!;

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

    [CascadingParameter(Name = "NavMenuCollapsed")]
    private bool NavMenuCollapsed { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

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

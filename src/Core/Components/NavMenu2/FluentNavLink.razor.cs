using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI.Utilities;


namespace Microsoft.Fast.Components.FluentUI;

#nullable enable
public partial class FluentNavLink 
{
    internal string? ClassValue => new CssBuilder("fluent-nav-item")
        .AddClass(Class)
        .Build();

    internal string? LinkClassname => new CssBuilder("fluent-nav-link")
        .AddClass($"disabled", Disabled)
        .Build();
   

    internal Dictionary<string, object?> Attributes
    {
        get => Disabled ? new Dictionary<string, object?>() : new Dictionary<string, object?>
        {
            { "href", Href },
            { "target", Target },
            { "rel", !string.IsNullOrWhiteSpace(Target) ? "noopener noreferrer" : string.Empty }
        };
    }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// URL for the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Icon to use if set.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

    /// <summary>
    /// The color of the icon. It supports the theme colors, default value uses the themes drawer icon color.
    /// </summary>
    [Parameter]
    public Color IconColor { get; set; } = Color.Accent;

    [Parameter]
    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

    /// <summary>
    /// The target attribute specifies where to open the link, if Href is specified. 
    /// Possible values: _blank | _self | _parent | _top.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

    /// <summary>
    /// Class names to use to indicate the item is active, separated by space.
    /// </summary>
    [Parameter]
    public string ActiveClass { get; set; } = "active";

    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// If true, force browser to redirect outside component router-space.
    /// </summary>
    [Parameter]
    public bool ForceLoad { get; set; }

    /// <summary>
    /// Gets or sets the content to display 
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    protected async Task OnClickHandler(MouseEventArgs ev)
    {
        if (Disabled)
            return;
        if (Href != null)
        {
            NavigationManager.NavigateTo(Href, ForceLoad);
        }
        else
        {
            await OnClick.InvokeAsync(ev);
        }
    }
}
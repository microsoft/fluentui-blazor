using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// Base class for <see cref="FluentNavGroup"/> and <see cref="FluentNavLink"/>.
/// </summary>
public abstract class FluentNavBase : FluentComponentBase
{
    /// <summary>
    /// URL for the group.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// The target attribute specifies where to open the group, if Href is specified. 
    /// Possible values: _blank | _self | _parent | _top.
    /// </summary>
    [Parameter]
    public string? Target { get; set; }

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

    /// <summary>
    /// If true, the button will be disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the content to be shown.
    /// </summary>  
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Class names to use to indicate the item is active, separated by space.
    /// </summary>
    [Parameter]
    public string ActiveClass { get; set; } = "active";

    /// <summary>
    /// Gets or sets how the link should be matched. Defaults to <see cref="NavLinkMatch.Prefix"/>.
    /// </summary>
    [Parameter]
    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

    [CascadingParameter(Name = "NavMenuExpanded")]
    public bool NavMenuExpanded { get; private set; }

    /// <summary>
    /// Returns <see langword="true"/> if the item has an <see cref="Icon"/> set.
    /// </summary>
    internal bool HasIcon => Icon is not null;

    /// <summary>
    /// The callback to invoke when the item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// If true, force browser to redirect outside component router-space.
    /// </summary>
    [Parameter]
    public bool ForceLoad { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected async Task OnClickHandler(MouseEventArgs ev)
    {
        if (Disabled)
        {
            return;
        }
        if (!string.IsNullOrEmpty(Href))
        {
            NavigationManager.NavigateTo(Href, ForceLoad);
        }
        else
        {
            await OnClick.InvokeAsync(ev);
        }
    }
}


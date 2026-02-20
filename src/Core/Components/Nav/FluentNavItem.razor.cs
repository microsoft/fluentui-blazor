// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a navigation menu item that renders content within a Fluent UI styled navigation link.
/// </summary>
public partial class FluentNavItem : FluentNavBase
{
    private const string RelNoOpenerNoReferrer = "noopener noreferrer";

    /// <summary />
    protected bool _isSubItem => Category != null;

    /// <summary />
    public FluentNavItem(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-navitem")
        .AddClass("fluent-navsubitem", _isSubItem)
        .AddClass("disabled", Disabled)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>
    /// Gets or sets the parent <see cref="FluentNavCategory"/> component for this instance.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentNav"/>. It enables the component to access shared state or functionality from
    /// its parent navigation menu.</remarks>
    [CascadingParameter(Name = "Category")]
    internal FluentNavCategory? Category { get; set; }

    /// <summary>
    /// Gets or sets the icon to use when the item is not hovered/selected/active.
    /// </summary>
    [Parameter]
    public Icon? IconRest { get; set; }

    /// <summary>
    /// Gets or sets the icon to use when the item is hovered/selected/active.
    /// </summary>
    [Parameter]
    public Icon? IconActive { get; set; }

    /// <summary>
    /// Gets or sets the href of the link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// The callback to invoke when the item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// If true, the item will be disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the target attribute that specifies where to open the group, if Href is specified.
    /// Possible values: _blank | _self | _parent | _top.
    /// </summary>
    [Parameter]
    public LinkTarget? Target { get; set; }

    /// <summary>
    /// Gets or sets the class names to use to indicate the item is active, separated by space.
    /// </summary>
    [Parameter]
    public string ActiveClass { get; set; } = "active";

    /// <summary>
    /// Gets or sets how the link should be matched.
    /// Defaults to <see cref="NavLinkMatch.Prefix"/>.
    /// </summary>
    [Parameter]
    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

    /// <summary>
    /// Gets or sets the tooltip to display when the mouse is placed over the item.
    /// </summary>
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary>
    /// Gets or sets the content of the nav menu item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the active state on this navigation item
    /// </summary>
    public bool Active => _isActive;

    /// <summary>
    /// Validates that this component is used within a FluentNav.
    /// </summary>
    protected override void OnParametersSet()
    {
        _hrefAbsolute = Href == null ? null : this.NavigationManager.ToAbsoluteUri(Href).AbsoluteUri;
        UpdateActiveState();
    }

    /// <summary />
    protected override void OnInitialized()
    {
        // Validate that this component is used within a FluentNav
        if (Owner?.GetType() != typeof(FluentNav))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavItem)} can only be used as a direct child of {nameof(FluentNav)}.");
        }

        if (Category != null && Category.GetType() != typeof(FluentNavCategory))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavItem)} can only be used as a direct child of {nameof(FluentNav)} or a {nameof(FluentNavCategory)}.");
        }

        Owner?.Register(this);

        // Register with parent category if this is a subitem
        if (_isSubItem)
        {
            Category?.RegisterSubitem(this);
        }
    }

    /// <summary>
    /// Updates the active state.
    /// </summary>
    internal override void UpdateActiveState(string? location = null)
    {
        // We could just re-render always, but for this component we know the
        // only relevant state change is to the _isActive property.
        var shouldBeActiveNow = ShouldMatch(location ?? this.NavigationManager.Uri, Match);
        if (shouldBeActiveNow != _isActive)
        {
            _isActive = shouldBeActiveNow;

            // Notify parent category if this is a subitem
            if (_isSubItem)
            {
                Category?.OnSubitemActiveStateChanged();
            }

            StateHasChanged();
        }
    }

    /// <summary />
    protected void RenderIcon(RenderTreeBuilder builder)
    {
        if (_isSubItem)
        {
            return;
        }

        if (Owner is not null && Owner.UseIcons)
        {
            if (IconRest is not null)
            {
                builder.OpenComponent<FluentIcon<Icon>>(0);
                builder.AddAttribute(1, "Value", _isActive ? IconActive ?? IconRest : IconRest);
                builder.AddAttribute(2, "Class", "icon");
                builder.AddAttribute(3, "Color", _isActive ? Color.Primary : Color.Default);
                builder.CloseComponent();
            }
            else
            {
                builder.OpenElement(4, "span");
                builder.AddAttribute(5, "style", "width: 20px;");
                builder.CloseElement();
            }
        }
    }

    /// <summary>
    /// Calls the <see cref="OnClick"/> delegate when specified (and item not disabled)
    /// </summary>
    protected async Task OnClickHandlerAsync(MouseEventArgs args)
    {
        if (Disabled)
        {
            return;
        }

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(args);
        }

        if (Owner is not null && Owner.OnItemClick.HasDelegate)
        {
            await Owner.OnItemClick.InvokeAsync(this);
        }

        // Hamburger menus should close when a nav item is clicked
        if (Owner?.OpenedHamburgers.Length > 0)
        {
            foreach (var hamburger in Owner.OpenedHamburgers)
            {
                await hamburger.HideAsync();
            }

            // Anchor links does not work inside a fluent-drawer.
            // We need to navigate using the NavigationManager to ensure the correct behavior.
            if (!string.IsNullOrEmpty(Href))
            {
                if (Target.HasValue)
                {
                    // Use JS to open link with target attribute
                    await JSRuntime.InvokeVoidAsync("window.open", Href, Target.ToAttributeValue(), RelNoOpenerNoReferrer);
                }
                else
                {
                    NavigationManager.NavigateTo(Href);
                }
            }
        }
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if (_isSubItem)
        {
            Category?.UnregisterSubitem(this);
        }

        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}

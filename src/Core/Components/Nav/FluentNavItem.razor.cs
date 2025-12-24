// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a navigation menu item that renders content within a Fluent UI styled navigation link.
/// </summary>
public partial class FluentNavItem : FluentComponentBase, IDisposable
{
    private const string EnableMatchAllForQueryStringAndFragmentSwitchKey = "Microsoft.AspNetCore.Components.Routing.NavLink.EnableMatchAllForQueryStringAndFragment";
    private string? _hrefAbsolute;

    private static readonly CaseInsensitiveCharComparer CaseInsensitiveComparer = new();
    private static readonly bool _enableMatchAllForQueryStringAndFragment = AppContext.TryGetSwitch(EnableMatchAllForQueryStringAndFragmentSwitchKey, out var switchValue) && switchValue;

    /// <summary />
    protected bool _isActive;

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

    /// <summary />
    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    /// <summary>
    /// Gets or sets the icon of the nav menu item.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; }

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
    /// Gets or sets the parent <see cref="FluentNav"/> component for this instance.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentNav"/>. It enables the component to access shared state or functionality from
    /// its parent navigation menu.</remarks>
    [CascadingParameter]
    public required FluentNav Owner { get; set; }

    /// <summary>
    /// Gets or sets the parent <see cref="FluentNavCategory"/> component for this instance.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentNav"/>. It enables the component to access shared state or functionality from
    /// its parent navigation menu.</remarks>
    [CascadingParameter(Name = "Category")]
    public FluentNavCategory? Category { get; set; }

    /// <summary>
    /// Gets the active state on this navigation item
    /// </summary>
    public bool Active => _isActive;

    /// <summary>
    /// Validates that this component is used within a FluentNav.
    /// </summary>
    protected override void OnParametersSet()
    {
        // Validate that this component is used within a FluentNav
        if (Owner.GetType() != typeof(FluentNav))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavItem)} can only be used as a direct child of {nameof(FluentNav)}.");
        }

        if (Category != null && Category.GetType() != typeof(FluentNavCategory))
        {
            throw new InvalidOperationException(
                $"{nameof(FluentNavItem)} can only be used as a direct child of {nameof(FluentNav)} or a {nameof(FluentNavCategory)}.");
        }

        _hrefAbsolute = Href == null ? null : NavigationManager.ToAbsoluteUri(Href).AbsoluteUri;
        _isActive = ShouldMatch(NavigationManager.Uri);
    }

    /// <summary />
    protected override void OnInitialized()
    {
        // We'll consider re-rendering on each location change
        NavigationManager.LocationChanged += OnLocationChanged;

        // Register with parent category if this is a subitem
        if (_isSubItem)
        {
            Category?.RegisterSubitem(this);
        }
    }

    /// <summary />
    protected void RenderIcon(RenderTreeBuilder builder)
    {
        if (_isSubItem)
        {
            return;
        }

        if (Owner.UseIcons)
        {
            if (Icon is not null)
            {
                builder.OpenComponent< FluentIcon<Icon>>(0);
                builder.AddAttribute(1, "Value", NavUtils.GetActiveIcon(Icon, _isActive));
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
}

/// <summary>
/// Handles location change events and updates the active state.
/// </summary>
protected virtual void OnLocationChanged(object? sender, LocationChangedEventArgs args)
{
    // We could just re-render always, but for this component we know the
    // only relevant state change is to the _isActive property.
    var shouldBeActiveNow = ShouldMatch(args.Location);
    if (shouldBeActiveNow != _isActive)
    {
        _isActive = shouldBeActiveNow;

        // Notify parent category if this is a subitem
        if (_isSubItem)
        {
            Category?.OnSubitemActiveStateChanged();
        }
    }
}

/// <summary>
/// Determines whether the current URI should match the link.
/// </summary>
/// <param name="uriAbsolute">The absolute URI of the current location.</param>
/// <returns>True if the link should be highlighted as active; otherwise, false.</returns>
[ExcludeFromCodeCoverage(Justification = "Copied from Blazor source")]
protected virtual bool ShouldMatch(string uriAbsolute)
{
    if (_hrefAbsolute == null)
    {
        return false;
    }

    var uriAbsoluteSpan = uriAbsolute.AsSpan();
    var hrefAbsoluteSpan = _hrefAbsolute.AsSpan();
    if (EqualsHrefExactlyOrIfTrailingSlashAdded(uriAbsoluteSpan, hrefAbsoluteSpan))
    {
        return true;
    }

    if (Match == NavLinkMatch.Prefix
        && IsStrictlyPrefixWithSeparator(uriAbsolute, _hrefAbsolute))
    {
        return true;
    }

    if (_enableMatchAllForQueryStringAndFragment || Match != NavLinkMatch.All)
    {
        return false;
    }

    var uriWithoutQueryAndFragment = GetUriIgnoreQueryAndFragment(uriAbsoluteSpan);
    if (EqualsHrefExactlyOrIfTrailingSlashAdded(uriWithoutQueryAndFragment, hrefAbsoluteSpan))
    {
        return true;
    }

    hrefAbsoluteSpan = GetUriIgnoreQueryAndFragment(hrefAbsoluteSpan);
    return EqualsHrefExactlyOrIfTrailingSlashAdded(uriWithoutQueryAndFragment, hrefAbsoluteSpan);
}

/// <inheritdoc />
public void Dispose()
{
    NavigationManager.LocationChanged -= OnLocationChanged;

    if (_isSubItem)
    {
        Category?.UnregisterSubitem(this);
    }

    GC.SuppressFinalize(this);
}

[ExcludeFromCodeCoverage(Justification = "Copied from Blazor source")]
private static ReadOnlySpan<char> GetUriIgnoreQueryAndFragment(ReadOnlySpan<char> uri)
{
    if (uri.IsEmpty)
    {
        return [];
    }

    var queryStartPos = uri.IndexOf('?');
    var fragmentStartPos = uri.IndexOf('#');

    if (queryStartPos < 0 && fragmentStartPos < 0)
    {
        return uri;
    }

    int minPos;
    if (queryStartPos < 0)
    {
        minPos = fragmentStartPos;
    }
    else if (fragmentStartPos < 0)
    {
        minPos = queryStartPos;
    }
    else
    {
        minPos = Math.Min(queryStartPos, fragmentStartPos);
    }

    return uri[..minPos];
}

[ExcludeFromCodeCoverage(Justification = "Copied from Blazor source")]
private static bool EqualsHrefExactlyOrIfTrailingSlashAdded(ReadOnlySpan<char> currentUriAbsolute, ReadOnlySpan<char> hrefAbsolute)
{
    if (currentUriAbsolute.SequenceEqual(hrefAbsolute, CaseInsensitiveComparer))
    {
        return true;
    }

    if (currentUriAbsolute.Length == hrefAbsolute.Length - 1)
    {
        // Special case: highlight links to http://host/path/ even if you're
        // at http://host/path (with no trailing slash)
        //
        // This is because the router accepts an absolute URI value of "same
        // as base URI but without trailing slash" as equivalent to "base URI",
        // which in turn is because it's common for servers to return the same page
        // for http://host/vdir as they do for host://host/vdir/ as it's no
        // good to display a blank page in that case.
        if (hrefAbsolute[^1] == '/' &&
            currentUriAbsolute.SequenceEqual(hrefAbsolute[..^1], CaseInsensitiveComparer))
        {
            return true;
        }
    }

    return false;
}

[ExcludeFromCodeCoverage(Justification = "Copied from Blazor source")]
private static bool IsUnreservedCharacter(char c)
{
    // Checks whether it is an unreserved character according to
    // https://datatracker.ietf.org/doc/html/rfc3986#section-2.3
    // Those are characters that are allowed in a URI but do not have a reserved
    // purpose (e.g. they do not separate the components of the URI)
    return char.IsLetterOrDigit(c) ||
            c == '-' ||
            c == '.' ||
            c == '_' ||
            c == '~';
}

[ExcludeFromCodeCoverage(Justification = "Copied from Blazor source")]
private static bool IsStrictlyPrefixWithSeparator(string value, string prefix)
{
    var prefixLength = prefix.Length;
    if (value.Length > prefixLength)
    {
        return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            && (
                // Only match when there's a separator character either at the end of the
                // prefix or right after it.
                // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
                // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
                prefixLength == 0
                || !IsUnreservedCharacter(prefix[prefixLength - 1])
                || !IsUnreservedCharacter(value[prefixLength])
            );
    }

    return false;
}

[ExcludeFromCodeCoverage(Justification = "Copied from Blazor source")]
private class CaseInsensitiveCharComparer : IEqualityComparer<char>
{
    public bool Equals(char x, char y)
    {
        return char.ToLowerInvariant(x) == char.ToLowerInvariant(y);
    }

    public int GetHashCode(char obj)
    {
        return char.ToLowerInvariant(obj).GetHashCode();
    }
}
}

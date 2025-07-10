// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentNavLink : FluentNavBase, IDisposable
{
    private const string EnableMatchAllForQueryStringAndFragmentSwitchKey = "Microsoft.AspNetCore.Components.Routing.NavLink.EnableMatchAllForQueryStringAndFragment";
    private static readonly bool _enableMatchAllForQueryStringAndFragment = AppContext.TryGetSwitch(EnableMatchAllForQueryStringAndFragmentSwitchKey, out var switchValue) && switchValue;

    private bool _isActive;
    private string? _hrefAbsolute;
    private string? _class;

    private readonly RenderFragment _renderContent;

    internal string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-nav-item")
        .Build();

    internal string? LinkClassValue => new CssBuilder("fluent-nav-link")
        .AddClass($"disabled", Disabled)
        .Build();

    internal Dictionary<string, object?> Attributes
    {
        get => Disabled ? [] : new Dictionary<string, object?>
        {
            { "href", Href },
            { "target", Target },
            { "rel", !string.IsNullOrWhiteSpace(Target) ? "noopener noreferrer" : string.Empty }
        };
    }

    public FluentNavLink()
    {
        _renderContent = RenderContent;
    }

    protected override void OnInitialized()
    {
        // We'll consider re-rendering on each location change
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs args)
    {
        // We could just re-render always, but for this component we know the
        // only relevant state change is to the _isActive property.
        var shouldBeActiveNow = ShouldMatch(args.Location);
        if (shouldBeActiveNow != _isActive)
        {
            _isActive = shouldBeActiveNow;
            UpdateCssClass();
            StateHasChanged();
        }
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        // Update computed state
        _hrefAbsolute = Href == null ? null : NavigationManager.ToAbsoluteUri(Href).AbsoluteUri;
        _isActive = ShouldMatch(NavigationManager.Uri);

        UpdateCssClass();
    }

    private void UpdateCssClass()
    {
        _class = _isActive ? CombineWithSpace(Class, ActiveClass) : Class;
    }

    /// <summary>
    /// Determines whether the current URI should match the link.
    /// </summary>
    /// <param name="uriAbsolute">The absolute URI of the current location.</param>
    /// <returns>True if the link should be highlighted as active; otherwise, false.</returns>
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

    private static ReadOnlySpan<char> GetUriIgnoreQueryAndFragment(ReadOnlySpan<char> uri)
    {
        if (uri.IsEmpty)
        {
            return ReadOnlySpan<char>.Empty;
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

        return uri.Slice(0, minPos);
    }


    private static readonly CaseInsensitiveCharComparer CaseInsensitiveComparer = new CaseInsensitiveCharComparer();

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
            if (hrefAbsolute[hrefAbsolute.Length - 1] == '/' &&
                currentUriAbsolute.SequenceEqual(hrefAbsolute.Slice(0, hrefAbsolute.Length - 1), CaseInsensitiveComparer))
            {
                return true;
            }
        }

        return false;
    }

    private static string? CombineWithSpace(string? str1, string str2)
        => str1 == null ? str2 : $"{str1} {str2}";

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
        else
        {
            return false;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // To avoid leaking memory, it's important to detach any event handlers in Dispose()
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    //private async Task HandleLinkKeyDownAsync(KeyboardEventArgs args)
    //{
    //    if (args is null || args.Code == "Tab")
    //    {
    //        return;
    //    }
    //    var handler = args.Code switch
    //    {
    //        "Enter" => OnClickHandlerAsync(new MouseEventArgs()),
    //        "Space" => OnClickHandlerAsync(new MouseEventArgs()),
    //        _ => Task.CompletedTask
    //    };
    //    await handler;
    //}

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

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides a base class for navigation items.
/// </summary>
public abstract class FluentNavBase : FluentComponentBase
{
    private const string EnableMatchAllForQueryStringAndFragmentSwitchKey = "Microsoft.AspNetCore.Components.Routing.NavLink.EnableMatchAllForQueryStringAndFragment";
    private static readonly CaseInsensitiveCharComparer CaseInsensitiveComparer = new();
    private static readonly bool _enableMatchAllForQueryStringAndFragment = AppContext.TryGetSwitch(EnableMatchAllForQueryStringAndFragmentSwitchKey, out var switchValue) && switchValue;

    /// <summary />
    protected string? _hrefAbsolute;

    /// <summary />
    protected bool _isActive;

    /// <summary />
    protected FluentNavBase(LibraryConfiguration configuration) : base(configuration)
    {
    }

    /// <summary />
    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Gets or sets the parent <see cref="FluentNav"/> component for this instance.
    /// </summary>
    /// <remarks>This property is typically set automatically by the Blazor framework when the component is
    /// used within a <see cref="FluentNav"/>. It enables the component to access shared state or functionality from
    /// its parent navigation menu.</remarks>
    [CascadingParameter]
    public required FluentNav Owner { get; set; }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        Owner.Unregister(this);

        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Updates the active state.
    /// </summary>
    internal abstract void UpdateActiveState(string? location = null);

    /// <summary>
    /// Determines whether the current URI should match the link.
    /// </summary>
    /// <param name="uriAbsolute">The absolute URI of the current location.</param>
    /// <param name="match">The <see cref="NavLinkMatch"/> value to use for comparison.</param>
    /// <returns>True if the link should be highlighted as active; otherwise, false.</returns>
    [ExcludeFromCodeCoverage(Justification = "Copied from Blazor source")]
    protected virtual bool ShouldMatch(string uriAbsolute, NavLinkMatch match)
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

        if (match == NavLinkMatch.Prefix
            && IsStrictlyPrefixWithSeparator(uriAbsolute, _hrefAbsolute))
        {
            return true;
        }

        if (_enableMatchAllForQueryStringAndFragment || match != NavLinkMatch.All)
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

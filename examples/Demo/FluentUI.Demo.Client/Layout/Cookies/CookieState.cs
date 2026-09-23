// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.Client.Layout.Cookies;

/// <summary>
/// Represents the state of cookie consent for a user, including their preferences for analytics, social media, and
/// advertising cookies.
/// </summary>
public class CookieState(bool? acceptAnalysis, bool? acceptSocialMedia, bool? acceptAdvertising)
{
    /// <summary>
    /// Gets or sets a value indicating whether the user has accepted analytics cookies.
    /// </summary>
    public bool? AcceptAnalytics { get; set; } = acceptAnalysis;

    /// <summary>
    /// Gets or sets a value indicating whether the user has accepted social media cookies.
    /// </summary>
    public bool? AcceptSocialMedia { get; set; } = acceptSocialMedia;

    /// <summary>
    /// Gets or sets a value indicating whether the user has accepted advertising cookies.
    /// </summary>
    public bool? AcceptAdvertising { get; set; } = acceptAdvertising;

    /// <summary />
    public CookieState() : this(null, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CookieState"/> class with the specified acceptance for all cookie types.
    /// </summary>
    /// <param name="acceptAll">A value indicating whether to accept all cookie types.</param>
    public CookieState(bool acceptAll) : this(acceptAll, acceptAll, acceptAll)
    {
    }
}

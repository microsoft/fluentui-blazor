// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicating which referrer to use when fetching the resource
/// </summary>
public enum LinkReferrerPolicy
{
    /// <summary>
    /// The Referer header will not be sent.
    /// </summary>
    [Description("no-referrer")]
    NoReferrer,

    /// <summary>
    /// No Referer header will be sent when navigating to an origin without TLS (HTTPS).
    /// </summary>
    [Description("no-referrer-when-downgrade")]
    NoReferrerWhenDowngrade,

    /// <summary>
    /// The referrer will be the origin of the page, which is roughly the scheme, the host, and the port.
    /// </summary>
    [Description("origin")]
    Origin,

    /// <summary>
    /// Navigation to other origins is limited to scheme, host, and port, while same-origin navigation includes the referrer’s path.
    /// </summary>
    [Description("origin-when-cross-origin")]
    OriginWhenCrossOrigin,

    /// <summary>
    /// The referrer will include the origin and the path (but not the fragment, password, or username)
    /// </summary>
    [Description("unsafe-url")]
    UnsafeUrl,
}

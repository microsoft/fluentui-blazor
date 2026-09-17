// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Display options for the hamburger icon.
/// </summary>
public enum HamburgerDisplay
{
    /// <summary>
    /// The hamburger icon is displayed on mobile devices only.
    /// </summary>
    [Description("")]
    MobileOnly,

    /// <summary>
    /// The hamburger icon is displayed on both desktop and mobile devices.
    /// </summary>
    [Description("mobile-desktop")]
    DesktopMobile,
}

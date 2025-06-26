// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of the <see cref="FluentTabs" />.
/// </summary>
public enum TabsAppearance
{
    /// <summary>
    /// No background and border styling. This is the default value.
    /// </summary>
    [Description("transparent")]
    Transparent,

    /// <summary>
    /// Minimizes emphasis to blend into the background until hovered or focused.
    /// </summary>
    [Description("subtle")]
    Subtle,
}

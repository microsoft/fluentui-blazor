// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of the <see cref="FluentLink" />.
/// </summary>
public enum LinkAppearance
{
    /// <summary>
    /// The link appears with the default style
    /// </summary>
    [Description("")]
    Default,

    /// <summary>
    /// Only the underlined style is retained on hovering 
    /// </summary>
    [Description("subtle")]
    Subtle,
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of a component.
/// </summary>
public enum Appearance
{
    /// <summary>
    /// Neutral appearance.
    /// </summary>
    [Obsolete("This value is obsolete. Use the ButtonAppearance.Default value instead.")]
    Neutral,

    /// <summary>
    /// Uses the current accent color.
    /// </summary>
    [Obsolete("This value is obsolete. Use the ButtonAppearance.Primary value instead.")]
    Accent,

    /// <summary>
    /// Shown as a link.
    /// </summary>
    [Obsolete("This value is obsolete.")]
    Hypertext,

    /// <summary>
    /// Show as a light button.
    /// </summary>
    [Obsolete("This value is obsolete. Use the ButtonAppearance.Transparent value instead.")]
    Lightweight,

    /// <summary>
    /// Show a border.
    /// </summary>
    [Obsolete("This value is obsolete. Use the ButtonAppearance.Outline value instead.")]
    Outline,

    /// <summary>
    /// Reveal on hover
    /// </summary>
    [Obsolete("This value is obsolete. Use the ButtonAppearance.Default value instead.")]
    Stealth,

    /// <summary>
    /// Show filled.
    /// </summary>
    [Obsolete("This value is obsolete. Use the ButtonAppearance.Default value instead.")]
    Filled,
}

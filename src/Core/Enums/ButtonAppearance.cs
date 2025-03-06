// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of the <see cref="FluentButton" />.
/// </summary>
public enum ButtonAppearance
{
    /// <summary>
    /// The button appears with the default style
    /// </summary>
    Default,

    /// <summary>
    /// Removes background styling.
    /// </summary>
    Outline,

    /// <summary>
    /// Emphasizes the button as a primary action.
    /// </summary>
    Primary,

    /// <summary>
    /// Minimizes emphasis to blend into the background until hovered or focused
    /// </summary>
    Subtle,

    /// <summary>
    /// Removes background and border styling.
    /// </summary>
    Transparent,
}

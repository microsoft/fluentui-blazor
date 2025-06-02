// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of the <see cref="FluentCard" />.
/// </summary>
public enum CardAppearance
{
    /// <summary>
    /// The card appears with the default style.
    /// </summary>
    [Description("default")]
    Default,

    /// <summary>
    /// The card is being displayed on a lighter gray or white surface.
    /// </summary>
    [Description("filled")]
    Filled,

    /// <summary>
    /// Use when you don't want a filled background color but a discernable outline (border) on the card.
    /// </summary>
    [Description("outline")]
    Outline,

    /// <summary>
    /// This variant doesn't have a background or border for the card container.
    /// </summary>
    [Description("subtle")]
    Subtle,
}

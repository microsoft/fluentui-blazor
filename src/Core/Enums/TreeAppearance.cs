// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Sets the visual appearance of the <see cref="FluentTree"/>
/// </summary>
public enum TreeAppearance
{
    /// <summary>
    /// Sublte appearance
    /// </summary>
    [Description("subtle")]
    Subtle,

    /// <summary>
    /// Subtle appearance with alpha
    /// </summary>
    [Description("subtle-alpha")]
    SubtleAlpha,

    /// <summary>
    /// Transparent appearance
    /// </summary>
    [Description("transparant")]
    Transparent,
}

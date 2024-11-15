// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Indicates the font family of the text shown in the <see cref="FluentText"/>.
/// </summary>
public enum TextFont
{
    /// <summary>
    /// Default font
    /// </summary>
    [Description("base")]
    Base,

    /// <summary>
    /// Numeric font
    /// </summary>
    [Description("numeric")]
    Numeric,

    /// <summary>
    /// Monospace font
    /// </summary>
    [Description("monospace")]
    Monospace,
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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

// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The visual appearance of the <see cref="FluentTextInput" />.
/// </summary>
public enum TextInputAppearance
{
    /// <summary>
    /// The default appearance.
    /// </summary>
    [Description("outline")]
    Outline,

    /// <summary>
    /// The appearance where a single line is drawn under the text.
    /// </summary>
    [Description("underline")]
    Underline,

    /// <summary>
    /// The appearance where the borders are filled with a lighter color.
    /// </summary>
    [Description("filled-lighter")]
    FilledLighter,

    /// <summary>
    /// The appearance where the borders are filled with a darker color.
    /// </summary>
    [Description("filled-darker")]
    FilledDarker,
}

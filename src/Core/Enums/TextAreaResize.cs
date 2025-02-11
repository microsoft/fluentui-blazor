// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Whether and how a user can resize the element. <see cref="TextAreaResize" />.
/// </summary>
public enum TextAreaResize
{
    /// <summary>
    /// The default appearance.
    /// </summary>
    [Description("none")]
    None,
    /// <summary>
    /// The appearance where the borders are filled with a lighter color.
    /// </summary>
    [Description("both")]
    Both,
    /// <summary>
    /// The appearance where the borders are filled with a darker color.
    /// </summary>
    [Description("horizontal")]
    Horizontal,
    /// <summary>
    /// The appearance where the borders are filled with a darker color.
    /// </summary>
    [Description("vertical")]
    Vertical,
}

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Describes the type of input of a <see cref="FluentTextInput"/>.
/// </summary>
public enum TextInputType
{
    /// <summary>
    /// The default type of text field
    /// </summary>
    [Description("text")]
    Text,

    /// <summary>
    /// A text field that is used for email address input.
    /// </summary>
    [Description("email")]
    Email,

    /// <summary>
    /// A text field that is used for password input.
    /// </summary>
    [Description("password")]
    Password,

    /// <summary>
    /// A text field that is used for telephone number input.
    /// </summary>
    [Description("tel")]
    Telephone,

    /// <summary>
    /// A text field that is used for URL input.
    /// </summary>
    [Description("url")]
    Url,

    /// <summary>
    /// A text field that is used for color input (hexadecimal color value).
    /// </summary>
    [Description("color")]
    Color,

    /// <summary>
    /// A text field that is used to search for a value. (accessibility)
    /// </summary>
    [Description("search")]
    Search,

    /// <summary>
    /// A text field that is used for number input.
    /// </summary>
    [Description("number")]
    Number,
}

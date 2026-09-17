// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Global attribute is an enumerated attribute that hints at the type of data that might be entered by the user while editing the element or its contents.
/// This allows a browser to display an appropriate virtual keyboard. 
/// </summary>
public enum TextInputMode
{
    /// <summary>
    /// No virtual keyboard. For when the page implements its own keyboard input control.
    /// </summary>
    [Description("none")]
    None,

    /// <summary>
    /// Standard input keyboard for the user's current locale. (default value)
    /// </summary>
    [Description("text")]
    Text,

    /// <summary>
    /// Fractional numeric input keyboard containing the digits and decimal separator for the user's locale (typically . or ,). Devices may or may not show a minus key (-).
    /// </summary>
    [Description("decimal")]
    Decimal,

    /// <summary>
    /// Numeric input keyboard, but only requires the digits 0–9. Devices may or may not show a minus key. 
    /// </summary>
    [Description("numeric")]
    Numeric,

    /// <summary>
    /// A telephone keypad input, including the digits 0–9, the asterisk (*), and the pound (#) key. 
    /// </summary>
    [Description("tel")]
    Telephone,

    /// <summary>
    /// A virtual keyboard optimized for search input. For instance, the return/submit key may be labeled "Search", along with possible other optimizations.
    /// </summary>
    [Description("search")]
    Search,

    /// <summary>
    /// A virtual keyboard optimized for entering email addresses. Typically includes the @character as well as other optimizations.
    /// </summary>
    [Description("email")]
    Email,

    /// <summary>
    /// A keypad optimized for entering URLs. This may have the / key more prominent, for example. Enhanced features could include history access and so on.
    /// </summary>
    [Description("url")]
    Url
}

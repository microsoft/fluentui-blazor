// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the interaction on the anchor element that triggers the popover to open or close.
/// This enumeration supports a bitwise combination of its member values.
/// </summary>
[Flags]
public enum PopoverTrigger
{
    /// <summary>
    /// The popover is not triggered automatically by the anchor element.
    /// </summary>
    [Description("")]
    None = 0,

    /// <summary>
    /// The popover toggles when the anchor element is clicked or touched.
    /// </summary>
    [Description("click")]
    Click = 1,

    /// <summary>
    /// The popover opens when the anchor element receives focus and closes when it loses focus.
    /// </summary>
    [Description("focus")]
    Focus = 2,

    /// <summary>
    /// The popover responds to both <see cref="Click"/> and <see cref="Focus"/> interactions.
    /// </summary>
    [Description("all")]
    All = Click | Focus,
}

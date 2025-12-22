// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Specifies the role of a <see cref="FluentMenuItem"/>.
/// </summary>

public enum MenuItemRole
{
    /// <summary>
    /// Default item
    /// </summary>
    [Description("menuitem")]
    MenuItem,

    /// <summary>
    /// Checkbox item
    /// </summary>
    [Description("menuitemcheckbox")]
    Checkbox,

    /// <summary>
    /// Large size.
    /// </summary>
    [Description("menuitemradio")]
    Radio,

    /// <summary>
    /// Checkbox item
    /// </summary>
    [Obsolete("This value is no longer used and will be removed in a future version. Use Checkbox value instead")]
    [Description("menuitemcheckboxobsolete")]
    MenuItemCheckbox = Checkbox,

    /// <summary>
    /// Checkbox item
    /// </summary>
    [Obsolete("This value is no longer used and will be removed in a future version. Use Radio value instead")]
    [Description("menuitemradioobsolete")]
    MenuItemRadio = Radio,
}

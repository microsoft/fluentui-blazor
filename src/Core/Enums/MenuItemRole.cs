// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
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
}

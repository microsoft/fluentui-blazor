// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines available theme modes.
/// </summary>
public enum ThemeMode
{
    /// <summary>
    /// Light theme mode.
    /// </summary>
    [Description("light")]
    Light,

    /// <summary>
    /// Dark theme mode.
    /// </summary>
    [Description("dark")]
    Dark,

    /// <summary>
    /// Follows the system theme mode.
    /// </summary>
    [Description("system")]
    System,
}

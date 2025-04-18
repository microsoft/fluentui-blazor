// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The size of the label in a <see cref="FluentField"/>.
/// </summary>
public enum FluentUITheme
{
    /// <summary>
    /// Light theme.
    /// </summary>
    [Description("light")]
    Light,

    /// <summary>
    /// Dark theme.
    /// </summary>
    [Description("dark")]
    Dark,

    /// <summary>
    /// System theme.
    /// </summary>
    [Description("system")]
    System,
}

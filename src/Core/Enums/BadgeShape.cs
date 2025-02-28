// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The shape of the <see cref="FluentBadge" />.
/// </summary>
public enum BadgeShape
{
    /// <summary>
    /// Badge uses a circular shape
    /// </summary>
    [Description("circular")]
    Circular,

    /// <summary>
    /// Badge uses a rounded shape
    /// </summary>
    [Description("rounded")]
    Rounded,

    /// <summary>
    /// Badge uses a square shape
    /// </summary>
    [Description("square")]
    Square,
}

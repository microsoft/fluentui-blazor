// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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

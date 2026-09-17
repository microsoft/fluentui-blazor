// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The size of the <see cref="FluentBadge" />.
/// </summary>
public enum BadgeSize
{
    /// <summary>
    /// Badge is displayed in medium size
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Badge is displayed in tiny size
    /// </summary>
    [Description("tiny")]
    Tiny,

    /// <summary>
    /// Badge is displayed in extra small size
    /// </summary>
    [Description("extra-small")]
    ExtraSmall,

    /// <summary>
    /// Badge is displayed in small size
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Badge is displayed in large size
    /// </summary>
    [Description("large")]
    Large,

    /// <summary>
    /// Badge is displayed in extra large size
    /// </summary>
    [Description("extra-large")]
    ExtraLarge,
}

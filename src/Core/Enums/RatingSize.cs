// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Sets the size of the Rating items.
/// </summary>
public enum RatingSize
{
    /// <summary>
    /// Medium size of the rating items.
    /// </summary>
    [Description("medium")]
    Medium,

    /// <summary>
    /// Small size of the rating items.
    /// </summary>
    [Description("small")]
    Small,

    /// <summary>
    /// Large size of the rating items.
    /// </summary>
    [Description("large")]
    Large,
}

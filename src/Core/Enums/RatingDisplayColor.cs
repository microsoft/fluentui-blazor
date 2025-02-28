// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.ComponentModel;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the color of the rating display items.
/// </summary>
public enum RatingDisplayColor
{
    /// <summary>
    /// Marigold value for rating display color
    /// </summary>
    [Description("marigold")]
    Marigold,

    /// <summary>
    /// Default value
    /// </summary>
    [Description("neutral")]
    Neutral,

    /// <summary>
    /// Brand value for rating display color
    /// </summary>
    [Description("brand")]
    Brand,
}
